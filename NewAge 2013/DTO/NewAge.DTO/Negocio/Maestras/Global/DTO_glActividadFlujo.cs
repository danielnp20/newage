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
    /// Models DTO_glActividadFlujo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glActividadFlujo : DTO_MasterBasic
    {
        #region DTO_glActividadFlujo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glActividadFlujo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.ProcedimientoDesc.Value = dr["ProcedimientoDesc"].ToString();
                    this.EtapaIncumplimientoDesc.Value = Convert.ToString(dr["EtapaIncumplimientoDesc"]);
                    this.DocumentoDesc.Value = dr["DocumentoDesc"].ToString();
                    this.LLamadaDesc.Value = dr["LLamadaDesc"].ToString();
                    this.ActividadReactivaDesc.Value = dr["ActividadReactivaDesc"].ToString();



                    this.ModuloDesc.Value = dr["ModuloDesc"].ToString();
                    this.seUsuarioDesc.Value = dr["seUsuarioDesc"].ToString();
                    this.seUsuarioID.Value = dr["UsuarioID1"].ToString();
                }
                else
                    this.seUsuarioID.Value = dr["seUsuarioID"].ToString();


                this.ProcedimientoID.Value = dr["ProcedimientoID"].ToString();
                this.EtapaIncumplimiento.Value = Convert.ToString(dr["EtapaIncumplimiento"]);
                this.UnidadTiempo.Value = Convert.ToByte(dr["UnidadTiempo"]);
                if (!string.IsNullOrWhiteSpace(dr["TiempoTermina"].ToString()))
                    this.TiempoTermina.Value = Convert.ToInt32(dr["TiempoTermina"]);
                if (!string.IsNullOrWhiteSpace(dr["AlarmaPeriodo1"].ToString()))
                    this.AlarmaPeriodo1.Value = Convert.ToInt32(dr["AlarmaPeriodo1"]);
                if (!string.IsNullOrWhiteSpace(dr["AlarmaPeriodo2"].ToString()))
                    this.AlarmaPeriodo2.Value = Convert.ToInt32(dr["AlarmaPeriodo2"]);
                if (!string.IsNullOrWhiteSpace(dr["AlarmaPeriodo3"].ToString()))
                    this.AlarmaPeriodo3.Value = Convert.ToInt32(dr["AlarmaPeriodo3"]);
                if (!string.IsNullOrWhiteSpace(dr["Alarma1Ind"].ToString()))
                    this.Alarma1Ind.Value = Convert.ToBoolean(dr["Alarma1Ind"]);
                if (!string.IsNullOrWhiteSpace(dr["Alarma2Ind"].ToString()))
                    this.Alarma2Ind.Value = Convert.ToBoolean(dr["Alarma2Ind"]);
                if (!string.IsNullOrWhiteSpace(dr["Alarma3Ind"].ToString()))
                    this.Alarma3Ind.Value = Convert.ToBoolean(dr["Alarma3Ind"]);
                if (!string.IsNullOrWhiteSpace(dr["Prioritaria"].ToString()))
                    this.Prioritaria.Value = Convert.ToBoolean(dr["Prioritaria"]);
                if (!string.IsNullOrWhiteSpace(dr["FinalizaProcesoInd"].ToString()))
                    this.FinalizaProcesoInd.Value = Convert.ToBoolean(dr["FinalizaProcesoInd"]);
                this.TipoControl.Value = Convert.ToByte(dr["TipoControl"]);
                this.TipoActividad.Value = Convert.ToByte(dr["TipoActividad"]);
                this.DocumentoID.Value = dr["DocumentoID"].ToString();
                this.LLamadaID.Value = dr["LLamadaID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["TipoReferencia"].ToString()))
                    this.TipoReferencia.Value = Convert.ToByte(dr["TipoReferencia"]);
                if (!string.IsNullOrWhiteSpace(dr["CorreoInd"].ToString()))
                    this.CorreoInd.Value = Convert.ToBoolean(dr["CorreoInd"]);
                if (!string.IsNullOrWhiteSpace(dr["CartaInd"].ToString()))
                    this.CartaInd.Value = Convert.ToBoolean(dr["CartaInd"]);
                this.TextoCorreo.Value = dr["TextoCorreo"].ToString();
                this.PlantillaCarta.Value = dr["PlantillaCarta"].ToString();
                this.ActividadReactiva.Value = dr["ActividadReactiva"].ToString();
                this.SistemaInd.Value = Convert.ToBoolean(dr["SistemaInd"]);
                this.AutomaticaInd.Value = Convert.ToBoolean(dr["AutomaticaInd"]);
                this.ModuloID.Value = dr["ModuloID"].ToString();                
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glActividadFlujo()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ProcedimientoID = new UDT_BasicID();
            this.ProcedimientoDesc = new UDT_Descriptivo();
            this.EtapaIncumplimiento = new UDT_BasicID();
            this.EtapaIncumplimientoDesc = new UDT_Descriptivo();
            this.UnidadTiempo = new UDTSQL_tinyint();
            this.TiempoTermina = new UDTSQL_int();
            this.AlarmaPeriodo1 = new UDTSQL_int();
            this.AlarmaPeriodo2 = new UDTSQL_int();
            this.AlarmaPeriodo3 = new UDTSQL_int();
            this.Alarma1Ind = new UDT_SiNo();
            this.Alarma2Ind = new UDT_SiNo();
            this.Alarma3Ind = new UDT_SiNo();
            this.Prioritaria = new UDT_SiNo();
            this.FinalizaProcesoInd = new UDT_SiNo();
            this.TipoControl = new UDTSQL_tinyint();
            this.TipoActividad = new UDTSQL_tinyint();
            this.DocumentoID = new UDT_BasicID();
            this.DocumentoDesc = new UDT_Descriptivo();
            this.LLamadaID = new UDT_BasicID();
            this.LLamadaDesc = new UDT_Descriptivo();
            this.TipoReferencia = new UDTSQL_tinyint();
            this.CorreoInd = new UDT_SiNo();
            this.CartaInd = new UDT_SiNo();
            this.TextoCorreo = new UDT_DescripTExt();
            this.PlantillaCarta = new UDT_DescripTBase();
            this.ActividadReactiva = new UDT_BasicID();
            this.ActividadReactivaDesc = new UDT_Descriptivo();
            this.SistemaInd = new UDT_SiNo();
            this.AutomaticaInd = new UDT_SiNo();
            this.ModuloID = new UDT_BasicID();
            this.ModuloDesc = new UDT_Descriptivo();
            this.seUsuarioID = new UDT_BasicID();
            this.seUsuarioDesc = new UDT_Descriptivo();
        }

        public DTO_glActividadFlujo(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_glActividadFlujo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        #region Propiedades

        [DataMember]
        public UDT_BasicID DocumentoID { get; set; }

        [DataMember]
        public UDT_Descriptivo DocumentoDesc { get; set; }

        [DataMember]
        public UDT_BasicID ProcedimientoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ProcedimientoDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint UnidadTiempo { get; set; }

        [DataMember]
        public UDTSQL_int TiempoTermina { get; set; }

        [DataMember]
        public UDT_BasicID LLamadaID { get; set; }

        [DataMember]
        public UDT_Descriptivo LLamadaDesc { get; set; }

        [DataMember]
        public UDT_BasicID EtapaIncumplimiento { get; set; }

        [DataMember]
        public UDT_Descriptivo EtapaIncumplimientoDesc { get; set; }

        [DataMember]
        public UDTSQL_int AlarmaPeriodo1 { get; set; }

        [DataMember]
        public UDTSQL_int AlarmaPeriodo2 { get; set; }

        [DataMember]
        public UDTSQL_int AlarmaPeriodo3 { get; set; }

        [DataMember]
        public UDT_SiNo Alarma1Ind { get; set; }

        [DataMember]
        public UDT_SiNo Alarma2Ind { get; set; }

        [DataMember]
        public UDT_SiNo Alarma3Ind { get; set; }

        [DataMember]
        public UDT_SiNo Prioritaria { get; set; }

        [DataMember]
        public UDT_SiNo FinalizaProcesoInd { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoControl { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoActividad { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoReferencia { get; set; }
        
        [DataMember]
        public UDT_SiNo CorreoInd { get; set; }
        
        [DataMember]
        public UDT_SiNo CartaInd { get; set; }

        [DataMember]
        public UDT_DescripTExt TextoCorreo { get; set; }
        
        [DataMember]
        public UDT_DescripTBase PlantillaCarta { get; set; }

        [DataMember]
        public UDT_BasicID ActividadReactiva { get; set; }

        [DataMember]
        public UDT_Descriptivo ActividadReactivaDesc { get; set; }

        [DataMember]
        public UDT_SiNo SistemaInd { get; set; }

        [DataMember]
        public UDT_SiNo AutomaticaInd { get; set; }

        [DataMember]
        public UDT_BasicID ModuloID { get; set; }

        [DataMember]
        public UDT_Descriptivo ModuloDesc { get; set; }

        [DataMember]
        public UDT_BasicID seUsuarioID { get; set; }

        [DataMember]
        public UDT_Descriptivo seUsuarioDesc { get; set; } 

        #endregion
    }
}
