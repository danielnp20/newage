using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewAge.Cliente.Proxy.Model
{
    /// <summary>
    /// Lista de los diferentes ambientes con los que puede interactuar el usuario
    /// </summary>
    public enum EnvironmentType
    {
        Web = 1,
        Windows = 2,
        Undetermined = -1
    }

    /// <summary>
    /// Lista de roles
    /// </summary>
    public enum Role
    {
        User = 1,
        Administrator = 2
    }
}