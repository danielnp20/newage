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
    /// Models DTO_faCliente
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_faCliente : DTO_MasterBasic
    {
        #region DTO_faCliente
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_faCliente(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.TerceroDesc.Value = dr["TerceroDesc"].ToString();
                    this.ZonaDesc.Value = dr["ZonaDesc"].ToString();
                    this.ListaPrecioDesc.Value = dr["ListaPrecioDesc"].ToString();
                    this.FacturaTipoDesc.Value = dr["FacturaTipoDesc"].ToString();
                }

                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.ZonaID.Value = dr["ZonaID"].ToString();
                this.ListaPrecioID.Value = dr["ListaPrecioID"].ToString();
                this.FacturaTipoID.Value = dr["FacturaTipoID"].ToString();
                this.Responsable.Value = dr["Responsable"].ToString();
                this.CargoResp.Value = dr["CargoResp"].ToString();
                this.CorreoResp.Value = dr["CorreoResp"].ToString();
                this.CelularResp.Value = dr["CelularResp"].ToString();
                this.TelefonoResp.Value = dr["TelefonoResp"].ToString();
                this.DireccionCom.Value = dr["DireccionCom"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["DiasVto"].ToString()))
                    this.DiasVto.Value = Convert.ToByte(dr["DiasVto"]);
                if (!string.IsNullOrWhiteSpace(dr["DiasPtoPago"].ToString()))
                    this.DiasPtoPago.Value = Convert.ToByte(dr["DiasPtoPago"]);
                if (!string.IsNullOrWhiteSpace(dr["PorcPtoPago"].ToString()))
                    this.PorcPtoPago.Value = Convert.ToDecimal(dr["PorcPtoPago"]);
                this.CondicionesCom.Value = dr["CondicionesCom"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["IndCupoCarter"].ToString()))
                    this.IndCupoCarter.Value = Convert.ToBoolean(dr["IndCupoCarter"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrCupoCartera"].ToString()))
                    this.VlrCupoCartera.Value = Convert.ToDecimal(dr["VlrCupoCartera"]);
                this.DatoAdd1.Value = dr["DatoAdd1"].ToString();
                this.DatoAdd2.Value = dr["DatoAdd2"].ToString();
                this.DatoAdd3.Value = dr["DatoAdd3"].ToString();
                this.DatoAdd4.Value = dr["DatoAdd4"].ToString();
                this.DatoAdd5.Value = dr["DatoAdd5"].ToString();
                this.DatoAdd6.Value = dr["DatoAdd6"].ToString();
                this.DatoAdd7.Value = dr["DatoAdd7"].ToString();
                this.DatoAdd8.Value = dr["DatoAdd8"].ToString();
                this.DatoAdd9.Value = dr["DatoAdd9"].ToString();
                this.DatoAdd10.Value = dr["DatoAdd10"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_faCliente()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TerceroID = new UDT_BasicID();
            this.TerceroDesc = new UDT_Descriptivo();
            this.ZonaID = new UDT_BasicID();
            this.ZonaDesc = new UDT_Descriptivo();
            this.ListaPrecioID = new UDT_BasicID();
            this.ListaPrecioDesc = new UDT_Descriptivo();
            this.FacturaTipoID = new UDT_BasicID();
            this.FacturaTipoDesc = new UDT_Descriptivo();
            this.Responsable = new UDTSQL_char(30);
            this.CargoResp = new UDTSQL_char(30);
            this.CorreoResp = new UDTSQL_char(50);
            this.CelularResp = new UDTSQL_char(15);
            this.TelefonoResp = new UDTSQL_char(30);
            this.DireccionCom = new UDT_DescripTBase();
            this.DiasVto = new UDTSQL_tinyint();
            this.DiasPtoPago = new UDTSQL_tinyint();
            this.PorcPtoPago = new UDT_PorcentajeID();
            this.CondicionesCom = new UDT_DescripTBase();
            this.IndCupoCarter = new UDT_SiNo();
            this.VlrCupoCartera = new UDT_Valor();
            this.DatoAdd1 = new UDTSQL_char(50);
            this.DatoAdd2 = new UDTSQL_char(50);
            this.DatoAdd3 = new UDTSQL_char(50);
            this.DatoAdd4 = new UDTSQL_char(50);
            this.DatoAdd5 = new UDTSQL_char(50);
            this.DatoAdd6 = new UDTSQL_char(50);
            this.DatoAdd7 = new UDTSQL_char(50);
            this.DatoAdd8 = new UDTSQL_char(50);
            this.DatoAdd9 = new UDTSQL_char(50);
            this.DatoAdd10 = new UDTSQL_char(50);
        }

        public DTO_faCliente(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_faCliente(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID TerceroID { get; set; }

        [DataMember]
        public UDT_Descriptivo TerceroDesc { get; set; }

        [DataMember]
        public UDT_BasicID ZonaID { get; set; }

        [DataMember]
        public UDT_Descriptivo ZonaDesc { get; set; }

        [DataMember]
        public UDT_BasicID ListaPrecioID { get; set; }

        [DataMember]
        public UDT_Descriptivo ListaPrecioDesc { get; set; }

        [DataMember]
        public UDT_BasicID FacturaTipoID { get; set; }

        [DataMember]
        public UDT_Descriptivo FacturaTipoDesc { get; set; }

        [DataMember]
        public UDTSQL_char Responsable { get; set; }

        [DataMember]
        public UDTSQL_char CargoResp { get; set; }

        [DataMember]
        public UDTSQL_char CorreoResp { get; set; }

        [DataMember]
        public UDTSQL_char CelularResp { get; set; }

        [DataMember]
        public UDTSQL_char TelefonoResp { get; set; }

        [DataMember]
        public UDT_DescripTBase DireccionCom { get; set; }

        [DataMember]
        public UDTSQL_tinyint DiasVto { get; set; }

        [DataMember]
        public UDTSQL_tinyint DiasPtoPago { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorcPtoPago { get; set; }

        [DataMember]
        public UDT_DescripTBase CondicionesCom { get; set; }

        [DataMember]
        public UDT_SiNo IndCupoCarter { get; set; }

        [DataMember]
        public UDT_Valor VlrCupoCartera { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd1 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd2 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd3 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd4 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd5 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd6 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd7 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd8 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd9 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd10 { get; set; }

    }
}

