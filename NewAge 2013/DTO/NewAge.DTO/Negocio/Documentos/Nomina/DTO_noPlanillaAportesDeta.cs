using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NewAge.DTO.UDT;
using System.Runtime.Serialization;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_noPlanillaAportesDeta
    {
        #region Contructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noPlanillaAportesDeta(IDataReader dr)
        {
            this.InitCols();
            try
            {   
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.TipoTrplanilla.Value = Convert.ToByte(dr["TipoTrplanilla"]);
                this.EmpleadoID.Value = dr["EmpleadoID"].ToString();
                this.TipoCotizante.Value = Convert.ToByte(dr["TipoCotizante"]);
                this.TipoDocNomina.Value = Convert.ToByte(dr["TipoDocNomina"]); 
                this.SubTipo.Value = Convert.ToByte(dr["SubTipo"]);
                this.ExtranjeroInd.Value = Convert.ToBoolean(dr["ExtranjeroInd"]);
                this.ExteriorInd.Value = Convert.ToBoolean(dr["ExteriorInd"]);
                this.Sueldo.Value = Convert.ToDecimal(dr["Sueldo"]);
                this.LugarGeograficoID.Value = dr["Ciudad"].ToString();
                this.SalIntegralInd.Value = Convert.ToBoolean(dr["SalIntegralInd"]);
                this.INGInd.Value = Convert.ToBoolean(dr["INGrInd"]);
                this.RETInd.Value = Convert.ToBoolean(dr["RETInd"]);
                this.TDEInd.Value = Convert.ToBoolean(dr["TDEInd"]);
                this.TAEInd.Value = Convert.ToBoolean(dr["TAEInd"]);
                this.TDPInd.Value = Convert.ToBoolean(dr["TDPInd"]);
                this.TAPInd.Value = Convert.ToBoolean(dr["TAPInd"]);
                this.VSPInd.Value = Convert.ToBoolean(dr["VSPInd"]);
                this.VTEInd.Value = Convert.ToBoolean(dr["VTEInd"]);
                this.VSTInd.Value = Convert.ToBoolean(dr["VSTInd"]);
                this.SLNInd.Value = Convert.ToBoolean(dr["SLNInd"]);
                this.IGEInd.Value = Convert.ToBoolean(dr["IGEInd"]);
                this.LMAInd.Value = Convert.ToBoolean(dr["LMAInd"]);
                this.VACInd.Value = Convert.ToBoolean(dr["VACInd"]);
                this.AVPInd.Value = Convert.ToBoolean(dr["AVPInd"]);
                this.VCTInd.Value = Convert.ToBoolean(dr["VCTInd"]);
                this.IRPInd.Value = Convert.ToBoolean(dr["IRPInd"]);
                this.FondoPension.Value = dr["FondoPension"].ToString();
                this.FondoPensionTR.Value = dr["FondoPensionTR"].ToString();
                this.DiasCotizadosPEN.Value = Convert.ToByte(dr["DiasCotizadosPEN"]);
                this.IngresoBasePEN.Value = Convert.ToDecimal(dr["IngresoBasePEN"]);
                this.TarifaPEN.Value = Convert.ToDecimal(dr["TarifaPEN"]);
                this.VlrEmpresaPEN.Value = Convert.ToDecimal(dr["VlrEmpresaPEN"]);
                this.VlrNominaPEN.Value = Convert.ToDecimal(dr["VlrNominaPEN"]);
                this.VlrEmpresaVOL.Value = Convert.ToDecimal(dr["VlrEmpresaVOL"]);
                this.VlrNoRetenido.Value = Convert.ToDecimal(dr["VlrNoRetenido"]);
                this.VlrSolidaridad.Value = Convert.ToDecimal(dr["VlrSolidaridad"]);
                this.VlrSubsistencia.Value = Convert.ToDecimal(dr["VlrSubsistencia"]);
                this.VlrNominaSOL.Value = Convert.ToDecimal(dr["VlrNominaSOL"]);
                this.VlrTrabajadorPEN.Value = Convert.ToDecimal(dr["VlrTrabajadorPEN"]);
                this.FondoSalud.Value = dr["FondoSalud"].ToString();
                this.FondoSaludTR.Value = dr["FondoSaludTR"].ToString();
                this.DiasCotizadosSLD.Value = Convert.ToByte(dr["DiasCotizadosSLD"]);
                this.IngresoBaseSLD.Value = Convert.ToDecimal(dr["IngresoBaseSLD"]);
                this.TarifaSLD.Value = Convert.ToDecimal(dr["TarifaSLD"]);
                this.VlrTrabajadorSLD.Value = Convert.ToDecimal(dr["VlrTrabajadorSLD"]);
                this.VlrEmpresaSLD.Value = Convert.ToDecimal(dr["VlrEmpresaSLD"]);
                this.VlrNominaSLD.Value = Convert.ToDecimal(dr["VlrNominaSLD"]);
                this.VlrUPC.Value = Convert.ToDecimal(dr["VlrUPC"]);
                this.AutorizacionEnf.Value = dr["AutorizacionEnf"].ToString();
                this.VlrIncapacidad.Value = Convert.ToDecimal(dr["VlrIncapacidad"]);
                this.AutorizacioMat.Value = dr["AutorizacioMat"].ToString();
                this.VlrMaternidad.Value = Convert.ToDecimal(dr["VlrMaternidad"]);
                this.DiasCotizadosARP.Value = Convert.ToByte(dr["DiasCotizadosARP"]);
                this.IngresoBaseARP.Value = Convert.ToDecimal(dr["IngresoBaseARP"]);
                this.TarifaARP.Value = Convert.ToDecimal(dr["TarifaARP"]);
                this.CtoARP.Value = dr["CtoARP"].ToString();
                this.VlrARP.Value = Convert.ToDecimal(dr["VlrARP"]);
                this.CajaNOID.Value = dr["CajaNOID"].ToString();
                this.DiasCotizadosCCF.Value = Convert.ToByte(dr["DiasCotizadosCCF"]);
                this.IngresoBaseCCF.Value = Convert.ToDecimal(dr["IngresoBaseCCF"]);
                this.TarifaCCF.Value = Convert.ToDecimal(dr["TarifaCCF"]);
                this.VlrCCF.Value = Convert.ToDecimal(dr["VlrCCF"]);
                this.TarifaIBF.Value = Convert.ToDecimal(dr["TarifaIBF"]);
                this.VlrICBF.Value = Convert.ToDecimal(dr["VlrIBF"]);
                this.TarifaSEN.Value = Convert.ToDecimal(dr["TarifaSEN"]);
                this.VlrSEN.Value = Convert.ToDecimal(dr["VlrSEN"]);
                this.IndExoneradoCCF.Value = Convert.ToBoolean(dr["IndExoneradoCCF"]);
                this.VlrTrabajadorVOL.Value = 0;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noPlanillaAportesDeta()
        {
            this.InitCols();
        }

          /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.TipoTrplanilla = new UDTSQL_tinyint();
            this.TipoDocNomina = new UDTSQL_tinyint();
            this.EmpleadoID = new UDT_EmpleadoID();
            this.TipoCotizante = new UDTSQL_tinyint();
            this.SubTipo = new UDTSQL_tinyint();
            this.ExtranjeroInd = new UDT_SiNo();
            this.ExteriorInd = new UDT_SiNo();
            this.LugarGeograficoID = new UDT_LugarGeograficoID();
            this.Sueldo = new UDT_Valor();
            this.SalIntegralInd = new UDT_SiNo();
            this.INGInd = new UDT_SiNo();
            this.RETInd = new UDT_SiNo();
            this.TDEInd = new UDT_SiNo();
            this.TAEInd = new UDT_SiNo();
            this.TDPInd = new UDT_SiNo();
            this.TAPInd = new UDT_SiNo();
            this.VSPInd = new UDT_SiNo();
            this.VTEInd = new UDT_SiNo();
            this.VSTInd = new UDT_SiNo();
            this.SLNInd = new UDT_SiNo();
            this.IGEInd = new UDT_SiNo();
            this.LMAInd = new UDT_SiNo();
            this.VACInd = new UDT_SiNo();
            this.AVPInd = new UDT_SiNo();
            this.VCTInd = new UDT_SiNo();
            this.IRPInd = new UDT_SiNo();
            this.FondoPension = new UDT_FondoNOID();
            this.FondoPensionTR = new UDT_FondoNOID();
            this.DiasCotizadosPEN = new UDTSQL_tinyint();
            this.IngresoBasePEN = new UDT_Valor();
            this.TarifaPEN = new UDT_PorcentajeID();
            this.VlrEmpresaPEN = new UDT_Valor();
            this.VlrNominaPEN = new UDT_Valor();
            this.VlrTrabajadorVOL = new UDT_Valor();
            this.VlrEmpresaVOL = new UDT_Valor();
            this.VlrNoRetenido = new UDT_Valor();
            this.VlrSolidaridad = new UDT_Valor();
            this.VlrSubsistencia = new UDT_Valor();
            this.VlrNominaSOL = new UDT_Valor();
            this.VlrTrabajadorPEN = new UDT_Valor();
            this.FondoSalud = new UDT_FondoNOID();
            this.FondoSaludTR = new UDT_FondoNOID();
            this.DiasCotizadosSLD = new UDTSQL_tinyint();
            this.IngresoBaseSLD = new UDT_Valor();
            this.TarifaSLD = new UDT_PorcentajeID();
            this.VlrTrabajadorSLD = new UDT_Valor();
            this.VlrEmpresaSLD = new UDT_Valor();
            this.VlrNominaSLD = new UDT_Valor();
            this.VlrUPC = new UDT_Valor();
            this.AutorizacionEnf = new UDTSQL_varchar(15);
            this.VlrIncapacidad = new UDT_Valor();
            this.AutorizacioMat = new UDTSQL_varchar(15);
            this.VlrMaternidad = new UDT_Valor();
            this.DiasCotizadosARP = new UDTSQL_tinyint();
            this.IngresoBaseARP = new UDT_Valor();
            this.TarifaARP = new UDT_PorcentajeID();
            this.CtoARP = new UDTSQL_varchar(9);
            this.VlrARP = new UDT_Valor();
            this.CajaNOID = new UDT_CajaNOID();
            this.DiasCotizadosCCF = new UDTSQL_tinyint();
            this.IngresoBaseCCF = new UDT_Valor();
            this.TarifaCCF = new UDT_PorcentajeID();
            this.VlrCCF = new UDT_Valor();
            this.TarifaIBF = new UDT_PorcentajeID();
            this.VlrICBF = new UDT_Valor();
            this.TarifaSEN = new UDT_PorcentajeID();
            this.VlrSEN = new UDT_Valor();
            this.IndExoneradoCCF = new UDT_SiNo();
            this.Consecutivo = new UDT_Consecutivo();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoTrplanilla { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoDocNomina { get; set; }

        [DataMember]
        public UDT_EmpleadoID EmpleadoID { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoCotizante { get; set; }

        [DataMember]
        public UDTSQL_tinyint SubTipo { get; set; }
    
        [DataMember]
        public UDT_SiNo ExtranjeroInd { get; set; }
        
        [DataMember]
        public UDT_SiNo ExteriorInd { get; set; }

        [DataMember]
        public UDT_LugarGeograficoID LugarGeograficoID { get; set; }

        [DataMember]
        public UDT_Valor Sueldo { get; set; }

        [DataMember]
        public UDT_SiNo SalIntegralInd { get; set; }

        [DataMember]
        public UDT_SiNo INGInd { get; set; }

        [DataMember]
        public UDT_SiNo RETInd { get; set; }

        [DataMember]
        public UDT_SiNo TDEInd { get; set; }

        [DataMember]
        public UDT_SiNo TAEInd { get; set; }

        [DataMember]
        public UDT_SiNo TDPInd { get; set; }

        [DataMember]
        public UDT_SiNo TAPInd { get; set; }

        [DataMember]
        public UDT_SiNo VSPInd { get; set; }

        [DataMember]
        public UDT_SiNo VTEInd { get; set; }

        [DataMember]
        public UDT_SiNo VSTInd { get; set; }

        [DataMember]
        public UDT_SiNo SLNInd { get; set; }

        [DataMember]
        public UDT_SiNo IGEInd { get; set; }

        [DataMember]
        public UDT_SiNo LMAInd { get; set; }

        [DataMember]
        public UDT_SiNo VACInd { get; set; }

        [DataMember]
        public UDT_SiNo AVPInd { get; set; }

        [DataMember]
        public UDT_SiNo VCTInd { get; set; }

        [DataMember]
        public UDT_SiNo IRPInd { get; set; }
        
        [DataMember]
        public UDT_FondoNOID FondoPension { get; set; }

        [DataMember]
        public UDT_FondoNOID FondoPensionTR { get; set; }

        [DataMember]
        public UDTSQL_tinyint DiasCotizadosPEN { get; set; }

        [DataMember]
        public UDT_Valor IngresoBasePEN { get; set; }

        [DataMember]
        public UDT_PorcentajeID TarifaPEN { get; set; }

        [DataMember]
        public UDT_Valor VlrTrabajadorPEN { get; set; }

        [DataMember]
        public UDT_Valor VlrEmpresaPEN { get; set; }

        [DataMember]
        public UDT_Valor VlrNominaPEN { get; set; }

        [DataMember]
        public UDT_Valor VlrTrabajadorVOL { get; set; }

        [DataMember]
        public UDT_Valor VlrEmpresaVOL { get; set; }

        [DataMember]
        public UDT_Valor VlrSolidaridad { get; set; }

        [DataMember]
        public UDT_Valor VlrSubsistencia { get; set; }

        [DataMember]
        public UDT_Valor VlrNominaSOL { get; set; }

        [DataMember]
        public UDT_Valor VlrNoRetenido { get; set; }

        [DataMember]
        public UDT_FondoNOID FondoSalud { get; set; }

        [DataMember]
        public UDT_FondoNOID FondoSaludTR { get; set; }

        [DataMember]
        public UDTSQL_tinyint DiasCotizadosSLD { get; set; }

        [DataMember]
        public UDT_Valor IngresoBaseSLD { get; set; }

        [DataMember]
        public UDT_PorcentajeID TarifaSLD { get; set; }

        [DataMember]
        public UDT_Valor VlrTrabajadorSLD { get; set; }

        [DataMember]
        public UDT_Valor VlrEmpresaSLD { get; set; }

        [DataMember]
        public UDT_Valor VlrNominaSLD { get; set; }

        [DataMember]
        public UDT_Valor VlrUPC { get; set; }

        [DataMember]
        public UDTSQL_varchar AutorizacionEnf { get; set; }

        [DataMember]
        public UDT_Valor VlrIncapacidad { get; set; }
        
        [DataMember]
        public UDTSQL_varchar AutorizacioMat { get; set; }

        [DataMember]
        public UDT_Valor VlrMaternidad { get; set; }

        [DataMember]
        public UDTSQL_tinyint DiasCotizadosARP { get; set; }

        [DataMember]
        public UDT_Valor IngresoBaseARP { get; set; }

        [DataMember]
        public UDT_PorcentajeID TarifaARP { get; set; }

        [DataMember]
        public UDTSQL_varchar CtoARP { get; set; }

        [DataMember]
        public UDT_Valor VlrARP { get; set; }

        [DataMember]
        public UDT_CajaNOID CajaNOID { get; set; }

        [DataMember]
        public UDTSQL_tinyint DiasCotizadosCCF { get; set; }

        [DataMember]
        public UDT_Valor IngresoBaseCCF { get; set; }

        [DataMember]
        public UDT_PorcentajeID TarifaCCF { get; set; }

        [DataMember]
        public UDT_Valor VlrCCF { get; set; }

        [DataMember]
        public UDT_PorcentajeID TarifaIBF { get; set; }

        [DataMember]
        public UDT_Valor VlrICBF { get; set; }

        [DataMember]
        public UDT_PorcentajeID TarifaSEN { get; set; }

        [DataMember]
        public UDT_Valor VlrSEN { get; set; }

        [DataMember]
        public UDT_SiNo IndExoneradoCCF { get; set; }
        
        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }


        #endregion
    }
}
