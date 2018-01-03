using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Nomina para aprobacion:
    /// Models DTO_NominaAprobacion
    /// </summary>
    [DataContract]
    [Serializable]
    [KnownType(typeof(DTO_acGrupo))]
    public class DTO_NominaAprobacion
    {
        #region DTO_ComprobanteAprobacion

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_NominaAprobacion(DTO_glDocumentoControl doc)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = doc.NumeroDoc.Value;
                this.PeriodoID.Value = doc.PeriodoDoc.Value;
                this.ComprobanteID.Value = doc.ComprobanteID.Value;
                this.ComprobanteNro.Value = doc.ComprobanteIDNro.Value;
                this.DocumentoID.Value = doc.DocumentoID.Value;
                this.DocumentoNro.Value = doc.DocumentoNro.Value;
                //this.UsuarioID.Value = doc.UsuarioRESP.Value;
                this.Aprobado.Value = false;
                this.Rechazado.Value = false;
                this.Seleccionar.Value = false;
 
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_NominaAprobacion()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.Aprobado = new UDT_SiNo();
            this.Rechazado = new UDT_SiNo();
            this.Seleccionar = new UDT_SiNo();
            this.Observacion = new UDT_DescripTExt();
            this.NumeroDoc = new UDT_Consecutivo();
            this.PeriodoID = new UDT_PeriodoID();
            this.ComprobanteID = new UDT_ComprobanteID();
            this.ComprobanteNro = new UDT_Consecutivo();
            this.DocumentoID = new UDT_DocumentoID();
            this.DocumentoNro = new UDT_Consecutivo();
            this.Descriptivo = new UDT_DescripTBase();
            //this.UsuarioID = new UDT_UsuarioID();
            this.FileUrl = "";
            this.Valor = new UDT_Valor();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_SiNo Aprobado { get; set; }

        [DataMember]
        public UDT_SiNo Rechazado { get; set; }

        [DataMember]
        public UDT_SiNo Seleccionar { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_PeriodoID PeriodoID { get; set; }

        [DataMember]
        public UDT_ComprobanteID ComprobanteID { get; set; }

        [DataMember]
        public UDT_Consecutivo ComprobanteNro { get; set; }

        [DataMember]
        public UDT_DocumentoID DocumentoID { get; set; }

        [DataMember]
        public UDT_Consecutivo DocumentoNro { get; set; }

        [DataMember]
        public UDT_DescripTBase Descriptivo { get; set; }

        //[DataMember]
        //public UDT_UsuarioID UsuarioID { get; set; }

        [DataMember]
        public string FileUrl { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        #endregion
    }
}
