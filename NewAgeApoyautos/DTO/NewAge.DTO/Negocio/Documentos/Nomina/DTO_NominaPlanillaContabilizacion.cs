using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Nomina para aprobacion:
    /// Models DTO_NominaAprobacion
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_NominaPlanillaContabilizacion
    {
        #region DTO_NominaPlanillaContabilizacion

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_NominaPlanillaContabilizacion(IDataReader dr)
        {
            this.InitCols();
            this.TerceroID.Value = dr["TerceroID"].ToString();
            this.TerceroDesc.Value = dr["TerceroDesc"].ToString();
            this.CuentaID.Value = dr["CuentaID"].ToString();
            this.ConceptoCargoID.Value = dr["ConceptoCargoID"].ToString();
            this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
            this.Valor.Value = Math.Round(Convert.ToDecimal(dr["Valor"]),4);
            this.Valor2.Value = Math.Round(Convert.ToDecimal(dr["Valor2"]), 4);
            this.Liquidacion.Value = Convert.ToByte(dr["Liquidacion"]);
            if(!string.IsNullOrEmpty(dr["ConceptoNOID"].ToString()))
                this.ConceptoNOID.Value = dr["ConceptoNOID"].ToString();
            this.EmpleadoID.Value = dr["EmpleadoID"].ToString();
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_NominaPlanillaContabilizacion()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.Seleccionar = new UDT_SiNo();
            this.TerceroID = new UDT_TerceroID();
            this.TerceroDesc = new UDT_DescripTBase();
            this.CuentaID = new UDT_CuentaID();
            this.CuentaCtpID = new UDT_CuentaID();
            this.ConceptoCargoID = new UDT_ConceptoCargoID();
            this.LineaPresupuestoID = new UDT_LineaPresupuestoID();    
            this.Valor = new UDT_Valor();
            this.Valor2 = new UDT_Valor();
            this.Liquidacion = new UDTSQL_tinyint();
            this.ConceptoNOID = new UDT_ConceptoNOID();
            this.Total = new UDT_Valor();
            this.EmpleadoID = new UDT_EmpleadoID();
            this.EmpleadoDesc = new UDT_DescripTBase();
            this.Detalle = new List<DTO_NominaPlanillaContabilizacion>();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_SiNo Seleccionar { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_DescripTBase TerceroDesc { get; set; }

        [DataMember]
        public UDT_CuentaID CuentaID { get; set; }

        [DataMember]
        public UDT_CuentaID CuentaCtpID { get; set; }
        
        [DataMember]
        public UDT_ConceptoCargoID ConceptoCargoID { get; set; }

        [DataMember]
        public UDT_LineaPresupuestoID LineaPresupuestoID { get; set; }
        
        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_Valor Valor2 { get; set; }

        [DataMember]
        public UDT_Valor Total { get; set; }

        [DataMember]
        public UDTSQL_tinyint Liquidacion { get; set; }

        [DataMember]
        public UDT_ConceptoNOID ConceptoNOID { get; set; }

        [DataMember]
        public UDT_EmpleadoID EmpleadoID { get; set; }

        [DataMember]
        public UDT_DescripTBase EmpleadoDesc { get; set; }

        [DataMember]
        public List<DTO_NominaPlanillaContabilizacion> Detalle { get; set; }

        #endregion
    }
}
