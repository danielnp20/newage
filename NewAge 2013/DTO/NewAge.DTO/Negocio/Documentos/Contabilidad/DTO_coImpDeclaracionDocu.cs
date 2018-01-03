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
    /// Models DTO_coImpDeclaracionDocu
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coImpDeclaracionDocu
    {
        #region DTO_coImpDeclaracionDocu

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coImpDeclaracionDocu(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.ImpuestoDeclID.Value = dr["ImpuestoDeclID"].ToString();
                this.Año.Value = Convert.ToInt32(dr["Año"]);
                this.Periodo.Value = Convert.ToByte(dr["Periodo"]);
                this.eg_coImpuestoDeclaracion.Value = dr["eg_coImpuestoDeclaracion"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coImpDeclaracionDocu()
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
            this.ImpuestoDeclID = new UDT_ImpuestoDeclID();
            this.Año = new UDTSQL_int();
            this.Periodo = new UDTSQL_tinyint();
            this.eg_coImpuestoDeclaracion = new UDT_EmpresaGrupoID();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_ImpuestoDeclID ImpuestoDeclID { get; set; }

        [DataMember]
        public UDTSQL_int Año { get; set; }

        [DataMember]
        public UDTSQL_tinyint Periodo { get; set; }

        [DataMember]
        public UDT_EmpresaGrupoID eg_coImpuestoDeclaracion { get; set; }

        #endregion

    }
}
