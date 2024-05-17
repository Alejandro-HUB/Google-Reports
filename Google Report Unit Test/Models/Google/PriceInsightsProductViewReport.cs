using GoogleReportUnitTest.Models.Google;
using Newtonsoft.Json;

namespace GoogleReportUnitTest.Models
{
    public partial class PriceInsightsProductViewReport : GoogleResponse
    {
        [JsonProperty("results")]
        public List<Result> Results { get; set; }
    }

    public partial class Result
    {
        [JsonProperty("priceInsights")]
        public PriceInsights PriceInsights { get; set; }
    }

    public partial class PriceInsights
    {
        [JsonProperty("suggestedPriceMicros")]
        public long SuggestedPriceMicros { get; set; }
        [JsonProperty("suggestedPriceCurrencyCode")]
        public string SuggestedPriceCurrencyCode { get; set; }
        [JsonProperty("predictedImpressionsChangeFraction")]
        public double PredictedImpressionsChangeFraction { get; set; }
        [JsonProperty("predictedClicksChangeFraction")]
        public double PredictedClicksChangeFraction { get; set; }
        [JsonProperty("predictedConversionsChangeFraction")]
        public double PredictedConversionsChangeFraction { get; set; }
    }
}
