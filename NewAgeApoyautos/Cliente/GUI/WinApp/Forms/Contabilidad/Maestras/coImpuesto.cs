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

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    class coImpuesto : MasterComplexForm
    {
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();

        ///<summary>
        /// Constructor 
        /// </summary>
        public coImpuesto() : base() { }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.coImpuesto;
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
                res = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_RegimenFiscalXDefecto);
            if (fieldName == "RegFisEmpresaDesc")
            {
                string regId = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_RegimenFiscalXDefecto);
                DTO_coRegimenFiscal idReg = (DTO_coRegimenFiscal)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coRegimenFiscal, false, regId, true);
                if (idReg != null)
                    res = idReg.Descriptivo.Value;
            }

            if (fieldName == "LugarGeograficoID")
            {
                res = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
            }
            if (fieldName == "LugarGeoDesc")
            {
                string lugarGeo = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                DTO_glLugarGeografico descGeo = (DTO_glLugarGeografico)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.glLugarGeografico, false, lugarGeo, true);
                if (descGeo != null)
                    res = descGeo.Descriptivo.Value;
            }

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

            //Valida que el regimen fiscal sea el de la tabla de control
            if (fc.FieldName == "RegimenFiscalEmpresaID")
            {
                string regFis = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_RegimenFiscalXDefecto);
                string v = Value.ToString();
                if (v != regFis)
                {
                    err = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_Impuesto_RegFisEmp_NotCompatible) + " " + regFis;
                    res = false;
                }
            }

            //Valida las reglas del tipo de impuesto
            if (fc.FieldName == "ImpuestoTipoID")
            {
                string v = Value.ToString();

                int impTipoDoc = AppMasters.coImpuestoTipo;
                DTO_coImpuestoTipo tipo = (DTO_coImpuestoTipo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, impTipoDoc, false, v, true);

                //Verifica si el impuesto es nacional: tipo de impuesto (impuesto alcance) 
                bool isNacional = tipo != null && tipo.ImpuestoAlcance.Value == 1 ? true : false;
                if (isNacional)
                {
                    int lgDoc = AppMasters.glLugarGeografico;
                    List<DTO_MasterBasic> lgs = _bc.AdministrationModel.MasterHierarchy_GetChindrenPaged(lgDoc, 1, 1,
                        OrderDirection.ASC, new UDT_BasicID(), string.Empty, string.Empty, true).ToList();
                    if (lgs.Count > 0)
                    {
                        UDT_BasicID idParent = new UDT_BasicID() { Value = lgs.First().ID.Value };
                        lgs = _bc.AdministrationModel.MasterHierarchy_GetChindrenPaged(lgDoc, 1, 1,
                            OrderDirection.ASC, idParent, string.Empty, string.Empty, true).ToList();

                        if (lgs.Count > 0)
                        {
                            DTO_MasterBasic lg = lgs.First();
                            FieldConfiguration newFC = this.GetFieldConfigByFieldName("LugarGeograficoID");
                            if (newFC != null)
                            {
                                DTO_glConsultaFiltro consulta=new DTO_glConsultaFiltro();

                                string valorDTO = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                                DTO_glLugarGeografico lugar = (DTO_glLugarGeografico)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.glLugarGeografico, false, valorDTO, true);
                                newFC.Editable = false;
                                this.SetEditGridValue(newFC.ColumnIndex, this._bc.GetControlValueByCompany(ModulesPrefix.co,AppControl.co_LugarGeoXDefecto));
                                this.SetEditGridValue(newFC.ColumnIndex + 1, lugar.Descriptivo);
                            }
                        }
                    }
                }
                else
                {
                    FieldConfiguration newFC = this.GetFieldConfigByFieldName("LugarGeograficoID");
                    if (newFC != null)
                    {
                        string valorDTO = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                        DTO_glLugarGeografico lugar = (DTO_glLugarGeografico)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.glLugarGeografico, false, valorDTO, true);
                                
                        newFC.Editable = true;
                        this.SetEditGridValue(newFC.ColumnIndex, this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto));
                        this.SetEditGridValue(newFC.ColumnIndex + 1, lugar.Descriptivo);
                    }
                }

            }

            return res;
        }

        #endregion
    }
}
