using System.Collections.Generic;

namespace NewAge.Web.Common.StorageObjects.Definitions
{
    public interface IStorageObject
    {
        /// <summary>
        /// Returns an object by the given key
        /// </summary>
        object Get(string keyName);

        /// <summary>
        /// Adds a new object with the given key
        /// </summary>
        bool Add(string keyName, object value);

        /// <summary>
        /// Gets the list ok all the keys.
        /// </summary>
        IEnumerable<string> GetKeys();

        /// <summary>
        /// Checks if an object with the key is alredy added
        /// </summary>
        bool ContainsKey(string keyName);
    }
}