using CatalystContest.Model;
using FileParser;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace CatalystContest
{
    public class Contest_lvl1
    {
        public void Run(string level)
        {
            var input = ParseInput(level);

            using var sw = new StreamWriter($"Output/{level}.out");

            sw.WriteLine($"{input.Min(i => i.Timestamp)} { input.Max(i => i.Timestamp)}");
            sw.WriteLine($"{input.Min(i => i.Latitude)} { input.Max(i => i.Latitude)}");
            sw.WriteLine($"{input.Min(i => i.Longitude)} { input.Max(i => i.Longitude)}");
            sw.WriteLine($"{input.Max(i => i.Altitude).ToString("##.0", CultureInfo.InvariantCulture)}");
        }

        private ICollection<Input> ParseInput(string level) => ParseInputPrivate(level).ToList();

        private IEnumerable<Input> ParseInputPrivate(string level)
        {
            var file = new ParsedFile($"Inputs/{level}");
            var numberOfLines = file.NextLine().NextElement<int>();
            for (int i = 0; i < numberOfLines; ++i)
            {
                var line = file.NextLine().ToSingleString();
                var elements = line.Split(",");

                if (elements.Length != 4)
                {
                    throw new Exception("Error parsing file");
                }

                yield return new Input(
                        double.Parse(elements[0]),
                        double.Parse(elements[1]),
                        double.Parse(elements[2]),
                        double.Parse(elements[3]));
            }
            if (!file.Empty)
            {
                throw new Exception("Error parsing file");
            }
        }
    }
}
