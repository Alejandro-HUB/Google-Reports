using GoogleReportUnitTest.Models.Google;
using Newtonsoft.Json;

namespace GoogleReportUnitTest.Models
{
    public partial class PriceCompetitivenessProductViewReport : GoogleResponse
    {
        [JsonProperty("results")]
        public List<Result> Results { get; set; }
    }

    public partial class Result
    {
        [JsonProperty("productView")]
        public ProductView ProductView { get; set; }

        [JsonProperty("priceCompetitiveness")]
        public PriceCompetitiveness PriceCompetitiveness { get; set; }
    }

    public partial class PriceCompetitiveness
    {
        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("benchmarkPriceCurrencyCode")]
        public string BenchmarkPriceCurrencyCode { get; set; }

        [JsonProperty("benchmarkPriceMicros")]
        public long BenchmarkPriceMicros { get; set; }
    }
}
