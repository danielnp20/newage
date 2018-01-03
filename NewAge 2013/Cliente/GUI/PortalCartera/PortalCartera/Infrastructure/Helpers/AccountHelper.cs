using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using NewAge.Cliente.GUI.PortalCartera.Infrastructure.Controller;

namespace NewAge.Cliente.GUI.PortalCartera.Infrastructure.Helpers
{
    /// <summary>
    /// Class AccountHelper:
    /// Extensions to expose account information
    /// </summary>
    public static class AccountHelper
    {
        /// <summary>
        /// Gets the user mail
        /// </summary>
        /// <param name="helper">Current helper</param>
        /// <returns>Returns the user mail</returns>
        public static string UserMail(this HtmlHelper helper)
        {
            var baseController = helper.ViewContext.Controller as BaseController;
            return baseController != null ? baseController.Persistance.UserMail : "";
        }

        /// <summary>
        /// Gets the user first name
        /// </summary>
        /// <param name="helper">Current helper</param>
        /// <returns>Returns the user first name</returns>
        public static string UserName(this HtmlHelper helper)
        {
            var baseController = helper.ViewContext.Controller as BaseController;
            return baseController != null ? baseController.Persistance.UserName : "";
        }


    }
}