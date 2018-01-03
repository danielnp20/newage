using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_plCierreCompromisos
    {
        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_plCierreCompromisos()
        {
            this.InitCols();
        }

        /// <summary>
        /// Constructor de lectura
        /// </summary>
        /// <param name="dr"></param>
        public DTO_plCierreCompromisos(IDataReader dr)
        {
            InitCols();
            try
            {
                this.PeriodoID.Value = Convert.ToDateTime(dr["PeriodoID"]);
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.ProyectoID.Value = Convert.ToString(dr["ProyectoID"]);
                this.CentroCostoID.Value = Convert.ToString(dr["CentroCostoID"]);
                this.LineaPresupuestoID.Value = Convert.ToString(dr["LineaPresupuestoID"]);
                this.CompActLocML.Value = Convert.ToDecimal(dr["CompActLocML"]);
                this.CompActLocME.Value = Convert.ToDecimal(dr["CompActLocME"]);
                this.CompActExtME.Value = Convert.ToDecimal(dr["CompActExtME"]);
                this.CompActExtML.Value = Convert.ToDecimal(dr["CompActExtML"]);
                this.CompSgtIniLocML.Value = Convert.ToDecimal(dr["CompSgtIniLocML"]);
                this.CompSgtIniLocME.Value = Convert.ToDecimal(dr["CompSgtIniLocME"]);
                this.CompSgtIniExtME.Value = Convert.ToDecimal(dr["CompSgtIniExtME"]);
                this.CompSgtIniExtML.Value = Convert.ToDecimal(dr["CompSgtIniExtML"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.PeriodoID= new UDT_PeriodoID();
            this.NumeroDoc= new UDT_Consecutivo();
            this.ProyectoID= new UDT_ProyectoID();
            this.CentroCostoID= new UDT_CentroCostoID();
            this.LineaPresupuestoID= new UDT_LineaPresupuestoID();
            this.CompActLocML= new UDT_Valor();
            this.CompActLocME = new UDT_Valor();
            this.CompActExtME = new UDT_Valor();
            this.CompActExtML = new UDT_Valor();
            this.CompSgtIniLocML = new UDT_Valor();
            this.CompSgtIniLocME = new UDT_Valor();
            this.CompSgtIniExtME = new UDT_Valor();
            this.CompSgtIniExtML = new UDT_Valor();
            this.Consecutivo= new UDT_Consecutivo();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_PeriodoID PeriodoID { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        public UDT_LineaPresupuestoID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_Valor CompActLocML { get; set; }

        [DataMember]
        public UDT_Valor CompActLocME { get; set; }

        [DataMember]
        public UDT_Valor CompActExtME { get; set; }

        [DataMember]
        public UDT_Valor CompActExtML { get; set; }

        [DataMember]
        public UDT_Valor CompSgtIniLocML { get; set; }

        [DataMember]
        public UDT_Valor CompSgtIniLocME { get; set; }

        [DataMember]
        public UDT_Valor CompSgtIniExtME { get; set; }

        [DataMember]
        public UDT_Valor CompSgtIniExtML { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }
        
        #endregion
    }
}
