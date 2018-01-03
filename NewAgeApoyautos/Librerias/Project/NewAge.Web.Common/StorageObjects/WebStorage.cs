using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewAge.Web.Common.StorageObjects.Definitions;

namespace NewAge.Web.Common.StorageObjects
{
    /// <summary>
    /// WebStorage: Implements storage using the HTTP Session object
    /// </summary>
    public class WebStorage : IStorageObject
    {
        #region Implementation of IStorageObject

        /// <summary>
        /// Returns an object by the given key
        /// </summary>
        public object Get(string keyName)
        {
            return HttpContext.Current.Session[keyName];
        }

        /// <summary>
        /// Adds a new object with the given key
        /// </summary>
        public bool Add(string keyName, object value)
        {
            HttpContext.Current.Session[keyName] = value;
            return true;
        }

        /// <summary>
        /// Gets the list ok all the keys
        /// </summary>
        public IEnumerable<string> GetKeys()
        {
            return (from object key in HttpContext.Current.Session.Keys select key as string).ToList();
        }

        /// <summary>
        /// Checks if an object with the key is alredy added
        /// </summary>
        public bool ContainsKey(string keyName)
        {
            return HttpContext.Current.Session[keyName] != null;
        }

        #endregion
    }
}