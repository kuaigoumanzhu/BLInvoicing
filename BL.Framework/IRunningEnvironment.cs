using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL.Framework
{
    /// <summary>
    /// 运行环境接口
    /// </summary>
    public interface IRunningEnvironment
    {
        /// <summary>
        /// 重新启动AppDomain
        /// </summary>
        void RestartAppDomain();
        /// <summary>
        /// 是否完全信任运行环境
        /// </summary>
        /// <returns></returns>
        bool IsFullTrust { get; }
    }
}
