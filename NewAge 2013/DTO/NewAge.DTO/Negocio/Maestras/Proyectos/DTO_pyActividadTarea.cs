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
    /// Models DTO_pyActividadTarea
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pyActividadTarea : DTO_MasterComplex
    {
        #region DTO_pyActividadTarea
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pyActividadTarea(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.ActividadDesc.Value = dr["ActividadDesc"].ToString();
                    this.ActividadEtapaDesc.Value = dr["ActividadEtapaDesc"].ToString();
                    this.ActividadFlujoDesc.Value = dr["ActividadFlujoDesc"].ToString();
                    this.ServicioDesc.Value = dr["ServicioDesc"].ToString();
                }
                this.ActividadID.Value = Convert.ToString(dr["ActividadID"]);
                this.ActividadEtapaID.Value = dr["ActividadEtapaID"].ToString();
                this.ActividadFlujoID.Value = dr["ActividadFlujoID"].ToString();
                if (!string.IsNullOrEmpty(dr["Observacion"].ToString()))
                    this.Observacion.Value = dr["Observacion"].ToString();
                if (!string.IsNullOrEmpty(dr["ServicioID"].ToString()))
                    this.ServicioID.Value = dr["ServicioID"].ToString();
                if (!string.IsNullOrEmpty(dr["SemanaPrograma"].ToString()))
                    this.SemanaPrograma.Value = Convert.ToInt32(dr["SemanaPrograma"]);
            }
            catch (Exception e)
            {
                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_pyActividadTarea() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ActividadID = new UDT_BasicID();
            this.ActividadDesc = new UDT_Descriptivo();
            this.ActividadEtapaID = new UDT_BasicID();
            this.ActividadEtapaDesc = new UDT_Descriptivo();
            this.ActividadFlujoID = new UDT_BasicID();
            this.ActividadFlujoDesc = new UDT_Descriptivo();
            this.Observacion = new UDT_DescripTExt();
            this.ServicioID = new UDT_BasicID();
            this.ServicioDesc = new UDT_Descriptivo();
            this.SemanaPrograma = new UDTSQL_int();
        }

        public DTO_pyActividadTarea(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_pyActividadTarea(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID ActividadID { get; set; }

        [DataMember]
        public UDT_Descriptivo ActividadDesc { get; set; }

        [DataMember]
        public UDT_BasicID ActividadEtapaID { get; set; }

        [DataMember]
        public UDT_Descriptivo ActividadEtapaDesc { get; set; }

        [DataMember]
        public UDT_BasicID ActividadFlujoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ActividadFlujoDesc { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        public UDT_BasicID ServicioID { get; set; }

        [DataMember]
        public UDT_Descriptivo ServicioDesc { get; set; }

        [DataMember]
        public UDTSQL_int SemanaPrograma { get; set; }
    }

}
