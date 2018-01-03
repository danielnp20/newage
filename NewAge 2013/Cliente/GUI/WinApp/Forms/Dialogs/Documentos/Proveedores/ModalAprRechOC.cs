using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using SentenceTransformer;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario para mostrar estado del orden de compra
    /// </summary>
    public partial class ModalAprRechOC : Form
    {
        #region Variables
        BaseController _bc = BaseController.GetInstance();

        private int _documentID;
        private DTO_prOrdenCompraDocu _header;
        private DTO_glDocumentoControl _docCtrl;
        #endregion
        
        public ModalAprRechOC(DTO_prOrdenCompraDocu header, DTO_glDocumentoControl docCtrl)
        {
            InitializeComponent();

            this._documentID = AppForms.ModalAprRechOCForm;
            this._header = header;
            this._docCtrl = docCtrl;
        
            #region Iniciate masterControls
            this._bc.InitMasterUC(this.masterUsuarioApr1, AppMasters.seUsuario, true, true, true, false);
            this._bc.InitMasterUC(this.masterUsuarioApr2, AppMasters.seUsuario, true, true, true, false);
            this._bc.InitMasterUC(this.masterUsuarioApr3, AppMasters.seUsuario, true, true, true, false);
            this._bc.InitMasterUC(this.masterUsuarioApr4, AppMasters.seUsuario, true, true, true, false);
            this._bc.InitMasterUC(this.masterUsuarioApr5, AppMasters.seUsuario, true, true, true, false);
            this._bc.InitMasterUC(this.masterUsuarioRech, AppMasters.seUsuario, true, true, true, false);
            #endregion

            FormProvider.LoadResources(this, this._documentID);
            this.LoadData();
        }

        #region Funciones privadas
        /// <summary>
        /// Cargar Datos
        /// </summary>
        private void LoadData()
        {
            if (string.IsNullOrEmpty(this._header.UsuarioRechaza.Value) && this._docCtrl.Estado.Value.Value == (byte)EstadoDocControl.Aprobado)
                this.cbAprobar.Checked = true;

            if (!string.IsNullOrEmpty(this._header.UsuarioRechaza.Value))
            {
                this.cbRechazar.Checked = true;
                this.masterUsuarioRech.Value = this._header.UsuarioRechaza.Value;
                this.dtFechaRech.DateTime = this._header.FechaERechazo.Value.Value;
                this.txtRechazar.Text = this._header.ObservRechazo.Value;
            }
        }
        #endregion

        #region Eventos
        /// <summary>
        /// Evento que se ejecuta al hacer click al boton
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnCerrar_Click(object sender, EventArgs e)
        {            
            this.Close();
        }
        #endregion
    }
}
