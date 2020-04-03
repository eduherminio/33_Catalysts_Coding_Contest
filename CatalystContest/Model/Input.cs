namespace CatalystContest.Model
{
    public class Input
    {
        public double Timestamp { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }

        public double Takeoff { get; set; }
        public string Start { get; set; }
        public string Destination { get; set; }

        public Input(double timestamp, double latitude, double longitude, double altitude)
        {
            Altitude = altitude;
            Longitude = longitude;
            Timestamp = timestamp;
            Latitude = latitude;
        }

        public Input(double timestamp, double latitude, double longitude, double altitude, string start, string destination, double takeoff)
            : this(timestamp, latitude, longitude, altitude)
        {
            Start = start;
            Destination = destination;
            Takeoff = takeoff;
        }
    }
}
