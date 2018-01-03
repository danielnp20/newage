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
    public class DAL_noPrestamo : DAL_Base
    {
      /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_noPrestamo(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }
        
        /// <summary>
        /// Obtiene prestamos asociados al empleados
        /// </summary>
        /// <param name="empleadoID">identicador de empleados</param>
        /// <returns>listado de prestamos</returns>
        public List<DTO_noPrestamo> DAL_noPrestamo_GetPrestamos(string empleadoID)
        {        
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText =     "   SELECT [EmpresaID]  " +
                                                  "   ,[EmpleadoID]  " +
                                                  "   ,[Numero]  " +
                                                  "   ,[Fecha]  " +
                                                  "   ,[VlrPrestamo]  " +
                                                  "   ,[VlrCuota]  " +
                                                  "   ,[DtoPrima]  " +
                                                  "   ,[VlrAbono]  " +
                                                  "   ,[QuincenaPagos]  " +
                                                  "   ,[ActivaInd]  " +
                                                  "   ,[eg_noEmpleado]  " +
                                                  "   , noConceptoNOM.ConceptoNOID " +
                                                  "   , noConceptoNOM.Descriptivo " +
                                                  "   ,[DocPrestamo] " +
                                                  "   ,[DocCxP] " +
                                                  "   FROM noPrestamo with(nolock)  " +
                                                  "   INNER JOIN noConceptoNOM on noConceptoNOM.ConceptoNOID = noPrestamo.ConceptoNOID  " +
                                                  "   where EmpleadoID = @EmpleadoID";

                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char);
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = empleadoID;

                List<DTO_noPrestamo> result = new List<DTO_noPrestamo>();
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_noPrestamo dto = new DTO_noPrestamo(dr);
                    dto.VlrSaldo.Value = Convert.ToDecimal(dto.VlrPrestamo.Value);
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noPrestamo_GetPrestamos");
                throw exception;
            }           
        }
        
        /// <summary>
        /// Adiciona un Prestamo
        /// </summary>
        /// <param name="prestamo">objeto prestamo</param>
        /// <returns>indica si la operacion se realizo con exito</returns>
        public bool DAL_noPrestamo_AddPrestamo(DTO_noPrestamo prestamo)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "    INSERT INTO noPrestamo " +
                                                "    ([EmpresaID] " +
                                                "    ,[EmpleadoID] " +
                                                "    ,[Fecha] " +
                                                "    ,[VlrPrestamo] " +
                                                "    ,[VlrCuota] " +
                                                "    ,[DtoPrima] " +
                                                "    ,[VlrAbono] " +
                                                "    ,[ActivaInd] " +
                                                "    ,[QuincenaPagos] " +
                                                "    ,[eg_noEmpleado] " +
                                                "    ,[ConceptoNOID] " +
                                                "    ,[DocPrestamo] " +
                                                "    ,[DocCxP] " +
                                                "    ,[eg_noConceptoNOM]) " +
                                                "    VALUES " +
                                                "    (@EmpresaID " +
                                                "    ,@EmpleadoID " +
                                                "    ,@Fecha " +
                                                "    ,@VlrPrestamo " +
                                                "    ,@VlrCuota " +
                                                "    ,@DtoPrima " +
                                                "    ,@VlrAbono " +
                                                "    ,@ActivaInd " +
                                                "    ,@QuincenaPagos " +
                                                "    ,@eg_noEmpleado " +
                                                "    ,@ConceptoNOID " +
                                                "    ,@DocPrestamo " +
                                                "    ,@DocCxP " +
                                                "    ,@eg_noConceptoNOM) ";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Fecha", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@VlrPrestamo", SqlDbType.Char, 20);
                mySqlCommandSel.Parameters.Add("@VlrCuota", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@DtoPrima", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrAbono", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@ActivaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@QuincenaPagos", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@eg_noEmpleado", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ConceptoNOID", SqlDbType.Char, UDT_ConceptoNOID.MaxLength);
                mySqlCommandSel.Parameters.Add("@DocPrestamo", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DocCxP", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@eg_noConceptoNOM", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = prestamo.EmpresaID.Value;
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = prestamo.EmpleadoID.Value;
                mySqlCommandSel.Parameters["@Fecha"].Value = prestamo.FechaPrestamo.Value;
                mySqlCommandSel.Parameters["@VlrPrestamo"].Value = prestamo.VlrPrestamo.Value;
                mySqlCommandSel.Parameters["@VlrCuota"].Value = prestamo.VlrCuota.Value;
                mySqlCommandSel.Parameters["@DtoPrima"].Value = prestamo.DtoPrima.Value;
                mySqlCommandSel.Parameters["@VlrAbono"].Value = prestamo.VlrAbono.Value;
                mySqlCommandSel.Parameters["@ActivaInd"].Value = prestamo.ActivoInd.Value;
                mySqlCommandSel.Parameters["@QuincenaPagos"].Value = prestamo.QuincenaPagos.Value;
                mySqlCommandSel.Parameters["@eg_noEmpleado"].Value = egCtrl;
                mySqlCommandSel.Parameters["@ConceptoNOID"].Value = prestamo.ConceptoNOID.Value;
                mySqlCommandSel.Parameters["@DocPrestamo"].Value = prestamo.DocPrestamo.Value;
                mySqlCommandSel.Parameters["@DocCxP"].Value = prestamo.DocCxP.Value;
                mySqlCommandSel.Parameters["@eg_noConceptoNOM"].Value = egCtrl;

                mySqlCommandSel.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "NovedadesNomina_GetNovedades");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza un pretamo de nomina
        /// </summary>
        /// <param name="prestamo">prestamo</param>
        /// <returns>true si la operacion es exitosa</returns>
        public bool DAL_noPrestamo_UpdPrestamo(DTO_noPrestamo prestamo)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "  UPDATE noPrestamo  " +
                                              "  SET [Fecha] = @Fecha  " +
                                              "  ,[VlrPrestamo] = @VlrPrestamo  " +
                                              "  ,[VlrCuota] = @VlrCuota  " +
                                              "  ,[DtoPrima] = @DtoPrima  " +
                                              "  ,[VlrAbono] = @VlrAbono  " +
                                              "  ,[QuincenaPagos] = @QuincenaPagos  " +
                                              "  ,[ActivaInd] = @ActivaInd  " +
                                              "  WHERE EmpresaID = @EmpresaID  " +
                                              "  AND EmpleadoID = @EmpleadoID  " +
                                              "  AND	Numero = @Numero";

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Numero", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Fecha", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@VlrPrestamo", SqlDbType.Char);
                mySqlCommandSel.Parameters.Add("@VlrCuota", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@DtoPrima", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VlrAbono", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@QuincenaPagos", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@ActivaInd", SqlDbType.Bit);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = prestamo.EmpresaID.Value;
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = prestamo.EmpleadoID.Value;
                mySqlCommandSel.Parameters["@Numero"].Value = prestamo.Numero.Value;
                mySqlCommandSel.Parameters["@Fecha"].Value = prestamo.FechaPrestamo.Value.Value;
                mySqlCommandSel.Parameters["@VlrPrestamo"].Value = prestamo.VlrPrestamo.Value;
                mySqlCommandSel.Parameters["@VlrCuota"].Value = prestamo.VlrCuota.Value;
                mySqlCommandSel.Parameters["@DtoPrima"].Value = prestamo.DtoPrima.Value;
                mySqlCommandSel.Parameters["@VlrAbono"].Value = prestamo.VlrAbono.Value.Value;
                mySqlCommandSel.Parameters["@QuincenaPagos"].Value = prestamo.QuincenaPagos.Value;
                mySqlCommandSel.Parameters["@ActivaInd"].Value = prestamo.ActivoInd.Value;

                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                mySqlCommandSel.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_noPrestamo_UpdPrestamo");
                throw exception;
            }
        }

        /// <summary>
        /// Verifica si la novedad eexiste
        /// /// </summary>
        /// <param name="empleadoID">detalle novedad</param>
        /// <returns>nunero de novedades</returns>
        public int DAL_noPrestamo_ExistPrestamo(DTO_noPrestamo prestamo)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "  SELECT COUNT(*)   " +
                                              "  FROM noPrestamo   " +
                                              "  WHERE EmpresaID = @EmpresaID  " +
                                              "  AND EmpleadoID = @EmpleadoID  " +
                                              "  AND	Numero = @Numero";
                
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EmpleadoID", SqlDbType.Char, UDT_EmpleadoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Numero", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = prestamo.EmpresaID.Value;
                mySqlCommandSel.Parameters["@EmpleadoID"].Value = prestamo.EmpleadoID.Value;
                mySqlCommandSel.Parameters["@Numero"].Value = prestamo.Numero.Value;

                int count = (int)mySqlCommandSel.ExecuteScalar();
                return count;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "NovedadesNomina_ExistPrestamo");
                throw exception;
            }
        }
        
    }
}
