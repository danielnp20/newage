﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class comprobante para aprobacion:
    /// Models DTO_ComprobanteAprobacion
    /// </summary>
    [DataContract]
    [Serializable]
    [KnownType(typeof(DTO_CausacionAprobacion))]
    public class DTO_ComprobanteAprobacion
    {
        #region DTO_ComprobanteAprobacion

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ComprobanteAprobacion(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.PeriodoID.Value = Convert.ToDateTime(dr["PeriodoID"]);
                this.ComprobanteID.Value = dr["ComprobanteID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["ComprobanteNro"].ToString()))
                    this.ComprobanteNro.Value = Convert.ToInt32(dr["ComprobanteNro"]);
                this.DocumentoID.Value = Convert.ToInt32(dr["DocumentoID"]);
                this.DocumentoNro.Value = Convert.ToInt32(dr["DocumentoNro"]);
                this.UsuarioID.Value = dr["UsuarioID"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ComprobanteAprobacion()
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
            this.Observacion = new UDT_DescripTExt();
            this.NumeroDoc = new UDT_Consecutivo();
            this.PeriodoID = new UDT_PeriodoID();
            this.ComprobanteID = new UDT_ComprobanteID();
            this.ComprobanteNro = new UDT_Consecutivo();
            this.DocumentoID = new UDT_DocumentoID();
            this.DocumentoNro = new UDT_Consecutivo();
            this.Descriptivo = new UDT_DescripTBase();
            this.UsuarioID = new UDT_UsuarioID();
            this.FileUrl = "";
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_SiNo Aprobado { get; set; }

        [DataMember]
        public UDT_SiNo Rechazado { get; set; }

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

        [DataMember]
        public UDT_UsuarioID UsuarioID { get; set; }

        [DataMember]
        public string FileUrl { get; set; }

        #endregion
    }
}