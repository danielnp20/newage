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
    /// Models DTO_pyTarea
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pyTarea : DTO_MasterBasic
    {
        #region DTO_pyTarea
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pyTarea(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if(!isReplica)
                {
                    this.ServicioDesc.Value = dr["ServicioDesc"].ToString();
                    this.UnidadInvDesc.Value = Convert.ToString(dr["UnidadInvDesc"]);
                    this.CapituloTareaDesc.Value = Convert.ToString(dr["CapituloTareaDesc"]);
                    this.LineaPresupuestoDesc.Value = Convert.ToString(dr["LineaPresupuestoDesc"]);
                }
                   
                if (!string.IsNullOrEmpty(dr["ServicioID"].ToString()))
                    this.ServicioID.Value = dr["ServicioID"].ToString();
                if (!string.IsNullOrEmpty(dr["Observacion"].ToString()))
                    this.Observacion.Value = dr["Observacion"].ToString();
                if (!string.IsNullOrEmpty(dr["TipoTarea"].ToString()))
                    this.TipoTarea.Value = Convert.ToByte(dr["TipoTarea"]);
                this.UnidadInvID.Value = Convert.ToString(dr["UnidadInvID"]);     
                this.CapituloTareaID.Value = Convert.ToString(dr["CapituloTareaID"]);     
                this.LineaPresupuestoID.Value = Convert.ToString(dr["LineaPresupuestoID"]);
                if (!string.IsNullOrEmpty(dr["EntregaIndividualInd"].ToString()))
                    this.EntregaIndividualInd.Value = Convert.ToBoolean(dr["EntregaIndividualInd"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>g
        public DTO_pyTarea()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ServicioID = new UDT_BasicID();
            this.ServicioDesc = new UDT_Descriptivo();
            this.Observacion = new UDT_DescripTExt();
            this.TipoTarea = new UDTSQL_tinyint();
            this.UnidadInvID = new UDT_BasicID();
            this.UnidadInvDesc = new UDT_Descriptivo();
            this.CapituloTareaID = new UDT_BasicID();
            this.CapituloTareaDesc = new UDT_Descriptivo();
            this.LineaPresupuestoID = new UDT_BasicID();
            this.LineaPresupuestoDesc = new UDT_Descriptivo();
            this.EntregaIndividualInd = new UDT_SiNo();
        }

        public DTO_pyTarea(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_pyTarea(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID ServicioID { get; set; }

        [DataMember]
        public UDT_Descriptivo ServicioDesc { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoTarea { get; set; }

        [DataMember]
        public UDT_BasicID UnidadInvID { get; set; }

        [DataMember]
        public UDT_Descriptivo UnidadInvDesc { get; set; }

        [DataMember]
        public UDT_BasicID CapituloTareaID { get; set; }

        [DataMember]
        public UDT_Descriptivo CapituloTareaDesc { get; set; }

        [DataMember]
        public UDT_BasicID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_Descriptivo LineaPresupuestoDesc { get; set; }

        [DataMember]
        public UDT_SiNo EntregaIndividualInd { get; set; }


    }

}
