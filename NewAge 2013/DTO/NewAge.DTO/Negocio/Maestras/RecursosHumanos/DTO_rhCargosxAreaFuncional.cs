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
    /// Models DTO_rhCargosxAreaFuncional
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_rhCargosxAreaFuncional : DTO_MasterComplex
    {
        #region DTO_rhCargosxAreaFuncional
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_rhCargosxAreaFuncional(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.AreaFuncionalDesc.Value = dr["AreaFuncionalDesc"].ToString();
                    this.CargoDesc.Value = dr["CargoDesc"].ToString();
                    this.CargoSuperiorDesc.Value = dr["CargoSuperiorDesc"].ToString();
                    this.HorarioDesc.Value = dr["HorarioDesc"].ToString();
                }

                this.AreaFuncionalID.Value = dr["AreaFuncionalID"].ToString();
                this.CargoEmpID.Value = dr["CargoEmpID"].ToString();
                this.CargoSuperior.Value = dr["CargoSuperior"].ToString();
                this.Cantidad.Value = Convert.ToInt16(dr["Cantidad"]);
                this.CantVacantes.Value = Convert.ToInt16(dr["CantVacantes"]);
                this.HorarioID.Value = dr["HorarioID"].ToString();
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_rhCargosxAreaFuncional() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas
            this.AreaFuncionalID = new UDT_BasicID();
            this.AreaFuncionalDesc = new UDT_Descriptivo();
            this.CargoEmpID = new UDT_BasicID();
            this.CargoDesc = new UDT_Descriptivo();
            this.CargoSuperior = new UDT_BasicID();
            this.CargoSuperiorDesc = new UDT_Descriptivo();
            this.Cantidad = new UDTSQL_smallint();
            this.CantVacantes = new UDTSQL_smallint();
            this.HorarioID = new UDT_BasicID();
            this.HorarioDesc = new UDT_Descriptivo();                     
        }

        public DTO_rhCargosxAreaFuncional(DTO_MasterComplex comp) : base(comp)
        {
            InitCols();
        }

        public DTO_rhCargosxAreaFuncional(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
  
        [DataMember]
        public UDT_BasicID AreaFuncionalID { get; set; }

        [DataMember]
        public UDT_Descriptivo AreaFuncionalDesc { get; set; }

        [DataMember]
        public UDT_BasicID CargoEmpID { get; set; }

        [DataMember]
        public UDT_Descriptivo CargoDesc { get; set; }

        [DataMember]
        public UDT_BasicID CargoSuperior { get; set; }

        [DataMember]
        public UDT_Descriptivo CargoSuperiorDesc { get; set; }

        [DataMember]
        public UDTSQL_smallint Cantidad { get; set; }

        [DataMember]
        public UDTSQL_smallint CantVacantes { get; set; }

        [DataMember]
        public UDT_BasicID HorarioID { get; set; }

        [DataMember]
        public UDT_Descriptivo HorarioDesc { get; set; }
             
        
    }
}
