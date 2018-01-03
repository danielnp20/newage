﻿using System;
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
    /// Models DTO_coReporte
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coReporte : DTO_MasterBasic 
    {
        #region DTO_coReporte
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coReporte(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {

                if (!isReplica)
                {
                    this.LibroXdefectoDesc.Value = Convert.ToString(dr["LibroXdefectoDesc"]);
                    this.DocumentoDesc.Value = Convert.ToString(dr["DocumentoDesc"]);
                }

                this.DocumentoID.Value = Convert.ToString(dr["DocumentoID"]);
                if (!string.IsNullOrWhiteSpace(dr["LibroXdefecto"].ToString()))
                    this.LibroXdefecto.Value = Convert.ToString(dr["LibroXdefecto"]);                 
                if (!string.IsNullOrWhiteSpace(dr["TipoReporte"].ToString()))
                    this.TipoReporte.Value = Convert.ToByte(dr["TipoReporte"]);
                if (!string.IsNullOrWhiteSpace(dr["TipoDato"].ToString()))
                    this.TipoDato.Value = Convert.ToByte(dr["TipoDato"]);
                if (!string.IsNullOrWhiteSpace(dr["ComparativoInd"].ToString()))
                    this.ComparativoInd.Value = Convert.ToBoolean(dr["ComparativoInd"]);
                if (!string.IsNullOrWhiteSpace(dr["PresentaTipo"].ToString()))
                    this.PresentaTipo.Value = Convert.ToByte(dr["PresentaTipo"]);
                if (!string.IsNullOrWhiteSpace(dr["Factor"].ToString()))
                    this.Factor.Value = Convert.ToByte(dr["Factor"]);
                if (!string.IsNullOrWhiteSpace(dr["Unidad"].ToString()))
                    this.Unidad.Value = Convert.ToString(dr["Unidad"]);
                if (!string.IsNullOrWhiteSpace(dr["IndicadorTipo"].ToString()))
                    this.IndicadorTipo.Value = Convert.ToByte(dr["IndicadorTipo"]);
            }
            catch (Exception e)
            {
               throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coReporte() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.DocumentoID = new UDT_BasicID();
            this.DocumentoDesc = new UDT_Descriptivo();
            this.LibroXdefecto = new UDT_BasicID();
            this.LibroXdefectoDesc = new UDT_Descriptivo();
            this.TipoDato = new UDTSQL_smallint();
            this.TipoReporte = new UDTSQL_smallint();
            this.ComparativoInd = new UDT_SiNo();
            this.PresentaTipo = new UDTSQL_tinyint();
            this.Factor = new UDT_Cantidad();
            this.Unidad = new UDTSQL_char(20);
            this.IndicadorTipo = new UDTSQL_tinyint();
        }

        public DTO_coReporte(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_coReporte(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID DocumentoID { get; set; }

        [DataMember]
        public UDT_Descriptivo DocumentoDesc { get; set; }

        [DataMember]
        public UDT_BasicID LibroXdefecto { get; set; }

        [DataMember]
        public UDT_Descriptivo LibroXdefectoDesc { get; set; }

        [DataMember]
        public UDTSQL_smallint TipoDato { get; set; }

        [DataMember]
        public UDTSQL_smallint TipoReporte { get; set; }

        [DataMember]
        public UDT_SiNo ComparativoInd { get; set; }

        [DataMember]
        public UDTSQL_tinyint PresentaTipo { get; set; }

        [DataMember]
        public UDT_Cantidad Factor { get; set; }

        [DataMember]
        public UDTSQL_char Unidad { get; set; }

        [DataMember]
        public UDTSQL_tinyint IndicadorTipo { get; set; }


    }
}