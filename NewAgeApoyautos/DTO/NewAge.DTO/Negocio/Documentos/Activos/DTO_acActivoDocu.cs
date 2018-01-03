using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio.Documentos.Activos
{
    /// <summary>
    /// Class Error:
    /// Models DTO_acActivoControl
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_acActivoDocu
    {
        #region Constructor

        public DTO_acActivoDocu(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.AsesorID.Value = dr["AsesorID"].ToString();
                this.MvtoTipoActID.Value = dr["MvtoTipoActID"].ToString();
                this.DocumentoREL.Value = Convert.ToInt32(dr["DocumentoREL"]);
                this.LocFisicaID.Value = dr["LocFisicaID"].ToString();
                this.Observacion.Value = dr["Observacion"].ToString();
                this.Valor.Value = Convert.ToInt32(dr["Valor"]);
                this.Iva.Value = Convert.ToInt32(dr["Iva"]);
                this.DatoAdd1.Value = dr["DatoAdd1"].ToString();
                this.DatoAdd2.Value = dr["DatoAdd2"].ToString();
                this.DatoAdd3.Value = dr["DatoAdd3"].ToString();
                this.DatoAdd4.Value = dr["DatoAdd4"].ToString();
                this.DatoAdd5.Value = dr["DatoAdd5"].ToString();
            }
            catch (Exception e)
            { 
                throw e; 
            }
        }

        public DTO_acActivoDocu()
        {
            InitCols();
        }

        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.AsesorID = new UDT_AsesorID();
            this.MvtoTipoActID = new UDT_MvtoTipoID();
            this.DocumentoREL = new UDT_Consecutivo();
            this.LocFisicaID = new UDT_LocFisicaID();
            this.Observacion = new UDT_DescripTExt();
            this.Valor = new UDT_Valor();
            this.Iva = new UDT_Valor();
            this.DatoAdd1 = new UDTSQL_char(20);
            this.DatoAdd2 = new UDTSQL_char(20);
            this.DatoAdd3 = new UDTSQL_char(20);
            this.DatoAdd4 = new UDTSQL_char(20);
            this.DatoAdd5 = new UDTSQL_char(20);
            this.NumeroDoc = new UDT_Consecutivo();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_AsesorID AsesorID { get; set; }

        [DataMember]
        public UDT_MvtoTipoID MvtoTipoActID { get; set; }

        [DataMember]
        public UDT_Consecutivo DocumentoREL { get; set; }

        [DataMember]
        public UDT_LocFisicaID LocFisicaID { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_Valor Iva { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char DatoAdd1 { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char DatoAdd2 { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char DatoAdd3 { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char DatoAdd4 { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char DatoAdd5 { get; set; }

        #endregion
    }
}
