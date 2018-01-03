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
    /// Models DTO_rhEstudioxCargo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_rhEstudioxCargo : DTO_MasterComplex
    {
        #region DTO_rhEstudioxCargo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_rhEstudioxCargo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.CargoDesc.Value = dr["CargoDesc"].ToString();
                }
                
                this.CargoEmpID.Value = dr["CargoEmpID"].ToString();
                this.Codigo.Value = dr["Codigo"].ToString();
                this.Descripcion.Value = dr["Descripcion"].ToString();
                this.ObligatorioInd.Value = Convert.ToBoolean(dr["ObligatorioInd"]);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_rhEstudioxCargo() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas
            this.CargoEmpID = new UDT_BasicID();
            this.CargoDesc = new UDT_Descriptivo();
            this.Codigo = new UDTSQL_char(5);
            this.Descripcion = new UDT_DescripTExt();
            this.ObligatorioInd = new UDT_SiNo();         
        }

        public DTO_rhEstudioxCargo(DTO_MasterComplex comp) : base(comp)
        {
            InitCols();
        }

        public DTO_rhEstudioxCargo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_BasicID CargoEmpID { get; set; }

        [DataMember]
        public UDT_Descriptivo CargoDesc { get; set; }

        [DataMember]
        public UDTSQL_char Codigo { get; set; }

        [DataMember]
        public UDT_DescripTExt Descripcion { get; set; }

        [DataMember]
        public UDT_SiNo ObligatorioInd { get; set; }   
    }
}
