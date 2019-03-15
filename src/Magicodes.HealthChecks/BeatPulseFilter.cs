// ======================================================================
//   
//           Copyright (C) 2019-2020 湖南心莱信息科技有限公司    
//           All rights reserved
//   
//           filename : BeatPulseFilter.cs
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

using System;
using BeatPulse;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Magicodes.HealthChecks
{
    internal class BeatPulseFilter : IStartupFilter
    {
        private readonly BeatPulseOptions _options;

        public BeatPulseFilter(BeatPulseOptions options)
        {
            this._options = options;
        }

        public Action<IApplicationBuilder> Configure(
            Action<IApplicationBuilder> next)
        {
            return (Action<IApplicationBuilder>)(builder =>
            {
                builder.MapWhen((Func<HttpContext, bool>)(context => context.IsBeatPulseRequest(this._options)), (Action<IApplicationBuilder>)(appBuilder => appBuilder.UseMiddleware<BeatPulseMiddleware>((object)this._options)));
                next(builder);
            });
        }
    }
}