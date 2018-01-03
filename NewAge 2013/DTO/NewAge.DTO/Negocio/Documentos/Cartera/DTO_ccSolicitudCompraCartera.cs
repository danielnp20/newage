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
    /// 
    /// Models DTO_ccSolicitudCompraCartera
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccSolicitudCompraCartera
    {
        #region DTO_ccSolicitudCompraCartera

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccSolicitudCompraCartera(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.FinancieraID.Value = dr["FinancieraID"].ToString();
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                this.Documento.Value = Convert.ToInt32(dr["Documento"]);
                if (!string.IsNullOrWhiteSpace(dr["DocCompra"].ToString())) 
                    this.DocCompra.Value = Convert.ToInt32(dr["DocCompra"].ToString());
                this.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"].ToString());
                this.VlrSaldo.Value = Convert.ToDecimal(dr["VlrSaldo"].ToString());
                if (!string.IsNullOrWhiteSpace(dr["DocAnticipo"].ToString())) 
                    this.DocAnticipo.Value = Convert.ToInt32(dr["DocAnticipo"]);
                if (!string.IsNullOrWhiteSpace(dr["IndRecibePazySalvo"].ToString())) 
                    this.IndRecibePazySalvo.Value = Convert.ToBoolean(dr["IndRecibePazySalvo"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaPazySalvo"].ToString()))
                    this.FechaPazySalvo.Value = Convert.ToDateTime(dr["FechaPazySalvo"]);
                if (!string.IsNullOrWhiteSpace(dr["UsuarioID"].ToString()))
                    this.UsuarioID.Value = dr["UsuarioID"].ToString();
                this.ExternaInd.Value = Convert.ToBoolean(dr["ExternaInd"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccSolicitudCompraCartera()
        {
            InitCols();
        }

        public void InitCols()
        {
            this.NumeroDoc    = new UDT_Consecutivo();
            this.FinancieraID = new UDT_BasicID();
            this.Documento  = new UDT_LibranzaID();
            this.DocCompra  = new UDT_Consecutivo();
            this.VlrCuota = new UDT_Valor();
            this.VlrSaldo = new UDT_Valor();
            this.Consecutivo = new UDT_Consecutivo();
            this.DocAnticipo = new UDT_Consecutivo();
            this.IndRecibePazySalvo = new UDT_SiNo();
            this.FechaPazySalvo = new UDTSQL_smalldatetime();
            this.UsuarioID = new UDT_UsuarioID();
            this.ExternaInd = new UDT_SiNo();

            //Extras
            this.AnticipoInd = new UDT_SiNo();
            this.ClienteID = new UDT_ClienteID();
            this.Descriptivo = new UDT_DescripTBase();
            this.FechaDoc = new UDTSQL_datetime();
            this.NuevoPyS = new UDT_SiNo();
            this.TipoEmpresa = new UDTSQL_tinyint();
            this.EC_Proposito = new UDTSQL_tinyint();
            this.EC_ValorAbono = new UDT_Valor();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_BasicID FinancieraID { get; set; }

        [DataMember]
        public UDT_LibranzaID Documento { get; set; }

        [DataMember]
        public UDT_Consecutivo DocCompra { get; set; }

        [DataMember]
        public UDT_Valor VlrCuota { get; set; }

        [DataMember]
        public UDT_Valor VlrSaldo { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        [DataMember]
        public UDT_Consecutivo DocAnticipo { get; set; }

        [DataMember]
        public UDT_SiNo IndRecibePazySalvo { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaPazySalvo { get; set;}

        [DataMember]
        public UDT_UsuarioID UsuarioID{ get; set; }

        [DataMember]
        public UDT_SiNo ExternaInd { get; set; }

        //Extra 

        [DataMember]
        public UDT_SiNo AnticipoInd { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDT_DescripTBase Descriptivo { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaDoc { get; set; }

        [DataMember]
        public UDT_SiNo NuevoPyS { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoEmpresa { get; set; }

        [DataMember]
        public UDTSQL_tinyint EC_Proposito { get; set; }

        [DataMember]
        public UDT_Valor EC_ValorAbono{ get; set; }

        #endregion
    }
}
