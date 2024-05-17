using GoogleReportUnitTest.Enum;
using GoogleReportUnitTest.Helpers;
using GoogleReportUnitTest.Models;
using GoogleReportUnitTest.Models.GDF;
using GoogleReportUnitTest.Models.Google;
using GoogleReportUnitTest.Services;
using System.Text;

namespace GoogleReportUnitTest
{
    public class Program
    {
        private static string _merchantId { get; set; }
        private static string _refreshToken { get; set; }
        private static GoogleShoppingAPIService _googleShoppingAPIService { get; set; }
        private const string FREE_LISTINGS_AND_SHOPPING_ADS_DATE_RANGE = "'2023-06-30' AND '2024-01-01'";
        private const string ORGANIC_ADS_DATE_RANGE = "'2023-06-30' AND '2024-01-01'";
        private const string SHOPPING_ADS_DATE_RANGE = "'2023-06-30' AND '2024-01-01'";

        public static void ProgramConstructor() 
        {
            _merchantId = "";
            _refreshToken = "";
            _googleShoppingAPIService = new GoogleShoppingAPIService(_merchantId, _refreshToken);
        }

        public static void Main(string[] args)
        {
            ProgramConstructor();

            // Build custom report for customer
            var freeListingsAndShoppingAdsReport = new MerchantPerformanceReport();
            var organicAdsReport = new MerchantPerformanceReport();
            var shoppingAdsReport = new MerchantPerformanceReport();
            var priceGapReport = new PriceCompetitivenessProductViewReport();
            var priceSuggestionsReport = new PriceInsightsProductViewReport();
            var shoppingExperienceScoreCard = new GoogleResponse();
            var promotions = new GooglePromotions();

            // 1) Get the data
            Step1GetTheData(ref freeListingsAndShoppingAdsReport, ref organicAdsReport, ref shoppingAdsReport, ref priceGapReport, ref priceSuggestionsReport,
                ref shoppingExperienceScoreCard, ref promotions);

            // 2) Parse the data
            var managedServiceReport = Step2ParseTheData(freeListingsAndShoppingAdsReport, organicAdsReport, shoppingAdsReport, priceGapReport, priceSuggestionsReport,
                shoppingExperienceScoreCard, promotions);

            // 3) Write report with data
            Step3WriteReportWithData(managedServiceReport);
        }

        public static void Step1GetTheData(ref MerchantPerformanceReport freeListingsAndShoppingAdsReport, ref MerchantPerformanceReport organicAdsReport,
            ref MerchantPerformanceReport shoppingAdsReport, ref PriceCompetitivenessProductViewReport priceGapReport, 
            ref PriceInsightsProductViewReport priceSuggestionsReport, ref GoogleResponse shoppingExperienceScoreCard, ref GooglePromotions promotions)
        {
            // 1) Get the data

            // Clicks and Impressions - Free Listings and Shopping Ads
            freeListingsAndShoppingAdsReport = _googleShoppingAPIService.GetReport<MerchantPerformanceReport>(googleReportType: GoogleReportTypes.MerchantPerformanceView,
                fields: new List<string> { "segments.program", "segments.date", "segments.offer_id", "metrics.impressions", "metrics.clicks", "metrics.ctr" },
                conditions: new List<string> { $"segments.date BETWEEN {FREE_LISTINGS_AND_SHOPPING_ADS_DATE_RANGE}", "segments.program iN ('SHOPPING_ADS','FREE_PRODUCT_LISTING')", "metrics.clicks > 0", "metrics.impressions > 0" },
                pageSize: 50,
                orderField: "metrics.clicks DESC",
                limit: 50,
                pageToken: null);

            // Clicks and Impressions - Free Listings (Organic Ads)
            organicAdsReport = _googleShoppingAPIService.GetReport<MerchantPerformanceReport>(googleReportType: GoogleReportTypes.MerchantPerformanceView,
                fields: new List<string> { "segments.program", "segments.date", "segments.offer_id", "metrics.impressions", "metrics.clicks", "metrics.ctr" },
                conditions: new List<string> { $"segments.date BETWEEN {ORGANIC_ADS_DATE_RANGE}", "segments.program iN ('FREE_PRODUCT_LISTING')", "metrics.clicks > 0", "metrics.impressions > 0" },
                pageSize: 50,
                orderField: "metrics.clicks DESC",
                limit: 50,
                pageToken: null);

            // Clicks and Impressions - Shopping Ads
            shoppingAdsReport = _googleShoppingAPIService.GetReport<MerchantPerformanceReport>(googleReportType: GoogleReportTypes.MerchantPerformanceView,
                fields: new List<string> { "segments.program", "segments.date", "segments.offer_id", "metrics.impressions", "metrics.clicks", "metrics.ctr" },
                conditions: new List<string> { $"segments.date BETWEEN {SHOPPING_ADS_DATE_RANGE}", "segments.program iN ('SHOPPING_ADS')", "metrics.clicks > 0", "metrics.impressions > 0" },
                pageSize: 50,
                orderField: "metrics.clicks DESC",
                limit: 50,
                pageToken: null);

            // Price Gap Opportunities
            priceGapReport = _googleShoppingAPIService.GetReport<PriceCompetitivenessProductViewReport>(googleReportType: GoogleReportTypes.PriceCompetitivenessProductView,
                fields: new List<string> { "product_view.id", "product_view.title", "product_view.brand", "product_view.price_micros", "product_view.currency_code",
                    "price_competitiveness.country_code", "price_competitiveness.benchmark_price_micros", "price_competitiveness.benchmark_price_currency_code" },
                conditions: null,
                pageSize: 50,
                orderField: "product_view.id",
                limit: null,
                pageToken: null);

            // Price Suggestions Opportunities
            priceSuggestionsReport = _googleShoppingAPIService.GetReport<PriceInsightsProductViewReport>(googleReportType: GoogleReportTypes.PriceInsightsProductView,
                fields: new List<string> { "product_view.id", "product_view.title", "product_view.brand", "product_view.price_micros", "product_view.currency_code",
                    "price_insights.suggested_price_micros", "price_insights.suggested_price_currency_code", "price_insights.predicted_impressions_change_fraction",
                    "price_insights.predicted_clicks_change_fraction", "price_insights.predicted_conversions_change_fraction" },
                conditions: null,
                pageSize: 50,
                orderField: "product_view.id",
                limit: null,
                pageToken: null);

            // Shopping Experience Score Card 
            shoppingExperienceScoreCard = new GoogleResponse { Message = "Not available through API" };

            // Promotions
            promotions = _googleShoppingAPIService.GetPromotions();
        }

        public static ManagedServiceReport Step2ParseTheData(MerchantPerformanceReport freeListingsAndShoppingAdsReport, MerchantPerformanceReport organicAdsReport,
            MerchantPerformanceReport shoppingAdsReport, PriceCompetitivenessProductViewReport priceGapReport,
            PriceInsightsProductViewReport priceSuggestionsReport, GoogleResponse shoppingExperienceScoreCard, GooglePromotions promotions)
        {
            // 2) Parse the data

            var managedServiceReport = new ManagedServiceReport();

            // Clicks and Impressions - Free Listings and Shopping Ads
            managedServiceReport.FreeListingsAndShoppingAds = Calculate.CalculateMetricsReport(managedServiceReport.FreeListingsAndShoppingAds, freeListingsAndShoppingAdsReport);

            // Clicks and Impressions - Free Listings (Organic Ads)
            managedServiceReport.FreeListings = Calculate.CalculateMetricsReport(managedServiceReport.FreeListings, organicAdsReport);

            // Clicks and Impressions - Shopping Ads
            managedServiceReport.ShoppingAds = Calculate.CalculateMetricsReport(managedServiceReport.ShoppingAds, shoppingAdsReport);

            // Price Gap Opportunities
            managedServiceReport.PriceGapOpportunities = Calculate.CalculatePriceGapReport(managedServiceReport.PriceGapOpportunities, priceGapReport);

            // Price Suggestions Opportunities
            managedServiceReport.PriceSuggestionOpportunities = Calculate.CalculatePricInsightsReport(managedServiceReport.PriceSuggestionOpportunities, priceSuggestionsReport);

            // Shopping Experience Score Card 
            managedServiceReport.ShoppingExperienceScoreCard = shoppingExperienceScoreCard;

            // Promotions
            managedServiceReport.Promotions = null;

            return managedServiceReport;
        }

        public static void Step3WriteReportWithData(ManagedServiceReport managedServiceReport)
        {
            // Get the Downloads folder path
            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string reportFilePath = Path.Combine(downloadsPath, "Google Report.txt");

            // Create a StringBuilder to build the report content
            StringBuilder reportContent = new StringBuilder();

            // Add report title
            reportContent.AppendLine("Google Performance Report");
            reportContent.AppendLine();

            // Total Impressions Review
            reportContent.AppendLine("Total Impressions Review – " + (managedServiceReport.FreeListingsAndShoppingAds.TotalImpressions > 0 ? "Good" : "Needs Work"));
            reportContent.AppendLine();

            // Total Clicks Review
            reportContent.AppendLine("Total Clicks Review – " + (managedServiceReport.FreeListingsAndShoppingAds.TotalClicks > 0 ? "Good" : "Needs Work"));
            reportContent.AppendLine();

            // Shopping and Organic Ads
            reportContent.AppendLine("Shopping and Organic Ads");
            reportContent.AppendLine(FREE_LISTINGS_AND_SHOPPING_ADS_DATE_RANGE.Replace("AND", "to"));
            reportContent.AppendLine($"Clicks = {managedServiceReport.FreeListingsAndShoppingAds.TotalClicks} ({(managedServiceReport.FreeListingsAndShoppingAds.ClicksRateOfChange != null ? managedServiceReport.FreeListingsAndShoppingAds.ClicksRateOfChange : "N/A")})");
            reportContent.AppendLine($"Impressions = {managedServiceReport.FreeListingsAndShoppingAds.TotalImpressions} ({(managedServiceReport.FreeListingsAndShoppingAds.ImpressionsRateOfChange != null ? managedServiceReport.FreeListingsAndShoppingAds.ImpressionsRateOfChange : "N/A")})");
            reportContent.AppendLine($"CTR = {managedServiceReport.FreeListingsAndShoppingAds.CTR} ({(managedServiceReport.FreeListingsAndShoppingAds.CTRRateOfChange != null ? managedServiceReport.FreeListingsAndShoppingAds.CTRRateOfChange : "N/A")})");
            reportContent.AppendLine();

            // Organic Ads
            reportContent.AppendLine("Organic Ads");
            reportContent.AppendLine(ORGANIC_ADS_DATE_RANGE.Replace("AND", "to"));
            reportContent.AppendLine($"Clicks = {managedServiceReport.FreeListings.TotalClicks} ({(managedServiceReport.FreeListings.ClicksRateOfChange != null ? managedServiceReport.FreeListings.ClicksRateOfChange : "N/A")})");
            reportContent.AppendLine($"Impressions = {managedServiceReport.FreeListings.TotalImpressions} ({(managedServiceReport.FreeListings.ImpressionsRateOfChange != null ? managedServiceReport.FreeListings.ImpressionsRateOfChange : "N/A")})");
            reportContent.AppendLine($"CTR = {managedServiceReport.FreeListings.CTR} ({(managedServiceReport.FreeListings.CTRRateOfChange != null ? managedServiceReport.FreeListings.CTRRateOfChange : "N/A")})");
            reportContent.AppendLine($"Free Listings conversions = {managedServiceReport.FreeListings.FreeListingsConversions}");
            reportContent.AppendLine();

            // Shopping Ads
            reportContent.AppendLine("Shopping Ads");
            reportContent.AppendLine(SHOPPING_ADS_DATE_RANGE.Replace("AND", "to"));
            reportContent.AppendLine($"Clicks = {managedServiceReport.ShoppingAds.TotalClicks} ({(managedServiceReport.ShoppingAds.ClicksRateOfChange != null ? managedServiceReport.ShoppingAds.ClicksRateOfChange : "N/A")})");
            reportContent.AppendLine($"Impressions = {managedServiceReport.ShoppingAds.TotalImpressions} ({(managedServiceReport.ShoppingAds.ImpressionsRateOfChange != null ? managedServiceReport.ShoppingAds.ImpressionsRateOfChange : "N/A")})");
            reportContent.AppendLine($"CTR = {managedServiceReport.ShoppingAds.CTR} ({(managedServiceReport.ShoppingAds.CTRRateOfChange != null ? managedServiceReport.ShoppingAds.CTRRateOfChange : "N/A")})");
            reportContent.AppendLine();

            // Price Gap Opportunities
            reportContent.AppendLine("Price Gap Opportunities (Based on Google’s Analysis)");
            reportContent.AppendLine("Title\tYour price\tPrice on Google\tPrice gap\tClicks");
            foreach (var priceGapOpportunity in managedServiceReport.PriceGapOpportunities.OfType<GooglePriceGapOpportunitiesReport>())
            {
                reportContent.AppendLine($"{priceGapOpportunity.Title}\t${priceGapOpportunity.YourPrice:F2}\t${priceGapOpportunity.PriceOnGoogle:F2}\t{priceGapOpportunity.PriceGap}%\t{priceGapOpportunity.Clicks}");
            }
            reportContent.AppendLine();

            // Price Suggestion Opportunities
            reportContent.AppendLine("Price Suggestion Opportunities (Based on Google’s Analysis)");
            reportContent.AppendLine("Title\tYour price\tSuggested price\tClick uplift\tConversion uplift");
            // Skip if no data for Price Suggestion Opportunities
            if (managedServiceReport.PriceSuggestionOpportunities.Count == 0)
            {
                reportContent.AppendLine("Not Available");
                reportContent.AppendLine();
            }
            else
            {
                foreach (var priceSuggestionOpportunity in managedServiceReport.PriceSuggestionOpportunities.OfType<GooglePriceSuggestionOpportunitiesReport>())
                {
                    reportContent.AppendLine($"{priceSuggestionOpportunity.Title}\t${priceSuggestionOpportunity.YourPrice:F2}\t${priceSuggestionOpportunity.PriceOnGoogle:F2}\t{priceSuggestionOpportunity.ClickUplift}\t{priceSuggestionOpportunity.ConversionUplift}");
                }
                reportContent.AppendLine();
            }

            // Shopping Experience ScoreCard
            reportContent.AppendLine("Shopping Experience ScoreCard - " + (managedServiceReport.ShoppingExperienceScoreCard.Message != "Not available through API" ? "Okay" : "Not Available"));
            reportContent.AppendLine(managedServiceReport.ShoppingExperienceScoreCard.Message);
            reportContent.AppendLine();

            // Promotions
            reportContent.AppendLine("Promotions");
            reportContent.AppendLine("Not Available"); // Data is not available in the API for this section
            reportContent.AppendLine();

            // Write the report content to the file
            File.WriteAllText(reportFilePath, reportContent.ToString());
        }
    }
}
