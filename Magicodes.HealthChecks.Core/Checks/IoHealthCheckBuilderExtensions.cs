using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Magicodes.HealthChecks.Core.Checks
{
    public static class IoHealthCheckBuilderExtensions
    {
        public static IHealthChecksBuilder AddIo(this IHealthChecksBuilder builder, string path, string name, HealthStatus? failureStatus = default,
            IEnumerable<string> tags = default)
        {
            return builder.Add(new HealthCheckRegistration(
                name,
                sp => new IoHealthCheck(path),
                failureStatus,
                tags));
        }
    }
}
