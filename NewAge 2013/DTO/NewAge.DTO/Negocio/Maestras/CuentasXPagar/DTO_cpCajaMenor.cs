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
    /// Models DTO_cpCajaMenor
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_cpCajaMenor : DTO_MasterBasic
    {
        #region DTO_cpCajaMenor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_cpCajaMenor(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.ResponsableDesc.Value = dr["ResponsableDesc"].ToString();
                    this.coDocumentoDesc.Value = dr["coDocumentoDesc"].ToString();
                    this.PrefijoDesc.Value = dr["PrefijoDesc"].ToString();
                    this.CuentaDesc.Value = dr["CuentaDesc"].ToString();
                    this.MonedaDesc.Value = dr["MonedaDesc"].ToString();
                    this.ProyectoDesc.Value = dr["ProyectoDesc"].ToString();
                    this.CentroCostoDesc.Value = dr["CentroCostoDesc"].ToString();
                    this.AreaFuncionalDesc.Value = dr["AreaFuncionalDesc"].ToString();
                }

                this.CuentaID.Value = dr["CuentaID"].ToString();
                this.MonedaID.Value = dr["MonedaID"].ToString();
                this.Responsable.Value = dr["Responsable"].ToString();
                this.coDocumentoID.Value = dr["coDocumentoID"].ToString();
                this.PrefijoID.Value = dr["PrefijoID"].ToString();
                this.ProyectoID.Value = dr["ProyectoID"].ToString();
                this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                this.AreaFuncionalID.Value = dr["AreaFuncionalID"].ToString();
                this.ValorFondo.Value = Convert.ToDecimal(dr["ValorFondo"]);
                this.IVACostoInd.Value = Convert.ToBoolean(dr["IVACostoInd"]);
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_cpCajaMenor() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Responsable = new UDT_BasicID();
            this.ResponsableDesc = new UDT_Descriptivo();
            this.coDocumentoID = new UDT_BasicID();
            this.coDocumentoDesc = new UDT_Descriptivo();
            this.PrefijoID = new UDT_BasicID();
            this.PrefijoDesc = new UDT_Descriptivo();
            this.CuentaID = new UDT_BasicID();
            this.CuentaDesc = new UDT_Descriptivo();
            this.MonedaID = new UDT_BasicID();
            this.MonedaDesc = new UDT_Descriptivo();
            this.ProyectoID = new UDT_BasicID();
            this.ProyectoDesc = new UDT_Descriptivo();
            this.CentroCostoID = new UDT_BasicID();
            this.CentroCostoDesc = new UDT_Descriptivo();
            this.AreaFuncionalID = new UDT_BasicID();
            this.AreaFuncionalDesc = new UDT_Descriptivo();
            this.ValorFondo = new UDT_Valor();
            this.IVACostoInd = new UDT_SiNo();
        }

        public DTO_cpCajaMenor(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_cpCajaMenor(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID Responsable { get; set; }

        [DataMember]
        public UDT_Descriptivo ResponsableDesc { get; set; }

        [DataMember]
        public UDT_BasicID PrefijoID { get; set; }

        [DataMember]
        public UDT_BasicID coDocumentoID { get; set; }

        [DataMember]
        public UDT_Descriptivo coDocumentoDesc { get; set; }

        [DataMember]
        public UDT_Descriptivo PrefijoDesc { get; set; }

        [DataMember]
        public UDT_BasicID CuentaID { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaDesc { get; set; }

        [DataMember]
        public UDT_BasicID MonedaID { get; set; }

        [DataMember]
        public UDT_Descriptivo MonedaDesc { get; set; }

        [DataMember]
        public UDT_BasicID ProyectoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ProyectoDesc { get; set; }

        [DataMember]
        public UDT_BasicID CentroCostoID { get; set; }

        [DataMember]
        public UDT_Descriptivo CentroCostoDesc { get; set; }

        [DataMember]
        public UDT_BasicID AreaFuncionalID { get; set; }

        [DataMember]
        public UDT_Descriptivo AreaFuncionalDesc { get; set; }

        [DataMember]
        public UDT_Valor ValorFondo { get; set; }

        [DataMember]
        public UDT_SiNo IVACostoInd { get; set; }

    }
}
