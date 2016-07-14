using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using BL.Framework.Cache;
using BL.Framework;
using Autofac.Integration.Mvc;
using BL.MVC;

namespace BL.Web
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            InitialzeDIContainer();
            DeleteView();
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        /// <summary>
        /// hpf 初始化DI容器
        /// </summary>
        private void InitialzeDIContainer()
        {
            var builder = new ContainerBuilder();
            //hpf 此缓存支持独立程序（服务或单独dll脱离system.web）用以放置业务数据等内容（单例性能）
            builder.Register(c => new RuntimeMemoryCache()).As<ICache>().SingleInstance();
            //用户身份认证
            builder.Register(c => new FormsAuthenticationService()).As<IAuthenticationService>().PropertiesAutowired().InstancePerHttpRequest();
            //创建容器
            IContainer container = builder.Build();
            //将Autofac容器中的实例注册到mvc自带DI容器中（这样才获取到每请求缓存的实例）
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            DIContainer.RegisterContainer(container);
        }

        /// <summary>
        /// hpf 删除没用的视图引擎，不需要webform的
        /// </summary>
        private void DeleteView()
        {
            var viewEngines = ViewEngines.Engines;
            var webFormsEngine = viewEngines.OfType<WebFormViewEngine>().FirstOrDefault();
            if (webFormsEngine != null)
            {
                viewEngines.Remove(webFormsEngine);
            }
        }
    }
}