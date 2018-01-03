using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_QueryInformeMensualCierre
    {
        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_QueryInformeMensualCierre()
        {
            this.InitCols();
        }

        /// <summary>
        /// Constructor de lectura
        /// </summary>
        /// <param name="dr"></param>
        public DTO_QueryInformeMensualCierre(IDataReader dr, bool socioInd = false)
        {
            InitCols();
            try
            {
                this.PeriodoID.Value = Convert.ToDateTime(dr["PeriodoID"]);
                this.ProyectoID.Value = dr["ProyectoID"].ToString();
                this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                this.RecursoID.Value = dr["RecursoID"].ToString();
                this.ActividadID.Value = dr["ActividadID"].ToString();
                this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
                this.ContratoID.Value = dr["ContratoID"].ToString();
                this.Campo.Value = dr["Campo"].ToString();
                this.Pozo.Value = dr["Pozo"].ToString();
                this.SaldoInicial.Value = Convert.ToDecimal(dr["SaldoInicial"]);
                this.SaldoEnero.Value = Convert.ToDecimal(dr["SaldoEnero"]);
                this.SaldoFebrero.Value = Convert.ToDecimal(dr["SaldoFebrero"]);
                this.SaldoMarzo.Value = Convert.ToDecimal(dr["SaldoMarzo"]);
                this.SaldoAbril.Value = Convert.ToDecimal(dr["SaldoAbril"]);
                this.SaldoMayo.Value = Convert.ToDecimal(dr["SaldoMayo"]);
                this.SaldoJunio.Value = Convert.ToDecimal(dr["SaldoJunio"]);
                this.SaldoJulio.Value = Convert.ToDecimal(dr["SaldoJulio"]);
                this.SaldoAgosto.Value = Convert.ToDecimal(dr["SaldoAgosto"]);
                this.SaldoSeptiembre.Value = Convert.ToDecimal(dr["SaldoSeptiembre"]);
                this.SaldoOctubre.Value = Convert.ToDecimal(dr["SaldoOctubre"]);
                this.SaldoNoviembre.Value = Convert.ToDecimal(dr["SaldoNoviembre"]);
                this.SaldoDiciembre.Value = Convert.ToDecimal(dr["SaldoDiciembre"]);
                this.SaldoFinal.Value = Convert.ToDecimal(dr["SaldoFinal"]);
                //Operacion Conjuntas
                try { if(socioInd) this.SocioID.Value = dr["SocioID"].ToString(); } catch (Exception) { throw;} } catch (Exception e)   { ; }
        }

        /// <summary>
        /// Constructor de lectura
        /// </summary>
        /// <param name="dr"></param>
        public DTO_QueryInformeMensualCierre(bool InitValues)
        {
            InitCols();
            try
            {
                this.SaldoInicial.Value = 0;
                this.SaldoEnero.Value = 0;
                this.SaldoFebrero.Value = 0;
                this.SaldoMarzo.Value = 0;
                this.SaldoAbril.Value = 0;
                this.SaldoMayo.Value = 0;
                this.SaldoJunio.Value = 0;
                this.SaldoJulio.Value = 0;
                this.SaldoAgosto.Value = 0;
                this.SaldoSeptiembre.Value = 0;
                this.SaldoOctubre.Value = 0;
                this.SaldoNoviembre.Value = 0;
                this.SaldoDiciembre.Value = 0;
                this.SaldoFinal.Value = 0;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.PeriodoID= new UDT_PeriodoID();
            this.MonedaID = new UDT_MonedaID();
            this.ProyectoID= new UDT_ProyectoID();
            this.CentroCostoID= new UDT_CentroCostoID();
            this.RecursoID = new UDT_RecursoID();
            this.ActividadID = new UDT_ActividadID();
            this.LineaPresupuestoID= new UDT_LineaPresupuestoID();
            this.ContratoID = new UDT_ContratoID();
            this.Campo = new UDT_AreaFisicaID();
            this.Pozo = new UDT_LugarGeograficoID();
            this.Grupo = new UDT_ActividadID();
            this.SocioID = new UDT_SocioID();
            this.Descriptivo = new UDT_Descriptivo();
            this.SaldoInicial = new UDT_Valor();
            this.SaldoEnero = new UDT_Valor();
            this.SaldoFebrero = new UDT_Valor();
            this.SaldoMarzo = new UDT_Valor();
            this.SaldoAbril = new UDT_Valor();
            this.SaldoMayo = new UDT_Valor();
            this.SaldoJunio = new UDT_Valor();
            this.SaldoJulio = new UDT_Valor();
            this.SaldoAgosto = new UDT_Valor();
            this.SaldoSeptiembre = new UDT_Valor();
            this.SaldoOctubre = new UDT_Valor();
            this.SaldoNoviembre = new UDT_Valor();
            this.SaldoDiciembre = new UDT_Valor();
            this.SaldoFinal = new UDT_Valor();
            this.DetalleNivel1 = new List<DTO_QueryInformeMensualCierre>();
            this.DetalleNivel2 = new List<DTO_QueryInformeMensualCierre>();
            this.DetalleNivel3 = new List<DTO_QueryInformeMensualCierre>();
        }  

        #region Propiedades

        [DataMember]
        public UDT_PeriodoID PeriodoID { get; set; }

        [DataMember]
        public UDT_MonedaID MonedaID { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        public UDT_RecursoID RecursoID { get; set; }

        [DataMember]
        public UDT_ActividadID ActividadID { get; set; }

        [DataMember]
        public UDT_LineaPresupuestoID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_ContratoID ContratoID { get; set; }

        [DataMember]
        public UDT_AreaFisicaID Campo { get; set; }

        [DataMember]
        public UDT_LugarGeograficoID Pozo { get; set; }

        [DataMember]
        public UDT_ActividadID Grupo { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_SocioID SocioID { get; set; }

        [DataMember]
        public UDT_Descriptivo Descriptivo { get; set; }

        [DataMember]
        public UDT_Valor SaldoInicial { get; set; }

        [DataMember]
        public UDT_Valor SaldoEnero { get; set; }

        [DataMember]
        public UDT_Valor SaldoFebrero { get; set; }

        [DataMember]
        public UDT_Valor SaldoMarzo { get; set; }

        [DataMember]
        public UDT_Valor SaldoAbril { get; set; }

        [DataMember]
        public UDT_Valor SaldoMayo { get; set; }

        [DataMember]
        public UDT_Valor SaldoJunio { get; set; }

        [DataMember]
        public UDT_Valor SaldoJulio { get; set; }

        [DataMember]
        public UDT_Valor SaldoAgosto { get; set; }

        [DataMember]
        public UDT_Valor SaldoSeptiembre { get; set; }

        [DataMember]
        public UDT_Valor SaldoOctubre { get; set; }

        [DataMember]
        public UDT_Valor SaldoNoviembre { get; set; }

        [DataMember]
        public UDT_Valor SaldoDiciembre { get; set; }

        [DataMember]
        public UDT_Valor SaldoFinal { get; set; }

        [DataMember]
        [AllowNull]
        public List<DTO_QueryInformeMensualCierre> DetalleNivel1 { get; set; }

        [DataMember]
        [AllowNull]
        public List<DTO_QueryInformeMensualCierre> DetalleNivel2 { get; set; }

        [DataMember]
        [AllowNull]
        public List<DTO_QueryInformeMensualCierre> DetalleNivel3 { get; set; }

        #endregion
    }
}
