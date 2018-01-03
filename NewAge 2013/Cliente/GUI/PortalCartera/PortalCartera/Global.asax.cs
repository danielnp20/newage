using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Converters;
using NewAge.Cliente.GUI.PortalCartera;
using NewAge.Cliente.GUI.PortalCartera.Infrastructure;
using NewAge.Negocio;
using NewAge.DTO.Negocio;

namespace NewAge.Cliente.GUI.PortalCartera
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    /// <summary>
    /// The application
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Event that fires when the application starts
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }

        /// <summary>
        /// Event that fires when an authenticate request is made
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        public void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.User != null)
            {
                var userId = HttpContext.Current.User.Identity;
                if (userId.IsAuthenticated)
                {
                    var identity = (FormsIdentity)HttpContext.Current.User.Identity;
                    var ticket = identity.Ticket;
                    HttpContext.Current.User = new GenericPrincipal(userId, new[] { ticket.UserData });
                }
            }
        }

        /// <summary>
        /// Event that fires when a session starts
        /// </summary>
        protected void Session_Start()
        {
            if (Request.IsAuthenticated && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (!String.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {
                    //Intercept current route
                    var currentContext = new HttpContextWrapper(HttpContext.Current);
                    RouteTable.Routes.GetRouteData(currentContext);

                    var persistance = new WebPersistance();
                    ModuloGlobal _proxyGlobal = new ModuloGlobal(persistance.Connection, null, null, 0, string.Empty);

                    //var user = proxy.GetUser(HttpContext.Current.User.Identity.Name);
                    DTO_seUsuario user = _proxyGlobal.seUsuario_GetUserbyID(HttpContext.Current.User.Identity.Name);

                    if (user != null)
                    {
                        persistance.UserMail = user.CorreoElectronico.Value;
                        persistance.UserName = user.Descriptivo.Value;
                        persistance.UserId = user.ID.Value;
                    }
                    else
                    {
                        Session.RemoveAll();
                        Session.Abandon();
                        FormsAuthentication.SignOut();
                    }
                }
            }
        }
    }
}