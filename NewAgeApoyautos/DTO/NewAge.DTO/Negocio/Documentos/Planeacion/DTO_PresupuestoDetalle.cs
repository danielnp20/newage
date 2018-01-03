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
    public class DTO_PresupuestoDetalle
    {
        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_PresupuestoDetalle()
        {
            this.InitCols();
        }

        /// <summary>
        /// Constructor de lectura
        /// </summary>
        /// <param name="dr"></param>
        public DTO_PresupuestoDetalle(IDataReader dr)
        {
            InitCols();
            try
            {
                this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
                this.LineaPresupuestoDescripcion.Value = dr["LineaPresupuestoDescripcion"].ToString();
                this.ConceptoCargoID.Value = dr["ConceptoCargoID"].ToString();
                this.ConceptoCargoDescripcion.Value = dr["ConceptoCargoDescripcion"].ToString();
                this.ValorML.Value = Convert.ToDecimal(dr["ValorML"]);
                this.ValorME.Value = Convert.ToDecimal(dr["ValorME"]);
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
            this.LineaPresupuestoID = new UDT_ConceptoCargoID();
            this.LineaPresupuestoDescripcion = new UDT_DescripTBase();
            this.ConceptoCargoID = new UDT_ConceptoCargoID();
            this.ConceptoCargoDescripcion = new UDT_DescripTBase();
            this.ValorML = new UDT_Valor();
            this.ValorME = new UDT_Valor();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_ConceptoCargoID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_DescripTBase LineaPresupuestoDescripcion { get; set; }
        
        [DataMember]
        public UDT_ConceptoCargoID ConceptoCargoID { get; set; }

        [DataMember]
        public UDT_DescripTBase ConceptoCargoDescripcion { get; set; }

        [DataMember]
        public UDT_Valor ValorML { get; set; }

        [DataMember]
        public UDT_Valor ValorME { get; set; }

        #endregion
    }
}
