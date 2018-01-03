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
    /// Models DTO_glGarantiaControl
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glGarantiaControl
    {
        #region Constructor

		/// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glGarantiaControl(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = Convert.ToString(dr["EmpresaID"]);
                this.DocumentoID.Value = Convert.ToInt32(dr["DocumentoID"]);
                this.TerceroID.Value = Convert.ToString(dr["TerceroID"]);
                if (!string.IsNullOrWhiteSpace(dr["NumeroDoc"].ToString())) 
                    this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.GarantiaID.Value = Convert.ToString(dr["GarantiaID"]);
                this.FechaINI.Value = Convert.ToDateTime(dr["FechaINI"]);
                this.FechaVTO.Value = Convert.ToDateTime(dr["FechaVTO"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaRegistro"].ToString()))
                    this.FechaRegistro.Value = Convert.ToDateTime(dr["FechaRegistro"]);

                this.CodigoGarantia.Value = Convert.ToString(dr["CodigoGarantia"]);
                this.CodigoGarantia1.Value = Convert.ToString(dr["CodigoGarantia1"]);
                this.Placa.Value = Convert.ToString(dr["Placa"]);
                if (!string.IsNullOrWhiteSpace(dr["Modelo"].ToString()))
                    this.Modelo.Value = Convert.ToInt16(dr["Modelo"]);
                if (!string.IsNullOrWhiteSpace(dr["Ano"].ToString()))
                    this.Ano.Value = Convert.ToInt16(dr["Ano"]);
                this.FaseColdaID.Value = Convert.ToString(dr["FaseColdaID"]);
                if (!string.IsNullOrWhiteSpace(dr["FuentePRE"].ToString()))
                    this.FuentePRE.Value = Convert.ToByte(dr["FuentePRE"]);
                if (!string.IsNullOrWhiteSpace(dr["FuenteHIP"].ToString()))
                    this.FuenteHIP.Value = Convert.ToByte(dr["FuenteHIP"]);
                this.Direccion.Value = Convert.ToString(dr["Direccion"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrFuente"].ToString()))
                    this.VlrFuente.Value = Convert.ToDecimal(dr["VlrFuente"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaFuente"].ToString()))
                    this.FechaFuente.Value = Convert.ToDateTime(dr["FechaFuente"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrAsegurado"].ToString()))
                    this.VlrAsegurado.Value = Convert.ToDecimal(dr["VlrAsegurado"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor1"].ToString()))
                    this.Valor1.Value = Convert.ToDecimal(dr["Valor1"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor2"].ToString()))
                    this.Valor2.Value = Convert.ToDecimal(dr["Valor2"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor3"].ToString()))
                    this.Valor3.Value = Convert.ToDecimal(dr["Valor3"]);
                this.Dato1.Value = Convert.ToString(dr["Dato1"]);
                this.Dato2.Value = Convert.ToString(dr["Dato2"]);
                this.Dato3.Value = Convert.ToString(dr["Dato3"]);
                this.Descripcion.Value = Convert.ToString(dr["Descripcion"]);
                this.ActivoInd.Value = Convert.ToBoolean(dr["ActivoInd"]);
                this.Matricula.Value = Convert.ToString(dr["Matricula"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
                if (!string.IsNullOrEmpty(dr["NuevoInd"].ToString()))
                    this.NuevoInd.Value = Convert.ToBoolean(dr["NuevoInd"]);
                if (!string.IsNullOrEmpty(dr["VehiculoTipo"].ToString()))
                    this.VehiculoTipo.Value = Convert.ToByte(dr["VehiculoTipo"]);
                if (!string.IsNullOrEmpty(dr["InmuebleTipo"].ToString()))
                    this.InmuebleTipo.Value = Convert.ToByte(dr["InmuebleTipo"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrGarantia"].ToString()))
                    this.VlrGarantia.Value = Convert.ToDecimal(dr["VlrGarantia"]);
                this.UpdateInd = false;

                this.PrefijoID.Value = Convert.ToString(dr["PrefijoID"]);

                //this.DocumentoNro.Value = Convert.ToString(dr["DocumentoNro"]);
                if (!string.IsNullOrWhiteSpace(dr["DocumentoNro"].ToString()))
                    this.DocumentoNro.Value = Convert.ToInt32(dr["DocumentoNro"]);

                if (!string.IsNullOrEmpty(dr["RetiraInd"].ToString()))
                    this.RetiraInd.Value = Convert.ToBoolean(dr["RetiraInd"]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glGarantiaControl()
        {
            InitCols();
            this.VlrFuente.Value = 0;
            this.VlrAsegurado.Value = 0;
            this.VlrGarantia.Value = 0;
            this.Valor1.Value = 0;
            this.Valor2.Value = 0;
            this.Valor3.Value = 0;
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.DocumentoID = new UDT_DocumentoID();
            this.TerceroID = new UDT_TerceroID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.GarantiaID = new UDT_CodigoGrl10();
            this.FechaINI = new UDTSQL_smalldatetime();
            this.FechaVTO = new UDTSQL_smalldatetime();
            this.FechaRegistro = new UDTSQL_smalldatetime();
            this.CodigoGarantia1 = new UDT_CodigoGrl20();
            this.CodigoGarantia = new UDT_CodigoGrl20();
            this.Placa = new UDT_CodigoGrl20();
            this.Modelo = new UDTSQL_smallint();
            this.Ano = new UDTSQL_smallint();
            this.FaseColdaID = new UDT_CodigoGrl10();
            this.FuentePRE = new UDTSQL_tinyint();
            this.FuenteHIP = new UDTSQL_tinyint();
            this.Direccion = new UDT_DescripTBase();
            this.VlrFuente = new UDT_Valor();
            this.FechaFuente = new UDTSQL_smalldatetime(); 
            this.VlrAsegurado = new UDT_Valor();
            this.Valor1 = new UDT_Valor();
            this.Valor2 = new UDT_Valor();
            this.Valor3 = new UDT_Valor();
            this.Dato1 = new UDTSQL_char(20);
            this.Dato2 = new UDTSQL_char(20);
            this.Dato3 = new UDTSQL_char(20);
            this.Descripcion = new UDT_DescripTExt();
            this.ActivoInd = new UDT_SiNo();
            this.Consecutivo = new UDT_Consecutivo();
            this.NuevoInd = new UDT_SiNo();
            this.Matricula = new UDT_CodigoGrl20();
            this.VehiculoTipo = new UDTSQL_tinyint();
            this.InmuebleTipo = new UDTSQL_tinyint();
            this.VlrGarantia = new UDT_Valor();
            //Adicionales
            this.Marca = new UDT_SiNo();
            this.TerceroDesc = new UDT_Descriptivo();
            this.DocumentoTercero = new UDT_DescripTBase();
            this.PrefDoc = new UDT_DescripTBase();
            this.GarantiaDesc = new UDT_Descriptivo();

            this.PrefijoID = new UDT_PrefijoID();
            this.DocumentoNro = new UDT_Consecutivo();
            this.RetiraInd = new UDT_SiNo(); 
        }

	    #endregion
        
        #region Propiedades

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_DocumentoID DocumentoID { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_CodigoGrl10 GarantiaID { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaINI { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaVTO { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaRegistro { get; set; }


        [DataMember]
        public UDT_CodigoGrl20 CodigoGarantia { get; set; }

        [DataMember]
        public UDT_CodigoGrl20 CodigoGarantia1 { get; set; }

        [DataMember]
        public UDT_CodigoGrl20 Placa { get; set; }

        [DataMember]
        public UDTSQL_smallint Modelo { get; set; }

        [DataMember]
        public UDTSQL_smallint Ano { get; set; }

        [DataMember]
        public UDT_CodigoGrl10 FaseColdaID { get; set; }

        [DataMember]
        public UDTSQL_tinyint FuentePRE { get; set; }

        [DataMember]
        public UDTSQL_tinyint FuenteHIP { get; set; }

        [DataMember]
        public UDT_DescripTBase Direccion { get; set; }

        [DataMember]
        public UDT_Valor VlrFuente { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaFuente { get; set; }

        [DataMember]
        public UDT_Valor VlrAsegurado { get; set; }

        [DataMember]
        public UDT_Valor Valor1 { get; set; }

        [DataMember]
        public UDT_Valor Valor2 { get; set; }

        [DataMember]
        public UDT_Valor Valor3 { get; set; }

        [DataMember]
        public UDTSQL_char Dato1 { get; set; }

        [DataMember]
        public UDTSQL_char Dato2 { get; set; }

        [DataMember]
        public UDTSQL_char Dato3 { get; set; }

        [DataMember]
        public UDT_DescripTExt Descripcion { get; set; }

        [DataMember]
        public UDT_SiNo ActivoInd { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        [DataMember]
        public UDT_SiNo NuevoInd { get; set; }

        [DataMember]
        public UDT_CodigoGrl20 Matricula { get; set; }

        [DataMember]
        public UDTSQL_tinyint VehiculoTipo { get; set; }

        [DataMember]
        public UDTSQL_tinyint InmuebleTipo { get; set; }

        [DataMember]
        public UDT_Valor VlrGarantia { get; set; }

        #region Campos Adicionales

        [DataMember]
        [AllowNull]
        public UDT_SiNo Marca { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Descriptivo TerceroDesc { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Descriptivo GarantiaDesc { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTBase PrefDoc { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTBase DocumentoTercero { get; set; }

        [DataMember]
        [AllowNull]
        public bool UpdateInd { get; set; }

        #endregion


        [DataMember]
        public UDT_PrefijoID PrefijoID { get; set; }

        [DataMember]
        public UDT_Consecutivo DocumentoNro { get; set; }

        [DataMember]
        public UDT_SiNo RetiraInd { get; set; }

        #endregion
    }
}
