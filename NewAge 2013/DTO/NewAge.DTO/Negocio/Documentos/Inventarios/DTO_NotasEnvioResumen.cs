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
    /// Models DTO_NotasEnvioResumen
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_NotasEnvioResumen
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_NotasEnvioResumen(IDataReader dr)
        {
            this.InitCols();
            this.Seleccionar.Value = false;
            this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
            this.DocumentoID.Value = Convert.ToInt32(dr["DocumentoID"]);
            this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
            this.PrefijoID.Value = Convert.ToString(dr["PrefijoID"]);
            this.DocumentoNro.Value = Convert.ToInt32(dr["DocumentoNro"]);
            this.BodegaOrigenID.Value = Convert.ToString(dr["BodegaOrigenID"]);
            if (!string.IsNullOrWhiteSpace(dr["DocumentoREL"].ToString()))
                this.DocumentoREL.Value = Convert.ToInt32(dr["DocumentoREL"]);
            if (!string.IsNullOrWhiteSpace(dr["ClienteID"].ToString()))
                this.ClienteID.Value = Convert.ToString(dr["ClienteID"]);
            //if (!string.IsNullOrWhiteSpace(dr["VtoFecha"].ToString()))
            //    this.VtoFecha.Value = Convert.ToDateTime(dr["VtoFecha"]);
            if (!string.IsNullOrWhiteSpace(dr["Observacion"].ToString()))
                this.Observacion.Value = Convert.ToString(dr["Observacion"]);
            this.NotaEnvioRel.Value = Convert.ToInt32(dr["NotaEnvioRel"]);
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_NotasEnvioResumen()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Seleccionar = new UDT_SiNo();
            this.NumeroDoc = new UDT_Consecutivo();
            this.DocumentoID = new UDT_DocumentoID();
            this.Fecha = new UDTSQL_smalldatetime();
            this.PrefijoID = new UDT_PrefijoID();
            this.DocumentoNro = new UDT_Consecutivo();
            this.BodegaOrigenID = new UDT_BodegaID();
            this.DocumentoREL = new UDT_Consecutivo();
            this.ClienteID = new UDT_ClienteID();
            //this.VtoFecha = new UDTSQL_smalldatetime();
            this.Observacion = new UDT_DescripTExt();
            this.NotaEnvioRel = new UDT_Consecutivo();
            this.TerceroID = new UDT_TerceroID();
            this.DocumentoTercero = new UDT_DescripTExt();
            this.ValorLocal = new UDT_Valor();
            this.ValorExt = new UDT_Valor();
        }

        #endregion

        [DataMember]
        public UDT_SiNo Seleccionar { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_DocumentoID DocumentoID { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Fecha { get; set; }

        [DataMember]
        public UDT_PrefijoID PrefijoID { get; set; }

        [DataMember]
        public UDT_Consecutivo DocumentoNro { get; set; }

        [DataMember]
        public UDT_BodegaID BodegaOrigenID { get; set; }

        [DataMember]
        public UDT_Consecutivo DocumentoREL { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        //[DataMember]
        //public UDTSQL_smalldatetime VtoFecha { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        public UDT_Consecutivo NotaEnvioRel { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTExt DocumentoTercero { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorLocal { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorExt { get; set; }
    }
}
