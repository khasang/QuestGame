using Ninject;
using Ninject.Modules;
using QuestGame.WebApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace QuestGame.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            NinjectModule registrations = new NinjectRegistrations();
            var kernel = new StandardKernel(registrations);
            var ninjectResolver = new NinjectDependencyResolver(kernel);

            DependencyResolver.SetResolver(ninjectResolver); // MVC
            GlobalConfiguration.Configuration.DependencyResolver = ninjectResolver; // Web API
        }

        //protected void Application_Error(Object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //ловим последнее возникшее исключение
        //        Exception lastError = Server.GetLastError();

        //        if (lastError != null)
        //        {
        //            //Записываем непосредственно исключение, вызвавшее данное, в
        //            //Session для дальнейшего использования
        //            Session["ErrorException"] = lastError.InnerException;
        //        }

        //        // Обнуление ошибки на сервере
        //        Server.ClearError();

        //        // Перенаправление на свою страницу отображения ошибки
        //        Response.Redirect(Request.UrlReferrer.PathAndQuery);                
        //    }
        //    catch (Exception)
        //    {
        //        // если мы всёже приходим сюда - значит обработка исключения 
        //        // сама сгенерировала исключение, мы ничего не делаем, чтобы
        //        // не создать бесконечный цикл
        //        Response.Write("К сожалению произошла критическая ошибка. Нажмите кнопку 'Назад' в браузере и попробуйте ещё раз. ");
        //    }
        //}
    }
}
