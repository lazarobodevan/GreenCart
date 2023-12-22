using System.Globalization;

namespace backend.Utils {
    public class DateUtils {

        public static DateTime ConvertStringToDateTime(string strDate, string strFormat) {
            
            if (DateTime.TryParseExact(strDate, strFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate)) {
                return DateTime.SpecifyKind(parsedDate.ToUniversalTime(), DateTimeKind.Utc);
            }
            throw new Exception("Formato inválido de data");
        }

    }
}
