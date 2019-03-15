using BeatPulse;
using BeatPulse.Core;
using Magicodes.HealthChecks.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Magicodes.HealthChecks
{
    public static class Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <returns></returns>
        public static IWebHostBuilder UseHealthChecks(this IWebHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, services) =>
            {
                var optionsConfiguration = new BeatPulseOptionsConfiguration();
                services.BuildServiceProvider().GetRequiredService<IConfiguration>().Bind("BeatPulseOptions", optionsConfiguration);
                optionsConfiguration.ConfigurePath(path: "health") //默认路径
                         .ConfigureOutputCache(5)   //缓存10秒
                         .ConfigureTimeout(milliseconds: 1500) // 超时时间
                         .ConfigureDetailedOutput(detailedOutput: true, includeExceptionMessages: true); //default (true,false)

                services.AddSingleton<IStartupFilter>(new BeatPulseFilter(optionsConfiguration));
            });

            return hostBuilder;
        }

        /// <summary>
        /// 添加监控检查,支持配置文件配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="beatPulseContext"></param>
        /// <returns></returns>
        public static IServiceCollection AddHealthChecks(
            this IServiceCollection services, IConfiguration configuration, BeatPulseContext beatPulseContext = null)
        {
            if (Convert.ToBoolean(configuration["HealthChecks:IsEnable"] ?? "false"))
            {
                services.AddBeatPulse(setup =>
                {
                    if (beatPulseContext != null)
                    {
                        setup = beatPulseContext;
                    }

                    var sqlServerSec = configuration.GetSection("HealthChecks:SqlServer");
                    if (sqlServerSec != null)
                    {
                        var sqlConfigs = sqlServerSec.Get<List<SqlServerConfigModel>>();
                        if (sqlConfigs != null)
                        {
                            foreach (var sqlConfig in sqlConfigs)
                            {
                                //添加对sql server的监控检查
                                setup.AddSqlServer(sqlConfig.ConnectionString, sqlConfig.HealthQuery, sqlConfig.Name, sqlConfig.DefaultPath);
                            }
                        }


                    }

                    var redisSec = configuration.GetSection("HealthChecks:Redis");
                    if (redisSec != null)
                    {
                        var configs = redisSec.Get<List<RedisConfigModel>>();
                        if (configs != null)
                        {
                            foreach (var config in configs)
                            {
                                //添加对Redis 的监控检查
                                setup.AddRedis(config.RedisConnectionString, config.Name, config.DefaultPath);
                            }
                        }
                    }

                    var pingSec = configuration.GetSection("HealthChecks:Pings");
                    if (pingSec != null)
                    {
                        var configs = pingSec.Get<List<PingConfigModel>>();
                        if (configs != null)
                        {
                            foreach (var config in configs)
                            {
                                //添加对Ping 的监控检查
                                setup.AddPingLiveness(options => options.AddHost(config.Host, config.Timeout), config.Name, config.DefaultPath);
                            }
                        }
                    }

                });
            }

            return services;
        }
    }
}
