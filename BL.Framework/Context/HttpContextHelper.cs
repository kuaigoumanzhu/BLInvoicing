using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BL.Framework.Context
{
    /// <summary>
    /// 用于访问当前请求上下文的工具类
    /// </summary>
    public class HttpContextHelper
    {
        public static string AppRootPath
        {
            get { return HttpRuntime.AppDomainAppPath; }
        }
        public static string RequestFilePath
        {
            get { return HttpContext.Current.Request.FilePath; }
        }
        public static string RequestRawUrl
        {
            get { return HttpContext.Current.Request.RawUrl; }
        }
        public static string UserIdentityName
        {
            get
            {
                if (HttpContext.Current.Request.IsAuthenticated == false)
                    return null;
                else
                    return HttpContext.Current.User.Identity.Name;
            }
        }
    }
}
