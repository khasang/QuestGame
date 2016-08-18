using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using QuestGame.Domain.Entities;
using System.Diagnostics;

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


            //using (var db = new QuestGameContext())
            //{
            //    var q = new Quest() { Title = "Первый Квест", CountComplite = 5, Rate = 2, AddDate = DateTime.Now, ModifyDate = DateTime.Now };
            //    var c = new QuestContent() { Image = null, Text = "Описание квеста", Video = null, ModifyDate = DateTime.Now };
            //    q.QuestContent = c;

            //    var s = new Stage() { Points = 50, Title = "Сцена 1", ModifyDate = DateTime.Now };
            //    var c1 = new StageContent() { Image = null, Text = "Описание Сцены", Video = null, ModifyDate = DateTime.Now };

            //    s.StageContent = c1;

            //    q.Stages.Add(s);

            //    db.Quests.Add(q);

            //    var result = db.SaveChanges();
            //}


        }
    }
}
