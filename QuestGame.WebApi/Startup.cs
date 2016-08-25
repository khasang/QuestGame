using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(QuestGame.WebApi.Startup))]

namespace QuestGame.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            //app.UseErrorPage(); // See Microsoft.Owin.Diagnostics
            //app.UseWelcomePage("/Welcome"); // See Microsoft.Owin.Diagnostics 
            //app.Run(async context =>
            //{
            //    await context.Response.WriteAsync("Hello world using OWIN TestServer");
            //});
        }
    }
}
