using CatalystContest.Model;
using FileParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CatalystContest
{
    public class Contest_lvl4
    {
        public class Lvl4Input
        {
            public string FlightId { get; set; }
            public double Timestamp { get; set; }

            public Lvl4Input(string flightId, double timestamp)
            {
                FlightId = flightId;
                Timestamp = timestamp;
            }
        }

        public void Run(string level)
        {
            var input = ParseInput(level);

            var result = CalculatePositions(input).ToList();

            using var sw = new StreamWriter($"Output/{level}.out");

            foreach (var item in result)
            {
                sw.WriteLine($"{item.Latitude} {item.Longitude} {item.Altitude}");
            }
        }

        private IEnumerable<Input> CalculatePositions(IEnumerable<Lvl4Input> items)
        {
            foreach (var item in items)
            {
                var flightInfo = ParseFlightInfo(item.FlightId);

                Input input = null;
                for (int i = 0; i < flightInfo.FlightInfo.Count; ++i)
                {
                    var info = flightInfo.FlightInfo.ElementAt(i);
                    if (flightInfo.Takeoff + info.TimestampOffset > item.Timestamp)
                    {
                        var previousInfo = flightInfo.FlightInfo.ElementAt(i - 1);

                        double prevTimestamp = flightInfo.Takeoff + previousInfo.TimestampOffset;
                        double nextTimestamp = flightInfo.Takeoff + info.TimestampOffset;
                        input = new Input(
                            0,
                            InterpolateLinearly(item.Timestamp, prevTimestamp, nextTimestamp, previousInfo.Lat, info.Lat),
                            InterpolateLinearly(item.Timestamp, prevTimestamp, nextTimestamp, previousInfo.Lon, info.Lon),
                            InterpolateLinearly(item.Timestamp, prevTimestamp, nextTimestamp, previousInfo.Altitude, info.Altitude));
                        break;
                    }
                    else if (flightInfo.Takeoff + info.TimestampOffset == item.Timestamp)
                    {
                        input = new Input(
                            0,
                            info.Lat,
                            info.Lon,
                            info.Altitude);
                        break;
                    }
                }
                yield return input ?? throw new Exception("Error finding results to interpolate");
            }
        }

        private static double InterpolateLinearly(double x, double x0, double x1, double y0, double y1)
        {
            if ((x1 - x0) == 0)
            {
                return (y0 + y1) / 2;
            }
            return y0 + (x - x0) * (y1 - y0) / (x1 - x0);
        }

        private ICollection<Lvl4Input> ParseInput(string level) => ParseInputPrivate(level).ToList();

        private IEnumerable<Lvl4Input> ParseInputPrivate(string level)
        {
            var file = new ParsedFile($"Inputs/{level}");
            var numberOfLines = file.NextLine().NextElement<int>();
            for (int i = 0; i < numberOfLines; ++i)
            {
                var line = file.NextLine();

                yield return new Lvl4Input(
                        line.NextElement<string>(),
                        line.NextElement<double>());
            }
            if (!file.Empty)
            {
                throw new Exception("Error parsing file");
            }
        }

        private FlightInput ParseFlightInfo(string flightId)
        {
            var file = new ParsedFile($"Inputs/usedFlights/{flightId}.csv");
            var result = new FlightInput()
            {
                Start = file.NextLine().ToSingleString(),
                End = file.NextLine().ToSingleString(),
                Takeoff = double.Parse(file.NextLine().ToSingleString())
            };

            var numberOfLines = file.NextLine().NextElement<int>();
            for (int i = 0; i < numberOfLines; ++i)
            {
                var line = file.NextLine().ToSingleString();
                var elements = line.Split(",");

                if (elements.Length != 4)
                {
                    throw new Exception("Error parsing file");
                }

                result.FlightInfo.Add(new FlightInfo(
                        double.Parse(elements[0]),
                        double.Parse(elements[1]),
                        double.Parse(elements[2]),
                        double.Parse(elements[3])));
            }

            result.FlightInfo = result.FlightInfo.OrderBy(f => f.TimestampOffset).ToList();

            if (!file.Empty)
            {
                throw new Exception("Error parsing file");
            }

            return result;
        }
    }
}
