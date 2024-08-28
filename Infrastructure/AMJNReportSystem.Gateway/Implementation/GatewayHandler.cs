using AMJNReportSystem.Gateway.Extensions;
using AMJNReportSystem.Application;
using AMJNReportSystem.Application.Models;
using AMJNReportSystem.Application.Wrapper;
using Microsoft.Extensions.Configuration;
using System.Net;
using AMJNReportSystem.Domain.Entities;

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
    }
}