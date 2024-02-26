namespace backend.Shared.Classes {
    public class Location {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int RadiusInKm { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Address { get; set; }
        public string? ZipCode { get; set; }

        public Location(Models.Location location) {
            City = location.City;
            State = location.State;
            ZipCode = location.ZipCode;
            Address = location.Address;
        }

        public Location() { }
    }
}
