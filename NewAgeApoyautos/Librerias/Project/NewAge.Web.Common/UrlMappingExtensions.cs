using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace NewAge.Web.Common
{
    /// <summary>
    /// Class UrlMappingExtensions:
    /// Extensions for the HtmlHelper to map URLs for different theme resources
    /// </summary>
    public static class UrlMappingExtensions
    {
        #region Top Url mappings

        /// <summary>
        /// Gets an absolute reference to the content directory
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <returns>Returns an absolute reference to the content directory</returns>
        private static string ContentRoot(HtmlHelper helper)
        {
            return VirtualPathUtility.ToAbsolute("~/Content");
        }

        /// <summary>
        /// Builds an image URL
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <param name="imageFile">The file name of the image</param>
        /// <returns>Returns an image URL</returns>
        public static string ImageUrl(this HtmlHelper helper, string imageFile)
        {
            return String.Format("{0}/{1}", VirtualPathUtility.ToAbsolute("~/Images"), imageFile);
        }

        /// <summary>
        /// Builds an css URL
        /// </summary>
        /// <param name="helper">Current helper</param>
        /// <param name="cssFile">The file name of the css</param>
        /// <returns>Returns an css URL</returns>
        public static string CssUrl(this HtmlHelper helper, string cssFile)
        {
            return String.Format("{0}/{1}", ContentRoot(helper), cssFile);
        }

        /// <summary>
        /// Builds an swf URL
        /// </summary>
        /// <param name="helper">Current helper</param>
        /// <param name="swfFile">The file name of the swf</param>
        /// <returns>Returns an swf URL</returns>
        public static string SwfUrl(this HtmlHelper helper, string swfFile)
        {
            return String.Format("{0}/{1}/{2}", ContentRoot(helper), "Swf", swfFile);
        }

        /// <summary>
        /// Builds an xml URL
        /// </summary>
        /// <param name="helper">Current helper</param>
        /// <param name="swfFile">The file name of the xml</param>
        /// <returns>Returns an xml URL</returns>
        public static string XmlUrl(this HtmlHelper helper, string swfFile)
        {
            return String.Format("{0}/{1}/{2}", ContentRoot(helper), "Xml", swfFile);
        }

        /// <summary>
        /// Builds an html URL
        /// </summary>
        /// <param name="helper">Current helper</param>
        /// <param name="htmlFile">The file name of the html</param>
        /// <returns>Returns an html URL</returns>
        public static string HtmlUrl(this HtmlHelper helper, string htmlFile)
        {
            return String.Format("{0}/{1}/{2}", ContentRoot(helper), "Html", htmlFile);
        }

        #endregion

        #region Other mappings

        /// <summary>
        /// Builds an script URL
        /// </summary>
        /// <param name="helper">Current helper</param>
        /// <param name="scriptFile">The file name of the script</param>
        /// <returns>Returns an script URL</returns>
        public static string ScriptUrl(this HtmlHelper helper, string scriptFile)
        {
            return VirtualPathUtility.ToAbsolute(String.Format("~/{0}/{1}", "Scripts", helper.Encode(scriptFile)));
        }

        /// <summary>
        /// Gets the current application base Url
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <returns>Returns the current application base Url</returns>
        public static string CurrentAppUrl(this HtmlHelper helper)
        {
            var request = helper.ViewContext.HttpContext.Request;
            var args = new[]
            {
                request.Url.Scheme,
                request.Url.Host,
                request.Url.Port == 80 ? String.Empty : ":" + request.Url.Port,
                request.ApplicationPath
            };
            string appPath = String.Format("{0}://{1}{2}{3}", args).Trim(new[] {'/'});

            return appPath;
        }

        /// <summary>
        /// Gets the current application base Url with no path
        /// </summary>
        /// <param name="helper">Helper</param>
        /// <returns>Returns the current application base Url with no path</returns>
        public static string CurrentAppUrlNoPath(this HtmlHelper helper)
        {
            var request = helper.ViewContext.HttpContext.Request;
            var args = new[]
            {
                request.Url.Scheme,
                request.Url.Host,
                request.Url.Port == 80 ? String.Empty : ":" + request.Url.Port
            };
            string appPath = String.Format("{0}://{1}{2}", args).Trim(new[] {'/'});

            return appPath;
        }

        #endregion
    }
}