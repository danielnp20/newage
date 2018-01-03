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
    /// Models DTO_acContabiliza
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_acContabiliza : DTO_MasterComplex
    {
        #region DTO_acContabiliza

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_acContabiliza(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.ActivoClaseDesc.Value = dr["ActivoClaseDesc"].ToString();
                    this.CuentaDesc.Value = dr["CuentaDesc"].ToString();
                    this.ClaseBSDesc.Value = dr["ClaseBSDesc"].ToString();
                }

                this.ActivoClaseID.Value = dr["ActivoClaseID"].ToString();
                this.ComponenteActivoID.Value = dr["ComponenteActivoID"].ToString();
                this.ComponenteActivoDesc.Value = dr["ComponenteActivoDesc"].ToString();
                this.CuentaID.Value = dr["CuentaID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["ClaseBSID"].ToString()))
                    this.ClaseBSID.Value = dr["ClaseBSID"].ToString();
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_acContabiliza()
            : base()
        {
            InitCols();
        }

        /// <summary>
        /// Construye el DTO_ a partir de una consulta hecha en la bd
        /// </summary>
        /// <param name="dr">IDataReader</param>
        public DTO_acContabiliza(IDataReader dr)
        {
            InitCols();
            try
            {
                this.ActivoClaseID.Value = dr["ActivoClaseID"].ToString();
                this.ComponenteActivoID.Value = dr["ComponenteActivoID"].ToString();
                this.CuentaID.Value = dr["CuentaID"].ToString();
                this.ClaseBSID.Value = dr["ClaseBSID"].ToString();
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ActivoClaseID = new UDT_BasicID();
            this.ActivoClaseDesc = new UDT_Descriptivo();
            this.ComponenteActivoID = new UDT_BasicID();
            this.ComponenteActivoDesc = new UDT_Descriptivo();
            this.CuentaID = new UDT_BasicID();
            this.CuentaDesc = new UDT_Descriptivo();
            this.ClaseBSID = new UDT_BasicID();
            this.ClaseBSDesc = new UDT_Descriptivo();
        }

        public DTO_acContabiliza(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_acContabiliza(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID ActivoClaseID { get; set; }

        [DataMember]
        public UDT_Descriptivo ActivoClaseDesc { get; set; }

        [DataMember]
        public UDT_BasicID ComponenteActivoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ComponenteActivoDesc { get; set; }
        
        [DataMember]
        public UDT_BasicID CuentaID { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaDesc { get; set; }

        [DataMember]
        public UDT_BasicID ClaseBSID { get; set; }

        [DataMember]
        public UDT_Descriptivo ClaseBSDesc { get; set; }

    }
  }