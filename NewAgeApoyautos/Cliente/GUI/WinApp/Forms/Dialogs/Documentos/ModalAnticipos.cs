using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using NewAge.DTO.UDT;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Collections;
using DevExpress.XtraGrid.Views.Grid;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de consulta maestro (modal)
    /// </summary>
    public partial class ModalAnticipos : Form
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();
        private int _documentID = AppForms.ModalAnticiposForm;
        private TipoMoneda _tipoMoneda;
        private decimal _tasaCambio;
        private DateTime _periodo;
        private List<DTO_AnticiposResumen> _currentData;
        private string _tercero;
        protected string unboundPrefix = "Unbound_";
        public bool ReturnVals = false;
        public List<DTO_AnticiposResumen> ReturnList = new List<DTO_AnticiposResumen>();

        #endregion

        /// <summary>
        /// Constructor de la grilla de anticipos 
        /// </summary>
        /// <param name="antResumen">Lista de anticipos que ya fueron cargados</param>
        /// <param name="periodo">Periodo sobre el cual se van a consultar los anticipos</param>
        /// <param name="tm">Tipo de moneda sobre la cual se esta trabajando en el documento</param>
        /// <param name="tasaCambio">Tasa de cambio actual del documento</param>
        public ModalAnticipos(List<DTO_AnticiposResumen> antResumen, DateTime periodo, TipoMoneda tm, decimal tasaCambio, string terceroID)
        {
            //Inicializa el formulario
            InitializeComponent();
            FormProvider.LoadResources(this, this._documentID);

            //variables
            this._tipoMoneda = tm;
            this._tasaCambio = tasaCambio;
            this._periodo = periodo;
            this._currentData = antResumen;
            this._tercero = terceroID;

            //Carga de datos
            this.LoadGridStructure();
            this.LoadGridData();
        }

        #region Funciones privadas

        /// <summary>
        /// Genera la estructura de la grilla
        /// </summary>
        private void LoadGridStructure()
        {
            try
            {
                //Campo de marca
                GridColumn sel = new GridColumn();
                sel.FieldName = this.unboundPrefix + "Seleccionar";
                sel.UnboundType = UnboundColumnType.Boolean;
                sel.VisibleIndex = 0;
                sel.Width = 20;
                sel.Visible = true;
                //sel.ReadOnly = false;
                sel.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                sel.OptionsColumn.ShowCaption = false;
                this.gvData.Columns.Add(sel);

                GridColumn fecha = new GridColumn();
                fecha.FieldName = this.unboundPrefix + "Fecha";
                fecha.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_Fecha");
                fecha.UnboundType = UnboundColumnType.String;
                fecha.VisibleIndex = 1;
                fecha.Width = 100;
                fecha.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(fecha);

                GridColumn prefDoc = new GridColumn();
                prefDoc.FieldName = this.unboundPrefix + "DocumentoTercero";
                prefDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_DocumentoTercero");
                prefDoc.UnboundType = UnboundColumnType.String;
                prefDoc.VisibleIndex = 2;
                prefDoc.Width = 170;
                prefDoc.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(prefDoc);

                GridColumn mda = new GridColumn();
                mda.FieldName = this.unboundPrefix + "MonedaID";
                mda.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_MonedaID");
                mda.UnboundType = UnboundColumnType.String;
                mda.VisibleIndex = 3;
                mda.Width = 100;
                mda.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(mda);

                GridColumn valor = new GridColumn();
                valor.FieldName = this.unboundPrefix + "Valor";
                valor.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_Valor");
                valor.UnboundType = UnboundColumnType.Decimal;
                valor.VisibleIndex = 4;
                valor.Width = 110;
                valor.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(valor);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalAnticipos.cs", "LoadGridStructure"));
            }
        }

        /// <summary>
        /// Carga los datos de la grilla
        /// </summary>
        private void LoadGridData()
        {
            try
            {
                this.gcData.DataSource = this._currentData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalAnticipos", "LoadGridData"));
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvData_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (fieldName == "Valor")
                e.RepositoryItem = this.editSpin;
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvData_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            DTO_AnticiposResumen dto = this._currentData.ElementAt(e.ListSourceRowIndex);
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                if (fieldName == "Seleccionar")
                    e.Value = dto.Seleccionar.Value;
                if (fieldName == "Fecha")
                    e.Value = dto.Fecha.Value.Value.ToString(FormatString.DB_Date_YYYY_MM_DD);
                if (fieldName == "DocumentoTercero")
                    e.Value = dto.DocumentoTercero.Value;
                if (fieldName == "MonedaID")
                    e.Value = dto.MonedaID.Value;
                if (fieldName == "Valor")
                    e.Value = this._tipoMoneda == TipoMoneda.Local ? dto.ML.Value : dto.ME.Value;
            }
            if (e.IsSetData)
            {
                if (fieldName == "Seleccionar")
                    dto.Seleccionar.Value = Convert.ToBoolean(e.Value);
            }
        }

        /// <summary>
        /// Devuelve el registro seleccionado
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnReturn_Click(object sender, EventArgs e)
        {
            try
            {
                this.ReturnVals = true;
                this._currentData.ForEach(a =>
                {
                    if (a.Seleccionar.Value.Value)
                    {
                        if (this._tipoMoneda == TipoMoneda.Local)
                            a.ME.Value = this._tasaCambio == 0 ? 0 : Math.Round(a.ML.Value.Value / this._tasaCambio, 2);
                        else
                            a.ML.Value = this._tasaCambio == 0 ? 0 : Math.Round(a.ME.Value.Value * this._tasaCambio, 2);

                        this.ReturnList.Add(a);
                    }
                });

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalAnticipos.cs", "btnReturn_Click"));
            }
        }

        #endregion

    }
}
