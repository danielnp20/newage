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
    /// Models DTO_ccCompradorAnexos
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccCompradorAnexos : DTO_MasterComplex
    {
        #region DTO_ccCompradorAnexos
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccCompradorAnexos(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.CompradorCarteraDesc.Value = dr["CompradorCarteraDesc"].ToString();
                    this.DocumListaDesc.Value = dr["DocumListaDesc"].ToString();
                }

                this.CompradorCarteraID.Value = dr["CompradorCarteraID"].ToString();
                this.DocumListaID.Value = dr["DocumListaID"].ToString();
                
            }
            catch (Exception e)
            {
                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccCompradorAnexos() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.CompradorCarteraID = new UDT_BasicID();
            this.CompradorCarteraDesc = new UDT_Descriptivo();
            this.DocumListaID = new UDT_BasicID();
            this.DocumListaDesc = new UDT_Descriptivo();
        }

        public DTO_ccCompradorAnexos(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccCompradorAnexos(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID CompradorCarteraID { get; set; }

        [DataMember]
        public UDT_Descriptivo CompradorCarteraDesc { get; set; }

        [DataMember]
        public UDT_BasicID DocumListaID { get; set; }

        [DataMember]
        public UDT_Descriptivo DocumListaDesc { get; set; }

    }

}