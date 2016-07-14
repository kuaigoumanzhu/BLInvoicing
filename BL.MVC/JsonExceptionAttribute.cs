using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace BL.MVC
{
    /// <summary>
    /// hpf 加入action级ajax请求发生500内部错误时返回给浏览器json提示
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class JsonExceptionAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                //返回异常json
                filterContext.Result = new JsonResult
                {
                    Data = new UiResponse { statusCode = "300", message = filterContext.Exception.Message }
                };
            }
        }
    }
}
