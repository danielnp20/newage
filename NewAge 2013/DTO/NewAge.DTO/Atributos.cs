using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewAge.DTO.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AllowNull : Attribute {}

    [AttributeUsage(AttributeTargets.Property)]
    public class NotImportable : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public class Filtrable : Attribute { }
}
