using GoogleReportUnitTest.Models.GDF;
using GoogleReportUnitTest.Models;

namespace GoogleReportUnitTest.Helpers
{
    public static class Calculate
    {
        public static GoogleMetricsReport CalculateMetricsReport(GoogleMetricsReport googleMetricsReport,
            MerchantPerformanceReport merchantPerformanceReport, bool isFreeListings = false)
        {
            if (merchantPerformanceReport.IsSuccess && merchantPerformanceReport?.Results != null)
            {
                googleMetricsReport = new GoogleMetricsReport();

                // Total Clicks
                googleMetricsReport.TotalClicks = merchantPerformanceReport.Results.Select(r => r.Metrics.Clicks).Sum();

                // Clicks Rate of Change
                googleMetricsReport.ClicksRateOfChange = Conversion.CalculateRateOfChange(merchantPerformanceReport.Results.Select(r => (float)r.Metrics.Clicks).ToList());

                // Total Impressions
                googleMetricsReport.TotalImpressions = merchantPerformanceReport.Results.Select(r => r.Metrics.Impressions).Sum();

                // Impressions Rate of Change
                googleMetricsReport.ImpressionsRateOfChange = Conversion.CalculateRateOfChange(merchantPerformanceReport.Results.Select(r => (float)r.Metrics.Impressions).ToList());

                // Overall CTR
                googleMetricsReport.CTR = googleMetricsReport.TotalImpressions > 0 ? googleMetricsReport.TotalClicks / googleMetricsReport.TotalImpressions : 0;

                // CTR Rate of Change
                googleMetricsReport.CTRRateOfChange = Conversion.CalculateRateOfChange(merchantPerformanceReport.Results.Select(r => r.Metrics.Ctr).ToList());

                // Free Listings Conversions
                googleMetricsReport.FreeListingsConversions = isFreeListings ? merchantPerformanceReport.Results.Count(r => r.Metrics.Clicks > 0) : null;
            }

            return googleMetricsReport;
        }

        public static List<GooglePriceGapOpportunitiesReport> CalculatePriceGapReport(List<GooglePriceGapOpportunitiesReport> googlePriceGapOpportunitiesReport,
            PriceCompetitivenessProductViewReport priceGapReport)
        {
            if (priceGapReport.IsSuccess && priceGapReport?.Results != null)
            {
                googlePriceGapOpportunitiesReport = new List<GooglePriceGapOpportunitiesReport>();

                foreach (var reportEntry in priceGapReport.Results)
                {
                    if (reportEntry?.PriceCompetitiveness != null)
                    {
                        var entry = new GooglePriceGapOpportunitiesReport();

                        // Title
                        entry.Title = reportEntry.ProductView.Title;

                        // YourPrice
                        entry.YourPrice = Conversion.ConvertMicrosToUSD(reportEntry.ProductView.PriceMicros);

                        // PriceOnGoogle
                        entry.PriceOnGoogle = Conversion.ConvertMicrosToUSD(reportEntry.PriceCompetitiveness.BenchmarkPriceMicros);

                        // PriceGap
                        entry.PriceGap = Conversion.CalculatePriceGap(entry.YourPrice, entry.PriceOnGoogle);

                        // Clicks
                        entry.Clicks = 0;

                        // Add entry
                        googlePriceGapOpportunitiesReport.Add(entry);
                    }
                }

            }

            return googlePriceGapOpportunitiesReport;
        }

        public static List<GooglePriceSuggestionOpportunitiesReport> CalculatePricInsightsReport(List<GooglePriceSuggestionOpportunitiesReport> googlePriceSuggestionOpportunities,
            PriceInsightsProductViewReport priceSuggestionsReport)
        {
            if (priceSuggestionsReport.IsSuccess && priceSuggestionsReport?.Results != null)
            {
                googlePriceSuggestionOpportunities = new List<GooglePriceSuggestionOpportunitiesReport>();

                foreach (var reportEntry in priceSuggestionsReport.Results)
                {
                    if (reportEntry?.PriceInsights != null)
                    {
                        var entry = new GooglePriceSuggestionOpportunitiesReport();

                        // Title
                        entry.Title = reportEntry.ProductView.Title;

                        // YourPrice
                        entry.YourPrice = Conversion.ConvertMicrosToUSD(reportEntry.ProductView.PriceMicros);

                        // PriceOnGoogle
                        entry.PriceOnGoogle = Conversion.ConvertMicrosToUSD(reportEntry.PriceInsights.SuggestedPriceMicros);

                        // ClickUplift
                        entry.ClickUplift = Conversion.CalculateUplift(entry.YourPrice, reportEntry.PriceInsights.PredictedClicksChangeFraction);

                        // ConversionUplift
                        entry.ConversionUplift = Conversion.CalculateUplift(entry.YourPrice, reportEntry.PriceInsights.PredictedConversionsChangeFraction);

                        // Add entry
                        googlePriceSuggestionOpportunities.Add(entry);
                    }
                }

            }

            return googlePriceSuggestionOpportunities;
        }
    }
}
