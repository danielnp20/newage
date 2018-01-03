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
    /// Models DTO_pyBienServicioXTarea
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pyBienServicioXTarea : DTO_MasterComplex
    {
        #region pyBienServicioXTarea
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pyBienServicioXTarea(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.ActividadFlujoDesc.Value = dr["ActividadFlujoDesc"].ToString();
                    this.CodigoBSDesc.Value = dr["CodigoBSDesc"].ToString();
                    this.ReferenciaDesc.Value = dr["ReferenciaDesc"].ToString();
                }
                this.ActividadFlujoID.Value = dr["ActividadFlujoID"].ToString();
                if (!string.IsNullOrEmpty(dr["CodigoBSID"].ToString()))
                    this.CodigoBSID.Value = Convert.ToString(dr["CodigoBSID"]);
                if (!string.IsNullOrEmpty(dr["inReferenciaID"].ToString()))
                    this.inReferenciaID.Value = Convert.ToString(dr["inReferenciaID"]);
                if (!string.IsNullOrEmpty(dr["Cantidad"].ToString()))
                    this.Cantidad.Value = Convert.ToDecimal(dr["Cantidad"]);
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
        public DTO_pyBienServicioXTarea() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ActividadFlujoID = new UDT_BasicID();
            this.ActividadFlujoDesc = new UDT_Descriptivo();
            this.CodigoBSID = new UDT_BasicID();
            this.CodigoBSDesc = new UDT_Descriptivo();
            this.inReferenciaID = new UDT_BasicID();
            this.ReferenciaDesc = new UDT_Descriptivo();
            this.Cantidad = new UDT_Cantidad();
            this.Observacion = new UDT_DescripTExt();
        }

        public DTO_pyBienServicioXTarea(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_pyBienServicioXTarea(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID ActividadFlujoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ActividadFlujoDesc { get; set; }

        [DataMember]
        public UDT_BasicID CodigoBSID { get; set; }

        [DataMember]
        public UDT_Descriptivo CodigoBSDesc { get; set; }

        [DataMember]
        public UDT_BasicID inReferenciaID { get; set; }

        [DataMember]
        public UDT_Descriptivo ReferenciaDesc { get; set; }

        [DataMember]
        public UDT_Cantidad Cantidad { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }
    }

}
