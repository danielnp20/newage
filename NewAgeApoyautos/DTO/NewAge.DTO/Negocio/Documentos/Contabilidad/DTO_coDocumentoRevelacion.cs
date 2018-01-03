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
    /// Class:
    /// Models DTO_coDocumentoRevelacion
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coDocumentoRevelacion
    {
        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coDocumentoRevelacion()
        {
            InitCols();        
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coDocumentoRevelacion(IDataReader dr)
        {
            InitCols();
            this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
            this.EmpresaID.Value = dr["EmpresaID"].ToString(); ;
            this.NotaRevelacionID.Value = dr["NotaRevelacionID"].ToString();
            this.TituloRevelacion.Value = dr["TituloRevelacion"].ToString();
            this.Revelacion.Value = dr["Revelacion"].ToString();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.EmpresaID = new UDT_EmpresaID();
            this.NotaRevelacionID = new UDT_NotaRevelacionID();
            this.TituloRevelacion = new UDT_DescripTBase();
            this.Revelacion = new UDT_DescripTExt();
        }
	    #endregion
 
        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_NotaRevelacionID NotaRevelacionID { get; set; }

        [DataMember]
        public UDT_DescripTBase TituloRevelacion { get; set; }

        [DataMember]
        public UDT_DescripTExt Revelacion { get; set; }     

    }
}
