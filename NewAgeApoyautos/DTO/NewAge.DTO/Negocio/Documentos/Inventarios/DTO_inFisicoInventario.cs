using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using NewAge.DTO.Negocio.Reportes;
using NewAge.DTO.Reportes;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_inFisicoInventario
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inFisicoInventario : DTO_BasicReport
    {

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_inFisicoInventario(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = (dr["EmpresaID"]).ToString();
                this.BodegaID.Value = (dr["BodegaID"]).ToString();
                this.inReferenciaID.Value = (dr["inReferenciaID"]).ToString();
                this.ActivoID.Value = Convert.ToInt32((dr["ActivoID"]));
                this.EstadoInv.Value = Convert.ToByte(dr["EstadoInv"]);
                this.Parametro1.Value = (dr["Parametro1"]).ToString();
                this.Parametro2.Value = (dr["Parametro2"]).ToString();
                this.Periodo.Value = Convert.ToDateTime(dr["Periodo"]);
                this.CantKardex.Value = Convert.ToDecimal(dr["CantKardex"]);
                this.CantFisico.Value = Convert.ToDecimal(dr["CantFisico"]);
                this.CantEntradaDoc.Value = Convert.ToDecimal(dr["CantEntradaDoc"]);
                this.CantSalidaDoc.Value = Convert.ToDecimal(dr["CantSalidaDoc"]);
                this.CantAjuste.Value = Convert.ToDecimal(dr["CantAjuste"]);
                this.FobLocal.Value = Convert.ToDecimal(dr["FobLocal"]);
                this.CostoLocal.Value = Convert.ToDecimal(dr["CostoLocal"]);
                this.FobExtra.Value = Convert.ToDecimal(dr["FobExtra"]);
                this.CostoExtra.Value = Convert.ToDecimal(dr["CostoExtra"]);
                if (!string.IsNullOrEmpty(dr["NumeroDoc"].ToString()))
                    this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.Observacion.Value = dr["Observacion"].ToString();
               
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_inFisicoInventario()
        {
            InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.BodegaID = new UDT_BodegaID();
            this.inReferenciaID = new UDT_inReferenciaID();
            this.ActivoID = new UDT_ActivoID();
            this.EstadoInv = new UDTSQL_tinyint();
            this.Parametro1 = new UDT_ParametroInvID();
            this.Parametro2 = new UDT_ParametroInvID();
            this.Periodo = new UDTSQL_smalldatetime();
            this.CantKardex = new UDT_Cantidad();
            this.CantFisico = new UDT_Cantidad();
            this.CantEntradaDoc = new UDT_Cantidad();
            this.CantSalidaDoc = new UDT_Cantidad();
            this.CantAjuste = new UDT_Cantidad();
            this.FobLocal = new UDT_Valor();
            this.CostoLocal = new UDT_Valor();
            this.FobExtra = new UDT_Valor();
            this.CostoExtra = new UDT_Valor();
            this.ReferenciaP1P2ID = new UDT_DescripTExt();
            this.ReferenciaP1P2Desc = new UDT_DescripTExt();
            this.SerialID = new UDT_DescripTExt();
            this.NumeroDoc = new UDT_Consecutivo();
            this.Observacion = new UDT_DescripTExt();          
        }

        #region Propiedades

        [DataMember]
        [NotImportable]
        public int Index { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_BodegaID BodegaID { get; set; }

        [DataMember]
        public UDT_inReferenciaID inReferenciaID { get; set; }

        [DataMember]
        public UDT_ActivoID ActivoID { get; set; }

        [DataMember]
        public UDTSQL_tinyint EstadoInv { get; set; }

        [DataMember]
        public UDT_ParametroInvID Parametro1 { get; set; }

        [DataMember]
        public UDT_ParametroInvID Parametro2 { get; set; }
       
        [DataMember]
        public UDTSQL_smalldatetime Periodo { get; set; }

        [DataMember]
        public UDT_Cantidad CantKardex { get; set; }

        [DataMember]
        public UDT_Cantidad CantFisico { get; set; }

        [DataMember]
        public UDT_Cantidad CantEntradaDoc { get; set; }

        [DataMember]
        public UDT_Cantidad CantSalidaDoc { get; set; }

        [DataMember]
        public UDT_Cantidad CantAjuste { get; set; }

        [DataMember]
        public UDT_Valor FobLocal { get; set; }

        [DataMember]
        public UDT_Valor CostoLocal { get; set; }

        [DataMember]
        public UDT_Valor FobExtra { get; set; }

        [DataMember]
        public UDT_Valor CostoExtra { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTExt SerialID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTExt ReferenciaP1P2ID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTExt ReferenciaP1P2Desc { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        [AllowNull]
        public Dictionary<string,bool> ListSeriales { get; set; }

        #endregion
    }

}
