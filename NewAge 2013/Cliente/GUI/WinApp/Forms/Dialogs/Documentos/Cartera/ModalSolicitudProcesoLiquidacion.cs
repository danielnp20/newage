using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using NewAge.DTO.Negocio;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ModalSolicitudProcesoLiquidacion : Form
    {
        #region Variables
        private BaseController _bc = BaseController.GetInstance();
        private int _documentID = AppForms.ModalProcesoSolicitudes;
        protected string unboundPrefix = "Unbound_";
        #endregion

        public ModalSolicitudProcesoLiquidacion()
        {
            //Inicializa El Formulario
            InitializeComponent();
            FormProvider.LoadResources(this, this._documentID);
            
            //Carga de datos
            this.LoadGridStructure();
            this.LoadGridData();
        }

        #region Funciones Privadas

        /// <summary>
        /// Genera la estructura de la grilla
        /// </summary>
        private void LoadGridStructure()
        {
            //Cooperativa
            GridColumn Cooperativa = new GridColumn();
            Cooperativa.FieldName = this.unboundPrefix + "Cooperativa";
            Cooperativa.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_Cooperativa");
            Cooperativa.UnboundType = UnboundColumnType.Boolean;
            Cooperativa.VisibleIndex = 0;
            Cooperativa.Width = 20;
            Cooperativa.Visible = true;
            Cooperativa.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            Cooperativa.OptionsColumn.ShowCaption = false;
            this.gvData.Columns.Add(Cooperativa);

            //NombreCooperativa
            GridColumn NombreCope = new GridColumn();
            NombreCope.FieldName = this.unboundPrefix + "NombreCope";
            NombreCope.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_NombreCope");
            NombreCope.UnboundType = UnboundColumnType.Boolean;
            NombreCope.VisibleIndex = 1;
            NombreCope.Width = 20;
            NombreCope.Visible = true;
            NombreCope.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            NombreCope.OptionsColumn.ShowCaption = false;
            this.gvData.Columns.Add(NombreCope);

            //Libranza
            GridColumn Libranza = new GridColumn();
            Libranza.FieldName = this.unboundPrefix + "Libranza";
            Libranza.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_Libranza");
            Libranza.UnboundType = UnboundColumnType.Integer;
            Libranza.VisibleIndex = 2;
            Libranza.Width = 20;
            Libranza.Visible = true;
            Libranza.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            Libranza.OptionsColumn.ShowCaption = false;
            this.gvData.Columns.Add(Libranza);

            //Valor Solicitado
            GridColumn VlrSolicitado = new GridColumn();
            VlrSolicitado.FieldName = this.unboundPrefix + "VlrSolicitado";
            VlrSolicitado.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_VlrSolicitado");
            VlrSolicitado.UnboundType = UnboundColumnType.Integer;
            VlrSolicitado.VisibleIndex = 3;
            VlrSolicitado.Width = 20;
            VlrSolicitado.Visible = true;
            VlrSolicitado.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            VlrSolicitado.OptionsColumn.ShowCaption = false;
            this.gvData.Columns.Add(VlrSolicitado);

            //Valor Cuota
            GridColumn VlrCuota = new GridColumn();
            VlrCuota.FieldName = this.unboundPrefix + "VlrSolicitado";
            VlrCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_VlrCuota");
            VlrCuota.UnboundType = UnboundColumnType.Integer;
            VlrCuota.VisibleIndex = 4;
            VlrCuota.Width = 20;
            VlrCuota.Visible = true;
            VlrCuota.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            VlrCuota.OptionsColumn.ShowCaption = false;
            this.gvData.Columns.Add(VlrCuota);

            //Plazo
            GridColumn Plazo = new GridColumn();
            Plazo.FieldName = this.unboundPrefix + "Plazo";
            Plazo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_Plazo");
            Plazo.UnboundType = UnboundColumnType.Integer;
            Plazo.VisibleIndex = 4;
            Plazo.Width = 20;
            Plazo.Visible = true;
            Plazo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            Plazo.OptionsColumn.ShowCaption = false;
            this.gvData.Columns.Add(VlrCuota);
        }

        private void LoadGridData() 
        {
            //try
            //{
            //    List<DTO_NotasEnvioResumen> notas = _bc.AdministrationModel.LiquidacionCredito_GetFromSolicitud(this.ac

            //    if (this._notaEnvioRel != 0)
            //    {
            //        notas.Find(nota => nota.NumeroDoc.Value.Value == this._notaEnvioRel).Seleccionar.Value = true;
            //    }

            //    this._currentData = notas;
            //    this.gcData.DataSource = this._currentData;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "ModalSolicitudProcesoLiquidacion-LoadGridData"));
            //}
        }
        #endregion

    }
}
