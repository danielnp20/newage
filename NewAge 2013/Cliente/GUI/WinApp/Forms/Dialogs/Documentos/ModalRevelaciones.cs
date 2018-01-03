using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;

namespace NewAge.Forms.Dialogs.Documentos
{
    public partial class ModalRevelaciones : Form
    {
        #region Variables

        private DTO_coDocumentoRevelacion documento = null;
        private BaseController _bc = BaseController.GetInstance();
        private int _documentID = AppForms.ModalRevelaciones;

        #endregion

        public ModalRevelaciones(string notaRevelacionID)
        {
            InitializeComponent();

            this.InitControls();
            this.uc_NotaRevelacion.Value = notaRevelacionID;
            this.uc_NotaRevelacion.Focus();

            FormProvider.LoadResources(this, this._documentID);
        }

        public ModalRevelaciones(DTO_coDocumentoRevelacion revelacion)
        {
            InitializeComponent();

            this.InitControls();
            this.uc_NotaRevelacion.Value = revelacion.NotaRevelacionID.Value;
            this.txtTituloRevelacion.Text = revelacion.TituloRevelacion.Value;
            this.uc_Revelaciones.ValueXML = revelacion.Revelacion.Value;

            FormProvider.LoadResources(this, this._documentID);
        }

        #region Funciones 

        /// <summary>
        /// Inicializa los controles
        /// </summary>
        private void InitControls()
        {
            this._bc.InitMasterUC(this.uc_NotaRevelacion, AppMasters.coReporteNota, true, true, false, false); 
        }

        /// <summary>
        /// Valida los campos obligatorios
        /// </summary>
        private void FieldsValidate()
        {
            if (!string.IsNullOrEmpty(this.uc_NotaRevelacion.Value) && !string.IsNullOrEmpty(this.uc_Revelaciones.ValueXML) && !string.IsNullOrEmpty(this.txtTituloRevelacion.Text))
                this.btnIncluirRevelacion.Enabled = true;
            else
                this.btnIncluirRevelacion.Enabled = false;
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Valida cuando sale del contro nota Revelacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uc_NotaRevelacion_Leave(object sender, EventArgs e)
        {
            this.FieldsValidate();
        }

        /// <summary>
        /// Valida con sale del control titulo Revelacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTituloRevelacion_Leave(object sender, EventArgs e)
        {
            this.FieldsValidate();
        }

        /// <summary>
        /// Valida cuando sale del contro revelacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uc_Revelaciones_Leave(object sender, EventArgs e)
        {
            this.FieldsValidate();
        }

        /// <summary>
        /// Incluye objeto de Revelacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIncluirRevelacion_Click(object sender, EventArgs e)
        {
            try
            {
                documento = new DTO_coDocumentoRevelacion();
                documento.NotaRevelacionID.Value = this.uc_NotaRevelacion.Value;
                documento.TituloRevelacion.Value = this.txtTituloRevelacion.Text;
                documento.Revelacion.Value = this.uc_Revelaciones.ValueXML;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }        
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Documento Revelacion
        /// </summary>
        public DTO_coDocumentoRevelacion DocRevelacion
        {
            get { return documento;  }
        }

        #endregion

       
    }
}
