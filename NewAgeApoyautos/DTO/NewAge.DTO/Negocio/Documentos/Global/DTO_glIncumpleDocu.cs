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
    public class DTO_glIncumpleDocu
    {
        #region Constructor

		/// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glIncumpleDocu(IDataReader dr)
        {
            InitCols();
            try
            {            
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.NumDocIncumplido.Value = Convert.ToInt32(dr["NumDocIncumplido"]);
                this.EmpresaID.Value = Convert.ToString(dr["EmpresaID"]);
                this.DocumentoID.Value = Convert.ToInt32(dr["DocumentoID"]);
                this.FechaINI.Value = Convert.ToDateTime(dr["FechaINI"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaFIN"].ToString()))
                    this.FechaFIN.Value = Convert.ToDateTime(dr["FechaFIN"]);
                if (!string.IsNullOrWhiteSpace(dr["RespTipo"].ToString()))
                    this.RespTipo.Value = Convert.ToByte(dr["RespTipo"]);
                if (!string.IsNullOrWhiteSpace(dr["RespTercero"].ToString()))
                    this.RespTercero.Value = Convert.ToString(dr["RespTercero"]);
                this.EtapaIncumplimiento.Value = Convert.ToString(dr["EtapaIncumplimiento"]);
                
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glIncumpleDocu()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.NumDocIncumplido = new UDT_Consecutivo();
            this.EmpresaID = new UDT_EmpresaID();
            this.DocumentoID = new UDT_DocumentoID();
            this.FechaINI = new UDTSQL_smalldatetime();
            this.FechaFIN = new UDTSQL_smalldatetime();
            this.RespTipo = new UDTSQL_tinyint();
            this.RespTercero = new UDT_TerceroID();
            this.EtapaIncumplimiento = new UDTSQL_char(10);
        }

	    #endregion
        
        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }
        public UDT_Consecutivo NumDocIncumplido { get; set; }
        public UDT_EmpresaID EmpresaID { get; set; }
        public UDT_DocumentoID DocumentoID { get; set; }
        public UDTSQL_smalldatetime FechaINI { get; set; }
        public UDTSQL_smalldatetime FechaFIN { get; set; }
        public UDTSQL_tinyint RespTipo { get; set; }
        public UDT_TerceroID RespTercero { get; set; }
        public UDTSQL_char EtapaIncumplimiento { get; set; }

        

        #endregion

    }
}
