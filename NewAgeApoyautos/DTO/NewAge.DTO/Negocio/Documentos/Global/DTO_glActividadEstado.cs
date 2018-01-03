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
    public class DTO_glActividadEstado
    {
        #region Constructor

		/// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glActividadEstado(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.ActividadFlujoID.Value = dr["ActividadFlujoID"].ToString();
                this.seUsuarioID.Value = Convert.ToInt32(dr["seUsuarioID"]);
                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.FechaInicio.Value = Convert.ToDateTime(dr["FechaInicio"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaFin"].ToString()))
                    this.FechaFin.Value = Convert.ToDateTime(dr["FechaFin"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaCerrado"].ToString()))
                    this.FechaCerrado.Value = Convert.ToDateTime(dr["FechaCerrado"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaAlarma1"].ToString()))
                    this.FechaAlarma1.Value = Convert.ToDateTime(dr["FechaAlarma1"]);
                if (!string.IsNullOrWhiteSpace(dr["UsuarioAlarma1"].ToString()))
                    this.UsuarioAlarma1.Value = Convert.ToString(dr["UsuarioAlarma1"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaAlarma2"].ToString()))
                    this.FechaAlarma2.Value = Convert.ToDateTime(dr["FechaAlarma2"]);
                if (!string.IsNullOrWhiteSpace(dr["UsuarioAlarma2"].ToString()))
                    this.UsuarioAlarma2.Value = Convert.ToString(dr["UsuarioAlarma2"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaAlarma3"].ToString()))
                    this.FechaAlarma3.Value = Convert.ToDateTime(dr["FechaAlarma3"]);
                if (!string.IsNullOrWhiteSpace(dr["UsuarioAlarma3"].ToString()))
                    this.UsuarioAlarma3.Value = Convert.ToString(dr["UsuarioAlarma3"]);
                if (!string.IsNullOrWhiteSpace(dr["UsuarioDelegado"].ToString()))
                    this.UsuarioDelegado.Value = Convert.ToString(dr["UsuarioDelegado"]);
                if (!string.IsNullOrWhiteSpace(dr["Observaciones"].ToString()))
                    this.Observaciones.Value = Convert.ToString(dr["Observaciones"]);
                if (!string.IsNullOrWhiteSpace(dr["ActividadBase"].ToString()))
                    this.ActividadBase.Value = Convert.ToString(dr["ActividadBase"]);
                this.CerradoInd.Value = Convert.ToBoolean(dr["CerradoInd"]);
                this.AlarmaInd.Value = Convert.ToBoolean(dr["AlarmaInd"]);
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
        public DTO_glActividadEstado()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.ActividadFlujoID = new UDT_ActividadFlujoID();
            this.ActividadFlujoDesc = new UDT_Descriptivo();
            this.TerceroID = new UDT_TerceroID();
            this.TerceroDesc = new UDT_Descriptivo();
            this.seUsuarioID = new UDT_seUsuarioID();
            this.UsuarioIDDesc = new UDT_UsuarioID();
            this.FechaInicio = new UDTSQL_datetime();
            this.FechaFin = new UDTSQL_datetime();
            this.FechaCerrado = new UDTSQL_datetime();
            this.FechaAlarma1 = new UDTSQL_datetime();
            this.UsuarioAlarma1 = new UDT_UsuarioID();
            this.FechaAlarma2 = new UDTSQL_datetime();
            this.UsuarioAlarma2 = new UDT_UsuarioID();
            this.FechaAlarma3 = new UDTSQL_datetime();
            this.UsuarioAlarma3 = new UDT_UsuarioID();
            this.UsuarioDelegado = new UDT_UsuarioID();
            this.Observaciones = new UDT_DescripTExt();
            this.ActividadBase = new UDT_ActividadFlujoID();
            this.CerradoInd = new UDT_SiNo();
            this.AlarmaInd = new UDT_SiNo();
            this.Consecutivo = new UDT_Consecutivo();
        }

	    #endregion
        
        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_ActividadFlujoID ActividadFlujoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ActividadFlujoDesc { get; set; }

        [DataMember]
        public UDT_seUsuarioID seUsuarioID { get; set; }

        [DataMember]
        public UDT_UsuarioID UsuarioIDDesc { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_Descriptivo TerceroDesc { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaInicio { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaFin { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaCerrado { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaAlarma1 { get; set; }

        [DataMember]
        public UDT_UsuarioID UsuarioAlarma1 { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaAlarma2 { get; set; }

        [DataMember]
        public UDT_UsuarioID UsuarioAlarma2 { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaAlarma3 { get; set; }

        [DataMember]
        public UDT_UsuarioID UsuarioAlarma3 { get; set; }

        [DataMember]
        public UDT_UsuarioID UsuarioDelegado { get; set; }
        
        [DataMember]
        public UDT_DescripTExt Observaciones { get; set; }

        [DataMember]
        public UDT_ActividadFlujoID ActividadBase { get; set; }

        [DataMember]
        public UDT_SiNo CerradoInd { get; set; }

        [DataMember]
        public UDT_SiNo AlarmaInd { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }


        #endregion

    }
}
