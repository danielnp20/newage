using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;
using System.Data;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Models DTO_coImpDeclaracionDetaCuenta
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coImpDeclaracionDetaCuenta
    {
        #region DTO_coImpDeclaracionDetaCuenta

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coImpDeclaracionDetaCuenta(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.Renglon.Value = dr["Renglon"].ToString();
                this.CuentaID.Value = dr["CuentaID"].ToString();
                this.VlrBaseML.Value = Convert.ToDecimal(dr["VlrBaseML"]);
                this.ValorML.Value = Convert.ToDecimal(dr["ValorML"]);
                this.eg_coPlanCuenta.Value = dr["eg_coPlanCuenta"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coImpDeclaracionDetaCuenta()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.Renglon = new UDT_Renglon();
            this.CuentaID = new UDT_CuentaID();
            this.VlrBaseML = new UDT_Valor();
            this.ValorML = new UDT_Valor();
            this.ValorME = new UDT_Valor();
            this.eg_coPlanCuenta = new UDT_EmpresaGrupoID();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_Renglon Renglon { get; set; }

        [DataMember]
        public UDT_CuentaID CuentaID { get; set; }

        [DataMember]
        public UDT_Valor VlrBaseML { get; set; }

        [DataMember]
        public UDT_Valor ValorML { get; set; }

        [DataMember]
        public UDT_Valor ValorME { get; set; }

        [DataMember]
        public UDT_EmpresaGrupoID eg_coPlanCuenta { get; set; }

        #endregion

    }
}
