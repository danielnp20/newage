using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewAge.Librerias.ExceptionHandler
{
    public class MentorDataParametersException : Exception
    {
        #region Propiedades

        /// <summary>
        /// Key with the lenght problem
        /// </summary>
        public string Key
        {
            get;
            set;
        }

        /// <summary>
        /// Max Lenght permited over the key
        /// </summary>
        public ExType Type
        {
            get;
            set;
        }

        /// <summary>
        /// Max Lenght permited over the key
        /// </summary>
        public int MaxLenght
        {
            get;
            set;
        }

        public enum ExType
        {
            Lenght = 0,
            Table = 1
        }

        public string FieldName
        {
            get;
            set;
        }

        #endregion

        #region Constructoras

        public MentorDataParametersException(string key, ExType type, int? maxLenght)
        {
            this.Key = key;
            this.Type = type;
            if (maxLenght != null)
            {
                this.MaxLenght = Convert.ToInt32(maxLenght);
            }
        }

        public MentorDataParametersException(string message, ExType type, string key, int? maxLenght)
            : base(message)
        {
            this.Key = key;
            this.Type = type;
            if (maxLenght != null)
            {
                this.MaxLenght = Convert.ToInt32(maxLenght);
            }
        }

        public MentorDataParametersException(string message, Exception inner, ExType type, string key, int? maxLenght)
            : base(message, inner)
        {
            this.Key = key;
            this.Type = type;
            if (maxLenght != null)
            {
                this.MaxLenght = Convert.ToInt32(maxLenght);
            }
        }

        #endregion
    }
}
