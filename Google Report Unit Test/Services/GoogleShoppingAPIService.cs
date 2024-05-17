using GoogleReportUnitTest.Models.Google;
using GoogleReportUnitTest.API;
using GoogleReportUnitTest.Enum;
using Newtonsoft.Json;

namespace GoogleReportUnitTest.Services
{
    public class GoogleShoppingAPIService
    {
        private GoogleShoppingAPIClient _googleShoppingAPIClient { get; set; }
        private string _merchantId;
        public GoogleShoppingAPIService(string merchantId, string refreshToken) 
        { 
            _merchantId = merchantId;
            _googleShoppingAPIClient = new GoogleShoppingAPIClient(refreshToken);
        }

        public T GetReport<T>(GoogleReportTypes googleReportType, List<string> fields, List<string> conditions = null, 
            int? pageSize = null, string pageToken = null, string orderField = null, int? limit = null) where T : GoogleResponse, new()
        {
            var reportType = googleReportType.ToString();
            var predicate = conditions == null ? null : $"WHERE {string.Join(" AND ", conditions ?? new List<string>())}";
            var orderBy = string.IsNullOrEmpty(orderField) ? null : $" ORDER BY {orderField}";
            var limitNumber = limit == null ? null : $" LIMIT {limit.Value}";
            var query = $"SELECT {string.Join(",", fields)} FROM {reportType} {predicate}{orderBy ?? ""}{limitNumber ?? ""}";
            var requestBody = new
            {
                query = query,
                pageSize = pageSize,
                pageToken = pageToken,
            };

            switch (googleReportType)
            {
                case GoogleReportTypes.MerchantPerformanceView:
                    break;
                case GoogleReportTypes.PriceCompetitivenessProductView:
                case GoogleReportTypes.PriceInsightsProductView:
                    if (conditions != null && conditions.Any(x => x.ToLower().Contains("metrics.date")))
                    {
                        throw new Exception("This report does not support filtering by date.");
                    }
                    break;
                default:
                    return null;
            }

            return _googleShoppingAPIClient.SendRequest<T>(HttpMethod.Post, $"{_merchantId}/reports/search", JsonConvert.SerializeObject(requestBody));
        }

        public GooglePromotions GetPromotions()
        {
            return _googleShoppingAPIClient.SendRequest<GooglePromotions>(HttpMethod.Get, $"{_merchantId}/promotions");
        }
    }
}
