using System;
using System.Collections.Generic;
using HealthChecks.Hangfire;
using Magicodes.HealthChecks.Core.Checks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Magicodes.HealthChecks.Core
{
    public static class Extensions
    {
        /// <summary>
        ///     添加监控检查,支持配置文件配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddHealthChecks(
            this IServiceCollection services, IConfiguration configuration)
        {
            if (Convert.ToBoolean(configuration["HealthChecks-UI:IsEnable"]))
            {
                var healthChecksService = services
                    .AddHealthChecks();

                //添加对sql server的监控检查
                if (Convert.ToBoolean(configuration["HealthChecks-UI:SqlServer:IsEnable"]))
                    healthChecksService.AddSqlServer(
                        configuration["ConnectionStrings:Default"],
                        configuration["HealthChecks-UI:SqlServer:HealthQuery"],
                        configuration["HealthChecks-UI:SqlServer:Name"],
                        HealthStatus.Degraded,
                        new string[] { "db", "sql", "sqlserver" }
                        );

                //添加对Redis的监控检查
                if (Convert.ToBoolean(configuration["HealthChecks-UI:Redis:IsEnable"]))
                    healthChecksService.AddRedis(
                        configuration["Abp:RedisCache:ConnectionString"], 
                        tags: new string[] { "redis" }
                    );

                //添加对Hangfire的监控检查
                if (Convert.ToBoolean(configuration["HealthChecks-UI:Hangfire:IsEnable"]))
                    healthChecksService.AddHangfire(options =>
                    {
                        options.MaximumJobsFailed = Convert.ToInt32(configuration["HealthChecks-UI:Hangfire:MaximumJobsFailed"]);
                        options.MinimumAvailableServers = Convert.ToInt32(configuration["HealthChecks-UI:Hangfire:MinimumAvailableServers"]);
                    }, tags: new string[] { "hangfire" });

                //添加对SignalR的监控检查
                if (Convert.ToBoolean(configuration["HealthChecks-UI:SignalR:IsEnable"]))
                    healthChecksService.AddSignalRHub(configuration["HealthChecks-UI:SignalR:Url"],tags: new string[] { "signalr" });

                //添加对Pings的监控检查
                if (Convert.ToBoolean(configuration["HealthChecks-UI:Pings:IsEnable"]))
                    healthChecksService.AddPingHealthCheck(setup =>
                    {
                        setup.AddHost(configuration["HealthChecks-UI:Pings:Host"], Convert.ToInt32(configuration["HealthChecks-UI:Pings:TimeOut"]));
                    }, tags: new string[] { "ping" });

                //添加对I/O的监控检查
                if (Convert.ToBoolean(configuration["HealthChecks-UI:Io:IsEnable"]))
                    healthChecksService.AddCheck<IoHealthCheck>("io");
            }
            return services;
        }
    }
}