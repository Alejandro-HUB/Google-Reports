namespace GoogleReportUnitTest.Helpers
{
    public static class Conversion
    {
        // Function to calculate the rate of change
        public static string CalculateRateOfChange(List<float> values)
        {
            double sum = 0;
            for (int i = 1; i < values.Count; i++)
            {
                sum += (values[i] - values[i - 1]) / values[i - 1];
            }
            var rateOfChange = sum / (values.Count - 1);
            return (rateOfChange * 100).ToString("0.##") + "%";
        }

        // Helper method to calculate price gap
        public static string CalculatePriceGap(float yourPrice, float priceOnGoogle)
        {
            // Calculate price gap percentage
            return (((priceOnGoogle - yourPrice) / priceOnGoogle) * 100).ToString("0.##") + "%";
        }

        // 1 millionth of a standard unit, 1 USD = 1000000 micros
        public static float ConvertMicrosToUSD(float input)
        {
            return input / 1000000; // 1 USD = 1,000,000 micros
        }

        // Helper method to calculate uplift
        public static string CalculateUplift(float yourPrice, double changeFraction)
        {
            // Calculate uplift percentage
            return ((yourPrice * changeFraction) * 100).ToString("0.##") + "%";
        }
    }
}
