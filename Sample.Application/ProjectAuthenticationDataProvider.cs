namespace Sample.Application
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Autofac;
    using Microsoft.Practices.Unity;
    using PSP.Data;
    using PSP.Services;
    using Zcf.Data;
    using Zcf.Security;

    public class ProjectAuthenticationDataProvider : Zcf.Security.IAuthenticationDataProvider
    {
        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Zcf.Security.IUser GetUser(string name, string password)
        {
            var builder = new ContainerBuilder();
            builder.Register<IDbFactory>(c => new DbFactory(new PspContext())).InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<AuthenticationDataProvider>().As<IAuthenticationDataProvider>();
            builder.RegisterType<UserRepository>().As<PSP.Data.IUserRepository>();
            builder.RegisterType<RoleRepository>().As<PSP.Data.IRoleRepository>();

            var container = builder.Build();
            return container.Resolve<IAuthenticationDataProvider>().GetUser(name, password);

            //using (var container = DIContainer.Default.CreateChildContainer())
            //{
            //    return container.Resolve<Zcf.Security.IAuthenticationDataProvider>().GetUser(name, password);
            //}
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Zcf.Security.IUser GetUser(string name)
        {
            var builder = new ContainerBuilder();
            builder.Register<IDbFactory>(c => new DbFactory(new PspContext())).InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<AuthenticationDataProvider>().As<IAuthenticationDataProvider>();
            builder.RegisterType<UserRepository>().As<PSP.Data.IUserRepository>();
            builder.RegisterType<RoleRepository>().As<PSP.Data.IRoleRepository>();

            var container = builder.Build();
            return container.Resolve<IAuthenticationDataProvider>().GetUser(name);
            //using (var container = DIContainer.Default.CreateChildContainer())
            //{
            //    return container.Resolve<Zcf.Security.IAuthenticationDataProvider>().GetUser(name);
            //}
        }

        /// <summary>
        /// Gets the user by name or email.
        /// </summary>
        /// <param name="nameOrEmail">The name or email.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Zcf.Security.IUser GetUserByNameOrEmail(string nameOrEmail, string password)
        {
            using (var container = DIContainer.Default.CreateChildContainer())
            {
                return container.Resolve<Zcf.Security.IAuthenticationDataProvider>().GetUser(nameOrEmail, password);
            }
        }

        /// <summary>
        /// Updates the user last activity time.
        /// </summary>
        /// <param name="name">The name.</param>
        public void UpdateUserLastActivityTime(string name)
        {
            string fName = name;
        }

        /// <summary>
        /// Updates the user last login time.
        /// </summary>
        /// <param name="name">The name.</param>
        public void UpdateUserLastLoginTime(string name)
        {
            string fName = name;
        }

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        //public void UpdateUser(Zcf.Security.IUser user)
        //{
        //    using (var container = DIContainer.Default.CreateChildContainer())
        //    {
        //        container.Resolve<Zcf.Security.IAuthenticationDataProvider>().UpdateUser(user);
        //    }
        //}
    }
}
