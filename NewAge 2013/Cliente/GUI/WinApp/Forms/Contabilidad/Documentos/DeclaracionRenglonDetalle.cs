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
using System.Reflection;
using NewAge.DTO.UDT;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using NewAge.DTO.Negocio;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using SentenceTransformer;
using NewAge.DTO.Resultados;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class DeclaracionRenglonDetalle : Form
    {
        #region Variables
        BaseController _bc = BaseController.GetInstance();
        private string _frmName;
        private int _documentID;
        private string unboundPrefix = "Unbound_";

        //Info de grilla y formulario
        private List<DTO_DetalleRenglon> data;
        private string _impuestoID;
        private string _renglon;
        private short _añoDeclaracion;
        private short _mesDeclaracion;
        private int? _numeroDoc;
        private DTO_glDocumentoControl _ctrl;

        #endregion

        public DeclaracionRenglonDetalle(string impuestoID, string renglon, short añoDeclaracion, short mesDeclaracion, int? numeroDoc)
        {
            try
            {
                this.InitializeComponent();

                this._impuestoID = impuestoID;
                this._renglon = renglon;
                this._añoDeclaracion = añoDeclaracion;
                this._mesDeclaracion = mesDeclaracion;
                this._numeroDoc = numeroDoc;
                this._documentID = AppDocuments.DeclaracionImpuestos;
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
                FormProvider.LoadResources(this, this._documentID);

                this.txtAñoFiscal.Text = this._añoDeclaracion.ToString();
                this.txtPeriodoFiscal.Text = this._mesDeclaracion.ToString();
                this.txtDeclaracion.Text = impuestoID;
                this.txtRenglon.Text = renglon;

                //Carga las variables iniciales
                this.AddGridCols();
                this.LoadData();

                if (numeroDoc.HasValue)
                {
                    this._ctrl = _bc.AdministrationModel.glDocumentoControl_GetByID(this._numeroDoc.Value);
                    this.txtComp.Text = this._ctrl.ComprobanteID.Value;
                    this.txtNroComp.Text = this._ctrl.ComprobanteIDNro.Value.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "ProcesarDeclaracion.cs-ProcesarDeclaracion"));
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridCols()
        {
            //Cuenta
            GridColumn renglon = new GridColumn();
            renglon.FieldName = this.unboundPrefix + "CuentaID";
            renglon.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Cuenta");
            renglon.UnboundType = UnboundColumnType.String;
            renglon.VisibleIndex = 0;
            renglon.Width = 70;
            renglon.OptionsColumn.AllowEdit = false;
            this.gvCuentas.Columns.Add(renglon);

            //Fecha
            GridColumn fecha = new GridColumn();
            fecha.FieldName = this.unboundPrefix + "Fecha";
            fecha.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Fecha");
            fecha.UnboundType = UnboundColumnType.DateTime;
            fecha.VisibleIndex = 1;
            fecha.Width = 50;
            fecha.OptionsColumn.AllowEdit = false;
            this.gvCuentas.Columns.Add(fecha);

            //Comprobante
            GridColumn comp = new GridColumn();
            comp.FieldName = this.unboundPrefix + "Comprobante";
            comp.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Comprobante");
            comp.UnboundType = UnboundColumnType.String;
            comp.VisibleIndex = 2;
            comp.Width = 60;
            comp.OptionsColumn.AllowEdit = false;
            this.gvCuentas.Columns.Add(comp);

            //Tercero
            GridColumn tercero = new GridColumn();
            tercero.FieldName = this.unboundPrefix + "TerceroID";
            tercero.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_TerceroID");
            tercero.UnboundType = UnboundColumnType.String;
            tercero.VisibleIndex = 3;
            tercero.Width = 60;
            tercero.OptionsColumn.AllowEdit = false;
            this.gvCuentas.Columns.Add(tercero);

            //Nombre
            GridColumn nombre = new GridColumn();
            nombre.FieldName = this.unboundPrefix + "Nombre";
            nombre.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Nombre");
            nombre.UnboundType = UnboundColumnType.String;
            nombre.VisibleIndex = 4;
            nombre.Width = 70;
            nombre.OptionsColumn.AllowEdit = false;
            this.gvCuentas.Columns.Add(nombre);

            //Base
            GridColumn baseML = new GridColumn();
            baseML.FieldName = this.unboundPrefix + "VlrBaseML";
            baseML.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Base");
            baseML.UnboundType = UnboundColumnType.String;
            baseML.VisibleIndex = 5;
            baseML.Width = 50;
            baseML.OptionsColumn.AllowEdit = false;
            this.gvCuentas.Columns.Add(baseML);

            //Valor
            GridColumn valor = new GridColumn();
            valor.FieldName = this.unboundPrefix + "VlrMdaLoc";
            valor.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Valor");
            valor.UnboundType = UnboundColumnType.String;
            valor.VisibleIndex = 6;
            valor.Width = 50;
            valor.OptionsColumn.AllowEdit = false;
            this.gvCuentas.Columns.Add(valor);

            //Porcentaje
            GridColumn porc = new GridColumn();
            porc.FieldName = this.unboundPrefix + "Porcentaje";
            porc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Porcentaje");
            porc.UnboundType = UnboundColumnType.String;
            porc.VisibleIndex = 7;
            porc.Width = 50;
            porc.OptionsColumn.AllowEdit = false;
            this.gvCuentas.Columns.Add(porc);
        }

        /// <summary>
        /// Actualiza la informacion de la grilla
        /// </summary>
        private void LoadData()
        {
            try
            {
                this.data = _bc.AdministrationModel.DetallesRenglon_Get(this._impuestoID, this._renglon, this._mesDeclaracion, this._añoDeclaracion);
                this.gcCuentas.DataSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "ProcesarDeclaracion.cs-LoadData"));
            }
        }

        #endregion

        #region Eventos grilla

        /// <summary>
        /// Asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvCuentas_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "VlrBaseML" || fieldName == "VlrMdaLoc")
                e.RepositoryItem = this.editSpin;
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvCuentas_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                #region Trae datos
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                        e.Value = pi.GetValue(dto, null);
                    else
                        e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        if (fi.FieldType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32")
                            e.Value = fi.GetValue(dto);
                        else
                            e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                    }
                }
                #endregion
            }
            if (e.IsSetData)
            {
                #region Asigna datos
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (e.Value == null)
                    e.Value = string.Empty;
                if (pi != null)
                {
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                        e.Value = pi.GetValue(dto, null);
                    else
                    {
                        UDT udtProp = (UDT)pi.GetValue(dto, null);
                        udtProp.SetValueFromString(e.Value.ToString());
                    }
                }
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        UDT udtProp = (UDT)fi.GetValue(dto);
                        udtProp.SetValueFromString(e.Value.ToString());
                    }
                }
                #endregion
            }
        }

        /// <summary>
        /// Funcion que se ejecuta al hacer doble click sobre la info de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvCuentas_DoubleClick(object sender, EventArgs e)
        {
            //try
            //{
            //    GridView view = sender as GridView;
            //    GridHitInfo info = view.CalcHitInfo(this.gcCuentas.PointToClient(MousePosition));
            //    if (info.HitTest != GridHitTest.Column)
            //    {
            //        string msgTitleData = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_GetData);
            //        string msgGetData = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.GetDocument);

            //        if (MessageBox.Show(msgGetData, msgTitleData, MessageBoxButtons.YesNo) == DialogResult.Yes)
            //        {
            //            if (info.InRow || info.InRowCell)
            //            {
            //                DTO_DetalleRenglon renglon = this.data[info.RowHandle];
            //                DTO_coPlanCuenta cta = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, false, renglon.CuentaID.Value, false);
            //                string cSaldo = cta.ConceptoSaldoID.Value;
            //                DTO_glConceptoSaldo saldo = (DTO_glConceptoSaldo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, false, cSaldo, false);
            //                SaldoControl saldoControl = (SaldoControl)Enum.Parse(typeof(SaldoControl), saldo.coSaldoControl.Value.Value.ToString());

            //                switch (saldoControl)
            //                {
            //                    case SaldoControl.Cuenta:
            //                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDocCta));
            //                        break;
            //                    case SaldoControl.Activo_Diferido:
            //                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDocAct));
            //                        break;
            //                    case SaldoControl.Inventario:
            //                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDocInv));
            //                        break;
            //                    case SaldoControl.Doc_Interno:
            //                        DTO_glDocumentoControl docCtrlInt = _bc.AdministrationModel.glDocumentoControl_GetInternalDoc(Convert.ToInt32(saldo.DocumentoID.Value), renglon.PrefijoCOM.Value, Convert.ToInt32(renglon.DocumentoCOM.Value));

            //                        if (docCtrlInt == null)
            //                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
            //                        else
            //                            new ShowDocumentForm(docCtrlInt, null).Show();
            //                        break;
            //                    case SaldoControl.Doc_Externo:
            //                        DTO_glDocumentoControl docCtrlExt = _bc.AdministrationModel.glDocumentoControl_GetExternalDoc(Convert.ToInt32(saldo.DocumentoID.Value), renglon.TerceroID.Value, renglon.DocumentoCOM.Value);

            //                        if (docCtrlExt == null)
            //                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
            //                        else
            //                            new ShowDocumentForm(docCtrlExt, null).Show();

            //                        break;
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        #endregion

    }
}
