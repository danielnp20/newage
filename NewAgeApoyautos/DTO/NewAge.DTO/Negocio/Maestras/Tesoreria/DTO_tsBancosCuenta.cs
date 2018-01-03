using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_tsBancosCuenta
    /// </summary>  
    [DataContract]
    [Serializable]
    public class DTO_tsBancosCuenta : DTO_MasterBasic
    {
        #region DTO_tsBancosCuenta
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_tsBancosCuenta(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.BancoDesc.Value = dr["BancoDesc"].ToString();
                    this.coDocumentoDesc.Value = dr["coDocumentoDesc"].ToString();
                    this.ProyectoDesc.Value = dr["ProyectoDesc"].ToString();
                    this.TerceroBancoDesc.Value = dr["TerceroBancoDesc"].ToString();
                    this.CentroCostoDesc.Value = dr["CentroCostoDesc"].ToString();
                    this.CuentaOrdPostfechadoDesc.Value = dr["CuentaOrdPostfechadoDesc"].ToString();
                    this.CodigoMigraPDesc.Value = Convert.ToString(dr["CodigoMigraPDesc"]);
                    this.CodigoMigraRDesc.Value = Convert.ToString(dr["CodigoMigraRDesc"]);
                    this.CodigoMigraEDesc.Value = Convert.ToString(dr["CodigoMigraEDesc"]);
                    this.DocumentoDesc.Value = Convert.ToString(dr["DocumentoDesc"]);
                }

                this.BancoID.Value = dr["BancoID"].ToString();
                this.CuentaTipo.Value = Convert.ToByte(dr["CuentaTipo"]);
                this.TipoLiquComision.Value = Convert.ToByte(dr["TipoLiquComision"]);
                this.FormaPago.Value = Convert.ToByte(dr["FormaPago"]);
                this.coDocumentoID.Value = dr["coDocumentoID"].ToString();
                this.CuentaNumero.Value = dr["CuentaNumero"].ToString();
                this.CuentaElectronica.Value = dr["CuentaElectronica"].ToString();
                this.TerceroBanco.Value = dr["TerceroBanco"].ToString();
                this.ChequeInicial.Value = Convert.ToInt32(dr["ChequeInicial"]);
                this.ChequeFinal.Value = Convert.ToInt32(dr["ChequeFinal"]);
                this.ProyectoID.Value = dr["ProyectoID"].ToString();
                this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                if(!string.IsNullOrEmpty(dr["FactXCheque"].ToString()))
                    this.FactXCheque.Value = Convert.ToByte(dr["FactXCheque"]);
                this.CuentaOrdPostfechado.Value = dr["CuentaOrdPostfechado"].ToString();
                if (!string.IsNullOrEmpty(dr["CodMigraPagos"].ToString()))
                    this.CodMigraPagos.Value = Convert.ToString(dr["CodMigraPagos"]);
                if (!string.IsNullOrEmpty(dr["CodMigraRecaudos"].ToString()))
                    this.CodMigraRecaudos.Value = Convert.ToString(dr["CodMigraRecaudos"]);
                if (!string.IsNullOrEmpty(dr["CodMigraExtractos"].ToString()))
                    this.CodMigraExtractos.Value = Convert.ToString(dr["CodMigraExtractos"]);
                this.DocumentoID.Value = Convert.ToString(dr["DocumentoID"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_tsBancosCuenta()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.BancoID = new UDT_BasicID();
            this.BancoDesc = new UDT_Descriptivo();
            this.CuentaTipo = new UDTSQL_tinyint();
            this.TipoLiquComision = new UDTSQL_tinyint();
            this.FormaPago = new UDTSQL_tinyint();
            this.coDocumentoID = new UDT_BasicID();
            this.coDocumentoDesc = new UDT_Descriptivo();
            this.CuentaNumero = new UDTSQL_char(20);
            this.CuentaElectronica = new UDTSQL_char(20);
            this.TerceroBanco = new UDT_BasicID();
            this.TerceroBancoDesc = new UDT_Descriptivo();
            this.ChequeInicial = new UDTSQL_int();
            this.ChequeFinal = new UDTSQL_int();
            this.ProyectoID = new UDT_BasicID();
            this.ProyectoDesc = new UDT_Descriptivo();
            this.CentroCostoID = new UDT_BasicID();
            this.CentroCostoDesc = new UDT_Descriptivo();
            this.FactXCheque = new UDTSQL_tinyint();
            this.CuentaOrdPostfechado = new UDT_BasicID();
            this.CuentaOrdPostfechadoDesc = new UDT_Descriptivo();
            this.CodMigraPagos = new UDT_BasicID();
            this.CodigoMigraPDesc = new UDT_Descriptivo();
            this.CodMigraRecaudos = new UDT_BasicID();
            this.CodigoMigraRDesc = new UDT_Descriptivo();
            this.CodMigraExtractos = new UDT_BasicID();
            this.CodigoMigraEDesc = new UDT_Descriptivo();
            this.DocumentoID = new UDT_BasicID();
            this.DocumentoDesc = new UDT_Descriptivo();
        }

        public DTO_tsBancosCuenta(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_tsBancosCuenta(DTO_aplMaestraPropiedades masterProperties) 
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_BasicID BancoID { get; set; }

        [DataMember]
        public UDT_Descriptivo BancoDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint CuentaTipo { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoLiquComision { get; set; }

        [DataMember]
        public UDTSQL_tinyint FormaPago { get; set; }

        [DataMember]
        public UDT_BasicID coDocumentoID { get; set; }

        [DataMember]
        public UDT_Descriptivo coDocumentoDesc { get; set; }

        [DataMember]
        public UDTSQL_char CuentaNumero { get; set; }

        [DataMember]
        public UDTSQL_char CuentaElectronica { get; set; }

        [DataMember]
        public UDT_BasicID TerceroBanco { get; set; }

        [DataMember]
        public UDT_Descriptivo TerceroBancoDesc { get; set; }

        [DataMember]
        public UDTSQL_int ChequeInicial { get; set; }

        [DataMember]
        public UDTSQL_int ChequeFinal { get; set; }

        [DataMember]
        public UDT_BasicID ProyectoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ProyectoDesc { get; set; }

        [DataMember]
        public UDT_BasicID CentroCostoID { get; set; }

        [DataMember]
        public UDT_Descriptivo CentroCostoDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint FactXCheque { get; set; }

        [DataMember]
        public UDT_BasicID CuentaOrdPostfechado { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaOrdPostfechadoDesc { get; set; }
        
        [DataMember]
        public UDT_BasicID CodMigraPagos { get; set; }
        
        [DataMember]
        public UDT_Descriptivo CodigoMigraPDesc { get; set; }
        
        [DataMember]
        public UDT_BasicID CodMigraRecaudos { get; set; }
        
        [DataMember]
        public UDT_Descriptivo CodigoMigraRDesc { get; set; }
        
        [DataMember]
        public UDT_BasicID CodMigraExtractos { get; set; }
        
        [DataMember]
        public UDT_Descriptivo CodigoMigraEDesc { get; set; }

        [DataMember]
        public UDT_BasicID DocumentoID { get; set; }

        [DataMember]
        public UDT_Descriptivo DocumentoDesc { get; set; }

    }
}
