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
    /// Models DTO_drActividadChequeoLista  
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_drActividadChequeoLista : DTO_MasterComplex
    {
        #region DTO_drActividadChequeoLista

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_drActividadChequeoLista(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                   this.ActividadDesc.Value = dr["ActividadDesc"].ToString();
                   
                this.ActividadFlujoID.Value = dr["ActividadFlujoID"].ToString();
                this.Descripcion.Value = dr["Descripcion"].ToString();
                this.EmpleadoInd.Value = Convert.ToBoolean(dr["EmpleadoInd"]);
                this.PrestServiciosInd.Value = Convert.ToBoolean(dr["PrestServiciosInd"]);
                this.TransportadorInd.Value = Convert.ToBoolean(dr["TransportadorInd"]);
                this.IndependienteInd.Value = Convert.ToBoolean(dr["IndependienteInd"]);
                this.PensionadoInd.Value = Convert.ToBoolean(dr["PensionadoInd"]);
                this.ExcluyeCodInd.Value = Convert.ToBoolean(dr["ExcluyeCodInd"]);


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
        public DTO_drActividadChequeoLista(IDataReader dr)
        {
            InitCols();
            try
            {
                this.ActividadFlujoID.Value = dr["ActividadFlujoID"].ToString();
                this.Descripcion.Value = dr["Descripcion"].ToString();
                this.EmpleadoInd.Value = Convert.ToBoolean(dr["EmpleadoInd"]);
                this.PrestServiciosInd.Value = Convert.ToBoolean(dr["PrestServiciosInd"]);
                this.TransportadorInd.Value = Convert.ToBoolean(dr["TransportadorInd"]);
                this.IndependienteInd.Value = Convert.ToBoolean(dr["IndependienteInd"]);
                this.PensionadoInd.Value = Convert.ToBoolean(dr["PensionadoInd"]);
                this.ExcluyeCodInd.Value = Convert.ToBoolean(dr["ExcluyeCodInd"]);
                this.consecutivo.Value = Convert.ToInt32(dr["ReplicaID"]);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_drActividadChequeoLista() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ActividadFlujoID = new UDT_BasicID();
            this.ActividadDesc = new UDT_Descriptivo();
            this.Descripcion = new UDTSQL_char(200);
            this.EmpleadoInd = new UDT_SiNo();
            this.PrestServiciosInd = new UDT_SiNo();
            this.TransportadorInd = new UDT_SiNo();
            this.IndependienteInd = new UDT_SiNo();
            this.PensionadoInd = new UDT_SiNo();
            this.ExcluyeCodInd = new UDT_SiNo();
            //Adicionales
            this.Incluye = new UDTSQL_char(20);
            this.consecutivo = new UDTSQL_int();
        }

        public DTO_drActividadChequeoLista(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_drActividadChequeoLista(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID ActividadFlujoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ActividadDesc { get; set; }

        [DataMember]
        public UDTSQL_char Descripcion { get; set; }

        [DataMember]
        public UDT_SiNo EmpleadoInd { get; set; }

        [DataMember]
        public UDT_SiNo PrestServiciosInd { get; set; }

        [DataMember]
        public UDT_SiNo TransportadorInd { get; set; }

        [DataMember]
        public UDT_SiNo IndependienteInd { get; set; }

        [DataMember]
        public UDT_SiNo PensionadoInd { get; set; }

        [DataMember]
        public UDT_SiNo ExcluyeCodInd { get; set; }
        //Adicionales        
        [DataMember]
        public UDTSQL_char Incluye { get; set; }

        [DataMember]
        public UDTSQL_int consecutivo { get; set; }



    }

}
