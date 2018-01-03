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
    /// Models DTO_ccSolicitudComponentes
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccSolicitudComponentes
    {
        #region DTO_ccSolicitudComponentes

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccSolicitudComponentes(IDataReader dr)
        {
            InitCols();
            try
            {
                ///Campo Adicional
                this.Descripcion.Value = dr["Descriptivo"].ToString();

                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.ComponenteCarteraID.Value = dr["ComponenteCarteraID"].ToString();
                this.CuotaValor.Value = Convert.ToDecimal(dr["CuotaValor"]);
                this.TotalValor.Value = Convert.ToDecimal(dr["TotalValor"]);
                this.CompInvisibleInd.Value = Convert.ToBoolean(dr["CompInvisibleInd"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
                if (!string.IsNullOrWhiteSpace(dr["PorCapital"].ToString()))
                    this.PorCapital.Value = Convert.ToDecimal(dr["PorCapital"]);

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
        public DTO_ccSolicitudComponentes()
        {
            InitCols();
        }

        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.ComponenteCarteraID = new UDT_ComponenteCarteraID();
            this.CuotaValor = new UDT_Valor();
            this.TotalValor = new UDT_Valor();
            this.Consecutivo = new UDT_Consecutivo();
            this.Descripcion = new UDT_DescripTBase();
            //Campos Extras
            this.NroComponente = new UDTSQL_tinyint();
            this.CompInvisibleInd = new UDT_SiNo();
            this.PagoCuota = new UDT_Valor();
            this.PagoTotal = new UDT_Valor();
            this.Porcentaje = new UDT_PorcentajeID();
            this.PorCapital = new UDT_PorcentajeCarteraID();
        }

        #endregion

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_ComponenteCarteraID ComponenteCarteraID { get; set; }

        [DataMember]
        public UDT_Valor CuotaValor { get; set; }

        [DataMember]
        public UDT_Valor TotalValor { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        [DataMember]
        public UDT_DescripTBase Descripcion { get; set; }

       //Campos Adicionales
        [DataMember]
        public UDTSQL_tinyint NroComponente { get; set; }

        [DataMember]
        public UDT_SiNo CompInvisibleInd { get; set; }
        
        [DataMember]
        public UDT_Valor PagoCuota { get; set; }

        [DataMember]
        public UDT_Valor PagoTotal { get; set; }

        [DataMember]
        public UDT_PorcentajeID Porcentaje { get; set; }

        [DataMember]
        public UDT_PorcentajeCarteraID PorCapital { get; set; }
    }
}
