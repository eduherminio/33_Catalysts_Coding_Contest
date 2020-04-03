using System.Collections.Generic;

namespace CatalystContest.Model
{
    public class FlightInput
    {
        public string FlightId { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public double Takeoff { get; set; }

        public ICollection<FlightInfo> FlightInfo { get; set; }

        public FlightInput()
        {
            FlightInfo = new List<FlightInfo>();
        }
    }

    public class FlightInfo
    {
        public double TimestampOffset { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public double Altitude { get; set; }

        public FlightInfo(double timestampOffset, double lat, double lon, double altitude)
        {
            TimestampOffset = timestampOffset;
            Lat = lat;
            Lon = lon;
            Altitude = altitude;
        }
    }
}
