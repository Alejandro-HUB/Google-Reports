using Newtonsoft.Json;

namespace GoogleReportUnitTest.Models.Google
{
    public partial class GooglePromotions : GoogleResponse
    {
        [JsonProperty("promotions")]
        public List<Promotion> Promotions { get; set; }

        [JsonProperty("nextPageToken")]
        public string NextPageToken { get; set; }
    }

    public partial class Promotion
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("targetCountry")]
        public string TargetCountry { get; set; }

        [JsonProperty("contentLanguage")]
        public string ContentLanguage { get; set; }

        [JsonProperty("promotionId")]
        public string PromotionId { get; set; }

        [JsonProperty("productApplicability")]
        public string ProductApplicability { get; set; }

        [JsonProperty("offerType")]
        public string OfferType { get; set; }

        [JsonProperty("longTitle")]
        public string LongTitle { get; set; }

        [JsonProperty("promotionEffectiveDates")]
        public string PromotionEffectiveDates { get; set; }

        [JsonProperty("redemptionChannel")]
        public List<string> RedemptionChannel { get; set; }

        [JsonProperty("couponValueType")]
        public string CouponValueType { get; set; }

        [JsonProperty("promotionDestinationIds")]
        public List<string> PromotionDestinationIds { get; set; }

        [JsonProperty("itemId")]
        public List<string> ItemId { get; set; }

        [JsonProperty("brand")]
        public List<string> Brand { get; set; }

        [JsonProperty("itemGroupId")]
        public List<string> ItemGroupId { get; set; }

        [JsonProperty("productType")]
        public List<string> ProductType { get; set; }

        [JsonProperty("itemIdExclusion")]
        public List<string> ItemIdExclusion { get; set; }

        [JsonProperty("brandExclusion")]
        public List<string> BrandExclusion { get; set; }

        [JsonProperty("itemGroupIdExclusion")]
        public List<string> ItemGroupIdExclusion { get; set; }

        [JsonProperty("productTypeExclusion")]
        public List<string> ProductTypeExclusion { get; set; }

        [JsonProperty("shippingServiceNames")]
        public List<string> ShippingServiceNames { get; set; }

        [JsonProperty("promotionEffectiveTimePeriod")]
        public PromotionTimePeriod PromotionEffectiveTimePeriod { get; set; }

        [JsonProperty("storeCode")]
        public List<string> StoreCode { get; set; }

        [JsonProperty("storeCodeExclusion")]
        public List<string> StoreCodeExclusion { get; set; }

        [JsonProperty("promotionStatus")]
        public PromotionStatus PromotionStatus { get; set; }

        [JsonProperty("genericRedemptionCode")]
        public string GenericRedemptionCode { get; set; }

        [JsonProperty("promotionDisplayDates")]
        public string PromotionDisplayDates { get; set; }

        [JsonProperty("minimumPurchaseAmount")]
        public PriceAmount MinimumPurchaseAmount { get; set; }

        [JsonProperty("minimumPurchaseQuantity")]
        public long MinimumPurchaseQuantity { get; set; }

        [JsonProperty("limitQuantity")]
        public long LimitQuantity { get; set; }

        [JsonProperty("limitValue")]
        public PriceAmount LimitValue { get; set; }

        [JsonProperty("percentOff")]
        public long PercentOff { get; set; }

        [JsonProperty("moneyOffAmount")]
        public PriceAmount MoneyOffAmount { get; set; }

        [JsonProperty("getThisQuantityDiscounted")]
        public long GetThisQuantityDiscounted { get; set; }

        [JsonProperty("PriceAmount")]
        public PriceAmount PriceAmount { get; set; }

        [JsonProperty("freeGiftDescription")]
        public string FreeGiftDescription { get; set; }

        [JsonProperty("freeGiftItemId")]
        public string FreeGiftItemId { get; set; }

        [JsonProperty("moneyBudget")]
        public PriceAmount MoneyBudget { get; set; }

        [JsonProperty("orderLimit")]
        public long OrderLimit { get; set; }

        [JsonProperty("promotionDisplayTimePeriod")]
        public PromotionTimePeriod PromotionDisplayTimePeriod { get; set; }

        [JsonProperty("storeApplicability")]
        public string StoreApplicability { get; set; }

        [JsonProperty("promotionUrl")]
        public string PromotionUrl { get; set; }
    }

    public partial class PriceAmount
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }

    public partial class PromotionTimePeriod
    {
        [JsonProperty("startTime")]
        public string StartTime { get; set; }

        [JsonProperty("endTime")]
        public string EndTime { get; set; }
    }

    public partial class PromotionStatus
    {
        [JsonProperty("destinationStatuses")]
        public List<DestinationStatus> DestinationStatuses { get; set; }

        [JsonProperty("promotionIssue")]
        public List<PromotionIssue> PromotionIssue { get; set; }

        [JsonProperty("creationDate")]
        public string CreationDate { get; set; }

        [JsonProperty("lastUpdateDate")]
        public string LastUpdateDate { get; set; }
    }

    public partial class DestinationStatus
    {
        [JsonProperty("destination")]
        public string Destination { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public partial class PromotionIssue
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("detail")]
        public string Detail { get; set; }
    }
}
