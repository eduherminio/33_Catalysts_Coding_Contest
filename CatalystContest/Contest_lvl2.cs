using CatalystContest.Model;
using FileParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CatalystContest
{
    public class Contest_lvl2
    {
        public class Trip : IEquatable<Trip>
        {
            public string Start { get; set; }
            public string Destination { get; set; }
            public double Takeoff { get; set; }

            public Trip(Input input)
            {
                Start = input.Start;
                Destination = input.Destination;
                Takeoff = input.Takeoff;
            }

            #region Equals override
            public override bool Equals(object obj)
            {
                return Equals(obj as Trip);
            }

            public bool Equals(Trip other)
            {
                return Takeoff == other.Takeoff;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Start, Destination, Takeoff);
            }

            public static bool operator ==(Trip left, Trip right)
            {
                return EqualityComparer<Trip>.Default.Equals(left, right);
            }

            public static bool operator !=(Trip left, Trip right)
            {
                return !(left == right);
            }

            #endregion
        }

        public void Run(string level)
        {
            var input = ParseInput(level);

            var groups = input.GroupBy(i => new { i.Start, i.Destination })
                .OrderBy(g => g.First().Start)
                .ThenBy(g => g.First().Destination)
                .Select(g => g.ToHashSet());

            using var sw = new StreamWriter($"Output/{level}.out");

            foreach (var group in groups)
            {
                sw.WriteLine($"{group.First().Start} {group.First().Destination} {group.Count}");
            }
        }

        private ICollection<Trip> ParseInput(string level)
        {
            return ParseInputPrivate(level).Select(i => new Trip(i)).ToList();
        }

        private IEnumerable<Input> ParseInputPrivate(string level)
        {
            var file = new ParsedFile($"Inputs/{level}");
            var numberOfLines = file.NextLine().NextElement<int>();
            for (int i = 0; i < numberOfLines; ++i)
            {
                var line = file.NextLine().ToSingleString();
                var elements = line.Split(",");

                if (elements.Length != 7)
                {
                    throw new Exception("Error parsing file");
                }

                yield return new Input(
                        double.Parse(elements[0]),
                        double.Parse(elements[1]),
                        double.Parse(elements[2]),
                        double.Parse(elements[3]),
                        elements[4],
                        elements[5],
                        double.Parse(elements[6]));
            }
            if (!file.Empty)
            {
                throw new Exception("Error parsing file");
            }
        }
    }
}
