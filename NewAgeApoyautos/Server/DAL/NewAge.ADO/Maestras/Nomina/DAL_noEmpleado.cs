using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.UDT;
using NewAge.DTO.Negocio;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using System.Diagnostics;

namespace NewAge.ADO
{
    public class DAL_noEmpleado : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_noEmpleado(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones Publicas
        
        /// <summary>
        /// Generar un listado de empleados de acuerdo al periodo y el estado de la liquidación
        /// </summary>
        /// <param name="documentoID">ID del documento</param>
        /// <param name="periodo">perido</param>
        /// <param name="estadoLiquidacion">estado de la liquidación</param>
        /// <param name="procesadaInd">indica si el documento ya fue procesada</param>
        /// <param name="estadoEmpleado">estado del empleado</param>
        /// <returns>listado de empleados</returns>
        public List<DTO_noEmpleado> DAL_noEmpleadoGet(int documentoID, DateTime periodo, byte estadoLiquidacion, bool procesadaInd, byte estadoEmpleado)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = " SELECT  noEmpleado.*    " +
                                              "   FROM	glDocumentoControl    " +
                                              "   INNER JOIN noLiquidacionesDocu ON noLiquidacionesDocu.NumeroDoc = glDocumentoControl.NumeroDoc    " +
                                              "   INNER JOIN noEmpleado ON noEmpleado.TerceroID = glDocumentoControl.TerceroID    " +
                                              "   WHERE	glDocumentoControl.DocumentoID = @DocumentoID     " +
                                                       "  AND glDocumentoControl.PeriodoDoc = @PeriodoDoc    " +
                                                       "  AND glDocumentoControl.Estado = @EstadoLiquidacion    " +
                                                       "  AND noLiquidacionesDocu.ProcesadoInd = @IndProcesado    " +
		                                               "  AND noEmpleado.Estado = @EstadoEmpleado";

                mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PeriodoDoc", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@EstadoLiquidacion", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@EstadoEmpleado", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@IndProcesado", SqlDbType.TinyInt);

                mySqlCommandSel.Parameters["@DocumentoID"].Value = documentoID;
                mySqlCommandSel.Parameters["@PeriodoDoc"].Value = periodo;
                mySqlCommandSel.Parameters["@EstadoLiquidacion"].Value = estadoLiquidacion;
                mySqlCommandSel.Parameters["@EstadoEmpleado"].Value = estadoEmpleado;
                mySqlCommandSel.Parameters["@IndProcesado"].Value = procesadaInd ? 1 : 0;

                List<DTO_noEmpleado> result = new List<DTO_noEmpleado>();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_aplMaestraPropiedades p = StaticMethods.GetParameters(base.MySqlConnection, base.MySqlConnectionTx, AppMasters.noEmpleado, this.loggerConnectionStr);
                    DTO_noEmpleado dto = new DTO_noEmpleado(dr, p, true);
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noEmpleadoGet");
                throw exception;
            }        
        }

        /// <summary>
        /// Lista los empleados segun el estado
        /// </summary>
        /// <param name="activoInd">estado</param>
        /// <param name="empleado">empleado</param>
        /// <returns>lista de empleados</returns>
        public List<DTO_noEmpleado> DAL_noEmpleado_SearchEmpleados(bool activoInd, string empleado)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = " SELECT [EmpresaGrupoID], [EmpleadoID], [Descriptivo], [TerceroID], [QuincenalInd], [eg_coTercero], [ActivoInd]   " +
                                               ",[CtrlVersion], [ReplicaID] FROM noEmpleado with(nolock) WHERE ActivoInd = @activoInd   " +
                                               " AND EmpleadoID = @EmpleadoID  ";

                mySqlCommandSel.Parameters.Add("@ActivoInd", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters["@ActivoInd"].Value = activoInd;
                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char);
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = empleado;

                List<DTO_noEmpleado> result = new List<DTO_noEmpleado>();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_aplMaestraPropiedades p = StaticMethods.GetParameters(base.MySqlConnection, base.MySqlConnectionTx, AppMasters.noEmpleado, this.loggerConnectionStr);
                    DTO_noEmpleado dto = new DTO_noEmpleado(dr, p, false);
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "noEmpleado_GetEmpleados");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene el numero de contrato para ingreso o reingreso de empleados
        /// </summary>
        /// <returns></returns>
        public int DAL_noEmpleado_GetNewContratoID()
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "  select ISNULL(MAX(ContratoNOID), 1) from noEmpleado ";

                int numContratoID = (int)mySqlCommandSel.ExecuteScalar();
                return numContratoID + 1;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noEmpleado_GetNewContratoID");
                throw exception;
            }
        }

        /// <summary>
        /// Trae el numero de veces que un TerceroID se repite
        /// </summary>
        /// <param name="empleadoID">tercero</param>
        /// <returns>un contador con la cantidad de veces que el tercero se repitio</returns>
        public int DAL_noEmpleado_CountTerceroID(string tercero)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "select COUNT(TerceroID) AS Contador from noEmpleado with (nolock) where TerceroID=@TerceroID "+
                    " and EmpresaGrupoID=@EmpresaID ";

                mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters["@TerceroID"].Value = tercero;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value; 

                int count = (int)mySqlCommandSel.ExecuteScalar();
                return count;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noEmpleado_CountTerceroID");
                throw exception;
            }
        }
        
        /// <summary>
        /// Determina si el sabado es día laboral para el empleado
        /// </summary>
        /// <param name="empleadoID">identificador del empleado</param>
        /// <returns>true si el sabado es día laboral</returns>
        public bool DAL_NoEmpleado_IsSabLaboralByEmpleado(string empleadoID)
        { 
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText =   "   Select noRol.SabadoLaboralInd   " +
                                                "   from   " +
                                                "   noEmpleado    " +
                                                "   INNER JOIN noRol ON noEmpleado.RolNOID = noEmpleado.RolNOID " +
                                                "   WHERE noEmpleado.EmpleadoID = @EmpleadoID";

                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = empleadoID;            

                byte isSabado = (byte)mySqlCommandSel.ExecuteScalar();
                return isSabado == 1 ? true : false;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_NoEmpleado_IsSabLaboralByEmpleado");
                throw exception;
            }        
        }

        /// <summary>
        /// Trae el estado de las liquidaciones del empleado del periodo de liquidación en curso
        /// </summary>
        /// <param name="empleadoID">empleadoID</param>
        /// <returns>Estado Liquidaciones</returns>
        public DTO_noEstadoLiquidaciones DAL_noEmpleadoEstadoLiquidaciones(string empleadoID)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = new SqlCommand("Nomina_PrerrequisitosLiquidaciones", base.MySqlConnection.CreateCommand().Connection);
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandType = CommandType.StoredProcedure;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = empleadoID;

                DTO_noEstadoLiquidaciones result = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_noEstadoLiquidaciones(dr);
                }
                dr.Close();
                return result;
               
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noEmpleadoEstadoLiquidaciones");
                throw exception;
            }
        }

        #endregion
    }
}
