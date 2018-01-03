using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;
using NewAge.Librerias.Project;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Clase DTO_glActividadControl
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glActividadControl
    {
        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glActividadControl()
        {
            this.InitCols();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DTO_glActividadControl(IDataReader dr)
        {
            this.InitCols();
            this.EmpresaID.Value = Convert.ToString(dr["EmpresaID"]);
            this.DocumentoID.Value = Convert.ToInt32(dr["DocumentoID"]);
            if (!string.IsNullOrWhiteSpace(dr["NumeroDoc"].ToString()))
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
            this.ActividadFlujoID.Value = dr["ActividadFlujoID"].ToString();
            this.Descriptivo.Value = dr["Descriptivo"].ToString();    
            this.UsuarioID.Value = Convert.ToInt32(dr["UsuarioID"]);
            if (!string.IsNullOrWhiteSpace(dr["CompAnula"].ToString()))
                this.CompAnula.Value = dr["CompAnula"].ToString();
            if (!string.IsNullOrWhiteSpace(dr["CompNroAnula"].ToString()))
                this.CompNroAnula.Value = Convert.ToInt32(dr["CompNroAnula"]);
            if (!string.IsNullOrWhiteSpace(dr["Fecha"].ToString()))
                this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
            this.Periodo.Value = Convert.ToDateTime(dr["Periodo"]);
            this.Observacion.Value = Convert.ToString(dr["Observacion"]);
            this.AlarmaInd.Value = Convert.ToBoolean(dr["AlarmaInd"]);
            this.UsuarioDesc.Value = dr["UsuarioDesc"].ToString(); 
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.DocumentoID = new UDT_DocumentoID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.ActividadFlujoID = new UDT_ActividadFlujoID();
            this.Descriptivo = new UDT_DescripTBase();
            this.UsuarioID = new UDT_Consecutivo();
            this.CompAnula = new UDT_ComprobanteID();
            this.CompNroAnula = new UDT_Consecutivo();
            this.Fecha = new UDTSQL_datetime();
            this.Periodo = new UDTSQL_datetime();
            this.Observacion = new UDT_DescripUnFormat();
            this.AlarmaInd = new UDT_SiNo();
            this.UsuarioDesc = new UDT_DescripTBase();
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Gets or sets the EmpresaID
        /// </summary>
        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        /// <summary>
        /// Gets or sets the DocumentoID
        /// </summary>
        [DataMember]
        public UDT_DocumentoID DocumentoID { get; set; }

        /// <summary>
        /// Gets or sets the NumeroDoc
        /// </summary>
        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        /// <summary>
        /// Gets or sets the Actividad
        /// </summary>
        [DataMember]
        public UDT_ActividadFlujoID ActividadFlujoID { get; set; }

        /// <summary>
        /// Gets or sets the Actividad
        /// </summary>
        [DataMember]
        public UDT_DescripTBase Descriptivo { get; set; }

        /// <summary>
        /// Gets or sets the UsuarioID
        /// </summary>
        [DataMember]
        public UDT_Consecutivo UsuarioID { get; set; }

        /// <summary>
        /// Gets or sets the comprobante de anulacion
        /// </summary>
        [DataMember]
        public UDT_ComprobanteID CompAnula { get; set; }

        /// <summary>
        /// Gets or sets the CompNroAnula
        /// </summary>
        [DataMember]
        public UDT_Consecutivo CompNroAnula { get; set; }

        /// <summary>
        /// Gets or sets the Fecha
        /// </summary>
        [DataMember]
        public UDTSQL_datetime Fecha { get; set; }

        /// <summary>
        /// Gets or sets the Periodo
        /// </summary>
        [DataMember]
        public UDTSQL_datetime Periodo { get; set; }

        /// <summary>
        /// Gets or sets the Observacion
        /// </summary>
        [DataMember]
        public UDT_DescripUnFormat Observacion { get; set; }

        /// <summary>
        /// Gets or sets the AlarmaInd
        /// </summary>
        [DataMember]
        public UDT_SiNo AlarmaInd { get; set; }

        #endregion

        #region Propiedades Descripciones

        /// <summary>
        /// Login del usuario
        /// </summary>
        [DataMember]
        public UDT_DescripTBase UsuarioDesc { get; set; }

        #endregion
    }
}
