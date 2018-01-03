using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.DTO.Negocio;
using NewAge.ADO;
using System.Data.SqlClient;

namespace NewAge.Negocio
{
    public class ModuloFachada
    {
        /// <summary>
        /// Segun el modulo devuelve el objeto del modulo correspondiente
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public ModuloBase GetModule(ModulesPrefix module, SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userId, string loggerConn)
        {
            switch (module)
            {
                case ModulesPrefix.ac:
                    return new ModuloActivosFijos(conn, tx, emp, userId, loggerConn);
                case ModulesPrefix.apl:
                    return new ModuloAplicacion(conn, tx, emp, userId, loggerConn);
                case ModulesPrefix.cc:
                    return new ModuloCartera(conn, tx, emp, userId, loggerConn);
                case ModulesPrefix.cf:
                    return new ModuloCarteraFin(conn, tx, emp, userId, loggerConn);
                case ModulesPrefix.co:
                    return new ModuloContabilidad(conn, tx, emp, userId, loggerConn);
                case ModulesPrefix.cp:
                    return new ModuloCuentasXPagar(conn, tx, emp, userId, loggerConn);
                case ModulesPrefix.gl:
                    return new ModuloGlobal(conn, tx, emp, userId, loggerConn);
                case ModulesPrefix.ts:
                    return new ModuloTesoreria(conn, tx, emp, userId, loggerConn);
                case ModulesPrefix.fa:
                    return new ModuloFacturacion(conn, tx, emp, userId, loggerConn);
                case ModulesPrefix.pr:
                    return new ModuloProveedores(conn, tx, emp, userId, loggerConn);
                case ModulesPrefix.@in:
                    return new ModuloInventarios(conn, tx, emp, userId, loggerConn);
                case ModulesPrefix.oc:
                    return new ModuloOpConjuntas(conn, tx, emp, userId, loggerConn);
                case ModulesPrefix.pl:
                    return new ModuloPlaneacion(conn, tx, emp, userId, loggerConn);
                case ModulesPrefix.py:
                    return new ModuloProyectos(conn, tx, emp, userId, loggerConn);
                case ModulesPrefix.dr:
                    return new ModuloDecisorRiesgo(conn, tx, emp, userId, loggerConn);
            }
            return null;
        }
    }
}
