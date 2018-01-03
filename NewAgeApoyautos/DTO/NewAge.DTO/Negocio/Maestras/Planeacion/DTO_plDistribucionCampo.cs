using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_plDistribucionCampo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_plDistribucionCampo : DTO_MasterComplex
    {
        #region DTO_plDistribucionCampo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_plDistribucionCampo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.AreaFisicaDesc.Value = dr["AreaFisicaDesc"].ToString();
                }
                this.PeriodoID.Value = Convert.ToDateTime(dr["PeriodoID"]);
                this.AreaFisicaID.Value = dr["AreaFisicaID"].ToString();
                this.TipoCosteo.Value = Convert.ToByte(dr["TipoCosteo"]);
                this.Porcentaje01.Value = Convert.ToDecimal(dr["Porcentaje01"]);
                this.Porcentaje02.Value = Convert.ToDecimal(dr["Porcentaje02"]);
                this.Porcentaje03.Value = Convert.ToDecimal(dr["Porcentaje03"]);
                this.Porcentaje04.Value = Convert.ToDecimal(dr["Porcentaje04"]);
                this.Porcentaje05.Value = Convert.ToDecimal(dr["Porcentaje05"]);
                this.Porcentaje06.Value = Convert.ToDecimal(dr["Porcentaje06"]);
                this.Porcentaje07.Value = Convert.ToDecimal(dr["Porcentaje07"]);
                this.Porcentaje08.Value = Convert.ToDecimal(dr["Porcentaje08"]);
                this.Porcentaje09.Value = Convert.ToDecimal(dr["Porcentaje09"]);
                this.Porcentaje10.Value = Convert.ToDecimal(dr["Porcentaje10"]);
                this.Porcentaje11.Value = Convert.ToDecimal(dr["Porcentaje11"]);
                this.Porcentaje12.Value = Convert.ToDecimal(dr["Porcentaje12"]);

            }
            catch(Exception e)
            {
               throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_plDistribucionCampo() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas
            this.PeriodoID = new UDT_PeriodoID();
            this.AreaFisicaID = new UDT_BasicID();
            this.AreaFisicaDesc = new UDT_Descriptivo();
            this.TipoCosteo = new UDTSQL_tinyint();
            this.Porcentaje01 = new UDT_PorcentajeID();
            this.Porcentaje02 = new UDT_PorcentajeID();
            this.Porcentaje03 = new UDT_PorcentajeID();
            this.Porcentaje04 = new UDT_PorcentajeID();
            this.Porcentaje05 = new UDT_PorcentajeID();
            this.Porcentaje06 = new UDT_PorcentajeID();
            this.Porcentaje07 = new UDT_PorcentajeID();
            this.Porcentaje08 = new UDT_PorcentajeID();
            this.Porcentaje09 = new UDT_PorcentajeID();
            this.Porcentaje10 = new UDT_PorcentajeID();
            this.Porcentaje11 = new UDT_PorcentajeID();
            this.Porcentaje12 = new UDT_PorcentajeID();
        }

        public DTO_plDistribucionCampo(DTO_MasterComplex comp)
            : base(comp)
        {
            InitCols();
        }

        public DTO_plDistribucionCampo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_PeriodoID PeriodoID { get; set; }

        [DataMember]
        public UDT_BasicID AreaFisicaID { get; set; }

        [DataMember]
        public UDT_Descriptivo AreaFisicaDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoCosteo { get; set; }

        [DataMember]
        public UDT_PorcentajeID Porcentaje01 { get; set; }

        [DataMember]
        public UDT_PorcentajeID Porcentaje02 { get; set; }

        [DataMember]
        public UDT_PorcentajeID Porcentaje03 { get; set; }

        [DataMember]
        public UDT_PorcentajeID Porcentaje04 { get; set; }

        [DataMember]
        public UDT_PorcentajeID Porcentaje05 { get; set; }

        [DataMember]
        public UDT_PorcentajeID Porcentaje06 { get; set; }

        [DataMember]
        public UDT_PorcentajeID Porcentaje07 { get; set; }

        [DataMember]
        public UDT_PorcentajeID Porcentaje08 { get; set; }

        [DataMember]
        public UDT_PorcentajeID Porcentaje09 { get; set; }

        [DataMember]
        public UDT_PorcentajeID Porcentaje10 { get; set; }

        [DataMember]
        public UDT_PorcentajeID Porcentaje11 { get; set; }

        [DataMember]
        public UDT_PorcentajeID Porcentaje12 { get; set; }



       
    }
}
