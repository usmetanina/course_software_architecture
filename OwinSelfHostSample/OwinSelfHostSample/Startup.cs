﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Owin;
using System.Web.Http;

namespace OwinSelfHostSample
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            appBuilder.UseWebApi(config);
        }
    }
}