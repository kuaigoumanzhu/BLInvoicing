using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL.Framework.Cache
{
    /// <summary>
    /// 缓存策略，全局、局部
    /// </summary>
    public enum CacheVersionType
    {
        None,
        /// <summary>
        /// 提升缓存使用率，全局
        /// </summary>
        GlobalVersion,
        /// <summary>
        /// 提升缓存使用率，分区缓存
        /// </summary>
        AreaVersion
    }
}
