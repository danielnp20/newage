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
    /// Models DTO_pyTrabajo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pyTrabajo : DTO_MasterBasic
    {
        #region pyTrabajo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pyTrabajo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["Observacion"].ToString()))
                    this.Observacion.Value = dr["Observacion"].ToString();
                if (!isReplica)
                {
                    this.CapituloTareaDesc.Value = Convert.ToString(dr["CapituloTareaDesc"]);
                    this.UnidadInvDesc.Value = Convert.ToString(dr["UnidadInvDesc"]);
                }
                this.CapituloTareaID.Value = Convert.ToString(dr["CapituloTareaID"]);                  
                this.UnidadInvID.Value = Convert.ToString(dr["UnidadInvID"]);
                if (!string.IsNullOrEmpty(dr["TipoConstrudata"].ToString()))
                   this.TipoConstrudata.Value = Convert.ToByte(dr["TipoConstrudata"]);
                this.Formula.Value = Convert.ToString(dr["Formula"]);
            }
            catch (Exception e)
            {
                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_pyTrabajo() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Observacion = new UDT_DescripTExt();
            this.CapituloTareaID = new UDT_BasicID();
            this.CapituloTareaDesc = new UDT_Descriptivo();

            this.UnidadInvID = new UDT_BasicID();
            this.UnidadInvDesc = new UDT_Descriptivo();
            this.TipoConstrudata = new UDTSQL_tinyint();
            this.Formula = new UDTSQL_char(50);
            this.Cantidad = new UDT_Cantidad();
            this.CostoBase = new UDT_Valor();
        }

        public DTO_pyTrabajo(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_pyTrabajo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
   
        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }     

        [DataMember]
        public UDT_BasicID CapituloTareaID { get; set; }

        [DataMember]
        public UDT_Descriptivo CapituloTareaDesc { get; set; }

        [DataMember]
        public UDT_BasicID UnidadInvID { get; set; }

        [DataMember]
        public UDT_Descriptivo UnidadInvDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoConstrudata { get; set; }

        [DataMember]
        public UDTSQL_char Formula { get; set; }

        [DataMember]
        public UDT_Cantidad Cantidad { get; set; }

        [DataMember]
        public UDT_Valor CostoBase { get; set; }
    }

}
