﻿using System;
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
    /// Models DTO_ccLineaComponente
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccLineaComponente : DTO_MasterComplex
    {
        #region DTO_ccLineaComponente
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccLineaComponente(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                    this.LineaCreditoDesc.Value = dr["LineaCreditoDesc"].ToString();
                if (!isReplica)
                    this.ComponenteCarteraDesc.Value = dr["ComponenteCarteraDesc"].ToString();
                this.LineaCreditoID.Value = dr["LineaCreditoID"].ToString();
                this.ComponenteCarteraID.Value = dr["ComponenteCarteraID"].ToString();
                if (!string.IsNullOrEmpty(dr["Valor"].ToString()))
                    this.Valor.Value = Convert.ToDecimal(dr["Valor"].ToString());
                if (!string.IsNullOrEmpty(dr["PorcentajeID"].ToString()))
                    this.PorcentajeID.Value = Convert.ToDecimal(dr["PorcentajeID"].ToString());
                if (!string.IsNullOrEmpty(dr["FactorCesion"].ToString()))
                    this.FactorCesion.Value = Convert.ToDecimal(dr["FactorCesion"].ToString());
              
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccLineaComponente()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.LineaCreditoID = new UDT_BasicID();
            this.LineaCreditoDesc = new UDT_Descriptivo();
            this.ComponenteCarteraID = new UDT_BasicID();
            this.ComponenteCarteraDesc = new UDT_Descriptivo();
            this.Valor = new UDT_Valor();
            this.PorcentajeID = new UDT_PorcentajeCarteraID();
            this.FactorCesion = new UDT_PorcentajeID();
        
        }

        public DTO_ccLineaComponente(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccLineaComponente(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID LineaCreditoID { get; set; }

        [DataMember]
        public UDT_Descriptivo LineaCreditoDesc { get; set; }

        [DataMember]
        public UDT_BasicID ComponenteCarteraID { get; set; }

        [DataMember]
        public UDT_Descriptivo ComponenteCarteraDesc { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_PorcentajeCarteraID PorcentajeID { get; set; }

        [DataMember]
        public UDT_PorcentajeID FactorCesion { get; set; }



    }

}