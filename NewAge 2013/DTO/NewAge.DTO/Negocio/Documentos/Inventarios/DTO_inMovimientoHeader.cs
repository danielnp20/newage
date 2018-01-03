using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.Attributes;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// DTO Tabla DTO_inMovimientoHeader
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inMovimientoDocu
    {
        #region Constructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_inMovimientoDocu(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = (dr["EmpresaID"]).ToString();
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.BodegaOrigenID.Value = dr["BodegaOrigenID"].ToString();
                this.BodegaDestinoID.Value = dr["BodegaDestinoID"].ToString();
                this.AsesorID.Value = dr["AsesorID"].ToString(); 
                this.MvtoTipoInvID.Value = dr["MvtoTipoInvID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["DocumentoREL"].ToString()))
                    this.DocumentoREL.Value = Convert.ToInt32(dr["DocumentoREL"]);
                this.ClienteID.Value = dr["ClienteID"].ToString();
                this.Observacion.Value = dr["Observacion"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["VtoFecha"].ToString()))
                    this.VtoFecha.Value = Convert.ToDateTime(dr["VtoFecha"]);
                if (!string.IsNullOrWhiteSpace(dr["NotaEnvioREL"].ToString()))
                    this.NotaEnvioREL.Value = Convert.ToInt32(dr["NotaEnvioREL"]);
                if (!string.IsNullOrWhiteSpace(dr["TipoTransporte"].ToString()))
                    this.TipoTransporte.Value = Convert.ToByte(dr["TipoTransporte"]);
                if (!string.IsNullOrWhiteSpace(dr["AgenteAduanaID"].ToString()))
                    this.AgenteAduanaID.Value = dr["AgenteAduanaID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["ModalidadImp"].ToString()))
                    this.ModalidadImp.Value = Convert.ToByte(dr["ModalidadImp"]);
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
                if (!string.IsNullOrWhiteSpace(dr["TipoTraslado"].ToString()))
                    this.TipoTraslado.Value = Convert.ToByte(dr["TipoTraslado"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_inMovimientoDocu()
        {
            InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.BodegaOrigenID = new UDT_BodegaID();
            this.BodegaDestinoID = new UDT_BodegaID();
            this.AsesorID = new UDT_AsesorID();
            this.MvtoTipoInvID = new UDT_MvtoTipoID();
            this.DocumentoREL = new UDT_Consecutivo();
            this.ClienteID = new UDT_ClienteID();
            this.VtoFecha = new UDTSQL_smalldatetime();
            this.Observacion = new UDT_DescripTExt();
            this.NotaEnvioREL = new UDT_Consecutivo();
            this.TipoTransporte = new  UDTSQL_tinyint();
            this.AgenteAduanaID = new UDT_ProveedorID();
            this.ModalidadImp = new UDTSQL_tinyint();
            this.DatoAdd1 = new UDTSQL_char(20);
            this.DatoAdd2 = new UDTSQL_char(20);
            this.DatoAdd3 = new UDTSQL_char(20);
            this.DatoAdd4 = new UDTSQL_char(20);
            this.DatoAdd5 = new UDTSQL_char(20);
            this.DatoAdd6 = new UDTSQL_char(20);
            this.DatoAdd7 = new UDTSQL_char(20);
            this.DatoAdd8 = new UDTSQL_char(20);
            this.DatoAdd9 = new UDTSQL_char(20);
            this.DatoAdd10 = new UDTSQL_char(20);
            this.TipoTraslado = new UDTSQL_tinyint();
        }

        #endregion

        #region Propiedades

        [DataMember]
        [NotImportable]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_BodegaID BodegaOrigenID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_BodegaID BodegaDestinoID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_AsesorID AsesorID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_MvtoTipoID MvtoTipoInvID { get; set; }
        
        [DataMember]
        [AllowNull]
        public UDT_Consecutivo DocumentoREL { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_smalldatetime VtoFecha { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Consecutivo NotaEnvioREL { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_tinyint TipoTransporte { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_ProveedorID AgenteAduanaID { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_tinyint ModalidadImp { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char DatoAdd1 { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char DatoAdd2 { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char DatoAdd3 { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char DatoAdd4 { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char DatoAdd5 { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char DatoAdd6 { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char DatoAdd7 { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char DatoAdd8 { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char DatoAdd9 { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char DatoAdd10 { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_tinyint TipoTraslado { get; set; }

        #endregion
    }
}
