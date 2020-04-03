using CatalystContest.Model;
using FileParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CatalystContest
{
    public class Contest_lvl3
    {
        public void Run(string level)
        {
            var coordinates = ParseInput(level)
                .Select(i => Position.ToXYZ(i.Latitude, i.Longitude, i.Altitude));

            using var sw = new StreamWriter($"Output/{level}.out");

            foreach (var coord in coordinates)
            {
                sw.WriteLine($"{coord.X} {coord.Y} {coord.Z}");
            }
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

                if (elements.Length != 3)
                {
                    throw new Exception("Error parsing file");
                }

                yield return new Input(
                        0,
                        double.Parse(elements[0]),
                        double.Parse(elements[1]),
                        double.Parse(elements[2]));
            }
            if (!file.Empty)
            {
                throw new Exception("Error parsing file");
            }
        }
    }
}
