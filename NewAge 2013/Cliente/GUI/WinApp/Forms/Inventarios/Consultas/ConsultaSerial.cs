using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ConsultaSerial : FormWithToolbar
    {
        #region Variables
        private int _documentID;
        private ModulesPrefix _frmModule;
        private BaseController _bc = BaseController.GetInstance();
        private string _unboundPrefix = "Unbound_";
        private List<DTO_inQuerySeriales> _Data;
        #endregion

        public ConsultaSerial()
        {
            InitializeComponent();
            this.SetInitParameters();
        }

        #region Funciones Privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentID = AppQueries.QuerySeriales;
            this._frmModule = ModulesPrefix.@in;
            FormProvider.LoadResources(this, this._documentID);
            this.AddGridCols();
            this.Initontrols();
            //Carga la informacion de la maestras
            //_bc.InitMasterUC(this.masterCliente, AppMasters.ccCliente, false, true, true, false);
        }

        /// <summary>
        /// Función que carga el dataSource de la grilla
        /// </summary>
        private void LoadData()
        {
            this.gcDetail.DataSource = this._Data;
            this.gcDetail.RefreshDataSource();
        }

        /// <summary>
        /// Inicia los controlesdel form
        /// </summary>
        private void Initontrols()
        {
            //Inicia los controles Master Find
            this._bc.InitMasterUC(this.uc_MF_Bodega, AppMasters.inBodega, true, true, true, false);
            this._bc.InitMasterUC(this.uc_MF_Referencia, AppMasters.inReferencia, true, true, true, false);
            this._bc.InitMasterUC(this.uc_MF_Cliente, AppMasters.coTercero, true, true, true, false);

            //FormProvider.Master.itemPrint.Enabled = true;
        }

        /// <summary>
        /// Inicia las columnas de las grillas
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                //serial
                GridColumn serial = new GridColumn();
                serial.FieldName = this._unboundPrefix + "SerialID";
                serial.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Serial");
                serial.UnboundType = UnboundColumnType.String;
                serial.VisibleIndex = 1;
                serial.Width = 50;
                serial.Visible = true;
                serial.OptionsColumn.AllowEdit = false;
                this.gvDetail.Columns.Add(serial);

                //Bodega
                GridColumn bodega = new GridColumn();
                bodega.FieldName = this._unboundPrefix + "BodegaID";
                bodega.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Bodega");
                bodega.UnboundType = UnboundColumnType.DateTime;
                bodega.VisibleIndex = 0;
                bodega.Width = 40;
                bodega.Visible = true;
                bodega.OptionsColumn.AllowEdit = false;
                this.gvDetail.Columns.Add(bodega);

                //Referencia
                GridColumn referencia = new GridColumn();
                referencia.FieldName = this._unboundPrefix + "InReferenciaID";
                referencia.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Referencia");
                referencia.UnboundType = UnboundColumnType.String;
                referencia.VisibleIndex = 0;
                referencia.Width = 50;
                referencia.Visible = true;
                referencia.OptionsColumn.AllowEdit = false;
                this.gvDetail.Columns.Add(referencia);

                //Stand
                GridColumn stand = new GridColumn();
                stand.FieldName = this._unboundPrefix + "Stand";
                stand.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Stand");
                stand.UnboundType = UnboundColumnType.String;
                stand.VisibleIndex = 0;
                stand.Width = 30;
                stand.Visible = true;
                stand.OptionsColumn.AllowEdit = false;
                this.gvDetail.Columns.Add(stand);

                //Caja
                GridColumn caja = new GridColumn();
                caja.FieldName = this._unboundPrefix + "Caja";
                caja.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Caja");
                caja.UnboundType = UnboundColumnType.Decimal;
                caja.VisibleIndex = 0;
                caja.Width = 30;
                caja.Visible = true;
                caja.OptionsColumn.AllowEdit = false;
                this.gvDetail.Columns.Add(caja);

                //Documento
                GridColumn documento = new GridColumn();
                documento.FieldName = this._unboundPrefix + "Documento";
                documento.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Documento");
                documento.UnboundType = UnboundColumnType.String;
                documento.Width = 40;
                documento.VisibleIndex = 10;
                documento.AppearanceHeader.Font = new Font("Arial", 6.50F, FontStyle.Bold, GraphicsUnit.Pixel);
                documento.Visible = true;
                documento.OptionsColumn.AllowEdit = false;
                this.gvDetail.Columns.Add(documento);

                //Soporte
                GridColumn soporte = new GridColumn();
                soporte.FieldName = this._unboundPrefix + "DocSoporte";
                soporte.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Soporte");
                soporte.UnboundType = UnboundColumnType.String;
                soporte.Width = 40;
                soporte.VisibleIndex = 10;
                soporte.AppearanceHeader.Font = new Font("Arial", 6.50F, FontStyle.Bold, GraphicsUnit.Pixel);
                soporte.Visible = true;
                soporte.OptionsColumn.AllowEdit = false;
                this.gvDetail.Columns.Add(soporte);

                //Fecha
                GridColumn fecha = new GridColumn();
                fecha.FieldName = this._unboundPrefix + "Periodo";
                fecha.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Fecha");
                fecha.UnboundType = UnboundColumnType.DateTime;
                fecha.Width = 40;
                fecha.VisibleIndex = 10;
                fecha.AppearanceHeader.Font = new Font("Arial", 6.50F, FontStyle.Bold, GraphicsUnit.Pixel);
                fecha.Visible = true;
                fecha.OptionsColumn.AllowEdit = false;
                this.gvDetail.Columns.Add(fecha);

                //Tipo
                GridColumn tipo = new GridColumn();
                tipo.FieldName = this._unboundPrefix + "Tipo";
                tipo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Tipo");
                tipo.UnboundType = UnboundColumnType.String;
                tipo.Width = 20;
                tipo.VisibleIndex = 10;
                tipo.AppearanceHeader.Font = new Font("Arial", 6.50F, FontStyle.Bold, GraphicsUnit.Pixel);
                tipo.Visible = true;
                tipo.OptionsColumn.AllowEdit = false;
                this.gvDetail.Columns.Add(tipo);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-", "ChequesGirados.cs-AddGridCols"));
            }
        }
        #endregion

        #region Eventos Grilla

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetail_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
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


        #endregion

        #region Eventos Formulario

        /// <summary>
        /// Evento que consulta la información.
        /// </summary>
        /// <param name="sender">Evento</param>
        /// <param name="e"></param>
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                this._Data = this._bc.AdministrationModel.inSaldosExistencias_GetBySerial(this.txt_Serial.Text, this.uc_MF_Bodega.Value, this.uc_MF_Referencia.Value, this.uc_MF_Cliente.Value);
                if (this._Data.Count != null)
                    this.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinAppConsultaSeriales.cs", "Consulta Serial: " + ex.Message));
                throw;
            }
        }
        #endregion
    }
}
