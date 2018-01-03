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
    /// Models DTO_coCompDistribuyeTabla
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coCompDistribuyeTabla
    {
        #region DTO_coCompDistribuyeTabla

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coCompDistribuyeTabla(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
                this.Orden.Value = Convert.ToByte(dr["Orden"]);
                if (!string.IsNullOrWhiteSpace(dr["PorcentajeID"].ToString()))
                    this.PorcentajeID.Value = Convert.ToDecimal(dr["PorcentajeID"]);
                this.CuentaORIG.Value = dr["CuentaORIG"].ToString();
                this.CtoCostoORIG.Value = dr["CtoCostoORIG"].ToString();
                this.ProyectoORIG.Value = dr["ProyectoORIG"].ToString();
                this.CuentaCONT.Value = dr["CuentaCONT"].ToString();
                this.CtoCostoCONT.Value = dr["CtoCostoCONT"].ToString();
                this.ProyectoCONT.Value = dr["ProyectoCONT"].ToString();
                this.CuentaDEST.Value = dr["CuentaDEST"].ToString();
                this.CtoCostoDEST.Value = dr["CtoCostoDEST"].ToString();
                this.ProyectoDEST.Value = dr["ProyectoDEST"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coCompDistribuyeTabla()
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
            this.CuentaORIG = new UDT_CuentaID();
            this.CtoCostoORIG = new UDT_CentroCostoID();
            this.ProyectoORIG = new UDT_ProyectoID();
            this.Orden = new UDTSQL_tinyint();
            this.CuentaCONT = new UDT_CuentaID();
            this.CtoCostoCONT = new UDT_CentroCostoID();
            this.ProyectoCONT = new UDT_ProyectoID();
            this.CuentaDEST = new UDT_CuentaID();
            this.CtoCostoDEST = new UDT_CentroCostoID();
            this.ProyectoDEST = new UDT_ProyectoID();
            this.PorcentajeID = new UDT_PorcentajeID();
            this.Index = new UDT_Consecutivo();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        [DataMember]
        public UDT_CuentaID CuentaORIG { get; set; }

        [DataMember]
        public UDT_CentroCostoID CtoCostoORIG { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoORIG { get; set; }

        [DataMember]
        public UDTSQL_tinyint Orden { get; set; }

        [DataMember]
        public UDT_CuentaID CuentaCONT { get; set; }

        [DataMember]
        public UDT_CentroCostoID CtoCostoCONT { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoCONT { get; set; }

        [DataMember]
        public UDT_CuentaID CuentaDEST { get; set; }

        [DataMember]
        public UDT_CentroCostoID CtoCostoDEST { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoDEST { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorcentajeID { get; set; }

        [DataMember]
        public UDT_Consecutivo Index { get; set; }

        #endregion
    }
}
