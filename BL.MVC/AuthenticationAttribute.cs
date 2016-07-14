using BL.Framework;
using BL.Framework.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BL.MVC
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class AuthenticationAttribute : AuthorizeAttribute
    {
        ICache cacheService = DIContainer.Resolve<ICache>();
        CacheSession session;
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                bool disableCache = false;
                HttpContext httpContext = HttpContext.Current;
                //hpf 2015 11 每次响应都给一个cookie
                session = new CacheSession(httpContext, true);
                string ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                string ActionName = filterContext.ActionDescriptor.ActionName;
                object[] actionFilter = filterContext.ActionDescriptor.GetCustomAttributes(typeof(NoFilterAttribute), false);
                object[] controlllerFilter = filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(NoFilterAttribute), false);
                if (actionFilter.Length == 1)
                {
                    //无需验证
                    return;
                }
                if (httpContext == null || !session.IsAuthenticated)
                {
                    //没有登录
                    if (filterContext.HttpContext.Response.StatusCode != 301)
                    {
                        filterContext.Result = new RedirectResult("/Login/Login", true);
                        disableCache = true;
                    }
                }
                else
                {
                    //登录成功
                    if (UserContext.CurrentUser != null)
                    {

                    }
                    else
                    {
                        //登录超时
                        if (filterContext.HttpContext.Response.StatusCode != 301)
                        {
                            filterContext.Result = new RedirectResult("/Login/Login", true);
                            disableCache = true;
                        }
                    }
                }
                if (disableCache)//hpf 不写的话跳转301请求缓存会一直在浏览器中，下次登录会自动跳转到登录页面。
                {
                    filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
                    filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
                    filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                    filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    filterContext.HttpContext.Response.Cache.SetNoStore();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
