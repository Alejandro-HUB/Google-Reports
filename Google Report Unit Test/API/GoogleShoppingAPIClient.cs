using Newtonsoft.Json;
using System.Configuration;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Web;
using GoogleReportUnitTest.Models.Google;

namespace GoogleReportUnitTest.API
{
    public class GoogleShoppingAPIClient
    {
        private string _baseURI;
        private string _clientId;
        private string _clientSecret;
        private string _refreshToken;
        private string _accessToken;
        public GoogleShoppingAPIClient(string refreshToken)
        {
            _baseURI = "https://shoppingcontent.googleapis.com/content/v2.1/";
            _clientId = ConfigurationManager.AppSettings["OAuth2ClientId"]?.ToString();
            _clientSecret = ConfigurationManager.AppSettings["OAuth2ClientSecret"]?.ToString();
            _refreshToken = refreshToken;
            if (RefreshToken() == null)
            {
                throw new Exception("Failed to refresh token");
            }
        }

        public GoogleShoppingAPIRefreshToken RefreshToken()
        {
            var requestBody = new
            {
                client_id = _clientId,
                client_secret = _clientSecret,
                refresh_token = _refreshToken,
                grant_type = "refresh_token"
            };
            var response = SendRequest<GoogleShoppingAPIRefreshToken>(HttpMethod.Post, "o/oauth2/token", JsonConvert.SerializeObject(requestBody), null, null, "https://accounts.google.com/");
            if (response.IsSuccess)
            {
                _accessToken = response.AccessToken;
                return response;
            }
            else
            {
                return null;
            }
        }

        public T SendRequest<T>(HttpMethod requestMethod, string uri,
         string body = null, Dictionary<string, string> headers = null,
         Dictionary<string, string> queryParameters = null, string ovewriteBaseURL = "") where T : GoogleResponse, new()
        {


            int retryCount = 0;
            int retryTime = 3000; //Miliseconds (3 seconds)
            int maxRetries = 2;
            var responseString = string.Empty;
            T responseContent = new T();

        RetryTimestamp:

            try
            {
                // Create a new HttpClientHandler with automatic decompression
                var handler = new HttpClientHandler()
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                };

                // Create a new HttpClient
                using (HttpClient client = new HttpClient(handler))
                {
                    // Add headers to request
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                    client.DefaultRequestHeaders.Add("User-Agent", "GoDataFeed");
                    client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
                    client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                    if (headers != null)
                    {
                        foreach (var header in headers)
                        {
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                        }
                    }

                    // Add Auth Header
                    if (string.IsNullOrEmpty(ovewriteBaseURL))
                    {
                        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_accessToken}");
                    }

                    // Add Query Parameters to Request Uri
                    var baseURI = string.IsNullOrEmpty(ovewriteBaseURL) ? _baseURI : ovewriteBaseURL;
                    var requestUriBuilder = new UriBuilder(GetURI(baseURI, uri));
                    if (queryParameters != null)
                    {
                        var queryParamBuilder = HttpUtility.ParseQueryString(requestUriBuilder.Query);
                        foreach (var queryParam in queryParameters)
                        {
                            queryParamBuilder[queryParam.Key] = queryParam.Value;
                        }

                        requestUriBuilder.Query = queryParamBuilder.ToString();
                    }

                    // Create a new HttpRequestMessage with the specified HttpMethod and Request Uri
                    var request = new HttpRequestMessage(requestMethod, requestUriBuilder.Uri);

                    // Add Body to Request
                    if ((requestMethod == HttpMethod.Post
                        || requestMethod == HttpMethod.Put
                        || requestMethod == HttpMethod.Delete)
                        && !string.IsNullOrEmpty(body))
                    {
                        request.Content = new StringContent(body, Encoding.UTF8, "application/json");
                    }

                    // Get Response
                    var response = client.SendAsync(request).Result;
                    responseString = response.Content.ReadAsStringAsync().Result;

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        // Deserialize JSON directly from the response content
                        using (var stream = response.Content.ReadAsStreamAsync().Result)
                        using (var reader = new StreamReader(stream))
                        using (var jsonReader = new JsonTextReader(reader))
                        {
                            var serializer = JsonSerializer.CreateDefault();
                            responseContent = serializer.Deserialize<T>(jsonReader);
                            responseContent.IsSuccess = true;
                            responseContent.Message = responseString;
                        }
                    }
                    else if (response.StatusCode != HttpStatusCode.OK && retryCount <= maxRetries)
                    {
                        Thread.Sleep(retryTime);
                        retryCount++;
                        goto RetryTimestamp;
                    }
                    else
                    {
                        responseContent.IsSuccess = false;
                        responseContent.Message = responseString;
                    }
                }

                return responseContent;
            }
            catch (Exception ex)
            {
                responseContent.IsSuccess = false;
                responseContent.Message = ex.Message;
                return responseContent;
            }
        }

        /// <summary>
        /// Joins a base uri to a path
        /// </summary>
        /// <param name="baseURI"></param>
        /// <param name="postURI"></param>
        /// <returns></returns>
        protected string GetURI(string stringBaseURI, string postURI)
        {
            try
            {
                var baseURI = new Uri(stringBaseURI);
                var uri = new Uri(baseURI, postURI).OriginalString;
                return uri;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
