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

        #region Sobrecarga métodos de la maestar simple

        /// <summary>
        /// Metodo encargado de encapsular la insercion de registros de maestras simples
        /// </summary>
        /// <param name="tableName">Nombre de la tabla</param>
        /// <param name="insertList">Lista de dtos</param>
        /// <param name="userId">usuario logueado</param>
        /// <param name="documentId">docuemnto</param>
        /// <param name="accion">accion (insercion/importar)</param>
        /// <returns></returns>
        protected override DTO_TxResult DataAdd(List<DTO_MasterBasic> insertList, int accion)
        {
            foreach (DTO_MasterBasic d in insertList)
            {
                DTO_ccCliente cliente = (DTO_ccCliente)d;
                cliente.FechaExpDoc.Value = cliente.FechaNacimiento.Value;
            }
            byte[] bItems = CompressedSerializer.Compress<IEnumerable<DTO_MasterBasic>>(insertList);
            return _bc.AdministrationModel.ccCliente_Add(this.DocumentID, bItems);
        }

        /// <summary>
        /// Metodo que encapsula la funcion de actualizar
        /// </summary>
        /// <param name="tableName">Nombre de la tabla</param>
        /// <param name="dto">dto</param>
        /// <param name="userId">usuario</param>
        /// <param name="documentId">documento</param>
        /// <returns></returns>
        protected override DTO_TxResult DataUpdate(DTO_MasterBasic dto)
        {
            string sectorCartera = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_SectorCartera);
            DTO_ccCliente cliente = (DTO_ccCliente)dto;         

            //if (Convert.ToByte(sectorCartera) == (byte)SectorCartera.Financiero)
            //{
                DTO_coTercero tercero = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, cliente.ID.Value, true);
                cliente.LaboralDireccion.Value = string.IsNullOrEmpty(cliente.LaboralDireccion.Value)? tercero.Direccion.Value : cliente.LaboralDireccion.Value;
                cliente.NacimientoCiudad.Value = string.IsNullOrEmpty(cliente.NacimientoCiudad.Value)? tercero.LugarGeograficoID.Value : cliente.NacimientoCiudad.Value;
                cliente.ResidenciaCiudad.Value = string.IsNullOrEmpty(cliente.ResidenciaCiudad.Value) ? tercero.LugarGeograficoID.Value : cliente.ResidenciaCiudad.Value;
                cliente.ApellidoPri.Value = string.IsNullOrEmpty(cliente.ApellidoPri.Value) ? tercero.ApellidoPri.Value : cliente.ApellidoPri.Value;
                cliente.ApellidoSdo.Value = string.IsNullOrEmpty(cliente.ApellidoSdo.Value) ? tercero.ApellidoSdo.Value : cliente.ApellidoSdo.Value;
                cliente.NombrePri.Value = string.IsNullOrEmpty(cliente.NombrePri.Value) ? tercero.NombrePri.Value : cliente.NombrePri.Value;
                cliente.NombreSdo.Value = string.IsNullOrEmpty(cliente.NombreSdo.Value) ? tercero.NombreSdo.Value : cliente.NombreSdo.Value;
                if (cliente.FechaNacimiento.Value != null)
                {
                    cliente.FechaNacimiento.Value = Convert.ToDateTime(cliente.FechaNacimiento.Value.Value.ToShortDateString());
                    cliente.FechaExpDoc.Value = Convert.ToDateTime(cliente.FechaNacimiento.Value.Value.ToShortDateString());
                }
               
            //}
            return _bc.AdministrationModel.ccCliente_Update(cliente);
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
            if (!this.Insertando) {
                FieldConfiguration newFC2 = this.GetFieldConfigByFieldName("FechaRetiro");
                FieldConfiguration newFC3 = this.GetFieldConfigByFieldName("EstadoCartera");
                FieldConfiguration newFC4 = this.GetFieldConfigByFieldName("FechaINIEstado");
                FieldConfiguration newFC5 = this.GetFieldConfigByFieldName("AbogadoID");
                FieldConfiguration newFC6 = this.GetFieldConfigByFieldName("AbogadoDesc");
                FieldConfiguration newFC7 = this.GetFieldConfigByFieldName("CobranzaEstadoID");
                FieldConfiguration newFC8 = this.GetFieldConfigByFieldName("CobranzaEstadoDesc");
                FieldConfiguration newFC9 = this.GetFieldConfigByFieldName("CobranzaGestionID");
                FieldConfiguration newFC10 = this.GetFieldConfigByFieldName("CobranzaGestionDesc");
                FieldConfiguration newFC11 = this.GetFieldConfigByFieldName("DocumCobranza");
                FieldConfiguration newFC12 = this.GetFieldConfigByFieldName("ConsIncumplimiento");
                FieldConfiguration newFC13 = this.GetFieldConfigByFieldName("NumDocVencido");
                FieldConfiguration newFC14 = this.GetFieldConfigByFieldName("NumDocCJ");
                FieldConfiguration newFC15 = this.GetFieldConfigByFieldName("FechaIngreso");
                newFC2.Editable = false;
                newFC3.Editable = false;
                newFC4.Editable = false;
                newFC5.Editable = false;
                newFC6.Editable = false;
                newFC7.Editable = false;
                newFC8.Editable = false;
                newFC9.Editable = false;
                newFC10.Editable = false;
                newFC11.Editable = false;
                newFC12.Editable = false;
                newFC13.Editable = false;
                newFC14.Editable = false;
            }
            this.viewBeingEdited = null;
            this.viewBeingEdited = this.GetFocusedGridView();

            GridProperty gpHandle = (GridProperty)e.Column.View.GetFocusedRow();
            FieldConfiguration config = this.GetFieldConfigByCaption(gpHandle.Campo);

            if (config.FieldName == "ResidenciaDir" || config.FieldName == "DireccionAct")
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
            if (config.FieldName == "ResidenciaDir" || config.FieldName == "DireccionAct")
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
            string sectorCartera = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_SectorCartera);
            if (fc.FieldName == "CuentaTipo_1")
            {
                int v = Convert.ToInt32(Value);

                FieldConfiguration newFC1 = this.GetFieldConfigByFieldName("BcoCtaNro_1");
                newFC1.Editable = true;
                if (v == 3)
                {
                    newFC1.Editable = false;
                    newFC1.AllowNull = true;
                }
            }

            //Asigna Descripcion
            if (fc.FieldName == "TerceroID")
            {
                DTO_coTercero tercero = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, Value.ToString(), true);
                if (tercero != null)
                {
                    FieldConfiguration newFC1 = this.GetFieldConfigByFieldName("Descriptivo");
                    this.SetEditGridValue(newFC1.RowIndex, tercero.Descriptivo.Value);
                    newFC1.Editable = false;

                    //Asigna Direccion
                    FieldConfiguration newFC2 = this.GetFieldConfigByFieldName("ResidenciaDir");
                    this.SetEditGridValue(newFC2.RowIndex, tercero.Direccion.Value);

                    #region Asigna Telefonos
                    //Asigna teléfono
                    FieldConfiguration newFC3 = this.GetFieldConfigByFieldName("Telefono");
                    this.SetEditGridValue(newFC3.RowIndex, tercero.Tel1.Value);

                    //Asigna Cel
                    FieldConfiguration newFC4 = this.GetFieldConfigByFieldName("Celular");
                    this.SetEditGridValue(newFC4.RowIndex, tercero.Tel2.Value);

                    //Asigna teléfono 1
                    FieldConfiguration newFC21 = this.GetFieldConfigByFieldName("Telefono1");
                    this.SetEditGridValue(newFC21.RowIndex, string.IsNullOrEmpty(tercero.Telefono1.Value) ? tercero.Tel1.Value : tercero.Telefono1.Value);

                    //Asigna teléfono 2
                    FieldConfiguration newFC23 = this.GetFieldConfigByFieldName("Telefono2");
                    this.SetEditGridValue(newFC23.RowIndex, tercero.Telefono2.Value);

                    //Asigna Extension 1
                    FieldConfiguration newFC24 = this.GetFieldConfigByFieldName("Extension1");
                    this.SetEditGridValue(newFC24.RowIndex, tercero.Extension1.Value);

                    //Asigna Extension 2
                    FieldConfiguration newFC25 = this.GetFieldConfigByFieldName("Extension2");
                    this.SetEditGridValue(newFC25.RowIndex, tercero.Extension2.Value);

                    //Asigna Celular 1
                    FieldConfiguration newFC22 = this.GetFieldConfigByFieldName("Celular1");
                    this.SetEditGridValue(newFC22.RowIndex, string.IsNullOrEmpty(tercero.Celular1.Value) ? tercero.Tel2.Value : tercero.Celular1.Value);

                    //Asigna Cecular 2
                    FieldConfiguration newFC26 = this.GetFieldConfigByFieldName("Celular2");
                    this.SetEditGridValue(newFC26.RowIndex, tercero.Celular2.Value); 
                    #endregion

                    //Asigna Correo Electronico
                    FieldConfiguration newFC5 = this.GetFieldConfigByFieldName("Correo");
                        this.SetEditGridValue(newFC5.RowIndex, tercero.CECorporativo.Value);

                    // Asigna codigo del Cliente
                    FieldConfiguration newFC9 = this.GetFieldConfigByFieldName("ClienteID");
                    if (string.IsNullOrWhiteSpace(this.GetEditRow(newFC9.ColumnIndex).Valor.ToString()))
                        this.SetEditGridValue(newFC9.RowIndex, tercero.ID.Value);

                    // Asigna codigo del Banco
                    FieldConfiguration newFC10 = this.GetFieldConfigByFieldName("BancoID_1");
                    if (string.IsNullOrWhiteSpace(this.GetEditRow(newFC10.ColumnIndex).Valor.ToString()))
                        this.SetEditGridValue(newFC10.RowIndex, tercero.BancoID_1);

                    FieldConfiguration newFC110 = this.GetFieldConfigByFieldName("Banco1Desc");
                    if (string.IsNullOrWhiteSpace(this.GetEditRow(newFC110.ColumnIndex).Valor.ToString()))
                        this.SetEditGridValue(newFC110.RowIndex, tercero.Banco1Desc);

                    // Asigna el tipo de cuenta
                    FieldConfiguration newFC11 = this.GetFieldConfigByFieldName("CuentaTipo_1");
                    if (string.IsNullOrWhiteSpace(this.GetEditRow(newFC11.ColumnIndex).Valor.ToString()))
                        this.SetEditGridValue(newFC11.RowIndex, tercero.CuentaTipo_1);

                    // Asigna el numero de cuenta
                    FieldConfiguration newFC12 = this.GetFieldConfigByFieldName("BcoCtaNro_1");
                    if (string.IsNullOrWhiteSpace(this.GetEditRow(newFC12.ColumnIndex).Valor.ToString()))
                        this.SetEditGridValue(newFC12.RowIndex, tercero.BcoCtaNro_1);

                    //if (sectorCartera == ((byte)SectorCartera.Financiero).ToString())
                    //{
                        //Asigna Direccion laboral
                        FieldConfiguration newFC13 = this.GetFieldConfigByFieldName("LaboralDireccion");
                        this.SetEditGridValue(newFC13.RowIndex, tercero.Direccion.Value);

                        //Asigna NacimientoCiudad laboral
                        FieldConfiguration newFC14 = this.GetFieldConfigByFieldName("NacimientoCiudad");
                        this.SetEditGridValue(newFC14.RowIndex, tercero.LugarGeograficoID.Value);
                    //}
                    //Asigna ResidenciaCiudad laboral
                    FieldConfiguration newFC15 = this.GetFieldConfigByFieldName("ResidenciaCiudad");
                    this.SetEditGridValue(newFC15.RowIndex, tercero.LugarGeograficoID.Value);

                    //Asigna ResidenciaCiudad laboral Desc
                    FieldConfiguration newFC20 = this.GetFieldConfigByFieldName("ResidenciaCiudadDesc");
                    this.SetEditGridValue(newFC20.RowIndex, tercero.LugarGeoDesc.Value);

                    //Asigna Primer Apellido 
                    FieldConfiguration newFC16 = this.GetFieldConfigByFieldName("ApellidoPri");
                    this.SetEditGridValue(newFC16.RowIndex, tercero.ApellidoPri.Value);

                    //Asigna Segundo Apellido 
                    FieldConfiguration newFC17 = this.GetFieldConfigByFieldName("ApellidoSdo");
                    this.SetEditGridValue(newFC17.RowIndex, tercero.ApellidoSdo.Value);

                    //Asigna Primer Nombre 
                    FieldConfiguration newFC18 = this.GetFieldConfigByFieldName("NombrePri");
                    this.SetEditGridValue(newFC18.RowIndex, tercero.NombrePri.Value);

                    //Asigna Seg Nombre 
                    FieldConfiguration newFC19 = this.GetFieldConfigByFieldName("NombreSdo");
                    this.SetEditGridValue(newFC19.RowIndex, tercero.NombreSdo.Value);
                }
            }
            else if (fc.FieldName == "ApellidoPri" || fc.FieldName == "ApellidoSdo" || fc.FieldName == "NombrePri" || fc.FieldName == "NombreSdo")
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
                    nameDesc = nameDesc.TrimEnd();
                    this.SetEditGridValue(newFC5.RowIndex, nameDesc);
                    newFC5.Editable = false;
                }
                if (string.IsNullOrWhiteSpace(nameDesc))
                {
                    newFC5.Editable = true;
                    this.SetEditGridValue(newFC5.ColumnIndex, string.Empty);
                }
            }
            else if (fc.FieldName == "FechaNacimiento")
            {             
                if (sectorCartera == ((byte)SectorCartera.Financiero).ToString())
                {
                    //if (!string.IsNullOrEmpty(Value.ToString()))
                    //    Value = Convert.ToDateTime(Value).ToShortDateString();
                    //string fecha = ObjectCopier.Clone(Value).ToString();
                    //FieldConfiguration newFC1 = this.GetFieldConfigByFieldName("FechaExpDoc");
                    //if (!string.IsNullOrEmpty(fecha))
                    //    this.SetEditGridValue(newFC1.RowIndex,Value);
                }
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

        /// <summary>
        /// Funcion q se encarga de hacer un nuevo registro
        /// </summary>
        public override void TBNew(){
                base.TBNew();
                FieldConfiguration newFC1 = this.GetFieldConfigByFieldName("ClienteTipo");
                this.SetEditGridValue(newFC1.RowIndex, 1);

                FieldConfiguration newFC2 = this.GetFieldConfigByFieldName("FechaRetiro");
                FieldConfiguration newFC3 = this.GetFieldConfigByFieldName("EstadoCartera");
                FieldConfiguration newFC4 = this.GetFieldConfigByFieldName("FechaINIEstado");
                FieldConfiguration newFC5 = this.GetFieldConfigByFieldName("AbogadoID");
                FieldConfiguration newFC6 = this.GetFieldConfigByFieldName("AbogadoDesc");
                FieldConfiguration newFC7 = this.GetFieldConfigByFieldName("CobranzaEstadoID");
                FieldConfiguration newFC8 = this.GetFieldConfigByFieldName("CobranzaEstadoDesc");
                FieldConfiguration newFC9 = this.GetFieldConfigByFieldName("CobranzaGestionID");
                FieldConfiguration newFC10 = this.GetFieldConfigByFieldName("CobranzaGestionDesc");
                FieldConfiguration newFC11 = this.GetFieldConfigByFieldName("DocumCobranza");
                FieldConfiguration newFC12 = this.GetFieldConfigByFieldName("ConsIncumplimiento");
                FieldConfiguration newFC13 = this.GetFieldConfigByFieldName("NumDocVencido");
                FieldConfiguration newFC14 = this.GetFieldConfigByFieldName("NumDocCJ");                
                FieldConfiguration newFC15 = this.GetFieldConfigByFieldName("FechaIngreso");
                newFC2.Editable = false;
                newFC3.Editable = false;
                newFC4.Editable = false;
                newFC5.Editable = false;
                newFC6.Editable = false;
                newFC7.Editable = false;
                newFC8.Editable = false;
                newFC9.Editable = false;
                newFC10.Editable = false;
                newFC11.Editable = false;
                newFC12.Editable = false;
                newFC13.Editable = false;
                newFC14.Editable = false;
                newFC15.Editable = true;

                FieldConfiguration newFCDefecto = this.GetFieldConfigByFieldName("EstadoCartera");
                if (string.IsNullOrWhiteSpace(this.GetEditRow(newFCDefecto.ColumnIndex).Valor.ToString()))
                    this.SetEditGridValue(newFCDefecto.RowIndex, "1");

                // Asigna Tipo Residencia por defecto
               newFCDefecto = this.GetFieldConfigByFieldName("ResidenciaTipo");
                this.SetEditGridValue(newFCDefecto.RowIndex, "1");

                // Asigna Zona por defecto
                string zonaDef = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_Zona);
                newFCDefecto = this.GetFieldConfigByFieldName("ZonaID");
                this.SetEditGridValue(newFCDefecto.RowIndex, zonaDef);

                // Asigna cargo por defecto
                newFCDefecto = this.GetFieldConfigByFieldName("Cargo");
                this.SetEditGridValue(newFCDefecto.RowIndex, "N/A");
                
                // Asigna profesion por defecto
                string profesionDef = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ProfesionPorDefecto);
                newFCDefecto = this.GetFieldConfigByFieldName("ProfesionID");
                this.SetEditGridValue(newFCDefecto.RowIndex,profesionDef);

                // Asigna AsesorID por defecto
                string asesorxDef = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_AsesorXDefecto);
                newFCDefecto = this.GetFieldConfigByFieldName("AsesorID");
                this.SetEditGridValue(newFCDefecto.RowIndex,asesorxDef);

                // Asigna LaboralEntidad por defecto
                newFCDefecto = this.GetFieldConfigByFieldName("LaboralEntidad");
                this.SetEditGridValue(newFCDefecto.RowIndex,"N/A");

                // Asigna Antiguedad por defecto
                newFCDefecto = this.GetFieldConfigByFieldName("Antiguedad");
                this.SetEditGridValue(newFCDefecto.RowIndex,"1");

                // Asigna ClienteTipo por defecto
                newFCDefecto = this.GetFieldConfigByFieldName("ClienteTipo");
                this.SetEditGridValue(newFCDefecto.RowIndex,"1");

                // Asigna Estrato por defecto
                newFCDefecto = this.GetFieldConfigByFieldName("Estrato");
                this.SetEditGridValue(newFCDefecto.RowIndex,"1");

                // Asigna EscolaridadNivel por defecto
                newFCDefecto = this.GetFieldConfigByFieldName("EscolaridadNivel");
                this.SetEditGridValue(newFCDefecto.RowIndex,"0");

                // Asigna JornadaLaboral por defecto
                newFCDefecto = this.GetFieldConfigByFieldName("JornadaLaboral");
                this.SetEditGridValue(newFCDefecto.RowIndex,"1");

                // Asigna Ocupacion por defecto
                newFCDefecto = this.GetFieldConfigByFieldName("Ocupacion");
                this.SetEditGridValue(newFCDefecto.RowIndex,"1");

                // Asigna VlrDevengado por defecto
                newFCDefecto = this.GetFieldConfigByFieldName("VlrDevengado");
                this.SetEditGridValue(newFCDefecto.RowIndex,"0");

                // Asigna VlrDeducido por defecto
                newFCDefecto = this.GetFieldConfigByFieldName("VlrDeducido");
                this.SetEditGridValue(newFCDefecto.RowIndex,"0");

                // Asigna VlrActivos por defecto
                newFCDefecto = this.GetFieldConfigByFieldName("VlrActivos");
                this.SetEditGridValue(newFCDefecto.RowIndex,"0");

                // Asigna VlrPasivos por defecto
                newFCDefecto = this.GetFieldConfigByFieldName("VlrPasivos");
                this.SetEditGridValue(newFCDefecto.RowIndex,"0");

                // Asigna VlrMesada por defecto
                newFCDefecto = this.GetFieldConfigByFieldName("VlrMesada");
                this.SetEditGridValue(newFCDefecto.RowIndex,"0");

                 // Asigna VlrConsultado por defecto
                newFCDefecto = this.GetFieldConfigByFieldName("VlrConsultado");
                this.SetEditGridValue(newFCDefecto.RowIndex,"0");

                // Asigna VlrOpera por defecto
                newFCDefecto = this.GetFieldConfigByFieldName("VlrOpera");
                this.SetEditGridValue(newFCDefecto.RowIndex,"0");

        }

        #endregion
    }
        
}
