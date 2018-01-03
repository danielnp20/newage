using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using DevExpress.XtraEditors;
using NewAge.Librerias.Project;
using System.Text.RegularExpressions;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class CodificacionDireccion : Form
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();
        private int _documentID = 0;

        string[] direccionArray = new string[10];
        string direccionCompleta;

        private ButtonEdit _returnEdit; // Para buscarlo desde una grilla
        private TextBox _returnCod; // Para buscarlo desde un control

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor para botones en grillas
        /// </summary>
        /// <param name="origin">Boton de la grilla que inicia el formulario</param>
        /// <param name="lugarGeo">Identificador del lugar geografico</param>
        public CodificacionDireccion(ButtonEdit origin)
        {
            this._documentID = AppForms.CodificacionDireccion;

            InitializeComponent();

            FormProvider.LoadResources(this, this._documentID);            
            this._returnEdit = origin;

            this.InitControls();

            this.txtComplemento.Text = this._returnEdit.EditValue.ToString();
        }

        /// <summary>
        /// Constructor para campos de texto
        /// </summary>
        /// <param name="origin">Campo de texto que inicia el formulario</param>
        /// <param name="lugarGeo">Identificador del lugar geografico</param>
        public CodificacionDireccion(TextBox txtCod)
        {
            this._documentID = AppForms.CodificacionDireccion;

            InitializeComponent();

            FormProvider.LoadResources(this, this._documentID);
            this._returnCod = txtCod;

            this.InitControls();

            this.txtComplemento.Text = this._returnCod.Text.ToString();
        }

        #endregion

        #region Funciones Privadas
        
        /// <summary>
        /// Inicializa los controles
        /// </summary>
        private void InitControls()
        {
            try
            {
                #region Combo principal
                this.cmbCalle.Items.Add(new ComboBoxItem("", ""));
                this.cmbCalle.Items.Add(new ComboBoxItem("AC", "Avenida Calle"));
                this.cmbCalle.Items.Add(new ComboBoxItem("AK", "Avenida Carrera"));
                this.cmbCalle.Items.Add(new ComboBoxItem("CL", "Calle"));
                this.cmbCalle.Items.Add(new ComboBoxItem("KR", "Carrera"));
                this.cmbCalle.Items.Add(new ComboBoxItem("DG", "Diagonal"));
                this.cmbCalle.Items.Add(new ComboBoxItem("TV", "Transversal"));

                this.cmbCalle.DisplayMember = "Text";
                this.cmbCalle.SelectedIndex = 0;

                #endregion
                #region Combo complemento

                this.cmbComplemento.Items.Add(new ComboBoxItem("", ""));
                this.cmbComplemento.Items.Add(new ComboBoxItem("AP", "Apartamento"));
                this.cmbComplemento.Items.Add(new ComboBoxItem("AG", "Agrupación"));
                this.cmbComplemento.Items.Add(new ComboBoxItem("BL", "Bloque"));
                this.cmbComplemento.Items.Add(new ComboBoxItem("BG", "Bodega"));
                this.cmbComplemento.Items.Add(new ComboBoxItem("CN", "Camino"));
                this.cmbComplemento.Items.Add(new ComboBoxItem("CT", "Carretera"));
                this.cmbComplemento.Items.Add(new ComboBoxItem("CEL", "Célula"));
                this.cmbComplemento.Items.Add(new ComboBoxItem("CA", "Casa"));
                this.cmbComplemento.Items.Add(new ComboBoxItem("CONJ", "Conjunto"));
                this.cmbComplemento.Items.Add(new ComboBoxItem("CS", "Consultorio"));

                this.cmbComplemento.Items.Add(new ComboBoxItem("DP", "Depósito"));
                this.cmbComplemento.Items.Add(new ComboBoxItem("ED", "Edificio"));
                this.cmbComplemento.Items.Add(new ComboBoxItem("EN", "Entrada"));
                this.cmbComplemento.Items.Add(new ComboBoxItem("ESQ", "Esquina"));
                this.cmbComplemento.Items.Add(new ComboBoxItem("ET", "Etapa"));
                this.cmbComplemento.Items.Add(new ComboBoxItem("GJ", "Garaje"));
                this.cmbComplemento.Items.Add(new ComboBoxItem("IN", "Interior"));
                this.cmbComplemento.Items.Add(new ComboBoxItem("KM", "Kilómetro"));
                this.cmbComplemento.Items.Add(new ComboBoxItem("LC", "Local"));
                this.cmbComplemento.Items.Add(new ComboBoxItem("LT", "Lote"));

                this.cmbComplemento.Items.Add(new ComboBoxItem("MZ", "Manzana"));
                this.cmbComplemento.Items.Add(new ComboBoxItem("MN", "Mezanine"));
                this.cmbComplemento.Items.Add(new ComboBoxItem("MD", "Módulo"));
                this.cmbComplemento.Items.Add(new ComboBoxItem("OF", "Oficina"));
                this.cmbComplemento.Items.Add(new ComboBoxItem("PS", "Paseo"));
                this.cmbComplemento.Items.Add(new ComboBoxItem("PA", "Parcela"));
                this.cmbComplemento.Items.Add(new ComboBoxItem("PH", "Penthouse"));
                this.cmbComplemento.Items.Add(new ComboBoxItem("PI", "Piso"));
                this.cmbComplemento.Items.Add(new ComboBoxItem("PN", "Puente"));
                this.cmbComplemento.Items.Add(new ComboBoxItem("PD", "Predio"));

                this.cmbComplemento.Items.Add(new ComboBoxItem("SC", "Salón Comunal"));
                this.cmbComplemento.Items.Add(new ComboBoxItem("SR", "Sector"));
                this.cmbComplemento.Items.Add(new ComboBoxItem("SL", "Solar"));
                this.cmbComplemento.Items.Add(new ComboBoxItem("SS", "Semi Sotáno"));
                this.cmbComplemento.Items.Add(new ComboBoxItem("SM", "Super Manzana"));
                this.cmbComplemento.Items.Add(new ComboBoxItem("TO", "Torre"));
                this.cmbComplemento.Items.Add(new ComboBoxItem("UN", "Unidad"));
                this.cmbComplemento.Items.Add(new ComboBoxItem("UR", "Unidad Residencial"));
                this.cmbComplemento.Items.Add(new ComboBoxItem("URB", "Urbanización"));
                this.cmbComplemento.Items.Add(new ComboBoxItem("ZN", "Zona"));

                this.cmbComplemento.DisplayMember = "Text";
                this.cmbComplemento.SelectedIndex = 0;

                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CodificacionDireccion.cs", "InitControls"));
            }
        }

        /// <summary>
        /// Calcula la direccion temporal
        /// </summary>
        private void CalcularDireccion()
        {
            this.direccionCompleta = string.Empty;

            for (int i = 0; i < 10; ++i)
            {
                if (!string.IsNullOrWhiteSpace(direccionArray[i]))
                {
                    if (i != 0 && i != 2 && i != 7)
                        this.direccionCompleta += " ";

                    this.direccionCompleta += direccionArray[i];
                }
            }

            this.txtDireccion.Text = this.direccionCompleta;
        }

        /// <summary>
        /// Retorna la diraccion
        /// </summary>
        /// <param name="row"></param>
        private void ReturnAddress()
        {
            if (this._returnEdit != null)
                this._returnEdit.Text = this.direccionCompleta;

            if (this._returnCod != null)
                this._returnCod.Text = this.direccionCompleta;

            this.Close();
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Valida que solo numeros se pueden escribir
        /// </summary>
        /// <param name="sender">Objeto que ejecuta el evento</param>
        /// <param name="e">Argumentos del evento</param>
        protected void txtNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar != (Char)Keys.Back)
            //    e.Handled = true;
            //if (e.KeyChar == 46)
            //    e.Handled = true;
            
        }

        /// <summary>
        /// Evento que ese ejecuta al salir de un control de texto
        /// </summary>
        /// <param name="sender">Objeto que ejecuta el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void txtNumero_Leave(object sender, EventArgs e)
        {
            try
            {
                TextBox ctrl = (TextBox)sender;

                switch (ctrl.Name)
                {
                    case "txtCalleNumero":
                        this.direccionArray[1] = ctrl.Text.Trim();
                        break;
                    case "txtNumero":
                        this.direccionArray[6] = ctrl.Text.Trim();
                        break;
                    case "txtExtension":
                        this.direccionArray[8] = ctrl.Text.Trim();
                        break;
                }

                this.CalcularDireccion();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CodificacionDireccion.cs", "txtNumero_Leave"));
            }
        }

        /// <summary>
        /// Evento que ese ejecuta al cambiar la seleccion de un combo
        /// </summary>
        /// <param name="sender">Objeto que ejecuta el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void cmb_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                ComboBoxEx ctrl = (ComboBoxEx)sender;

                switch (ctrl.Name)
                {
                    case "cmbCalle":
                        this.direccionArray[0] = (ctrl.SelectedItem as ComboBoxItem).Value;
                        break;
                    case "cmbCalleLetra":
                        this.direccionArray[2] = ctrl.Text.Trim();
                        break;
                    case "cmbBis":
                        this.direccionArray[3] = ctrl.Text.Trim();
                        break;
                    case "cmbBisLetra":
                        this.direccionArray[4] = ctrl.Text.Trim();
                        break;
                    case "cmbCalleEste":
                        this.direccionArray[5] = ctrl.Text.Trim();
                        break;
                    case "cmbNumeroLetra":
                        this.direccionArray[7] = ctrl.Text.Trim();
                        break;
                    case "cmbExtensionEste":
                        this.direccionArray[9] = ctrl.Text.Trim();
                        break;
                }

                this.CalcularDireccion();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CodificacionDireccion.cs", "cmb_SelectedValueChanged"));
            }
        }

        /// <summary>
        /// Boton para reiniciar la direccion
        /// </summary>
        /// <param name="sender">Objeto que ejecuta el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void btn_Delete_Click(object sender, EventArgs e)
        {
            this.txtDireccion.Text = string.Empty;
        }

        /// <summary>
        /// Boton para agregar un complemento a la direccion
        /// </summary>
        /// <param name="sender">Objeto que ejecuta el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void btnAddComplemento_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtComplemento.Text))
                this.cmbComplemento.SelectedIndex = 0;

            
                this.direccionCompleta += " " + (this.cmbComplemento.SelectedItem as ComboBoxItem).Value + " " + this.txtComplemento.Text.Trim();

                this.txtComplemento.Text = string.Empty;
                this.cmbComplemento.SelectedIndex = 0;

                this.txtDireccion.Text = this.direccionCompleta;
            
        }

        /// <summary>
        /// Boton para retornar la direccion
        /// </summary>
        /// <param name="sender">Objeto que ejecuta el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                this.ReturnAddress();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CodificacionDireccion.cs", "btnAcptar_Click"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al presionar una tecla
        /// </summary>
        /// <param name="msg">Mensaje del evento</param>
        /// <param name="keyData">tecla presionada</param>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.Close();

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion

    }


}
