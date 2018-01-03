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
    /// Models DTO_coImpDeclaracionCuenta
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coImpDeclaracionCuenta : DTO_MasterComplex
    {
        #region DTO_coImpDeclaracionCuenta
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coImpDeclaracionCuenta(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.CuentaDesc.Value = dr["CuentaDesc"].ToString();
                    this.ImpuestoDeclDesc.Value = dr["ImpuestoDeclDesc"].ToString();
                    this.ConceptoCargoDesc.Value = dr["ConceptoCargoDesc"].ToString();
                }

                this.ImpuestoDeclID.Value = dr["ImpuestoDeclID"].ToString();
                this.Renglon.Value = dr["Renglon"].ToString();
                this.CuentaID.Value = dr["CuentaID"].ToString();
                this.ConceptoCargoID.Value = dr["ConceptoCargoID"].ToString();
            }
            catch (Exception e)
            {
               throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coImpDeclaracionCuenta()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas
            this.ImpuestoDeclID = new UDT_BasicID();
            this.ImpuestoDeclDesc = new UDT_Descriptivo();
            this.Renglon = new UDT_Renglon();
            this.CuentaID = new UDT_BasicID();
            this.CuentaDesc = new UDT_Descriptivo();
            this.ConceptoCargoID = new UDT_BasicID();
            this.ConceptoCargoDesc = new UDT_Descriptivo();
        }

        public DTO_coImpDeclaracionCuenta(DTO_MasterComplex comp)
            : base(comp)
        {
            InitCols();
        }

        public DTO_coImpDeclaracionCuenta(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID ImpuestoDeclID { get; set; }

        [DataMember]
        public UDT_Descriptivo ImpuestoDeclDesc { get; set; }

        [DataMember]
        public UDT_Renglon Renglon { get; set; }

        [DataMember]
        public UDT_BasicID CuentaID { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaDesc { get; set; }

        [DataMember]
        public UDT_BasicID ConceptoCargoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ConceptoCargoDesc { get; set; }
    }
}
