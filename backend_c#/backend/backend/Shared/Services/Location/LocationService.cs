using GoogleMaps.LocationServices;
using System;
using System.Threading.Tasks;

namespace backend.Shared.Services.Location {
    public class LocationService : ILocationService {

        private readonly GoogleLocationService GeolocationSys;

        public LocationService(GoogleLocationService geolocationSys) {
            GeolocationSys = geolocationSys;
        }

        public Models.Location GetLocationByLatLon(double lat, double lon) {
            
            try {
                var mapsLocation = this.GeolocationSys.GetAddressFromLatLang(lat, lon);
                var location = new Models.Location() {
                    Address = mapsLocation.Address,
                    City = mapsLocation.City,
                    State = mapsLocation.State,
                    ZipCode = mapsLocation.Zip
                };

                return location;

            }catch(Exception e) {
                throw new Exception($"Erro ao buscar no maps. Erro:{e.GetType()} ; Message: {e.Message}");
            }
        }
    }
}
