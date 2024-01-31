using System.Globalization;
using System.Text;

namespace backend.Utils {
    public class StringUtils {
        public string NormalizeString(string text) {
            string normalizedString = text.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString) {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c);
            }

            return stringBuilder.ToString().ToUpperInvariant();
        }
    }
}
