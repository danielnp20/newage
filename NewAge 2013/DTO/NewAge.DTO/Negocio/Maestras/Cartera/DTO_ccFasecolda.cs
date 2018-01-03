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
    /// Models DTO_ccFasecolda
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccFasecolda : DTO_MasterBasic
    {
        #region ccFasecolda
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccFasecolda(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.Marca.Value = Convert.ToString(dr["Marca"]);
                this.Clase.Value = Convert.ToString(dr["Clase"]);
                this.Tipo1.Value = Convert.ToString(dr["Tipo1"]);
                this.Tipo2.Value = Convert.ToString(dr["Tipo2"]);
                this.Tipo3.Value = Convert.ToString(dr["Tipo3"]);
                if (!string.IsNullOrEmpty(dr["TipoCaja"].ToString()))
                    this.TipoCaja.Value = Convert.ToByte(dr["TipoCaja"]);
                if (!string.IsNullOrEmpty(dr["Cilindraje"].ToString()))
                    this.Cilindraje.Value = Convert.ToInt32(dr["Cilindraje"]);
                if (!string.IsNullOrEmpty(dr["Servicio"].ToString()))
                    this.Servicio.Value = Convert.ToByte(dr["Servicio"]);
                if (!string.IsNullOrEmpty(dr["Pasajeros"].ToString()))
                    this.Pasajeros.Value = Convert.ToByte(dr["Pasajeros"]);
                if (!string.IsNullOrEmpty(dr["Carga"].ToString()))
                    this.Carga.Value = Convert.ToInt32(dr["Carga"]);
                if (!string.IsNullOrEmpty(dr["Puertas"].ToString()))
                    this.Puertas.Value = Convert.ToByte(dr["Puertas"]);
                if (!string.IsNullOrEmpty(dr["AireAcondicionadoInd"].ToString()))
                    this.AireAcondicionadoInd.Value = Convert.ToBoolean(dr["AireAcondicionadoInd"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>g
        public DTO_ccFasecolda()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Marca = new UDTSQL_char(40);
            this.Clase = new UDTSQL_char(40);
            this.Tipo1 = new UDT_DescripTBase();
            this.Tipo2 = new UDT_DescripTBase();
            this.Tipo3 = new UDT_DescripTBase();

            this.TipoCaja = new UDTSQL_tinyint();
            this.Cilindraje = new UDTSQL_int();
            this.Servicio = new UDTSQL_tinyint();
            this.Pasajeros = new UDTSQL_int();
            this.Carga = new UDTSQL_int();
            this.Puertas = new UDTSQL_tinyint();
            this.AireAcondicionadoInd = new UDT_SiNo();

        }

        public DTO_ccFasecolda(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccFasecolda(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDTSQL_char Marca { get; set; }

        [DataMember]
        public UDTSQL_char Clase { get; set; }

        [DataMember]
        public UDT_DescripTBase Tipo1 { get; set; }

        [DataMember]
        public UDT_DescripTBase Tipo2 { get; set; }

        [DataMember]
        public UDT_DescripTBase Tipo3 { get; set; }
        //
        [DataMember]
        public UDTSQL_tinyint TipoCaja { get; set; }

        [DataMember]
        public UDTSQL_int Cilindraje { get; set; }

        [DataMember]
        public UDTSQL_tinyint Servicio { get; set; }

        [DataMember]
        public UDTSQL_int Pasajeros { get; set; }

        [DataMember]
        public UDTSQL_int Carga { get; set; }

        [DataMember]
        public UDTSQL_tinyint Puertas { get; set; }

        [DataMember]
        public UDT_SiNo AireAcondicionadoInd { get; set; }

    }

}
