using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.Jwt;
using Owin;
using Sample.WebApi;
using System.Configuration;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Owin;
using Sample.WebApi;
using System.Configuration;
using System.Text;
using Zcf.Security;
using Autofac;
using Zcf.Security;
using System.Web.Http;


[assembly: OwinStartup(typeof(Sample.Web.Startup))]
namespace Sample.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = ConfigurationManager.AppSettings["JwtIssuer"], //some string, normally web url,  
                        ValidAudience = ConfigurationManager.AppSettings["JwtIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["JwtKey"]))
                    }
                });
        }
    }
}