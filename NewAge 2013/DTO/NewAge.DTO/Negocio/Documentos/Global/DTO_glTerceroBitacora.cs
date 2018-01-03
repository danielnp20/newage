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
    public class DTO_glTerceroBitacora
    {
        #region Constructor

		/// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glTerceroBitacora(IDataReader dr)
        {
            InitCols();
            try
            {

                this.TerceroID.Value = Convert.ToString(dr["TerceroID"]);
                this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                if (!string.IsNullOrWhiteSpace(dr["Observacion"].ToString()))
                    this.Observacion.Value = Convert.ToString(dr["Observacion"]);
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
        public DTO_glTerceroBitacora()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TerceroID = new UDT_TerceroID();
            this.Fecha = new UDTSQL_smalldatetime();
            this.Observacion = new UDT_DescripTExt();
            this.Consecutivo = new UDT_Consecutivo();
        }

	    #endregion
        
        #region Propiedades

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime Fecha { get; set; }
        
        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        #endregion

    }
}
