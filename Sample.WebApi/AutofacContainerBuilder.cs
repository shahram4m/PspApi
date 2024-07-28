using Autofac;
using PSP.Data;
using Zcf.Data;
using IContainer = Autofac.IContainer;
using System;
using System.Reflection;
using Zcf.Services;
using PSP.Services;
using Zcf.Security;
using Zcf.ExportImport;
using PSP.Security;
using PSP.Services.CommandsDefinitions;
using PSP.Entities;
using log4net;
using Autofac.Integration.Mvc;
using System.Web.Mvc;

namespace Sample.WebApi
{
    /// <summary>
    /// Class to build Autofac IOC container.
    /// </summary>
    public static class AutofacContainerBuilder
    {
        /// <summary>
        /// Configures and builds Autofac IOC container.
        /// </summary>
        /// <returns></returns>
        public static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            // register types
            builder.Register<IDbFactory>(c => new DbFactory(new PSP.Data.PspContext())).InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<AuthenticationDataProvider>().As<IAuthenticationDataProvider>();
            builder.RegisterType<OrganizationEntityFilter>().As<IOrganizationEntityFilter>();
            builder.RegisterType<TerminalStatusChangeHandler>().As<ITerminalStatusChangeHandler>();
            builder.RegisterType<TerminalCommandManager>().As<ITerminalCommandManager>();
            builder.RegisterType<DefineInSwitchLogRepository>().As<IDefineInSwitchLogRepository>();
            builder.RegisterType<ChangeBankAccountLogRepository>().As<IChangeBankAccountLogRepository>();
            builder.RegisterType<TerminalRepository>().As<ITerminalRepository>();
            builder.RegisterType<MerchantRepository>().As<IMerchantRepository>();
            builder.RegisterType<CommandFactory>().As<ICommandFactory>();
            builder.RegisterGeneric(typeof(ExcelExporter<>)).As(typeof(IExporter<>));
            builder.Register(c => LogManager.GetLogger(typeof(Object))).As<ILog>();
            builder.RegisterType<AccountSecurity>().As<IAccountSecurity>();

            var data = Assembly.Load("PSP.Data");
            var services = Assembly.Load("PSP.Services");

            builder.RegisterAssemblyTypes(data)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(services)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(services)
                .AsClosedTypesOf(typeof(IRepository<>));

            builder.RegisterAssemblyTypes(services)
                .AsClosedTypesOf(typeof(IBaseService<>));

            builder.RegisterAssemblyTypes(services)
                .AsClosedTypesOf(typeof(ICrudService<>));

            builder.RegisterAssemblyTypes(services)
                .AsClosedTypesOf(typeof(ICudService<>));

            builder.RegisterAssemblyTypes(services)
                .AsClosedTypesOf(typeof(IQueryableRepository<>));

            builder.RegisterAssemblyTypes(services)
                .AsClosedTypesOf(typeof(IExportableService<>));

            return builder.Build();


            //AutofacHostFactory.Container = builder.Build();
            // build container
            //builder.RegisterFilterProvider();
            //IContainer container = builder.Build();
            //DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}