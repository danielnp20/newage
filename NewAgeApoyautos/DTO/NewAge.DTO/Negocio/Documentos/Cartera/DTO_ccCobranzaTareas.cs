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
    /// Models DTO_ccCompradorFinalDeta
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccCobranzaTareas
    {
        #region ccCobranzaTareas

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccCobranzaTareas(IDataReader dr)
        {
            InitCols();
            try
            {
                this.ClienteID.Value = Convert.ToString(dr["ClienteID"]);
                this.Tarea.Value = Convert.ToString(dr["Tarea"]);
                this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["Observaciones"].ToString()))
                    this.Observaciones.Value = Convert.ToString(dr["Observaciones"]);
                if (!string.IsNullOrWhiteSpace(dr["ProcesadoIND"].ToString()))
                    this.ProcesadoIND.Value = Convert.ToBoolean(dr["ProcesadoIND"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccCobranzaTareas()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.ClienteID = new UDT_ClienteID();
            this.Tarea = new UDT_TareaID();
            this.Fecha = new UDTSQL_smalldatetime();
            this.NumeroDoc = new UDT_Consecutivo();
            this.Observaciones = new UDT_DescripTExt();
            this.ProcesadoIND = new UDT_SiNo();
            this.Consecutivo = new UDT_Consecutivo();
        }

        #endregion

        #region Propiedades
        
        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }
        
        [DataMember]
        public UDT_TareaID Tarea { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime Fecha { get; set; }
        
        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }
                
        [DataMember]
        public UDT_DescripTExt Observaciones { get; set; }

        [DataMember]
        public UDT_SiNo ProcesadoIND { get; set; }
        
        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        #endregion
    }
}
