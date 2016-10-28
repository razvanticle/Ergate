﻿using Microsoft.Owin;

[assembly: OwinStartup(typeof(AppHost.Startup))]

namespace AppHost
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Reflection;
    using System.Web.Compilation;
    using System.Web.Http;

    using iQuarc.AppBoot;
    using iQuarc.AppBoot.Unity;
    using iQuarc.AppBoot.WebApi;
    using iQuarc.DataAccess.AppBoot;

    using Owin;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            config.MapHttpAttributeRoutes();
            app.UseWebApi(config);

            var assemblies = this.GetApplicationAssemblies().ToArray();
            Bootstrapper bootstrapper = new Bootstrapper(assemblies);
            bootstrapper.ConfigureWithUnity();
            bootstrapper.AddRegistrationBehavior(new ServiceRegistrationBehavior());
            bootstrapper.AddRegistrationBehavior(DataAccessConfigurations.DefaultRegistrationConventions);
            bootstrapper.Run();

            bootstrapper.ConfigureWebApi(config);
        }

        private IEnumerable<Assembly> GetApplicationAssemblies()
        {
            var assemblies =
                BuildManager.GetReferencedAssemblies()
                    .Cast<Assembly>()
                    .Where(
                        a =>
                        a.GetName().Name.StartsWith("AppHost") 
                        || a.GetName().Name.StartsWith("iQuarc.DataAccess")
                        || a.GetName().Name.StartsWith("DataAccess") 
                        || a.GetName().Name.StartsWith("Contracts")
                        || a.GetName().Name.StartsWith("LandingPage.") 
                        || a.GetName().Name.StartsWith("Company."))
                    .ToList();

            return assemblies;
        }
    }
}