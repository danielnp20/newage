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
    public class DTO_ReportImportacionesTemporales
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ReportImportacionesTemporales(IDataReader dr)
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
                if (!string.IsNullOrEmpty(dr["FechaVencimiento"].ToString()))
                    this.FechaVencimiento.Value = Convert.ToDateTime(dr["FechaVencimiento"]);
                if (!string.IsNullOrEmpty(dr["FechaImportacion"].ToString()))
                    this.FechaImportacion.Value = Convert.ToDateTime(dr["FechaImportacion"]);
                if (!string.IsNullOrEmpty(dr["TipoRef"].ToString()))
                    this.TipoRef.Value = Convert.ToString(dr["TipoRef"]);
                if (!string.IsNullOrEmpty(dr["DescriptivoRef"].ToString()))
                    this.DescriptivoRef.Value = Convert.ToString(dr["DescriptivoRef"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportImportacionesTemporales()
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
            this.FechaImportacion = new UDTSQL_datetime();
            this.FechaVencimiento = new UDTSQL_datetime();
            this.Rompimiento = new UDTSQL_char(20);
            this.TipoRef = new UDT_ReferenciaID();
            this.DescriptivoRef = new UDT_Descriptivo();
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
        public UDTSQL_datetime FechaImportacion { get; set; }
        
        [DataMember]
        public UDTSQL_datetime FechaVencimiento { get; set; }
        
        // Rompimientos

        [DataMember]
        public object Rompimiento { get; set; }

        [DataMember]
        public UDT_ReferenciaID TipoRef { get; set; }

        [DataMember]
        public UDT_Descriptivo DescriptivoRef { get; set; }

        #endregion
    }
}
