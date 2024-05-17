using GoogleReportUnitTest.Models.Google;

namespace GoogleReportUnitTest.Models.GDF
{
    public class ManagedServiceReport
    {
        public GoogleMetricsReport FreeListingsAndShoppingAds { get; set; }
        public GoogleMetricsReport FreeListings { get; set; }
        public GoogleMetricsReport ShoppingAds { get; set; }
        public List<GooglePriceGapOpportunitiesReport> PriceGapOpportunities { get; set; }
        public List<GooglePriceSuggestionOpportunitiesReport> PriceSuggestionOpportunities { get; set; }
        public GoogleResponse ShoppingExperienceScoreCard { get; set; }
        public GoogleMetricsReport Promotions { get; set; }
    }
}
