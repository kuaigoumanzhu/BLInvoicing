using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BL.MVC
{
    public class CacheKey
    {
        public static string SessionName = "ZS_MP";
        public static string UserSessionName = "Session_";
        private static string GetSessionId()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(SessionName);
            string remoteBrowserIp = BL.Framework.Utilities.WebUtility.GetIP();
            return UserSessionName + remoteBrowserIp + ":" + cookie.Value;
        }
        public static string UserID
        {
            get { return GetSessionId(); }
        }
    }
}
