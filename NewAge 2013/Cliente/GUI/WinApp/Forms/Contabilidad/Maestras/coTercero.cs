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
using System.Reflection;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    class coTercero : MasterSimpleForm
    {
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();

        ///<summary>
        /// Constructor 
        /// </summary>
        public coTercero() : base() { }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.coTercero;
            base.InitForm();
        }

        #region Validaciones Del formulario

        /// <summary>
        /// Carga los datos iniciales de una maestra
        /// </summary>
        /// <param name="fieldName">Nombre del campo</param>
        /// <returns>Retorna el valor que debe ir en el campo</returns>
        protected override string NewRecordLoadData(string fieldName)
        {
            string res = string.Empty;
            if (fieldName == "RegimenFiscalEmpresaID")
            {
                res = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_RegimenFiscalXDefecto);
            }
            FieldConfiguration newFC1 = this.GetFieldConfigByFieldName("ApellidoSdo");
            FieldConfiguration newFC2 = this.GetFieldConfigByFieldName("NombrePri");
            FieldConfiguration newFC3 = this.GetFieldConfigByFieldName("NombreSdo");
            newFC1.Editable = true;
            newFC2.Editable = true;
            newFC3.Editable = true;

            return res;
        }

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

            //Valida los campos referentes a Bancos
            if (fc.FieldName == "BancoID_1")
            {
                string v = Value.ToString();

                int bancoDoc = AppMasters.tsBanco;
                DTO_tsBanco banco = (DTO_tsBanco)_bc.GetMasterDTO(AppMasters.MasterType.Simple, bancoDoc, false, v, true);
                FieldConfiguration newFC1 = this.GetFieldConfigByFieldName("CuentaTipo_1");
                FieldConfiguration newFC2 = this.GetFieldConfigByFieldName("BcoCtaNro_1");
                //FieldConfiguration newFC3 = this.GetFieldConfigByFieldName("BancoID_1");
                if (banco != null && newFC1 != null && newFC2 != null)
                {
                    newFC1.Editable = true;
                    newFC1.AllowNull = false;
                    newFC2.Editable = true;
                    newFC2.AllowNull = false;
                }
                else
                {
                    newFC1.Editable = false;
                    newFC1.AllowNull = true;
                    newFC2.Editable = false;
                    newFC2.AllowNull = true;
                    this.SetEditGridValue(newFC1.RowIndex, string.Empty);
                    this.SetEditGridValue(newFC2.RowIndex, string.Empty);
                    //this.SetEditGridValue(newFC3.RowIndex, string.Empty);
                }
            }

            //Valida tipo de Cuenta
            if (fc.FieldName == "CuentaTipo_1")
            {
                int v = Convert.ToInt32(Value);

                FieldConfiguration newFC1 = this.GetFieldConfigByFieldName("BcoCtaNro_1");
                newFC1.Editable = true;
                if(v==3)
                {
                    newFC1.Editable = false;
                    newFC1.AllowNull = true;
                }

            }

            //Valida los campos referentes a Bancos
            if (fc.FieldName == "BancoID_2")
            {
                string v = Value.ToString();

                int bancoDoc = AppMasters.tsBanco;
                DTO_tsBanco banco = (DTO_tsBanco)_bc.GetMasterDTO(AppMasters.MasterType.Simple, bancoDoc, false, v, true);
                FieldConfiguration newFC1 = this.GetFieldConfigByFieldName("CuentaTipo_2");
                FieldConfiguration newFC2 = this.GetFieldConfigByFieldName("BcoCtaNro_2");
                if (banco != null && newFC1 != null && newFC2 != null)
                {
                    newFC1.Editable = true;
                    newFC1.AllowNull = false;
                    newFC2.Editable = true;
                    newFC2.AllowNull = false;
                }
                else
                {
                    newFC1.Editable = false;
                    newFC1.AllowNull = true;
                    newFC2.Editable = false;
                    newFC2.AllowNull = true;
                    this.SetEditGridValue(newFC1.RowIndex, string.Empty);
                    this.SetEditGridValue(newFC2.RowIndex, string.Empty);
                }
            }
            //
            if (fc.FieldName == "ApellidoPri" || fc.FieldName == "ApellidoSdo" || fc.FieldName == "NombrePri" || fc.FieldName == "NombreSdo")
            {
                string v = Value.ToString();
                FieldConfiguration newFC1 = this.GetFieldConfigByFieldName("ApellidoPri");
                FieldConfiguration newFC2 = this.GetFieldConfigByFieldName("ApellidoSdo");
                FieldConfiguration newFC3 = this.GetFieldConfigByFieldName("NombrePri");
                FieldConfiguration newFC4 = this.GetFieldConfigByFieldName("NombreSdo");
                FieldConfiguration newFC5 = this.GetFieldConfigByFieldName("Descriptivo");
                string ap1 = this.GetEditRow(newFC1.RowIndex).Valor.ToString();
                string ap2 = this.GetEditRow(newFC2.RowIndex).Valor.ToString();
                string nom1 = this.GetEditRow(newFC3.RowIndex).Valor.ToString();
                string nom2 = this.GetEditRow(newFC4.RowIndex).Valor.ToString();
                 string nameDesc = string.Empty;
                if (newFC1 != null && newFC2 != null && newFC3 != null && newFC4 != null && newFC5 != null)
                {
                    switch (fc.FieldName)
                    {
                        case "ApellidoPri":
                            nameDesc += v + " " + ap2 + " " + nom1 + " " + nom2;
                            this.SetEditGridValue(newFC1.ColumnIndex, v);
                            break;
                        case "ApellidoSdo":
                            nameDesc += ap1 + " " + v + " " + nom1 + " " + nom2;
                            this.SetEditGridValue(newFC2.ColumnIndex, v);
                            break;
                        case "NombrePri":
                            nameDesc += ap1 + " " + ap2 + " " + v + " " + nom2;
                            this.SetEditGridValue(newFC3.ColumnIndex, v);
                            break;
                        case "NombreSdo":
                            nameDesc += ap1 + " " + ap2 + " " + nom1 + " " + v; 
                            this.SetEditGridValue(newFC4.ColumnIndex, v);
                            break;
                    }
                    nameDesc =  nameDesc.TrimEnd();
                    this.SetEditGridValue(newFC5.RowIndex, nameDesc);
                    newFC5.Editable = false;
                }
                if (string.IsNullOrWhiteSpace(nameDesc))
                {
                    newFC5.Editable = true;
                    this.SetEditGridValue(newFC5.ColumnIndex, string.Empty);
                }
            }
            //Calcula el digito de verificacion de acuerdo al NIT
            if (fc.FieldName == "TerceroID")
            {
                FieldConfiguration newFC = this.GetFieldConfigByFieldName("DigitoVerif");
                string v = Value.ToString();
                int dig = 0;
                if (v != string.Empty)
                {
                    dig =  Evaluador.Nit_DV(v);
                    this.SetEditGridValue(newFC.RowIndex, dig);
                }

            }
            //Verifica si es Regimen Simplificado 
            if (fc.FieldName == "ReferenciaID")
            {
                FieldConfiguration newFC = this.GetFieldConfigByFieldName("DeclaraIVAInd");
                string v = Value.ToString();
                DTO_coRegimenFiscal regimen = (DTO_coRegimenFiscal)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coRegimenFiscal, false, v, true);

                if (regimen.TipoTercero.Value == 2)
                {
                    newFC.Editable = false;
                    this.SetEditGridValue(newFC.RowIndex, bool.FalseString);
                }
                else
                {
                    newFC.Editable = true;
                }

            }
            //Verifica que los datos correspondan al tipo de persona natural 
            if (fc.FieldName == "TerceroDocTipoID")
            {
                string v = Value.ToString();
                int terTipoDoc = AppMasters.coTerceroDocTipo;
                DTO_coTerceroDocTipo tipoDoc = (DTO_coTerceroDocTipo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, terTipoDoc, false, v, true);
                FieldConfiguration newFC1 = this.GetFieldConfigByFieldName("ApellidoPri");
                FieldConfiguration newFC2 = this.GetFieldConfigByFieldName("ApellidoSdo");
                FieldConfiguration newFC3 = this.GetFieldConfigByFieldName("NombrePri");
                FieldConfiguration newFC4 = this.GetFieldConfigByFieldName("NombreSdo");
                FieldConfiguration newFC5 = this.GetFieldConfigByFieldName("Descriptivo");
                if (tipoDoc != null && tipoDoc.PersonaNaturalInd.Value == false)
                {
                    //this.SetEditGridValue(newFC1.RowIndex, string.Empty);
                    this.SetEditGridValue(newFC2.RowIndex, string.Empty);
                    this.SetEditGridValue(newFC3.RowIndex, string.Empty);
                    this.SetEditGridValue(newFC4.RowIndex, string.Empty);
                    //newFC1.Editable = false;
                    newFC2.Editable = false;
                    newFC3.Editable = false;
                    newFC4.Editable = false;
                    newFC5.Editable = false;
                }
                else if(tipoDoc != null && tipoDoc.PersonaNaturalInd.Value == true)
                {
                    //newFC1.Editable = true;
                    newFC2.Editable = true;
                    newFC3.Editable = true;
                    newFC4.Editable = true;
                    newFC5.Editable = false;
                }
                else
                    newFC5.Editable = false;
            }
         
            return res;
        }

        /// <summary>
        /// Sobrecargar para modificar alguna configuración de campo de la maestra
        /// </summary>
        public override void CustomizeFieldsConfig()
        {
            this.GetFieldConfigByFieldName("Descriptivo").Editable = false;
        }
    
        #endregion

        #region Eventos grilla edicion

        /// <summary>
        /// Asigna un editor de celda(button, check, textbox..) a la celda relacionada del index
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvRecordEdit_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            this.viewBeingEdited = null;
            this.viewBeingEdited = this.GetFocusedGridView();
            
            GridProperty gpHandle = (GridProperty)e.Column.View.GetFocusedRow();
            FieldConfiguration config = this.GetFieldConfigByCaption(gpHandle.Campo);

            if (config.FieldName == "Direccion")
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
            if (config.FieldName == "Direccion")
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

    }
}
