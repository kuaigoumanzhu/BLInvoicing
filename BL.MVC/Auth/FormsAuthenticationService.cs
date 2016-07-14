using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BL.Framework.Cache;
using System.Web;
using BL.Framework;
using BL.Models;

namespace BL.MVC
{
    /// <summary>
    /// hpf基于Form的身份认证服务实现
    /// </summary>
    public class FormsAuthenticationService : IAuthenticationService
    {
        ICache cacheService;
        CacheSession cacheSession;
        HttpContext httpContext = HttpContext.Current;
        //hpf 缓存相关
        public FormsAuthenticationService()
        {
            cacheService = DIContainer.Resolve<ICache>();
            cacheSession = new CacheSession(httpContext, true);
        }
        /// <summary>
        /// 获取当前认证的用户
        /// </summary>
        /// <returns>当前用户未通过认证则返回null</returns>
        public UserInfo GetAuthenticatedUser()
        {
            if (httpContext == null || !cacheSession.IsAuthenticated)
            {
                return null;//hpf未登录
            }
            return cacheService.Get<UserInfo>(cacheSession.SessionId);
        }
        public void SignIn(string loginName, UserInfo userData, TimeSpan expiration)
        {
            var sessionId = cacheSession.SessionId;
            cacheService.Set(sessionId, userData, expiration);
        }
        public void SignOut()
        {
            if (!string.IsNullOrEmpty(CacheKey.UserID))
            {
                cacheService.Remove(CacheKey.UserID);
            }
            //centralService.LogOut(FW.Security.AppKey.AppID);//hpf 如果有写用户登录退出
        }
    }
}
