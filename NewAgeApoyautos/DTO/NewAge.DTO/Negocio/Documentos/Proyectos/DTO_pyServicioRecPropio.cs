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
    /// 
    /// Models DTO_pyServicioRecPropio
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pyServicioRecPropio
    {
        #region DTO_pyServicioRecPropio

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pyServicioRecPropio(IDataReader dr)
        {
            InitCols();
            try
            {
                    
                this.ConsecDetalle.Value = Convert.ToInt32(dr["ConsecDetalle"]);
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["EmpleadoID"].ToString()))
                    this.EmpleadoID.Value = Convert.ToString(dr["EmpleadoID"]);
                if (!string.IsNullOrWhiteSpace(dr["ActivoID"].ToString()))
                    this.ActivoID.Value = Convert.ToInt32(dr["ActivoID"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaInicia"].ToString()))
                    this.FechaInicia.Value = Convert.ToDateTime(dr["FechaInicia"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaTermina"].ToString()))
                    this.FechaTermina.Value = Convert.ToDateTime(dr["FechaTermina"]);
                if (!string.IsNullOrWhiteSpace(dr["Observaciones"].ToString()))
                    this.Observaciones.Value = Convert.ToString(dr["Observaciones"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
             
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pyServicioRecPropio()
        {
            InitCols();
        }

        public void InitCols()
        {
            this.ConsecDetalle = new UDT_Consecutivo();
            this.NumeroDoc = new UDT_Consecutivo();
            this.EmpleadoID = new UDT_EmpleadoID();
            this.ActivoID = new UDT_ActivoID();
            this.FechaInicia = new UDTSQL_smalldatetime();
            this.FechaTermina = new UDTSQL_smalldatetime();
            this.Observaciones = new UDT_DescripTExt();
            this.Consecutivo = new UDT_Consecutivo();
        }

        #endregion

        #region Propiedades
        [DataMember]
        public UDT_Consecutivo ConsecDetalle { get; set; }
        
        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }
        
        [DataMember]
        public UDT_EmpleadoID EmpleadoID { get; set; }
        
        [DataMember]
        public UDT_ActivoID ActivoID { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime FechaInicia { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime FechaTermina { get; set; }
        
        [DataMember]
        public UDT_DescripTExt Observaciones { get; set; }
        
        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        #endregion 

    }
}
