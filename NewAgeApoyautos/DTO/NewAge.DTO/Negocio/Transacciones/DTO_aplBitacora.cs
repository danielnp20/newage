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
    /// <summary>
    /// Class Error:
    /// Models DTO_aplBitacora
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_aplBitacora
    {
        #region Contructora

        /// <summary>
        /// Constructor
        /// </summary>
        public DTO_aplBitacora(IDataReader dr)
        {
            try
            {
                this.InitCols();
                this.BitacoraID.Value = Convert.ToInt32(dr["BitacoraID"]);
                this.EmpresaID.Value = Convert.ToString(dr["EmpresaID"]);
                this.DocumentoID.Value = Convert.ToInt32(dr["DocumentoID"]);
                this.AccionID.Value = Convert.ToInt16(dr["AccionID"]);
                this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                this.seUsuarioID.Value = Convert.ToInt32(dr["seUsuarioID"]);
                this.llp01.Value = dr["llp01"].ToString();
                this.llp02.Value = dr["llp02"].ToString();
                this.llp03.Value = dr["llp03"].ToString();
                this.llp04.Value = dr["llp04"].ToString();
                this.BitacoraOrigenID.Value = Convert.ToInt32(dr["BitacoraOrigenID"]);
                this.BitacoraPadreID.Value = Convert.ToInt32(dr["BitacoraPadreID"]);
                if (!string.IsNullOrWhiteSpace(dr["BitacoraAnulacionID"].ToString()))
                    this.BitacoraAnulacionID.Value = Convert.ToInt32(dr["BitacoraAnulacionID"]);
                this.Usuario.Value = dr["UsuarioDesc"].ToString();
                this.Empresa.Value = dr["EmpresaDesc"].ToString();
                this.Documento.Value = dr["DocumentoDesc"].ToString();
                //this.Accion.Value = dr["AccionDesc"].ToString();
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_aplBitacora()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.BitacoraID = new UDT_BitacoraID();
            this.EmpresaID = new UDT_EmpresaID();
            this.DocumentoID = new UDT_DocumentoID();
            this.AccionID = new UDT_AccionID();
            this.Fecha = new UDTSQL_datetime();
            this.seUsuarioID = new UDT_seUsuarioID();
            this.llp01 = new UDTSQL_varchar(50);
            this.llp02 = new UDTSQL_varchar(50);
            this.llp03 = new UDTSQL_varchar(50);
            this.llp04 = new UDTSQL_varchar(50);
            this.BitacoraOrigenID = new UDT_BitacoraID();
            this.BitacoraPadreID = new UDT_BitacoraID();
            this.BitacoraAnulacionID = new UDT_BitacoraID();
            this.Actualizaciones = new List<DTO_aplBitacoraAct>();
            this.Usuario = new UDT_DescripTBase();
            this.Documento = new UDT_DescripTBase();
            this.Empresa = new UDT_DescripTBase();
            this.Accion = new UDT_DescripTBase();
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Gets or sets the BitacoraID
        /// </summary>
        [DataMember]
        public UDT_BitacoraID BitacoraID { get; set; }

        /// <summary>
        /// Gets or sets the EmpresaID
        /// </summary>
        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        /// <summary>
        /// Gets or sets the DocumentoID
        /// </summary>
        [DataMember]
        public UDT_DocumentoID DocumentoID { get; set; }

        /// <summary>
        /// Gets or sets the AccionID
        /// </summary>
        [DataMember]
        public UDT_AccionID AccionID { get; set; }

        /// <summary>
        /// Gets or sets the Fecha
        /// </summary>
        [DataMember]
        public UDTSQL_datetime Fecha { get; set; }

        /// <summary>
        /// Gets or sets the seUsuarioID
        /// </summary>
        [DataMember]
        public UDT_seUsuarioID seUsuarioID { get; set; }

        /// <summary>
        /// Gets or sets the llp01
        /// </summary>
        [DataMember]
        public UDTSQL_varchar llp01 { get; set; }

        /// <summary>
        /// Gets or sets the llp02
        /// </summary>
        [DataMember]
        public UDTSQL_varchar llp02 { get; set; }

        /// <summary>
        /// Gets or sets the llp03
        /// </summary>
        [DataMember]
        public UDTSQL_varchar llp03 { get; set; }

        /// <summary>
        /// Gets or sets the llp04
        /// </summary>
        [DataMember]
        public UDTSQL_varchar llp04 { get; set; }

        /// <summary>
        /// Gets or sets the llp05
        /// </summary>
        [DataMember]
        public UDTSQL_varchar llp05 { get; set; }

        /// <summary>
        /// Gets or sets the llp06
        /// </summary>
        [DataMember]
        public UDTSQL_varchar llp06 { get; set; }

        /// <summary>
        /// Gets or sets the BitacoraOrigenID
        /// </summary>
        [DataMember]
        public UDT_BitacoraID BitacoraOrigenID { get; set; }

        /// <summary>
        /// Gets or sets the BitacoraPadreID
        /// </summary>
        [DataMember]
        public UDT_BitacoraID BitacoraPadreID { get; set; }

        /// <summary>
        /// Gets or sets the BitacoraAnulacionID
        /// </summary>
        [DataMember]
        //public Nullable<UDT_BitacoraID> BitacoraAnulacionID
        public UDT_BitacoraID BitacoraAnulacionID { get; set; }

        /// <summary>
        /// Gets or sets the BitacoraAnulacionID
        /// </summary>
        [DataMember]
        //public Nullable<UDT_BitacoraID> BitacoraAnulacionID
        public List<DTO_aplBitacoraAct> Actualizaciones { get; set;}

        #endregion

        #region Propiedades Descripciones

        //Login del usuario
        [DataMember]
        public UDT_DescripTBase Usuario { get; set; }

        //Nombre del documento
        [DataMember]
        public UDT_DescripTBase Documento { get; set; }

        //Empresa
        [DataMember]
        public UDT_DescripTBase Empresa { get; set; }

        //Accion realizada
        [DataMember]
        public UDT_DescripTBase Accion { get; set; }

        #endregion
    }
}
