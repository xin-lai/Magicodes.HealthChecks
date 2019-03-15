// ======================================================================
//   
//           Copyright (C) 2019-2020 湖南心莱信息科技有限公司    
//           All rights reserved
//   
//           filename : ConfigModelBase.cs
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
    public abstract class ConfigModelBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 默认路径
        /// </summary>
        public virtual string DefaultPath { get; set; }
    }
}