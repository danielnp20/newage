using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;

namespace NewAge.DTO.Reportes
{
    /// <summary>
    /// Class Error:
    /// Models DTO_aplBitacora
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_repBitacora : DTO_BasicReport
    {
        #region Contructora

        /// <summary>
        /// Constructor
        /// </summary>
        public DTO_repBitacora(IDataReader dr)
        {
            this.BitacoraID = Convert.ToInt32(dr["BitacoraID"]);
            this.EmpresaID = Convert.ToString(dr["EmpresaID"]);
            this.DocumentoID = Convert.ToInt32(dr["DocumentoID"]);
            this.AccionID = Convert.ToInt16(dr["AccionID"]);
            this.Fecha = Convert.ToDateTime(dr["Fecha"]);
            this.seUsuarioID = Convert.ToInt32(dr["seUsuarioID"]);
            this.llp01 = dr["llp01"].ToString();
            this.llp02 = dr["llp02"].ToString();
            this.llp03 = dr["llp03"].ToString();
            this.llp04 = dr["llp04"].ToString();
            this.BitacoraOrigenID = Convert.ToInt32(dr["BitacoraOrigenID"]);
            this.BitacoraPadreID = Convert.ToInt32(dr["BitacoraPadreID"]);
            this.BitacoraAnulacionID = Convert.ToInt32(dr["BitacoraAnulacionID"]);
        }
        #endregion

        #region Propiedades

        /// <summary>
        /// Gets or sets the BitacoraID
        /// </summary>
        [DataMember]
        public int BitacoraID { get; set; }

        /// <summary>
        /// Gets or sets the EmpresaID
        /// </summary>
        [DataMember]
        public string EmpresaID { get; set; }

        /// <summary>
        /// Gets or sets the DocumentoID
        /// </summary>
        [DataMember]
        public int DocumentoID { get; set; }

        /// <summary>
        /// Gets or sets the AccionID
        /// </summary>
        [DataMember]
        public short AccionID { get; set; }

        /// <summary>
        /// Gets or sets the Fecha
        /// </summary>
        [DataMember]
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Gets or sets the seUsuarioID
        /// </summary>
        [DataMember]
        public int seUsuarioID { get; set; }

        /// <summary>
        /// Gets or sets the llp01
        /// </summary>
        [DataMember]
        public string llp01 { get; set; }

        /// <summary>
        /// Gets or sets the llp02
        /// </summary>
        [DataMember]
        public string llp02 { get; set; }

        /// <summary>
        /// Gets or sets the llp03
        /// </summary>
        [DataMember]
        public string llp03 { get; set; }

        /// <summary>
        /// Gets or sets the llp04
        /// </summary>
        [DataMember]
        public string llp04 { get; set; }

        /// <summary>
        /// Gets or sets the BitacoraOrigenID
        /// </summary>
        [DataMember]
        public int BitacoraOrigenID { get; set; }

        /// <summary>
        /// Gets or sets the BitacoraPadreID
        /// </summary>
        [DataMember]
        public int BitacoraPadreID { get; set; }

        /// <summary>
        /// Gets or sets the BitacoraAnulacionID
        /// </summary>
        [DataMember]
        //public Nullable<UDT_BitacoraID> BitacoraAnulacionID
        public int BitacoraAnulacionID { get; set; }

        #endregion
    }
}
