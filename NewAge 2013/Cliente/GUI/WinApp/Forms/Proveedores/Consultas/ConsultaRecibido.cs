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
using System.Text.RegularExpressions;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ConsultaRecibido : FormWithToolbar
    {
        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        //Variables basicas
        private FormTypes _frmType = FormTypes.Query;
        private string _frmName;
        private int _documentID;
        private ModulesPrefix _frmModule;
        private string _unboundPrefix = "Unbound_";
        private int _pageSize = 50;
        private int indexFila = 0;
        //Variables de data
        private List<DTO_ConsultaCompras> _listDocuments = null;
        private List<DTO_ConsultaDocRelacion> _listDocumentsRelacion = null;

        #endregion

        ///<summary>
        /// Constructor 
        /// </summary>
        public ConsultaRecibido()
        {
            this.InitializeComponent();
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter,this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);    
                this.AddGridColsDocument();
                this.AddGridColsDocRelacion();
                this._bc.Pagging_Init(this.pgGrid, _pageSize);
                this._bc.Pagging_SetEvent(this.pgGrid, this.pagging_Click);
                this.pgGrid.UpdatePageNumber(0, true, true, false);   
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "ConsultaSolicitud.cs", "ConsultaSolicitud: " + ex.Message));
            }
        }

        #region Funciones privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentID = AppQueries.QueryRecibidos;
            this._frmModule = ModulesPrefix.pr;
            this._listDocuments = new List<DTO_ConsultaCompras>();

            #region Inicializa Controles
            this._bc.InitMasterUC(this.masterPrefijo, AppMasters.glPrefijo, true, true, false, false);
            this.masterPrefijo.Value = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
            #endregion

        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridColsDocument()
        {
            //Prefijo Documento
            GridColumn prefDoc = new GridColumn();
            prefDoc.FieldName = this._unboundPrefix + "PrefDoc";
            prefDoc.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_PrefDoc");
            prefDoc.UnboundType = UnboundColumnType.String;
            prefDoc.VisibleIndex = 1;
            prefDoc.Width = 80;
            prefDoc.Visible = true;
            prefDoc.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(prefDoc);
            //Fecha
            GridColumn fecha = new GridColumn();
            fecha.FieldName = this._unboundPrefix + "Fecha";
            fecha.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_Fecha");
            fecha.UnboundType = UnboundColumnType.DateTime;
            fecha.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            fecha.AppearanceCell.Options.UseTextOptions = true;
            fecha.VisibleIndex = 2;
            fecha.Width = 50;
            fecha.Visible = true;
            fecha.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(fecha);
            //Proveedor
            GridColumn proveedorID = new GridColumn();
            proveedorID.FieldName = this._unboundPrefix + "ProveedorID";
            proveedorID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ProveedorID");
            proveedorID.UnboundType = UnboundColumnType.String;
            proveedorID.VisibleIndex = 3;
            proveedorID.Width = 80;
            proveedorID.Visible = true;
            proveedorID.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(proveedorID);
            //Nombre de Proveedor
            GridColumn ProveedorNombre = new GridColumn();
            ProveedorNombre.FieldName = this._unboundPrefix + "ProveedorNombre";
            ProveedorNombre.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ProveedorNombre");
            ProveedorNombre.UnboundType = UnboundColumnType.String;
            ProveedorNombre.VisibleIndex = 4;
            ProveedorNombre.Width = 200;
            ProveedorNombre.Visible = true;
            ProveedorNombre.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(ProveedorNombre);
            //BodegaID
            GridColumn Bodega = new GridColumn();
            Bodega.FieldName = this._unboundPrefix + "Bodega";
            Bodega.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Bodega");
            Bodega.UnboundType = UnboundColumnType.String;
            Bodega.VisibleIndex = 5;
            Bodega.Width = 120;
            Bodega.Visible = true;
            Bodega.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(Bodega);
            //Cantidad Rec Total
            GridColumn cantidadRecTotal = new GridColumn();
            cantidadRecTotal.FieldName = this._unboundPrefix + "CantidadRec";
            cantidadRecTotal.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_CantidadRecTotal");
            cantidadRecTotal.UnboundType = UnboundColumnType.Decimal;
            cantidadRecTotal.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            cantidadRecTotal.AppearanceCell.Options.UseTextOptions = true;
            cantidadRecTotal.VisibleIndex = 6;
            cantidadRecTotal.Width = 100;
            cantidadRecTotal.Visible = false;
            cantidadRecTotal.ColumnEdit = this.editValue;
            cantidadRecTotal.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(cantidadRecTotal);
            //Cantidad Fact Total
            GridColumn cantidadFactTotal = new GridColumn();
            cantidadFactTotal.FieldName = this._unboundPrefix + "CantidadFact";
            cantidadFactTotal.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_CantidadFactTotal");
            cantidadFactTotal.UnboundType = UnboundColumnType.Decimal;
            cantidadFactTotal.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            cantidadFactTotal.AppearanceCell.Options.UseTextOptions = true;
            cantidadFactTotal.VisibleIndex = 7;
            cantidadFactTotal.Width = 100;
            cantidadFactTotal.Visible = false;
            cantidadFactTotal.ColumnEdit = this.editValue;
            cantidadFactTotal.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(cantidadFactTotal);
            //Estado
            GridColumn estado = new GridColumn();
            estado.FieldName = this._unboundPrefix + "Estado";
            estado.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Estado");
            estado.UnboundType = UnboundColumnType.String;
            estado.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            estado.AppearanceCell.Options.UseTextOptions = true;
            estado.VisibleIndex = 9;
            estado.Width = 70;
            estado.Visible = true;
            estado.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(estado);

            //Ver Documento
            GridColumn file = new GridColumn();
            file.FieldName = this._unboundPrefix + "FileUrl";
            file.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ViewDocument");
            file.UnboundType = UnboundColumnType.String;
            file.Width = 100;
            file.VisibleIndex = 10;
            file.Visible = true;
            file.OptionsColumn.AllowEdit = true;
            file.ColumnEdit = this.LinkEdit;
            file.OptionsColumn.ShowCaption = false;
            //file.AppearanceCell.ForeColor = Color.Blue;
            this.gvDocument.Columns.Add(file);

            #region Detalle
            
            //Prefijo Documento
            GridColumn PrefDocOrig = new GridColumn();
            PrefDocOrig.FieldName = this._unboundPrefix + "PrefDocOrig";
            PrefDocOrig.Caption = "Doc Ord Comp";
            PrefDocOrig.UnboundType = UnboundColumnType.String;
            PrefDocOrig.VisibleIndex = 0;
            PrefDocOrig.Width = 60;
            PrefDocOrig.Visible = true;
            PrefDocOrig.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(PrefDocOrig);

            GridColumn CodigoBSID = new GridColumn();
            CodigoBSID.FieldName = this._unboundPrefix + "CodigoBSID";
            CodigoBSID.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_CodigoBSID");
            CodigoBSID.UnboundType = UnboundColumnType.String;
            CodigoBSID.VisibleIndex = 0;
            CodigoBSID.Width = 80;
            CodigoBSID.Visible = true;
            CodigoBSID.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(CodigoBSID);

            GridColumn inReferenciaID = new GridColumn();
            inReferenciaID.FieldName = this._unboundPrefix + "inReferenciaID";
            inReferenciaID.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_inReferenciaID");
            inReferenciaID.UnboundType = UnboundColumnType.String;
            inReferenciaID.VisibleIndex = 1;
            inReferenciaID.Width = 80;
            inReferenciaID.Visible = true;
            inReferenciaID.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(inReferenciaID);

            GridColumn descripcion = new GridColumn();
            descripcion.FieldName = this._unboundPrefix + "Descriptivo";
            descripcion.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Descriptivo");
            descripcion.UnboundType = UnboundColumnType.String;
            descripcion.VisibleIndex = 2;
            descripcion.Width = 100;
            descripcion.Visible = true;
            descripcion.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(descripcion);

            //MarcaInvID
            GridColumn MarcaInvIDConsol = new GridColumn();
            MarcaInvIDConsol.FieldName = this._unboundPrefix + "MarcaInvID";
            MarcaInvIDConsol.Caption = this._bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_MarcaInvID");
            MarcaInvIDConsol.UnboundType = UnboundColumnType.String;
            MarcaInvIDConsol.VisibleIndex = 3;
            MarcaInvIDConsol.Width = 60;
            MarcaInvIDConsol.Visible = true;
            MarcaInvIDConsol.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(MarcaInvIDConsol);

            //RefProveedor
            GridColumn RefProveedorConsol = new GridColumn();
            RefProveedorConsol.FieldName = this._unboundPrefix + "RefProveedor";
            RefProveedorConsol.Caption = this._bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_RefProveedor");
            RefProveedorConsol.UnboundType = UnboundColumnType.String;
            RefProveedorConsol.VisibleIndex = 4;
            RefProveedorConsol.Width = 60;
            RefProveedorConsol.Visible = true;
            RefProveedorConsol.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(RefProveedorConsol);

            //Cantidad recibido
            GridColumn CantidadRec = new GridColumn();
            CantidadRec.FieldName = this._unboundPrefix + "CantidadRec";
            CantidadRec.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_CantidadRec");
            CantidadRec.UnboundType = UnboundColumnType.Decimal;
            CantidadRec.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantidadRec.AppearanceCell.Options.UseTextOptions = true;
            CantidadRec.ColumnEdit = this.editValue;
            CantidadRec.VisibleIndex = 5;
            CantidadRec.Width = 80;
            CantidadRec.Visible = true;
            CantidadRec.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(CantidadRec);

            //Cantidad factura
            GridColumn CantidadFact = new GridColumn();
            CantidadFact.FieldName = this._unboundPrefix + "CantidadFact";
            CantidadFact.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_CantidadFact");
            CantidadFact.UnboundType = UnboundColumnType.Decimal;
            CantidadFact.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantidadFact.AppearanceCell.Options.UseTextOptions = true;
            CantidadFact.ColumnEdit = this.editValue;
            CantidadFact.VisibleIndex = 6;
            CantidadFact.Width = 80;
            CantidadFact.Visible = true;
            CantidadFact.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(CantidadFact);

            //Proyecto
            GridColumn proyecto = new GridColumn();
            proyecto.FieldName = this._unboundPrefix + "ProyectoID";
            proyecto.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_ProyectoID");
            proyecto.UnboundType = UnboundColumnType.String;
            proyecto.VisibleIndex = 7;
            proyecto.Width = 100;
            proyecto.Visible = true;
            proyecto.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(proyecto);

            //Centro de costo
            GridColumn ctoCosto = new GridColumn();
            ctoCosto.FieldName = this._unboundPrefix + "CentroCostoID";
            ctoCosto.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_CentroCostoID");
            ctoCosto.UnboundType = UnboundColumnType.String;
            ctoCosto.VisibleIndex = 8;
            ctoCosto.Width = 100;
            ctoCosto.Visible = true;
            ctoCosto.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(ctoCosto);

            #endregion

            this.gvDocument.OptionsView.ColumnAutoWidth = true;
            this.gvDetalle.OptionsView.ColumnAutoWidth = true;

        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridColsDocRelacion()
        {
            //PrefDoc
            GridColumn prefDoc = new GridColumn();
            prefDoc.FieldName = this._unboundPrefix + "PrefDoc";
            prefDoc.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_PrefDoc");
            prefDoc.UnboundType = UnboundColumnType.String;
            prefDoc.VisibleIndex = 1;
            prefDoc.Width = 80;
            prefDoc.Visible = true;
            prefDoc.OptionsColumn.AllowEdit = false;
            this.gvDocRelacion.Columns.Add(prefDoc);
            //Fecha
            GridColumn fecha = new GridColumn();
            fecha.FieldName = this._unboundPrefix + "Fecha";
            fecha.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_Fecha");
            fecha.UnboundType = UnboundColumnType.DateTime;
            fecha.VisibleIndex = 2;
            fecha.Width = 80;
            fecha.Visible = true;
            fecha.OptionsColumn.AllowEdit = false;
            this.gvDocRelacion.Columns.Add(fecha);
            //Proveedor
            GridColumn proveedorID = new GridColumn();
            proveedorID.FieldName = this._unboundPrefix + "ProveedorID";
            proveedorID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ProveedorID");
            proveedorID.UnboundType = UnboundColumnType.String;
            proveedorID.VisibleIndex = 3;
            proveedorID.Width = 80;
            proveedorID.Visible = true;
            proveedorID.OptionsColumn.AllowEdit = false;
            this.gvDocRelacion.Columns.Add(proveedorID);
            //Nombre de Proveedor
            GridColumn ProveedorNombre = new GridColumn();
            ProveedorNombre.FieldName = this._unboundPrefix + "ProveedorNombre";
            ProveedorNombre.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ProveedorNombre");
            ProveedorNombre.UnboundType = UnboundColumnType.String;
            ProveedorNombre.VisibleIndex = 4;
            ProveedorNombre.Width = 80;
            ProveedorNombre.Visible = true;
            ProveedorNombre.OptionsColumn.AllowEdit = false;
            this.gvDocRelacion.Columns.Add(ProveedorNombre);
            // Nro Factura
            GridColumn facturaNro = new GridColumn();
            facturaNro.FieldName = this._unboundPrefix + "FacturaNro";
            facturaNro.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FacturaNro");
            facturaNro.UnboundType = UnboundColumnType.String;
            facturaNro.VisibleIndex = 5;
            facturaNro.Width = 80;
            facturaNro.Visible = true;
            facturaNro.OptionsColumn.AllowEdit = false;
            this.gvDocRelacion.Columns.Add(facturaNro);
            //Moneda
            GridColumn MonedaID = new GridColumn();
            MonedaID.FieldName = this._unboundPrefix + "MonedaID";
            MonedaID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_MonedaID");
            MonedaID.UnboundType = UnboundColumnType.String;
            MonedaID.VisibleIndex = 6;
            MonedaID.Width = 80;
            MonedaID.Visible = true;
            MonedaID.OptionsColumn.AllowEdit = false;
            this.gvDocRelacion.Columns.Add(MonedaID);
            //Cantidad
            GridColumn cantidad = new GridColumn();
            cantidad.FieldName = this._unboundPrefix + "Cantidad";
            cantidad.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Cantidad");
            cantidad.UnboundType = UnboundColumnType.Decimal;
            cantidad.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            cantidad.AppearanceCell.Options.UseTextOptions = true;
            cantidad.ColumnEdit = this.editValue;
            cantidad.VisibleIndex = 7;
            cantidad.Width = 100;
            cantidad.Visible = true;
            cantidad.OptionsColumn.AllowEdit = false;
            this.gvDocRelacion.Columns.Add(cantidad);
            //Valor unitario
            GridColumn ValorUni = new GridColumn();
            ValorUni.FieldName = this._unboundPrefix + "ValorUni";
            ValorUni.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ValorUni");
            ValorUni.UnboundType = UnboundColumnType.Decimal;
            ValorUni.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ValorUni.AppearanceCell.Options.UseTextOptions = true;
            ValorUni.VisibleIndex = 8;
            ValorUni.Width = 100;
            ValorUni.Visible = false;
            ValorUni.ColumnEdit = this.editValue;
            ValorUni.OptionsColumn.AllowEdit = false;
            this.gvDocRelacion.Columns.Add(ValorUni);
            //Valor
            GridColumn valorTotal = new GridColumn();
            valorTotal.FieldName = this._unboundPrefix + "Valor";
            valorTotal.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ValorTotal");
            valorTotal.UnboundType = UnboundColumnType.Decimal;
            valorTotal.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            valorTotal.AppearanceCell.Options.UseTextOptions = true;
            valorTotal.VisibleIndex = 9;
            valorTotal.Width = 100;
            valorTotal.Visible = true;
            valorTotal.ColumnEdit = this.editValue;
            valorTotal.OptionsColumn.AllowEdit = false;
            this.gvDocRelacion.Columns.Add(valorTotal);
            //Ver Documento
            GridColumn file = new GridColumn();
            file.FieldName = this._unboundPrefix + "FileUrl";
            file.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ViewDocument");
            file.UnboundType = UnboundColumnType.String;
            file.Width = 100;
            file.VisibleIndex = 10;
            file.Visible = true;
            file.OptionsColumn.AllowEdit = true;
            file.ColumnEdit = this.LinkEditDocRelacion;
            file.OptionsColumn.ShowCaption = false;
            this.gvDocRelacion.Columns.Add(file);
        }

        /// <summary>
        /// Carga los datos de la grilla
        /// </summary>
        private void LoadGridData()
        {
            try
            {
                this.gcDocument.DataSource = this._listDocuments;
                this.gbQueryDoc.Enabled = false;
                this.pgGrid.UpdatePageNumber(this._listDocuments.Count, false, true, false);          
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaBodega.cs", "ConsultaBodega(" + this._documentID + ")-LoadGridData: " + ex.Message));
            }
        }
        #endregion

        #region Eventos Controles

        /// <summary>
        /// Valida que solo numeros se pueden escribir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNro_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                e.Handled = true;
            if (e.KeyChar == 46)
                e.Handled = true;
        }

        /// <summary>
        /// Valida que el numero del recibo ingresado exista
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtNro_Leave(object sender, EventArgs e)
        {
            if (this.masterPrefijo.ValidID && !string.IsNullOrEmpty(this.txtNro.Text) && this.txtNro.Text != "0")
            {
                List<DTO_glDocumentoControl> docSelect = new List<DTO_glDocumentoControl>();
                DTO_glDocumentoControl doc = new DTO_glDocumentoControl();
                doc.PrefijoID.Value = this.masterPrefijo.Value;
                doc.DocumentoNro.Value = !string.IsNullOrEmpty(this.txtNro.Text) ? Convert.ToInt32(this.txtNro.Text) : 0;
                docSelect.Add(doc);
                this._listDocuments = this._bc.AdministrationModel.ConsultaCompras_Get(AppDocuments.Recibido, docSelect);
                this.LoadGridData();
            }
        }

        /// <summary>
        /// Evento que se ejecuta al darle click al paginador
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void pagging_Click(object sender, System.EventArgs e)
        {
            try
            {
                var tmp = this._listDocuments.Skip((this.pgGrid.PageNumber - 1) * this._pageSize).Take(this._pageSize).ToList<DTO_ConsultaCompras>();
                this.pgGrid.UpdatePageNumber(this._listDocuments.Count, false, false, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaBodega.cs", "pagging_Click: " + ex.Message));
            }
        }

        /// <summary>
        /// Se realiza al entrar el boton
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnQueryDoc_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> docs = new List<int>();
                docs.Add(AppDocuments.Recibido);
                ModalQueryDocument getDocControl = new ModalQueryDocument(docs, true, false);
                getDocControl.ShowDialog();
                if (getDocControl.ListaDocSelected.Count > 0)
                {
                    List<DTO_glDocumentoControl> docSelect = getDocControl.ListaDocSelected;
                    this._listDocuments = this._bc.AdministrationModel.ConsultaCompras_Get(AppDocuments.Recibido, docSelect);
                    if (this._listDocuments.Count != 1)
                    {
                        this.txtNro.Text = string.Empty;
                        this.masterPrefijo.Value = string.Empty;
                    }
                    else
                    {
                        this.txtNro.Text = getDocControl.DocumentoControl.DocumentoNro.Value.ToString();
                        this.masterPrefijo.Value = getDocControl.DocumentoControl.PrefijoID.Value;
                    }
                    this.LoadGridData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaSolicitud.cs", "btnQueryDoc_Click"));
            }
        }

        /// <summary>
        /// Link al documento de activos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void LinkEdit_Click(object sender, System.EventArgs e)
        {
            try
            {
                int fila = this.gvDocument.FocusedRowHandle;

                DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl();
                DTO_Comprobante comprobante = new DTO_Comprobante();

                ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(this._listDocuments[fila].NumeroDoc.Value.Value);
                comprobante = !string.IsNullOrEmpty(ctrl.ComprobanteID.Value) ? this._bc.AdministrationModel.Comprobante_Get(true, false, ctrl.PeriodoDoc.Value.Value, ctrl.ComprobanteID.Value, ctrl.ComprobanteIDNro.Value.Value, null, null, null) : null;

                ShowDocumentForm documentForm = new ShowDocumentForm(ctrl, comprobante);
                documentForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentReverciones.cs", "LinkEdit_Click"));
            }
        }

        /// <summary>
        /// Link al documento
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void LinkEditDocRelacion_Click(object sender, System.EventArgs e)
        {
            try
            {
                int fila = this.gvDocRelacion.FocusedRowHandle;

                DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl();
                DTO_Comprobante comprobante = new DTO_Comprobante();

                ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(this._listDocumentsRelacion[fila].FacturaDocuID.Value.Value);
                comprobante = !string.IsNullOrEmpty(ctrl.ComprobanteID.Value) ? this._bc.AdministrationModel.Comprobante_Get(true, false, ctrl.PeriodoDoc.Value.Value, ctrl.ComprobanteID.Value, ctrl.ComprobanteIDNro.Value.Value, null, null, null) : null;

                ShowDocumentForm documentForm = new ShowDocumentForm(ctrl, comprobante);
                documentForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentReverciones.cs", "LinkEditDocRelacion_Click"));
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
                FormProvider.Master.Form_Enter(this, this._documentID, this._frmType, this._frmModule);
                FormProvider.Master.itemSearch.Visible = false;
                FormProvider.Master.itemUpdate.Visible = false;              
                FormProvider.Master.tbBreak1.Visible = false;
                FormProvider.Master.itemNew.Visible = true;
                FormProvider.Master.itemExport.Visible = true;
                FormProvider.Master.itemExport.Enabled = true;
                FormProvider.Master.itemNew.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Get); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaSolicitud.cs", "Form_Enter: " + ex.Message));
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
                FormProvider.Master.Form_Leave(this, this._documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaSolicitud.cs", "Form_Leave: " + ex.Message));
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
                FormProvider.Master.Form_Closing(this, this._documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaSolicitud.cs", "Form_Closing: " + ex.Message));
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
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this._frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaSolicitud.cs", "Form_FormClosed: " + ex.Message));
            }
        }

        #endregion

        #region Eventos Grillas

        /// <summary>
        /// Asigna texto por defecto para la columna de archivos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
            try
            {
                if (fieldName == "FileUrl" && this.gvDocument.DataRowCount > 0)
                    e.DisplayText = e.Column.Caption;
                if (fieldName == "Estado" && this.gvDocument.DataRowCount > 0)
                {
                    if (Convert.ToInt32(e.Value) == -1)
                        e.DisplayText = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateCerrado);
                    if (Convert.ToInt32(e.Value) == 0)
                        e.DisplayText = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateAnulado);
                    if (Convert.ToInt32(e.Value) == 1)
                        e.DisplayText = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateSinAprobar);
                    if (Convert.ToInt32(e.Value) == 2)
                        e.DisplayText = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateParaAprobacion);
                    if (Convert.ToInt32(e.Value) == 3)
                        e.DisplayText = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateAprobado);
                    if (Convert.ToInt32(e.Value) == 4)
                        e.DisplayText = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateRevertido);
                    if (Convert.ToInt32(e.Value) == 5)
                        e.DisplayText = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateDevuelto);
                    if (Convert.ToInt32(e.Value) == 6)
                        e.DisplayText = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateRadicado);
                    if (Convert.ToInt32(e.Value) == 7)
                        e.DisplayText = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateRevisado);
                    if (Convert.ToInt32(e.Value) == 8)
                        e.DisplayText = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateContabilizado);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaSolicitud.cs", "gvDocument_CustomColumnDisplayText: " + ex.Message));
            }
        }

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            this.gcDocRelacion.DataSource = null;
            this.gvDocRelacion.RefreshData();
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

            if (e.IsGetData)
            {
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
                        if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
                            e.Value = fi.GetValue(dto);
                        else
                            e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                    }
                }
            }
            if (e.IsSetData)
            {
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
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                        {
                            //e.Value = pi.GetValue(dto, null);
                        }
                        else
                        {
                            UDT udtProp = (UDT)fi.GetValue(dto);
                            udtProp.SetValueFromString(e.Value.ToString());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetalle_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

            if (e.IsGetData)
            {
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
                        if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
                            e.Value = fi.GetValue(dto);
                        else
                            e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                    }
                }
            }
            if (e.IsSetData)
            {
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
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                        {
                            //e.Value = pi.GetValue(dto, null);
                        }
                        else
                        {
                            UDT udtProp = (UDT)fi.GetValue(dto);
                            udtProp.SetValueFromString(e.Value.ToString());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetalle_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                this.gvDetalle.PostEditor();
                this.gvDocument.PostEditor();
                GridView view = (GridView)sender;
                //Obtiene Recibidos segun el identificador de la OC
                DTO_ConsultaComprasDet consultaDet = (DTO_ConsultaComprasDet)view.GetRow(view.FocusedRowHandle);
                List<DTO_prDetalleDocu> detalleRecib = this._bc.AdministrationModel.prDetalleDocu_GetByDocument(AppDocuments.Recibido, consultaDet.RecibidoDocuID.Value.Value);
                this._listDocumentsRelacion = new List<DTO_ConsultaDocRelacion>();
                if (detalleRecib.Count > 0)
                {
                    foreach (var det in detalleRecib)
                    {
                        if (!this._listDocumentsRelacion.Exists(x => x.FacturaDocuID.Value == det.FacturaDocuID.Value))
                        {
                            DTO_glDocumentoControl docCtrlFactura = this._bc.AdministrationModel.glDocumentoControl_GetByID(det.FacturaDocuID.Value.Value);
                            if (docCtrlFactura != null)
                            {
                                //Llena el detalle de cada factura
                                DTO_ConsultaDocRelacion consultaDoc = new DTO_ConsultaDocRelacion();
                                consultaDoc.FacturaDocuID.Value = docCtrlFactura.NumeroDoc.Value;
                                consultaDoc.PrefDoc.Value = docCtrlFactura.PrefDoc.Value;
                                consultaDoc.Fecha.Value = docCtrlFactura.FechaDoc.Value;
                                consultaDoc.ProveedorID.Value = docCtrlFactura.TerceroID.Value;
                                consultaDoc.FacturaNro.Value = docCtrlFactura.DocumentoTercero.Value;
                                DTO_coTercero tercero = (DTO_coTercero)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, docCtrlFactura.TerceroID.Value, true);
                                consultaDoc.ProveedorNombre.Value = tercero != null ? tercero.Descriptivo.Value : string.Empty;
                                consultaDoc.Cantidad.Value = detalleRecib.Sum(x => x.CantidadRec.Value);
                                consultaDoc.Valor.Value = docCtrlFactura.Valor.Value;
                                consultaDoc.MonedaID.Value = docCtrlFactura.MonedaID.Value;
                                this._listDocumentsRelacion.Add(consultaDoc);
                            }
                        }
                    }
                }
                this.gcDocRelacion.DataSource = this._listDocumentsRelacion;
            }
            catch (Exception ex)
            {
                this.gcDocRelacion.DataSource = null;
                this.gvDocRelacion.RefreshData();
            }
        }

        /// <summary>
        /// Evento que se presenta al dar clic una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetalle_RowClick(object sender, RowClickEventArgs e)
        {
            try
            {
                this.gvDetalle.PostEditor();
                this.gvDocument.PostEditor();
                GridView view = (GridView)sender;
                //Obtiene Recibidos segun el identificador de la OC
                DTO_ConsultaComprasDet consultaDet = (DTO_ConsultaComprasDet)view.GetRow(view.FocusedRowHandle);
                List<DTO_prDetalleDocu> detalleRecib = this._bc.AdministrationModel.prDetalleDocu_GetByDocument(AppDocuments.Recibido, consultaDet.RecibidoDocuID.Value.Value);
                this._listDocumentsRelacion = new List<DTO_ConsultaDocRelacion>();
                if (detalleRecib.Count > 0)
                {                   
                    foreach (var det in detalleRecib)
                    {
                        if (!this._listDocumentsRelacion.Exists(x => x.FacturaDocuID.Value == det.FacturaDocuID.Value))
                        {
                            DTO_glDocumentoControl docCtrlFactura = this._bc.AdministrationModel.glDocumentoControl_GetByID(det.FacturaDocuID.Value.Value);
                            if (docCtrlFactura != null)
                            {
                                //Llena el detalle de cada factura
                                DTO_ConsultaDocRelacion consultaDoc = new DTO_ConsultaDocRelacion();
                                consultaDoc.FacturaDocuID.Value = docCtrlFactura.NumeroDoc.Value;
                                consultaDoc.PrefDoc.Value = docCtrlFactura.PrefDoc.Value;
                                consultaDoc.Fecha.Value = docCtrlFactura.FechaDoc.Value;
                                consultaDoc.ProveedorID.Value = docCtrlFactura.TerceroID.Value;
                                consultaDoc.FacturaNro.Value = docCtrlFactura.DocumentoTercero.Value;
                                DTO_coTercero tercero = (DTO_coTercero)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, docCtrlFactura.TerceroID.Value, true);
                                consultaDoc.ProveedorNombre.Value = tercero != null ? tercero.Descriptivo.Value : string.Empty;
                                consultaDoc.Cantidad.Value = detalleRecib.Sum(x => x.CantidadRec.Value) ;
                                consultaDoc.Valor.Value = docCtrlFactura.Valor.Value;
                                consultaDoc.MonedaID.Value = docCtrlFactura.MonedaID.Value;
                                this._listDocumentsRelacion.Add(consultaDoc);
                            }
                        }
                    }                   
                }
                this.gcDocRelacion.DataSource = this._listDocumentsRelacion;
            }
            catch (Exception ex)
            {
                this.gcDocRelacion.DataSource = null;
                this.gvDocRelacion.RefreshData();
            }
        }

        #endregion

        #region Eventos Barra de Herramientas
        /// <summary>
        /// Boton para iniciar un nuevo documento
        /// </summary>
        public override void TBNew()
        {
            try
            {
                this._listDocuments = new List<DTO_ConsultaCompras>();
                this._listDocumentsRelacion = new List<DTO_ConsultaDocRelacion>();

                this.masterPrefijo.Value = string.Empty;
                this.txtNro.Text = string.Empty;
                this.gvDocument.ActiveFilterString = string.Empty;
                this.gcDocument.DataSource = null;
                this.gcDocRelacion.DataSource = null;
                this.gvDocument.RefreshData();
                this.gvDocRelacion.RefreshData();
                this.gbQueryDoc.Enabled = true;
                this.pgGrid.UpdatePageNumber(this._listDocuments.Count, false, true, false);
                this.masterPrefijo.Focus();
                this.masterPrefijo.Value = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Solicitud.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para importar datos la data actual
        /// </summary>
        public override void TBExport()
        {
            try
            {
                if (this._listDocuments.Count > 0)
                {
                    DataTableOperations tableOp = new DataTableOperations();
                    List<DTO_glMovimientoDeta> tmp = new List<DTO_glMovimientoDeta>();
                    System.Data.DataTable dataexcel = tableOp.Convert_GenericListToDataTable(typeof(DTO_ConsultaCompras), this._listDocuments);
                    List<string> colsRemove = new List<string>();
                    foreach (DataColumn col in dataexcel.Columns)
                    {
                        var columnsVisibles = this.gvDocument.VisibleColumns;
                        var colGrid = columnsVisibles.Where(x => x.FieldName == this._unboundPrefix + col.Caption).ToList();
                        if (colGrid.Count == 0)
                            colsRemove.Add(col.Caption);

                        if (col.Caption == "Estado")
                        {
                            for (int i = 0; i < col.Table.Rows.Count; i++)
                            {
                                if (col.Table.Rows[i]["Estado"].ToString() == "1")
                                    col.Table.Rows[i].SetField(col, "Sin Aprobar");
                                else if (col.Table.Rows[i]["Estado"].ToString() == "2")
                                    col.Table.Rows[i].SetField(col, "Para Aprobación");
                                else if (col.Table.Rows[i]["Estado"].ToString() == "3")
                                    col.Table.Rows[i].SetField(col, "Aprobado");
                                else if (col.Table.Rows[i]["Estado"].ToString() == "4")
                                    col.Table.Rows[i].SetField(col, "Revertido");
                                else if (col.Table.Rows[i]["Estado"].ToString() == "-1")
                                    col.Table.Rows[i].SetField(col, "Anulado");
                            }
                        }

                    }
                    foreach (var col in colsRemove)
                        dataexcel.Columns.Remove(col);

                    ReportExcelBase frm = new ReportExcelBase(dataexcel, this._documentID);
                    frm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaMovimiento.cs", "TBExport"));
            }
        }
        #endregion

    }
}
