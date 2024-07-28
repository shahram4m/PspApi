using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.SessionState;

namespace Sample.WebApi
{
    public class WebApiSessionModule : IHttpModule
    {
        private static readonly string SessionStateCookieName = "ASP.NET_SessionId";

        public void Init(HttpApplication context)
        {
            context.PostAuthorizeRequest += this.OnPostAuthorizeRequest;
            context.PostRequestHandlerExecute += this.PostRequestHandlerExecute;
        }

        public void Dispose()
        {
        }

        protected virtual void OnPostAuthorizeRequest(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;

            if (this.IsWebApiRequest(context))
            {
                context.SetSessionStateBehavior(SessionStateBehavior.Required);
            }
        }

        protected virtual void PostRequestHandlerExecute(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;

            if (this.IsWebApiRequest(context))
            {
                this.AddSessionCookieToResponseIfNeeded(context);
            }
        }

        protected virtual void AddSessionCookieToResponseIfNeeded(HttpContext context)
        {
            HttpSessionState session = context.Session;

            if (session == null)
            {
                // session not available
                return;
            }

            if (!session.IsNewSession)
            {
                // it's safe to assume that the cookie was
                // received as part of the request so there is
                // no need to set it
                return;
            }

            string cookieName = GetSessionCookieName();
            HttpCookie cookie = context.Response.Cookies[cookieName];
            if (cookie == null || cookie.Value != session.SessionID)
            {
                context.Response.Cookies.Remove(cookieName);
                context.Response.Cookies.Add(new HttpCookie(cookieName, session.SessionID));
            }
        }

        protected virtual string GetSessionCookieName()
        {
            var sessionStateSection = (SessionStateSection)ConfigurationManager.GetSection("system.web/sessionState");

            return sessionStateSection != null && !string.IsNullOrWhiteSpace(sessionStateSection.CookieName) ? sessionStateSection.CookieName : SessionStateCookieName;
        }

        protected virtual bool IsWebApiRequest(HttpContext context)
        {
            string requestPath = context.Request.AppRelativeCurrentExecutionFilePath;

            if (requestPath == null)
            {
                return false;
            }

            return requestPath.StartsWith("api", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}