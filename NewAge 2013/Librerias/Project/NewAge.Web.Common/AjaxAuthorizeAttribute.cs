using System;
using System.Web.Mvc;

namespace NewAge.Web.Common
{
    /// <summary>
    /// Class AjaxAuthorizeAttribute:
    /// Manage the authorization attribute operations over an Ajax reques
    /// </summary>
    public class AjaxAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Overrides the on authorization method from the authorization attribute
        /// </summary>
        /// <param name="filterContext">Filter context</param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            // Only do something if we are about to give a HttpUnauthorizedResult and we are in AJAX mode.
            if (!(filterContext.Result is HttpUnauthorizedResult) || !filterContext.HttpContext.Request.IsAjaxRequest())
                return;

            filterContext.HttpContext.Response.StatusCode = 401;
            //filterContext.Result = null;
            filterContext.HttpContext.Response.End();
        }
    }
}