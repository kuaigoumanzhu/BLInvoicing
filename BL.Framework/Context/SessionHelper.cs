using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BL.Framework.Context
{
    public class SessionHelper
    {
        /// <summary>
        /// 根据session名称获取session对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object GetSession(string name)
        {
            return HttpContext.Current.Session[name];

        }
        /// <summary>
        /// 设置session
        /// </summary>
        /// <param name="name">session名</param>
        /// <param name="value">session值</param>
        /// <param name="iExpires">默认为20分钟</param>
        public static void SetSession(string name, object value, int iExpires = 20)
        {
            HttpContext.Current.Session.Remove(name);
            HttpContext.Current.Session.Add(name, value);
            HttpContext.Current.Session.Timeout = iExpires;
        }
        /// <summary>
        /// 清空所有session
        /// </summary>
        public static void ClearAllSession()
        {
            HttpContext.Current.Session.Clear();
        }
        /// <summary>
        /// 删除一个指定的session
        /// </summary>
        /// <param name="name"></param>
        public static void RemoveSession(string name)
        {
            HttpContext.Current.Session.Remove(name);
        }
        /// <summary>
        /// 删除所有的session
        /// </summary>
        public static void RemoveAllSession()
        {
            HttpContext.Current.Session.RemoveAll();
        }
    }
}
