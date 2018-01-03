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
    public class DTO_InfoTarea
    {
        #region Constructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_InfoTarea(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaAlarma1"].ToString()))
                    this.FechaAlarma1.Value = Convert.ToDateTime(dr["FechaAlarma1"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaInicio"].ToString()))
                    this.FechaInicio.Value = Convert.ToDateTime(dr["FechaInicio"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaFin"].ToString()))
                    this.FechaFin.Value = Convert.ToDateTime(dr["FechaFin"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaCerrado"].ToString()))
                    this.FechaCerrado.Value = Convert.ToDateTime(dr["FechaCerrado"]);
                if (!string.IsNullOrWhiteSpace(dr["UnidadTiempo"].ToString()))
                    this.UnidadTiempo.Value = Convert.ToByte(dr["UnidadTiempo"]);
                if (!string.IsNullOrWhiteSpace(dr["CerradoInd"].ToString()))
                    this.CerradoInd.Value = Convert.ToBoolean(dr["CerradoInd"]);
                this.ActividadFlujoID.Value = dr["ActividadFlujoID"].ToString();
                this.ActividadFlujoDesc.Value = dr["ActividadFlujoDesc"].ToString();
                this.TerceroIDActEstado.Value = dr["TerceroIDActEstado"].ToString();
                this.TerceroID.Value = dr["TerceroID"].ToString();              
                this.TerceroDesc.Value = dr["TerceroDesc"].ToString();
                this.DocumentoID.Value = Convert.ToInt32(dr["DocumentoID"]);
                this.DocumentoTipo.Value = Convert.ToByte(dr["DocumentoTipo"]);
                this.PrefijoID.Value = dr["PrefijoID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["DocumentoNro"].ToString()))
                    this.DocumentoNro.Value = Convert.ToInt32(dr["DocumentoNro"]);
                this.DocumentoTercero.Value = dr["DocumentoTercero"].ToString();
                this.UsuarioID.Value = dr["UsuarioID"].ToString();
                this.Observaciones.Value = dr["Observaciones"].ToString();
                this.LlamadaID.Value = dr["LlamadaID"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_InfoTarea()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.FechaAlarma1 = new UDTSQL_datetime();
            this.FechaInicio = new UDTSQL_datetime();
            this.FechaFin = new UDTSQL_datetime();
            this.FechaCerrado = new UDTSQL_datetime();
            this.Incumplimiento = new UDTSQL_int();
            this.UnidadTiempo = new UDTSQL_tinyint();
            this.ActividadFlujoID = new UDT_ActividadFlujoID();
            this.ActividadFlujoDesc = new UDT_Descriptivo();
            this.TerceroID = new UDT_TerceroID();
            this.TerceroIDActEstado = new UDT_TerceroID();
            this.TerceroDesc = new UDT_Descriptivo();
            this.DocumentoID = new UDTSQL_int();
            this.DocumentoTipo = new UDTSQL_tinyint();
            this.PrefDoc = new UDT_DescripTBase();
            this.PrefijoID = new UDT_PrefijoID();
            this.DocumentoNro = new UDT_Consecutivo();
            this.DocumentoTercero = new UDT_DescripTBase();
            this.UsuarioID = new UDT_UsuarioID();
            this.Observaciones = new UDT_DescripTExt();
            this.LlamadaID = new UDT_BasicID();
            this.LlamadaDesc = new UDT_Descriptivo();
            this.CerradoInd = new UDT_SiNo();
            this.UsuarioAlarma1 = new UDT_seUsuarioID();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaAlarma1 { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaInicio { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaFin { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaCerrado { get; set; }

        [DataMember]
        public UDTSQL_int Incumplimiento { get; set; }

        [DataMember]
        public UDTSQL_tinyint UnidadTiempo { get; set; }

        [DataMember]
        public UDT_SiNo CerradoInd { get; set; }

        [DataMember]
        public UDT_ActividadFlujoID ActividadFlujoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ActividadFlujoDesc { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroIDActEstado { get; set; }

        [DataMember]
        public UDT_Descriptivo TerceroDesc { get; set; }

        [DataMember]
        public UDTSQL_int DocumentoID { get; set; }

        [DataMember]
        public UDTSQL_tinyint DocumentoTipo { get; set; }

        [DataMember]
        public UDT_DescripTBase PrefDoc { get; set; }

        [DataMember]
        public UDT_PrefijoID PrefijoID { get; set; }

        [DataMember]
        public UDT_Consecutivo DocumentoNro { get; set; }

        [DataMember]
        public UDT_DescripTBase DocumentoTercero { get; set; }

        [DataMember]
        public UDT_UsuarioID UsuarioID { get; set; }

        [DataMember]
        public UDT_DescripTExt Observaciones { get; set; }

        [DataMember]
        public UDT_BasicID LlamadaID { get; set; }

        [DataMember]
        public UDT_Descriptivo LlamadaDesc { get; set; }

        [DataMember]
        public string VerDoc { get; set; }

        [DataMember]
        public UDT_seUsuarioID UsuarioAlarma1 { get; set; }

        #endregion

    }
}
