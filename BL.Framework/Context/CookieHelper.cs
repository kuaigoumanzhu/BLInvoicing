using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BL.Framework.Context
{
    public class CookieHelper
    {
        /// <summary>
        /// 清除指定Cookie(设置超时)
        /// </summary>
        /// <param name="cookieName">cookie名称</param>
        public static void ClearCookie(string cookieName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddYears(-3);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }
        /// <summary>
        /// 清除指定cookie
        /// </summary>
        /// <param name="cookieName">cookie名称</param>
        public static void RemoveCookie(string cookieName)
        {
            HttpContext.Current.Response.Cookies.Remove(cookieName);
        }
        /// <summary>
        /// 清除所有的cookie
        /// </summary>
        public static void ClearAllCookie()
        {
            HttpContext.Current.Response.Cookies.Clear();
        }
        /// <summary>
        /// 获取指定cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static string GetCookieValue(string cookieName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            string str = string.Empty;
            if (cookie != null)
            {
                str = HttpUtility.UrlDecode(cookie.Value);
            }
            return str;
        }
        /// <summary>
        /// 添加一个Cookie
        /// </summary>
        /// <param name="cookiename">cookie名</param>
        /// <param name="cookievalue">cookie值</param>
        /// <param name="expires">过期时间 DateTime</param>
        public static void AddCookie(string cookiename, string cookievalue, double days = 1.0)
        {
            HttpCookie cookie = new HttpCookie(cookiename)
            {
                Value = cookievalue,
                Expires = DateTime.Now.AddDays(days)
            };
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        /// <summary>
        /// 更新Cookie
        /// </summary>
        /// <param name="cookiename">cookie名</param>
        /// <param name="cookievalue">cookie值</param>
        /// <param name="expires">过期时间 DateTime</param>
        public static void SetCookie(string cookiename, string cookievalue, double days = 1.0)
        {
            HttpCookie cookie = new HttpCookie(cookiename)
            {
                Value = cookievalue,
                Expires = DateTime.Now.AddDays(days)
            };
            HttpContext.Current.Response.Cookies.Set(cookie);
        }
    }
}
