using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Resultados;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using DevExpress.XtraGrid.Views.Grid;
using System.Threading;
using System.Globalization;
using NewAge.DTO.Attributes;
using System.Drawing;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de documentos
    /// </summary>
    public partial class ConsultaFlujoCaja : FormWithToolbar
    {
        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        //Variables Privadas
        private FormTypes _frmType = FormTypes.Document;
        private string _frmName;
        private int userID = 0;
        //Para manejo de propiedades
        private string empresaID = string.Empty;
        private int documentID;
        private ModulesPrefix frmModule;
        //Variables para importar
        private string unboundPrefix = "Unbound_";
        // Variables Formulario
        //Variables de datos
        private DTO_QueryFlujoCaja _rowCurrent = new DTO_QueryFlujoCaja();
        private GridView _gridDetalleCurrent = new GridView();
        private DTO_QueryFlujoCajaDetalle _rowTareaCurrent = new DTO_QueryFlujoCajaDetalle();
        private List<DTO_QueryFlujoCajaDetalle> _ListDetalle = new List<DTO_QueryFlujoCajaDetalle>();
        private List<DTO_QueryFlujoCaja> _listProyectos = new List<DTO_QueryFlujoCaja>();
        private string semana = string.Empty;
        private decimal sumamA = 0;
        private decimal sumam0 = 0;
        private decimal sumam1 = 0;
        private decimal sumam2 = 0;
        private decimal sumam3 = 0;
        private decimal sumam4 = 0;
        private decimal sumam5 = 0;
        private decimal sumam6 = 0;
        private decimal sumamM = 0;


        #endregion        

        ///<summary>
        /// Constructor 
        /// </summary>
        public ConsultaFlujoCaja()
        {
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());

                this.frmModule = ModulesPrefix.ts;

                FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaFlujoCaja.cs", "ConsultaFlujoCaja"));
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Add Columns in Grid
        /// </summary>
        private void AddGridCols()
        {

            #region Grilla Proyectos
            GridColumn Documento = new GridColumn();
            Documento.FieldName = this.unboundPrefix + "Documento";
            Documento.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Documento");
            Documento.UnboundType = UnboundColumnType.String;
            //Documento.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            //Documento.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Documento.AppearanceCell.Options.UseTextOptions = true;
            Documento.AppearanceCell.Options.UseFont = true;
            Documento.VisibleIndex = 1;
            Documento.Width = 90;
            Documento.Visible = true;
            Documento.OptionsColumn.AllowEdit = false;
            this.gvProyectos.Columns.Add(Documento);

            GridColumn PerA = new GridColumn();
            PerA.FieldName = this.unboundPrefix + "PerA";
            
            PerA.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PerA");
            PerA.UnboundType = UnboundColumnType.Decimal;
            PerA.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            PerA.VisibleIndex = 2;
            PerA.AppearanceCell.Options.UseTextOptions = true;          
            PerA.Width = 110;
            PerA.Visible = true;
            PerA.OptionsColumn.AllowEdit = false;
            PerA.ColumnEdit = this.editValue2;
            this.gvProyectos.Columns.Add(PerA);

            

            GridColumn Per0 = new GridColumn();
            Per0.FieldName = this.unboundPrefix + "Per0";
            this.semana = this._bc.AdministrationModel.Global_DiaSemana(0);
            Per0.Caption = this.semana.ToString();
            Per0.UnboundType = UnboundColumnType.Decimal;
            Per0.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Per0.AppearanceCell.Options.UseTextOptions = true;          
            Per0.VisibleIndex = 3;
            Per0.Width = 110;
            Per0.Visible = true;
            Per0.ColumnEdit = this.editValue2;
            Per0.OptionsColumn.AllowEdit = false;
            this.gvProyectos.Columns.Add(Per0);

            GridColumn Per1 = new GridColumn();
            Per1.FieldName = this.unboundPrefix + "Per1";
            this.semana = this._bc.AdministrationModel.Global_DiaSemana(1);
            Per1.Caption = this.semana.ToString();
            Per1.UnboundType = UnboundColumnType.Decimal;
            Per1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Per1.AppearanceCell.Options.UseTextOptions = true;            
            Per1.VisibleIndex = 4;
            Per1.Width = 110;
            Per1.Visible = true;
            Per1.OptionsColumn.AllowEdit = false;
            Per1.ColumnEdit = this.editValue2;
            this.gvProyectos.Columns.Add(Per1);

            GridColumn Per2 = new GridColumn();
            Per2.FieldName = this.unboundPrefix + "Per2";
            this.semana = this._bc.AdministrationModel.Global_DiaSemana(2);
            Per2.Caption = this.semana.ToString();
            Per2.UnboundType = UnboundColumnType.Decimal;
            Per2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Per2.AppearanceCell.Options.UseTextOptions = true;          
            Per2.VisibleIndex = 5;
            Per2.Width = 110;
            Per2.Visible = true;
            Per2.OptionsColumn.AllowEdit = false;
            Per2.ColumnEdit = this.editValue2;
            this.gvProyectos.Columns.Add(Per2);

            GridColumn Per3 = new GridColumn();
            Per3.FieldName = this.unboundPrefix + "Per3";
            this.semana = this._bc.AdministrationModel.Global_DiaSemana(3);
            Per3.Caption = this.semana.ToString();
            Per3.UnboundType = UnboundColumnType.Decimal;
            Per3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Per3.AppearanceCell.Options.UseTextOptions = true;          
            Per3.VisibleIndex = 6;
            Per3.Width = 110;
            Per3.Visible = true;
            Per3.OptionsColumn.AllowEdit = false;
            Per3.ColumnEdit = this.editValue2;
            this.gvProyectos.Columns.Add(Per3);

            GridColumn Per4 = new GridColumn();
            Per4.FieldName = this.unboundPrefix + "Per4";
            this.semana = this._bc.AdministrationModel.Global_DiaSemana(4);
            Per4.Caption = this.semana.ToString();
            Per4.UnboundType = UnboundColumnType.Decimal;
            Per4.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Per4.AppearanceCell.Options.UseTextOptions = true;            
            Per4.VisibleIndex = 7;
            Per4.Width = 110;
            Per4.Visible = true;
            Per4.OptionsColumn.AllowEdit = false;
            Per4.ColumnEdit = this.editValue2;
            this.gvProyectos.Columns.Add(Per4);

            GridColumn Per5 = new GridColumn();
            Per5.FieldName = this.unboundPrefix + "Per5";
            this.semana = this._bc.AdministrationModel.Global_DiaSemana(5);
            Per5.Caption = this.semana.ToString();
            Per5.UnboundType = UnboundColumnType.Decimal;            
            Per5.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Per5.AppearanceCell.Options.UseTextOptions = true;
            Per5.VisibleIndex = 8;
            Per5.Width = 110;
            Per5.Visible = true;
            Per5.OptionsColumn.AllowEdit = false;
            Per5.ColumnEdit = this.editValue2;
            this.gvProyectos.Columns.Add(Per5);

            GridColumn Per6 = new GridColumn();
            Per6.FieldName = this.unboundPrefix + "Per6";
            this.semana = this._bc.AdministrationModel.Global_DiaSemana(6);
            Per6.Caption = this.semana.ToString();
            Per6.UnboundType = UnboundColumnType.Decimal;
            Per6.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;           
            Per6.AppearanceCell.Options.UseTextOptions = true;
            Per6.AppearanceCell.Options.UseFont = true;
            Per6.AppearanceCell.Options.UseForeColor = true;
            Per6.VisibleIndex = 11;
            Per6.Width = 110;
            Per6.Visible = true;
            Per6.OptionsColumn.AllowEdit = false;
            Per6.ColumnEdit = this.editValue2;
            this.gvProyectos.Columns.Add(Per6);

            GridColumn PerM = new GridColumn();
            PerM.FieldName = this.unboundPrefix + "PerM";            
            PerM.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PerM");
            PerM.UnboundType = UnboundColumnType.Decimal;
            PerM.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            PerM.AppearanceCell.Options.UseTextOptions = true;
            PerM.AppearanceCell.Options.UseFont = true;
            PerM.AppearanceCell.Options.UseForeColor = true;
            PerM.VisibleIndex = 12;
            PerM.Width = 100;
            PerM.Visible = true;
            PerM.OptionsColumn.AllowEdit = false;
            PerM.ColumnEdit = this.editValue2;
            this.gvProyectos.Columns.Add(PerM);

            #endregion
            #region Grilla  Detalle
            //Documento Detalle
            GridColumn DocDetalle = new GridColumn();
            DocDetalle.FieldName = this.unboundPrefix + "Documento";
            DocDetalle.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_Documento");
            DocDetalle.UnboundType = UnboundColumnType.String;
            DocDetalle.VisibleIndex = 0;
            DocDetalle.Width = 90;
            DocDetalle.Visible = true;
            DocDetalle.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(DocDetalle);

            //terceroID
            GridColumn Tercero = new GridColumn();
            Tercero.FieldName = this.unboundPrefix + "Tercero";
            Tercero.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_Tercero");
            Tercero.UnboundType = UnboundColumnType.String;
            Tercero.VisibleIndex = 1;
            Tercero.Width = 100;
            Tercero.Visible = true;
            Tercero.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(Tercero);

            //Nombre
            GridColumn Nombre = new GridColumn();
            Nombre.FieldName = this.unboundPrefix + "Nombre";
            Nombre.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_Nombre");
            Nombre.UnboundType = UnboundColumnType.String;
            Nombre.VisibleIndex = 2;
            Nombre.Width = 200;
            Nombre.Visible = true;
            Nombre.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(Nombre);

            //Factura
            GridColumn Factura = new GridColumn();
            Factura.FieldName = this.unboundPrefix + "Factura";
            Factura.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_Factura");
            Factura.UnboundType = UnboundColumnType.String;
            Factura.VisibleIndex = 5;
            Factura.Width = 100;
            Factura.Visible = true;
            Factura.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(Factura);

            //FechaVto
            GridColumn FechaVto = new GridColumn();
            FechaVto.FieldName = this.unboundPrefix + "FechaVto";
            FechaVto.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaVto");
            FechaVto.UnboundType = UnboundColumnType.DateTime;
            FechaVto.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            FechaVto.AppearanceCell.Options.UseTextOptions = true;
            FechaVto.VisibleIndex = 6;
            FechaVto.Width = 100;
            FechaVto.Visible = true;
            FechaVto.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(FechaVto);

            //Saldo
            GridColumn Saldo = new GridColumn();
            Saldo.FieldName = this.unboundPrefix + "SaldoML";
            Saldo.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_SaldoML");
            Saldo.UnboundType = UnboundColumnType.Decimal;
            Saldo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            //Saldo.AppearanceCell.Font = new System.Drawing.Font("Arial Narrow", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Saldo.AppearanceCell.Options.UseTextOptions = true;
            Saldo.AppearanceCell.Options.UseFont = true;
            Saldo.VisibleIndex = 8;
            Saldo.Width = 100;
            Saldo.Visible = true;
            Saldo.ColumnEdit = this.editValue2Cant;
            Saldo.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(Saldo);

            #endregion
        }

        /// <summary>
        /// Carga la información
        /// </summary>
        /// <param name="firstTime"></param>
        private void LoadData()
        {
            try
            {
                this.dtFechaCorte.Enabled = false;
                this.dtFechaCorte.ReadOnly = true;
                this._listProyectos = this._bc.AdministrationModel.tsFlujoCaja(this.dtFechaCorte.DateTime);

                foreach (DTO_QueryFlujoCaja proy in this._listProyectos)
                {
                    if (proy.Documento.Value == "Recaudos")
                    {
                        this.sumamA += Convert.ToDecimal(proy.PerA.Value);
                        this.sumam0 += Convert.ToDecimal(proy.Per0.Value);
                        this.sumam1 += Convert.ToDecimal(proy.Per1.Value);
                        this.sumam2 += Convert.ToDecimal(proy.Per2.Value);
                        this.sumam3 += Convert.ToDecimal(proy.Per3.Value);
                        this.sumam4 += Convert.ToDecimal(proy.Per4.Value);
                        this.sumam5 += Convert.ToDecimal(proy.Per5.Value);
                        this.sumam6 += Convert.ToDecimal(proy.Per6.Value);
                        this.sumamM += Convert.ToDecimal(proy.PerM.Value);
                    }
                    else
                    {
                        this.sumamA -= Convert.ToDecimal(proy.PerA.Value);
                        this.sumam0 -= Convert.ToDecimal(proy.Per0.Value);
                        this.sumam1 -= Convert.ToDecimal(proy.Per1.Value);
                        this.sumam2 -= Convert.ToDecimal(proy.Per2.Value);
                        this.sumam3 -= Convert.ToDecimal(proy.Per3.Value);
                        this.sumam4 -= Convert.ToDecimal(proy.Per4.Value);
                        this.sumam5 -= Convert.ToDecimal(proy.Per5.Value);
                        this.sumam6 -= Convert.ToDecimal(proy.Per6.Value);
                        this.sumamM -= Convert.ToDecimal(proy.PerM.Value);
                    }
                    

                    this._ListDetalle = this._bc.AdministrationModel.tsFlujoCajaDetalle(proy.Documento.Value);
                }
                this.LoadGrids();
                this.lblPA.Text = this.sumamA.ToString("n0");
                this.lblP0.Text = this.sumam0.ToString("n0");
                this.lblP1.Text = this.sumam1.ToString("n0");
                this.lblP2.Text = this.sumam2.ToString("n0");
                this.lblP3.Text = this.sumam3.ToString("n0");
                this.lblP4.Text = this.sumam4.ToString("n0");
                this.lblP5.Text = this.sumam5.ToString("n0");
                this.lblP6.Text = this.sumam6.ToString("n0");
                this.lblPM.Text = this.sumamM.ToString("n0");

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaFlujoCaja", "LoadData"));
            }
        }

        /// <summary>
        /// Carga la información del detalle
        /// </summary>
        /// <param name="firstTime"></param>
        private void LoadGrids()
        {
            try
            {
                this.gcProyectos.DataSource = null;
                this.gcProyectos.DataSource = this._listProyectos;
                this.gcProyectos.RefreshDataSource();
                this.gcDetalle.RefreshDataSource();
                this.gcDetalle.DataSource = null;
                this.gcDetalle.DataSource = this._ListDetalle;
                this.gcDetalle.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionTiempo", "LoadData"));
            }
        }

        /// <summary>
        /// Carga la información de cada compra
        /// </summary>
        private void LoadGridDetalle()
        {
            try
            {
                this.gcDetalle.RefreshDataSource();
                this.gcDetalle.DataSource = null;
                this.gcDetalle.DataSource = this._ListDetalle;
                this.gcDetalle.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaFlujoCajaOld", "LoadReferencias"));
            }
        }
        /// <summary>
        /// Refrescar Formulario
        /// </summary>
        private void RefreshForm()
        {


            this.gcProyectos.DataSource = this._listProyectos;
            this.gcProyectos.RefreshDataSource();

            this.gcDetalle.DataSource = this._ListDetalle;
            this.gcDetalle.RefreshDataSource();

            //this.masterProyecto.Focus();
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            InitializeComponent();
            this.frmModule = ModulesPrefix.ts;
            this.documentID = AppQueries.QueryFlujoCaja;
            this.AddGridCols();

            this.dtFechaCorte.DateTime = DateTime.Now;
            this.LoadData();

            this.empresaID = _bc.AdministrationModel.Empresa.ID.Value;
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;
        }

        /// <summary>
        /// Valida una fila
        /// </summary>
        /// <param name="fila">Indice de la fila a validar</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        private bool ValidateRow(int fila)
        {
            return true;
        }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Enter(this, this.documentID, this._frmType, this.frmModule);
                FormProvider.Master.itemDelete.Visible = false;
                FormProvider.Master.itemCopy.Visible = false;
                FormProvider.Master.itemPaste.Visible = false;
                FormProvider.Master.itemRevert.Visible = false;
                FormProvider.Master.itemFilter.Visible = false;
                FormProvider.Master.itemFilterDef.Visible = false;
                FormProvider.Master.itemGenerateTemplate.Visible = false;
                FormProvider.Master.itemImport.Visible = false;
                FormProvider.Master.itemSendtoAppr.Visible = false;
                FormProvider.Master.itemExport.Visible = false;
                FormProvider.Master.itemImport.Visible = false;
                FormProvider.Master.itemSave.Visible = false;
                FormProvider.Master.itemPrint.Visible = false;
                FormProvider.Master.itemUpdate.Visible = true;
                FormProvider.Master.itemUpdate.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaFlujoCaja", "Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void Form_Leave(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Leave(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaFlujoCaja", "Form_Leave"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaFlujoCaja", "Form_Closing"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma se cierra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this.frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaFlujoCaja", "Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos Header Superior

        /// <summary>
        /// Evento que se ejecuta al salir del numero de documento (glDocumentoControl - NumeroDoc)
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtNumeroDoc_Leave(object sender, EventArgs e) { }

        /// <summary>
        /// Evento que se ejecuta al pararse sobre el control de fecha
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void dtFecha_Enter(object sender, EventArgs e) { }
       
        /// <summary>
        /// Valida que solo ingrese numeros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNumPrefix_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        #endregion

        #region Eventos Header
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtFechaCorte_EditValueChanged(object sender, EventArgs e)
        {
            if (this.gvProyectos.DataRowCount > 0)
            {

            }
        }

        #endregion

        #region Eventos Grilla

        #region Proyectos

        /// <summary>
        /// Se ejecutar cuando se selecciona un registro de la Grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDocument_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0)
                {

                    this._rowCurrent = (DTO_QueryFlujoCaja)this.gvProyectos.GetRow(e.FocusedRowHandle);

                    if (this._rowCurrent != null)
                    {

                        DTO_QueryFlujoCaja det = new DTO_QueryFlujoCaja();

                        this._ListDetalle = this._bc.AdministrationModel.tsFlujoCajaDetalle(this._rowCurrent.Documento.Value);
                    }
                    this.LoadGridDetalle();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaFlujoCaja.cs", "gvDocument_FocusedRowChanged"));
            }
        }

        private void gvProyectos_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            try
            {
                int semana = e.Column.VisibleIndex-2;
                if (e.RowHandle >= 0)
                {

                    this._rowCurrent = (DTO_QueryFlujoCaja)this.gvProyectos.GetRow(e.RowHandle);
                    
                    if (this._rowCurrent != null)
                    {
                        
                        DTO_QueryFlujoCaja det = new DTO_QueryFlujoCaja();

                        this._ListDetalle = this._bc.AdministrationModel.tsFlujoCajaDetalle(this._rowCurrent.Documento.Value);

                        if (semana<0)
                            this._ListDetalle = this._ListDetalle.FindAll(x => x.SemanaDif.Value < 0);
                        else
                            if (semana>6)
                                this._ListDetalle = this._ListDetalle.FindAll(x => x.SemanaDif.Value > 6);
                            else
                                this._ListDetalle = this._ListDetalle.FindAll(x => x.SemanaDif.Value == semana);
                        
                    }
                    this.LoadGridDetalle();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaFlujoCaja.cs", "gvProyectos_RowCellClick"));
            }
        }
        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName =!e.Column.FieldName.Equals("Editar")? e.Column.FieldName.Substring(this.unboundPrefix.Length) : e.Column.FieldName;

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


        #endregion

        /// <summary>
        /// Al entrar al link de la grilla para ver la descripcion de documento
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void editLink_Click(object sender, EventArgs e)
        {
            try
            {             
                //List<DTO_glDocumentoControl> ctrlsAnexos = this._bc.AdministrationModel.pyProyectoMvto_GetDocsAnexo(this._rowDetalle.ConsecMvto.Value);
                //ModalViewDocuments viewDocs = new ModalViewDocuments(ctrlsAnexos,Convert.ToByte(this.rdGroupVer.SelectedIndex));
                //viewDocs.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaFlujoCaja.cs", "editLink_Click"));
            }
        }

        /// <summary>
        /// Abre la modal para detalle
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>


        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Nuevo
        /// </summary>
        public override void TBNew()
        {
            this.RefreshForm();
        }

        /// <summary>
        /// Boton para actualizar la lista de documentos
        /// </summary>
        public override void TBUpdate()
        {
            try
            {
                this.LoadData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
