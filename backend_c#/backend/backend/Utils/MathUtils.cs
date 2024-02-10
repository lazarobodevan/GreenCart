namespace backend.Utils {
    public class MathUtils {

        public static double CalcAverageBasedOnPreviousAverage(double prevAvg, int numberOfPrevElem, int sumNewElem, int numberOfNewElem) {
            return (double)(prevAvg * (double)numberOfPrevElem + sumNewElem) / (double)(numberOfPrevElem + numberOfNewElem);
        }
    }
}
