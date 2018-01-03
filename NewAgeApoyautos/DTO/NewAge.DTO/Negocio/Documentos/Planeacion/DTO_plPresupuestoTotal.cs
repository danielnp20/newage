using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_plPresupuestoTotal
    {
        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_plPresupuestoTotal()
        {
            this.InitCols();
        }

        public DTO_plPresupuestoTotal(IDataReader dr)
        {
            InitCols();
            try
            {
                this.ProyectoID.Value = dr["ProyectoID"].ToString();
                this.Ano.Value = Convert.ToInt32(dr["Ano"]);
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
                this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                this.ValorLoc00.Value = Convert.ToDecimal(dr["ValorLoc00"]);
                this.ValorLoc01.Value = Convert.ToDecimal(dr["ValorLoc01"]);
                this.ValorLoc02.Value = Convert.ToDecimal(dr["ValorLoc02"]);
                this.ValorLoc03.Value = Convert.ToDecimal(dr["ValorLoc03"]);
                this.ValorLoc04.Value = Convert.ToDecimal(dr["ValorLoc04"]);
                this.ValorLoc05.Value = Convert.ToDecimal(dr["ValorLoc05"]);
                this.ValorLoc06.Value = Convert.ToDecimal(dr["ValorLoc06"]);
                this.ValorLoc07.Value = Convert.ToDecimal(dr["ValorLoc07"]);
                this.ValorLoc08.Value = Convert.ToDecimal(dr["ValorLoc08"]);
                this.ValorLoc09.Value = Convert.ToDecimal(dr["ValorLoc09"]);
                this.ValorLoc10.Value = Convert.ToDecimal(dr["ValorLoc10"]);
                this.ValorLoc11.Value = Convert.ToDecimal(dr["ValorLoc11"]);
                this.ValorLoc12.Value = Convert.ToDecimal(dr["ValorLoc12"]);
                this.EquivExt00.Value = Convert.ToDecimal(dr["EquivExt00"]);
                this.EquivExt01.Value = Convert.ToDecimal(dr["EquivExt01"]);
                this.EquivExt02.Value = Convert.ToDecimal(dr["EquivExt02"]);
                this.EquivExt03.Value = Convert.ToDecimal(dr["EquivExt03"]);
                this.EquivExt04.Value = Convert.ToDecimal(dr["EquivExt04"]);
                this.EquivExt05.Value = Convert.ToDecimal(dr["EquivExt05"]);
                this.EquivExt06.Value = Convert.ToDecimal(dr["EquivExt06"]);
                this.EquivExt07.Value = Convert.ToDecimal(dr["EquivExt07"]);
                this.EquivExt08.Value = Convert.ToDecimal(dr["EquivExt08"]);
                this.EquivExt09.Value = Convert.ToDecimal(dr["EquivExt09"]);
                this.EquivExt10.Value = Convert.ToDecimal(dr["EquivExt10"]);
                this.EquivExt11.Value = Convert.ToDecimal(dr["EquivExt11"]);
                this.EquivExt12.Value = Convert.ToDecimal(dr["EquivExt12"]);
                this.ValorExt00.Value = Convert.ToDecimal(dr["ValorExt00"]); 
                this.ValorExt01.Value = Convert.ToDecimal(dr["ValorExt01"]);
                this.ValorExt02.Value = Convert.ToDecimal(dr["ValorExt02"]);
                this.ValorExt03.Value = Convert.ToDecimal(dr["ValorExt03"]);
                this.ValorExt04.Value = Convert.ToDecimal(dr["ValorExt04"]);
                this.ValorExt05.Value = Convert.ToDecimal(dr["ValorExt05"]);
                this.ValorExt06.Value = Convert.ToDecimal(dr["ValorExt06"]);
                this.ValorExt07.Value = Convert.ToDecimal(dr["ValorExt07"]);
                this.ValorExt08.Value = Convert.ToDecimal(dr["ValorExt08"]);
                this.ValorExt09.Value = Convert.ToDecimal(dr["ValorExt09"]);
                this.ValorExt10.Value = Convert.ToDecimal(dr["ValorExt10"]);
                this.ValorExt11.Value = Convert.ToDecimal(dr["ValorExt11"]);
                this.ValorExt12.Value = Convert.ToDecimal(dr["ValorExt12"]);
                this.EquivLoc00.Value = Convert.ToDecimal(dr["EquivLoc00"]);
                this.EquivLoc01.Value = Convert.ToDecimal(dr["EquivLoc01"]);
                this.EquivLoc02.Value = Convert.ToDecimal(dr["EquivLoc02"]);
                this.EquivLoc03.Value = Convert.ToDecimal(dr["EquivLoc03"]);
                this.EquivLoc04.Value = Convert.ToDecimal(dr["EquivLoc04"]);
                this.EquivLoc05.Value = Convert.ToDecimal(dr["EquivLoc05"]);
                this.EquivLoc06.Value = Convert.ToDecimal(dr["EquivLoc06"]);
                this.EquivLoc07.Value = Convert.ToDecimal(dr["EquivLoc07"]);
                this.EquivLoc08.Value = Convert.ToDecimal(dr["EquivLoc08"]);
                this.EquivLoc09.Value = Convert.ToDecimal(dr["EquivLoc09"]);
                this.EquivLoc10.Value = Convert.ToDecimal(dr["EquivLoc10"]);
                this.EquivLoc11.Value = Convert.ToDecimal(dr["EquivLoc11"]);
                this.EquivLoc12.Value = Convert.ToDecimal(dr["EquivLoc12"]);
                this.VlrSaldoAntLoc.Value = 0;
                this.VlrMvtoLocal.Value = 0;
                this.VlrNuevoSaldoLoc.Value = 0;
                this.VlrSaldoAntExtr.Value = 0;
                this.VlrMvtoExtr.Value = 0;
                this.VlrNuevoSaldoExtr.Value = 0;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.Ano = new UDTSQL_int();
            this.EmpresaID = new UDT_EmpresaID();
            this.ProyectoID = new UDT_ProyectoID();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.LineaPresupuestoID = new UDT_LineaPresupuestoID();
            this.ValorLoc00 = new UDT_Valor(); 
            this.ValorLoc01 = new UDT_Valor();
            this.ValorLoc02 = new UDT_Valor();
            this.ValorLoc03 = new UDT_Valor();
            this.ValorLoc04 = new UDT_Valor();
            this.ValorLoc05 = new UDT_Valor();
            this.ValorLoc06 = new UDT_Valor();
            this.ValorLoc07 = new UDT_Valor();
            this.ValorLoc08 = new UDT_Valor();
            this.ValorLoc09 = new UDT_Valor();
            this.ValorLoc10 = new UDT_Valor();
            this.ValorLoc11 = new UDT_Valor();
            this.ValorLoc12 = new UDT_Valor();
            this.EquivExt00 = new UDT_Valor();
            this.EquivExt01 = new UDT_Valor();
            this.EquivExt02 = new UDT_Valor();
            this.EquivExt03 = new UDT_Valor();
            this.EquivExt04 = new UDT_Valor();
            this.EquivExt05 = new UDT_Valor();
            this.EquivExt06 = new UDT_Valor();
            this.EquivExt07 = new UDT_Valor();
            this.EquivExt08 = new UDT_Valor();
            this.EquivExt09 = new UDT_Valor();
            this.EquivExt10 = new UDT_Valor();
            this.EquivExt11 = new UDT_Valor();
            this.EquivExt12 = new UDT_Valor();
            this.ValorExt00 = new UDT_Valor();
            this.ValorExt01 = new UDT_Valor();
            this.ValorExt02 = new UDT_Valor();
            this.ValorExt03 = new UDT_Valor();
            this.ValorExt04 = new UDT_Valor();
            this.ValorExt05 = new UDT_Valor();
            this.ValorExt06 = new UDT_Valor();
            this.ValorExt07 = new UDT_Valor();
            this.ValorExt08 = new UDT_Valor();
            this.ValorExt09 = new UDT_Valor();
            this.ValorExt10 = new UDT_Valor();
            this.ValorExt11 = new UDT_Valor();
            this.ValorExt12 = new UDT_Valor();
            this.EquivLoc00 = new UDT_Valor();
            this.EquivLoc01 = new UDT_Valor();
            this.EquivLoc02 = new UDT_Valor();
            this.EquivLoc03 = new UDT_Valor();
            this.EquivLoc04 = new UDT_Valor();
            this.EquivLoc05 = new UDT_Valor();
            this.EquivLoc06 = new UDT_Valor();
            this.EquivLoc07 = new UDT_Valor();
            this.EquivLoc08 = new UDT_Valor();
            this.EquivLoc09 = new UDT_Valor();
            this.EquivLoc10 = new UDT_Valor();
            this.EquivLoc11 = new UDT_Valor();
            this.EquivLoc12 = new UDT_Valor();
            //Adicionales
            this.VlrSaldoAntLoc = new UDT_Valor();
            this.VlrMvtoLocal = new UDT_Valor();
            this.VlrNuevoSaldoLoc = new UDT_Valor();
            this.VlrSaldoAntExtr = new UDT_Valor();
            this.VlrMvtoExtr = new UDT_Valor();
            this.VlrNuevoSaldoExtr = new UDT_Valor();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDTSQL_int Ano { get; set; }

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        public UDT_LineaPresupuestoID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_Valor ValorLoc00 { get; set; }

        [DataMember]
        public UDT_Valor ValorLoc01 { get; set; }

        [DataMember]
        public UDT_Valor ValorLoc02 { get; set; }

        [DataMember]
        public UDT_Valor ValorLoc03 { get; set; }

        [DataMember]
        public UDT_Valor ValorLoc04 { get; set; }

        [DataMember]
        public UDT_Valor ValorLoc05 { get; set; }

        [DataMember]
        public UDT_Valor ValorLoc06 { get; set; }

        [DataMember]
        public UDT_Valor ValorLoc07 { get; set; }

        [DataMember]
        public UDT_Valor ValorLoc08 { get; set; }

        [DataMember]
        public UDT_Valor ValorLoc09 { get; set; }

        [DataMember]
        public UDT_Valor ValorLoc10 { get; set; }

        [DataMember]
        public UDT_Valor ValorLoc11 { get; set; }

        [DataMember]
        public UDT_Valor ValorLoc12 { get; set; }

        [DataMember]
        public UDT_Valor EquivExt00 { get; set; }

        [DataMember]
        public UDT_Valor EquivExt01 { get; set; }

        [DataMember]
        public UDT_Valor EquivExt02 { get; set; }

        [DataMember]
        public UDT_Valor EquivExt03 { get; set; }

        [DataMember]
        public UDT_Valor EquivExt04 { get; set; }

        [DataMember]
        public UDT_Valor EquivExt05 { get; set; }

        [DataMember]
        public UDT_Valor EquivExt06 { get; set; }

        [DataMember]
        public UDT_Valor EquivExt07 { get; set; }

        [DataMember]
        public UDT_Valor EquivExt08 { get; set; }

        [DataMember]
        public UDT_Valor EquivExt09 { get; set; }

        [DataMember]
        public UDT_Valor EquivExt10 { get; set; }

        [DataMember]
        public UDT_Valor EquivExt11 { get; set; }

        [DataMember]
        public UDT_Valor EquivExt12 { get; set; }
        
        [DataMember]
        public UDT_Valor ValorExt00 { get; set; }
        
        [DataMember]
        public UDT_Valor ValorExt01 { get; set; }

        [DataMember]
        public UDT_Valor ValorExt02 { get; set; }

        [DataMember]
        public UDT_Valor ValorExt03 { get; set; }

        [DataMember]
        public UDT_Valor ValorExt04 { get; set; }

        [DataMember]
        public UDT_Valor ValorExt05 { get; set; }

        [DataMember]
        public UDT_Valor ValorExt06 { get; set; }

        [DataMember]
        public UDT_Valor ValorExt07 { get; set; }

        [DataMember]
        public UDT_Valor ValorExt08 { get; set; }

        [DataMember]
        public UDT_Valor ValorExt09 { get; set; }

        [DataMember]
        public UDT_Valor ValorExt10 { get; set; }

        [DataMember]
        public UDT_Valor ValorExt11 { get; set; }

        [DataMember]
        public UDT_Valor ValorExt12 { get; set; }

        [DataMember]
        public UDT_Valor EquivLoc00 { get; set; }

        [DataMember]
        public UDT_Valor EquivLoc01 { get; set; }

        [DataMember]
        public UDT_Valor EquivLoc02 { get; set; }

        [DataMember]
        public UDT_Valor EquivLoc03 { get; set; }

        [DataMember]
        public UDT_Valor EquivLoc04 { get; set; }

        [DataMember]
        public UDT_Valor EquivLoc05 { get; set; }

        [DataMember]
        public UDT_Valor EquivLoc06 { get; set; }

        [DataMember]
        public UDT_Valor EquivLoc07 { get; set; }

        [DataMember]
        public UDT_Valor EquivLoc08 { get; set; }

        [DataMember]
        public UDT_Valor EquivLoc09 { get; set; }

        [DataMember]
        public UDT_Valor EquivLoc10 { get; set; }

        [DataMember]
        public UDT_Valor EquivLoc11 { get; set; }

        [DataMember]
        public UDT_Valor EquivLoc12 { get; set; }

        [DataMember]
        [NotImportable]
        [AllowNull]
        public UDT_Valor VlrSaldoAntLoc { get; set; }

        [DataMember]
        [NotImportable]
        [AllowNull]
        public UDT_Valor VlrSaldoAntExtr { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor VlrMvtoLocal { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor VlrMvtoExtr { get; set; }

        [DataMember]
        [NotImportable]
        [AllowNull]
        public UDT_Valor VlrNuevoSaldoLoc { get; set; }

        [DataMember]
        [NotImportable]
        [AllowNull]
        public UDT_Valor VlrNuevoSaldoExtr { get; set; }

        #endregion
    }
}
