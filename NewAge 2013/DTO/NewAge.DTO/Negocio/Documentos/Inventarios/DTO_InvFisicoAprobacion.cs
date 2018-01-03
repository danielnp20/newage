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
    /// Models DTO_InvFisicoAprobacion
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_InvFisicoAprobacion : DTO_ComprobanteAprobacion
    {
        #region DTO_AnticipoAprobacion

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_InvFisicoAprobacion(IDataReader dr) : base(dr)
        {
            InitCols();
            try
            {
                this.BodegaID.Value = dr["BodegaID"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_InvFisicoAprobacion()
            : base()
        {
            InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.BodegaID = new UDT_BodegaID();
            this.BodegaDesc = new UDT_DescripTBase();
            this.CantAjusteEntrada = new UDT_Cantidad();
            this.CantAjusteSalida = new UDT_Cantidad();
            this.ValorAjusteEntrada = new UDT_Valor();
            this.ValorAjusteSalida = new UDT_Valor();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_BodegaID BodegaID { get; set; }

        [DataMember]
        public UDT_DescripTBase BodegaDesc { get; set; }

        [DataMember]
        public UDT_Cantidad CantAjusteEntrada { get; set; }

        [DataMember]
        public UDT_Valor ValorAjusteEntrada { get; set; }

        [DataMember]
        public UDT_Cantidad CantAjusteSalida { get; set; }

        [DataMember]
        public UDT_Valor ValorAjusteSalida { get; set; }   

        #endregion
    }
}
