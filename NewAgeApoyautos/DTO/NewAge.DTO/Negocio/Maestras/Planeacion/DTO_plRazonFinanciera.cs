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
    /// Models DTO_plRazonFinanciera
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_plRazonFinanciera : DTO_MasterComplex
    {
        #region DTO_plRazonFinanciera
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_plRazonFinanciera(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.indFinacieroDesc.Value = dr["indFinacieroDesc"].ToString();
                }

                this.indFinaciero.Value = dr["indFinaciero"].ToString();
                this.GrupoCuenta.Value = dr["GrupoCuenta"].ToString();
                this.IndDenominador.Value = Convert.ToBoolean(dr["IndDenominador"]);
                this.IndSigno.Value = Convert.ToBoolean(dr["IndSigno"]);
                this.IndAntiguedad.Value = Convert.ToBoolean(dr["IndAntiguedad"]);
            }
            catch (Exception e)
            {               
                throw e;
            }            
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>g
        public DTO_plRazonFinanciera() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.indFinaciero = new UDT_BasicID();
            this.indFinacieroDesc = new UDT_Descriptivo();
            this.GrupoCuenta = new UDT_DescripTBase();
            this.IndDenominador = new UDT_SiNo();
            this.IndSigno = new UDT_SiNo();
            this.IndAntiguedad = new UDT_SiNo();
        }

        public DTO_plRazonFinanciera(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_plRazonFinanciera(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_BasicID indFinaciero { get; set; }

        [DataMember]
        public UDT_Descriptivo indFinacieroDesc { get; set; }

        [DataMember]
        public UDT_DescripTBase GrupoCuenta { get; set; }

        [DataMember]
        public UDT_SiNo IndDenominador { get; set; }

        [DataMember]
        public UDT_SiNo IndSigno { get; set; }

        [DataMember]
        public UDT_SiNo IndAntiguedad { get; set; }
    }

}
