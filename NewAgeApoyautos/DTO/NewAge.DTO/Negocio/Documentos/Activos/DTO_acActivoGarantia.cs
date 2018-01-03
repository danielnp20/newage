using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_acActivoGarantia
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_acActivoGarantia
    {
        #region Constructor
     
        public DTO_acActivoGarantia(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.GarantiaRef.Value = dr["GarantiaRef"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["ActivoID"].ToString()))
                    this.ActivoID.Value = Convert.ToInt32(dr["ActivoID"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaINICliente"].ToString()))
                    this.FechaINICliente.Value = Convert.ToDateTime(dr["FechaINICliente"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaFINCliente"].ToString()))
                    this.FechaFINCliente.Value = Convert.ToDateTime(dr["FechaFINCliente"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaINIProveedor"].ToString()))
                    this.FechaINIProveedor.Value = Convert.ToDateTime(dr["FechaINIProveedor"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaFINProveedor"].ToString()))
                    this.FechaFINProveedor.Value = Convert.ToDateTime(dr["FechaFINProveedor"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaFINEmpresa"].ToString()))
                    this.FechaFINEmpresa.Value = Convert.ToDateTime(dr["FechaFINEmpresa"]);
            }
            catch (Exception e)
            { ; }
        }



        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_acActivoGarantia()
        {
            InitCols();
        }
        
        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.ActivoID = new UDT_Consecutivo();
            this.GarantiaRef = new UDTSQL_char(30);
            this.FechaINICliente = new UDTSQL_smalldatetime();
            this.FechaFINCliente = new UDTSQL_smalldatetime();
            this.FechaINIProveedor = new UDTSQL_smalldatetime();
            this.FechaFINProveedor = new UDTSQL_smalldatetime();
            this.FechaFINEmpresa = new UDTSQL_smalldatetime();

            //Extras
            this.ProyectoID = new UDT_ProyectoID();
            this.TipoAct = new UDTSQL_char(1);
            this.inReferenciaID = new UDT_ReferenciaID();
            this.Descriptivo = new UDT_Descriptivo();
            this.Serial = new UDT_Descriptivo();
            this.ProveedorID = new UDT_ProveedorID();
            this.FechaCompra = new UDTSQL_smalldatetime();
        }
        #endregion

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_Consecutivo ActivoID { get; set; }

        #region Propiedades Extras
        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDTSQL_char TipoAct { get; set; }

        [DataMember]
        public UDT_ReferenciaID inReferenciaID { get; set; }

        [DataMember]
        public UDT_Descriptivo Descriptivo { get; set; }

        [DataMember]
        public UDT_Descriptivo Serial { get; set; }

        [DataMember]
        public UDT_ProveedorID ProveedorID { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaCompra { get; set; }

        #endregion

        #region Propiedades      

        [DataMember]
        public UDTSQL_char GarantiaRef { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaINICliente { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaFINCliente { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaINIProveedor { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaFINProveedor { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaFINEmpresa { get; set; }    
  
        #endregion
  


       

    }
}
