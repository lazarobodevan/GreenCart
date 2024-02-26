using System.Threading.Tasks;

namespace backend.Shared.Services.Location {
    public interface ILocationService {

        Models.Location GetLocationByLatLon(double lat, double lon);
    }
}
