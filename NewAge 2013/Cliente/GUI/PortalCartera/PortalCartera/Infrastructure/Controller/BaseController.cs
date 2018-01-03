using NewAge.ADO;
using NewAge.Cliente.Proxy.Model;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Mail;
using NewAge.Librerias.Project;
using NewAge.Negocio;
using NewAge.Web.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.Security;

namespace NewAge.Cliente.GUI.PortalCartera.Infrastructure.Controller
{
    public class BaseController : System.Web.Mvc.Controller
    {
        /// <summary>
        /// Web persistance
        /// </summary>
        private readonly WebPersistance _persistance;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Web.Mvc.Controller"/> class.
        /// </summary>
        public BaseController()
        {
            _persistance = new WebPersistance();
            if(_persistance.Empresa == null)
            {
                var empresaId = ConfigurationManager.AppSettings["NewAge.Default.Company"];
                var basicDTO = this.GetMasterDTO(AppMasters.glEmpresa, false, empresaId, true, null);

                DTO_glEmpresa empresa = (DTO_glEmpresa)basicDTO;
                _persistance.Empresa = empresa;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public WebPersistance Persistance
        {
            get { return _persistance; }
        }

        #region Proxies

        public ModuloGlobal GlobalProxy
        {
            get
            {
                return new ModuloGlobal(ConnectionsManager.ADO_ConnectDB(), null, _persistance.Empresa, 0, string.Empty);    
            }
        }

        public ModuloCartera CarteraProxy
        {
            get
            {
                return new ModuloCartera(ConnectionsManager.ADO_ConnectDB(), null, _persistance.Empresa, 0, string.Empty);
            }
        }


        #endregion

        #region MVC Operations

        /// <summary>
        /// Renders a partial view and return its rendered html as a string
        /// </summary>
        /// <param name="controller">Controller</param>
        /// <param name="viewName">View name</param>
        /// <param name="model">Model</param>
        /// <returns>Returns a string with the html from redered view</returns>
        protected string RenderPartialViewToString(System.Web.Mvc.Controller controller, string viewName, object model)
        {
            controller.ViewData.Model = model;
            try
            {
                using (var sw = new StringWriter())
                {
                    var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                    var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                    viewResult.View.Render(viewContext, sw);
                    viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);

                    return sw.GetStringBuilder().ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        /// <summary>
        /// Authenticates the current user
        /// </summary>
        protected bool AuthenticateUser(DTO_coTercero client, bool rememberMe)
        {
            if (client != null)
            {
                Persistance.UserMail = client.CECorporativo.Value;
                Persistance.UserName = client.Descriptivo.Value;
                Persistance.UserId = client.ID.Value;

                var cookie = FormsAuthentication.GetAuthCookie(client.CECorporativo.Value, rememberMe);
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                if (rememberMe)
                {
                    cookie.Expires = DateTime.Now.AddDays(Convert.ToInt32(ConfigurationManager.AppSettings["Authentication.Cookie.Remember.Days"]));
                }

                //TODO: Store real roles
                var roles = new string[] { };
                if (ticket != null)
                {
                    ticket = new FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration, ticket.IsPersistent, String.Join(",", roles), FormsAuthentication.FormsCookiePath);
                    if (FormsAuthentication.SlidingExpiration)
                    {
                        ticket = FormsAuthentication.RenewTicketIfOld(ticket);
                    }
                    cookie.Value = FormsAuthentication.Encrypt(ticket);

                    System.Web.HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new FormsIdentity(ticket), roles);
                    Response.Cookies.Add(cookie);

                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Mails

        /// <summary>
        /// Envia un mensaje segun la configuracion del sistema
        /// Mira si se envia desde el outlook del usuario o desde la cuenta de AMP
        /// </summary>
        /// <param name="subject">Asunto</param>
        /// <param name="body">Cuerpo</param>
        /// <param name="recipients">Destinatarios</param>
        internal bool SendMail(List<string> confirmationMails, MailMessageType type, object model)
        {
            try
            {
                var mailer = new Mailer();
                foreach (var mail in confirmationMails)
                {
                    var to = new List<MailAddress> { new MailAddress(mail) };
                    mailer.SendMessage(type, model, to);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        #endregion

        #region Maestras

        /// <summary>
        /// Trae un DTO de una maestra simple o jerarquica
        /// </summary>
        /// <param name="tipo">Tipo de maestra</param>
        /// <param name="docId">Id del documento</param>
        /// <param name="isValueInt">Identifica si el id es entero</param>
        /// <param name="value">Valor del DTO</param>
        /// <returns>Retorna el DTO requerido</returns>
        internal Object GetMasterDTO(int docId, bool isValueInt, string value, bool active, List<DTO_glConsultaFiltro> filtros = null)
        {
            var masterSimpleDAL = new DAL_MasterSimple(ConnectionsManager.ADO_ConnectDB(), null, _persistance.Empresa, 0, string.Empty);
            masterSimpleDAL.DocumentID = docId;

            UDT_BasicID udt = new UDT_BasicID() { Value = value, IsInt = isValueInt };
            DTO_MasterBasic obj = masterSimpleDAL.DAL_MasterSimple_GetByID(udt, true, filtros);

            if (active && obj != null && !obj.ActivoInd.Value.Value)
            {
                obj = null;
            }

            return obj;
        }

        #endregion

    }
}