using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;
using System.Data;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Models DTO_DetalleRenglon
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_DetalleRenglon
    {
        #region DTO_DetalleRenglon

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_DetalleRenglon(IDataReader dr)
        {
            InitCols();
            try
            {
                //this.ImpuestoDeclID.Value = dr["ImpuestoDeclID"].ToString();
                //this.Renglon.Value = dr["Renglon"].ToString();
                this.CuentaID.Value = dr["CuentaID"].ToString();
                this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                this.ComprobanteID.Value = dr["ComprobanteID"].ToString().Trim();
                this.ComprobanteNro.Value = Convert.ToInt32(dr["ComprobanteNro"]);
                this.Comprobante = this.ComprobanteID.Value + " - " + this.ComprobanteNro.Value.ToString();
                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.Nombre.Value = dr["Nombre"].ToString();
                this.VlrBaseML.Value = Convert.ToDecimal(dr["VlrBaseML"]);
                this.VlrMdaLoc.Value = Convert.ToDecimal(dr["VlrMdaLoc"]);
                this.PrefijoCOM.Value = dr["PrefijoCOM"].ToString();
                this.DocumentoCOM.Value = dr["DocumentoCOM"].ToString();

                this.Porcentaje = "0 %";
                if (this.VlrBaseML.Value.Value != 0)
                    this.Porcentaje = (Math.Round((this.VlrMdaLoc.Value.Value / this.VlrBaseML.Value.Value * 100), 2)).ToString() + "%";
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_DetalleRenglon()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ImpuestoDeclID = new UDT_ImpuestoDeclID();
            this.Renglon = new UDT_Renglon();
            this.CuentaID = new UDT_CuentaID();
            this.ComprobanteID = new UDT_ComprobanteID();
            this.ComprobanteNro = new UDT_Consecutivo();
            this.Fecha = new UDTSQL_smalldatetime();
            this.TerceroID = new UDT_TerceroID();
            this.Nombre = new UDT_DescripTBase();
            this.VlrBaseML = new UDT_Valor();
            this.VlrMdaLoc = new UDT_Valor();
            this.PrefijoCOM = new UDT_PrefijoID();
            this.DocumentoCOM = new UDTSQL_char(20);
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_ImpuestoDeclID ImpuestoDeclID { get; set; }

        [DataMember]
        public UDT_Renglon Renglon { get; set; }

        [DataMember]
        public UDT_CuentaID CuentaID { get; set; }

        [DataMember]
        public UDT_ComprobanteID ComprobanteID { get; set; }

        [DataMember]
        public UDT_Consecutivo ComprobanteNro { get; set; }

        [DataMember]
        public string Comprobante { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Fecha { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_DescripTBase Nombre { get; set; }

        [DataMember]
        public UDT_Valor VlrBaseML { get; set; }
        
        [DataMember]
        public UDT_Valor VlrMdaLoc { get; set; }

        [DataMember]
        public UDT_PrefijoID PrefijoCOM { get; set; }

        [DataMember]
        public UDTSQL_char DocumentoCOM { get; set; }

        [DataMember]
        public string Porcentaje { get; set; }

        #endregion

    }
}
