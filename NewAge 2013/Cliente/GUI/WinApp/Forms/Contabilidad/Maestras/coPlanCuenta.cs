using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using System.Windows.Forms;
using NewAge.DTO.Resultados;
using NewAge.DTO.UDT;
using System.Reflection;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    class coPlanCuenta : MasterHierarchyForm
    {
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();

        ///<summary>
        /// Constructor 
        /// </summary>
        public coPlanCuenta() : base() { }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.coPlanCuenta;
            base.InitForm();
        }

        #region Validaciones Del formulario

        public override void TBNew()
        {
            base.TBNew();
            if (_bc.GetControlValueByCompany(ModulesPrefix.pl, AppControl.pl_ConsPresupuestalInd)=="0")
            {
                FieldConfiguration newFC = this.GetFieldConfigByFieldName("LineaPresupuestalInd");
                newFC.Editable = false;
            }
        }

        /// <summary>
        /// Carga los datos iniciales de una maestra
        /// </summary>
        /// <param name="fieldName">Nombre del campo</param>
        /// <returns>Retorna el valor que debe ir en el campo</returns>
        protected override string NewRecordLoadData(string fieldName)
        {
            try
            {
                string res = string.Empty;
                FieldConfiguration newFC = this.GetFieldConfigByFieldName(fieldName);

                #region Trae el dato de Cuenta Grupo por defecto
                //if (fieldName == "CuentaGrupoID")
                //{
                //    IEnumerable<DTO_MasterBasic> ctagrupos = _bc.AdministrationModel.MasterSimple_GetAll(AppMasters.coCuentaGrupo, true);
                //    foreach (DTO_coCuentaGrupo grupo in ctagrupos)
                //    {
                //        if (grupo.MascaraCta.Value == 0)
                //        {
                //            this.SetEditGridValue(newFC.ColumnIndex, grupo.ID);
                //            this.SetEditGridValue(newFC.ColumnIndex + 1, grupo.Descriptivo);
                //            break;
                //        }
                //    }
                //} 
                #endregion

                #region Trae el dato de Linea presupuestal por defecto
                if (fieldName == "LineaPresupuestalInd")
                {
                    res = _bc.GetControlValueByCompany(ModulesPrefix.pl, AppControl.pl_ConsPresupuestalInd);
                    if (res.Equals("0"))
                    {
                        //res = bool.FalseString;
                        newFC.Editable = false;
                    }
                    else if (res.Equals("1"))
                    {
                        //res = bool.FalseString;
                        newFC.Editable = true;
                    }
                } 
                #endregion

                #region Habilita o deshabilita el campo de cuentagrupo Oc si el modulo oc esta o no activo
                if (fieldName == "ocCuentaGrupoID")
                {
                    if(_bc.AdministrationModel.Modules.ContainsKey(ModulesPrefix.oc.ToString()))
                        newFC.Editable = true;
                    else
                        newFC.Editable = false;
                } 
                #endregion

                #region Desactiva o no los campos de Ajustes de acuerdo a la data de glControl
                if (fieldName == "AjCambioTerceroInd")
                {
                    if (newFC != null)
                    {
                        if (_bc.AdministrationModel.MultiMoneda)
                        {
                            res = bool.FalseString;
                            newFC.Editable = true;
                        }
                        else
                        {
                            res = bool.FalseString;
                            newFC.Editable = false;
                        }
                    }
                } 
                #endregion

                #region Cuenta Corporativa
                string ctaCorporativa = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_EmpresaCorporativa);
                if (ctaCorporativa != null)
                {
                    
                }
                
                #endregion
                return res;
            }
            catch (Exception)
            {
                throw;
            }
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

            #region CuentaGrupoID
            if (fc.FieldName == "CuentaGrupoID")
            {
                FieldConfiguration newFC1 = this.GetFieldConfigByFieldName("MascaraCta");
                int ctaGrupo = AppMasters.coCuentaGrupo;
                DTO_coCuentaGrupo ctaGrupodto = (DTO_coCuentaGrupo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, ctaGrupo, false, Value.ToString(), true);
                this.SetEditGridValue(newFC1.ColumnIndex, ctaGrupodto.MascaraCta.Value);
                //int mascaraGrupoCta = Convert.ToInt32(ctaGrupodto.MascaraCta.Value);
                //int longiTotal = this.Table.CodeLength(this.Table.LevelsUsed());
                //int[] nivelUsed = this.Table.CompleteLevelLengths();
                //if (!nivelUsed.Contains<int>(mascaraGrupoCta) && mascaraGrupoCta != 0)
                //{
                //    err = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_PlanCta_CuentaGrupo_InvalidMask);
                //}

            }
            #endregion

            #region IMpuestoTipoID
            if (fc.FieldName == "ImpuestoTipoID")
            {
                FieldConfiguration newFC1 = this.GetFieldConfigByFieldName("ImpuestoPorc");
                FieldConfiguration newFC2 = this.GetFieldConfigByFieldName("MontoMinimo");
                FieldConfiguration newFC3 = this.GetFieldConfigByFieldName("TerceroInd");
                FieldConfiguration newFC5 = this.GetFieldConfigByFieldName("NITCierreAnual");
                FieldConfiguration newFC6 = this.GetFieldConfigByFieldName("ImpuestoTipoID");
                FieldConfiguration newFC7 = this.GetFieldConfigByFieldName("ImpTipoDesc");
                FieldConfiguration newFC8 = this.GetFieldConfigByFieldName("NITCierreDesc");
                FieldConfiguration newFC9 = this.GetFieldConfigByFieldName("Tipo");
                FieldConfiguration newFC10 = this.GetFieldConfigByFieldName("LugarGeograficoInd");
                DialogResult Result = DialogResult.None;

                //Valida si el Ind de LugarGeografico está activo, sino no está trae un NITCierreAnual(coTercero) por defecto 
                //string lg = this.GetEditRow(newFC10.ColumnIndex).Valor.ToString();
                bool LugGeoInd = this.GetEditRow(newFC10.ColumnIndex).Valor.Equals(bool.FalseString) ? true : false;
                if (LugGeoInd)
                {
                    string tercDef = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                    int terDoc = AppMasters.coTercero;
                    DTO_coTercero tercero = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, terDoc, false, tercDef, true);
                    if (tercero != null)
                    {
                        string idTer = tercero.ID.Value;
                        string desTer = tercero.Descriptivo.Value;
                        this.SetEditGridValue(newFC5.ColumnIndex, idTer);
                        this.SetEditGridValue(newFC8.ColumnIndex, desTer);
                    }
                }
                else
                {
                    this.SetEditGridValue(newFC5.ColumnIndex, string.Empty);
                    this.SetEditGridValue(newFC8.ColumnIndex, string.Empty);
                }
                //Valida multiples campos que dependen si existe Tipo de Impuesto y Tipo(resultado)
                string v = Value.ToString();
                int ImpTipoDoc = AppMasters.coImpuestoTipo;
                DTO_coImpuestoTipo tipo = (DTO_coImpuestoTipo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, ImpTipoDoc, false, v, true);

                if (!base.Insertando && !string.IsNullOrEmpty(Value.ToString()))
                    Result = MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_PlanCta_ImpTipo_ChangeSure), this._frmName, MessageBoxButtons.YesNo);
                if (base.Insertando || Result == DialogResult.Yes)
                {
                    bool isTipoResul = this.GetEditRow(newFC9.ColumnIndex).Valor.Equals("2") ? true : false;
                    if (isTipoResul)
                    {
                        newFC5.AllowNull = false;
                    }
                    if (tipo != null && newFC1 != null && newFC2 != null && newFC3 != null && newFC5 != null && newFC6 != null && newFC9 != null)
                    {
                        newFC1.Editable = true;
                        newFC2.Editable = true;
                        this.SetEditGridValue(newFC3.ColumnIndex, bool.TrueString);
                    }
                    else
                    {
                        newFC1.Editable = false;
                        newFC1.AllowNull = true;
                        newFC2.Editable = false;
                        newFC2.AllowNull = true;
                        newFC5.AllowNull = true;
                        this.SetEditGridValue(newFC1.ColumnIndex, string.Empty);
                        this.SetEditGridValue(newFC2.ColumnIndex, string.Empty);
                        this.SetEditGridValue(newFC3.ColumnIndex, bool.FalseString);
                        this.SetEditGridValue(newFC5.ColumnIndex, string.Empty);
                        this.SetEditGridValue(newFC8.ColumnIndex, string.Empty);
                    }
                }
                else if (Result == DialogResult.No)
                {
                    int rIndex = this.gvModule.FocusedRowHandle;
                    string colName = this.unboundPrefix + fc.FieldName;
                    string colNameDesc = this.unboundPrefix + newFC7.FieldName;
                    string valImp = this.gvModule.GetRowCellValue(rIndex, colName).ToString();
                    string valImpDesc = this.gvModule.GetRowCellValue(rIndex, colNameDesc).ToString();
                    this.SetEditGridValue(newFC7.ColumnIndex, valImpDesc);
                    if (!Value.Equals(valImp))
                        res = false;
                    if (string.IsNullOrEmpty(valImp))
                    {
                        this.SetEditGridValue(newFC6.ColumnIndex, string.Empty);
                        this.SetEditGridValue(newFC5.ColumnIndex, string.Empty);
                        this.SetEditGridValue(newFC8.ColumnIndex, string.Empty);
                    }
                }
            }
            #endregion

            //Cambia el valor de los campos de Ajuste en cambio segun la info de ConceptoSaldo
            #region ConceptoSaldo
            if (fc.FieldName == "ConceptoSaldoID")
            {
                FieldConfiguration newFC1 = this.GetFieldConfigByFieldName("AjCambioTerceroInd");
                FieldConfiguration newFC2 = this.GetFieldConfigByFieldName("AjCambioRealizadoInd");
                FieldConfiguration newFC3 = this.GetFieldConfigByFieldName("DocumentoControlInd");
                FieldConfiguration newFC4 = this.GetFieldConfigByFieldName("TerceroInd");
                FieldConfiguration newFC5 = this.GetFieldConfigByFieldName("TerceroSaldosInd");
                string v = Value.ToString();
                int ConceptoSaldo = AppMasters.glConceptoSaldo;
                DTO_glConceptoSaldo ConceptoSaldoDto = (DTO_glConceptoSaldo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, ConceptoSaldo, false, v, true);
                if (ConceptoSaldoDto != null)
                {
                    if (ConceptoSaldoDto.coSaldoControl.Value == (byte)SaldoControl.Cuenta)
                    {
                        newFC1.Editable = true;
                        newFC2.Editable = true;
                        newFC3.Editable = true;
                        newFC4.Editable = true;
                        newFC5.Editable = true;
                    }
                    else
                    {
                        newFC1.Editable = false;
                        newFC2.Editable = false;
                        newFC3.Editable = false;
                        newFC4.Editable = false;
                        newFC5.Editable = false;
                        if (ConceptoSaldoDto.coSaldoControl.Value == (byte)SaldoControl.Doc_Externo || ConceptoSaldoDto.coSaldoControl.Value == (byte)SaldoControl.Doc_Interno)
                        {
                            this.SetEditGridValue(newFC1.ColumnIndex, bool.TrueString);
                            this.SetEditGridValue(newFC2.ColumnIndex, bool.TrueString);
                            this.SetEditGridValue(newFC3.ColumnIndex, bool.TrueString);
                            this.SetEditGridValue(newFC4.ColumnIndex, bool.TrueString);
                            this.SetEditGridValue(newFC5.ColumnIndex, bool.TrueString);
                        }
                        else
                        {
                            this.SetEditGridValue(newFC1.ColumnIndex, bool.FalseString);
                            this.SetEditGridValue(newFC2.ColumnIndex, bool.FalseString);
                            this.SetEditGridValue(newFC3.ColumnIndex, bool.FalseString);
                            this.SetEditGridValue(newFC4.ColumnIndex, bool.FalseString);
                            this.SetEditGridValue(newFC5.ColumnIndex, bool.FalseString);
                        }
                    }
                }
            }
            #endregion

            //Desactiva o no los campos de Ajustes de acuerdo a la informacion dada por glControl
            #region AjCambioTerceroInd
            if (fc.FieldName == "AjCambioTerceroInd")
            {
                FieldConfiguration newFC = this.GetFieldConfigByFieldName(fc.FieldName);
                if (newFC != null)
                {
                    bool IndAj = true;
                    if (!_bc.AdministrationModel.MultiMoneda)
                        IndAj = false;
                    if (Convert.ToBoolean(Value) && !IndAj)
                    {
                        string errMsg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_PlanCta_AjCambioInd_OnlyValue);
                        err = string.Format(errMsg, fc.FieldName, IndAj);
                        res = false;
                    }
                }
            }
            #endregion

            #region Cuenta Corporativa

            if (Field == "CuantaAlternaID")
            {
                if (Value.ToString() == this.empresa.ID.Value)
                    this.GetFieldConfigByFieldName("CuantaAlternaID").Editable = false;
            }
            #endregion

            #region TipoImpuesto
            if (fc.FieldName == "TipoImpuesto")
            {
                FieldConfiguration newFC = this.GetFieldConfigByFieldName(fc.FieldName);
                FieldConfiguration newFC2 = this.GetFieldConfigByFieldName("LugarGeograficoInd");
                if (newFC.Caption == "ReteICA")
                    newFC2.Editable = true;
                else
                    newFC2.Editable = false;
            }
            #endregion

            return res;
        }
        
        /// <summary>
        /// Metodo encargado de encapsular la insercion de registros de maestras simples
        /// </summary>
        /// <param name="tableName">Nombre de la tabla</param>
        /// <param name="insertList">Lista de dtos</param>
        /// <param name="userId">usuario logueado</param>
        /// <param name="documentId">docuemnto</param>
        /// <param name="accion">accion (insercion/importar)</param>
        /// <returns></returns>
        protected override DTO_TxResult DataAdd(List<DTO_MasterHierarchyBasic> insertList, int accion)
        {
            try
            {
                DTO_coPlanCuenta planCta = insertList.Count > 0 ? (DTO_coPlanCuenta)insertList[0] : null;
                int mascaraGrupoCta = Convert.ToInt32(planCta.MascaraCta.Value);
                int longTotalCta = this.table.CodeLength(this.table.LevelsUsed());
                if (planCta != null)
                {
                    if (planCta.ID.Value.Length == mascaraGrupoCta && mascaraGrupoCta < longTotalCta && mascaraGrupoCta != 0)
                    {
                        int[] nivelUsed = this.table.CompleteLevelLengths();
                        int[] longNivels = this.table.LevelLengths();
                        int c = -1;

                        string zero = string.Empty;
                        foreach (int nivel in nivelUsed)
                        {
                            c++;
                            if (nivel > mascaraGrupoCta)
                            {
                                DTO_coPlanCuenta ctaAux = new DTO_coPlanCuenta();
                                PropertyInfo[] propiedades = typeof(DTO_coPlanCuenta).GetProperties();
                                foreach (PropertyInfo pi in propiedades)
                                {
                                    Object o = pi.GetValue(planCta, null);
                                    if (o is UDT)
                                    {
                                        UDT udtOrigen = (UDT)o;
                                        PropertyInfo valueProperty = udtOrigen.GetType().GetProperty("Value");
                                        Object value = valueProperty.GetValue(udtOrigen, null);
                                        UDT udtDestino = (UDT)pi.GetValue(ctaAux, null);
                                        valueProperty.SetValue(udtDestino, value, null);
                                    }
                                }
                                for (int i = 0; i < longNivels[c]; i++)
                                    zero += "0";
                                ctaAux.ID.Value += zero;
                                if (ctaAux.ID.Value.Length == longTotalCta)
                                {
                                    ctaAux.MovInd.Value = true;
                                }
                                insertList.Add(ctaAux);
                            }
                        }
                    }
                }
                return base.DataAdd(insertList, Convert.ToInt32(FormsActions.Add));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Sobrecargar para modificar alguna configuración de campo de la maestra
        /// </summary>
        public override void CustomizeFieldsConfig()
        {
            string imp = string.Empty;
            try
            {
                string linPresIndDefault =(_bc.GetControlValueByCompany(ModulesPrefix.pl, AppControl.pl_ConsPresupuestalInd));
                if (linPresIndDefault.Equals("0"))
                    this.GetFieldConfigByFieldName("LineaPresupuestalInd").Editable = false;
                if (!_bc.AdministrationModel.MultiMoneda)
                    this.GetFieldConfigByFieldName("AjCambioTerceroInd").Editable = false;
                string empresaCorp = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_EmpresaCorporativa);
                
                if (empresaCorp != null)
                {
                    if (_bc.AdministrationModel.Empresa.ID.Value == empresaCorp)
                    {
                        this.GetFieldConfigByFieldName("CuentaAlternaID").Editable = false;   
                    }
                }
            }
            catch (Exception)
            {
                ;
            }
        }

        #endregion
    }
}
