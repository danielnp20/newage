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
    /// Models DTO_QueryTrazabilidad
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_QueryTrazabilidad
    {
        #region DTO_QueryTrazabilidad

        public DTO_QueryTrazabilidad(IDataReader dr)
        {
            InitCols();
            try
            {
                this.ConsecTarea.Value = Convert.ToInt32(dr["ConsecTarea"]);
                this.ConsecDeta.Value = Convert.ToInt32(dr["ConsecDeta"]);
                this.ConsecMvto.Value = Convert.ToInt32(dr["ConsecMvto"]);
                this.RecursoID.Value = Convert.ToString(dr["RecursoID"]);
                this.RecursoDesc.Value = dr["RecursoDesc"].ToString();
                this.TipoRecurso.Value = Convert.ToByte(dr["TipoRecurso"]);
                this.UnidadInvID.Value = Convert.ToString(dr["UnidadInvID"]);
                this.inReferenciaID.Value = Convert.ToString(dr["inReferenciaID"]);
                this.CodigoBSID.Value = Convert.ToString(dr["CodigoBSID"]);
                this.MarcaInvID.Value = Convert.ToString(dr["MarcaInvID"]);
                this.RefProveedor.Value = Convert.ToString(dr["RefProveedor"]);
                this.TareaCliente.Value = Convert.ToString(dr["TareaCliente"]);
                this.TareaID.Value = Convert.ToString(dr["TareaID"]);
                this.TareaDesc.Value = Convert.ToString(dr["TareaDesc"]);
                this.CantidadTarea.Value = Convert.ToDecimal(dr["CantidadTarea"]);
                this.CantPreproyecto.Value = Convert.ToDecimal(dr["CantPreproyecto"]);
                this.VlrUnitPreproyecto.Value = Convert.ToDecimal(dr["VlrUnitPreproyecto"]);
                this.VlrTotPreproyecto.Value = Convert.ToDecimal(dr["VlrTotPreproyecto"]);
                this.VlrUnitCLIPreproyecto.Value = Convert.ToDecimal(dr["VlrUnitCLIPreproyecto"]);
                this.VlrTotCLIPreproyecto.Value = Convert.ToDecimal(dr["VlrTotCLIPreproyecto"]);
                this.CantPresupuestado.Value = Convert.ToDecimal(dr["CantPresupuestado"]);
                this.VlrPresupuestado.Value = Convert.ToDecimal(dr["VlrPresupuestado"]);
                this.CantSolicitado.Value = Convert.ToDecimal(dr["CantSolicitado"]);
                this.VlrSolicitado.Value = Convert.ToDecimal(dr["VlrSolicitado"]);
                this.CantComprado.Value = Convert.ToDecimal(dr["CantComprado"]);
                this.VlrComprado.Value = Convert.ToDecimal(dr["VlrComprado"]);
                this.CantPreComprado.Value = Convert.ToDecimal(dr["CantPreComprado"]);
                this.VlrPreComprado.Value = Convert.ToDecimal(dr["VlrPreComprado"]);
                this.CantRecibido.Value = Convert.ToDecimal(dr["CantRecibido"]);
                this.VlrRecibido.Value = Convert.ToDecimal(dr["VlrRecibido"]);
                this.CantConsumido.Value = Convert.ToDecimal(dr["CantConsumido"]);
                this.VlrConsumido.Value = Convert.ToDecimal(dr["VlrConsumido"]);
                this.CantFacturado.Value = Convert.ToDecimal(dr["CantFacturado"]);
                this.VlrFacturado.Value = Convert.ToDecimal(dr["VlrFacturado"]);
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
        public DTO_QueryTrazabilidad()
        {
            this.InitCols();
            this.CantidadTarea.Value = 0;
            this.CantPreproyecto.Value = 0;
            this.VlrUnitPreproyecto.Value = 0;
            this.VlrTotPreproyecto.Value = 0;
            this.VlrUnitCLIPreproyecto.Value = 0;
            this.VlrTotCLIPreproyecto.Value = 0;
            this.CantPresupuestado.Value = 0;
            this.VlrPresupuestado.Value = 0;
            this.CantSolicitado.Value = 0;
            this.VlrSolicitado.Value = 0;
            this.CantComprado.Value = 0;
            this.VlrComprado.Value = 0;
            this.CantPreComprado.Value = 0;
            this.VlrPreComprado.Value = 0;
            this.CantRecibido.Value = 0;
            this.VlrRecibido.Value = 0;
            this.CantConsumido.Value = 0;
            this.VlrConsumido.Value = 0;
            this.CantFacturado.Value = 0;
            this.VlrFacturado.Value = 0;
        }

        public void InitCols()
        {
            this.ConsecTarea = new UDT_Consecutivo();
            this.ConsecDeta = new UDT_Consecutivo();
            this.ConsecMvto = new UDT_Consecutivo();
            this.RecursoID = new UDT_CodigoGrl();
            this.RecursoDesc = new UDT_Descriptivo();
            this.TipoRecurso = new UDTSQL_tinyint();
            this.UnidadInvID = new UDT_UnidadInvID();
            this.inReferenciaID = new UDT_inReferenciaID();
            this.CodigoBSID = new UDT_CodigoBSID();
            this.MarcaInvID = new UDT_CodigoGrl();
            this.RefProveedor = new UDT_CodigoGrl20();
            this.TareaCliente = new UDT_CodigoGrl20();
            this.TareaID = new UDT_CodigoGrl();
            this.TareaDesc = new UDT_DescripTExt();            
            this.CantidadTarea = new UDT_Cantidad();
            this.CantPreproyecto = new UDT_Cantidad();           
            this.VlrUnitPreproyecto = new UDT_Valor();
            this.VlrTotPreproyecto = new UDT_Valor();
            this.VlrUnitCLIPreproyecto = new UDT_Valor();
            this.VlrTotCLIPreproyecto = new UDT_Valor();
            this.CantPresupuestado = new UDT_Cantidad();
            this.VlrPresupuestado = new UDT_Valor(); 
            this.CantSolicitado = new UDT_Cantidad();
            this.VlrSolicitado = new UDT_Valor();
            this.CantComprado = new UDT_Cantidad();
            this.VlrComprado = new UDT_Valor();
            this.CantPreComprado = new UDT_Cantidad();
            this.VlrPreComprado = new UDT_Valor();
            this.CantRecibido = new UDT_Cantidad();
            this.VlrRecibido = new UDT_Valor();
            this.CantConsumido = new UDT_Cantidad();
            this.VlrConsumido = new UDT_Valor();
            this.CantFacturado = new UDT_Cantidad();
            this.VlrFacturado = new UDT_Valor();
            this.Detalle = new List<DTO_QueryTrazabilidad>();            
        }
        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo ConsecTarea { get; set; }

        [DataMember]
        public UDT_Consecutivo ConsecDeta { get; set; }

        [DataMember]
        public UDT_Consecutivo ConsecMvto { get; set; }

        [DataMember]
        public UDT_CodigoGrl RecursoID { get; set; }

        [DataMember]
        public UDT_Descriptivo RecursoDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoRecurso { get; set; }

        [DataMember]
        public UDT_UnidadInvID UnidadInvID { get; set; }

        [DataMember]
        public UDT_inReferenciaID inReferenciaID { get; set; }

        [DataMember]
        public UDT_CodigoBSID CodigoBSID { get; set; }

        [DataMember]
        public UDT_CodigoGrl20 TareaCliente { get; set; }

        [DataMember]
        public UDT_CodigoGrl TareaID { get; set; }

        [DataMember]
        public UDT_DescripTExt TareaDesc { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadTarea { get; set; }

        [DataMember]
        public UDT_Cantidad CantPreproyecto { get; set; }

        [DataMember]
        public UDT_Valor VlrUnitPreproyecto { get; set; }

        [DataMember]
        public UDT_Valor VlrTotPreproyecto { get; set; }

        [DataMember]
        public UDT_Valor VlrUnitCLIPreproyecto { get; set; }

        [DataMember]
        public UDT_Valor VlrTotCLIPreproyecto { get; set; }

        [DataMember]
        public UDT_Cantidad CantPresupuestado { get; set; }

        [DataMember]
        public UDT_Valor VlrPresupuestado { get; set; }

        [DataMember]
        public UDT_Cantidad CantSolicitado { get; set; }

        [DataMember]
        public UDT_Valor VlrSolicitado { get; set; }

        [DataMember]
        public UDT_Cantidad CantComprado { get; set; }

        [DataMember]
        public UDT_Valor VlrComprado { get; set; }

        [DataMember]
        public UDT_Cantidad CantPreComprado { get; set; }

        [DataMember]
        public UDT_Valor VlrPreComprado { get; set; }

        [DataMember]
        public UDT_Cantidad CantRecibido { get; set; }

        [DataMember]
        public UDT_Valor VlrRecibido { get; set; }

        [DataMember]
        public UDT_Cantidad CantConsumido { get; set; }

        [DataMember]
        public UDT_Valor VlrConsumido { get; set; }

        [DataMember]
        public UDT_Cantidad CantFacturado { get; set; }

        [DataMember]
        public UDT_Valor VlrFacturado { get; set; }

        [DataMember]
        public UDT_CodigoGrl MarcaInvID { get; set; }

        [DataMember]
        public UDT_CodigoGrl20 RefProveedor { get; set; }

        [DataMember]
        public List<DTO_QueryTrazabilidad> Detalle { get; set; }
        #endregion
    }
}
