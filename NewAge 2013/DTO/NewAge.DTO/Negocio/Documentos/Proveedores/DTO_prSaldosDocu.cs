using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;
using System.Reflection;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class recibidos de bienes y servicios:
    /// Models DTO_prSaldosDocu
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prSaldosDocu
    {
        #region DTO_prSaldosDocu

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_prSaldosDocu(IDataReader dr)
        {
            InitCols();
            try
            {
                this.ConsecutivoDetaID.Value = Convert.ToInt32(dr["ConsecutivoDetaID"]);
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.CantidadDocu.Value = Convert.ToDecimal(dr["CantidadDocu"]);
                this.CantidadMovi.Value = Convert.ToDecimal(dr["CantidadMovi"]);
            }
            catch (Exception e)
            {
                ;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_prSaldosDocu()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ConsecutivoDetaID = new UDT_Consecutivo();
            this.EmpresaID = new UDT_EmpresaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.CantidadDocu = new UDT_Cantidad();
            this.CantidadMovi = new UDT_Cantidad();
        }
        #endregion

        #region Propiedades
        [DataMember]
        public UDT_Consecutivo ConsecutivoDetaID { get; set; }

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadDocu { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadMovi { get; set; }

        #endregion
    }
}

