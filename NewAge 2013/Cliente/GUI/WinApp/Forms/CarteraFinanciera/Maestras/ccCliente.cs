using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Clases;
using System.Windows.Forms;
using NewAge.Librerias.Project;
using NewAge.DTO.Resultados;
using NewAge.DTO.UDT;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraEditors;
using DevExpress.XtraVerticalGrid.Events;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    class ccCliente : MasterSimpleForm
    {
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();

        ///<summary>
        /// Constructor 
        /// </summary>
        public ccCliente() : base() { }

        ///<summary>
        /// Constructor 
        /// </summary>
        public ccCliente(string mod) : base(mod) { }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.ccCliente;
            base.InitForm();
        }

        #region Eventos grilla edicion

        /// <summary>
        /// Asigna un editor de celda(button, check, textbox..) a la celda relacionada del index
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvRecordEdit_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (!this.Insertando) {
                FieldConfiguration newFC2 = this.GetFieldConfigByFieldName("PuntosTOT");
                FieldConfiguration newFC3 = this.GetFieldConfigByFieldName("AbogadoID");
                FieldConfiguration newFC4 = this.GetFieldConfigByFieldName("AbogadoDesc");
                FieldConfiguration newFC5 = this.GetFieldConfigByFieldName("FechaINIEstado");
                FieldConfiguration newFC6 = this.GetFieldConfigByFieldName("EstadoCartera");
                FieldConfiguration newFC7 = this.GetFieldConfigByFieldName("FechaRetiro");
                FieldConfiguration newFC8 = this.GetFieldConfigByFieldName("FechaIngreso");
                newFC2.Editable = true;
                newFC3.Editable = true;
                newFC4.Editable = true;
                newFC5.Editable = true;
                newFC6.Editable = true;
                newFC7.Editable = true;
                newFC8.Editable = true;
            }
            this.viewBeingEdited = null;
            this.viewBeingEdited = this.GetFocusedGridView();

            GridProperty gpHandle = (GridProperty)e.Column.View.GetFocusedRow();
            FieldConfiguration config = this.GetFieldConfigByCaption(gpHandle.Campo);

            if (config.FieldName == "ResidenciaDir")
            {
                try
                {

                    TextFieldConfiguration bec = (TextFieldConfiguration)config;
                    btnRecordEdit.Mask.MaskType = MaskType.RegEx;
                    btnRecordEdit.Mask.EditMask = bec.Regex;
                    btnRecordEdit.MaxLength = bec.MaxLength;
                    btnRecordEdit.ReadOnly = true;
                    btnRecordEdit.Buttons[0].Enabled = true;
                    e.RepositoryItem = this.btnRecordEdit;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                base.gvRecordEdit_CustomRowCellEditForEditing(sender, e);
            
        }

        /// <summary>
        /// Evento que se genera al hacer click en un grupo 
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>
        /// <param name="e">Argumentos del evento</param>        
        protected override void btnRecordEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FieldConfiguration config = this.GetFocusedFieldConfig();
            if (config.FieldName == "ResidenciaDir")
            {
                try
                {
                    this.viewBeingEdited = this.GetFocusedGridView();

                    ButtonEdit origin = (ButtonEdit)sender;
                    CodificacionDireccion cod = new CodificacionDireccion(origin);
                    cod.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                base.btnRecordEdit_ButtonClick(sender, e);
        }

        #endregion

        #region Validaciones sobre el cliente
        /// <summary>
        /// Sobrecargar para procesar el cambio en un campo especifico de la grilla de edición
        /// </summary>
        /// <param name="Field">Nombre del campo en la grilla de edición (caption)</param>
        /// <param name="Value">Valor ingresado</param>
        /// <param name="RowIndex">Numero de la fila en la grilla de edición</param>
        protected override bool FieldValidate(string Field, object Value, int RowIndex, out string err)
        {
            bool res = true;
            err = string.Empty;
            FieldConfiguration fc = this.GetFieldConfigByCaption(Field);
            string terceroID = Value.ToString();
            DTO_coTercero tercero = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, terceroID, true);

            //Asigna Descripcion
            if (fc.FieldName == "TerceroID")
            {
                if (tercero != null)
                {
                    FieldConfiguration newFC1 = this.GetFieldConfigByFieldName("Descriptivo");
                    this.SetEditGridValue(newFC1.RowIndex, tercero.Descriptivo);
                    newFC1.Editable = false;

                    //Asigna Direccion
                    FieldConfiguration newFC2 = this.GetFieldConfigByFieldName("ResidenciaDir");
                    //if (string.IsNullOrWhiteSpace(this.GetEditRow(newFC2.ColumnIndex).Valor.ToString()))
                        this.SetEditGridValue(newFC2.RowIndex, tercero.Direccion);

                    //Asigna teléfono
                    FieldConfiguration newFC3 = this.GetFieldConfigByFieldName("Telefono");
                    //if (string.IsNullOrWhiteSpace(this.GetEditRow(newFC3.ColumnIndex).Valor.ToString()))
                        this.SetEditGridValue(newFC3.RowIndex, tercero.Tel1);


                    //Asigna Cel
                    FieldConfiguration newFC4 = this.GetFieldConfigByFieldName("Celular");
                    //if (string.IsNullOrWhiteSpace(this.GetEditRow(newFC4.ColumnIndex).Valor.ToString()))
                        this.SetEditGridValue(newFC4.RowIndex, tercero.Tel2);

                    //Asigna Correo Electronico
                    FieldConfiguration newFC5 = this.GetFieldConfigByFieldName("Correo");
                    //if (string.IsNullOrWhiteSpace(this.GetEditRow(newFC5.ColumnIndex).Valor.ToString()))
                        this.SetEditGridValue(newFC5.RowIndex, tercero.CECorporativo);

                }
            }
            return res;
        }
        public override void TBNew() {
                base.TBNew();
                FieldConfiguration newFC1 = this.GetFieldConfigByFieldName("ClienteTipo");
                this.SetEditGridValue(newFC1.RowIndex, 1);
                FieldConfiguration newFC2 = this.GetFieldConfigByFieldName("PuntosTOT");
                FieldConfiguration newFC3 = this.GetFieldConfigByFieldName("AbogadoID");
                FieldConfiguration newFC4 = this.GetFieldConfigByFieldName("AbogadoDesc");
                FieldConfiguration newFC5 = this.GetFieldConfigByFieldName("FechaINIEstado");
                FieldConfiguration newFC6 = this.GetFieldConfigByFieldName("EstadoCartera");
                FieldConfiguration newFC7 = this.GetFieldConfigByFieldName("FechaRetiro");
                FieldConfiguration newFC8 = this.GetFieldConfigByFieldName("FechaIngreso");
                newFC2.Editable = false;
                newFC3.Editable = false;
                newFC4.Editable = false;
                newFC5.Editable = false;
                newFC6.Editable = false;
                newFC7.Editable = false;
                newFC8.Editable = false;
        }
        #endregion
    }
        
}
