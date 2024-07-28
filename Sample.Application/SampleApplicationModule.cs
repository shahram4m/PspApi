using Autofac;
using Sample.Application.Services;
using Sample.Core.Data.Database;
using Sample.Core.Expressions;
using Sample.Core.Modules;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Text;
using System.Configuration;
using System.Text;
using Zcf.Security;
using Autofac;
using Zcf.Security;
using Zcf.Data;
using PSP.Services;
using Zcf.ExportImport;
using PSP.Data;
using PSP.Services.CommandsDefinitions;
using log4net;
using PSP.Security;
using Zcf.Services;
using Microsoft.Practices.Unity;

namespace Sample.Application
{
    public class SampleApplicationModule : ILamaModule
    {
        public void Initialize(ContainerBuilder builder)
        {
            Authentication.Default.DataProvider = DIContainer.Default.Resolve<ProjectAuthenticationDataProvider>();

            // register types
            builder.Register<IDbFactory>(c => new DbFactory(new PSP.Data.PspContext())).InstancePerLifetimeScope();
            builder.RegisterType<Zcf.Data.UnitOfWork>().As<Zcf.Data.IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<AuthenticationDataProvider>().As<IAuthenticationDataProvider>();
            builder.RegisterGeneric(typeof(ExcelExporter<>)).As(typeof(IExporter<>));
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
                .AsClosedTypesOf(typeof(Zcf.Data.IRepository<>));

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

            builder
                .RegisterType<Sample.Core.Data.Database.DataContext>()
                .As<IDataContext>()
                .As<IDataContextAsync>()
                .WithParameter("nameOrConnectionString", "DbContext")
                .InstancePerLifetimeScope();

            RegisterServices(builder);
        }

        public void PostInitialize(ILifetimeScope container)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Sample.Core.Data.Database.DataContext, Sample.Application.Migrations.Configuration>(true));
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<PspContext, PSP.Data.Migrations.Configuration>());
        }

        private void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<BaseService>().PropertiesAutowired().InstancePerLifetimeScope();

            var baseServiceType = typeof(BaseService);
            var applicationServices = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(type => ExpressionsHelper.HasBaseType(type, baseServiceType));

            foreach (var applicationService in applicationServices)
            {
                builder
                    .RegisterType(applicationService)
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                    .InstancePerLifetimeScope();
            }
        }
    }
}
