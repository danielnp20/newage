using System;
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
    /// Class comprobante para aprobacion:
    /// Models DTO_AnticipoAprobacion
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_AnticipoAprobacion : DTO_ComprobanteAprobacion
    {
        #region DTO_AnticipoAprobacion

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_AnticipoAprobacion(IDataReader dr) : base(dr)
        {
            InitCols();
            try
            {
                try { this.TarjetaCreditoID.Value = dr["TarjetaCreditoID"].ToString(); }  catch (Exception) { };
                this.TerceroID.Value = dr["TerceroID"].ToString(); 
                this.DescriptivoTercero.Value = dr["DescriptivoTercero"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["DocumentoTercero"].ToString()))
                    this.DocumentoTercero.Value = Convert.ToString(dr["DocumentoTercero"]);
                this.MonedaID.Value = dr["MonedaID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["Valor"].ToString()))
                    this.Valor.Value = Convert.ToDecimal(dr["Valor"]); 
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_AnticipoAprobacion()
            : base()
        {
            InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.TarjetaCreditoID = new UDT_TarjetaCreditoID();
            this.TerceroID = new UDT_TerceroID();
            this.DescriptivoTercero = new UDT_DescripTBase();
            this.DocumentoTercero = new UDTSQL_char(20);
            this.MonedaID = new UDT_MonedaID();
            this.Valor = new UDT_Valor();
            this.Descripcion = new UDT_DescripTExt();
            this.AnticipoTipoID = new UDT_AnticipoTipoID();
            this.Detalle = new List<DTO_cpTarjetaPagos>();
            this.FileUrl = "";
        }

        #endregion

        #region Propiedades

        [AllowNull]
        [DataMember]
        public UDT_TarjetaCreditoID TarjetaCreditoID { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_DescripTBase DescriptivoTercero { get; set; }

        [DataMember]
        public UDTSQL_char DocumentoTercero { get; set; }

        [DataMember]
        public UDT_MonedaID MonedaID { get; set; }

        [DataMember]
        public UDT_DescripTExt Descripcion { get; set; }

        [DataMember]
        public UDT_AnticipoTipoID AnticipoTipoID  { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [AllowNull]
        [DataMember]
        public List<DTO_cpTarjetaPagos> Detalle { get; set; }

        [DataMember]
        public string FileUrl { get; set; }

        #endregion
    }
}
