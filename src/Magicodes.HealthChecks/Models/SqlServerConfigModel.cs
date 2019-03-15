namespace Magicodes.HealthChecks.Models
{
    public class SqlServerConfigModel : ConfigModelBase
    {
        public SqlServerConfigModel()
        {
            Name = "SqlServerLiveness";
            DefaultPath = "sqlserver";
        }
        /// <summary>
        /// 连接字符串配置
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 监控查询语句
        /// </summary>
        public string HealthQuery { get; set; } = "SELECT 1;";

    }
}
