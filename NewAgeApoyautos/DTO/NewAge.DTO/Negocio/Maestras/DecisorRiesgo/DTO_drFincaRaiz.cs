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
    /// Models DTO_drFincaRaiz
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_drFincaRaiz : DTO_MasterBasic
    {
        #region DTO_drFincaRaiz
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_drFincaRaiz(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            #region Asignar Valores
            InitCols();
            try
            {
                this.InMueblesIND.Value = Convert.ToBoolean(dr["InMueblesIND"]);
                this.Hipotecas.Value = Convert.ToByte(dr["Hipotecas"]);
                this.Restricciones.Value = Convert.ToByte(dr["Restricciones"]);
                this.Factor.Value           = Convert.ToDecimal(dr["Factor"]);
            }
            catch (Exception e)
            {
                throw e;
            } 
            #endregion

        }

        #region Inicializar Variables
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.InMueblesIND = new UDT_SiNo(); 
            this.Hipotecas = new UDTSQL_tinyint();
            this.Restricciones = new UDTSQL_tinyint();           
            this.Factor = new UDT_PorcentajeID();
        } 
        #endregion

        #region Constructor
        public DTO_drFincaRaiz(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_drFincaRaiz()
            : base()
        {
            InitCols();
        }

        public DTO_drFincaRaiz(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }   
        #endregion

        #region Propiedades

        [DataMember]
        public UDT_SiNo InMueblesIND { get; set; }

        [DataMember]
        public UDTSQL_tinyint Hipotecas { get; set; }

        [DataMember]
        public UDTSQL_tinyint Restricciones { get; set; }
      
        [DataMember]
        public UDT_PorcentajeID Factor { get; set; }

        #endregion
        #endregion 


    }

}
