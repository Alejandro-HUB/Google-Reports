using GoogleReportUnitTest.Models.Google;
using Newtonsoft.Json;

namespace GoogleReportUnitTest.Models
{
    public partial class MerchantPerformanceReport : GoogleResponse
    {
        [JsonProperty("results")]
        public List<Result> Results { get; set; }
    }

    public partial class Metrics
    {
        [JsonProperty("clicks")]
        public int Clicks { get; set; }

        [JsonProperty("impressions")]
        public int Impressions { get; set; }

        [JsonProperty("ctr")]
        public float Ctr { get; set; }
    }

    public partial class Segments
    {
        [JsonProperty("program")]
        public string Program { get; set; }

        [JsonProperty("offerId")]
        public string OfferId { get; set; }
    }
}
