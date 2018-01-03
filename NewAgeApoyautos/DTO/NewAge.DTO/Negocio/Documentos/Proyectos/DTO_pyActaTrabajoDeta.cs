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
       #region pySolServicioDeta

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
            //Adicionales
            this.TareaCliente = new UDT_DescripTBase();
            this.ConsecTarea = new UDT_Consecutivo();
            this.RecursoID = new UDT_CodigoGrl();
            this.RecursoDesc = new UDT_Descriptivo();
            this.ProveedorID = new UDT_ProveedorID();
            this.UnidadInvID = new UDT_UnidadInvID();
            this.TipoRecurso = new UDTSQL_tinyint();
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
         public UDT_ProveedorID ProveedorID { get; set; }

         [DataMember]
         public UDT_UnidadInvID UnidadInvID { get; set; }

         [DataMember]
         public UDTSQL_tinyint TipoRecurso { get; set; }

        [DataMember]
        public string PrefDocOC { get; set; }

        #endregion
    }
}
