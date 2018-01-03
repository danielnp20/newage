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
    public partial class ModalDesestimiento : Form
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
        public string _Observacion = string.Empty;
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
        public ModalDesestimiento(string nombreCampo, int idMaestra = 0)
        {
            this.InitializeComponent();
            try
            {
                this._idMaestra = idMaestra;
                this._nombreCampo = nombreCampo;
                this.InitControls();
                //FormProvider.LoadResources(this, this._documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinAppModalDesestimiento.cs", "ModalDesestimiento: " + ex.Message));
            }
        }

        #region Funciones privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void InitControls()
        {
                this.pnlMaster.Visible = true;
                this.lblMaster.Text = this._nombreCampo;
                
                this._bc.InitMasterUC(this.master1, this._idMaestra, false, true, false, false);
               // this._bc.InitMasterUC(this.master2, this._idMaestra, false, true, false, false);
                
                //this.master2.Visible = false;
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
                //if (this.master1.Value != this.master2.Value)
                //    result = false;
                //else
                    this._valorFinal = this.master1.Value;
                    this._Observacion = this.txt2.Text;
            if (!result)
            {
                MessageBox.Show("Los datos no coinciden, verifique");
                this._valorFinal = string.Empty;
                this._Observacion = string.Empty;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void txt_KeyDown(object sender, KeyEventArgs e)
        {

        }

        #endregion               

    }
}
