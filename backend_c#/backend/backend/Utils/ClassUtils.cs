using System.Linq;

namespace backend.Utils {
    public static class ClassUtils {

        public static bool IsAllPropsNull<T>(this T obj) {
            return typeof(T).GetProperties().All(a => a.GetValue(obj) != null);
        }
    }
}
