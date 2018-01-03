using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_glDocMigracionEstructura
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glDocMigracionEstructura : DTO_MasterBasic
    {
        #region glDocMigracionEstructura
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glDocMigracionEstructura(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.DocumentoDesc.Value = dr["DocumentoDesc"].ToString();
                }

                this.DocumentoID.Value = Convert.ToString(dr["DocumentoID"]);
                //this.CodigoDoc.Value = Convert.ToString(dr["CodigoDoc"]);
                //this.Descriptivo.Value = Convert.ToString(dr["Descriptivo"]);
                this.NombreArchivo.Value = Convert.ToString(dr["NombreArchivo"]);
                //this.TipoArchivo.Value = Convert.ToByte(dr["TipoArchivo"]);
                this.SeparadorCampo.Value = Convert.ToByte(dr["SeparadorCampo"]);
                if (!string.IsNullOrEmpty(dr["PrimerRegDetalleLinea"].ToString()))
                    this.PrimerRegDetalleLinea.Value = Convert.ToByte(dr["PrimerRegDetalleLinea"]);
                if (!string.IsNullOrEmpty(dr["PrimerRegDetalleTipoCpo"].ToString()))
                    this.PrimerRegDetalleTipoCpo.Value = Convert.ToByte(dr["PrimerRegDetalleTipoCpo"]);
                if (!string.IsNullOrEmpty(dr["PrimerRegDetalleTexto"].ToString()))
                    this.PrimerRegDetalleTexto.Value = Convert.ToString(dr["PrimerRegDetalleTexto"]);
                if (!string.IsNullOrEmpty(dr["CpoNumeroLogitud"].ToString()))
                    this.CpoNumeroLogitud.Value = Convert.ToByte(dr["CpoNumeroLogitud"]);
                if (!string.IsNullOrEmpty(dr["CpoNumeroEncierra"].ToString()))
                    this.CpoNumeroEncierra.Value = Convert.ToByte(dr["CpoNumeroEncierra"]);
                this.CpoNumeroJustifica.Value = Convert.ToByte(dr["CpoNumeroJustifica"]);
                this.CpoNumeroCerosInd.Value = Convert.ToBoolean(dr["CpoNumeroCerosInd"]);
                this.CpoNumeroMilSepara.Value = Convert.ToByte(dr["CpoNumeroMilSepara"]);
                if (!string.IsNullOrEmpty(dr["CpoValorLogitud"].ToString()))
                    this.CpoValorLogitud.Value = Convert.ToByte(dr["CpoValorLogitud"]);
                if (!string.IsNullOrEmpty(dr["CpoValorEncierra"].ToString()))
                    this.CpoValorEncierra.Value = Convert.ToByte(dr["CpoValorEncierra"]);
                if (!string.IsNullOrEmpty(dr["CpoValorCaracterEspecial"].ToString()))
                    this.CpoValorCaracterEspecial.Value = Convert.ToString(dr["CpoValorCaracterEspecial"]);
                this.CpoValorJustifica.Value = Convert.ToByte(dr["CpoValorJustifica"]);
                this.CpoValorCerosInd.Value = Convert.ToBoolean(dr["CpoValorCerosInd"]);
                this.CpoValorDecimales.Value = Convert.ToByte(dr["CpoValorDecimales"]);
                this.CpoValorDecimalSepara.Value = Convert.ToByte(dr["CpoValorDecimalSepara"]);
                this.CpoValorMilSepara.Value = Convert.ToByte(dr["CpoValorMilSepara"]);
                if (!string.IsNullOrEmpty(dr["CpoTextoLogitud"].ToString()))
                    this.CpoTextoLogitud.Value = Convert.ToByte(dr["CpoTextoLogitud"]);
                if (!string.IsNullOrEmpty(dr["CpoTextoEncierra"].ToString()))
                    this.CpoTextoEncierra.Value = Convert.ToByte(dr["CpoTextoEncierra"]);
                this.CpoTextoJustifica.Value = Convert.ToByte(dr["CpoTextoJustifica"]);
                this.CpoTextoMinusculasInd.Value = Convert.ToBoolean(dr["CpoTextoMinusculasInd"]);
                if (!string.IsNullOrEmpty(dr["CpoFechaEncierra"].ToString()))
                    this.CpoFechaEncierra.Value = Convert.ToByte(dr["CpoFechaEncierra"]);
                this.CpoFechaTipo.Value = Convert.ToByte(dr["CpoFechaTipo"]);
                this.CpoFechaSeparador.Value = Convert.ToByte(dr["CpoFechaSeparador"]);
                this.CpoFechaAno.Value = Convert.ToByte(dr["CpoFechaAno"]);
                this.CpoFechaMes.Value = Convert.ToByte(dr["CpoFechaMes"]);
                this.RegistroInicialnd.Value = Convert.ToBoolean(dr["RegistroInicialnd"]);
                this.RegistroFinallnd.Value = Convert.ToBoolean(dr["RegistroFinallnd"]);
                this.RegistroDetalle1lnd.Value = Convert.ToBoolean(dr["RegistroDetalle1lnd"]);
                
            }
            catch (Exception e)
            {
               throw e;
            }         
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glDocMigracionEstructura()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.DocumentoID = new UDT_BasicID();
            this.DocumentoDesc = new UDT_Descriptivo();
            //this.CodigoDoc = new UDT_CodigoGrl5();
            //this.Descriptivo = new UDT_DescripTBase();
            this.NombreArchivo = new UDT_CodigoGrl20();
            //this.TipoArchivo = new UDTSQL_tinyint();
            this.SeparadorCampo = new UDTSQL_tinyint();
            this.PrimerRegDetalleLinea = new UDTSQL_tinyint();
            this.PrimerRegDetalleTipoCpo = new UDTSQL_tinyint();
            this.PrimerRegDetalleTexto = new UDT_CodigoGrl20();
            this.CpoNumeroLogitud = new UDTSQL_tinyint();
            this.CpoNumeroEncierra = new UDTSQL_tinyint();
            this.CpoNumeroJustifica = new UDTSQL_tinyint();
            this.CpoNumeroCerosInd = new UDT_SiNo();
            this.CpoNumeroMilSepara = new UDTSQL_tinyint();
            this.CpoValorLogitud = new UDTSQL_tinyint();
            this.CpoValorEncierra = new UDTSQL_tinyint();
            this.CpoValorCaracterEspecial = new UDTSQL_char(1);
            this.CpoValorJustifica = new UDTSQL_tinyint();
            this.CpoValorCerosInd = new UDT_SiNo();
            this.CpoValorDecimales = new UDTSQL_tinyint();
            this.CpoValorDecimalSepara = new UDTSQL_tinyint();
            this.CpoValorMilSepara = new UDTSQL_tinyint();
            this.CpoTextoLogitud = new UDTSQL_tinyint();
            this.CpoTextoEncierra = new  UDTSQL_tinyint();
            this.CpoTextoJustifica = new UDTSQL_tinyint();
            this.CpoTextoMinusculasInd = new UDT_SiNo();
            this.CpoFechaEncierra = new UDTSQL_tinyint();
            this.CpoFechaTipo = new UDTSQL_tinyint();
            this.CpoFechaSeparador = new UDTSQL_tinyint();
            this.CpoFechaAno = new UDTSQL_tinyint();
            this.CpoFechaMes = new UDTSQL_tinyint();
            this.RegistroInicialnd = new UDT_SiNo();
            this.RegistroFinallnd = new UDT_SiNo();
            this.RegistroDetalle1lnd = new UDT_SiNo();
        }

        public DTO_glDocMigracionEstructura(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_glDocMigracionEstructura(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_BasicID DocumentoID { get; set; }

        [DataMember]
        public UDT_Descriptivo DocumentoDesc { get; set; }
                
        //[DataMember]
        //public UDT_CodigoGrl5 CodigoDoc { get; set; }

        //[DataMember]
        //public UDT_DescripTBase Descriptivo { get; set; }
        
        [DataMember]
        public UDT_CodigoGrl20 NombreArchivo { get; set; }
        
        //[DataMember]
        //public UDTSQL_tinyint TipoArchivo { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint SeparadorCampo { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint PrimerRegDetalleLinea { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint PrimerRegDetalleTipoCpo { get; set; }
        
        [DataMember]
        public UDT_CodigoGrl20 PrimerRegDetalleTexto { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint CpoNumeroLogitud { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint CpoNumeroEncierra { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint CpoNumeroJustifica { get; set; }
        
        [DataMember]
        public UDT_SiNo CpoNumeroCerosInd { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint CpoNumeroMilSepara { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint CpoValorLogitud { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint CpoValorEncierra { get; set; }
        
        [DataMember]
        public UDTSQL_char CpoValorCaracterEspecial { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint CpoValorJustifica { get; set; }
        
        [DataMember]
        public UDT_SiNo CpoValorCerosInd { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint CpoValorDecimales { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint CpoValorDecimalSepara { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint CpoValorMilSepara { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint CpoTextoLogitud { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint CpoTextoEncierra { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint CpoTextoJustifica { get; set; }
        
        [DataMember]
        public UDT_SiNo CpoTextoMinusculasInd { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint CpoFechaEncierra { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint CpoFechaTipo { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint CpoFechaSeparador { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint CpoFechaAno { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint CpoFechaMes { get; set; }
        
        [DataMember]
        public UDT_SiNo RegistroInicialnd { get; set; }
        
        [DataMember]
        public UDT_SiNo RegistroFinallnd { get; set; }
        
        [DataMember]
        public UDT_SiNo RegistroDetalle1lnd { get; set; }

    }

}
