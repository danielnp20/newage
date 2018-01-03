using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Clases;
using System.Windows.Forms;
using NewAge.Librerias.Project;
using NewAge.DTO.Resultados;
using NewAge.DTO.UDT;
using System.IO;
using System.Threading;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    class seUsuario : MasterSimpleForm
    {
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        
        ///<summary>
        /// Constructor 
        /// </summary>
        public seUsuario() : base() { }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.seUsuario;
            base.InitForm();
        }

        #region Funciones virtuales(master simple)

        /// <summary>
        /// Metodo encargado de encapsular la insercion de registros de maestras simples
        /// </summary>
        /// <param name="tableName">Nombre de la tabla</param>
        /// <param name="insertList">Lista de dtos</param>
        /// <param name="userId">usuario logueado</param>
        /// <param name="documentId">docuemnto</param>
        /// <param name="accion">accion (insercion/importar)</param>
        /// <returns></returns>
        protected override DTO_TxResult DataAdd(List<DTO_MasterBasic> insertList, int accion)
        {
            try
            {
                List<DTO_MasterBasic> users = new List<DTO_MasterBasic>();

                //Agregar el password
                foreach (DTO_MasterBasic basic in insertList)
                {
                    DTO_seUsuario usr = (DTO_seUsuario)basic;

                    string pwd = Path.GetRandomFileName();
                    pwd = pwd.Replace(".", "");

                    usr.ContrasenaLimpia = usr.ID.Value;
                    users.Add(usr);
                }

                byte[] bItems = CompressedSerializer.Compress<IEnumerable<DTO_MasterBasic>>(users);
                DTO_TxResult result = _bc.AdministrationModel.seUsuario_Add(bItems, +_bc.AdministrationModel.User.ReplicaID.Value.Value, accion);

                if (result.Result == ResultValue.OK)
                {
                    foreach (DTO_seUsuario usr in users)
                    {
                        try
                        {
                            string subject = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_NewUser_Subject);
                            string bodyFormat = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_NewUser_Body);

                            string body = string.Format(bodyFormat, usr.ID.Value, usr.ContrasenaLimpia);

                            _bc.SendMail(this.DocumentID, subject, body, usr.CorreoElectronico.Value);

                        }
                        catch (Exception e)
                        {
                            string err = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_SendMail);
                            MessageBox.Show(err);
                        }

                    }
                }

                return result;
            }
            catch (Exception ex)
            { 
                throw ex; 
            }
        }

        /// <summary>
        /// Metodo que encapsula la funcion de actualizar
        /// </summary>
        /// <param name="tableName">Nombre de la tabla</param>
        /// <param name="dto">dto</param>
        /// <param name="userId">usuario</param>
        /// <param name="documentId">documento</param>
        /// <returns></returns>
        protected override DTO_TxResult DataUpdate(DTO_MasterBasic dto)
        {
            DTO_seUsuario usr = (DTO_seUsuario)dto;
            return _bc.AdministrationModel.seUsuario_Update(usr, _bc.AdministrationModel.User.ReplicaID.Value.Value);
        }

        #endregion

        #region Eventos barra de herramientas

        /// <summary>
        /// Boton para generar reportes
        /// </summary>
        public override void TBResetPwd()
        {
            Thread process = new Thread(this.ResetPwdThread);
            process.Start();
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de importacion
        /// </summary>
        public virtual void ResetPwdThread()
        {
            try
            {
                DTO_seUsuario usr = (DTO_seUsuario)this.selectedDto;

                string msgText = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ResultPwd), usr.ID.Value);
                string msgTitle = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_ResetPwd);

                if (MessageBox.Show(msgText, msgTitle, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string subject = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_ResetPassword_Subject);
                    string bodyFormat = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_ResetPassword_Body);

                    string pwd = Path.GetRandomFileName();
                    pwd = pwd.Replace(".", "");

                    bool res = _bc.AdministrationModel.seUsuario_ResetPassword(usr.ReplicaID.Value.Value, usr.ID.Value);
                    string body = string.Format(bodyFormat, usr.ID.Value);

                    DTO_TxResult result = new DTO_TxResult();//this._bc.SendMail(this.DocumentID, subject, body, usr.CorreoElectronico.Value);
                    result.Result = ResultValue.OK;
                    MessageForm frm = new MessageForm(result);
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                string err = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_SendMail);
                MessageBox.Show(err);
            }
        }

        protected override bool FieldValidate(string Field, object Value, int RowIndex, out string msg)
        {
            bool res = true;
            msg = string.Empty;
            FieldConfiguration fc = this.GetFieldConfigByCaption(Field);
            #region Seccion Funcional

            if (fc.FieldName == "SeccionFuncionalID")
            {
                FieldConfiguration newFC2 = this.GetFieldConfigByFieldName("AreaFuncionalID");
                FieldConfiguration newFC3 = this.GetFieldConfigByFieldName("AreaFuncionalDesc");

                string In = Value.ToString();
                DTO_glSeccionFuncional seccion = (DTO_glSeccionFuncional)_bc.GetMasterDTO(AppMasters.MasterType.Simple,AppMasters.glSeccionFuncional,false,In,true);
                if (seccion != null)
                {
                    DTO_glAreaFuncional area = (DTO_glAreaFuncional)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glAreaFuncional, false, seccion.AreaFuncionalID.Value, true);
                    if (area != null)
                    {
                        this.SetEditGridValue(newFC2.RowIndex, area.ID.Value);
                        this.SetEditGridValue(newFC3.RowIndex, area.Descriptivo);
                    }
                    else
                    {
                        string msgNO = _bc.GetResource(LanguageTypes.Messages, "El area funcional de la Sección funcional no existe");
                        MessageBox.Show(msgNO);
                    } 
                }                              
            }

            #endregion
            return res;
        }


        #endregion
    } 
}
