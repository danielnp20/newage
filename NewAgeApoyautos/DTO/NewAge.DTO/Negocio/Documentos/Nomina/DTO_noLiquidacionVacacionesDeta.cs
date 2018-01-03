using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_noLiquidacionVacacionesDeta
    {
        #region Contructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noLiquidacionVacacionesDeta(IDataReader dr)
        {
            this.InitCols();
            try
            {               
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.EmpleadoID.Value = dr["EmpleadoID"].ToString();
                this.PeriodoInicial.Value = Convert.ToDateTime(dr["PeriodoInicial"].ToString());
                this.PeriodoFinal.Value = Convert.ToDateTime(dr["PeriodoFinal"].ToString());
                this.DiasLegales.Value = Convert.ToInt32(dr["DiasLegales"].ToString());
                this.DiasVacaTomados.Value = Convert.ToInt32(dr["DiasVacaTomados"].ToString());
                this.DiasVacaPagados.Value = Convert.ToInt32(dr["DiasVacaPagados"].ToString());
                this.Estado.Value = Convert.ToBoolean(dr["Estado"]);
                if(!string.IsNullOrEmpty((dr["FechaPago"].ToString())))
                    this.FechaPago.Value = Convert.ToDateTime(dr["FechaPago"].ToString());
                
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noLiquidacionVacacionesDeta()
        {
            this.InitCols();
        }

          /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.EmpleadoID = new UDT_EmpleadoID();
            this.PeriodoInicial = new UDTSQL_smalldatetime();
            this.PeriodoFinal = new UDTSQL_smalldatetime();
            this.DiasLegales = new UDT_Consecutivo();
            this.DiasVacaPagados = new UDT_Consecutivo();
            this.DiasVacaTomados = new UDT_Consecutivo();
            this.Estado = new UDT_SiNo();
            this.FechaPago = new UDTSQL_smalldatetime();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_EmpleadoID EmpleadoID { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime PeriodoInicial { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime PeriodoFinal { get; set; }

        [DataMember]
        public UDT_Consecutivo DiasLegales { get; set; }

        [DataMember]
        public UDT_Consecutivo DiasVacaTomados { get; set; }

        [DataMember]
        public UDT_Consecutivo DiasVacaPagados { get; set; }

        [DataMember]
        public UDT_SiNo Estado { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaPago { get; set; }
        
        
        #endregion
    }
}
