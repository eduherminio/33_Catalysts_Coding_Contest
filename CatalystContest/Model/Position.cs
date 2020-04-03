using System;
using System.Collections.Generic;
using System.Text;

namespace CatalystContest.Model
{
    public class Position
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public double DistanceTo(Position otherPoint)
        {
            return Math.Sqrt(
                Math.Pow(otherPoint.X - X, 2)
                + Math.Pow(otherPoint.Y - Y, 2)
                + Math.Pow(otherPoint.Z - Z, 2));
        }

        public static Position ToXYZ(double latitude, double longitude, double altitude)
        {
            const int R = 6371000;

            var cosLat = Math.Cos(latitude * Math.PI / 180);
            var sinLat = Math.Sin(latitude * Math.PI / 180);

            var cosLong = Math.Cos(longitude * Math.PI / 180);
            var sinLong = Math.Sin(longitude * Math.PI / 180);

            return new Position
            {
                X = (R + altitude) * cosLat * cosLong,
                Y = (R + altitude) * cosLat * sinLong,
                Z = (R + altitude) * sinLat
            };
        }
    }
}
