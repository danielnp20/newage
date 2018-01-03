using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.ADO
{
    public class DAL_noPlanillaDiariaTrabajo : DAL_Base
    {
      /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_noPlanillaDiariaTrabajo(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }


        /// <summary>
        /// Obtiene las planillas diarias de trabajos asociadas al empleado
        /// </summary>
        /// <param name="empleadoID">identicador de empleados</param>
        /// <returns>listado de planillas</returns>
        public List<DTO_noPlanillaDiariaTrabajo> DAL_noPlanillaDiariaTrabajo_GetPlanillaDiaria(string empleadoID)
        {        
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText =     "    SELECT [EmpresaID]    " +
                                                  "    ,[ContratoNOID]    " +
                                                  "    ,[Fecha]    " +
                                                  "    ,[EmpleadoID]    " +
                                                  "    ,[ConceptoNOPlanillaID]    " +
                                                  "    ,[HorasNORDiu]    " +
                                                  "    ,[HorasEXTDiu]    " +
                                                  "    ,[HorasEXTNoc]    " +
                                                  "    ,[HorasRECNoc]    " +
                                                  "    ,[Identificador]    " +
                                                  "    ,[eg_noEmpleado]    " +
                                                  "    ,[eg_noConceptoPlaTra]    " +
                                                  "    FROM noTrabajoDiarioPlanilla    " +
                                                  "    where EmpleadoID = @empleadoID";

                mySqlCommandSel.Parameters.Add("@empleadoID");
                mySqlCommandSel.Parameters["@empleadoID"].Value = empleadoID;

                List<DTO_noPlanillaDiariaTrabajo> result = new List<DTO_noPlanillaDiariaTrabajo>();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_noPlanillaDiariaTrabajo dto = new DTO_noPlanillaDiariaTrabajo(dr);
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noPlanillaDiariaTrabajo_GetPlanillaDiaria");
                throw exception;
            }           
        }
        
        /// <summary>
        /// Obtiene las planillas diarias de trabajos asociadas al empleado
        /// </summary>
        /// <param name="empleadoID">identicador de empleados</param>
        /// <returns>listado de planillas</returns>
        public DTO_noPlanillaDiariaTrabajo DAL_noPlanillaDiariaTrabajo_GetPlanillaDiaria(string empleadoID, DateTime periodo)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "    SELECT [EmpresaID]    " +
                                                  "    ,[ContratoNOID]    " +
                                                  "    ,[Fecha]    " +
                                                  "    ,[EmpleadoID]    " +
                                                  "    ,noConceptoPlaTra.ConceptoNOPlanillaID   " +
                                                  "    ,noConceptoPlaTra.Tipo   "   +
                                                  "    ,[HorasNORDiu]    " +
                                                  "    ,[HorasEXTDiu]    " +
                                                  "    ,[HorasEXTNoc]    " +
                                                  "    ,[HorasRECNoc]    " +
                                                  "    ,[Identificador]    " +
                                                  "    ,[eg_noEmpleado]    " +
                                                  "    ,[eg_noConceptoPlaTra]    " +
                                                  "    FROM noTrabajoDiarioPlanilla     " +
                                                  "    INNER JOIN noConceptoPlaTra on noConceptoPlaTra.ConceptoNOPlanillaID = noTrabajoDiarioPlanilla.ConceptoNOPlanillaID  " +
                                                  "    where EmpleadoID = @EmpleadoID   " +
                                                  "    and Fecha = @Fecha   ";

                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char);
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = empleadoID;
                mySqlCommandSel.Parameters.Add("@Fecha", SqlDbType.DateTime);
                mySqlCommandSel.Parameters["@Fecha"].Value = periodo;

                DTO_noPlanillaDiariaTrabajo result = null;
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_noPlanillaDiariaTrabajo(dr);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noPlanillaDiariaTrabajo_GetPlanillaDiaria");
                throw exception;
            }
        }

        /// <summary>
        /// Adiciona una planilla diaria de trabajo
        /// </summary>
        /// <param name="prestamo">planilla</param>
        /// <returns>indica si la operacion se realizo con exito</returns>
        public bool DAL_noPlanillaDiariaTrabajo_AddPlanillaDiaria(DTO_noPlanillaDiariaTrabajo planilla)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText =  "   INSERT INTO [NewAge].[dbo].[noTrabajoDiarioPlanilla]    " +
                                               "   ([EmpresaID]    " +
                                               "   ,[ContratoNOID]    " +
                                               "   ,[Fecha]    " +
                                               "   ,[EmpleadoID]    " +
                                               "   ,[ConceptoNOPlanillaID]    " +
                                               "   ,[HorasNORDiu]    " +
                                               "   ,[HorasEXTDiu]    " +
                                               "   ,[HorasEXTNoc]    " +
                                               "   ,[HorasRECNoc]    " +
                                               "   ,[eg_noEmpleado]    " +
                                               "   ,[eg_noConceptoPlaTra])    " +
                                               "   VALUES    " +
                                               "   (@EmpresaID    " +
                                               "   ,@ContratoNOID    " +
                                               "   ,@Fecha    " +
                                               "   ,@EmpleadoID    " +
                                               "   ,@ConceptoNOPlanillaID    " +
                                               "   ,@HorasNORDiu    " +
                                               "   ,@HorasEXTDiu    " +
                                               "   ,@HorasEXTNoc    " +
                                               "   ,@HorasRECNoc    " +
                                               "   ,@eg_noEmpleado    " +
                                               "   ,@eg_noConceptoPlaTra)    ";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ContratoNOID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Fecha", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ConceptoNOPlanillaID", SqlDbType.Char, UDT_ConceptoNOPlanillaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@HorasNORDiu", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@HorasEXTDiu", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@HorasEXTNoc", SqlDbType.TinyInt);  
                mySqlCommandSel.Parameters.Add("@HorasRECNoc", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@eg_noEmpleado", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_noConceptoPlaTra", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = planilla.EmpresaID.Value;
                mySqlCommandSel.Parameters["@ContratoNOID"].Value = planilla.ContratoNOID.Value;
                mySqlCommandSel.Parameters["@Fecha"].Value = planilla.FechaPlanilla.Value;
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = planilla.EmpleadoID.Value;
                mySqlCommandSel.Parameters["@ConceptoNOPlanillaID"].Value = planilla.ConceptoNOPlanillaID.Value;
                mySqlCommandSel.Parameters["@HorasNORDiu"].Value = planilla.HorasNORDiu.Value;
                mySqlCommandSel.Parameters["@HorasEXTDiu"].Value = planilla.HorasEXTDiu.Value;
                mySqlCommandSel.Parameters["@HorasEXTNoc"].Value = planilla.HorasEXTNoc.Value; 
                mySqlCommandSel.Parameters["@HorasRECNoc"].Value = planilla.HorasRECNoc.Value; 
                mySqlCommandSel.Parameters["@eg_noEmpleado"].Value = egCtrl;
                mySqlCommandSel.Parameters["@eg_noConceptoPlaTra"].Value = egCtrl;
     

                mySqlCommandSel.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noPlanillaDiariaTrabajo_AddPlanillaDiaria");
                throw exception;
            }
        }
    }
}
