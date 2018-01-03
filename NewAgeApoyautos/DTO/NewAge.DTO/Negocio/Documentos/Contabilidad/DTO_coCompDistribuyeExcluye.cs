using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Models DTO_coCompDistribuyeExcluye
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coCompDistribuyeExcluye
    {
        #region DTO_coCompDistribuyeExcluye

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coCompDistribuyeExcluye(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
                this.CuentaEXCL.Value = dr["CuentaEXCL"].ToString();
                this.CtoCostoEXCL.Value = dr["CtoCostoEXCL"].ToString();
                this.ProyectoEXCL.Value = dr["ProyectoEXCL"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coCompDistribuyeExcluye()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.Consecutivo = new UDT_Consecutivo();
            this.CuentaEXCL = new UDT_CuentaID();
            this.CtoCostoEXCL = new UDT_CentroCostoID();
            this.ProyectoEXCL = new UDT_ProyectoID();
            this.Index = new UDT_Consecutivo();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        [DataMember]
        public UDT_CuentaID CuentaEXCL { get; set; }

        [DataMember]
        public UDT_CentroCostoID CtoCostoEXCL { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoEXCL { get; set; }

        [DataMember]
        public UDT_Consecutivo Index { get; set; }

        #endregion

    }
}
