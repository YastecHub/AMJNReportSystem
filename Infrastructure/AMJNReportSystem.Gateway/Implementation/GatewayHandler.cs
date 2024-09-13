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

namespace AMJNReportSystem.Gateway.Implementations
{
    public class GatewayHandler : IGatewayHandler
    {
        private readonly HttpClient _client;
        private readonly string _baseApiPath;
        private readonly IConfigurationSection _config;

        public GatewayHandler(IConfiguration configuration)
        {
            _client = new HttpClient();
            _config = configuration.GetSection("Api");
            _baseApiPath = _config.GetSection("Url").Value;
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


        public async Task<UserApi> GetMemberByChandaNoAsync(int chandaNo)
        {
            var url = $"{_config.Value}members/{chandaNo}";
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(url);
            request.Method = HttpMethod.Get;
            request.Headers.Add("ApiKey", _config.GetSection("ApiKey").Value);

            var response = await _client.SendAsync(request);
            if (response.StatusCode.Equals(HttpStatusCode.OK))
            {
                return await response.ReadContentAs<UserApi>();
            }
            else if (response.StatusCode.Equals(HttpStatusCode.NotFound))
            {
                return null;
            }
            throw new Exception(response.StatusCode.ToString());
        }

        public async Task<TokenResponse> GenerateToken(MemberLoginRequest memberLoginRequest)
        {
            var url = $"{_config.Value}token";
            var credentials = new TokenConstant
            {
                Username = memberLoginRequest.ChandaNo,
                Password = memberLoginRequest.Password
            };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = jsonContent
            };
            var response = await _client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(await response.Content.ReadAsStringAsync());
                return tokenResponse;
            }
            else if (response.StatusCode.Equals(HttpStatusCode.NotFound))
            {
                return null;
            }
            throw new Exception(response.StatusCode.ToString());
        }

    }
}