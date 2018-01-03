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
	/// Models DTO_glLlamadasControl
	/// </summary>
	[DataContract]
	[Serializable]
	public class DTO_glLlamadasControl
	{
		#region Constructor

		/// <summary>
		/// Constuye el DTO a partir de un resultado de base de datos
		/// </summary>
		/// <param name="?"></param>
		public DTO_glLlamadasControl(IDataReader dr)
		{
			InitCols();
			try
			{
                if (!String.IsNullOrWhiteSpace(dr["NumReferencia"].ToString()))
                    this.NumReferencia.Value = Convert.ToInt32(dr["NumReferencia"]);
				this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
				this.ActividadFlujoID.Value = dr["ActividadFlujoID"].ToString();
				this.IdentificadorPrg.Value = Convert.ToInt32(dr["IdentificadorPrg"]);
                this.TerceroID.Value = Convert.ToString(dr["TerceroID"]);
				this.CodPregunta.Value = dr["CodPregunta"].ToString();
				this.UsuarioID.Value = dr["UsuarioID"].ToString();
				this.Fecha.Value = Convert.ToDateTime(dr["Fecha"].ToString());
				this.PersonaConsulta.Value = dr["PersonaConsulta"].ToString();
				this.RelacionTitular.Value = dr["RelacionTitular"].ToString();
				this.Observaciones.Value = dr["Observaciones"].ToString();
                this.LLamadaREF.Value = dr["LLamadaREF"].ToString();
                this.CodLLamada.Value = dr["CodLLamada"].ToString();
                this.NuevallamadaInd.Value = Convert.ToBoolean(dr["NuevallamadaInd"]);
				this.FechaProxllamada.Value = Convert.ToDateTime(dr["FechaProxllamada"]);
                if (!String.IsNullOrWhiteSpace(dr["FechaCompromiso"].ToString()))
                    this.FechaCompromiso.Value = Convert.ToDateTime(dr["FechaCompromiso"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorCompromiso1"].ToString()))
                    this.ValorCompromiso1.Value = Convert.ToDecimal(dr["ValorCompromiso1"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorCompromiso2"].ToString()))
                    this.ValorCompromiso2.Value = Convert.ToDecimal(dr["ValorCompromiso2"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorCompromiso3"].ToString()))
                    this.ValorCompromiso3.Value = Convert.ToDecimal(dr["ValorCompromiso3"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorCompromiso4"].ToString()))
                    this.ValorCompromiso4.Value = Convert.ToDecimal(dr["ValorCompromiso4"]);
                if (!String.IsNullOrWhiteSpace(dr["ValorCompromiso5"].ToString()))
                    this.ValorCompromiso5.Value = Convert.ToDecimal(dr["ValorCompromiso5"]);

			}
			catch (Exception e)
			{
				throw e;
			}
		}

		/// <summary>
		/// Constructor por defecto
		/// </summary>
		public DTO_glLlamadasControl()
		{
			InitCols();
		}

		/// Inicializa las columnas
		/// </summary>
		private void InitCols()
		{	
			this.NumeroDoc = new UDT_Consecutivo();	
			this.ActividadFlujoID = new UDT_ActividadFlujoID();
            this.NumReferencia = new UDT_Consecutivo();
			this.IdentificadorPrg = new UDT_Consecutivo();
            this.TerceroID = new UDT_TerceroID();
			this.CodPregunta = new UDTSQL_char(10);
			this.UsuarioID = new UDT_UsuarioID();
			this.Fecha = new UDTSQL_datetime();
			this.PersonaConsulta = new UDTSQL_char(50);
			this.RelacionTitular = new UDTSQL_char(50);
			this.Observaciones = new UDT_DescripTExt();
            this.LLamadaREF = new UDTSQL_char(50);
            this.CodLLamada = new UDT_CodigoGrl5();
			this.NuevallamadaInd = new UDT_SiNo();
			this.FechaProxllamada = new UDTSQL_datetime();
            this.FechaCompromiso = new UDTSQL_datetime();
            this.ValorCompromiso1 = new UDT_Valor();
            this.ValorCompromiso2 = new UDT_Valor();
            this.ValorCompromiso3 = new UDT_Valor();
            this.ValorCompromiso4 = new UDT_Valor();
            this.ValorCompromiso5 = new UDT_Valor();

            //Campo Adicional
            this.Pregunta = new UDT_DescripTBase();
            this.TipoReferencia = new UDTSQL_tinyint();
            this.NombreReferencia = new UDT_Descriptivo();
		}

		#endregion
		
		#region Propiedades

		[DataMember]
		[NotImportable]
		public UDT_Consecutivo NumeroDoc { get; set; }

		[DataMember]
		[NotImportable]
		public UDT_ActividadFlujoID ActividadFlujoID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumReferencia { get; set; }

		[DataMember]
		[NotImportable]
		public UDT_Consecutivo IdentificadorPrg { get; set; }

        [DataMember]
		[NotImportable]
        public UDT_TerceroID TerceroID { get; set; }

		[DataMember]
		[NotImportable]
		public UDTSQL_char CodPregunta { get; set; }

		[DataMember]
		[NotImportable]
		public UDT_UsuarioID UsuarioID { get; set; }

		[DataMember]
		[NotImportable]
		public UDTSQL_datetime Fecha { get; set; }

		[DataMember]
		[NotImportable]
		public UDTSQL_char PersonaConsulta { get; set; }

		[DataMember]
		[NotImportable]
		public UDTSQL_char RelacionTitular { get; set; }

		[DataMember]
		[NotImportable]
		public UDT_DescripTExt Observaciones { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_char LLamadaREF { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_CodigoGrl5 CodLLamada { get; set; }

		[DataMember]
		[NotImportable]
		public UDT_SiNo NuevallamadaInd { get; set; }

		[DataMember]
		[NotImportable]
		public UDTSQL_datetime FechaProxllamada { get; set; }
        
        [DataMember]
        [NotImportable]
        public UDTSQL_datetime FechaCompromiso { get; set; }
        
        [DataMember]
        [NotImportable]
        public UDT_Valor ValorCompromiso1 { get; set; }
        
        [DataMember]
        [NotImportable]
        public UDT_Valor ValorCompromiso2 { get; set; }
        
        [DataMember]
        [NotImportable]
        public UDT_Valor ValorCompromiso3 { get; set; }
        
        [DataMember]
        [NotImportable]
        public UDT_Valor ValorCompromiso4 { get; set; }
        
        [DataMember]
        [NotImportable]
        public UDT_Valor ValorCompromiso5 { get; set; }

		[DataMember]
		[NotImportable]
		public int Index { get; set; }

        //Campo Adicional
        [DataMember]
        [NotImportable]
        public UDT_DescripTBase Pregunta { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_tinyint TipoReferencia { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Descriptivo NombreReferencia { get; set; }

		#endregion
	}
}
