using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;
using System.Reflection;

namespace NewAge.DTO.Negocio
{
    #region Documentos
    /// <summary>
    /// Class Models DTO_QueryFlujoFondos
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_QueryFlujoFondos
    {
        #region DTO_QueryFlujoFondos

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_QueryFlujoFondos(IDataReader dr)
        {
            InitCols();
            try
            {
                this.Proyecto.Value = dr["Proyecto"].ToString();
                this.DocProyecto.Value = dr["DocProyecto"].ToString();
                this.ProyectoDesc.Value = dr["ProyectoDesc"].ToString();
                if (!string.IsNullOrEmpty(dr["PerA"].ToString()))
                    this.PerA.Value = Convert.ToDecimal(dr["PerA"]);
                if (!string.IsNullOrEmpty(dr["Per0"].ToString()))
                    this.Per0.Value = Convert.ToDecimal(dr["Per0"]);
                if (!string.IsNullOrEmpty(dr["Per1"].ToString()))
                    this.Per1.Value = Convert.ToDecimal(dr["Per1"]);
                if (!string.IsNullOrEmpty(dr["Per2"].ToString()))
                    this.Per2.Value = Convert.ToDecimal(dr["Per2"]);
                if (!string.IsNullOrEmpty(dr["Per3"].ToString()))
                    this.Per3.Value = Convert.ToDecimal(dr["Per3"]);
                if (!string.IsNullOrEmpty(dr["Per4"].ToString()))
                    this.Per4.Value = Convert.ToDecimal(dr["Per4"]);
                if (!string.IsNullOrEmpty(dr["Per5"].ToString()))
                    this.Per5.Value = Convert.ToDecimal(dr["Per5"]);
                if (!string.IsNullOrEmpty(dr["Per6"].ToString()))
                    this.Per6.Value = Convert.ToDecimal(dr["Per6"]);
                if (!string.IsNullOrEmpty(dr["PerM"].ToString()))
                    this.PerM.Value = Convert.ToDecimal(dr["PerM"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_QueryFlujoFondos()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Proyecto = new UDT_ProyectoID();
            this.DocProyecto = new UDTSQL_char(20);
            this.ProyectoDesc = new UDT_Descriptivo();
            this.PerA = new UDTSQL_decimal();
            this.Per0 = new UDTSQL_decimal();
            this.Per1 = new UDTSQL_decimal();
            this.Per2 = new UDTSQL_decimal();
            this.Per3 = new UDTSQL_decimal();
            this.Per4 = new UDTSQL_decimal();
            this.Per5 = new UDTSQL_decimal();
            this.Per6 = new UDTSQL_decimal();
            this.PerM = new UDTSQL_decimal();

            this.Tareas = new List<DTO_QueryFlujoFondosTareas>();
            this.Detalle = new List<DTO_QueryFlujoFondosDetalle>();

        }
        #endregion

        #region Propiedades

        [DataMember]
        public UDT_ProyectoID Proyecto { get; set; }

        [DataMember]
        public UDTSQL_char DocProyecto { get; set; }

        [DataMember]
        public UDT_Descriptivo ProyectoDesc { get; set; }

        [DataMember]
        public UDTSQL_decimal PerA { get; set; }

        [DataMember]
        public UDTSQL_decimal Per0 { get; set; }

        [DataMember]
        public UDTSQL_decimal Per1 { get; set; }

        [DataMember]
        public UDTSQL_decimal Per2 { get; set; }

        [DataMember]
        public UDTSQL_decimal Per3 { get; set; }

        [DataMember]
        public UDTSQL_decimal Per4 { get; set; }

        [DataMember]
        public UDTSQL_decimal Per5 { get; set; }

        [DataMember]
        public UDTSQL_decimal Per6 { get; set; }

        [DataMember]
        public UDTSQL_decimal PerM { get; set; }

        [DataMember]
        public List<DTO_QueryFlujoFondosTareas> Tareas { get; set; }

        [DataMember]
        public List<DTO_QueryFlujoFondosDetalle> Detalle { get; set; }
        #endregion
    }
    #endregion


    #region Detalle Flujo
    /// <summary>
    /// Class Models DTO_QueryFlujoFondosDetalle
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_QueryFlujoFondosDetalle
    {
        public DTO_QueryFlujoFondosDetalle(IDataReader dr)
        {
            InitDetCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["PerA"].ToString()))
                    this.PerA.Value = Convert.ToDecimal(dr["PerA"]);
                if (!string.IsNullOrEmpty(dr["Per0"].ToString()))
                    this.Per0.Value = Convert.ToDecimal(dr["Per0"]);
                if (!string.IsNullOrEmpty(dr["Per1"].ToString()))
                    this.Per1.Value = Convert.ToDecimal(dr["Per1"]);
                if (!string.IsNullOrEmpty(dr["Per2"].ToString()))
                    this.Per2.Value = Convert.ToDecimal(dr["Per2"]);
                if (!string.IsNullOrEmpty(dr["Per3"].ToString()))
                    this.Per3.Value = Convert.ToDecimal(dr["Per3"]);
                if (!string.IsNullOrEmpty(dr["Per4"].ToString()))
                    this.Per4.Value = Convert.ToDecimal(dr["Per4"]);
                if (!string.IsNullOrEmpty(dr["Per5"].ToString()))
                    this.Per5.Value = Convert.ToDecimal(dr["Per5"]);
                if (!string.IsNullOrEmpty(dr["Per6"].ToString()))
                    this.Per6.Value = Convert.ToDecimal(dr["Per6"]);
                if (!string.IsNullOrEmpty(dr["PerM"].ToString()))
                    this.PerM.Value = Convert.ToDecimal(dr["PerM"]);
                this.Documento.Value = dr["Documento"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DTO_QueryFlujoFondosDetalle()
        {
            InitDetCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitDetCols()
        {
            this.Documento = new UDTSQL_char(50);
            this.PerA = new UDTSQL_decimal();
            this.Per0 = new UDTSQL_decimal();
            this.Per1 = new UDTSQL_decimal();
            this.Per2 = new UDTSQL_decimal();
            this.Per3 = new UDTSQL_decimal();
            this.Per4 = new UDTSQL_decimal();
            this.Per5 = new UDTSQL_decimal();
            this.Per6 = new UDTSQL_decimal();
            this.PerM = new UDTSQL_decimal();
            this.Factor = new UDTSQL_decimal();
        }

        #region Propiedades
        [DataMember]
        public UDTSQL_char Documento { get; set; }

        [DataMember]
        public UDTSQL_decimal PerA { get; set; }

        [DataMember]
        public UDTSQL_decimal Per0 { get; set; }

        [DataMember]
        public UDTSQL_decimal Per1 { get; set; }

        [DataMember]
        public UDTSQL_decimal Per2 { get; set; }

        [DataMember]
        public UDTSQL_decimal Per3 { get; set; }

        [DataMember]
        public UDTSQL_decimal Per4 { get; set; }

        [DataMember]
        public UDTSQL_decimal Per5 { get; set; }

        [DataMember]
        public UDTSQL_decimal Per6 { get; set; }

        [DataMember]
        public UDTSQL_decimal PerM { get; set; }

        [DataMember]
        public UDTSQL_decimal Factor { get; set; }

        #endregion
    }
    #endregion

    #region Tareas
    /// <summary>
    /// Class Models DTO_QueryFlujoFondosTareas
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_QueryFlujoFondosTareas
    {
        public DTO_QueryFlujoFondosTareas(IDataReader dr)
        {
            InitTareasCols();
            try
            {
                this.TareaID.Value = Convert.ToString(dr["TareaID"]);
                this.DescTarea.Value = Convert.ToString(dr["DescTarea"]);
                if (!string.IsNullOrEmpty(dr["PerA"].ToString()))
                    this.PerA.Value = Convert.ToDecimal(dr["PerA"]);
                if (!string.IsNullOrEmpty(dr["Per0"].ToString()))
                    this.Per0.Value = Convert.ToDecimal(dr["Per0"]);
                if (!string.IsNullOrEmpty(dr["Per1"].ToString()))
                    this.Per1.Value = Convert.ToDecimal(dr["Per1"]);
                if (!string.IsNullOrEmpty(dr["Per2"].ToString()))
                    this.Per2.Value = Convert.ToDecimal(dr["Per2"]);
                if (!string.IsNullOrEmpty(dr["Per3"].ToString()))
                    this.Per3.Value = Convert.ToDecimal(dr["Per3"]);
                if (!string.IsNullOrEmpty(dr["Per4"].ToString()))
                    this.Per4.Value = Convert.ToDecimal(dr["Per4"]);
                if (!string.IsNullOrEmpty(dr["Per5"].ToString()))
                    this.Per5.Value = Convert.ToDecimal(dr["Per5"]);
                if (!string.IsNullOrEmpty(dr["Per6"].ToString()))
                    this.Per6.Value = Convert.ToDecimal(dr["Per6"]);
                if (!string.IsNullOrEmpty(dr["PerM"].ToString()))
                    this.PerM.Value = Convert.ToDecimal(dr["PerM"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DTO_QueryFlujoFondosTareas()
        {
            InitTareasCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitTareasCols()
        {
            this.TareaID = new UDT_CodigoGrl();
            this.DescTarea = new UDT_DescripUnFormat();
            this.PerA = new UDTSQL_decimal();
            this.Per0 = new UDTSQL_decimal();
            this.Per1 = new UDTSQL_decimal();
            this.Per2 = new UDTSQL_decimal();
            this.Per3 = new UDTSQL_decimal();
            this.Per4 = new UDTSQL_decimal();
            this.Per5 = new UDTSQL_decimal();
            this.Per6 = new UDTSQL_decimal();
            this.PerM = new UDTSQL_decimal();
            this.DetalleTarea= new List<DTO_QueryFlujoFondosDetalleTarea>();
        }

        #region Propiedades



        [DataMember]
        public UDT_CodigoGrl TareaID { get; set; }

        [DataMember]
        public UDT_DescripUnFormat DescTarea { get; set; }


        [DataMember]
        public UDTSQL_decimal PerA { get; set; }

        [DataMember]
        public UDTSQL_decimal Per0 { get; set; }

        [DataMember]
        public UDTSQL_decimal Per1 { get; set; }

        [DataMember]
        public UDTSQL_decimal Per2 { get; set; }

        [DataMember]
        public UDTSQL_decimal Per3 { get; set; }

        [DataMember]
        public UDTSQL_decimal Per4 { get; set; }

        [DataMember]
        public UDTSQL_decimal Per5 { get; set; }

        [DataMember]
        public UDTSQL_decimal Per6 { get; set; }

        [DataMember]
        public UDTSQL_decimal PerM { get; set; }


        [DataMember]
        public List<DTO_QueryFlujoFondosDetalleTarea> DetalleTarea{ get; set; }
        #endregion
    }
    #endregion
    #region Tareas Detalle
    /// <summary>
    /// Class Models DTO_QueryFlujoFondosDetalleTarea
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_QueryFlujoFondosDetalleTarea
    {
        public DTO_QueryFlujoFondosDetalleTarea(IDataReader dr)
        {
            InitTareasDetCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["PerA"].ToString()))
                    this.PerA.Value = Convert.ToDecimal(dr["PerA"]);
                if (!string.IsNullOrEmpty(dr["Per0"].ToString()))
                    this.Per0.Value = Convert.ToDecimal(dr["Per0"]);
                if (!string.IsNullOrEmpty(dr["Per1"].ToString()))
                    this.Per1.Value = Convert.ToDecimal(dr["Per1"]);
                if (!string.IsNullOrEmpty(dr["Per2"].ToString()))
                    this.Per2.Value = Convert.ToDecimal(dr["Per2"]);
                if (!string.IsNullOrEmpty(dr["Per3"].ToString()))
                    this.Per3.Value = Convert.ToDecimal(dr["Per3"]);
                if (!string.IsNullOrEmpty(dr["Per4"].ToString()))
                    this.Per4.Value = Convert.ToDecimal(dr["Per4"]);
                if (!string.IsNullOrEmpty(dr["Per5"].ToString()))
                    this.Per5.Value = Convert.ToDecimal(dr["Per5"]);
                if (!string.IsNullOrEmpty(dr["Per6"].ToString()))
                    this.Per6.Value = Convert.ToDecimal(dr["Per6"]);
                if (!string.IsNullOrEmpty(dr["PerM"].ToString()))
                    this.PerM.Value = Convert.ToDecimal(dr["PerM"]);
                this.Documento.Value = dr["Documento"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DTO_QueryFlujoFondosDetalleTarea()
        {
            InitTareasDetCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitTareasDetCols()
        {
            this.Documento = new UDTSQL_char(50);
            this.PerA = new UDTSQL_decimal();
            this.Per0 = new UDTSQL_decimal();
            this.Per1 = new UDTSQL_decimal();
            this.Per2 = new UDTSQL_decimal();
            this.Per3 = new UDTSQL_decimal();
            this.Per4 = new UDTSQL_decimal();
            this.Per5 = new UDTSQL_decimal();
            this.Per6 = new UDTSQL_decimal();
            this.PerM = new UDTSQL_decimal();
            this.Factor = new UDTSQL_decimal();
        }

        #region Propiedades

        [DataMember]
        public UDTSQL_char Documento { get; set; }

        [DataMember]
        public UDTSQL_decimal PerA { get; set; }

        [DataMember]
        public UDTSQL_decimal Per0 { get; set; }

        [DataMember]
        public UDTSQL_decimal Per1 { get; set; }

        [DataMember]
        public UDTSQL_decimal Per2 { get; set; }

        [DataMember]
        public UDTSQL_decimal Per3 { get; set; }

        [DataMember]
        public UDTSQL_decimal Per4 { get; set; }

        [DataMember]
        public UDTSQL_decimal Per5 { get; set; }

        [DataMember]
        public UDTSQL_decimal Per6 { get; set; }

        [DataMember]
        public UDTSQL_decimal PerM { get; set; }

        [DataMember]
        public UDTSQL_decimal Factor { get; set; }
        #endregion
    }
    #endregion
}
