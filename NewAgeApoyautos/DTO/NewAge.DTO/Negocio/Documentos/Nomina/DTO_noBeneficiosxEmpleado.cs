using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_noBeneficiosxEmpleado
    {
        #region Constructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noBeneficiosxEmpleado(IDataReader dr)
        {
            this.InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.EmpleadoID.Value = dr["EmpleadoID"].ToString();
                this.ContratoNOID.Value = Convert.ToInt32(dr["ContratoNOID"].ToString());
                this.CompFlexibleID.Value = dr["CompFlexibleID"].ToString();
                this.Valor.Value = Convert.ToDecimal(dr["Valor"].ToString());
                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.ActivaInd.Value = dr["ActivaInd"].ToString() == "1" ? true : false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noBeneficiosxEmpleado()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.EmpleadoID = new UDT_EmpleadoID();
            this.ContratoNOID = new UDT_ContratoNOID();
            this.CompFlexibleID = new UDT_CompFlexibleID();
            this.Valor = new UDT_Valor();
            this.TerceroID = new UDT_TerceroID();
            this.ActivaInd = new UDT_SiNo();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_EmpleadoID EmpleadoID { get; set; }

        [DataMember]
        public UDT_ContratoNOID ContratoNOID { get; set; }
   
        [DataMember]
        public UDT_CompFlexibleID CompFlexibleID { get; set; }
        
        [DataMember]
        public UDT_Valor Valor { get; set; }
        
        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }
        
        [DataMember]
        public UDT_SiNo ActivaInd { get; set; }


        #endregion
    }
}
