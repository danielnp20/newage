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
    public class DTO_PresupuestoAprobDetalle
    {
        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_PresupuestoAprobDetalle()
        {
            this.InitCols();
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_PresupuestoAprobDetalle(IDataReader dr)
        {
           this.InitCols();
            try
            {
                try { this.CentroCostoID.Value = dr["CentroCostoID"].ToString(); }                catch (Exception) { ;}
                try { this.CentroCostoDesc.Value = dr["CentroCostoDesc"].ToString(); }            catch (Exception) { ;}
                try { this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString(); }      catch (Exception) { ;}
                try { this.LineaPresDesc.Value = dr["LineaPresDesc"].ToString(); }                catch (Exception) { ;}  
                try { this.ObservacionDeta.Value = dr["ObservacionDeta"].ToString(); }           catch (Exception) { ;}  
                try { this.ValorML.Value = Convert.ToDecimal(dr["ValorML"]); }                catch (Exception) { ;}
                try { this.ValorME.Value = Convert.ToDecimal(dr["ValorME"]); }                catch (Exception) { ;}
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
            this.CentroCostoID = new UDT_AreaPresupuestalID();
            this.CentroCostoDesc = new UDT_Descriptivo();
            this.LineaPresupuestoID = new UDT_AreaPresupuestalID();
            this.LineaPresDesc = new UDT_Descriptivo();
            this.ObservacionDeta = new UDT_DescripTExt();
            this.ValorML = new UDT_Valor();
            this.ValorME = new UDT_Valor();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_AreaPresupuestalID CentroCostoID { get; set; }

        [DataMember]
        public UDT_Descriptivo CentroCostoDesc { get; set; }

        [DataMember]
        public UDT_AreaPresupuestalID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_Descriptivo LineaPresDesc { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTExt ObservacionDeta { get; set; }

        [DataMember]
        public UDT_Valor ValorML { get; set; }

        [DataMember]
        public UDT_Valor ValorME { get; set; }

        #endregion
    }
}
