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
    /// Models DTO_prProveedorPrecio
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prProveedorPrecio : DTO_MasterComplex
    {
        #region DTO_prProveedorProductoTipo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_prProveedorPrecio(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.ProveedorDesc.Value = dr["ProveedorDesc"].ToString();
                    this.inReferenciaDesc.Value = dr["inReferenciaDesc"].ToString();
                }
                this.ProveedorID.Value = dr["ProveedorID"].ToString();
                this.inReferenciaID.Value = dr["inReferenciaID"].ToString();
                if (!string.IsNullOrEmpty(dr["ValorLocal"].ToString()))
                    this.ValorLocal.Value = Convert.ToDecimal(dr["ValorLocal"]);
                if (!string.IsNullOrEmpty(dr["ValorExtra"].ToString()))
                    this.ValorExtra.Value = Convert.ToDecimal(dr["ValorExtra"]);
            }
            catch (Exception e)
            {
              throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_prProveedorPrecio() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ProveedorID = new UDT_BasicID();
            this.ProveedorDesc = new UDT_Descriptivo();
            this.inReferenciaID = new UDT_BasicID();
            this.inReferenciaDesc = new UDT_Descriptivo();
            this.ValorLocal = new UDT_Valor();
            this.ValorExtra = new UDT_Valor();
        }

        public DTO_prProveedorPrecio(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_prProveedorPrecio(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID ProveedorID { get; set; }

        [DataMember]
        public UDT_Descriptivo ProveedorDesc { get; set; }

        [DataMember]
        public UDT_BasicID inReferenciaID { get; set; }

        [DataMember]
        public UDT_Descriptivo inReferenciaDesc { get; set; }        

        [DataMember]
        public UDT_Valor ValorLocal { get; set; }

        [DataMember]
        public UDT_Valor ValorExtra { get; set; }
    }
}
