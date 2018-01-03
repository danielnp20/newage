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
    /// Models DTO_coCargoCosto
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coBalanceReclasifica : DTO_MasterComplex
    {
        #region DTO_coBalanceReclasifica
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coBalanceReclasifica(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.CuentaORIDesc.Value = dr["CuentaORIDesc"].ToString();
                    this.CtoCostoORIDesc.Value = dr["CtoCostoORIDesc"].ToString();
                    this.ProyectoORIDesc.Value = dr["ProyectoORIDesc"].ToString();
                    this.LineaPresupORIDesc.Value = dr["LineaPresupORIDesc"].ToString();
                    this.ConceptoCargoORIDesc.Value = dr["ConceptoCargoORIDesc"].ToString();
                    this.CuentaDESDesc.Value = dr["CuentaDESDesc"].ToString();
                    this.CtoCostoDESDesc.Value = dr["CtoCostoDESDesc"].ToString();
                    this.ProyectoDESDesc.Value = dr["ProyectoDESDesc"].ToString();
                    this.LineaPresupDESDesc.Value = dr["LineaPresupDESDesc"].ToString();
                    this.ConceptoCargoDESDesc.Value = dr["ConceptoCargoDESDesc"].ToString();
                }

                this.CuentaORI.Value = dr["CuentaORI"].ToString();
                this.CtoCostoORI.Value = dr["CtoCostoORI"].ToString();
                this.ProyectoORI.Value = dr["ProyectoORI"].ToString();
                this.LineaPresupORI.Value = dr["LineaPresupORI"].ToString();
                this.ConceptoCargoORI.Value = dr["ConceptoCargoORI"].ToString();
                this.CuentaDES.Value = dr["CuentaDES"].ToString();
                this.CtoCostoDES.Value = dr["CtoCostoDES"].ToString();
                this.ProyectoDES.Value = dr["ProyectoDES"].ToString();
                this.LineaPresupDES.Value = dr["LineaPresupDES"].ToString();
                this.ConceptoCargoDES.Value = dr["ConceptoCargoDES"].ToString();
                this.AgrupaTercero.Value = Convert.ToByte(dr["AgrupaTercero"]);
            }
            catch (Exception e)
            {
               throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coBalanceReclasifica() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.CuentaORI = new UDT_BasicID();
            this.CuentaORIDesc = new UDT_Descriptivo();
            this.CtoCostoORI = new UDT_BasicID();
            this.CtoCostoORIDesc = new UDT_Descriptivo();
            this.ProyectoORI = new UDT_BasicID();
            this.ProyectoORIDesc = new UDT_Descriptivo();
            this.LineaPresupORI = new UDT_BasicID();
            this.LineaPresupORIDesc = new UDT_Descriptivo();
            this.ConceptoCargoORI = new UDT_BasicID();
            this.ConceptoCargoORIDesc = new UDT_Descriptivo();
            this.CuentaDES = new UDT_BasicID();
            this.CuentaDESDesc = new UDT_Descriptivo();
            this.CtoCostoDES = new UDT_BasicID();
            this.CtoCostoDESDesc = new UDT_Descriptivo();
            this.ProyectoDES = new UDT_BasicID();
            this.ProyectoDESDesc = new UDT_Descriptivo();
            this.LineaPresupDES = new UDT_BasicID();
            this.LineaPresupDESDesc = new UDT_Descriptivo();
            this.ConceptoCargoDES = new UDT_BasicID();
            this.ConceptoCargoDESDesc = new UDT_Descriptivo();
            this.AgrupaTercero = new UDTSQL_tinyint();

        }

        public DTO_coBalanceReclasifica(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_coBalanceReclasifica(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID CuentaORI { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaORIDesc { get; set; }

        [DataMember]
        public UDT_BasicID CtoCostoORI { get; set; }

        [DataMember]
        public UDT_Descriptivo CtoCostoORIDesc { get; set; }

        [DataMember]
        public UDT_BasicID ProyectoORI { get; set; }

        [DataMember]
        public UDT_Descriptivo ProyectoORIDesc { get; set; }

        [DataMember]
        public UDT_BasicID LineaPresupORI { get; set; }

        [DataMember]
        public UDT_Descriptivo LineaPresupORIDesc { get; set; }

        [DataMember]
        public UDT_BasicID ConceptoCargoORI { get; set; }

        [DataMember]
        public UDT_Descriptivo ConceptoCargoORIDesc { get; set; }

        [DataMember]
        public UDT_BasicID CuentaDES { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaDESDesc { get; set; }

        [DataMember]
        public UDT_BasicID CtoCostoDES { get; set; }

        [DataMember]
        public UDT_Descriptivo CtoCostoDESDesc { get; set; }

        [DataMember]
        public UDT_BasicID ProyectoDES { get; set; }

        [DataMember]
        public UDT_Descriptivo ProyectoDESDesc { get; set; }

        [DataMember]
        public UDT_BasicID LineaPresupDES { get; set; }

        [DataMember]
        public UDT_Descriptivo LineaPresupDESDesc { get; set; }

        [DataMember]
        public UDT_BasicID ConceptoCargoDES { get; set; }

        [DataMember]
        public UDT_Descriptivo ConceptoCargoDESDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint AgrupaTercero { get; set; }
    }

}
