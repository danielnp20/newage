using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class comprobante para aprobacion:
    /// Models DTO_CorreoCliente.
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_CorreoCliente : DTO_SerializedObject
    {
        #region DTO_CorreoCliente

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_CorreoCliente(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.Libranza.Value = Convert.ToInt32(dr["Libranza"]);
                this.ClienteID.Value = dr["ClienteID"].ToString();
                this.Nombre.Value = dr["Nombre"].ToString();
                this.Correo.Value = dr["Correo"].ToString();
                this.Conyuge.Value = dr["Conyuge"].ToString();           
                this.Codeudor1.Value = dr["Codeudor1"].ToString();
                this.Codeudor2.Value = dr["Codeudor2"].ToString();
                this.Codeudor3.Value = dr["Codeudor3"].ToString();
                this.Codeudor4.Value = dr["Codeudor4"].ToString();
                this.Codeudor5.Value = dr["Codeudor5"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["EstadoDeuda"].ToString()))
                    this.EstadoDeuda.Value = Convert.ToByte(dr["EstadoDeuda"]);
                if (!string.IsNullOrWhiteSpace(dr["SdoCapital"].ToString()))
                    this.SdoCapital.Value = Convert.ToDecimal(dr["SdoCapital"]);
                if (!string.IsNullOrWhiteSpace(dr["SdoSeguro"].ToString()))
                    this.SdoSeguro.Value = Convert.ToDecimal(dr["SdoSeguro"]);
                if (!string.IsNullOrWhiteSpace(dr["ClienteInd"].ToString()))
                    this.ClienteInd.Value = Convert.ToBoolean(dr["ClienteInd"]);
                this.ConyugeInd.Value = false;
                this.CodeudorInd.Value = false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_CorreoCliente()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            #region Campos Propios
            this.NumeroDoc = new UDT_Consecutivo();
            this.Libranza = new UDT_LibranzaID();
            this.ClienteID = new UDT_ClienteID();
            this.Nombre = new UDTSQL_char(100);
            this.Correo = new UDTSQL_char(100);
            this.Conyuge = new UDT_ClienteID();          
            this.Codeudor1 = new UDT_TerceroID();
            this.Codeudor2 = new UDT_TerceroID();
            this.Codeudor3 = new UDT_TerceroID();
            this.Codeudor4 = new UDT_TerceroID();
            this.Codeudor5 = new UDT_TerceroID();  
            this.EstadoDeuda = new UDTSQL_tinyint();
            this.Aprobado = new UDT_SiNo();
            this.SdoCapital = new UDT_Valor();
            this.SdoSeguro = new UDT_Valor();
            this.ClienteInd = new UDT_SiNo();
            this.ConyugeInd = new UDT_SiNo();
            this.CodeudorInd = new UDT_SiNo();
            #endregion            
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDTSQL_char Nombre { get; set; }

        [DataMember]
        public UDTSQL_char Correo { get; set; }

        [DataMember]
        public UDT_LibranzaID Libranza { get; set; }

        [DataMember]
        public UDT_ClienteID Conyuge { get; set; }

        [DataMember]
        public UDT_TerceroID Codeudor1 { get; set; }

        [DataMember]
        public UDT_TerceroID Codeudor2 { get; set; }

        [DataMember]
        public UDT_TerceroID Codeudor3 { get; set; }

        [DataMember]
        public UDT_TerceroID Codeudor4 { get; set; }

        [DataMember]
        public UDT_TerceroID Codeudor5 { get; set; }

        [DataMember]
        public UDTSQL_tinyint EstadoDeuda { get; set; }

        [DataMember]
        public UDT_SiNo Aprobado { get; set; }  

        [DataMember]
        public UDT_Valor SdoCapital { get; set; }

        [DataMember]
        public UDT_Valor SdoSeguro { get; set; }

        [DataMember]
        public UDT_SiNo ClienteInd { get; set; }

        [DataMember]
        public UDT_SiNo ConyugeInd { get; set; }

        [DataMember]
        public UDT_SiNo CodeudorInd { get; set; }

        #endregion

    }
}
