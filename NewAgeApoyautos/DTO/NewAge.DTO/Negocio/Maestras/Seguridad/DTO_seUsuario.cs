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
    /// Models DTO_seUsuario
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_seUsuario : DTO_MasterBasic
    {
        #region DTO_seUsuario

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_seUsuario(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.IdiomaDesc.Value = dr["IdiomaDesc"].ToString();
                    this.EmpresaPrefDesc.Value = dr["EmpresaPrefDesc"].ToString();
                    this.EmpresaDesc.Value = dr["EmpresaDesc"].ToString();
                    this.AreaFuncionalDesc.Value = dr["AreaFuncionalDesc"].ToString();
                    this.SeccionFuncionalDesc.Value = dr["SeccionFuncionalDesc"].ToString();
                    this.HorarioDesc.Value = dr["HorarioDesc"].ToString();
                }

                this.IdiomaID.Value = Convert.ToString(dr["IdiomaID"]);
                this.EmpresaID.Value = Convert.ToString(dr["EmpresaID"]);
                this.EmpresaIDPref.Value = Convert.ToString(dr["EmpresaIDPref"]);
                this.AreaFuncionalID.Value = Convert.ToString(dr["AreaFuncionalID"]);
                if (!string.IsNullOrWhiteSpace(dr["SeccionFuncionalID"].ToString()))
                    this.SeccionFuncionalID.Value = Convert.ToString(dr["SeccionFuncionalID"]);
                this.ConexionInd.Value = Convert.ToBoolean(dr["ConexionInd"]);
                if (!string.IsNullOrWhiteSpace(dr["ConexionFec"].ToString()))
                    this.ConexionFec.Value = Convert.ToDateTime(dr["ConexionFec"]);
                this.CorreoElectronico.Value = Convert.ToString(dr["CorreoElectronico"]);
               
                //Ahora se hace en una consulta aparte que llena con getbytes
                //this.Contrasena.Value = Convert.ToInt16(dr["Contrasena"]);
                if (!string.IsNullOrWhiteSpace(dr["ContrasenaRep"].ToString()))
                    this.ContrasenaRep.Value = Convert.ToInt16(dr["ContrasenaRep"]);

                if (!string.IsNullOrWhiteSpace(dr["FechaUltimaDig"].ToString()))
                    this.FechaUltimaDig.Value = Convert.ToDateTime(dr["FechaUltimaDig"]);
                if (!string.IsNullOrWhiteSpace(dr["ContrasenaFecCambio"].ToString()))
                    this.ContrasenaFecCambio.Value = Convert.ToDateTime(dr["ContrasenaFecCambio"]);
                this.ResponsableTercerosInd.Value = Convert.ToBoolean(dr["ResponsableTercerosInd"]);
                if (!string.IsNullOrWhiteSpace(dr["UsuarioDelegado"].ToString()))
                    this.UsuarioDelegado.Value = dr["UsuarioDelegado"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["FechaDelegaINI"].ToString()))
                    this.FechaDelegaINI.Value = Convert.ToDateTime(dr["FechaDelegaINI"].ToString());
                if (!string.IsNullOrWhiteSpace(dr["FechaDelegaFIN"].ToString()))
                    this.FechaDelegaFIN.Value = Convert.ToDateTime(dr["FechaDelegaFIN"].ToString());
                if (!string.IsNullOrWhiteSpace(dr["DelegacionActivaInd"].ToString()))
                    this.DelegacionActivaInd.Value = Convert.ToBoolean(dr["DelegacionActivaInd"]);
                this.HorarioID.Value = dr["HorarioID"].ToString();
                this.Telefono.Value = dr["Telefono"].ToString();

            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_seUsuario()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.IdiomaID = new UDT_BasicID();
            this.IdiomaDesc = new UDT_Descriptivo();
            this.EmpresaIDPref = new UDT_BasicID();
            this.EmpresaPrefDesc = new UDT_Descriptivo();
            this.EmpresaID = new UDT_BasicID();
            this.EmpresaDesc = new UDT_Descriptivo();
            this.AreaFuncionalID = new UDT_BasicID();
            this.AreaFuncionalDesc = new UDT_Descriptivo();
            this.SeccionFuncionalID = new UDT_BasicID();
            this.SeccionFuncionalDesc = new UDT_Descriptivo();
            this.ConexionInd = new UDT_SiNo();
            this.ConexionFec = new UDTSQL_datetime();
            this.CorreoElectronico = new UDT_DescripTBase();
            //this.Contrasena = new  byte[]();
            this.ContrasenaRep = new UDTSQL_smallint();
            this.FechaUltimaDig = new UDTSQL_datetime();
            this.ContrasenaFecCambio= new UDTSQL_datetime();
            this.ResponsableTercerosInd = new UDT_SiNo();
            this.UsuarioDelegado = new UDT_UsuarioID();
            this.FechaDelegaINI = new UDTSQL_smalldatetime();
            this.FechaDelegaFIN = new UDTSQL_smalldatetime();
            this.DelegacionActivaInd = new UDT_SiNo();
            this.HorarioID = new UDT_BasicID();
            this.HorarioDesc = new UDT_Descriptivo();
            this.Telefono = new UDT_DescripTBase();
        }

        public DTO_seUsuario(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_seUsuario(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID IdiomaID { get; set; }

        [DataMember]
        public UDT_Descriptivo IdiomaDesc { get; set; }

        [DataMember]
        public UDT_BasicID EmpresaIDPref { get; set; }

        [DataMember]
        public UDT_Descriptivo EmpresaPrefDesc { get; set; }

        [DataMember]
        public UDT_BasicID EmpresaID { get; set; }

        [DataMember]
        public UDT_Descriptivo EmpresaDesc { get; set; }

        [DataMember]
        public UDT_BasicID AreaFuncionalID { get; set; }

        [DataMember]
        public UDT_Descriptivo AreaFuncionalDesc { get; set; }

        [DataMember]
        public UDT_BasicID SeccionFuncionalID { get; set; }

        [DataMember]
        public UDT_Descriptivo SeccionFuncionalDesc { get; set; }

        [DataMember]
        public UDT_SiNo ConexionInd { get; set; }

        [DataMember]
        public UDTSQL_datetime ConexionFec { get; set; }

        [DataMember]
        public UDT_DescripTBase CorreoElectronico { get; set; }

        [DataMember]
        public byte[] Contrasena { get; set; }

        [DataMember]
        public UDTSQL_smallint ContrasenaRep { get; set; }

        [DataMember]
        public UDTSQL_datetime ContrasenaFecCambio { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaUltimaDig { get; set; }

        [DataMember]
        public UDT_SiNo ResponsableTercerosInd { get; set; }

        [DataMember]
        public UDT_UsuarioID UsuarioDelegado { get; set; }

                [DataMember]
        public UDT_Descriptivo UsuarioDesc { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaDelegaINI { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaDelegaFIN { get; set; }

        [DataMember]
        public UDT_SiNo DelegacionActivaInd { get; set; }

        [DataMember]
        public UDT_BasicID HorarioID { get; set; }

        [DataMember]
        public UDT_Descriptivo HorarioDesc { get; set; }

        [DataMember]
        public UDT_DescripTBase Telefono { get; set; }


        //Datos extras

        [DataMember]
        public string ContrasenaLimpia { get; set; }

    }

}