using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using NewAge.DTO.Negocio.Reportes;
using NewAge.DTO.Reportes;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_inControlSaldosCostos
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inControlSaldosCostos : DTO_BasicReport
    {
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_inControlSaldosCostos(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = (dr["EmpresaID"]).ToString();
                this.BodegaID.Value = (dr["BodegaID"]).ToString();
                this.inReferenciaID.Value = (dr["inReferenciaID"]).ToString();
                this.ActivoID.Value = Convert.ToInt32((dr["ActivoID"]));
                this.EstadoInv.Value = Convert.ToByte(dr["EstadoInv"]);
                this.Parametro1.Value = (dr["Parametro1"]).ToString();
                this.Parametro2.Value = (dr["Parametro2"]).ToString();
                this.Periodo.Value = Convert.ToDateTime(dr["Periodo"]);
                this.CosteoGrupoInvID.Value = (dr["CosteoGrupoInvID"]).ToString();
                this.RegistroSaldo.Value = Convert.ToInt32(dr["RegistroSaldo"]);
                this.RegistroCosto.Value = Convert.ToInt32(dr["RegistroCosto"]);
                this.IdentificadorTr.Value = Convert.ToInt32(dr["IdentificadorTr"]);
                this.BodegaTipo.Value = Convert.ToByte(dr["BodegaTipo"]);
                this.CosteoTipo.Value = Convert.ToByte(dr["CosteoTipo"]);
                this.UbicacionID = (dr["UbicacionID"]).ToString().TrimEnd();
                this.Seleccion.Value = false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
                /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_inControlSaldosCostos()
        {
            InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.BodegaID = new UDT_BodegaID();
            this.inReferenciaID = new UDT_inReferenciaID();
            this.ActivoID = new UDT_ActivoID();
            this.EstadoInv = new UDTSQL_tinyint();
            this.Parametro1 = new UDT_ParametroInvID();
            this.Parametro2 = new UDT_ParametroInvID();
            this.IdentificadorTr = new UDT_Consecutivo();
            this.Periodo = new UDTSQL_smalldatetime();
            this.CosteoGrupoInvID = new UDT_CosteoGrupoInvID();
            this.RegistroSaldo = new UDT_Consecutivo();
            this.RegistroCosto = new UDT_Consecutivo();
            //Items adicionales
            this.CantidadDisp = new UDT_Cantidad();
            this.ValorLocalDisp = new UDT_Valor();
            this.ValorExtranjeroDisp = new UDT_Valor();
            this.ValorFobLocalDisp = new UDT_Valor();
            this.ValorFobExtDisp = new UDT_Valor();
            this.BodegaDesc = new UDT_DescripTExt();
            this.ReferenciaP1P2ID = new UDT_DescripTExt();
            this.ReferenciaP1P2Desc = new UDT_DescripTExt();
            this.Parametro1Desc = new UDT_DescripTExt();
            this.Parametro2Desc = new UDT_DescripTExt();
            this.SerialID = new UDT_DescripTExt();
            this.Seleccion = new UDT_SiNo();
            this.UnidadInvID = new UDT_UnidadInvID();
            this.BodegaTipo = new UDTSQL_tinyint();
            this.CosteoTipo = new UDTSQL_tinyint();
            this.BodegaActivaInd = new UDT_SiNo();
            this.DetalleMvto = new List<DTO_glMovimientoDeta>();
            this.CostosExistencia = new DTO_inCostosExistencias();
            this.SaldosExistencias = new DTO_inSaldosExistencias();
        }

        #region Propiedades

        [DataMember]
        [NotImportable]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_BodegaID BodegaID { get; set; }

        [DataMember]
        public UDT_inReferenciaID inReferenciaID { get; set; }

        [DataMember]
        public UDT_ActivoID ActivoID { get; set; }

        [DataMember]
        public UDTSQL_tinyint EstadoInv { get; set; }

        [DataMember]
        public UDT_ParametroInvID Parametro1 { get; set; }

        [DataMember]
        public UDT_ParametroInvID Parametro2 { get; set; }

        [DataMember]
        public UDT_Consecutivo IdentificadorTr { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime Periodo { get; set; }

        [DataMember]
        public UDT_CosteoGrupoInvID CosteoGrupoInvID { get; set; }

        [DataMember]
        public UDT_Consecutivo RegistroSaldo { get; set; }

        [DataMember]
        public UDT_Consecutivo RegistroCosto { get; set; }

        //Adicionales
        [DataMember]
        [AllowNull]
        public UDT_Cantidad CantidadDisp { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorLocalDisp { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorExtranjeroDisp { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorFobLocalDisp { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorFobExtDisp { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTExt BodegaDesc { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTExt ReferenciaP1P2ID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTExt ReferenciaP1P2Desc { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTExt Parametro1Desc { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTExt Parametro2Desc { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTExt SerialID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_SiNo Seleccion { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_UnidadInvID UnidadInvID { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_tinyint BodegaTipo   { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_tinyint CosteoTipo { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_SiNo BodegaActivaInd { get; set; }

        [DataMember]
        [AllowNull]
        public string UbicacionID { get; set; }

        [DataMember]
        public List<DTO_glMovimientoDeta> DetalleMvto { get; set; }

        [DataMember]
        public DTO_inCostosExistencias CostosExistencia { get; set; }

        [DataMember]
        public DTO_inSaldosExistencias SaldosExistencias { get; set; }

        #endregion
    }

}
