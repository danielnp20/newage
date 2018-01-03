using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.Serialization;

namespace SentenceTransformer
{
    [Serializable]
    [DataContract]
    public class ConsultasFields
    {
        #region Contructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public ConsultasFields()
        {
            this.Field = string.Empty;
            this.MetaData = string.Empty;
            this.Tipo = typeof(string);
            //this.A_Filtro = false;
            this.A_Seleccion = false;
            this.Orden = string.Empty;
            this.OrdenIndex = 99;
            //this.Filtro = string.Empty;
            this.A_GroupBy = false;
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public ConsultasFields(string field, string fieldShown, Type tipo)
        {
            this.Field = field;
            this.FieldShown = fieldShown;
            this.MetaData = string.Empty;
            this.Tipo = tipo;
            //this.A_Filtro = false;
            this.A_Seleccion = false;
            this.Orden = string.Empty;
            this.OrdenIndex = 99;
            //this.Filtro = string.Empty;
            this.A_GroupBy = false;
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Gets or sets the Field
        /// </summary>
        [DataMember]
        public string Field
        {
            get;
            set;
        }

        /// <summary>
        /// nombre de campo aplicado los recursos
        /// </summary>
        [DataMember]
        public string FieldShown
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the MetaData
        /// </summary>        
        [DataMember]
        public string MetaData
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Tipo
        /// </summary>        
        [DataMember]
        public Type Tipo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Tipo
        /// </summary>        
        [DataMember]
        public string TipoRsx
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the A_Seleccion
        /// </summary>
        [DataMember]
        public bool A_Seleccion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Orden
        /// </summary>
        [DataMember]
        public string Orden
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the OrdenIndex
        /// </summary>
        [DataMember]
        public int OrdenIndex
        {
            get;
            set;
        }

        ///// <summary>
        ///// Gets or sets the Filtro
        ///// </summary>
        //public string Filtro
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Gets or sets the A_GroupBy
        /// </summary>
        [DataMember]
        public bool A_GroupBy
        {
            get;
            set;
        }

        #endregion
    }
}
