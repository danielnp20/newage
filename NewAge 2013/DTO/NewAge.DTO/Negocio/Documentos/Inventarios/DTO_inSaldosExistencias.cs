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
    /// DTO Tabla DTO_inSaldosExistencias
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inSaldosExistencias
    {
        #region Constructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_inSaldosExistencias(IDataReader dr)
        {
            InitCols();
            try
            {
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.Periodo.Value = Convert.ToDateTime(dr["Periodo"]);
                this.BodegaID.Value = dr["BodegaID"].ToString();
                this.inReferenciaID.Value = dr["inReferenciaID"].ToString();
                this.EstadoInv.Value = Convert.ToByte(dr["EstadoInv"]);
                this.Parametro1.Value = dr["Parametro1"].ToString();
                this.Parametro2.Value = dr["Parametro2"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["IdentificadorTr"].ToString()))
                    this.IdentificadorTr.Value = Convert.ToInt32(dr["IdentificadorTr"]);
                if (!string.IsNullOrWhiteSpace(dr["ActivoID"].ToString()))
                    this.ActivoID.Value = Convert.ToInt32(dr["ActivoID"]);
                if (!string.IsNullOrWhiteSpace(dr["CantInicial"].ToString()))
                    this.CantInicial.Value = Convert.ToDecimal(dr["CantInicial"]);
                if (!string.IsNullOrWhiteSpace(dr["CantEntrada"].ToString()))
                    this.CantEntrada.Value = Convert.ToDecimal(dr["CantEntrada"]);
                if (!string.IsNullOrWhiteSpace(dr["CantRetiro"].ToString()))
                    this.CantRetiro.Value = Convert.ToDecimal(dr["CantRetiro"]);
            }
            catch (Exception e)
            { 
                throw; 
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_inSaldosExistencias()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        protected virtual void InitCols()
        {
            this.Consecutivo = new UDT_Consecutivo();
            this.EmpresaID = new UDT_EmpresaID();
            this.Periodo = new  UDTSQL_smalldatetime();
            this.BodegaID = new UDT_BodegaID();
            this.inReferenciaID = new UDT_inReferenciaID();
            this.ActivoID = new UDT_ActivoID();
            this.EstadoInv = new UDTSQL_tinyint();
            this.Parametro1 = new UDT_ParametroInvID();
            this.Parametro2 = new UDT_ParametroInvID();
            this.IdentificadorTr = new UDT_Consecutivo();
            this.CantInicial = new UDT_Cantidad();
            this.CantEntrada = new UDT_Cantidad();
            this.CantRetiro = new UDT_Cantidad();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Periodo { get; set; }

        [DataMember]
        public UDT_BodegaID BodegaID { get; set; }

        [DataMember]
        public UDT_inReferenciaID inReferenciaID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_ActivoID ActivoID { get; set; }

        [DataMember]
        public UDTSQL_tinyint EstadoInv { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_ParametroInvID Parametro1 { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_ParametroInvID Parametro2 { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Consecutivo IdentificadorTr { get; set; }

        [DataMember]
        public UDT_Cantidad CantInicial { get; set; }

        [DataMember]
        public UDT_Cantidad CantEntrada { get; set; }

        [DataMember]
        public UDT_Cantidad CantRetiro { get; set; }

        #endregion
    }
}
