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
    public partial class ModalSolicitudes : Form
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();
        private int _documentID = AppForms.ModalSolicitudesForm;
        private DateTime _periodo;
        private List<DTO_prSolicitudResumen> _currentData;
        private DTO_prSolicitudResumen _currentRow;
        //private string _tercero;
        protected string unboundPrefix = "Unbound_";
        public bool ReturnVals = false;
        public List<DTO_prSolicitudResumen> ReturnList = new List<DTO_prSolicitudResumen>();
        public List<DTO_prSolicitudResumen> ReturnExistente = new List<DTO_prSolicitudResumen>();
        private bool isValid = true;
        private string _monedaLocal = string.Empty;
        private string _monedaExtranj = string.Empty;
        private TipoMoneda _tipoMda = TipoMoneda.Local;
        #endregion

        /// <summary>
        /// Constructor de la grilla de solicitudes 
        /// </summary>
        /// <param name="solResumen">Lista de solicitudes que ya fueron cargados</param>
        /// <param name="periodo">Periodo sobre el cual se van a consultar las solicitudes</param>
        public ModalSolicitudes(List<DTO_prSolicitudResumen> solResumen, DateTime periodo, int documentInvoke, TipoMoneda tipoMda)
        {
            //Inicializa el formulario
            InitializeComponent();
            FormProvider.LoadResources(this, this._documentID);

            //variables
            this._periodo = periodo;
            this._tipoMda = tipoMda;
            this._currentData = solResumen;
            foreach (DTO_prSolicitudResumen exist in solResumen.FindAll(x => x.NumeroDocOC.Value != null && x.NumeroDocOC.Value != 0))
                ReturnExistente.Add(exist);

            //Carga de datos
            this.AddGridCols();
            this.LoadGridData(documentInvoke);
        }

        #region Funciones privadas

        /// <summary>
        /// Genera la estructura de la grilla
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                //Campo de marca
                GridColumn sel = new GridColumn();
                sel.FieldName = this.unboundPrefix + "Selected";
                sel.Caption = "√";
                sel.ToolTip = _bc.GetResource(LanguageTypes.Forms, "Seleccionar");
                sel.AppearanceHeader.ForeColor = Color.Lime;
                sel.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                sel.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                sel.AppearanceHeader.Options.UseTextOptions = true;
                sel.AppearanceHeader.Options.UseFont = true;
                sel.AppearanceHeader.Options.UseForeColor = true;      
                sel.UnboundType = UnboundColumnType.Boolean;
                sel.VisibleIndex = 0;
                sel.Width = 20;
                sel.Fixed = FixedStyle.Left;
                sel.Visible = true;
                sel.OptionsColumn.AllowEdit = true;
                this.gvData.Columns.Add(sel);

                GridColumn prefDoc = new GridColumn();
                prefDoc.FieldName = this.unboundPrefix + "PrefDoc";
                prefDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_PrefDoc");
                prefDoc.UnboundType = UnboundColumnType.String;
                prefDoc.VisibleIndex = 1;
                prefDoc.Width = 55;
                prefDoc.Visible = true;
                prefDoc.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(prefDoc);

                //ProyectoID
                GridColumn ProyectoID = new GridColumn();
                ProyectoID.FieldName = this.unboundPrefix + "ProyectoID";
                ProyectoID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_ProyectoID");
                ProyectoID.UnboundType = UnboundColumnType.String;
                ProyectoID.VisibleIndex = 2;
                ProyectoID.Width = 50;
                ProyectoID.Visible = true;
                ProyectoID.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(ProyectoID);

                GridColumn DatoAdd4 = new GridColumn();
                DatoAdd4.FieldName = this.unboundPrefix + "DatoAdd4";
                DatoAdd4.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_DatoAdd4");
                DatoAdd4.UnboundType = UnboundColumnType.String;
                DatoAdd4.VisibleIndex = 3;
                DatoAdd4.Width = 50;
                DatoAdd4.Visible = true;
                DatoAdd4.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(DatoAdd4);

                GridColumn fecha = new GridColumn();
                fecha.FieldName = this.unboundPrefix + "Fecha";
                fecha.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_Fecha");
                fecha.UnboundType = UnboundColumnType.String;
                fecha.VisibleIndex = 4;
                fecha.Width = 50;
                fecha.Visible = true;
                fecha.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(fecha);

                //CodigoServicios
                GridColumn codBS = new GridColumn();
                codBS.FieldName = this.unboundPrefix + "CodigoBSID";
                codBS.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_CodigoBSID");
                codBS.UnboundType = UnboundColumnType.String;
                codBS.VisibleIndex = 5;
                codBS.Width = 50;
                codBS.Visible = true;
                codBS.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(codBS);

                //CodigoReferencia
                GridColumn codRef = new GridColumn();
                codRef.FieldName = this.unboundPrefix + "inReferenciaID";
                codRef.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_inReferenciaID");
                codRef.UnboundType = UnboundColumnType.String;
                codRef.VisibleIndex = 6;
                codRef.Width = 55;
                codRef.Visible = true;
                codRef.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(codRef);

                //Parameter1
                GridColumn param1 = new GridColumn();
                param1.FieldName = this.unboundPrefix + "Parametro1";
                param1.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_Parametro1");
                param1.UnboundType = UnboundColumnType.String;
                param1.VisibleIndex = 7;
                param1.Width = 20;
                param1.Visible = false;
                param1.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(param1);

                //Parameter2
                GridColumn param2 = new GridColumn();
                param2.FieldName = this.unboundPrefix + "Parametro2";
                param2.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_Parametro2");
                param2.UnboundType = UnboundColumnType.String;
                param2.VisibleIndex = 8;
                param2.Width = 20;
                param2.Visible = false;
                param2.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(param2);

                //Descriptivo
                GridColumn desc = new GridColumn();
                desc.FieldName = this.unboundPrefix + "Descriptivo";
                desc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_Descriptivo");
                desc.UnboundType = UnboundColumnType.String;
                desc.VisibleIndex = 9;
                desc.Width = 240;
                desc.Visible = true;
                desc.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(desc);

                //MarcaInvID
                GridColumn MarcaInvID = new GridColumn();
                MarcaInvID.FieldName = this.unboundPrefix + "MarcaInvID";
                MarcaInvID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_MarcaInvID");
                MarcaInvID.UnboundType = UnboundColumnType.String;
                MarcaInvID.VisibleIndex = 10;
                MarcaInvID.Width = 35;
                MarcaInvID.Visible = true;
                MarcaInvID.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(MarcaInvID);

                //RefProveedor
                GridColumn RefProveedor = new GridColumn();
                RefProveedor.FieldName = this.unboundPrefix + "RefProveedor";
                RefProveedor.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_RefProveedor");
                RefProveedor.UnboundType = UnboundColumnType.String;
                RefProveedor.VisibleIndex = 11;
                RefProveedor.Width = 35;
                RefProveedor.Visible = true;
                RefProveedor.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(RefProveedor);

                //UnidadInvID
                GridColumn unidad = new GridColumn();
                unidad.FieldName = this.unboundPrefix + "UnidadInvID";
                unidad.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_UnidadInvID");
                unidad.UnboundType = UnboundColumnType.String;
                unidad.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                unidad.AppearanceCell.Options.UseTextOptions = true;
                unidad.VisibleIndex = 12;
                unidad.Width = 35;
                unidad.Visible = true;
                unidad.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(unidad);

                //OrigenMonetario
                GridColumn OrigenMonetario = new GridColumn();
                OrigenMonetario.FieldName = this.unboundPrefix + "OrigenMonetario";
                OrigenMonetario.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_OrigenMonetario");
                OrigenMonetario.UnboundType = UnboundColumnType.String;
                OrigenMonetario.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                OrigenMonetario.AppearanceCell.Options.UseTextOptions = true;
                OrigenMonetario.VisibleIndex = 13;
                OrigenMonetario.Width = 35;
                OrigenMonetario.Visible = true;
                OrigenMonetario.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(OrigenMonetario);

                //Cantidad Solicitud
                GridColumn cant = new GridColumn();
                cant.FieldName = this.unboundPrefix + "CantidadSol";
                cant.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_CantidadSol");
                cant.UnboundType = UnboundColumnType.Decimal;
                cant.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                cant.AppearanceCell.Options.UseTextOptions = true;
                cant.VisibleIndex = 14;
                cant.Width = 60;
                cant.Visible = true;
                cant.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(cant);

                //Cantidad Orden de Compra
                GridColumn cantOrd = new GridColumn();
                cantOrd.FieldName = this.unboundPrefix + "CantidadOrdenComp";
                cantOrd.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_CantidadOrdenComp");
                cantOrd.UnboundType = UnboundColumnType.Decimal;
                cantOrd.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                cantOrd.AppearanceCell.Options.UseTextOptions = true;
                cantOrd.VisibleIndex = 15;
                cantOrd.Width = 60;
                cantOrd.Visible = true;
                cantOrd.OptionsColumn.AllowEdit = true;
                this.gvData.Columns.Add(cantOrd);

                this.gvData.OptionsView.ColumnAutoWidth = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalSolicitudes.cs", "LoadGridStructure"));
            }
        }

        /// <summary>
        /// Carga los datos de la grilla
        /// </summary>
        private void LoadGridData(int documentInvoke)
        {
            try
            {
                List<DTO_prSolicitudResumen> solicitudes = _bc.AdministrationModel.Solicitud_GetResumen(documentInvoke, this._bc.AdministrationModel.User, ModulesPrefix.pr, this._tipoMda);
                List<DTO_prSolicitudResumen> newData = new List<DTO_prSolicitudResumen>();

                //Carga la informacion de las facturas de acuerdo con el tipo de moneda
                solicitudes.ForEach(newSol =>
                {
                    #region Revisa las facturas que ya estan asignados
                    this._currentData.ForEach(sol =>
                    {
                        bool found = false;
                        if
                        (!found && sol.NumeroDoc.Value == newSol.NumeroDoc.Value && sol.ConsecutivoDetaID.Value == newSol.ConsecutivoDetaID.Value)
                        {
                            found = true;
                            newSol.Selected.Value = true;
                            newSol.CantidadSol.Value = newSol.CantidadSolTOT.Value;
                            newSol.CantidadOrdenComp.Value = sol.CantidadOrdenComp.Value;
                            newSol.ValorUni.Value = (sol.ValorUni.Value != null) ? sol.ValorUni.Value.Value : 0;
                        }
                    });
                    #endregion
                    newData.Add(newSol);
                });
                this._currentData = solicitudes;
                this._currentData = this._currentData.OrderBy(x => x.Descriptivo.Value).ToList();
                this.gcData.DataSource = this._currentData;
                if (solicitudes.Count > 0)
                    this.gvData.OptionsView.ShowAutoFilterRow = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalSolicitudes.cs", "LoadGridData"));
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

            if (fieldName.Contains("Cantidad"))
            {
                e.RepositoryItem = this.editSpinDecimal;
            }
        }

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
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32")
                    {
                        e.Value = pi.GetValue(dto, null);
                    }
                    else
                    {
                        e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                    }
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32")
                        {
                            e.Value = fi.GetValue(dto);
                        }
                        else
                        {
                            e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                        }
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
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32")
                    {
                        e.Value = pi.GetValue(dto, null);
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
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32")
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
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvData_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            this._currentRow = (DTO_prSolicitudResumen)this.gvData.GetRow(e.RowHandle);
            if (this._currentRow != null)
            {
                if (fieldName == "Selected")
                {
                    GridColumn col = new GridColumn();
                    col = this.gvData.Columns[this.unboundPrefix + "CantidadOrdenComp"];

                    if (Convert.ToBoolean(e.Value))
                        this.gvData.SetRowCellValue(e.RowHandle, col, this._currentRow.CantidadSol.Value);
                    else
                        this.gvData.SetRowCellValue(e.RowHandle, col, 0);
                }

                if (fieldName == "CantidadOrdenComp")
                {
                    if (this._currentRow.Selected.Value.Value && (Convert.ToDecimal(e.Value) > this._currentRow.CantidadSol.Value.Value || Convert.ToDecimal(e.Value) <= 0))
                    {
                        this.gvData.SetColumnError(e.Column, "Cantidad invalida");
                        this.isValid = false;
                    }
                    else
                    {
                        this.gvData.SetColumnError(e.Column, string.Empty);
                        this.isValid = true;
                    }
                } 
            }
        }

        /// <summary>
        /// Evento que se ejecuta al cambiar el foco de una columna a otra
        /// Cambia el estilo de una celda segun las condiciones del formulario
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvData_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            bool editableCell = false;
            string fieldName = this.gvData.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);

            if (fieldName == "CantidadOrdenComp")
                editableCell = true;

            if (editableCell)
                this.gvData.Appearance.FocusedCell.BackColor = Color.White;
            else
                this.gvData.Appearance.FocusedCell.BackColor = Color.Lavender;
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvData_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            if (!this.isValid)
                e.Allow = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvData_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string fieldName = this.gvData.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "CantidadOrdenComp")
            {
                if (!this._currentRow.Selected.Value.Value)
                    e.Cancel = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvData_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                string fieldName = this.gvData.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
                if (e.FocusedRowHandle >= 0 && this.gvData.DataRowCount > 0)
                    this._currentRow = (DTO_prSolicitudResumen)this.gvData.GetRow(e.FocusedRowHandle);
            }
            catch (Exception ex)
            {                
                 MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalSolicitudes.cs", "gvData_FocusedRowChanged"));
            }         
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvData_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == this.unboundPrefix + "OrigenMonetario")
                {
                    if (Convert.ToByte(e.Value) == 1)
                        e.DisplayText = "Loc";
                    else if (Convert.ToByte(e.Value) == 2)
                        e.DisplayText = "Ext";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RadicacionRecibido.cs", "gvDetalle_CustomColumnDisplayText"));
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
                if (!this.gvData.HasColumnErrors)
                {
                    this.ReturnVals = true;
                    this._currentData.ForEach(a =>
                    {
                        if (a.Selected.Value.Value)
                        {
                            if (a.CantidadOrdenComp.Value > 0 && a.CantidadOrdenComp.Value <= a.CantidadSol.Value)
                            {
                                this.ReturnList.Add(a);
                            }
                            else
                            {
                                string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_InvalidSolCantidad) + "-" + a.Descriptivo.Value;
                                MessageBox.Show(string.Format(msg, a.PrefDoc.ToString()));
                            }
                        }
                    });

                    //Recorre los detalles que ya existen y agrega los nuevos
                    foreach (DTO_prSolicitudResumen exist in ReturnExistente)
                    {
                        //Busca en los seleccionados si existe el item ya guardado en bd
                        DTO_prSolicitudResumen sol = this.ReturnList.Find(x=>x.inReferenciaID.Value == exist.inReferenciaID.Value && x.CodigoBSID.Value == exist.CodigoBSID.Value &&
                                                       x.Documento1ID.Value == exist.Documento1ID.Value && x.Detalle1ID.Value == exist.Detalle1ID.Value &&
                                                       x.Documento2ID.Value == exist.Documento2ID.Value && x.Detalle2ID.Value == exist.Detalle2ID.Value &&
                                                       x.Documento3ID.Value == exist.Documento3ID.Value && x.Detalle3ID.Value == exist.Detalle3ID.Value &&
                                                       x.Documento4ID.Value == exist.Documento4ID.Value && x.Detalle4ID.Value == exist.Detalle4ID.Value &&
                                                       x.Documento5ID.Value == exist.Documento5ID.Value && x.Detalle5ID.Value == exist.Detalle5ID.Value);
                        if (sol == null)
                            this.ReturnList.Add(exist);
                        else
                            sol.NumeroDocOC.Value = ReturnExistente.First().NumeroDocOC.Value;
                        //else //Si existe suma la cantidad
                        //    sol.CantidadOrdenComp.Value += (exist.CantidadOrdenComp.Value.HasValue ? exist.CantidadOrdenComp.Value : 0);
                    }

                    this.Close(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalSolicitudes.cs", "btnReturn_Click"));
            }
        }

        /// <summary>
        /// Se realiza al cambiar el valor del control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void chkSelect_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < this.gvData.DataRowCount; i++)
                {
                    DTO_prSolicitudResumen row = (DTO_prSolicitudResumen)this.gvData.GetRow(i);
                    row.Selected.Value = this.chkSelect.Checked;

                    // sol.Selected.Value = this.chkSelect.Checked;
                    if (row.Selected.Value.Value)
                        row.CantidadOrdenComp.Value = row.CantidadSol.Value;
                    else
                        row.CantidadOrdenComp.Value = 0;
                }
            }
            catch (Exception ex)
            {
                  MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalSolicitudes.cs", "chkSelect_CheckedChanged"));
            }
            this.gcData.RefreshDataSource();
        }

        #endregion

    



      
    }
}
