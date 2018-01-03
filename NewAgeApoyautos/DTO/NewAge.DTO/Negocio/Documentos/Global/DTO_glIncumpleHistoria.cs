using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;

namespace NewAge.DTO.Negocio
{
    public class DTO_glIncumpleHistoria
    {
        #region Constructor

		/// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glIncumpleHistoria(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.FechaProgramada.Value = Convert.ToDateTime(dr["FechaProgramada"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaEjecuta"].ToString()))
                    this.FechaEjecuta.Value = Convert.ToDateTime(dr["FechaEjecuta"]);
                if (!string.IsNullOrWhiteSpace(dr["MovimientoTipo"].ToString()))
                    this.MovimientoTipo.Value = Convert.ToByte(dr["MovimientoTipo"]);
                this.ActividadFlujoID.Value = Convert.ToString(dr["ActividadFlujoID"]);
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
        public DTO_glIncumpleHistoria()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Consecutivo = new UDT_Consecutivo();
            this.NumeroDoc = new UDT_Consecutivo();
            
        }

	    #endregion
        
        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime FechaProgramada { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime FechaEjecuta { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint MovimientoTipo { get; set; }
        
        [DataMember]
        public UDT_ActividadFlujoID ActividadFlujoID { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }
        #endregion

    }
}
