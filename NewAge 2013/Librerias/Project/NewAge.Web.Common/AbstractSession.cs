using System.Collections.Generic;
using System.Dynamic;
using NewAge.Web.Common.StorageObjects;
using NewAge.Web.Common.StorageObjects.Definitions;

namespace NewAge.Web.Common
{
    public class AbstractSession : DynamicObject
    {
        /// <summary>
        /// Storage
        /// </summary>
        private readonly IStorageObject _storage;

        /// <summary>
        /// Enables derived types to initialize a new instance of the <see cref="T:System.Dynamic.DynamicObject"/> type.
        /// </summary>
        public AbstractSession(IStorageObject storage)
        {
            _storage = storage;
        }

        /// <summary>
        /// When a new property is set, 
        /// add the property name and value to the dictionary
        /// </summary>     
        public override bool TrySetMember (SetMemberBinder binder, object value)
        {
            _storage.Add(binder.Name, value);

            return true;
        }

        /// <summary>
        /// When user accesses something, return the value if we have it
        /// </summary>      
        public override bool TryGetMember (GetMemberBinder binder, out object result)
        {
            result = _storage.Get(binder.Name);
            return true;
        }


        /// <summary>
        /// Return all dynamic member names
        /// </summary>
        /// <returns/>
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _storage.GetKeys();
        }
    }
}