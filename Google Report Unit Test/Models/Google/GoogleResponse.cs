using System.Text.Json.Serialization;

namespace GoogleReportUnitTest.Models.Google
{
    public class GoogleResponse
    {
        [JsonIgnore]
        public string Message { get; set; }
        [JsonIgnore]
        public bool IsSuccess { get; set; }
    }
}
