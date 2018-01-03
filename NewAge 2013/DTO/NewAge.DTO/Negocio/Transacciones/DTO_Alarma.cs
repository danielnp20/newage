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
    /// Models DTO_Alarma
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_Alarma : DTO_SerializedObject
    {
        #region Constructor

		/// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_Alarma(IDataReader dr)
        {
            try
            {
                this.NumeroDoc = dr["NumeroDoc"].ToString();
                this.EmpresaID = dr["EmpresaID"].ToString();
                this.Procedimiento = dr["Procedimiento"].ToString();
                this.ActividadFlujoID = dr["ActividadFlujoID"].ToString();
                this.Actividad = dr["Actividad"].ToString();
                this.DocumentoID = dr["DocumentoID"].ToString();
                this.DocumentoDesc = dr["DocumentoDesc"].ToString();
                this.TerceroID = string.IsNullOrWhiteSpace(dr["TerceroID"].ToString()) ? string.Empty : dr["TerceroID"].ToString();
                this.TerceroDesc = string.IsNullOrWhiteSpace(dr["TerceroDesc"].ToString()) ? string.Empty : dr["TerceroDesc"].ToString();
                this.PrefijoID = dr["PrefijoID"].ToString();
                this.Consecutivo = dr["Consecutivo"].ToString();
                this.UsuarioRESPID = dr["UsuarioRESPID"].ToString(); //Deben quedar con este nombre
                this.UsuarioRESP = dr["UsuarioRESP"].ToString(); //Deben quedar con este nombre
                this.UsuarioID1 = dr["AlarmaUsuario1"].ToString();
                this.UsuarioID2 = dr["AlarmaUsuario2"].ToString();
                this.UsuarioID3 = dr["AlarmaUsuario3"].ToString();
            }
            catch (Exception e)
            { ; }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_Alarma(){}

	    #endregion
        
        #region Propiedades

        [DataMember]
        public string NumeroDoc { get; set; }
        
        [DataMember]
        public string EmpresaID { get; set; }

        [DataMember]
        public string Procedimiento { get; set; }

        [DataMember]
        public string Actividad { get; set; }

        [DataMember]
        public string ActividadFlujoID { get; set; }

        [DataMember]
        public string DocumentoID { get; set; }

        [DataMember]
        public string DocumentoDesc { get; set; }
        
        [DataMember]
        public string TerceroID { get; set; }
        
        [DataMember]
        public string TerceroDesc { get; set; }
        
        [DataMember]
        public string PrefijoID { get; set; }
        
        [DataMember]
        public string Consecutivo { get; set; }
        
        [DataMember]
        public string UsuarioRESPID { get; set; }
        
        [DataMember]
        public string UsuarioRESP { get; set; }
        
        [DataMember]
        public string FechaAlarma1 { get; set; }
        
        [DataMember]
        public string FechaAlarma2 { get; set; }
        
        [DataMember]
        public string FechaAlarma3 { get; set; }
        
        [DataMember]
        public string UsuarioID1 { get; set; }
        
        [DataMember]
        public string UsuarioID2 { get; set; }
        
        [DataMember]
        public string UsuarioID3 { get; set; }
        
        [DataMember]
        public string UsuarioMail1 { get; set; }
        
        [DataMember]
        public string UsuarioMail2 { get; set; }
        
        [DataMember]
        public string UsuarioMail3 { get; set; }
        
        [DataMember]
        public string UsuarioLang1 { get; set; }
        
        [DataMember]
        public string UsuarioLang2 { get; set; }
        
        [DataMember]
        public string UsuarioLang3 { get; set; }
        
        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public bool Finaliza { get; set; }

        [DataMember]
        public string ExtraField { get; set; }

        #endregion
    }
}
