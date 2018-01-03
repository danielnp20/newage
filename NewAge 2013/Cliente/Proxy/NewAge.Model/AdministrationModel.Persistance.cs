using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Windows.Forms;
using NewAge.DTO.Negocio;

namespace NewAge.Cliente.Proxy.Model
{
    /// <summary>
    /// Extiendo la clase AdministrationModel
    /// </summary>
    public partial class AdministrationModel
    {
        /// <summary>
        /// Implementa persistencia para la clase AdministrationModel
        /// </summary>
        public interface IAdministrationModelPersistance : IPersistance
        {
            /// <summary>
            /// Saca el usuario actual
            /// </summary>
            void LogOut();

            /// <summary>
            /// Autentica un usuario
            /// </summary>
            /// <param name="userId">User identifier</param>
            /// <param name="rememberMe">True to remember the user, false otherwise</param>
            void LogOn(string userMail, bool rememberMe);

            /// <summary>
            /// Usuario
            /// </summary>
            DTO_seUsuario User { get; set; }

            /// <summary>
            /// Lista de datos de la tabla de control
            /// </summary>
            List<DTO_glControl> ControlList { get; set; }

            /// <summary>
            /// Empresa en la que se esta trabajando
            /// </summary>
            DTO_glEmpresa Empresa { get; set; }

            /// <summary>
            /// Pais en el que se esta trabajando
            /// </summary>
            DTO_glPais Pais { get; set; }

            /// <summary>
            /// Grupo de empresas por defecto
            /// </summary>
            string EmpresaGrupoGeneral { get; set; }

            /// <summary>
            /// Seguridades del usuario
            /// </summary>
            Dictionary<string, string> FormsSecurity { get; set; }

            /// <summary>
            /// Diccionario con lista de configuraciones
            /// </summary>
            Dictionary<string, string> Config { get; set; }

            /// <summary>
            /// Diccionario con lista de modulos
            /// </summary>
            Dictionary<string, string> Modules { get; set; }

            /// <summary>
            /// Diccionario con lista de idiomas
            /// </summary>
            Dictionary<string, Dictionary<string, string>> Languages { get; set; }

            /// <summary>
            /// Diccionario con lista de menus por modulo
            /// </summary>
            Dictionary<string, MainMenu> Menus { get; set; }

            /// <summary>
            /// Diccionario con lista de tablas
            /// </summary>
            Dictionary<Tuple<int, string>, DTO_glTabla> Tables { get; set; }

            /// <summary>
            /// Diccionario con lista de maestras
            /// </summary>
            Dictionary<int, DTO_aplMaestraPropiedades> MasterProperties { get; set; }

            /// <summary>
            /// Objeto que contiene los ultimos datos copiados
            /// </summary>
            object DataCopied { get; set; }

            /// <summary>
            /// Indica si la empresa sobre la que se trabaja maneja multimoneda
            /// </summary>
            bool MultiMoneda { get; set; }
        }

        /// <summary>
        /// Implementa persistencia Web para la clase AdministrationModel
        /// </summary>
        internal class WebAdministrationModelPersistance : IAdministrationModelPersistance
        {
            /// <summary>
            /// Redirecciona si la session ha expirado
            /// </summary>
            /// <param name="sessionId">Identificador de session</param>
            private void RedirectIfSessionExpired(string sessionId)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated && HttpContext.Current.Session[sessionId] == null)
                {
                    HttpContext.Current.Session.RemoveAll();
                    HttpContext.Current.Session.Abandon();
                    FormsAuthentication.SignOut();
                    HttpContext.Current.Response.Redirect(FormsAuthentication.LoginUrl);
                }
            }

            /// <summary>
            /// Obtiene la ubicación del error 
            /// </summary>
            /// <returns>Retorna la ubicación del error</returns>
            public string GetErrorLocation()
            {
                return HttpContext.Current.Request.Url.ToString();
            }
            
            /// <summary>
            /// Saca el usuario actual
            /// </summary>
            public void LogOut()
            {
                HttpContext.Current.Session.RemoveAll();
                HttpContext.Current.Session.Abandon();
                FormsAuthentication.SignOut();
                //HttpContext.Current.Response.Redirect(FormsAuthentication.LoginUrl);
            }

            /// <summary>
            /// Autentica un usuario
            /// </summary>
            /// <param name="userId">User Identifiar</param>
            /// <param name="rememberMe">True to remember the user, false otherwise</param>
            public void LogOn(string userMail, bool rememberMe)
            {
                //Timeout
                Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                AuthenticationSection authentication = (AuthenticationSection)config.GetSection("system.web/authentication");
                double timeout = authentication.Forms.Timeout.TotalMinutes;

                //Login
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userMail, DateTime.Now, DateTime.Now.AddMinutes(timeout), rememberMe, Role.User.ToString());
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
                if (rememberMe)
                {
                    cookie.Expires = ticket.Expiration;
                }
                cookie.Path = FormsAuthentication.FormsCookiePath;
                HttpContext.Current.Response.Cookies.Add(cookie);
            }

            /// <summary>
            /// Código del usuario
            /// </summary>
            public DTO_seUsuario User
            {
                get
                {
                    return HttpContext.Current.Session["User"] == null ? new DTO_seUsuario() : (DTO_seUsuario)HttpContext.Current.Session["User"];
                }
                set
                {
                    HttpContext.Current.Session["User"] = value;
                }
            }

            /// <summary>
            /// Lista de datos de la tabla de control
            /// </summary>
            public List<DTO_glControl> ControlList
            {
                get
                {
                    return HttpContext.Current.Session["ControlList"] == null ? new List<DTO_glControl>() : (List<DTO_glControl>)HttpContext.Current.Session["ControlList"];
                }
                set
                {
                    HttpContext.Current.Session["ControlList"] = value;
                }
            }

            /// <summary>
            /// Empresa en la que se esta trabajando
            /// </summary>
            public DTO_glEmpresa Empresa
            {
                get
                {
                    return HttpContext.Current.Session["Empresa"] == null ? new DTO_glEmpresa() : (DTO_glEmpresa)HttpContext.Current.Session["Empresa"];
                }
                set
                {
                    HttpContext.Current.Session["Empresa"] = value;
                }
            }

            /// <summary>
            /// Pais en el que se esta trabajando
            /// </summary>
            public DTO_glPais Pais
            {
                get
                {
                    return HttpContext.Current.Session["Pais"] == null ? new DTO_glPais() : (DTO_glPais)HttpContext.Current.Session["Pais"];
                }
                set
                {
                    HttpContext.Current.Session["Pais"] = value;
                }
            }

            /// <summary>
            /// Grupo de empresas por defecto
            /// </summary>
            public string EmpresaGrupoGeneral
            {
                get
                {
                    return HttpContext.Current.Session["EmpresaGrupoGeneral"] == null ? string.Empty : HttpContext.Current.Session["EmpresaGrupoGeneral"].ToString();
                }
                set
                {
                    HttpContext.Current.Session["EmpresaGrupoGeneral"] = value;
                }
            }

            /// <summary>
            /// Seguridades del usuario
            /// </summary>
            public Dictionary<string, string> FormsSecurity
            {
                get
                {
                    return HttpContext.Current.Session["FormsSecurity"] == null ? new Dictionary<string, string>() : (Dictionary<string, string>)HttpContext.Current.Session["FormsSecurity"];
                }
                set
                {
                    HttpContext.Current.Session["FormsSecurity"] = value;
                }
            }

            /// <summary>
            /// Diccionario con lista de configuraciones
            /// </summary>
            public Dictionary<string, string> Config
            {
                get
                {
                    return HttpContext.Current.Session["Config"] == null ? new Dictionary<string, string>() : (Dictionary<string, string>)HttpContext.Current.Session["Config"];
                }
                set
                {
                    HttpContext.Current.Session["Config"] = value;
                }
            }

            /// <summary>
            /// Diccionario con lista de modulos
            /// </summary>
            public Dictionary<string, string> Modules
            {
                get
                {
                    return HttpContext.Current.Session["Modules"] == null ? new Dictionary<string, string>() : (Dictionary<string, string>)HttpContext.Current.Session["Modules"];
                }
                set
                {
                    HttpContext.Current.Session["Modules"] = value;
                }
            }

            /// <summary>
            /// Diccionario con lista de idiomas
            /// </summary>
            public Dictionary<string, Dictionary<string, string>> Languages
            {
                get
                {
                    return HttpContext.Current.Session["Languages"] == null ? new Dictionary<string, Dictionary<string, string>>() : (Dictionary<string, Dictionary<string, string>>)HttpContext.Current.Session["Languages"];
                }
                set
                {
                    HttpContext.Current.Session["Languages"] = value;
                }
            }

            /// <summary>
            /// Diccionario con lista de modulos
            /// </summary>
            public Dictionary<string, MainMenu> Menus
            {
                get
                {
                    return HttpContext.Current.Session["Menus"] == null ? new Dictionary<string, MainMenu>() : (Dictionary<string, MainMenu>)HttpContext.Current.Session["Menus"];
                }
                set
                {
                    HttpContext.Current.Session["Menus"] = value;
                }
            }

            /// <summary>
            /// Diccionario con lista de tablas
            /// </summary>
            public Dictionary<Tuple<int, string>, DTO_glTabla> Tables
            {
                get
                {
                    return HttpContext.Current.Session["Tables"] == null ? new Dictionary<Tuple<int, string>, DTO_glTabla>() : (Dictionary<Tuple<int, string>, DTO_glTabla>)HttpContext.Current.Session["Tables"];
                }
                set
                {
                    HttpContext.Current.Session["Tables"] = value;
                }
            }

            /// <summary>
            /// Diccionario con lista de maestras
            /// </summary>
            public Dictionary<int, DTO_aplMaestraPropiedades> MasterProperties
            {
                get
                {
                    return HttpContext.Current.Session["MasterProperties"] == null ? new Dictionary<int, DTO_aplMaestraPropiedades>() : (Dictionary<int, DTO_aplMaestraPropiedades>)HttpContext.Current.Session["MasterProperties"];
                }
                set
                {
                    HttpContext.Current.Session["MasterProperties"] = value;
                }
            }

            /// <summary>
            /// Objeto que contiene los ultimos datos copiados
            /// </summary>
            public object DataCopied
            {
                get
                {
                    return HttpContext.Current.Session["DataCopied"];
                }
                set
                {
                    HttpContext.Current.Session["DataCopied"] = value;
                }
            }

            /// <summary>
            /// Indica si la empresa sobre la que se trabaja maneja multimoneda
            /// </summary>
            public bool MultiMoneda
            {
                get
                {
                    return HttpContext.Current.Session["MasterProperties"] == null ? false : (bool)HttpContext.Current.Session["MasterProperties"];
                }
                set
                {
                    HttpContext.Current.Session["MultiMoneda"] = value;
                }
            }

        }

        /// <summary>
        /// Implementa persistencia Windows para la clase AdministrationModel
        /// </summary>
        internal class WindowsAdministrationModelPersistance : IAdministrationModelPersistance
        {
            /// <summary>
            /// Obtiene la ubicación del error 
            /// </summary>
            /// <returns>Retorna la ubicación del error</returns>
            public string GetErrorLocation()
            {
                throw new ApplicationException("Sin implementación");
            }

            /// <summary>
            /// Saca el usuario actual
            /// </summary>
            public void LogOut()
            {
                this.FormsSecurity = new Dictionary<string, string>();
                this.Config = new Dictionary<string, string>();
                this.Menus = new Dictionary<string, MainMenu>();
                this.Modules = new Dictionary<string, string>();

                this.Languages = new Dictionary<string, Dictionary<string, string>>();
            }

            /// <summary>
            /// Autentica un usuario
            /// </summary>
            /// <param name="userId">User identifier</param>
            /// <param name="rememberMe">True to remember the user, false otherwise</param>
            public void LogOn(string userId, bool rememberMe)
            {
                
            }

            /// <summary>
            /// Código del usuario
            /// </summary>
            public DTO_seUsuario User { get; set; }

            /// <summary>
            /// Lista de datos de la tabla de control
            /// </summary>
            public List<DTO_glControl> ControlList { get; set; }

            /// <summary>
            /// Empresa en la que se esta trabajando
            /// </summary>
            public DTO_glEmpresa Empresa { get; set; }

            /// <summary>
            /// Pais en el que se esta trabajando
            /// </summary>
            public DTO_glPais Pais { get; set; }

            /// <summary>
            /// Grupo de empresas por defecto
            /// </summary>
            public string EmpresaGrupoGeneral { get; set; }

            /// <summary>
            /// Seguridades del usuario
            /// </summary>
            public Dictionary<string, string> FormsSecurity { get; set; }

            /// <summary>
            /// Diccionario con lista de configuraciones
            /// </summary>
            public Dictionary<string, string> Config { get; set; }

            /// <summary>
            /// Diccionario con lista de modulos
            /// </summary>
            public Dictionary<string, string> Modules { get; set; }

            /// <summary>
            /// Diccionario con lista de idiomas
            /// </summary>
            public Dictionary<string, Dictionary<string, string>> Languages { get; set; }

            /// <summary>
            /// Diccionario con lista de menus por modulo
            /// </summary>
            public Dictionary<string, MainMenu> Menus { get; set; }

            /// <summary>
            /// Diccionario con lista de tablas
            /// </summary>
            public Dictionary<Tuple<int, string>, DTO_glTabla> Tables { get; set; }

            /// <summary>
            /// Diccionario con lista de maestras
            /// </summary>
            public Dictionary<int, DTO_aplMaestraPropiedades> MasterProperties { get; set; }

            /// <summary>
            /// Objeto que contiene los ultimos datos copiados
            /// </summary>
            public object DataCopied { get; set; }

            /// <summary>
            /// Indica si la empresa sobre la que se trabaja maneja multimoneda
            /// </summary>
            public bool MultiMoneda { get; set; }

        }
    }
}
