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
    /// Models DTO_prConvenioSolicitudDeta
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prConvenioSolicitudDeta
    {
        #region DTO_prConvenioSolicitudDeta

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_prConvenioSolicitudDeta(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]); 
                this.CodigoBSID.Value = dr["CodigoBSID"].ToString(); 
                this.inReferenciaID.Value = dr["inReferenciaID"].ToString(); 
                 if (!string.IsNullOrWhiteSpace(dr["CantidadSol"].ToString()))
                    this.CantidadSol.Value = Convert.ToDecimal(dr["CantidadSol"]);
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
        public DTO_prConvenioSolicitudDeta()
        {
            this.InitCols();
        }


        /// <summary
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.CodigoBSID = new UDT_CodigoBSID();
            this.inReferenciaID = new UDT_inReferenciaID();
            this.CantidadSol = new UDT_Cantidad();
            this.ValorUni = new UDT_Valor();
            this.Valor = new UDT_Valor();
        }

        #endregion

        #region Propiedades
        [DataMember]
        [NotImportable]
        public int Index { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumeroDoc { get; set; }
        
        [DataMember]
        [Filtrable]
        public UDT_CodigoBSID CodigoBSID { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_inReferenciaID inReferenciaID { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadSol { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor ValorUni { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor Valor { get; set; }

        #endregion
    }
}
