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
    public partial class ModalStandar : Form
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();
        private int _documentID;
        private string unboundPrefix = "Unbound_";

        #endregion

        /// <summary>
        /// Constructor de la grilla de NotasEnvio 
        /// </summary>
        /// <param name="factResumen">Lista de notas envio que ya fueron cargados</param>
        public ModalStandar(int document, Dictionary<string, object> filter)
        {
            this.InitializeComponent();
            this.SetInitParameters(document);
            this.Loadata(filter);
        }

        #region Funciones privadas

        /// <summary>
        /// Genera la estructura de la grilla
        /// </summary>
        private void AddGridCols()
        {
            try
            {
               if(this._documentID == AppDocuments.PlaneacionCompras)
               {
                   this.lblTitle.Text = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_lblStockBodega");
                   #region Columnas
                   GridColumn BodegaID = new GridColumn();
                   BodegaID.FieldName = this.unboundPrefix + "BodegaID";
                   BodegaID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_BodegaID");
                   BodegaID.UnboundType = UnboundColumnType.String;
                   BodegaID.VisibleIndex = 0;
                   BodegaID.Width = 80;
                   BodegaID.Visible = true;
                   BodegaID.OptionsColumn.AllowEdit = false;
                   this.gvData.Columns.Add(BodegaID);

                   GridColumn BodegaDesc = new GridColumn();
                   BodegaDesc.FieldName = this.unboundPrefix + "BodegaDesc";
                   BodegaDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_BodegaDesc");
                   BodegaDesc.UnboundType = UnboundColumnType.String;
                   BodegaDesc.VisibleIndex = 1;
                   BodegaDesc.Width = 200;
                   BodegaDesc.Visible = true;
                   BodegaDesc.OptionsColumn.AllowEdit = false;
                   this.gvData.Columns.Add(BodegaDesc);

                   GridColumn CantidadDisp = new GridColumn();
                   CantidadDisp.FieldName = this.unboundPrefix + "CantidadDisp";
                   CantidadDisp.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CantidadDisp");
                   CantidadDisp.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                   CantidadDisp.AppearanceCell.Options.UseTextOptions = true;
                   CantidadDisp.UnboundType = UnboundColumnType.Decimal;
                   CantidadDisp.VisibleIndex = 2;
                   CantidadDisp.Width = 70;
                   CantidadDisp.Visible = true;
                   CantidadDisp.ColumnEdit = this.editCant2;
                   CantidadDisp.OptionsColumn.AllowEdit = false;
                   this.gvData.Columns.Add(CantidadDisp);

                   this.gvData.OptionsBehavior.Editable = true;
                   this.gvData.OptionsView.ColumnAutoWidth = true;
                   #endregion            
               }
               if (this._documentID == AppForms.EnvioCorreoClientes)
               {
                   this.lblTitle.Text = this._bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
                   #region Columnas
                   GridColumn ClienteID = new GridColumn();
                   ClienteID.FieldName = this.unboundPrefix + "ClienteID";
                   ClienteID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ClienteID");
                   ClienteID.UnboundType = UnboundColumnType.String;
                   ClienteID.VisibleIndex = 0;
                   ClienteID.Width = 100;
                   ClienteID.Visible = true;
                   ClienteID.OptionsColumn.AllowEdit = false;
                   this.gvData.Columns.Add(ClienteID);

                   GridColumn Nombre = new GridColumn();
                   Nombre.FieldName = this.unboundPrefix + "Nombre";
                   Nombre.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Nombre");
                   Nombre.UnboundType = UnboundColumnType.String;
                   Nombre.VisibleIndex = 1;
                   Nombre.Width = 200;
                   Nombre.Visible = true;
                   Nombre.OptionsColumn.AllowEdit = false;
                   this.gvData.Columns.Add(Nombre);

                   GridColumn Correo = new GridColumn();
                   Correo.FieldName = this.unboundPrefix + "Correo";
                   Correo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Correo");
                   Correo.UnboundType = UnboundColumnType.String;
                   Correo.VisibleIndex = 1;
                   Correo.Width = 100;
                   Correo.Visible = true;
                   Correo.OptionsColumn.AllowEdit = false;
                   this.gvData.Columns.Add(Correo);

                   GridColumn Libranza = new GridColumn();
                   Libranza.FieldName = this.unboundPrefix + "Libranza";
                   Libranza.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Libranza");
                   Libranza.UnboundType = UnboundColumnType.String;
                   Libranza.VisibleIndex = 1;
                   Libranza.Width = 100;
                   Libranza.Visible = true;
                   Libranza.OptionsColumn.AllowEdit = false;
                   this.gvData.Columns.Add(Libranza);

                   GridColumn ClienteInd = new GridColumn();
                   ClienteInd.FieldName = this.unboundPrefix + "ClienteInd";
                   ClienteInd.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ClienteInd");
                   ClienteInd.UnboundType = UnboundColumnType.Boolean;
                   ClienteInd.VisibleIndex = 1;
                   ClienteInd.Width = 50;
                   ClienteInd.Visible = true;
                   ClienteInd.OptionsColumn.AllowEdit = false;
                   this.gvData.Columns.Add(ClienteInd);

                   GridColumn ConyugeInd = new GridColumn();
                   ConyugeInd.FieldName = this.unboundPrefix + "ConyugeInd";
                   ConyugeInd.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ConyugeInd");
                   ConyugeInd.UnboundType = UnboundColumnType.Boolean;
                   ConyugeInd.VisibleIndex = 1;
                   ConyugeInd.Width = 50;
                   ConyugeInd.Visible = true;
                   ConyugeInd.OptionsColumn.AllowEdit = false;
                   this.gvData.Columns.Add(ConyugeInd);

                   GridColumn CodeudorInd = new GridColumn();
                   CodeudorInd.FieldName = this.unboundPrefix + "CodeudorInd";
                   CodeudorInd.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CodeudorInd");
                   CodeudorInd.UnboundType = UnboundColumnType.Boolean;
                   CodeudorInd.VisibleIndex = 1;
                   CodeudorInd.Width = 50;
                   CodeudorInd.Visible = true;
                   CodeudorInd.OptionsColumn.AllowEdit = false;
                   this.gvData.Columns.Add(CodeudorInd);

                   this.gvData.OptionsBehavior.Editable = true;
                   this.gvData.OptionsView.ColumnAutoWidth = true;
                   #endregion
               }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalStandar.cs", "LoadGridStructure"));
            }
        }

        /// <summary>
        /// Carga los datos de la grilla
        /// </summary>
        private void Loadata(Dictionary<string, object> filter)
        {
            try
            {
                if(this._documentID == AppDocuments.PlaneacionCompras)
                {
                    List<DTO_inControlSaldosCostos> mvtosxReferencia = new List<DTO_inControlSaldosCostos>();
                    List<DTO_inControlSaldosCostos> listSaldosCostosGrid = new List<DTO_inControlSaldosCostos>();
                    #region Carga las bodegas con la referenciaID
                    DTO_inControlSaldosCostos filterInv = new DTO_inControlSaldosCostos();
                    filterInv.inReferenciaID.Value = filter.First().Value.ToString();

                    foreach (DTO_inControlSaldosCostos inv in mvtosxReferencia.FindAll(x=>x.BodegaTipo.Value == (byte)TipoBodega.Stock))
                    {     
                        this.gcData.DataSource = listSaldosCostosGrid;                       
                    }
                    #endregion
                }
                if (this._documentID == AppForms.EnvioCorreoClientes)
                {
                    List<DTO_CorreoCliente> result = new List<DTO_CorreoCliente>();
                    foreach (var item in filter)
	                {
                        result.Add((DTO_CorreoCliente)item.Value);
	                }                    
                    this.gcData.DataSource = result;         
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalStandar.cs", "Loadata"));
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters(int document)
        {
            try
            {
                this._documentID = document;

                //Carga de datos
                this.AddGridCols();               
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalEstandar.cs", "SetInitParameters"));
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvData_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" ||
                        pi.PropertyType.Name == "Double")
                        e.Value = pi.GetValue(dto, null);
                    else
                        e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" ||
                            pi.PropertyType.Name == "Double")
                            e.Value = fi.GetValue(dto);
                        else
                            e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                    }
                }
                if (e.Value == null && pi != null && pi.PropertyType.Name == "UDT_Cantidad")
                    e.Value = 0;
            }
            if (e.IsSetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (e.Value == null)
                    e.Value = string.Empty;
                if (pi != null)
                {
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" ||
                        pi.PropertyType.Name == "Double")
                        e.Value = pi.GetValue(dto, null);
                    else if (pi.PropertyType.Name == "UDTSQL_smalldatetime" || pi.PropertyType.Name == "UDTSQL_datetime")
                    {
                        UDT udtProp = (UDT)pi.GetValue(dto, null);
                        udtProp.SetValueFromString(Convert.ToDateTime(e.Value).ToShortDateString());
                    }
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
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" ||
                            pi.PropertyType.Name == "Double")
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
        /// Abre la modal para detalle
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void editBtn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                //string refSelected = this.gvReferencia.GetRowCellValue(this.gvReferencia.FocusedRowHandle, this._unboundPrefix + "ID").ToString();
                //string BodegaSelected = this.gvBodega.GetRowCellValue(this.gvBodega.FocusedRowHandle, this._unboundPrefix + "BodegaID").ToString();
                //ModalConsultaInventario fact = new ModalConsultaInventario(refSelected, BodegaSelected, true, (this._userBodega != null ? this._userBodega.ConsultaCostosInd.Value.Value : false));
                //fact.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaBodega.cs", "editBtnDetail_ButtonClick: " + ex.Message));
            }
        }

        /// <summary>
        /// Cierra el form
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnAccept_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

    }
}
