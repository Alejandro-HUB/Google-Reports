namespace GoogleReportUnitTest.Models.GDF
{
    public class GooglePriceGapOpportunitiesReport
    {
        public string Title { get; set; }
        public float YourPrice { get; set; }
        public float PriceOnGoogle { get; set; }
        public string PriceGap { get; set; }
        public int Clicks { get; set; }
    }
}
