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
    /// Models DTO_SolicitudGestion
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_SolicitudGestion
    {
        #region OperacionesPendientes

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_SolicitudGestion(IDataReader dr)
        {
            InitCols();
            try
            {
                this.Libranza.Value= Convert.ToInt32(dr["Libranza"]);
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.DocumentoID.Value = Convert.ToInt32(dr["DocumentoID"]);
                this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                this.ClienteID.Value = Convert.ToString(dr["ClienteID"]);
                this.Nombre.Value = Convert.ToString(dr["Nombre"]);
                this.Zona.Value = Convert.ToString(dr["Zona"]);
                this.Pagaduria.Value = Convert.ToString(dr["Pagaduria"]);
                this.LineaCredito.Value = Convert.ToString(dr["LineaCredito"]);
                this.TipoOperacion.Value = Convert.ToString(dr["TipoOperacion"]);
                this.Plazo.Value = Convert.ToByte(dr["Plazo"]);
                if (!string.IsNullOrEmpty(dr["VlrSolicitado"].ToString()))
                    this.VlrSolicitado.Value = Convert.ToDecimal(dr["VlrSolicitado"]);
                this.Etapa.Value = Convert.ToString(dr["Etapa"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_SolicitudGestion()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.Libranza= new UDT_Consecutivo();
            this.NumeroDoc = new UDT_Consecutivo();
            this.DocumentoID = new UDT_Consecutivo();
            this.Fecha = new UDTSQL_smalldatetime();
            this.ClienteID= new UDT_ClienteID();
            this.Nombre= new UDT_DescripTExt();
            this.Zona = new UDT_Descriptivo();
            this.Pagaduria=new UDT_Descriptivo();
            this.LineaCredito = new UDT_Descriptivo();
            this.TipoOperacion=new UDT_DescripTExt();
            this.Plazo = new UDTSQL_tinyint();
            this.VlrSolicitado = new UDT_Valor();
            this.Etapa = new UDT_Descriptivo();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo Libranza { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_Consecutivo DocumentoID { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Fecha { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID{ get; set; }
        
        [DataMember]
        public UDT_DescripTExt Nombre{ get; set; }      
        
        [DataMember]
        public UDT_Descriptivo Zona { get; set; }

        [DataMember]
        public UDT_Descriptivo Pagaduria { get; set; }

        [DataMember]
        public UDT_Descriptivo LineaCredito { get; set; }

        [DataMember]
        public UDT_DescripTExt TipoOperacion { get; set; }

        [DataMember]
        public UDTSQL_tinyint Plazo { get; set; }

        [DataMember]
        public UDT_Valor VlrSolicitado { get; set; }

        [DataMember]
        public UDT_Descriptivo Etapa { get; set; }

        [DataMember]
        public string ViewDoc { get; set; }
        #endregion
    }
}
