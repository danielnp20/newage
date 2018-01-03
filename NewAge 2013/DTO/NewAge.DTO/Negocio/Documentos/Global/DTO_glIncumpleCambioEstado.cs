using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;

namespace NewAge.DTO.Negocio
{
    public class DTO_glIncumpleCambioEstado
    {
        #region Constructor

		/// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glIncumpleCambioEstado(IDataReader dr)
        {
            InitCols();
            try
            {
                this.DocumentoID.Value = Convert.ToInt32(dr["DocumentoID"]);
                if (!string.IsNullOrWhiteSpace(dr["TerceroID"].ToString()))
                    this.TerceroID.Value = Convert.ToString(dr["TerceroID"]);
                this.EtapaIncumplimiento.Value = Convert.ToString(dr["EtapaIncumplimiento"]);
                if (!string.IsNullOrWhiteSpace(dr["Abogado"].ToString()))
                    this.Abogado.Value = Convert.ToString(dr["Abogado"]);
                this.FechaINI.Value = Convert.ToDateTime(dr["FechaINI"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaFIN"].ToString()))
                    this.FechaFIN.Value = Convert.ToDateTime(dr["FechaFIN"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaCompromisoIni"].ToString()))
                    this.FechaCompromisoIni.Value = Convert.ToDateTime(dr["FechaCompromisoIni"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaCompromiso"].ToString()))
                    this.FechaCompromiso.Value = Convert.ToDateTime(dr["FechaCompromiso"]);
                if (!string.IsNullOrWhiteSpace(dr["TerminaIncumplimientoInd"].ToString()))
                    this.TerminaIncumplimientoInd.Value = Convert.ToBoolean(dr["TerminaIncumplimientoInd"]);
                if (!string.IsNullOrWhiteSpace(dr["Observaciones"].ToString()))
                    this.Observaciones.Value = Convert.ToString(dr["Observaciones"]);

                if (!string.IsNullOrWhiteSpace(dr["Numero1"].ToString()))
                    this.Numero1.Value = Convert.ToInt32(dr["Numero1"]);
                if (!string.IsNullOrWhiteSpace(dr["Numero2"].ToString()))
                    this.Numero2.Value = Convert.ToInt32(dr["Numero2"]);
                if (!string.IsNullOrWhiteSpace(dr["Numero3"].ToString()))
                    this.Numero3.Value = Convert.ToInt32(dr["Numero3"]);
                if (!string.IsNullOrWhiteSpace(dr["Numero4"].ToString()))
                    this.Numero4.Value = Convert.ToInt32(dr["Numero4"]);
                if (!string.IsNullOrWhiteSpace(dr["Numero5"].ToString()))
                    this.Numero5.Value = Convert.ToInt32(dr["Numero5"]);

                if (!string.IsNullOrWhiteSpace(dr["Valor1"].ToString()))
                    this.Valor1.Value = Convert.ToDecimal(dr["Valor1"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor2"].ToString()))
                    this.Valor2.Value = Convert.ToDecimal(dr["Valor2"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor3"].ToString()))
                    this.Valor3.Value = Convert.ToDecimal(dr["Valor3"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor4"].ToString()))
                    this.Valor4.Value = Convert.ToDecimal(dr["Valor4"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor5"].ToString()))
                    this.Valor5.Value = Convert.ToDecimal(dr["Valor5"]);

                this.Dato1.Value = dr["Dato1"].ToString();
                this.Dato2.Value = dr["Dato2"].ToString();
                this.Dato3.Value = dr["Dato3"].ToString();
                this.Dato4.Value = dr["Dato4"].ToString();
                this.Dato5.Value = dr["Dato5"].ToString();

                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glIncumpleCambioEstado()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.DocumentoID = new UDT_DocumentoID();
            this.TerceroID = new UDT_TerceroID();
            this.EtapaIncumplimiento = new UDTSQL_char(10);
            this.Abogado = new UDT_TerceroID();
            this.FechaINI = new UDTSQL_smalldatetime();
            this.FechaFIN = new UDTSQL_smalldatetime();
            this.FechaCompromiso = new UDTSQL_smalldatetime();
            this.FechaCompromisoIni = new UDTSQL_smalldatetime();
            this.TerminaIncumplimientoInd = new UDT_SiNo();
            this.Observaciones = new UDT_DescripTExt();
            this.Numero1 = new UDTSQL_int();
            this.Numero2 = new UDTSQL_int();
            this.Numero3 = new UDTSQL_int();
            this.Numero4 = new UDTSQL_int();
            this.Numero5 = new UDTSQL_int();
            this.Valor1 = new UDT_Valor();
            this.Valor2 = new UDT_Valor();
            this.Valor3 = new UDT_Valor();
            this.Valor4 = new UDT_Valor();
            this.Valor5 = new UDT_Valor();
            this.Dato1 = new UDTSQL_char(50);
            this.Dato2 = new UDTSQL_char(50);
            this.Dato3 = new UDTSQL_char(50);
            this.Dato4 = new UDTSQL_char(50);
            this.Dato5 = new UDTSQL_char(50);
            this.Consecutivo = new UDT_Consecutivo();
            //Otros
            this.ConsCierreDiaCartera = new UDT_Consecutivo();
            
        }

	    #endregion
        
        #region Propiedades

        [DataMember]
        public UDT_DocumentoID DocumentoID { get; set; }
        
        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }
        
        [DataMember]
        public UDTSQL_char EtapaIncumplimiento { get; set; }
        
        [DataMember]
        public UDT_TerceroID Abogado { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime FechaINI { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime FechaFIN { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaCompromisoIni { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaCompromiso { get; set; }
        
        [DataMember]
        public UDT_SiNo TerminaIncumplimientoInd { get; set; }
        
        [DataMember]
        public UDT_DescripTExt Observaciones { get; set; }

        [DataMember]
        public UDTSQL_int Numero1 { get; set; }

        [DataMember]
        public UDTSQL_int Numero2 { get; set; }

        [DataMember]
        public UDTSQL_int Numero3 { get; set; }

        [DataMember]
        public UDTSQL_int Numero4 { get; set; }

        [DataMember]
        public UDTSQL_int Numero5 { get; set; }

        [DataMember]
        public UDT_Valor Valor1 { get; set; }

        [DataMember]
        public UDT_Valor Valor2 { get; set; }

        [DataMember]
        public UDT_Valor Valor3 { get; set; }

        [DataMember]
        public UDT_Valor Valor4 { get; set; }

        [DataMember]
        public UDT_Valor Valor5 { get; set; }

        [DataMember]
        public UDTSQL_char Dato1 { get; set; }

        [DataMember]
        public UDTSQL_char Dato2 { get; set; }

        [DataMember]
        public UDTSQL_char Dato3 { get; set; }

        [DataMember]
        public UDTSQL_char Dato4 { get; set; }

        [DataMember]
        public UDTSQL_char Dato5 { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        //Otros

        [DataMember]
        public UDT_Consecutivo ConsCierreDiaCartera { get; set; }
        #endregion

    }
}
