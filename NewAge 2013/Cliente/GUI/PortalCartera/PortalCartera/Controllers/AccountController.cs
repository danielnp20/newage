using NewAge.Cliente.GUI.PortalCartera.Infrastructure.Controller;
using NewAge.DTO.MailModels;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Mail;
using NewAge.Librerias.Project;
using NewAge.Negocio;
using NewAge.Web.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NewAge.Cliente.GUI.PortalCartera.Controllers
{
    /// <summary>
    /// Class AccountController:
    /// Provides the account functionalities
    /// </summary>
    [AjaxAuthorize]
    public class AccountController : BaseController
    {
        /// <summary>
        /// Default action
        /// </summary>
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Logins an user
        /// </summary>
        /// <param name="email">Mail</param>
        /// <param name="password">Password</param>
        /// <param name="rememberMe">True to remember the user</param>
        /// <returns>Returns the login validation response</returns>
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult JsonLogin(string id, string password, bool rememberMe = false)
        {
            try
            {
                if (!string.IsNullOrEmpty(id.Trim()) && !string.IsNullOrEmpty(password.Trim()))
                {
                    var result =  this.GlobalProxy.coTercero_ValidateUserCredentials(id.Trim(), password.Trim());

                    // Contraseña incorrecta
                    if (result != UserResult.AlreadyMember)
                    {
                        return Json(new { Success = false, Response = result });
                    }
                    else 
                    {
                        DTO_MasterBasic basicDTO = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.coTercero, false, id.Trim(), true);
                        DTO_coTercero cliente = (DTO_coTercero)basicDTO;

                        this.AuthenticateUser(cliente, rememberMe);
                        var url = Url.Action("Index", "Home");
                        return Json(new { Success = true, Code = result, Url = url });
                    }
                }

                return Json(new { Success = false, Code = ResponseCode.GeneralError });
            }
            catch (Exception)
            {
                return Json(new { Success = false, Code = ResponseCode.GeneralError });
            }
        }

        /// <summary>
        /// Logins an user
        /// </summary>
        /// <param name="email">Mail</param>
        /// <param name="password">Password</param>
        /// <param name="rememberMe">True to remember the user</param>
        /// <returns>Returns the login validation response</returns>
        [AllowAnonymous]
        [HttpPost]
        public JsonResult ForgotPassword(string email, string id)
        {
            try
            {
                id = id.Trim();
                email = email.Trim();
                DTO_MasterBasic basicDTO = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.coTercero, false, id, true);
                DTO_coTercero cliente = (DTO_coTercero)basicDTO;

                if(cliente == null || cliente.CECorporativo.Value.Trim() != email)
                {
                    return Json(new { Success = false, ValidUser = false });
                }

                string pwd = Path.GetRandomFileName();
                pwd = pwd.Replace(".", "");
                bool passwordSent = this.GlobalProxy.coTercero_ResetPassword(id, pwd);
                if (passwordSent)
                {
                    List<string> mails = new List<string>();
                    mails.Add(email.Trim());

                    DTO_CorreoOnline mail = new DTO_CorreoOnline()
                    {
                        UrlHost = Request.UrlReferrer.AbsoluteUri,
                        UserName = cliente.Descriptivo.Value,
                        Password = pwd
                    };
                    passwordSent = this.SendMail(mails, MailMessageType.UpdateClientPassword, mail);
                }

                return Json(new { Success = passwordSent, Pass = passwordSent });
            }
            catch (Exception)
            {
                return Json(new { Success = false, Code = ResponseCode.GeneralError });
            }
        }

        /// <summary>
        /// Reset an user password
        /// </summary>
        /// <returns>Returns the login validation response</returns>
        [HttpPost]
        public JsonResult ResetPassword(string password)
        {
            try
            {
                bool mailSent = false;
                bool passwordSent = this.GlobalProxy.coTercero_UpdatePassword(this.Persistance.UserId, password, false);
                if(passwordSent)
                {
                    List<string> mails = new List<string>();
                    mails.Add(this.Persistance.UserMail);

                    DTO_CorreoOnline mail = new DTO_CorreoOnline()
                    {
                        UrlHost = Request.UrlReferrer.AbsoluteUri,
                        UserName = this.Persistance.UserName,
                        Password = password
                    };
                    mailSent = this.SendMail(mails, MailMessageType.UpdateClientPassword, mail);
                }

                return Json(new { Success = passwordSent, MailSent = mailSent});
            }
            catch (Exception)
            {
                return Json(new { Success = false, Code = ResponseCode.GeneralError });
            }
        }


        /// <summary>
        /// Log off the user
        /// </summary>
        /// <returns>Returns the default action</returns>
        public ActionResult LogOff()
        {
            Session.RemoveAll();
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}
