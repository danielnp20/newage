using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_ReportEquiposArrendados
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ReportEquiposArrendados(IDataReader dr)
        {
            this.InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["PlaquetaID"].ToString()))
                    this.PlaquetaID.Value = Convert.ToString(dr["PlaquetaID"]);
                if (!string.IsNullOrEmpty(dr["Descripcion"].ToString()))
                    this.Descripcion.Value = Convert.ToString(dr["Descripcion"]);
                if (!string.IsNullOrEmpty(dr["SerialID"].ToString()))
                    this.SerialID.Value = Convert.ToString(dr["SerialID"]);
                if (!string.IsNullOrEmpty(dr["Localizacion"].ToString()))
                    this.Localizacion.Value = Convert.ToString(dr["Localizacion"]);
                if (!string.IsNullOrEmpty(dr["NotaEnvio"].ToString()))
                    this.NotaEnvio.Value = Convert.ToString(dr["NotaEnvio"]);
                if (!string.IsNullOrEmpty(dr["FechaEntrega"].ToString()))
                    this.FechaEntrega.Value = Convert.ToDateTime(dr["FechaEntrega"]);
                if (!string.IsNullOrEmpty(dr["TerceroID"].ToString()))
                    this.TerceroID.Value = Convert.ToString(dr["TerceroID"]);
                if (!string.IsNullOrEmpty(dr["TipoRef"].ToString()))
                    this.TipoRef.Value = Convert.ToString(dr["TipoRef"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportEquiposArrendados()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.PlaquetaID = new UDT_PlaquetaID();
            this.Descripcion = new UDT_DescripTExt();
            this.SerialID = new UDT_SerialID();
            this.Localizacion = new UDT_LocFisicaID();
            this.NotaEnvio = new UDTSQL_char(10);
            this.FechaEntrega = new UDTSQL_datetime();
            this.Rompimiento = new UDTSQL_char(20);
            this.TerceroID = new UDT_TerceroID();
            this.TipoRef = new UDT_ReferenciaID();
        }
        #endregion

        #region Propiedades

        //Fijas
        [DataMember]
        public UDT_PlaquetaID PlaquetaID { get; set; }
        
        [DataMember]
        public UDT_DescripTExt Descripcion { get; set; }
        
        [DataMember]
        public UDT_SerialID SerialID { get; set; }
        
        [DataMember]
        public UDT_LocFisicaID Localizacion { get; set; }
        
        [DataMember]
        public UDTSQL_char NotaEnvio { get; set; }
        
        [DataMember]
        public UDTSQL_datetime FechaEntrega { get; set; }
        
        // Rompimientos

        [DataMember]
        public UDTSQL_char Rompimiento { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_Descriptivo DescriptivoTer { get; set; }

        [DataMember]
        public UDT_ReferenciaID TipoRef { get; set; }

        [DataMember]
        public UDT_Descriptivo DescriptivoRef { get; set; }

        #endregion
    }
}
