using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_glGestionDocumentalBitacora
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glGestionDocumentalBitacora
    {
        #region Constructor

		/// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glGestionDocumentalBitacora(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.DocumentoTipoID.Value = dr["DocumentoTipoID"].ToString();
                this.DocumentoClaseID.Value = dr["DocumentoClaseID"].ToString();
                this.DocTipoMovimientoID.Value = dr["DocTipoMovimientoID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["NumeroDoc"].ToString()))
                    this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["Fecha"].ToString()))
                    this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.Nombre.Value = dr["Nombre"].ToString();
                this.DocumentoTercero.Value = dr["DocumentoTercero"].ToString();
                this.CodIdentificador.Value = dr["CodIdentificador"].ToString();
                this.Observacion.Value = dr["Observacion"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glGestionDocumentalBitacora()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.DocumentoTipoID = new UDT_CodigoGrl10();
            this.DocumentoClaseID = new UDT_CodigoGrl10();
            this.DocTipoMovimientoID = new UDT_CodigoGrl10();
            this.NumeroDoc = new UDT_Consecutivo();
            this.Fecha = new UDTSQL_smalldatetime();
            this.TerceroID = new UDT_TerceroID();
            this.Nombre = new UDTSQL_char(70);
            this.DocumentoTercero = new UDTSQL_char(30);   
            this.CodIdentificador = new UDTSQL_char(50);
            this.Observacion =new UDT_DescripTExt();
        }

	    #endregion
        
        #region Propiedades

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_CodigoGrl10 DocumentoTipoID { get; set; }

        [DataMember]
        public UDT_CodigoGrl10 DocumentoClaseID { get; set; }

        [DataMember]
        public UDT_CodigoGrl10 DocTipoMovimientoID { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Fecha { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDTSQL_char Nombre { get; set; }

        [DataMember]
        public UDTSQL_char DocumentoTercero { get; set; }

        [DataMember]
        public UDTSQL_char CodIdentificador { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        #endregion
    }
}
