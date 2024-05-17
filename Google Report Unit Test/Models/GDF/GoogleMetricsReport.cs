namespace GoogleReportUnitTest.Models.GDF
{
    public class GoogleMetricsReport
    {
        public int TotalClicks { get; set; }
        public string ClicksRateOfChange { get; set; }
        public int TotalImpressions { get; set; }
        public string ImpressionsRateOfChange { get; set; }
        public float CTR { get; set; }
        public string CTRRateOfChange { get; set; }
        public int? FreeListingsConversions { get; set; }
    }
}
