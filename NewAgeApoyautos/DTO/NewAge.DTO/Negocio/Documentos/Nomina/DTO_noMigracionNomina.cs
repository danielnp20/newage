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
    public class DTO_noMigracionNomina
    {
        #region Contructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noMigracionNomina(IDataReader dr)
        {
            this.InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.TipoLiquidacion.Value = Convert.ToInt32(dr["TipoLiquidacion"].ToString());
                this.EmpleadoID.Value = dr["EmpleadoID"].ToString();
                this.PeriodoID.Value = Convert.ToDateTime(dr["PeriodoID"].ToString());
                this.ConceptoNOID.Value = dr["ConceptoNOID"].ToString();
                this.Dias.Value = Convert.ToInt32(dr["Dias"].ToString());
                this.Base.Value = Math.Round(Convert.ToDecimal(dr["Base"].ToString()), 1);
                this.Valor.Value = Math.Round(Convert.ToDecimal(dr["Valor"].ToString()), 4);
                this.OrigenConcepto.Value = Convert.ToByte(dr["OrigenConcepto"].ToString());
                this.FondoNOID.Value = dr["FondoNOID"].ToString();
                this.DiasVacacionesPagos.Value = Convert.ToInt32(dr["DiasVacacionesPagos"].ToString());
                this.DiasVacacionesTomados.Value = Convert.ToInt32(dr["DiasVacacionesTomados"].ToString());
                this.FechaSalidaVacaciones.Value = Convert.ToDateTime(dr["FechaSalidaVacaciones"].ToString());
                this.FechaLLegadaVacaciones.Value = Convert.ToDateTime(dr["FechaLLegadaVacaciones"].ToString());
               
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noMigracionNomina()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.TipoLiquidacion = new UDTSQL_int();
            this.EmpleadoID = new UDT_EmpleadoID();
            this.PeriodoID = new UDT_PeriodoID();
            this.ConceptoNOID = new UDT_ConceptoNOID();
            this.Dias = new UDTSQL_int();
            this.Base = new UDT_Valor();
            this.Valor = new UDT_Valor();
            this.OrigenConcepto = new UDTSQL_tinyint();
            this.FondoNOID = new UDT_FondoNOID();
            this.DiasVacacionesPagos = new UDTSQL_int();
            this.DiasVacacionesTomados = new UDTSQL_int();
            this.FechaSalidaVacaciones = new UDTSQL_smalldatetime();
            this.FechaLLegadaVacaciones = new UDTSQL_smalldatetime();   
        }

        #endregion
        #region Propiedades

        [DataMember]
        [NotImportable]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDTSQL_int TipoLiquidacion { get; set; }
        
        [DataMember]
        public UDT_EmpleadoID EmpleadoID { get; set; }

        [DataMember]
        public UDT_PeriodoID PeriodoID { get; set; }

        [DataMember]
        public UDT_ConceptoNOID ConceptoNOID { get; set; }

        [DataMember]
        public UDTSQL_int Dias { get; set; }

        [DataMember]
        public UDT_Valor Base { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDTSQL_tinyint OrigenConcepto { get; set; }

        [DataMember]
        public UDT_FondoNOID FondoNOID { get; set; }

        [DataMember]
        public UDTSQL_int DiasVacacionesPagos { get; set; }

        [DataMember]
        public UDTSQL_int DiasVacacionesTomados { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaSalidaVacaciones { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaLLegadaVacaciones { get; set; }

        #endregion
    }
}
