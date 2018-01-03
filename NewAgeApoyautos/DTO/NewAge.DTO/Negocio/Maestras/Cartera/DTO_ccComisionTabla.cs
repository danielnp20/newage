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
    /// Models DTO_ccComisionTabla
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccComisionTabla : DTO_MasterComplex
    {
        #region DTO_ccComisionTabla
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccComisionTabla(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            #region Asignar Valores
            InitCols();
            try
            {
                this.LineaCreditoID.Value = dr["LineaCreditoID"].ToString();
                if (!string.IsNullOrEmpty(dr["Valor"].ToString()))
                    this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                if (!string.IsNullOrEmpty(dr["PorcentajeID"].ToString()))
                this.PorcentajeID.Value = Convert.ToDecimal(dr["PorcentajeID"]);

                    if (!isReplica) // Solo Descriptivo, para validar campos que no existen en la base de datos
                        this.LineaCreditoDesc.Value = dr["LineaCreditoDesc"].ToString();
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
            this.LineaCreditoID = new UDT_BasicID();
            this.LineaCreditoDesc = new UDT_Descriptivo();
            this.Valor = new UDT_Valor();
            this.PorcentajeID = new UDT_PorcentajeCarteraID();
        } 
        #endregion

        #region Constructor
        public DTO_ccComisionTabla(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccComisionTabla()
            : base()
        {
            InitCols();
        }

        public DTO_ccComisionTabla(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }   
        #endregion

        #region Propiedades

        [DataMember]
        public UDT_BasicID LineaCreditoID { get; set; }

        [DataMember]
        public UDT_Descriptivo LineaCreditoDesc { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_PorcentajeCarteraID PorcentajeID { get; set; }

        #endregion
        #endregion 


    }

}
