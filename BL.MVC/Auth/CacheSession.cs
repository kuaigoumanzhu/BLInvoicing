using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BL.Framework.Cache;
using BL.Framework;
using System.Web;
using BL.Models;

namespace BL.MVC
{
    public class CacheSession
    {
        ICache cache = DIContainer.Resolve<ICache>();
        private HttpContext context;
        public CacheSession(bool IsReadOnly)
        {
            this.IsReadOnly = IsReadOnly;
        }
        public CacheSession(HttpContext context, bool IsReadOnly, TimeSpan TimeOut, ICache cacheService)
        {
            this.context = context;
            this.IsReadOnly = IsReadOnly;
            this.TimeOut = TimeOut;
        }
        public CacheSession(HttpContext context, bool IsReadOnly)
        {
            this.context = context;
            this.IsReadOnly = IsReadOnly;
            GetSessionId();
            if (CacheKey.UserID != null)
            {
                var userInfo = cache.Get<UserInfo>(CacheKey.UserID);
            }
        }
        //获取会话是否已经验证通过
        public bool IsAuthenticated
        {
            get
            {
                if (cache.Get(SessionId) == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        //会话唯一Id
        public string SessionId
        {
            get
            {
                return GetSessionId();
            }
        }

        public static string SessionName = "ZS_MP";
        public static string UserSessionName = "Session_";
        //hpf 指示会话是否为只读，true为只读
        public bool IsReadOnly { get; set; }
        //hpf 超时期限
        public TimeSpan TimeOut { get; set; }

        private string GetSessionId()
        {
            HttpCookie cookie = context.Request.Cookies.Get(SessionName);
            string remoteBrowserIp=BL.Framework.Utilities.WebUtility.GetIP(); 
            if(cookie==null||string.IsNullOrEmpty(cookie.Value))
            {
                string newSessionId = Guid.NewGuid().ToString();
                HttpCookie newCookie = new HttpCookie(SessionName, newSessionId);
                newCookie.HttpOnly = IsReadOnly;
                context.Response.Cookies.Add(newCookie);
                return UserSessionName + remoteBrowserIp + ":" + newSessionId;
            }else
            {
                return UserSessionName + remoteBrowserIp + ":" + cookie.Value;
            }
        }
    }
}
