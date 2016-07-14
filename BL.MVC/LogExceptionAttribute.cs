using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using BL.Framework;

namespace BL.MVC
{
    /// <summary>
    /// hpf 2016 07 10 加入全局异常处理500内部错误
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class LogExceptionAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                string controllerName = filterContext.RouteData.Values["controller"].ToString();
                string actionName = filterContext.RouteData.Values["action"].ToString();
                string msgTemp = BL.Framework.Utilities.WebUtility.GetIP() + "在执行controller" + controllerName + "的" + actionName + "时产生异常:" + filterContext.Exception.Message;
                //hpf此处写入异常日志
                BL.Framework.Logging.LogHelper.Fatal(msgTemp);
            }
            if (filterContext.Result is JsonResult)
            {
                filterContext.ExceptionHandled = true;//异常已处理
            }
            else
            {
                //通过base返回系统默认异常处理上向错误页面跳转
                base.OnException(filterContext);
            }
        }
    }
}
