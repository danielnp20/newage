using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Resultados;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Clase para mostrar mensajes de saldos
    /// </summary>
    public partial class BalanceForm : Form
    {
        /// <summary>
        /// Controlador base
        /// </summary>
        private BaseController _bc = BaseController.GetInstance();
        private int _docID;
        private string _tab = "\t";

        /// <summary>
        /// Constructor
        /// </summary>
        public BalanceForm(DTO_coCuentaSaldo CtaSaldoDTO, DTO_glDocumentoControl docCtrlDTO, string ML, string ME)
        {
            InitializeComponent();
            _docID = AppForms.BalanceForm;
            FormProvider.LoadResources(this, _docID);
            
            string msg = string.Empty;
            if (CtaSaldoDTO != null && docCtrlDTO != null)
            {
                //Trae el valor de los campos 
                this.lblPeriod.Text = docCtrlDTO.PeriodoDoc.Value.Value.ToString();
                this.lblTercero.Text = docCtrlDTO.TerceroID.Value.ToString();
                this.lblDoc.Text = docCtrlDTO.DocumentoID.Value.Value.ToString();
                this.lblCuenta.Text = docCtrlDTO.CuentaID.Value.ToString();

                msg = _bc.GetResource(LanguageTypes.Forms, this._docID + "_monedaLocal") + ": (" + ML + ")" + Environment.NewLine;

                //Trae el valor de las monedas
                decimal sumaML = CtaSaldoDTO.DbOrigenLocML.Value.Value + CtaSaldoDTO.DbOrigenExtML.Value.Value + CtaSaldoDTO.CrOrigenLocML.Value.Value + CtaSaldoDTO.CrOrigenExtML.Value.Value
                    + CtaSaldoDTO.DbSaldoIniLocML.Value.Value + CtaSaldoDTO.DbSaldoIniExtML.Value.Value + CtaSaldoDTO.CrSaldoIniLocML.Value.Value + CtaSaldoDTO.CrSaldoIniExtML.Value.Value;

                msg += _tab + _bc.GetResource(LanguageTypes.Forms, this._docID + "_valorBase") + " $ " + CtaSaldoDTO.vlBaseML.Value.Value + Environment.NewLine;
                msg += _tab + _bc.GetResource(LanguageTypes.Forms, this._docID + "_saldo") + " $ " + Math.Round(sumaML, 2);
                
                //Si es multimoneda muestras los valores extranjeros
                if (_bc.AdministrationModel.MultiMoneda)
                {
                    msg += Environment.NewLine + Environment.NewLine;
                    msg += _bc.GetResource(LanguageTypes.Forms, this._docID + "_monedaExtranjera") + ": (" + ME + ")" + Environment.NewLine;

                    decimal sumaME = CtaSaldoDTO.DbOrigenLocME.Value.Value + CtaSaldoDTO.DbOrigenExtME.Value.Value + CtaSaldoDTO.CrOrigenLocME.Value.Value + CtaSaldoDTO.CrOrigenExtME.Value.Value
                        + CtaSaldoDTO.DbSaldoIniLocME.Value.Value + CtaSaldoDTO.DbSaldoIniExtME.Value.Value + CtaSaldoDTO.CrSaldoIniLocME.Value.Value + CtaSaldoDTO.CrSaldoIniExtME.Value.Value;
                    msg += _tab + _bc.GetResource(LanguageTypes.Forms, this._docID + "_valorBase") + " $ " + CtaSaldoDTO.vlBaseME.Value.Value + Environment.NewLine;
                    msg += _tab + _bc.GetResource(LanguageTypes.Forms, this._docID + "_saldo") + " $ " + Math.Round(sumaME, 2) + Environment.NewLine;
                }
                this.lblMessage.Text = msg;
            }
            else
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_DocNotBalance));
        }
    }
}
