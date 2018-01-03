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
    /// DTO Tabla DTO_inCostosExistencias
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inCostosExistencias
    {
        #region Constructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_inCostosExistencias(IDataReader dr)
        {
            InitCols();
            try
            {
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
                this.EmpresaID.Value = (dr["EmpresaID"]).ToString();
                this.Periodo.Value = Convert.ToDateTime(dr["Periodo"]);
                this.CosteoGrupoInvID.Value = (dr["CosteoGrupoInvID"]).ToString(); 
                this.inReferenciaID.Value = (dr["inReferenciaID"]).ToString();
                if (!string.IsNullOrWhiteSpace(dr["ActivoID"].ToString()))
                    this.ActivoID.Value = Convert.ToInt32(dr["ActivoID"]);
                this.EstadoInv.Value = Convert.ToByte(dr["EstadoInv"]);
                if (!string.IsNullOrWhiteSpace(dr["IdentificadorTr"].ToString()))
                    this.IdentificadorTr.Value =  Convert.ToInt32(dr["IdentificadorTr"]);
                this.CantInicial.Value = Convert.ToDecimal(dr["CantInicial"]); 
                this.CantEntrada.Value = Convert.ToDecimal(dr["CantEntrada"]); 
                this.CantRetiro.Value = Convert.ToDecimal(dr["CantRetiro"]);
                this.FobLocSaldoIni.Value = Convert.ToDecimal(dr["FobLocSaldoIni"]);
                this.CtoLocSaldoIni.Value = Convert.ToDecimal(dr["CtoLocSaldoIni"]);
                this.FobLocEntrada.Value = Convert.ToDecimal(dr["FobLocEntrada"]);
                this.CtoLocEntrada.Value = Convert.ToDecimal(dr["CtoLocEntrada"]);
                this.FobLocSalida.Value = Convert.ToDecimal(dr["FobLocSalida"]);
                this.CtoLocSalida.Value = Convert.ToDecimal(dr["CtoLocSalida"]);
                this.FobExtSaldoIni.Value = Convert.ToDecimal(dr["FobExtSaldoIni"]);
                this.CtoExtSaldoIni.Value = Convert.ToDecimal(dr["CtoExtSaldoIni"]);
                this.FobExtEntrada.Value = Convert.ToDecimal(dr["FobExtEntrada"]);
                this.CtoExtEntrada.Value = Convert.ToDecimal(dr["CtoExtEntrada"]);
                this.FobExtSalida.Value = Convert.ToDecimal(dr["FobExtSalida"]);
                this.FobLocSaldoIniIFRS.Value = Convert.ToDecimal(dr["FobLocSaldoIniIFRS"]);
                this.CtoLocSaldoIniIFRS.Value = Convert.ToDecimal(dr["CtoLocSaldoIniIFRS"]);
                this.FobLocEntradaIFRS.Value = Convert.ToDecimal(dr["FobLocEntradaIFRS"]);
                this.CtoLocEntradaIFRS.Value = Convert.ToDecimal(dr["CtoLocEntradaIFRS"]);
                this.FobLocSalidaIFRS.Value = Convert.ToDecimal(dr["FobLocSalidaIFRS"]);
                this.CtoLocSalidaIFRS.Value = Convert.ToDecimal(dr["CtoLocSalidaIFRS"]);
                this.FobExtSaldoIniIFRS.Value = Convert.ToDecimal(dr["FobExtSaldoIniIFRS"]);
                this.CtoExtSaldoIniIFRS.Value = Convert.ToDecimal(dr["CtoExtSaldoIniIFRS"]);
                this.FobExtEntradaIFRS.Value = Convert.ToDecimal(dr["FobExtEntradaIFRS"]);
                this.CtoExtEntradaIFRS.Value = Convert.ToDecimal(dr["CtoExtEntradaIFRS"]);
                this.FobExtSalidaIFRS.Value = Convert.ToDecimal(dr["FobExtSalidaIFRS"]);
                this.CtoExtSalidaIFRS.Value = Convert.ToDecimal(dr["CtoExtSalidaIFRS"]);
                this.CtoExtSalida.Value = Convert.ToDecimal(dr["CtoExtSalida"]);
                this.AxISaldoIni.Value = Convert.ToDecimal(dr["AxISaldoIni"]);
                this.AxIEntrada.Value = Convert.ToDecimal(dr["AxIEntrada"]);
                this.AxISalida.Value = Convert.ToDecimal(dr["AxISalida"]);
            }
            catch (Exception e)
            { 
                throw; 
            }
        }

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_inCostosExistencias(bool ResetValues)
        {
            InitCols();
            try
            {
                this.CantInicial.Value = 0;
                this.CantEntrada.Value = 0;
                this.CantRetiro.Value = 0;
                this.FobLocSaldoIni.Value = 0;
                this.CtoLocSaldoIni.Value = 0;
                this.FobLocEntrada.Value = 0;
                this.CtoLocEntrada.Value = 0;
                this.FobLocSalida.Value = 0;
                this.CtoLocSalida.Value = 0;
                this.FobExtSaldoIni.Value = 0;
                this.CtoExtSaldoIni.Value = 0;
                this.FobExtEntrada.Value = 0;
                this.CtoExtEntrada.Value = 0;
                this.FobExtSalida.Value = 0;
                this.FobLocSaldoIniIFRS.Value = 0;
                this.CtoLocSaldoIniIFRS.Value = 0;
                this.FobLocEntradaIFRS.Value = 0;
                this.CtoLocEntradaIFRS.Value = 0;
                this.FobLocSalidaIFRS.Value = 0;
                this.CtoLocSalidaIFRS.Value = 0;
                this.FobExtSaldoIniIFRS.Value = 0;
                this.CtoExtSaldoIniIFRS.Value = 0;
                this.FobExtEntradaIFRS.Value = 0;
                this.CtoExtEntradaIFRS.Value = 0;
                this.FobExtSalidaIFRS.Value = 0;
                this.CtoExtSalidaIFRS.Value = 0;
                this.CtoExtSalida.Value = 0;
                this.AxISaldoIni.Value = 0;
                this.AxIEntrada.Value = 0;
                this.AxISalida.Value = 0;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_inCostosExistencias()
        {
            InitCols();
            this.CantInicial.Value = 0;
            this.CantEntrada.Value = 0;
            this.CantRetiro.Value = 0;
            this.FobLocSaldoIni.Value = 0;
            this.CtoLocSaldoIni.Value = 0;
            this.FobLocEntrada.Value = 0;
            this.CtoLocEntrada.Value = 0;
            this.FobLocSalida.Value = 0;
            this.CtoLocSalida.Value = 0;
            this.FobExtSaldoIni.Value = 0;
            this.CtoExtSaldoIni.Value = 0;
            this.FobExtEntrada.Value = 0;
            this.CtoExtEntrada.Value = 0;
            this.FobExtSalida.Value = 0;
            this.FobLocSaldoIniIFRS.Value = 0;
            this.CtoLocSaldoIniIFRS.Value = 0;
            this.FobLocEntradaIFRS.Value = 0;
            this.CtoLocEntradaIFRS.Value = 0;
            this.FobLocSalidaIFRS.Value = 0;
            this.CtoLocSalidaIFRS.Value = 0;
            this.FobExtSaldoIniIFRS.Value = 0;
            this.CtoExtSaldoIniIFRS.Value = 0;
            this.FobExtEntradaIFRS.Value = 0;
            this.CtoExtEntradaIFRS.Value = 0;
            this.FobExtSalidaIFRS.Value = 0;
            this.CtoExtSalidaIFRS.Value = 0;
            this.CtoExtSalida.Value = 0;
            this.AxISaldoIni.Value = 0;
            this.AxIEntrada.Value = 0;
            this.AxISalida.Value = 0;
        }

        /// Inicializa las columnas
        /// </summary>
        protected virtual void InitCols()
        {
            this.Consecutivo = new UDT_Consecutivo();
            this.EmpresaID = new UDT_EmpresaID();
            this.Periodo = new  UDTSQL_smalldatetime();
            this.CosteoGrupoInvID = new  UDT_CosteoGrupoInvID();
            this.inReferenciaID = new UDT_inReferenciaID();
            this.ActivoID = new UDT_ActivoID();
            this.EstadoInv = new UDTSQL_tinyint();
            this.IdentificadorTr = new UDT_Consecutivo();
            this.CantInicial = new UDT_Cantidad();
            this.CantEntrada = new UDT_Cantidad();
            this.CantRetiro = new UDT_Cantidad();
            this.FobLocSaldoIni = new UDT_Valor();
            this.CtoLocSaldoIni = new UDT_Valor();
            this.FobLocEntrada = new UDT_Valor();
            this.CtoLocEntrada = new UDT_Valor();
            this.FobLocSalida = new UDT_Valor();
            this.CtoLocSalida = new UDT_Valor();
            this.FobExtSaldoIni = new UDT_Valor();
            this.CtoExtSaldoIni = new UDT_Valor();
            this.FobExtEntrada = new UDT_Valor();
            this.CtoExtEntrada = new UDT_Valor();
            this.FobExtSalida = new UDT_Valor();
            this.FobLocSaldoIniIFRS = new UDT_Valor();
            this.CtoLocSaldoIniIFRS = new UDT_Valor();
            this.FobLocEntradaIFRS = new UDT_Valor();
            this.CtoLocEntradaIFRS = new UDT_Valor();
            this.FobLocSalidaIFRS = new UDT_Valor();
            this.CtoLocSalidaIFRS = new UDT_Valor();
            this.FobExtSaldoIniIFRS = new UDT_Valor();
            this.CtoExtSaldoIniIFRS = new UDT_Valor();
            this.FobExtEntradaIFRS = new UDT_Valor();
            this.CtoExtEntradaIFRS = new UDT_Valor();
            this.FobExtSalidaIFRS = new UDT_Valor();
            this.CtoExtSalidaIFRS = new UDT_Valor();
            this.CtoExtSalida = new UDT_Valor();
            this.AxISaldoIni = new UDT_Valor();
            this.AxIEntrada = new UDT_Valor();
            this.AxISalida = new UDT_Valor();
            this.CtoUnitarioLoc = new UDT_Valor();
            this.CtoUnitarioExt = new UDT_Valor();

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
        public UDT_CosteoGrupoInvID CosteoGrupoInvID { get; set; }

        [DataMember]
        public UDT_inReferenciaID inReferenciaID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_ActivoID ActivoID { get; set; }

        [DataMember]
        public UDTSQL_tinyint EstadoInv { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Consecutivo IdentificadorTr { get; set; }

        [DataMember]
        public UDT_Cantidad CantInicial { get; set; }

        [DataMember]
        public UDT_Cantidad CantEntrada { get; set; }

        [DataMember]
        public UDT_Cantidad CantRetiro { get; set; }

        [DataMember]
        public UDT_Valor FobLocSaldoIni { get; set; }

        [DataMember]
        public UDT_Valor CtoLocSaldoIni { get; set; }

        [DataMember]
        public UDT_Valor FobLocEntrada { get; set; }

       [DataMember]
        public UDT_Valor CtoLocEntrada { get; set; }

        [DataMember]
        public UDT_Valor FobLocSalida { get; set; }

        [DataMember]
        public UDT_Valor CtoLocSalida { get; set; }

        [DataMember]
        public UDT_Valor FobExtSaldoIni { get; set; }

        [DataMember]
        public UDT_Valor CtoExtSaldoIni { get; set; }

        [DataMember]
        public UDT_Valor FobExtEntrada { get; set; }

        [DataMember]
        public UDT_Valor CtoExtEntrada { get; set; }

        [DataMember]
        public UDT_Valor FobExtSalida { get; set; }
        
        [DataMember]
        public UDT_Valor FobLocSaldoIniIFRS { get; set; }
        
        [DataMember]
        public UDT_Valor CtoLocSaldoIniIFRS { get; set; }
        
        [DataMember]
        public UDT_Valor FobLocEntradaIFRS { get; set; }
        
        [DataMember]
        public UDT_Valor CtoLocEntradaIFRS { get; set; }
        
        [DataMember]
        public UDT_Valor FobLocSalidaIFRS { get; set; }
        
        [DataMember]
        public UDT_Valor CtoLocSalidaIFRS { get; set; }
        
        [DataMember]
        public UDT_Valor FobExtSaldoIniIFRS { get; set; }
        
        [DataMember]
        public UDT_Valor CtoExtSaldoIniIFRS { get; set; }
        
        [DataMember]
        public UDT_Valor FobExtEntradaIFRS { get; set; }
        
        [DataMember]
        public UDT_Valor CtoExtEntradaIFRS { get; set; }
        
        [DataMember]
        public UDT_Valor FobExtSalidaIFRS { get; set; }
        
        [DataMember]
        public UDT_Valor CtoExtSalidaIFRS { get; set; }

        [DataMember]
        public UDT_Valor CtoExtSalida { get; set; }

        [DataMember]
        public UDT_Valor AxISaldoIni { get; set; }

        [DataMember]
        public UDT_Valor AxIEntrada { get; set; }

        [DataMember]
        public UDT_Valor AxISalida { get; set; }

        //Adicionales
        [DataMember]
        [AllowNull]
        public UDT_Valor CtoUnitarioLoc { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor CtoUnitarioExt { get; set; }

        #endregion
    }
}
