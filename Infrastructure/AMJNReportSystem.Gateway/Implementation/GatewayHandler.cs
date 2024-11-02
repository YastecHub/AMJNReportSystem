using AMJNReportSystem.Gateway.Extensions;
using AMJNReportSystem.Application;
using AMJNReportSystem.Application.Models;
using AMJNReportSystem.Application.Wrapper;
using Microsoft.Extensions.Configuration;
using System.Net;
using AMJNReportSystem.Domain.Entities;
using Newtonsoft.Json;
using System.Text;
using AMJNReportSystem.Application.Identity.Users;
using AMJNReportSystem.Application.Identity.Tokens;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;
using static System.Net.WebRequestMethods;

namespace AMJNReportSystem.Gateway.Implementations
{
    public class GatewayHandler : IGatewayHandler
    {
        private readonly HttpClient _client;
        private readonly string _baseApiPath;
        private readonly IConfigurationSection _config;
        private readonly IMemoryCache _cache;

        public GatewayHandler(IConfiguration configuration, IMemoryCache cache)
        {
            _client = new HttpClient();
            _config = configuration.GetSection("Api");
            _baseApiPath = _config.GetSection("Url").Value;
            _cache = cache;
        }

        public async Task<PaginatedResult<Muqam>> GeMuqamatAsync(PaginationFilter filter)
        {
            var url = $"{_baseApiPath}Utility/muqamaat?PageNumber={filter.PageNumber}&PageSize={filter.PageSize}";
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(url);
            request.Method = HttpMethod.Get;
            request.Headers.Add("ApiKey", _config.GetSection("ApiKey").Value);

            var response = await _client.SendAsync(request);
            if (response.StatusCode.Equals(HttpStatusCode.OK))
            {
                return await response.ReadContentAs<PaginatedResult<Muqam>>();
            }
            else if (response.StatusCode.Equals(HttpStatusCode.NotFound))
            {
                return null;
            }
            throw new Exception(response.StatusCode.ToString());
        }

        public async Task<PaginatedResult<Dila>> GetDilaatAsync(PaginationFilter filter)
        {
            var url = $"{_baseApiPath}Utility/dilaat?PageNumber={filter.PageNumber}&PageSize={filter.PageSize}";
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(url);
            request.Method = HttpMethod.Get;
            request.Headers.Add("ApiKey", _config.GetSection("ApiKey").Value);

            var response = await _client.SendAsync(request);
            if (response.StatusCode.Equals(HttpStatusCode.OK))
            {
                return await response.ReadContentAs<PaginatedResult<Dila>>();
            }
            else if (response.StatusCode.Equals(HttpStatusCode.NotFound))
            {
                return null;
            }
            throw new Exception(response.StatusCode.ToString());
        }

        public async Task<PaginatedResult<Zone>> GetZonesAsync(PaginationFilter filter)
        {
            var url = $"{_baseApiPath}Utility/zones?PageNumber={filter.PageNumber}&PageSize={filter.PageSize}";
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(url);
            request.Method = HttpMethod.Get;
            request.Headers.Add("ApiKey", _config.GetSection("ApiKey").Value);

            var response = await _client.SendAsync(request);
            if (response.StatusCode.Equals(HttpStatusCode.OK))
            {
                return await response.ReadContentAs<PaginatedResult<Zone>>();
            }
            else if (response.StatusCode.Equals(HttpStatusCode.NotFound))
            {
                return null;
            }
            throw new Exception(response.StatusCode.ToString());
        }


        public async Task<string[]> GetMemberRoleAsync(int chandaNo)
        {
            var url = $"{_config.Value}{chandaNo}/userRoles";
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(url);
            request.Method = HttpMethod.Get;
            request.Headers.Add("ApiKey", _config.GetSection("ApiKey").Value);

            var response = await _client.SendAsync(request);
            if (response.StatusCode.Equals(HttpStatusCode.OK))
            {
                return await response.ReadContentAs<string[]>();
            }
            else if (response.StatusCode.Equals(HttpStatusCode.NotFound))
            {
                return null;
            }
            throw new Exception(response.StatusCode.ToString());
        }


        public async Task<User> GetMemberByChandaNoAsync(int chandaNo)
        {
            var url = $"{_config.Value}members/{chandaNo}";
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(url);
            request.Method = HttpMethod.Get;
            request.Headers.Add("ApiKey", _config.GetSection("ApiKey").Value);

            var response = await _client.SendAsync(request);
            if (response.StatusCode.Equals(HttpStatusCode.OK))
            {
                return await response.ReadContentAs<User>();
            }
            else if (response.StatusCode.Equals(HttpStatusCode.NotFound))
            {
                return null;
            }
            throw new Exception(response.StatusCode.ToString());
        }

        public async Task<MemberApiLoginResponse> GenerateToken(TokenRequest tokenRequest)
        {
            var url = $"{_config.Value}token";
            var credentials = new TokenConstant
            {
                Username = tokenRequest.ChandaNo,
                Password = tokenRequest.Password
            };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = jsonContent
            };
            var response = await _client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = JsonConvert.DeserializeObject<MemberApiLoginResponse>(await response.Content.ReadAsStringAsync());
                return tokenResponse;
            }
            else if (response.StatusCode.Equals(HttpStatusCode.NotFound))
            {
                return null;
            }
            throw new Exception(response.StatusCode.ToString());
        }


        public async Task<List<Muqam>?> GetListOfMuqamAsync()
        {
            var muqamis = new List<Muqam>();
            try
            {
                var cacheKey = $"AhmadiyyaMuslimJamaat_001";
                muqamis = _cache.Get<List<Muqam>>(cacheKey);
                if (muqamis == null || muqamis.Count < 0)
                {
                    var apiUrl  = $"{_config.Value}jamaats"; ;
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        
                        HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                        if (response.IsSuccessStatusCode)
                        {
                            string jsonContent = await response.Content.ReadAsStringAsync();
                            muqamis = JsonConvert.DeserializeObject<List<Muqam>>(jsonContent);

                            var cacheEntryOptions = new MemoryCacheEntryOptions()
                                      .SetSlidingExpiration(TimeSpan.FromSeconds(120))
                                      .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                                      .SetPriority(CacheItemPriority.Normal);

                            _cache.Set(cacheKey, muqamis, cacheEntryOptions);
                            return muqamis;
                        }
                    }
                }

                return muqamis;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}