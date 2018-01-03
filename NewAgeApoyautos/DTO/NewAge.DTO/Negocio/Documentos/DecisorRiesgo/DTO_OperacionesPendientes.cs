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
    /// Models DTO_OperacionesPendientes
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_OperacionesPendientes
    {
        #region OperacionesPendientes

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_OperacionesPendientes(IDataReader dr)
        {
            InitCols();
            try
            {
                this.Libranza.Value = Convert.ToInt32(dr["Libranza"]);
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.DocumentoID.Value = Convert.ToInt32(dr["DocumentoId"]);
                this.FechaRadica.Value = Convert.ToDateTime(dr["FechaRadica"]);
                this.FechaInicio.Value = Convert.ToDateTime(dr["FechaInicio"]);

                this.ClienteID.Value = Convert.ToString(dr["ClienteID"]);
                this.Nombre.Value = Convert.ToString(dr["Nombre"]);
                this.Vitrina.Value = Convert.ToString(dr["Vitrina"]);
                this.Zona.Value = Convert.ToString(dr["Zona"]);
                this.LineaCredito.Value = Convert.ToString(dr["LineaCredito"]);
                this.TipoOperacion.Value = Convert.ToString(dr["TipoOperacion"]);
                this.Plazo.Value = Convert.ToByte(dr["Plazo"]);
                if (!string.IsNullOrEmpty(dr["VlrSolicitado"].ToString()))
                    this.VlrSolicitado.Value = Convert.ToDecimal(dr["VlrSolicitado"]);
                this.Etapa.Value = Convert.ToString(dr["Etapa"]);
                this.Estado.Value = Convert.ToString(dr["Estado"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_OperacionesPendientes()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.Libranza = new UDT_Consecutivo();
            this.NumeroDoc = new UDT_Consecutivo();
            this.DocumentoID = new UDT_DocumentoID();
            this.FechaRadica = new UDTSQL_smalldatetime();
            this.FechaInicio = new UDTSQL_smalldatetime();
            this.ClienteID= new UDT_ClienteID();
            this.Nombre= new UDT_Descriptivo();
            this.Vitrina=new UDT_Descriptivo();
            this.Zona=new UDT_Descriptivo();
            this.LineaCredito = new UDT_Descriptivo();
            this.TipoOperacion=new UDT_DescripTExt();
            this.Plazo = new UDTSQL_tinyint();
            this.VlrSolicitado = new UDT_Valor();
            this.Etapa = new UDT_Descriptivo();
            this.Estado = new UDT_Descriptivo();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo Libranza { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_DocumentoID DocumentoID { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaRadica { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaInicio { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID{ get; set; }
        
        [DataMember]
        public UDT_Descriptivo Nombre{ get; set; }
        
        [DataMember]
        public UDT_Descriptivo Vitrina{ get; set; }
        
        [DataMember]
        public UDT_Descriptivo Zona { get; set; }

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
        public UDT_Descriptivo Estado { get; set; }

        [DataMember]
        public string ViewDoc { get; set; }

        #endregion
    }
}
