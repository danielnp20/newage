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
    /// Models DTO_pyProyectoDocu
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pyProyectoDocu
    {
        #region DTO_pyProyectoDocu

        public DTO_pyProyectoDocu(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.EmpresaID.Value = Convert.ToString(dr["EmpresaID"]);
                if (!string.IsNullOrEmpty(dr["DocSolicitud"].ToString()))
                    this.DocSolicitud.Value = Convert.ToInt32(dr["DocSolicitud"]);
                this.ClienteID.Value = Convert.ToString(dr["ClienteID"]);
                this.EmpresaNombre.Value = Convert.ToString(dr["EmpresaNombre"]);
                this.ResponsableEMP.Value = Convert.ToString(dr["ResponsableEMP"]);
                this.ResponsableCLI.Value = Convert.ToString(dr["ResponsableCLI"]);
                this.ResponsableCorreo.Value = Convert.ToString(dr["ResponsableCorreo"]);
                this.ResponsableTelefono.Value = Convert.ToString(dr["ResponsableTelefono"]);
                this.ContratoID.Value = Convert.ToString(dr["ContratoID"]);
                this.ClaseServicioID.Value = Convert.ToString(dr["ClaseServicioID"]);
                if (!string.IsNullOrEmpty(dr["TipoSolicitud"].ToString()))
                    this.TipoSolicitud.Value = Convert.ToByte(dr["TipoSolicitud"]);
                if (!string.IsNullOrEmpty(dr["PropositoProyecto"].ToString()))
                    this.PropositoProyecto.Value = Convert.ToByte(dr["PropositoProyecto"]);  
                if (!string.IsNullOrEmpty(dr["RecursosXTrabajoInd"].ToString()))
                    this.RecursosXTrabajoInd.Value = Convert.ToBoolean(dr["RecursosXTrabajoInd"]);
                this.DescripcionSOL.Value = Convert.ToString(dr["DescripcionSOL"]);
                this.Licitacion.Value = Convert.ToString(dr["Licitacion"]);
                if (!string.IsNullOrEmpty(dr["TasaCambio"].ToString()))
                    this.TasaCambio.Value = Convert.ToDecimal(dr["TasaCambio"]);
                this.MonedaPresupuesto.Value = Convert.ToString(dr["MonedaPresupuesto"]);
                if (!string.IsNullOrEmpty(dr["PorAjusteCambio"].ToString()))
                    this.PorAjusteCambio.Value = Convert.ToDecimal(dr["PorAjusteCambio"]);
                if (!string.IsNullOrEmpty(dr["TipoAjusteCambio"].ToString()))
                    this.TipoAjusteCambio.Value = Convert.ToByte(dr["TipoAjusteCambio"]);
                if (!string.IsNullOrEmpty(dr["PorIPC"].ToString()))
                    this.PorIPC.Value = Convert.ToDecimal(dr["PorIPC"]);
                if (!string.IsNullOrEmpty(dr["APUIncluyeAIUInd"].ToString()))
                    this.APUIncluyeAIUInd.Value = Convert.ToBoolean(dr["APUIncluyeAIUInd"]);
                if (!string.IsNullOrEmpty(dr["Jerarquia"].ToString()))
                    this.Jerarquia.Value = Convert.ToByte(dr["Jerarquia"]);
                if (!string.IsNullOrEmpty(dr["PorClienteADM"].ToString()))
                    this.PorClienteADM.Value = Convert.ToDecimal(dr["PorClienteADM"]);
                if (!string.IsNullOrEmpty(dr["PorClienteIMP"].ToString()))
                    this.PorClienteIMP.Value = Convert.ToDecimal(dr["PorClienteIMP"]);
                if (!string.IsNullOrEmpty(dr["PorClienteUTI"].ToString()))
                    this.PorClienteUTI.Value = Convert.ToDecimal(dr["PorClienteUTI"]);
                if (!string.IsNullOrEmpty(dr["PorEmpresaADM"].ToString()))
                    this.PorEmpresaADM.Value = Convert.ToDecimal(dr["PorEmpresaADM"]);
                if (!string.IsNullOrEmpty(dr["PorEmpresaIMP"].ToString()))
                    this.PorEmpresaIMP.Value = Convert.ToDecimal(dr["PorEmpresaIMP"]);
                if (!string.IsNullOrEmpty(dr["PorEmpresaUTI"].ToString()))
                    this.PorEmpresaUTI.Value = Convert.ToDecimal(dr["PorEmpresaUTI"]);
                if (!string.IsNullOrEmpty(dr["PorMultiplicadorPresup"].ToString()))
                    this.PorMultiplicadorPresup.Value = Convert.ToDecimal(dr["PorMultiplicadorPresup"]);
                if (!string.IsNullOrEmpty(dr["MultiplicadorActivoInd"].ToString()))
                    this.MultiplicadorActivoInd.Value = Convert.ToBoolean(dr["MultiplicadorActivoInd"]);
                if (!string.IsNullOrEmpty(dr["PorIVA"].ToString()))
                    this.PorIVA.Value = Convert.ToInt32(dr["PorIVA"]);
                if (!string.IsNullOrEmpty(dr["FechaInicio"].ToString()))
                    this.FechaInicio.Value = Convert.ToDateTime(dr["FechaInicio"]);
                if (!string.IsNullOrEmpty(dr["VersionPrevia"].ToString()))
                    this.VersionPrevia.Value = Convert.ToByte(dr["VersionPrevia"]);
                if (!string.IsNullOrEmpty(dr["Version"].ToString()))
                    this.Version.Value = Convert.ToByte(dr["Version"]);
                if (!string.IsNullOrEmpty(dr["TipoRedondeo"].ToString()))
                    this.TipoRedondeo.Value = Convert.ToByte(dr["TipoRedondeo"]);
                if (!string.IsNullOrEmpty(dr["EquipoCantidadInd"].ToString()))
                    this.EquipoCantidadInd.Value = Convert.ToBoolean(dr["EquipoCantidadInd"]);
                if (!string.IsNullOrEmpty(dr["PersonalCantidadInd"].ToString()))
                    this.PersonalCantidadInd.Value = Convert.ToBoolean(dr["PersonalCantidadInd"]);
                if (!string.IsNullOrEmpty(dr["FechaFijaEntregas"].ToString()))
                    this.FechaFijaEntregas.Value = Convert.ToDateTime(dr["FechaFijaEntregas"]);
                this.UsuarioFijaEntregas.Value = Convert.ToString(dr["UsuarioFijaEntregas"]);
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
        public DTO_pyProyectoDocu()
        {
            InitCols();
            this.PorEmpresaADM.Value = 0;
            this.PorEmpresaIMP.Value = 0;
            this.PorEmpresaUTI.Value = 0;
            this.PorClienteADM.Value = 0;
            this.PorClienteIMP.Value = 0;
            this.PorClienteUTI.Value = 0;
            this.Valor.Value = 0;
            this.ValorCliente.Value = 0;
            this.ValorIVA.Value = 0;
            this.ValorOtros.Value = 0;
        }

        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.EmpresaID = new UDT_EmpresaID();
            this.DocSolicitud = new UDT_Consecutivo();
            this.ClienteID = new UDT_ClienteID();
            this.EmpresaNombre = new UDT_DescripTExt();
            this.ResponsableEMP = new UDT_UsuarioID();
            this.ResponsableCLI = new UDT_DescripTBase();
            this.ResponsableCorreo = new UDT_DescripTBase();
            this.ResponsableTelefono = new UDT_DescripTBase();
            this.ContratoID = new UDT_ContratoID();
            this.ClaseServicioID = new UDT_CodigoGrl();
            this.PropositoProyecto = new UDTSQL_tinyint();
            this.TipoSolicitud = new UDTSQL_tinyint();
            this.RecursosXTrabajoInd = new UDT_SiNo();
            this.DescripcionSOL = new UDT_DescripTExt();
            this.Licitacion = new UDT_DescripTBase();
            this.TasaCambio = new UDT_TasaID();
            this.MonedaPresupuesto = new UDT_MonedaID();
            this.PorAjusteCambio = new UDT_PorcentajeID();
            this.TipoAjusteCambio = new UDTSQL_tinyint();
            this.PorIPC = new UDT_PorcentajeID();
            this.APUIncluyeAIUInd = new UDT_SiNo();
            this.Jerarquia = new UDTSQL_tinyint();
            this.PorClienteADM = new UDT_PorcentajeID();
            this.PorClienteIMP = new UDT_PorcentajeID();
            this.PorClienteUTI = new UDT_PorcentajeID();
            this.PorEmpresaADM = new UDT_PorcentajeID();
            this.PorEmpresaIMP = new UDT_PorcentajeID();
            this.PorEmpresaUTI = new UDT_PorcentajeID();
            this.PorMultiplicadorPresup = new UDT_PorcentajeID();
            this.MultiplicadorActivoInd = new UDT_SiNo();
            this.PorIVA = new UDT_PorcentajeID();
            this.FechaInicio = new UDTSQL_smalldatetime();
            this.VersionPrevia = new UDTSQL_tinyint();
            this.Version = new UDTSQL_tinyint();
            this.TipoRedondeo = new UDTSQL_tinyint();
            this.EquipoCantidadInd = new UDT_SiNo();
            this.PersonalCantidadInd = new UDT_SiNo();
            this.FechaFijaEntregas = new UDTSQL_smalldatetime();
            this.UsuarioFijaEntregas = new UDT_UsuarioID();
            //Adicionales
            this.Valor = new UDT_Valor();
            this.ValorCliente = new UDT_Valor();
            this.ValorIVA = new UDT_Valor();
            this.ValorOtros = new UDT_Valor();
            this.ClienteDesc = new UDT_Descriptivo();
            this.PrefDoc = new UDT_DescripTBase();
            this.DetalleTareas = new List<DTO_pyProyectoTarea>();
            this.TareasDeleted = new List<int>();
            this.RecursosDeleted = new List<int>();
        }
        #endregion


        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_Consecutivo DocSolicitud { get; set; }     

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDT_DescripTExt EmpresaNombre { get; set; }
        
        [DataMember]
        public UDT_UsuarioID ResponsableEMP { get; set; }

        [DataMember]
        public UDT_DescripTBase ResponsableCLI { get; set; }

        [DataMember]
        public UDT_DescripTBase ResponsableCorreo { get; set; }

        [DataMember]
        public UDT_DescripTBase ResponsableTelefono { get; set; }

        [DataMember]
        public UDT_ContratoID ContratoID { get; set; }
        
        [DataMember]
        public UDT_CodigoGrl ClaseServicioID { get; set; }

        [DataMember]
        public UDTSQL_tinyint PropositoProyecto { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoSolicitud { get; set; }

        [DataMember]
        public UDT_SiNo RecursosXTrabajoInd { get; set; }

        [DataMember]
        public UDT_DescripTExt DescripcionSOL { get; set; }

        [DataMember]
        public UDT_DescripTBase Licitacion { get; set; }

        [DataMember]
        public UDT_TasaID TasaCambio { get; set; }

        [DataMember]
        public UDT_MonedaID MonedaPresupuesto { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorAjusteCambio { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoAjusteCambio { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorIPC { get; set; }

        [DataMember]
        public UDT_SiNo APUIncluyeAIUInd { get; set; }

        [DataMember]
        public UDTSQL_tinyint Jerarquia { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorClienteADM { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorClienteIMP { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorClienteUTI { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorEmpresaADM { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorEmpresaIMP { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorEmpresaUTI { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorMultiplicadorPresup { get; set; }

        [DataMember]
        public UDT_SiNo MultiplicadorActivoInd { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorIVA { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaInicio { get; set; }

        [DataMember]
        public UDTSQL_tinyint VersionPrevia { get; set; }

        [DataMember]
        public UDTSQL_tinyint Version { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoRedondeo { get; set; }

        [DataMember]
        public UDT_SiNo EquipoCantidadInd { get; set; }

        [DataMember]
        public UDT_SiNo PersonalCantidadInd { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaFijaEntregas { get; set; }

        [DataMember]
        public UDT_UsuarioID UsuarioFijaEntregas { get; set; }

        //Adicionales
        [DataMember]
        public UDT_Valor Valor { get; set; } //Total Presupuesto

        [DataMember]
        public UDT_Valor ValorCliente { get; set; } //Total Presupuesto Cliente

        [DataMember]
        public UDT_Valor ValorIVA { get; set; } //Total IVA

        [DataMember]
        public UDT_Valor ValorOtros { get; set; } //Total Otros

        [DataMember]
        public UDT_Descriptivo ClienteDesc { get; set; }

        [DataMember]
        public UDT_DescripTBase PrefDoc { get; set; }

        [DataMember]
        public List<DTO_pyProyectoTarea> DetalleTareas { get; set; }

        [DataMember]
        public List<int> TareasDeleted { get; set; }

        [DataMember]
        public List<int> RecursosDeleted { get; set; }

        [DataMember]
        public decimal? VlrAnticipoFactVenta { get; set; }

        [DataMember]
        public decimal? VlrPorcAnticipo { get; set; } 

        #region Valores Control Costos
        [DataMember]
        public decimal? VlrUtilOperacional { get; set; } //Total Detalle Costos Final Proyecto 

        [DataMember]
        public decimal? VlrEstimadoNomina { get; set; }

        [DataMember]
        public decimal? VlrEstimadoAdmin { get; set; } 

        [DataMember]
        public decimal? VlrReteICA { get; set; } 

        [DataMember]
        public decimal? Vlr4x1000 { get; set; } 

        [DataMember]
        public decimal? VlrFinancieros { get; set; }  

        [DataMember]
        public decimal? VlrPoliza { get; set; } 

        [DataMember]
        public decimal? VlrGarSoporte { get; set; }  

        [DataMember]
        public decimal? VlrIntereses { get; set; } 

        [DataMember]
        public decimal? VlrCostosAdmin { get; set; }

        [DataMember]
        public decimal? VlrUtilSinImpuestos { get; set; }

        [DataMember]
        public decimal? VlrComision { get; set; }

        [DataMember]
        public decimal? VlrNETO { get; set; } 

        #endregion
    }
}
