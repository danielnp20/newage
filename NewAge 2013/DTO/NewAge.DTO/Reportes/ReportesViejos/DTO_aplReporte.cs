using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;
using System.Data;
using NewAge.Librerias.Project;

namespace NewAge.DTO.Reportes
{
    /// <summary>
    /// Clase que tiene un objeto de reportes 
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_aplReporte
    {

        #region Constructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_aplReporte(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.DocumentoID.Value = Convert.ToInt32(dr["DocumentoID"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_aplReporte()
        {
            this.InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.DocumentoID = new UDT_Consecutivo();
            //this.Data = new object();
        }

        #endregion

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_Consecutivo DocumentoID { get; set; }

        [DataMember]
        public byte[] Data { get; set; }

    }
}
