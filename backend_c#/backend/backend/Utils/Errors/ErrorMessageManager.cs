using Microsoft.Extensions.Localization;

namespace backend.Utils.Errors
{
    public class ErrorMessageManager
    {

        private static IStringLocalizer<ErrorMessageManager> _localizer;

        public static void Initialize(IStringLocalizer<ErrorMessageManager> localizer)
        {
            _localizer = localizer;
        }

        public static string GetErrorMessage(string key)
        {
            if (_localizer == null) {
                throw new InvalidOperationException("Error message manager not initialized. Call Initialize method first.");
            }

            return _localizer[key];
        }
    }
}
