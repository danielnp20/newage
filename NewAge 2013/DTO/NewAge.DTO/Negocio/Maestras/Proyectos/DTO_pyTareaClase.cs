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
    /// Models DTO_pyTareaClase
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pyTareaClase : DTO_MasterComplex
    {
        #region pyTrabajoXTarea
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pyTareaClase(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.TareaDesc.Value = dr["TareaDesc"].ToString();
                    this.ClaseServicioDesc.Value = dr["ClaseServicioDesc"].ToString();
                }
                if (!string.IsNullOrEmpty(dr["ClaseServicioID"].ToString()))
                    this.ClaseServicioID.Value = Convert.ToString(dr["ClaseServicioID"]);
                if (!string.IsNullOrEmpty(dr["TareaID"].ToString()))
                    this.TareaID.Value = Convert.ToString(dr["TareaID"]);
                if (!string.IsNullOrEmpty(dr["Observacion"].ToString()))
                    this.Observacion.Value = Convert.ToString(dr["Observacion"]);
                
            }
            catch (Exception e)
            {
                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_pyTareaClase() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //this.ActividadFlujoID = new UDT_BasicID();
            //this.ActividadFlujoDesc = new UDT_Descriptivo();
            this.ClaseServicioID = new UDT_BasicID();
            this.ClaseServicioDesc = new UDT_Descriptivo();        
            this.TareaID = new UDT_BasicID();
            this.TareaDesc = new UDT_Descriptivo();
            this.Observacion = new UDT_DescripTExt();
        }

        public DTO_pyTareaClase(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_pyTareaClase(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID ClaseServicioID { get; set; }

        [DataMember]
        public UDT_Descriptivo ClaseServicioDesc { get; set; }
        
        [DataMember]
        public UDT_BasicID TareaID { get; set; }

        [DataMember]
        public UDT_Descriptivo TareaDesc { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }
    }

}