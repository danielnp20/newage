using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class DTO_MasterBasic:
    /// Models del generic basic master DTO
    /// </summary>
    [DataContract]
    [Serializable]
    [KnownType(typeof(DTO_coCentroCosto))]
    [KnownType(typeof(DTO_coConceptoCargo))]
    [KnownType(typeof(DTO_coPlanCuenta))]
    [KnownType(typeof(DTO_coProyecto))]
    [KnownType(typeof(DTO_faServicios))]
    [KnownType(typeof(DTO_glAreaFuncional))]
    [KnownType(typeof(DTO_glLocFisica))]
    [KnownType(typeof(DTO_glLugarGeografico))]
    [KnownType(typeof(DTO_inRefClase))]
    [KnownType(typeof(DTO_pyTarea))]
    [KnownType(typeof(DTO_pyTrabajo))]
   
    public class DTO_MasterHierarchyBasic : DTO_MasterBasic
    {
        #region Constructor
        /// <summary>
        /// Builds a DTO from a datareader
        /// </summary>
        /// <param name="?"></param>
        public DTO_MasterHierarchyBasic(IDataReader dr, DTO_aplMaestraPropiedades props)
            : base(dr, props)
        {
            this.MovInd.Value = Convert.ToBoolean(dr["MovInd"]);
        }

        /// <summary>
        /// Builds a DTO from a data
        /// </summary>
        /// <param name="?"></param>
        public DTO_MasterHierarchyBasic(DTO_MasterBasic data, DTO_aplMaestraPropiedades props)
            : base(props)
        {
            base.ID = data.ID;
            base.Descriptivo = data.Descriptivo;
            base.ActivoInd = data.ActivoInd;
            base.CtrlVersion = data.CtrlVersion;
            base.EmpresaGrupoID = data.EmpresaGrupoID;
            base.ReplicaID = data.ReplicaID;
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_MasterHierarchyBasic() : base() {
            InitCols();
        }

        public DTO_MasterHierarchyBasic(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_MasterHierarchyBasic(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        } 

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            Jerarquia = new DTO_hierarchy();
            this.MovInd = new UDT_SiNo();
        }

        protected override void InitCols(DTO_aplMaestraPropiedades props)
        {
            base.InitCols(props);
            this.MovInd = new UDT_SiNo();
        }

        #endregion 

        /// <summary>
        /// Jerarquia relacionada con el dto
        /// </summary>
        [DataMember]
        public DTO_hierarchy Jerarquia{ get; set; }

        /// <summary>
        /// indicador de hoja de la jerarquia
        /// </summary>
        [DataMember]
        public UDT_SiNo MovInd { get; set; }
    }
}
