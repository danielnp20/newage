﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Models DTO_pyProyectoIngDetalleDeta
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pyProyectoIngDetalleDeta
    {
        #region DTO_pyProyectoIngDetalleDeta

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pyProyectoIngDetalleDeta(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.LocacionID.Value = dr["LocacionID"].ToString();
                this.ConsecTarea.Value = Convert.ToInt32(dr["ConsecTarea"]);
                this.RecursoID.Value = dr["RecursoID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["Cantidad"].ToString()))
                    this.Cantidad.Value = Convert.ToDecimal(dr["Cantidad"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);

                //Adicionales
                this.TareaID.Value = dr["TareaID"].ToString();
                this.TareaCliente.Value = dr["TareaCliente"].ToString();
                this.TareaDesc.Value = dr["TareaDesc"].ToString();
                this.RecursoDesc.Value = dr["RecursoDesc"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pyProyectoIngDetalleDeta()
        {
            InitCols();
        }

        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.LocacionID = new  UDTSQL_varchar(15);
            this.ConsecTarea = new UDT_Consecutivo();
            this.RecursoID = new UDT_CodigoGrl();
            this.Cantidad = new UDT_Cantidad();
            this.Consecutivo = new UDT_Consecutivo();
            //Adicionales
            this.TareaID = new UDT_CodigoGrl();
            this.TareaCliente = new UDT_CodigoGrl20();
            this.TareaDesc = new UDT_DescripUnFormat();
            this.RecursoDesc = new UDT_DescripUnFormat();
        }

        #endregion

        #region Propiedades

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDTSQL_varchar LocacionID { get; set; }

        [DataMember]
        public UDT_CodigoGrl20 TareaCliente { get; set; } //Adicionales

        [DataMember]
        public UDT_CodigoGrl TareaID { get; set; } //Adicionales

        [DataMember]
        public UDT_DescripUnFormat TareaDesc { get; set; } //Adicionales

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo ConsecTarea { get; set; }

        [DataMember]
        public UDT_CodigoGrl RecursoID { get; set; }

        [DataMember]
        public UDT_DescripUnFormat RecursoDesc { get; set; } //Adicionales

        [DataMember]
        public UDT_Cantidad Cantidad { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo Consecutivo { get; set; }

   
        #endregion
    }
}
