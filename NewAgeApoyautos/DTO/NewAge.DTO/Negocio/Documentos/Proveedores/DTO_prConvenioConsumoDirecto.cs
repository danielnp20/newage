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
    /// Models DTO_prConvenioConsumoDirecto
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prConvenioConsumoDirecto
    {
        #region DTO_prConvenioSolicitudDocu

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_prConvenioConsumoDirecto(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.FechaPlanilla.Value = Convert.ToDateTime(dr["FechaPlanilla"]);
                this.ProyectoID.Value = dr["ProyectoID"].ToString();              
                this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                this.ProveedorID.Value = dr["ProveedorID"].ToString();
                this.CodigoBSID.Value = dr["CodigoBSID"].ToString();
                this.inReferenciaID.Value = dr["inReferenciaID"].ToString();
                this.SerialID.Value = dr["SerialID"].ToString();
                this.Cantidad.Value = Convert.ToDecimal(dr["Cantidad"]);
                this.NumeroDocOC.Value = Convert.ToInt32(dr["NumeroDocOC"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_prConvenioConsumoDirecto()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.FechaPlanilla = new UDTSQL_datetime();
            this.ProyectoID = new UDT_ProyectoID();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.ProveedorID = new UDT_ProveedorID();
            this.CodigoBSID = new UDT_CodigoBSID();
            this.inReferenciaID = new UDT_inReferenciaID();
            this.SerialID = new UDT_SerialID();
            this.Cantidad = new UDT_Valor();
            this.PrefijoID = new UDT_PrefijoID();
            this.DocumentoNro = new UDT_Consecutivo();
            this.NumeroDocOC = new UDT_Consecutivo();
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
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaPlanilla { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        public UDT_ProveedorID ProveedorID { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_CodigoBSID CodigoBSID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_inReferenciaID inReferenciaID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_SerialID SerialID { get; set; }

        [DataMember]
        public UDT_Valor Cantidad { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_PrefijoID PrefijoID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Consecutivo DocumentoNro { get; set; }
        
        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumeroDocOC { get; set; }

        [DataMember]
        [NotImportable]
        [AllowNull]
        public UDT_Valor ValorUni { get; set; }

        [DataMember]
        [NotImportable]
        [AllowNull]
        public UDT_Valor Valor { get; set; }


        #endregion
    }
}
