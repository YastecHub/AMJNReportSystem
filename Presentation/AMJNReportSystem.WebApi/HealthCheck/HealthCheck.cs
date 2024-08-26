using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AMJNReportSystem.WebApi.HealthCheck
{
    public class HealthCheck : IHealthCheck
    {
        private Random _random = new Random();

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            var responseTime = _random.Next(1, 300);

            if (responseTime < 100)
            {
                return Task.FromResult(HealthCheckResult.Healthy("Healthy result from HealthCheck"));
            }
            else if (responseTime < 200)
            {
                return Task.FromResult(HealthCheckResult.Degraded("Degraded result from HealthCheck"));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("Unhealthy result from HealthCheck"));
        }
    }
}
