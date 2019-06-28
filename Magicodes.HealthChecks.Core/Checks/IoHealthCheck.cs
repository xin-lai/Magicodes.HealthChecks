using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Security.AccessControl;

namespace Magicodes.HealthChecks.Core.Checks
{
    public class IoHealthCheck : IHealthCheck
    {
        private readonly string _tempPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Temp", "Downloads");


        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            if (_tempPath!=null)
            {
                DirectoryInfo dir = new DirectoryInfo(_tempPath);
                var fileAcl = dir.GetAccessControl();
                var rules = fileAcl.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount)).OfType<FileSystemAccessRule>().ToList();
                if (rules.Exists(a => a.FileSystemRights == FileSystemRights.CreateFiles
                                      || a.FileSystemRights == FileSystemRights.WriteData
                                      || a.FileSystemRights == FileSystemRights.ExecuteFile
                                      || a.FileSystemRights == FileSystemRights.Traverse
                                      || a.FileSystemRights == FileSystemRights.ReadAndExecute
                                      || a.FileSystemRights == FileSystemRights.Write
                                      || a.FileSystemRights == FileSystemRights.Modify))
                {
                    return Task.FromResult(HealthCheckResult.Healthy());
                }
                else
                {
                    return Task.FromResult(HealthCheckResult.Unhealthy("无写入权限"));
                }
            }
            return Task.FromResult(HealthCheckResult.Unhealthy(description: "文件夹不存在！"));
        }
    }
}
