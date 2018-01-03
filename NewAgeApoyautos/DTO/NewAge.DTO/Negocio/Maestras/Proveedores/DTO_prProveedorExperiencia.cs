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
    /// Models DTO_prProveedorExperiencia
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prProveedorExperiencia : DTO_MasterComplex
    {
        #region DTO_prProveedorExperiencia
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_prProveedorExperiencia(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.ProveedorDesc.Value = dr["ProveedorDesc"].ToString();
                    this.EmpresaDesc.Value = dr["EmpresaDesc"].ToString();
                }
                this.ProveedorID.Value = dr["ProveedorID"].ToString();
                this.Empresa.Value = dr["Empresa"].ToString();
                if (!string.IsNullOrEmpty(dr["Producto"].ToString()))
                    this.Producto.Value = dr["Producto"].ToString();
                if (!string.IsNullOrEmpty(dr["Valor"].ToString()))
                    this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                if (!string.IsNullOrEmpty(dr["Fecha"].ToString()))
                    this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                if (!string.IsNullOrEmpty(dr["Observaciones"].ToString()))
                    this.Observaciones.Value = dr["Observaciones"].ToString();
                this.TelContacto.Value = dr["TelContacto"].ToString();
                this.Contacto.Value = dr["Contacto"].ToString();
            }
            catch (Exception e)
            {
              throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_prProveedorExperiencia() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ProveedorID = new UDT_BasicID();
            this.ProveedorDesc = new UDT_Descriptivo();
            this.Empresa = new UDT_BasicID();
            this.EmpresaDesc = new UDT_Descriptivo();
            this.Producto = new UDT_DescripTBase();
            this.Valor = new UDT_Valor();
            this.Fecha = new UDTSQL_smalldatetime();
            this.TelContacto = new UDT_DescripTBase();
            this.Contacto = new UDTSQL_char(40);
            this.Observaciones = new UDT_DescripTExt();
        }

        public DTO_prProveedorExperiencia(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_prProveedorExperiencia(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID ProveedorID { get; set; }

        [DataMember]
        public UDT_Descriptivo ProveedorDesc { get; set; }

        [DataMember]
        public UDT_BasicID Empresa { get; set; }

        [DataMember]
        public UDT_Descriptivo EmpresaDesc { get; set; }

        [DataMember]
        public UDT_DescripTBase Producto  { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Fecha { get; set; }

        [DataMember]
        public UDT_DescripTBase TelContacto { get; set; }

        [DataMember]
        public UDTSQL_char Contacto { get; set; }

        [DataMember]
        public UDT_DescripTExt Observaciones { get; set; }
    }
}
