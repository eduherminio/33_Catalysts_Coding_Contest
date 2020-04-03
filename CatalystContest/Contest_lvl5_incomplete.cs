using CatalystContest.Model;
using FileParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CatalystContest
{
    public class Contest_lvl5_incomplete
    {
        public class Lvl5Input
        {
            public double TransferRange { get; set; }
            public ICollection<string> FlightIds { get; set; }

            public Lvl5Input(double transferRange)
            {
                TransferRange = transferRange;
                FlightIds = new List<string>();
            }
        }

        public class Lvl5Output
        {
            public string FlightA { get; set; }
            public string FlightB { get; set; }
            public double Delay { get; set; }
            public double Timestamp { get; set; }

            public Lvl5Output(string flightA, string flightB, double delay, double timestamp)
            {
                FlightA = flightA;
                FlightB = flightB;
                Delay = delay;
                Timestamp = timestamp;
            }
        }

        public void Run(string level)
        {
            var input = ParseInput(level);
            using var sw = new StreamWriter($"Output/{level}.out");

            var flights = input.FlightIds.Select(ParseFlightInfo).ToList();

            var result = new List<Lvl5Output>();

            const double maxDelay = 3_600;
            const double minAltitude = 6_000;
            const double minDistance = 1_000;
            foreach (var flight in flights)
            {
                foreach (var anotherFlight in flights.Where(f => f.Start != flight.Start && f.End != flight.End))
                {
                    for (int t = (int)flight.Takeoff; t < flight.Takeoff + flight.FlightInfo.Last().TimestampOffset; ++t)
                    {
                        if (t > anotherFlight.Takeoff + anotherFlight.FlightInfo.Last().TimestampOffset
                            || t + 3600 < anotherFlight.Takeoff + anotherFlight.FlightInfo.First().TimestampOffset)
                        {
                            break;
                        }

                        for (int delay = 0; delay < maxDelay; ++delay)
                        {
                            var pos1 = CalculatePosition(t, flight, 0);
                            var pos2 = CalculatePosition(t, anotherFlight, delay);
                            if (pos1 == null || pos2 == null || pos1.Altitude < minAltitude || pos2.Altitude < minAltitude)
                            {
                                continue;
                            }

                            var p1 = Position.ToXYZ(pos1.Latitude, pos1.Longitude, pos1.Altitude);
                            var p2 = Position.ToXYZ(pos2.Latitude, pos2.Longitude, pos2.Altitude);

                            var distance = p1.DistanceTo(p2);

                            if (distance > minDistance && distance < input.TransferRange)
                            {
                                result.Add(new Lvl5Output(flight.FlightId, anotherFlight.FlightId, delay, t));
                            }
                        }
                    }
                }
            }

            var finalResult = result.FirstOrDefault();

            //...
        }

        private Input CalculatePosition(double timestamp, FlightInput flightInfo, double delay)
        {
            var realTimestamp = timestamp + delay;
            var info = flightInfo.FlightInfo.FirstOrDefault(f => flightInfo.Takeoff + +f.TimestampOffset == realTimestamp);
            if (info == null)
            {
                for (int t = 0; t < flightInfo.FlightInfo.Count; ++t)
                {
                    info = flightInfo.FlightInfo.ElementAt(t);
                    if (flightInfo.Takeoff + info.TimestampOffset > realTimestamp)
                    {
                        var previousInfo = flightInfo.FlightInfo.ElementAt(t - 1);

                        double prevTimestamp = flightInfo.Takeoff + previousInfo.TimestampOffset;
                        double nextTimestamp = flightInfo.Takeoff + info.TimestampOffset;
                        return new Input(
                             0,
                             InterpolateLinearly(realTimestamp, prevTimestamp, nextTimestamp, previousInfo.Lat, info.Lat),
                             InterpolateLinearly(realTimestamp, prevTimestamp, nextTimestamp, previousInfo.Lon, info.Lon),
                             InterpolateLinearly(realTimestamp, prevTimestamp, nextTimestamp, previousInfo.Altitude, info.Altitude));
                    }
                }

                return null;
            }
            else
            {
                return new Input(
                    0,
                    info.Lat,
                    info.Lon,
                    info.Altitude);
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

        private Lvl5Input ParseInput(string level)
        {
            var file = new ParsedFile($"Inputs/{level}");
            var result = new Lvl5Input(double.Parse(file.NextLine().ToSingleString()));

            var numberOfLines = file.NextLine().NextElement<int>();
            for (int i = 0; i < numberOfLines; ++i)
            {
                result.FlightIds.Add(file.NextLine().ToSingleString());
            }
            if (!file.Empty)
            {
                throw new Exception("Error parsing file");
            }

            return result;
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
