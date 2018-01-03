using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using System.Data.SqlClient;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models aplMaestraCampo
    /// </summary>
    [DataContract]
    [Serializable]
    [KnownType(typeof(ForeignKeyField))]
    public class DTO_aplMaestraCampo
    {
        #region Contructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_aplMaestraCampo(IDataReader dr)
        {
            try
            {
                this.MaestraCampoID = Convert.ToInt32(dr["MaestraCampoID"]);
                this.DocumentoID = Convert.ToInt32(dr["DocumentoID"]);
                this.PKInd = Convert.ToBoolean(dr["PKInd"]);
                this.NombreColumna = dr["NombreColumna"].ToString();
                this.Tipo = dr["Tipo"].ToString();
                this.LongitudMax = Convert.ToInt16(dr["LongitudMax"]);
                this.VacioInd = Convert.ToBoolean(dr["VacioInd"]);
                this.ActualizableInd = Convert.ToBoolean(dr["ActualizableInd"]);
                this.EditableInd = Convert.ToBoolean(dr["EditableInd"]);
                this.CambiosEnCascada = Convert.ToBoolean(dr["CambiosEnCascada"]);
                this.ImportacionInd = Convert.ToBoolean(dr["ImportacionInd"]);
                this.FKInd = Convert.ToBoolean(dr["FKInd"]);
                this.FKDocumentoID = !string.IsNullOrEmpty(dr["FKDocumentoID"].ToString()) ? Convert.ToInt32(dr["FKDocumentoID"]) : 0;
                this.FKNombreTabla = !string.IsNullOrEmpty(dr["FKNombreTabla"].ToString()) ? dr["FKNombreTabla"].ToString() : string.Empty;
                this.FKColumnaID = !string.IsNullOrEmpty(dr["FKColumnaID"].ToString()) ? dr["FKColumnaID"].ToString() : string.Empty;
                this.FKColumnaDesc = !string.IsNullOrEmpty(dr["FKColumnaDesc"].ToString()) ? dr["FKColumnaDesc"].ToString() : string.Empty;
                this.GrillaInd = Convert.ToBoolean(dr["GrillaInd"]);
                this.GrillaEdicionInd = Convert.ToBoolean(dr["GrillaEdicionInd"]);
                this.ColumnaTamano = Convert.ToInt16(dr["ColumnaTamano"]);
                this.ColumnaPosicion = Convert.ToInt16(dr["ColumnaPosicion"]);
                this.NivelJerarquia = Convert.ToInt16(dr["NivelJerarquia"]);
                this.RegExpression = dr["RegEx"].ToString();
                this.TablaDesc = dr["TablaDesc"].ToString();
                this.Tab = dr["Tab"].ToString();
            }
            catch (Exception e)
            {

            }
        } 

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_aplMaestraCampo(string tipo) 
        {
            this.Tipo = tipo;
        }

        #endregion

        #region Propiedades
        /// <summary>
        /// Gets or sets the MaestraCampoID
        /// </summary>
        [DataMember]
        public int MaestraCampoID { get; set; }

        /// <summary>
        /// indica si es llave primaria
        /// </summary>
        [DataMember]
        public Boolean PKInd { get; set; }

        /// <summary>
        /// Gets or sets the DocumentoID
        /// </summary>
        [DataMember]
        public int DocumentoID { get; set; }

        /// <summary>
        /// Gets or sets the NombreColumna
        /// </summary>
        [DataMember]
        public string NombreColumna { get; set; }

        /// <summary>
        /// Gets or sets the Tipo
        /// </summary>
        [DataMember]
        public string Tipo { get; set; }

        /// <summary>
        /// Gets or sets the LongitudMax
        /// </summary>
        [DataMember]
        public short LongitudMax { get; set; }


        /// <summary>
        /// indica si permite vacio
        /// </summary>
        [DataMember]
        public Boolean VacioInd { get; set; }

        /// <summary>
        /// Gets or sets the ActualizableInd
        /// </summary>
        [DataMember]
        public Boolean ActualizableInd { get; set; }

        /// <summary>
        /// Gets or sets the EditableInd
        /// </summary>
        [DataMember]
        public Boolean EditableInd { get; set; }

        /// <summary>
        /// Gets or sets the CambiosEnCascada
        /// </summary>
        [DataMember]
        public Boolean CambiosEnCascada { get; set; }

        /// <summary>
        /// Gets or sets the ImportacionInd
        /// </summary>
        [DataMember]
        public Boolean ImportacionInd { get; set; }

        /// <summary>
        /// Gets or sets the FKInd
        /// </summary>
        [DataMember]
        public Boolean FKInd { get; set; }

        /// <summary>
        /// Gets or sets the FKDocumentoID
        /// </summary>
        [DataMember]
        public int FKDocumentoID { get; set; }

        /// <summary>
        /// Gets or sets the FKNombreTabla
        /// </summary>
        [DataMember]
        public string FKNombreTabla { get; set; }

        /// <summary>
        /// Gets or sets the FKColumnaID
        /// </summary>
        [DataMember]
        public string FKColumnaID { get; set; }

        /// <summary>
        /// Gets or sets the FKColumnaDesc
        /// </summary>
        [DataMember]
        public string FKColumnaDesc { get; set; }


        /// <summary>
        /// Gets or sets the GrillaInd
        /// </summary>
        [DataMember]
        public Boolean GrillaInd { get; set; }

        /// <summary>
        /// Gets or sets the GrillaEdicionInd
        /// </summary>
        [DataMember]
        public Boolean GrillaEdicionInd { get; set; }

        /// <summary>
        /// Gets or sets the ColumnaTamano
        /// </summary>
        [DataMember]
        public int ColumnaTamano { get; set; }

        /// <summary>
        /// Gets or sets the ColumnaPosicion
        /// </summary>
        [DataMember]
        public int ColumnaPosicion { get; set; }

        /// <summary>
        /// Gets or sets the NivelJerarquia
        /// </summary>
        [DataMember]
        public int NivelJerarquia { get; set; }

        /// <summary>
        /// Gets or sets the Regular expression
        /// </summary>
        [DataMember]
        public string RegExpression { get; set; }

        /// <summary>
        /// Nombre de las tabla de descripciones dentro de los recursos
        /// </summary>
        [DataMember]
        public string TablaDesc { get; set; }

        /// <summary>
        /// Nombre del tab de la grilla de edicion
        /// </summary>
        [DataMember]
        public string Tab { get; set; }

        #endregion
    }

    [DataContract]
    [Serializable]
    public class ForeignKeyField : DTO_aplMaestraCampo
    {
        /// <summary>
        /// Constructor de un campo que tiene foreign key
        /// </summary>
        /// <param name="name">Nombre del campo en la maestra</param>
        /// <param name="udtType">Tipo de campo</param>
        /// <param name="tableName">Nombre de la tabla con la cual tiene el foreign key</param>
        /// <param name="foreignField">NKombre del campo en la tabla con el FK</param>
        public ForeignKeyField(string udtType, string tableName, string foreignField)
            : base(udtType)
        {
            this.TableName = tableName;
            this.ForeignField = foreignField;
        }

        public ForeignKeyField(SqlDataReader dr) : base(dr)
        {
            // TODO: Complete member initialization
            this.TableName = Convert.ToString(dr["FkNombreTabla"]);
            this.ForeignField = Convert.ToString(dr["FKColumnaID"]);
            this.LocalDescField = Convert.ToString(dr["FKColumnaDesc"]);
        }
        [DataMember]
        public string TableName;
        [DataMember]
        public string ForeignField;
        [DataMember]
        public string LocalDescField;
        [DataMember]
        public string ForeignDescField = "Descriptivo";
    }    
}
