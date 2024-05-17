using Newtonsoft.Json;

namespace GoogleReportUnitTest.Models
{
    public partial class Result
    {
        [JsonProperty("segments")]
        public Segments Segments { get; set; }

        [JsonProperty("metrics")]
        public Metrics Metrics { get; set; }
    }

    public partial class Metrics
    {
    }

    public partial class Segments
    {

        [JsonProperty("date")]
        public Date Date { get; set; }
    }

    public partial class Date
    {
        [JsonProperty("year")]
        public string Year { get; set; }

        [JsonProperty("month")]
        public string Month { get; set; }

        [JsonProperty("day")]
        public string Day { get; set; }
    }

    public partial class ProductView
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("brand")]
        public string Brand { get; set; }

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonProperty("priceMicros")]
        public long PriceMicros { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
