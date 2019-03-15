// ======================================================================
//   
//           Copyright (C) 2019-2020 湖南心莱信息科技有限公司    
//           All rights reserved
//   
//           filename : RedisConfigModel.cs
//           description :
//   
//           created by 雪雁 at  -- 
//           Mail: wenqiang.li@xin-lai.com
//           QQ群：85318032（编程交流）
//           Blog：http://www.cnblogs.com/codelove/
//           GitHub：https://github.com/xin-lai
//           Home：http://xin-lai.com
//           微信订阅号：magiccodes
//   
// ======================================================================
namespace Magicodes.HealthChecks.Models
{
    public class RedisConfigModel : ConfigModelBase
    {
        public RedisConfigModel()
        {
            Name = "RedisLiveness";
            DefaultPath = "redis";
        }
        public string RedisConnectionString { get; set; }
    }
}