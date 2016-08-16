using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using QuestGame.Domain;

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



            using (var db = new QuestGame.Domain.Entities.QuestGameContext())
            {
 
                var cont1 = new QuestContent { Image = null, ModifyDate = DateTime.Now, Text = "Некоторое описание квеста", Video = null };
                var quest = new Quest
                {
                    Active = true,
                    AddDate = DateTime.Now,
                    CountComplite = 2,
                    ModifyDate = DateTime.Now,
                    Rate = 5,
                    Title = "Первый квест",
                    QuestContent = cont1
                };

                var cont0 = new StageContent { Image = null, ModifyDate = DateTime.Now, Text = "Описание Сцены", Video = null };
                var stage = new Stage { AllowSkip = false, ModifyDate = DateTime.Now, Points = 50, Title = "Первый слайд квеста", StageContent = cont0, Owner = quest };


                quest.Stages.Add(stage);

                db.SaveChanges();
            }
        }
    }
}
