using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_cpTarjetaPagos
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_cpTarjetaPagos
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_cpTarjetaPagos(IDataReader dr)
        {
            this.InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.CargoEspecialID.Value = dr["CargoEspecialID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["Valor"].ToString()))
                    this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_cpTarjetaPagos()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.CargoEspecialID = new UDT_CargoEspecialID();
            this.Descriptivo = new UDT_DescripTBase();
            this.Valor = new UDT_Valor();
        }
        #endregion

        [DataMember]
        [NotImportable]
        public int Index { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_CargoEspecialID CargoEspecialID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTBase Descriptivo { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor Valor { get; set; }
         



    }
}
