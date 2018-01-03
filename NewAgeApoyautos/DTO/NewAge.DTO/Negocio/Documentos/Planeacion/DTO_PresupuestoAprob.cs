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
    public class DTO_PresupuestoAprob
    {
        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_PresupuestoAprob()
        {
            this.InitCols();
        }

        /// <summary>
        /// Constructor de lectura
        /// </summary>
        /// <param name="dr"></param>
        public DTO_PresupuestoAprob(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.PrefijoID.Value = dr["PrefijoID"].ToString();
                this.PeriodoDoc.Value =  Convert.ToDateTime(dr["PeriodoDoc"]);
                this.DocumentoNro.Value = Convert.ToInt32(dr["DocumentoNro"]);
                this.PrefDoc = this.PrefijoID.Value + "-" + this.DocumentoNro.Value.ToString();
                this.ProyectoID.Value = dr["ProyectoID"].ToString();
                this.ProyectoDesc.Value = dr["ProyectoDesc"].ToString();
                this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
                this.LineaPresDesc.Value = dr["LineaPresDesc"].ToString();
                this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                this.CentroCostoDesc.Value = dr["CentroCostoDesc"].ToString();
                this.TotalML.Value = Convert.ToDecimal(dr["ValorML"]);
                this.TotalME.Value = Convert.ToDecimal(dr["ValorME"]);
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
            this.Aprobado = new UDT_SiNo();
            this.Rechazado = new UDT_SiNo();
            this.Observacion = new UDT_DescripTExt();
            this.NumeroDoc = new UDT_Consecutivo();
            this.PrefijoID = new UDT_PrefijoID();
            this.PeriodoDoc = new UDTSQL_smalldatetime();
            this.DocumentoNro = new UDT_Consecutivo();
            this.ProyectoID = new UDT_ProyectoID();
            this.ProyectoDesc = new UDT_Descriptivo();
            this.LineaPresupuestoID = new UDT_AreaPresupuestalID();
            this.LineaPresDesc = new UDT_DescripTBase();
            this.CentroCostoID = new UDT_AreaPresupuestalID();
            this.CentroCostoDesc = new UDT_DescripTBase(); 
            this.TotalML = new UDT_Valor();
            this.TotalME = new UDT_Valor();
            this.Detalle = new List<DTO_PresupuestoAprobDetalle>();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public int Index { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_SiNo Aprobado { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_SiNo Rechazado { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTExt Observacion { get; set; }
       
        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_PrefijoID PrefijoID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Consecutivo DocumentoNro { get; set; }

        [DataMember]
        [AllowNull]
        public string PrefDoc { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_smalldatetime PeriodoDoc { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ProyectoDesc { get; set; }

        [DataMember]
        public UDT_AreaPresupuestalID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_DescripTBase LineaPresDesc { get; set; }

        [DataMember]
        public UDT_AreaPresupuestalID CentroCostoID { get; set; }

        [DataMember]
        public UDT_DescripTBase CentroCostoDesc { get; set; }

        [DataMember]
        public UDT_Valor TotalML { get; set; }

        [DataMember]
        public UDT_Valor TotalME { get; set; }

        [DataMember]
        public List<DTO_PresupuestoAprobDetalle> Detalle { get; set; }

        #endregion
    }
}
