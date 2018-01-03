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
    /// Models DTO_pyListaPrecio
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pyListaPrecio : DTO_MasterBasic
    {
        #region pyListaPrecio
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pyListaPrecio(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["Observacion"].ToString()))
                    this.Observacion.Value = dr["Observacion"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>g
        public DTO_pyListaPrecio()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Observacion = new UDT_DescripTExt();
        }

        public DTO_pyListaPrecio(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_pyListaPrecio(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

    }

}