using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// 
    /// Models DTO_pyActaTrabajoDeta
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pyActaTrabajoDeta
    {
        #region DTO_pyActaTrabajoDeta

        public DTO_pyActaTrabajoDeta(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.ConsOrdCompraDeta.Value = Convert.ToInt32(dr["ConsOrdCompraDeta"]);
                this.ConsProyMvto.Value = Convert.ToInt32(dr["ConsProyMvto"]);
                this.NumDocProyecto.Value = Convert.ToInt32(dr["NumDocProyecto"]);
                this.NumDocOrdCompra.Value = Convert.ToInt32(dr["NumDocOrdCompra"]);             
                this.CantidadPend.Value = Convert.ToInt32(dr["CantidadPend"]);
                this.CantidadREC.Value = Convert.ToDecimal(dr["CantidadREC"]);
                this.Observaciones.Value = Convert.ToString(dr["Observaciones"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
                if (!string.IsNullOrEmpty(dr["NumDocRecibido"].ToString()))
                    this.NumDocRecibido.Value = Convert.ToInt32(dr["NumDocRecibido"]);
                if (!string.IsNullOrEmpty(dr["ValorUniREC"].ToString()))
                    this.ValorUniREC.Value = Convert.ToDecimal(dr["ValorUniREC"]);
                this.MonedaOrdCompra.Value = Convert.ToString(dr["MonedaOrdCompra"]);
                //Adicionales
                this.TareaCliente.Value = Convert.ToString(dr["TareaCliente"]);
                this.RecursoID.Value = Convert.ToString(dr["Consecutivo"]);
                this.RecursoID.Value = Convert.ToString(dr["RecursoID"]);
                this.RecursoDesc.Value = Convert.ToString(dr["RecursoDesc"]);
                this.ProveedorID.Value = Convert.ToString(dr["ProveedorID"]);
                this.UnidadInvID.Value = Convert.ToString(dr["UnidadInvID"]);
                if (!string.IsNullOrEmpty(dr["TipoRecurso"].ToString()))
                    this.TipoRecurso.Value = Convert.ToByte(dr["TipoRecurso"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pyActaTrabajoDeta()
        {
            this.InitCols();
            this.CantidadPend.Value = 0;
            this.CantidadREC.Value = 0;
        }

        public void InitCols()
        {             
            this.NumeroDoc = new UDT_Consecutivo();
            this.ConsOrdCompraDeta = new UDT_Consecutivo();
            this.ConsProyMvto = new UDT_Consecutivo();
            this.NumDocProyecto = new UDT_Consecutivo();
            this.NumDocOrdCompra = new UDT_Consecutivo();
            this.NumDocRecibido = new UDT_Consecutivo();
            this.CantidadPend = new UDT_Cantidad();
            this.CantidadREC = new UDT_Cantidad();
            this.Observaciones = new UDT_DescripTExt();     
            this.Consecutivo = new UDT_Consecutivo();
            this.ValorUniREC = new UDT_Valor();
            this.MonedaOrdCompra = new UDT_MonedaID();

            //Adicionales
            this.TareaCliente = new UDT_DescripTBase();
            this.ConsecTarea = new UDT_Consecutivo();
            this.RecursoID = new UDT_CodigoGrl();
            this.RecursoDesc = new UDT_Descriptivo();
            this.ProveedorID = new UDT_DescripTExt();
            this.UnidadInvID = new UDT_UnidadInvID();
            this.TipoRecurso = new UDTSQL_tinyint();
            this.DocumentoNro = new UDTSQL_int();
            this.Estado = new UDTSQL_smallint();
            this.CantidadOC = new UDT_Cantidad();
            this.ConsActaProy = new UDTSQL_int();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_Consecutivo ConsOrdCompraDeta { get; set; }

        [DataMember]
        public UDT_Consecutivo ConsProyMvto { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocProyecto { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocOrdCompra { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocRecibido { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadPend { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadREC { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

         [DataMember]
        public UDT_DescripTExt Observaciones { get; set; }

        [DataMember]
        public UDT_MonedaID MonedaOrdCompra { get; set; }

        [DataMember]
        public UDT_Valor ValorUniREC { get; set; }

        //Adicionales

        [DataMember]
         public UDT_DescripTBase TareaCliente { get; set; }

         [DataMember]
         public UDT_Consecutivo ConsecTarea { get; set; }

        [DataMember]
         public UDT_CodigoGrl RecursoID { get; set; }

         [DataMember]
         public UDT_Descriptivo RecursoDesc { get; set; }

         [DataMember]
         public UDT_DescripTExt ProveedorID { get; set; }

         [DataMember]
         public UDT_UnidadInvID UnidadInvID { get; set; }

         [DataMember]
         public UDTSQL_tinyint TipoRecurso { get; set; }

        [DataMember]
        public string PrefDocOC { get; set; }

        [DataMember]
        public UDTSQL_int DocumentoNro { get; set; }

        [DataMember]
        public UDTSQL_smallint Estado { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadOC { get; set; }

        [DataMember]
        public UDTSQL_int ConsActaProy { get; set; }

        #endregion
    }
}
