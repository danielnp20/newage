using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using System.Reflection;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_glTabla
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glTabla : DTO_MasterBasic
    {
        #region Constructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glTabla(IDataReader dr, DTO_aplMaestraPropiedades props, bool isReplica) 
            : base(dr, props)
        {
            this.InitCols();
            this.EmpresaGrupoID.Value = Convert.ToString(dr["EmpresaGrupoID"]);
            this.DocumentoID.Value = Convert.ToInt32(dr["DocumentoID"]);
            this.TablaNombre.Value = Convert.ToString(dr["Descriptivo"]);
            
            try
            {
                if (!DBNull.Value.Equals(dr["Jerarquica"]))
                    this.Jerarquica.Value = Convert.ToBoolean(dr["Jerarquica"]);
                else
                    this.Jerarquica.Value = false;
            }
            catch (Exception ex)
            {
                ;
            }

            try
            {
                if (!DBNull.Value.Equals(dr["TipoSeguridad"]))
                    this.EGTipo.Value = Convert.ToByte(dr["TipoSeguridad"]);
                else
                    this.EGTipo.Value = 1;
            }
            catch (Exception)
            {
                ;
            }

            this.descrNivel1.Value = Convert.ToString(dr["descrNivel1"]);
            this.descrNivel2.Value = Convert.ToString(dr["descrNivel2"]);
            this.descrNivel3.Value = Convert.ToString(dr["descrNivel3"]);
            this.descrNivel4.Value = Convert.ToString(dr["descrNivel4"]);
            this.descrNivel5.Value = Convert.ToString(dr["descrNivel5"]);
            this.descrNivel6.Value = Convert.ToString(dr["descrNivel6"]);
            this.descrNivel7.Value = Convert.ToString(dr["descrNivel7"]);
            this.descrNivel8.Value = Convert.ToString(dr["descrNivel8"]);
            this.descrNivel9.Value = Convert.ToString(dr["descrNivel9"]);
            this.descrNivel10.Value = Convert.ToString(dr["descrNivel10"]);
            if (!DBNull.Value.Equals(dr["lonNivel1"]))
                this.lonNivel1.Value = Convert.ToByte(dr["lonNivel1"]);
            if (!DBNull.Value.Equals(dr["lonNivel2"]))
                this.lonNivel2.Value = Convert.ToByte(dr["lonNivel2"]);
            if (!DBNull.Value.Equals(dr["lonNivel3"]))
                this.lonNivel3.Value = Convert.ToByte(dr["lonNivel3"]);
            if (!DBNull.Value.Equals(dr["lonNivel4"]))
                this.lonNivel4.Value = Convert.ToByte(dr["lonNivel4"]);
            if (!DBNull.Value.Equals(dr["lonNivel5"]))
                this.lonNivel5.Value = Convert.ToByte(dr["lonNivel5"]);
            if (!DBNull.Value.Equals(dr["lonNivel6"]))
                this.lonNivel6.Value = Convert.ToByte(dr["lonNivel6"]);
            if (!DBNull.Value.Equals(dr["lonNivel7"]))
                this.lonNivel7.Value = Convert.ToByte(dr["lonNivel7"]);
            if (!DBNull.Value.Equals(dr["lonNivel8"]))
                this.lonNivel8.Value = Convert.ToByte(dr["lonNivel8"]);
            if (!DBNull.Value.Equals(dr["lonNivel9"]))
                this.lonNivel9.Value = Convert.ToByte(dr["lonNivel9"]);
            if (!DBNull.Value.Equals(dr["lonNivel10"]))
                this.lonNivel10.Value = Convert.ToByte(dr["lonNivel10"]);
            this.CtrlVersion.Value = Convert.ToInt16(dr["CtrlVersion"]);
            this.ReplicaID.Value = Convert.ToInt32(dr["ReplicaID"]);
        }

        public DTO_glTabla(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glTabla()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.DocumentoID = new UDT_DocumentoID();
            this.TablaNombre = new UDT_DescripTBase();
            this.Jerarquica = new UDT_SiNo();
            this.EGTipo = new UDTSQL_tinyint();
            this.lonNivel1 = new UDTSQL_tinyint();
            this.lonNivel2 = new UDTSQL_tinyint();
            this.lonNivel3 = new UDTSQL_tinyint();
            this.lonNivel4 = new UDTSQL_tinyint();
            this.lonNivel5 = new UDTSQL_tinyint();
            this.lonNivel6 = new UDTSQL_tinyint();
            this.lonNivel7 = new UDTSQL_tinyint();
            this.lonNivel8 = new UDTSQL_tinyint();
            this.lonNivel9 = new UDTSQL_tinyint();
            this.lonNivel10 = new UDTSQL_tinyint();
            this.descrNivel1 = new UDTSQL_varchar(50);
            this.descrNivel2 = new UDTSQL_varchar(50);
            this.descrNivel3 = new UDTSQL_varchar(50);
            this.descrNivel4 = new UDTSQL_varchar(50);
            this.descrNivel5 = new UDTSQL_varchar(50);
            this.descrNivel6 = new UDTSQL_varchar(50);
            this.descrNivel7 = new UDTSQL_varchar(50);
            this.descrNivel8 = new UDTSQL_varchar(50);
            this.descrNivel9 = new UDTSQL_varchar(50);
            this.descrNivel10 = new UDTSQL_varchar(50);
        }

        #endregion

        /// <summary>
        /// Gets or sets the DocumentoID
        /// </summary>
        [DataMember]
        public UDT_DocumentoID DocumentoID { get; set; }

        /// <summary>
        /// Gets or sets the TablaNombre
        /// </summary>
        [DataMember]
        public UDT_DescripTBase TablaNombre { get; set; }

        /// <summary>
        /// Gets or sets the Jerarquica
        /// </summary>
        [DataMember]
        public UDT_SiNo Jerarquica { get; set; }

        /// <summary>
        /// Gets or sets the Jerarquica
        /// </summary>
        [DataMember]
        public UDTSQL_tinyint EGTipo { get; set; }

        /// <summary>
        /// Gets or sets the lonNivel1
        /// </summary>
        [DataMember]
        public UDTSQL_tinyint lonNivel1 { get; set; }

        /// <summary>
        /// Gets or sets the lonNivel2
        /// </summary>
        [DataMember]
        public UDTSQL_tinyint lonNivel2 { get; set; }

        /// <summary>
        /// Gets or sets the lonNivel3
        /// </summary>
        [DataMember]
        public UDTSQL_tinyint lonNivel3 { get; set; }

        /// <summary>
        /// Gets or sets the lonNivel4
        /// </summary>
        [DataMember]
        public UDTSQL_tinyint lonNivel4 { get; set; }

        /// <summary>
        /// Gets or sets the lonNivel5
        /// </summary>
        [DataMember]
        public UDTSQL_tinyint lonNivel5 { get; set; }

        /// <summary>
        /// Gets or sets the lonNivel5
        /// </summary>
        [DataMember]
        public UDTSQL_tinyint lonNivel6 { get; set; }

        /// <summary>
        /// Gets or sets the lonNivel5
        /// </summary>
        [DataMember]
        public UDTSQL_tinyint lonNivel7 { get; set; }

        /// <summary>
        /// Gets or sets the lonNivel5
        /// </summary>
        [DataMember]
        public UDTSQL_tinyint lonNivel8 { get; set; }

        /// <summary>
        /// Gets or sets the lonNivel5
        /// </summary>
        [DataMember]
        public UDTSQL_tinyint lonNivel9 { get; set; }

        /// <summary>
        /// Gets or sets the lonNivel5
        /// </summary>
        [DataMember]
        public UDTSQL_tinyint lonNivel10 { get; set; }

        /// <summary>
        /// Gets or sets the descrNivel1
        /// </summary>
        [DataMember]
        public UDTSQL_varchar descrNivel1 { get; set; }

        /// <summary>
        /// Gets or sets the descrNivel2
        /// </summary>
        [DataMember]
        public UDTSQL_varchar descrNivel2 { get; set; }

        /// <summary>
        /// Gets or sets the descrNivel3
        /// </summary>
        [DataMember]
        public UDTSQL_varchar descrNivel3 { get; set; }

        /// <summary>
        /// Gets or sets the descrNivel4
        /// </summary>
        [DataMember]
        public UDTSQL_varchar descrNivel4 { get; set; }

         /// <summary>
        /// Gets or sets the descrNivel5
        /// </summary>
        [DataMember]
        public UDTSQL_varchar descrNivel5 { get; set; }

        /// <summary>
        /// Gets or sets the descrNivel5
        /// </summary>
        [DataMember]
        public UDTSQL_varchar descrNivel6 { get; set; }

        /// <summary>
        /// Gets or sets the descrNivel5
        /// </summary>
        [DataMember]
        public UDTSQL_varchar descrNivel7 { get; set; }

        /// <summary>
        /// Gets or sets the descrNivel5
        /// </summary>
        [DataMember]
        public UDTSQL_varchar descrNivel8 { get; set; }

        /// <summary>
        /// Gets or sets the descrNivel5
        /// </summary>
        [DataMember]
        public UDTSQL_varchar descrNivel9 { get; set; }

        /// <summary>
        /// Gets or sets the descrNivel5
        /// </summary>
        [DataMember]
        public UDTSQL_varchar descrNivel10 { get; set; }

        #region funciones

        /// <summary>
        /// Retorna las longitudes de todos los niveles en un arreglo
        /// Siempre retorna un arreglo con la longitud maxima (DTO_glTabla.MaxLevels)
        /// asi no se usen toos los niveles. Los que no se usan estan en 0.
        /// </summary>
        /// <returns></returns>
        public int[] LevelLengths()
        {
            int[] array= new int[DTO_glTabla.MaxLevels];
            for (int i = 0; i < DTO_glTabla.MaxLevels; i++)
            {
                int level = i + 1;
                PropertyInfo pi = this.GetType().GetProperty("lonNivel" + level);
                if (pi != null)
                {
                    UDTSQL_tinyint property = (UDTSQL_tinyint)pi.GetValue(this, null);
                    if (property != null)
                    {
                        array[i] = property.Value != null ? property.Value.Value : 0;
                    }
                    else
                    {
                        array[i] = 0;
                    }
                }
                else
                {
                    array[i] = 0;
                }
            }
            return array;
        }

        public int[] CompleteLevelLengths()
        {
            int[] lengths=this.LevelLengths();
            List<int> array = new List<int>();
            int sum=0;
            for (int i = 0; i < this.LevelsUsed(); i++)
            {
                sum+=lengths[i];
                array.Add(sum);
            }
            return array.ToArray();
        }

        /// <summary>
        /// Retorna las descripciones de todos los niveles en un arreglo
        /// Siempre retorna un arreglo con la longitud maxima (DTO_glTabla.MaxLevels)
        /// asi no se usen toos los niveles. Los que no se usan estan en 0.
        /// </summary>
        /// <returns></returns>
        public string[] LevelDescs()
        {
            string[] array= new string[DTO_glTabla.MaxLevels];
            for (int i = 0; i < DTO_glTabla.MaxLevels; i++)
            {
                int level = i + 1;
                PropertyInfo pi = this.GetType().GetProperty("descrNivel"+level);
                if (pi != null)
                {
                    UDTSQL_varchar property = (UDTSQL_varchar)pi.GetValue(this, null);
                    if (property != null)
                    {
                        array[i] = property.Value;
                    }
                    else
                    {
                        array[i] = string.Empty;
                    }
                }
                else
                {
                    array[i] = string.Empty;
                }
            }
            return array;
        }

        /// <summary>
        /// Retorma el total de la longitud del codigo dado el nivel
        /// </summary>
        /// <param name="level">nivel</param>
        /// <returns></returns>
        public int CodeLength(int level)
        {
            int[] lengths=this.LevelLengths();
            int res = 0;
            for (int i = 0; (i < level && i < lengths.Count()); i++)
            {
                res += lengths[i];
            }
            return res;
        }

        /// <summary>
        /// Dada la longitud de un código devuelve a que nivel pertenece.
        /// Si no concuerda con la jerarquia, retorna 0
        /// </summary>
        /// <param name="codeLength">Longitud del codigo</param>
        /// <returns>Nivel de la jerarquia iniciandoen 1</returns>
        public int LengthToLevel(int codeLength){
            int[] lengths=this.LevelLengths();
            int sum=0;
            int level=1;
            foreach (int length in lengths){
                sum+=length;
                if (sum==codeLength)
                    return level;
                level++;
            }
            return 0;
        }

        /// <summary>
        /// Cantidad de niveles usados
        /// </summary>
        /// <returns>Numero de niveles usados en esa tabla</returns>
        public int LevelsUsed()
        {
            return this.LevelLengths().Where(x => x > 0).Count();
        }

        #endregion

        #region static

        public const int MaxLevels = 10;

        #endregion

    }
}
