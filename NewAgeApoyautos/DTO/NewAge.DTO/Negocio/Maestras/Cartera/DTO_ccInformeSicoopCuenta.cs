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
    /// Models DTO_ccInformeSicoopCuenta
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccInformeSicoopCuenta : DTO_MasterComplex
    {
        #region ccInformeSicoopCuenta
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccInformeSicoopCuenta(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.CuentaDesc.Value = Convert.ToString(dr["ClienteDesc"].ToString());
                }
                this.Formato.Value = Convert.ToString(dr["Formato"].ToString());
                this.CuentaID.Value = Convert.ToString(dr["CuentaID"].ToString());
                
            }
            catch (Exception e)
            {
                
                throw e;
            }
        }
        
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccInformeSicoopCuenta() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Formato = new UDT_CodigoGrl5();
            this.CuentaID = new UDT_BasicID();
            this.CuentaDesc = new UDT_Descriptivo();
        }

        public DTO_ccInformeSicoopCuenta(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccInformeSicoopCuenta(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
        
        [DataMember]
        public UDT_CodigoGrl5 Formato { get; set; }

        [DataMember]
        public UDT_BasicID CuentaID { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaDesc { get; set; }

        

    }

}
