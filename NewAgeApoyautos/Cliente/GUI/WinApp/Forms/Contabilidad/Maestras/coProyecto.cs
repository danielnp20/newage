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
    class coProyecto : MasterHierarchyForm
    {
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        
        ///<summary>
        /// Constructor 
        /// </summary>
        public coProyecto() : base() { }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.coProyecto;
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
            FieldConfiguration newFC = this.GetFieldConfigByFieldName(fieldName);
            if (fieldName == "FAdpertura")
            {
                newFC.Editable = false;
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
            
            //Valida que la fecha de cierre elegida que no se inferior a la de fecha de apertura 
            if (fc.FieldName == "FCierre")
            {
                FieldConfiguration newFC = this.GetFieldConfigByFieldName("FApertura");
                if (newFC != null && Value != null)
                {
                    try
                    {
                        DateTime v = Convert.ToDateTime(Value);
                        DateTime date = Convert.ToDateTime(this.GetEditRow(newFC.ColumnIndex).Valor);
                        if (v < date)
                        {
                            err = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_Proyecto_FCierre_NotLess);
                            res = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        ;
                    }
                }
            }
            //Valida que la fecha de apertura elegida que no sea superior a la de fecha de cierre
            if (fc.FieldName == "FApertura")
            {
                FieldConfiguration newFC = this.GetFieldConfigByFieldName("FCierre");
                if (newFC != null && Value != null)
                {
                    try
                    {
                        DateTime v = Convert.ToDateTime(Value);
                        DateTime date = Convert.ToDateTime(this.GetEditRow(newFC.ColumnIndex).Valor);
                        if (v > date)
                        {
                            err = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Co_Proyecto_FApertura_NotSuperior);
                            res = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        ;
                    }
                }
            }
            if (fc.FieldName == "ProyectoTipo")
            {
                byte tipoProy = Convert.ToByte(Value);
                if (tipoProy == (byte)ProyectoTipo.Opex)
                {
                    FieldConfiguration newFC3 = this.GetFieldConfigByFieldName("PresupuestalInd");
                    this.SetEditGridValue(newFC3.ColumnIndex, bool.TrueString);
                }
            }
            if (fc.FieldName == "PresupuestalInd")
            {
                FieldConfiguration newFC = this.GetFieldConfigByFieldName("ProyectoTipo");
                try
                {
                    byte tipoProy = Convert.ToByte(this.GetEditRow(newFC.ColumnIndex).Valor);
                    bool valueCurrent = Convert.ToBoolean(Value);
                    if (!valueCurrent && tipoProy == (byte)ProyectoTipo.Opex)
                    {
                        this.SetEditGridValue(fc.ColumnIndex, bool.TrueString);
                        err = _bc.GetResource(LanguageTypes.Messages, "Los proyectos de Tipo Opex deben ser de Presupuesto");
                        res = false;
                    }
                }
                catch (Exception)
                {
                    ;
                }
            }
            return res;
        }
        #endregion
    }
}
