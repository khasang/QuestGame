using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using QuestGame.Domain.Entities;

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


            using (var db = new QuestGameContext())
            {
                var sc = new StageContent { ModifyDate = DateTime.Now, Text = "Описание сцены" };
                var s = new Stage { Points =50, Title = "Сцена первая", ModifyDate = DateTime.Now, StageContent = sc };

                var qc = new QuestContent { ModifyDate = DateTime.Now, Text = "Описание Квеста" };
                var q = new Quest { Rate = 3, CountComplite = 5, AddDate = DateTime.Now, ModifyDate = DateTime.Now, QuestContent = qc, Title = "Название квеста" };

                q.Stages.Add(s);

                db.Quests.Add(q);

                db.SaveChanges();
            }


        }
    }
}
