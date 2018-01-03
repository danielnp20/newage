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
using DevExpress.XtraGrid.Views.Grid;
using System.Reflection;
using DevExpress.XtraGrid.Views.Base;
using NewAge.Librerias.Project;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using System.Configuration;
using SentenceTransformer;
using System.Collections;
using DevExpress.XtraEditors;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ModalDigitacion : Form
    {
        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        //Variables basicas
        private FormTypes _frmType = FormTypes.Query;
        private string _frmName;
        private int _documentID;
        private string _unboundPrefix = "Unbound_";
        //Variables de data
        private Type _tipoControl = null;
        private bool _isNumeric = false;
        private Dictionary<string, string> _itemsCmb = null;
        private int _idMaestra = 0;
        private int _numDecimal = 0;
        public string _valorFinal = string.Empty;
        public string _nombreCampo = string.Empty;
        #endregion

        #region Propiedades

        ///// <summary>
        ///// Documentos Control Seleccionados
        ///// </summary>
        //public string IDSelected
        //{
        //    get { return this._rowCurrent.ID.Value; }
        //}

        #endregion

       /// <summary>
       /// Conctructior
       /// </summary>
       /// <param name="tipoControl">Tipo de Control a mostrar</param>
       /// <param name="isNumeric">si es numerico</param>
        /// <param name="itemsCmb">si es combo trae los items para mostrar</param>
       /// <param name="nameMaster">si es maestra indica el nombre de la tabla</param>
        public ModalDigitacion(Type tipoControl,string nombreCampo, bool isNumeric, Dictionary<string, string> itemsCmb, int idMaestra = 0,int numDecimal=0 )
        {
            this.InitializeComponent();
            try
            {
                this._tipoControl = tipoControl;
                this._isNumeric = isNumeric;
                this._idMaestra = idMaestra;
                this._itemsCmb = itemsCmb;
                this._nombreCampo = nombreCampo;
                this._numDecimal = numDecimal;
                this.InitControls();
                //FormProvider.LoadResources(this, this._documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinAppModalDigitacion.cs", "ModalDigitacion: " + ex.Message));
            }
        }

        #region Funciones privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void InitControls()
        {
            if (this._tipoControl == typeof(ControlsUC.uc_MasterFind))
            {
                this.pnlMaster.Visible = true;
                this.lblMaster.Text = this._nombreCampo;
                
                this._bc.InitMasterUC(this.master1, this._idMaestra, false, true, false, false);
                this._bc.InitMasterUC(this.master2, this._idMaestra, false, true, false, false);
                
                //this.master2.Visible = false;
            }
            else if (this._tipoControl == typeof(TextBox) && !this._isNumeric)
            {
                this.pnlText.Visible = true;
                this.lblTexto.Text = this._nombreCampo;
                this.txt1.PasswordChar = '*';
            }
            else if (this._tipoControl == typeof(TextBox) && this._isNumeric)
            {
                this.pnlNumeric.Visible = true;
                this.lblNumerico.Text = this._nombreCampo;
                this.numeric1.Text = "0";
                this.numeric1.Properties.PasswordChar = '*';
                this.numeric2.Text = "0";
                this.numeric1.Properties.Mask.EditMask = "n2";
                this.numeric2.Properties.Mask.EditMask = "n2";
                this.numeric2.Text = "0";
                
            }
            else if (this._tipoControl == typeof(TextEdit) )
            {
                this.pnlNumeric.Visible = true;
                this.lblNumerico.Text = this._nombreCampo;
                this.numeric1.Properties.PasswordChar = '*';
                if (this._numDecimal == 0)
                {
                    this.numeric1.Properties.Mask.EditMask = "c0";
                    this.numeric2.Properties.Mask.EditMask = "c0";
                }
                else
                {
                    this.numeric1.Properties.Mask.EditMask = "n2";
                    this.numeric2.Properties.Mask.EditMask = "n2";
                }


            }
            else if (this._tipoControl == typeof(LookUpEdit))
            {
                this.cmb1.Properties.ValueMember = "Key";
                this.cmb1.Properties.PasswordChar = '*';
                this.cmb1.Properties.DisplayMember = "Value";
                this.cmb1.Properties.DataSource = this._itemsCmb;
                this.cmb2.Properties.ValueMember = "Key";
                this.cmb2.Properties.DisplayMember = "Value";
                this.cmb2.Properties.DataSource = this._itemsCmb;
                this.cmb1.EditValue = this._itemsCmb.Keys.First().ToString();
                this.cmb2.EditValue = this._itemsCmb.Keys.First().ToString();
                this.lblCombo.Text = this._nombreCampo;
                this.pnlCombo.Visible = true;
            }
            else if (this._tipoControl == typeof(DateEdit))
            {
                this.pnlDate.Visible = true;
                this.dt1.Properties.PasswordChar = '*';
                this.lblFecha.Text = this._nombreCampo;
            }         
        }


        /// <summary>
        /// Permite validar si los campos son iguales para continuar
        /// </summary>
        /// <param name="valor1"></param>
        /// <param name="valor2"></param>
        /// <returns></returns>
        private bool ValidData()
        {
            bool result = true;
            if (this._tipoControl == typeof(ControlsUC.uc_MasterFind))
            {
                if (this.master1.Value != this.master2.Value)
                    result = false;
                else
                    this._valorFinal = this.master1.Value;
            }
            else if (this._tipoControl == typeof(TextBox) && !this._isNumeric)
            {
                if (this.txt1.Text != this.txt2.Text)
                    result = false;
                else
                    this._valorFinal = this.txt1.Text;
            }
            else if (this._tipoControl == typeof(TextBox) && this._isNumeric)
            {
                if (this.numeric1.EditValue.ToString() != this.numeric2.EditValue.ToString())
                    result = false;
                else
                    this._valorFinal = this.numeric1.EditValue.ToString();
            }

            else if (this._tipoControl == typeof(TextEdit))
            {
                if (this.numeric1.EditValue.ToString() != this.numeric2.EditValue.ToString())
                    result = false;
                else
                    this._valorFinal = this.numeric1.EditValue.ToString();
            }
            else if (this._tipoControl == typeof(LookUpEdit))
            {
                if (this.cmb1.EditValue != this.cmb2.EditValue)
                    result = false;
                else
                    this._valorFinal = this.cmb1.EditValue.ToString();
            }
            else if (this._tipoControl == typeof(DateEdit))
            {
                if (this.dt1.DateTime != this.dt2.DateTime)
                    result = false;
                else
                    this._valorFinal = this.dt1.DateTime.ToShortDateString();
            }

            if (!result)
            {
                MessageBox.Show("Los datos no coinciden, verifique");
                this._valorFinal = string.Empty;
            }
            return result;
        }

        #endregion

        #region Eventos Controles

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void btnAccept_Click(object sender, EventArgs e)
        {
            if(this.ValidData())
                this.Close();
        }

        /// <summary>
        /// Al hacer click para cancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this._valorFinal = string.Empty;
            this.Close();
        }

        #endregion               

    }
}
