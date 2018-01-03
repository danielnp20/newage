using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Resultados;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Controls;
using System.Threading;
using SentenceTransformer;
using NewAge.DTO.Attributes;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de documentos
    /// </summary>
    public partial class DocumentNominaBaseForm : FormWithToolbar
    {
        #region Delegados

        protected delegate void RefreshGrid();
        protected RefreshGrid refreshGridDelegate;
        /// <summary>
        /// Delegado que finaliza el proceso de importación
        /// </summary>
        protected virtual void RefreshGridMethod() { }

        protected delegate void Save();
        protected Save saveDelegate;
        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected virtual void SaveMethod() { }

        protected delegate void SendToApprove();
        protected SendToApprove sendToApproveDelegate;
        /// <summary>
        /// Delegado que actualiza el formulario despues de enviar un documento para aprobacion
        /// </summary>
        protected virtual void SendToApproveMethod() { }

        #endregion

        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        //Variables Privadas
        private FormTypes _frmType = FormTypes.Document;
        private string _frmName;

        //Variables Protegidas
        protected int userID = 0;
        //Para manejo de propiedades
        protected string empresaID = string.Empty;
        protected int documentID;
        protected ModulesPrefix frmModule;
        protected List<int> select = new List<int>();
        protected bool multiMoneda;
        //Internas del formulario
        protected string areaFuncionalID;
        protected string prefijoID;
        protected string terceroID;
        protected string unboundPrefix = "Unbound_";

     
        #endregion

        ///<summary>
        /// Constructor 
        /// </summary>)
        public DocumentNominaBaseForm()
        {
            try
            {
                //this.InitializeComponent();
                this.SetInitParameters();
                this.multiMoneda = _bc.AdministrationModel.MultiMoneda;
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());
                FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

                this.AfterInitialize();
                this.CleanFormat();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentNominaBaseForm.cs", "DocumentNominaBaseForm"));
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Loads the document main info
        /// </summary>
        private void LoadDocumentInfo(bool firstTime)
        {
            try
            {
                if (firstTime)
                {
                    //Llena el area funcional
                    this.areaFuncionalID = _bc.AdministrationModel.User.AreaFuncionalID.Value;
                    DTO_MasterBasic basicDTO = (DTO_MasterBasic)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glAreaFuncional, false, this.areaFuncionalID, true);
                    this.txtAF.Text = basicDTO.Descriptivo.Value;

                    this.prefijoID = _bc.GetPrefijo(this.areaFuncionalID, this.documentID);
                    DTO_glDocumento dtoDoc = (DTO_glDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, this.documentID.ToString(), true);

                    if (string.IsNullOrEmpty(this.prefijoID))
                    {
                        this.lblPrefix.Visible = false;
                        this.txtPrefix.Visible = false;
                    }
                    else
                    {
                        this.txtPrefix.Text = this.prefijoID;
                    }

                    this.terceroID = this._bc.GetControlValueByCompany(ModulesPrefix.co,AppControl.co_TerceroXDefecto);
                    this.txtDocumentoID.Text = this.documentID.ToString();
                    this.txtDocDesc.Text = dtoDoc.Descriptivo.Value;
                    this.txtNumeroDoc.Text = "0";

                    string periodo = _bc.GetControlValueByCompany(this.frmModule, AppControl.no_Periodo);
                    this.dtPeriod.DateTime = Convert.ToDateTime(periodo);
                    this.dtFecha.DateTime = this.dtPeriod.DateTime;
                    this.dtPeriod.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinAppDocumentNominaBaseForm.cs", "LoadDocumentInfo"));
            }
        }

        #endregion

        #region Funciones Virtuales (Abstractas)

        /// <summary>
        /// Carga la información de las grilla
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="refresh">Si debe volver a cargar la data del servidor</param>
        protected virtual void LoadData(bool firstTime) { }

        /// <summary>
        /// Trae el documento id de una maestra de acuerdo al nombre de un campo
        /// </summary>
        /// <param name="colName">Nombre del campo</param>
        /// <returns>Retorna el documento id de una maestra</returns>
        protected virtual int GetMasterDocumentID(string colName) { return 0; }

        /// <summary>
        /// Evento que obliga que se ejecute una funcion al cambiar de fila
        /// <param name="fila">Fila que se debe actualizar</param>
        /// <param name="oper">Indica si se debe ejecutar una segunda operacion</param>
        /// </summary>
        protected virtual void RowIndexChanged(int fila, bool oper) { }

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected virtual void SetInitParameters()
        {
            //Inicia las variables del formulario
            this.empresaID = _bc.AdministrationModel.Empresa.ID.Value;
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;

            this.LoadDocumentInfo(true);

            this.refreshGridDelegate = new RefreshGrid(this.RefreshGridMethod);
            this.saveDelegate = new Save(this.SaveMethod);
            this.sendToApproveDelegate = new SendToApprove(this.SendToApproveMethod);            
           
        }

        /// <summary>
        /// Limpia el formato de importacion segun algun documento
        /// </summary>
        protected virtual void CleanFormat() { }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected virtual void AfterInitialize() {   }

        /// <summary>
        /// Habilita o deshabilita la barra de herramientas segun donde el usuario este
        /// </summary>
        protected virtual void ValidHeaderTB() { }

        /// <summary>
        /// Carga la informacion temporal del documento
        /// </summary>
        /// <param name="aux">Informacion del temporal</param>
        protected virtual void LoadTempData(object aux) { }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected virtual void AddGridCols()
        {
            //Campo de marca
            GridColumn marca = new GridColumn();
            marca.FieldName = this.unboundPrefix + "Marca";
            marca.UnboundType = UnboundColumnType.Boolean;
            marca.VisibleIndex = 0;
            marca.Width = 20;
            marca.Visible = true;
            marca.Fixed = FixedStyle.Left;
            marca.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            marca.OptionsColumn.ShowCaption = false;
        }

        /// <summary>
        /// Extrae datos de la grilla para las cajas de texto
        /// </summary>
        /// <param name="isNew">Identifica si es un nuevo registro</param>
        /// <param name="rowIndex">Numero de la fila</param>
        protected virtual void LoadEditGridData(bool isNew, int rowIndex) { }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        protected virtual void AddNewRow() { }

        /// <summary>
        /// Valida una fila
        /// </summary>
        /// <param name="fila">Indice de la fila a validar</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        protected virtual bool ValidateRow(int fila)
        {
            return true;
        }

              /// <summary>
        /// Revisa si el documento actual tiene temporales
        /// </summary>
        /// <returns></returns>
        protected virtual bool HasTemporales()
        {
            return _bc.AdministrationModel.aplTemporales_HasTemp(this.documentID.ToString(), _bc.AdministrationModel.User);
        }

        /// <summary>
        /// Actualiza la informacion de los temporales
        /// </summary>
        protected virtual void UpdateTemp(object data)
        {
            try
            {
                _bc.AdministrationModel.aplTemporales_Save(this.documentID.ToString(), _bc.AdministrationModel.User, data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Enter(this, this.documentID, this._frmType, this.frmModule);

                if (FormProvider.Master.LoadFormTB)
                {
                    FormProvider.Master.itemSave.Enabled = false;
                    FormProvider.Master.itemDelete.Enabled = false;
                    FormProvider.Master.itemPrint.Enabled = false;
                    FormProvider.Master.itemCopy.Enabled = false;
                    FormProvider.Master.itemPaste.Enabled = false;
                    FormProvider.Master.itemImport.Enabled = false;
                    FormProvider.Master.itemExport.Enabled = false;
                    FormProvider.Master.itemRevert.Enabled = false;
                    FormProvider.Master.itemSendtoAppr.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinAppDocumentNominaBaseForm.cs", "Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Leave(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Leave(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinAppDocumentNominaBaseForm.cs", "Form_Leave"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {                
                FormProvider.Master.Form_Closing(this, this.documentID);                  
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinAppDocumentNominaBaseForm.cs", "Form_Closing"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma se cierra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this.frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinAppDocumentNominaBaseForm.cs", "Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos Header Superior

        /// <summary>
        /// Evento que se ejecuta al salir del numero de documento (glDocumentoControl - NumeroDoc)
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void txtNumeroDoc_Leave(object sender, EventArgs e) { }

        
        /// <summary>
        /// Evento que se ejecuta al pararse sobre el control de fecha
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void dtFecha_Enter(object sender, EventArgs e) { }

        /// <summary>
        /// Valida que el usuario haya ingresado una fecha
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void dtFecha_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.dtFecha.Text))
            {
                this.dtFecha.DateTime = this.dtFecha.Properties.MinValue;
            }
        }

        /// <summary>
        /// Valida que solo ingrese numeros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNumPrefix_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                e.Handled = true;
            if (e.KeyChar == 46)
                e.Handled = true;
        }

        #endregion
        
        #region Hilos

        /// <summary>
        /// Hilo de salvar
        /// </summary>
        public virtual void SaveThread() { }

        /// <summary>
        /// Hilo de Enviar para aprobación
        /// </summary>
        public virtual void SendToApproveThread() { }      

        #endregion

      
    }
}
