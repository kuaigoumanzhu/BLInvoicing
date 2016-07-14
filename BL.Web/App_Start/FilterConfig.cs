using BL.MVC;
using System.Web;
using System.Web.Mvc;

namespace BL.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //hpf 注册全局异常日志
            filters.Add(new LogExceptionAttribute());
            //hpf 去掉web.config中匿名访问权限，这里禁止匿名用户访问，开发登录，注册，从其他已赋权限系统用户跳转
            filters.Add(new AuthenticationAttribute());
        }
    }
}