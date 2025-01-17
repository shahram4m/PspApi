﻿using Autofac;
//using Autofac.Integration.Wcf;
using Sample.Core.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Http.WebHost;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace Sample.WebApi
{
    public class WebApiApplication : SampleWebApplication
    {
        protected void Application_Start()
        {
            base.Application_Start(GlobalConfiguration.Configuration, SampleWebApplicationConfiguration.DefaultConfiguration());
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }
   }
}
