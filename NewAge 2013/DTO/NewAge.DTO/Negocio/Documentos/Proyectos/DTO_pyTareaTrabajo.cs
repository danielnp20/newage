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
    /// Models DTO_pyTareaTrabajo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pyTareaTrabajo
    {
        #region pyTareaTrabajo

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pyTareaTrabajo(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.NumServiciodeta.Value = Convert.ToInt32(dr["NumServiciodeta"]);
                this.ResponsableEMP.Value = Convert.ToString(dr["ResponsableEMP"]);
                this.TipoTercero.Value = Convert.ToByte(dr["TipoTercero"]);

                if (!string.IsNullOrWhiteSpace(dr["FechaTerminadoPRO"].ToString()))
                    this.FechaTerminadoPRO.Value = Convert.ToDateTime(dr["FechaTerminadoPRO"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaInicio"].ToString()))
                    this.FechaInicio.Value = Convert.ToDateTime(dr["FechaInicio"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaTerminado"].ToString()))
                    this.FechaTerminado.Value = Convert.ToDateTime(dr["FechaTerminado"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaCierre"].ToString()))
                    this.FechaCierre.Value = Convert.ToDateTime(dr["FechaCierre"]);
                this.Estado.Value = Convert.ToByte(dr["Estado"]);
                if (!string.IsNullOrWhiteSpace(dr["Observaciones"].ToString()))
                    this.Observaciones.Value = Convert.ToString(dr["Observaciones"]);
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
        public DTO_pyTareaTrabajo()
        {
            InitCols();
        }

        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.NumServiciodeta = new UDT_Consecutivo();
            this.ResponsableEMP = new UDT_TerceroID();
            this.TipoTercero = new UDTSQL_tinyint();
            this.FechaInicioPRO = new UDTSQL_smalldatetime();
            this.FechaTerminadoPRO = new UDTSQL_smalldatetime();
            this.FechaInicio = new UDTSQL_smalldatetime();
            this.FechaTerminado = new UDTSQL_smalldatetime();
            this.FechaCierre = new UDTSQL_smalldatetime();
            this.Estado = new UDTSQL_tinyint();
            this.Observaciones = new UDT_DescripTExt();
        }

        #endregion
        
        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }
        
        [DataMember]
        public UDT_Consecutivo NumServiciodeta { get; set; }
        
        [DataMember]
        public UDT_TerceroID ResponsableEMP { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint TipoTercero { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime FechaInicioPRO { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime FechaTerminadoPRO { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime FechaInicio { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime FechaTerminado { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime FechaCierre { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint Estado { get; set; }
        
        [DataMember]
        public UDT_DescripTExt Observaciones { get; set; }
    }
}
