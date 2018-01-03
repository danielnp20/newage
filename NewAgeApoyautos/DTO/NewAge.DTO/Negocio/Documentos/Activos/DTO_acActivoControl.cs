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
    /// Models DTO_acActivoControl
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_acActivoControl
    {
        #region Constructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_acActivoControl(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["ActivoID"].ToString()))
                    this.ActivoID.Value = Convert.ToInt32(dr["ActivoID"]);
                this.PlaquetaID.Value = dr["PlaquetaID"].ToString();
                this.SerialID.Value = dr["SerialID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["ActivoPadreID"].ToString()))
                    this.ActivoPadreID.Value = Convert.ToInt32(dr["ActivoPadreID"]);
                if (!string.IsNullOrWhiteSpace(dr["RecibidoID"].ToString()))
                    this.RecibidoID.Value = Convert.ToInt32(dr["RecibidoID"]);
                this.Tipo.Value = Convert.ToByte(dr["Tipo"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaImportacion"].ToString()))
                    this.FechaImportacion.Value = Convert.ToDateTime(dr["FechaImportacion"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaVencimiento"].ToString()))
                    this.FechaVencimiento.Value = Convert.ToDateTime(dr["FechaVencimiento"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaCapitalizacion"].ToString()))
                    this.FechaCapitalizacion.Value = Convert.ToDateTime(dr["FechaCapitalizacion"]);
                this.DocumentoID.Value = Convert.ToInt32(dr["DocumentoID"]);
                this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                this.Periodo.Value = Convert.ToDateTime(dr["Periodo"]);
                this.CodigoBSID.Value = dr["CodigoBSID"].ToString();
                this.inReferenciaID.Value = dr["inReferenciaID"].ToString();
                this.EstadoInv.Value = Convert.ToByte(dr["EstadoInv"]);
                this.EstadoAct.Value = Convert.ToByte(dr["EstadoAct"]);
                this.ActivoGrupoID.Value = dr["ActivoGrupoID"].ToString();
                this.ActivoClaseID.Value = dr["ActivoClaseID"].ToString();
                this.ActivoTipoID.Value = dr["ActivoTipoID"].ToString();
                this.Modelo.Value = dr["Modelo"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["VidaUtilLOC"].ToString()))
                    this.VidaUtilLOC.Value = Convert.ToInt32(dr["VidaUtilLOC"].ToString());
                this.VidaUtilUSG.Value = Convert.ToInt32(dr["VidaUtilUSG"]);
                if (!string.IsNullOrWhiteSpace(dr["VidaUtilIFRS"].ToString()))
                    this.VidaUtilIFRS.Value = Convert.ToInt32(dr["VidaUtilIFRS"].ToString());
                if (!string.IsNullOrWhiteSpace(dr["TipoDepreLOC"].ToString()))
                    this.TipoDepreLOC.Value = Convert.ToByte(dr["TipoDepreLOC"].ToString());
                if (!string.IsNullOrWhiteSpace(dr["TipoDepreIFRS"].ToString()))
                    this.TipoDepreIFRS.Value = Convert.ToByte(dr["TipoDepreIFRS"].ToString());
                if (!string.IsNullOrWhiteSpace(dr["Turnos"].ToString()))
                    this.Turnos.Value = Convert.ToByte(dr["Turnos"]);
                if (!string.IsNullOrWhiteSpace(dr["AjustexInflacionInd"].ToString()))
                    this.AjustexInflacionInd.Value = Convert.ToBoolean(dr["AjustexInflacionInd"]);
                if (!string.IsNullOrWhiteSpace(dr["NumeroDocCompra"].ToString()))
                    this.NumeroDocCompra.Value = Convert.ToInt32(dr["NumeroDocCompra"]);
                this.MonedaID.Value = dr["MonedaID"].ToString();
                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.DocumentoTercero.Value = dr["DocumentoTercero"].ToString();
                this.ProyectoID.Value = dr["ProyectoID"].ToString();
                this.CentroCostoID.Value = Convert.ToString(dr["CentroCostoID"]);
                this.LocFisicaID.Value = dr["LocFisicaID"].ToString();
                this.Responsable.Value = dr["Responsable"].ToString();
                this.Propietario.Value = dr["Propietario"].ToString();
                this.BodegaID.Value = dr["BodegaID"].ToString();
                this.Observacion.Value = dr["Observacion"].ToString();
                this.EstadoActID.Value = dr["EstadoActID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["DocumentoAnula"].ToString()))
                    this.DocumentoAnula.Value = Convert.ToInt32(dr["DocumentoAnula"]);
                this.DatoAdd1.Value = dr["DatoAdd1"].ToString();
                this.DatoAdd2.Value = dr["DatoAdd2"].ToString();
                this.DatoAdd3.Value = dr["DatoAdd3"].ToString();
                this.DatoAdd4.Value = dr["DatoAdd4"].ToString();
                this.DatoAdd5.Value = dr["DatoAdd5"].ToString();
                this.ActivoTipoID.Value = dr["ActivoTipoID"].ToString();
                if (!string.IsNullOrEmpty(dr["TipoDepreUSG"].ToString()))
                    this.TipoDepreUSG.Value = Convert.ToByte(dr["TipoDepreUSG"]);
                this.ValorSalvamentoLOC.Value = Convert.ToDecimal(dr["ValorSalvamentoLOC"]);
                this.ValorSalvamentoUSG.Value = Convert.ToDecimal(dr["ValorSalvamentoUSG"]);
                this.ValorSalvamentoIFRS.Value = Convert.ToDecimal(dr["ValorSalvamentoIFRS"]);
                this.ValorSalvamentoIFRSUS.Value = Convert.ToDecimal(dr["ValorSalvamentoIFRSUS"]);
                this.ValorRetiroIFRS.Value = Convert.ToDecimal(dr["ValorRetiroIFRS"]);
                this.NumeroDocUltMvto.Value = Convert.ToInt32(dr["NumeroDocUltMvto"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DTO_acActivoControl(IDataReader dr, bool isNullble)
        {
            InitCols();
            try
            {
                this.SerialID.Value = dr["SerialID"].ToString();
                this.inReferenciaID.Value = dr["InreferenciaID"].ToString();
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                this.ActivoClaseID.Value = dr["ActivoClaseID"].ToString();
                this.CodigoBSID.Value = dr["CodigoBSID"].ToString();
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.ConsecutivoDetaID.Value = Convert.ToInt16(dr["ConsecutivoDetaID"]);
                if (!string.IsNullOrEmpty(dr["ProyectoID"].ToString()))
                    this.ProyectoID.Value = dr["ProyectoID"].ToString();
                if (!string.IsNullOrEmpty(dr["CentroCostoID"].ToString()))
                    this.CentroCostoID.Value = Convert.ToString(dr["CentroCostoID"]);
                this.CostoLOC.Value = Convert.ToDecimal(dr["CostoLOC"].ToString());
                this.CostoEXT.Value = Convert.ToDecimal(dr["CostoEXT"].ToString());
                this.Turnos.Value = 1;
                this.ValorRetiroIFRS.Value = 0;
            }
            catch (Exception e)
            { ; }
        }
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_acActivoControl()
        {
            InitCols();
        }
        
        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.ActivoID = new UDT_Consecutivo();
            this.ActivoPadreID = new UDT_Consecutivo();
            this.RecibidoID = new UDT_Consecutivo();
            this.PlaquetaID = new UDT_PlaquetaID();
            this.SerialID = new UDTSQL_char(25);
            this.ConsecutivoDetaID = new UDT_Consecutivo();
            this.Tipo = new UDTSQL_tinyint();
            this.FechaImportacion = new UDTSQL_smalldatetime();
            this.FechaVencimiento = new UDTSQL_smalldatetime();
            this.FechaCapitalizacion = new UDTSQL_smalldatetime();
            this.DocumentoID = new UDT_DocumentoID();
            this.Fecha = new UDTSQL_smalldatetime();
            this.Periodo = new UDT_PeriodoID();
            this.CodigoBSID = new UDT_CodigoBSID();
            this.inReferenciaID = new UDT_inReferenciaID();
            this.EstadoInv = new UDTSQL_tinyint();
            this.EstadoAct = new UDTSQL_tinyint();
            this.ActivoGrupoID = new UDT_ActivoGrupoID();
            this.ActivoClaseID = new UDT_ActivoClaseID();
            this.ActivoTipoID = new UDT_ActivoTipoID();
            this.Modelo = new UDTSQL_char(20);
            this.VidaUtilLOC = new UDTSQL_int();
            this.VidaUtilIFRS = new UDTSQL_int();
            this.TipoDepreLOC = new UDTSQL_tinyint();
            this.TipoDepreIFRS = new UDTSQL_tinyint();
            this.Turnos = new UDTSQL_tinyint();
            this.AjustexInflacionInd = new UDT_SiNo();
            this.NumeroDocCompra = new UDT_Consecutivo();
            this.MonedaID = new UDT_MonedaID();
            this.TerceroID = new UDT_TerceroID();
            this.DocumentoTercero = new UDTSQL_char(20);
            this.ProyectoID = new UDT_ProyectoID();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.LocFisicaID = new UDT_LocFisicaID();
            this.Responsable = new UDT_TerceroTipoID();
            this.Propietario = new UDT_TerceroID();
            this.BodegaID = new UDT_BodegaID();
            this.Observacion = new UDT_DescripTExt();
            this.EstadoActID = new UDT_EstadoActID();
            //Extras
            this.Descriptivo = new UDT_DescripTExt();
            this.TipoAct = new UDTSQL_char(1);
            this.DocumentoAnula = new UDT_Consecutivo();
            this.DatoAdd1 = new UDTSQL_char(20);
            this.DatoAdd2 = new UDTSQL_char(20);
            this.DatoAdd3 = new UDTSQL_char(20);
            this.DatoAdd4 = new UDTSQL_char(20);
            this.DatoAdd5 = new UDTSQL_char(20);
            this.NumeroDoc = new UDT_Consecutivo();
            this.CostoLOC = new UDT_Valor();
            this.CostoEXT = new UDT_Valor();
            this.CostoIniLOC = new UDT_Valor();
            this.CostoIniEXT = new UDT_Valor();
            this.VidaUtilUSG = new UDTSQL_int();
            this.TipoDepreUSG = new UDTSQL_tinyint();
            this.ValorSalvamentoLOC = new UDT_Valor();
            this.ValorSalvamentoUSG = new UDT_Valor();
            this.ValorSalvamentoIFRS = new UDT_Valor();
            this.ValorSalvamentoIFRSUS = new UDT_Valor();
            this.ValorRetiroIFRS = new UDT_Valor();
            this.NumeroDocUltMvto = new UDT_Consecutivo();
            this.CostoDiferencia = new UDT_Valor();
            this.Marca = new UDT_SiNo();
        }
        #endregion

        #region Propiedades

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo ActivoID { get; set; }

        [DataMember]
        public UDT_PlaquetaID PlaquetaID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Consecutivo ActivoPadreID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Consecutivo RecibidoID { get; set; }

        [DataMember]
        public UDTSQL_tinyint Tipo { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_smalldatetime FechaImportacion { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_smalldatetime FechaVencimiento { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_smalldatetime FechaCapitalizacion { get; set; }

        [DataMember]
        public UDT_DocumentoID DocumentoID { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Fecha { get; set; }

        [DataMember]
        public UDT_PeriodoID Periodo { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_CodigoBSID CodigoBSID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_inReferenciaID inReferenciaID { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_tinyint EstadoInv { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_tinyint EstadoAct { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_ActivoGrupoID ActivoGrupoID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_ActivoClaseID ActivoClaseID { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char Modelo { get; set; }

        [DataMember]
        public UDTSQL_int VidaUtilLOC { get; set; }

        [DataMember]
        public UDTSQL_int VidaUtilIFRS { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_tinyint TipoDepreLOC { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoDepreIFRS { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_tinyint Turnos { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_SiNo AjustexInflacionInd { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Consecutivo NumeroDocCompra { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_MonedaID MonedaID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char DocumentoTercero { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_LocFisicaID LocFisicaID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_TerceroTipoID Responsable { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_EstadoActID EstadoActID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Consecutivo DocumentoAnula { get; set; }

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
        public UDT_ActivoTipoID ActivoTipoID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_TerceroID Propietario { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char SerialID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_BodegaID BodegaID { get; set; }

        //Campos extras

        [DataMember]
        [AllowNull]
        public UDT_DescripTExt Descriptivo { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char TipoAct { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Consecutivo ConsecutivoDetaID { get; set; }        

        [DataMember]
        public UDTSQL_int VidaUtilUSG { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_tinyint TipoDepreUSG { get; set; }

        [DataMember]
        public UDT_Valor ValorSalvamentoLOC { get; set; }

        [DataMember]
        public UDT_Valor ValorSalvamentoUSG { get; set; }

        [DataMember]
        public UDT_Valor ValorSalvamentoIFRS { get; set; }

        [DataMember]
        public UDT_Valor ValorSalvamentoIFRSUS { get; set; }

        [DataMember]
        public UDT_Valor ValorRetiroIFRS { get; set; }
        
        #region Campos de Extras  Valor

        [DataMember]
        public UDT_Valor CostoLOC { get; set; }

        [DataMember]
        public UDT_Valor CostoEXT { get; set; }

        [DataMember]
        public UDT_Valor CostoDiferencia { get; set; }

        [DataMember]
        public UDT_Valor CostoIniLOC { get; set; }

        [DataMember]
        public UDT_Valor CostoIniEXT { get; set; }
        
        [DataMember]
        public UDT_SiNo Marca { get; set; }

        #endregion

        [DataMember]
        public UDT_Consecutivo NumeroDocUltMvto { get; set; }

        #endregion

    }
}
