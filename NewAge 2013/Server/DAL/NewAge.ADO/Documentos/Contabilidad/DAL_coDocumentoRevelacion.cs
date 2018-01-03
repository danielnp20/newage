using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using NewAge.DTO.UDT;

namespace NewAge.ADO
{
    public class DAL_coDocumentoRevelacion : DAL_Base
    {
        // <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_coDocumentoRevelacion(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Metodos

        /// <summary>
        /// Agrega una revelacion 
        /// </summary>
        /// <param name="revelacion">objeto de revelacion</param>
        public void DAL_coDocumentoRevelacion_Add(DTO_coDocumentoRevelacion revelacion)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommandSel.CommandText =
                    "   INSERT INTO coDocumentoRevelacion   " +
                    "          ( NumeroDoc   " +
                    "          , EmpresaID   " +
                    "          , NotaRevelacionID   " +
                    "          , TituloRevelacion   " +
                    "          , Revelacion   " +
                    "          , eg_coReporteNota )   " +
                    "    VALUES   " +
                    "          ( @NumeroDoc   " +
                    "          , @EmpresaID   " +
                    "          , @NotaRevelacionID   " +
                    "          , @TituloRevelacion   " +
                    "          , @Revelacion   " +
                    "          , @eg_coReporteNota ) ";
                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NotaRevelacionID", SqlDbType.Char, UDT_NotaRevelacionID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TituloRevelacion", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@Revelacion", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coReporteNota", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
              
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = revelacion.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = revelacion.EmpresaID.Value;
                mySqlCommandSel.Parameters["@NotaRevelacionID"].Value = revelacion.NotaRevelacionID.Value;
                mySqlCommandSel.Parameters["@TituloRevelacion"].Value = revelacion.TituloRevelacion.Value; 
                mySqlCommandSel.Parameters["@Revelacion"].Value = revelacion.Revelacion.Value;
                mySqlCommandSel.Parameters["@eg_coReporteNota"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coReporteNota, this.Empresa, egCtrl);
             
                #endregion

                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coDocumentoRevelacion_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene un documento revelación por numero de documento
        /// </summary>
        ///<param name="numeroDoc">número de documento</param>
        ///<returns>Revelación</returns>
        public DTO_coDocumentoRevelacion DAL_coDocumentoRevelacion_Get(int numeroDoc)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommandSel.CommandText =
                    "   SELECT * FROM coDocumentoRevelacion " +
                    "   WHERE NumeroDoc = @NumeroDoc ";
                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
         
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;
           
                #endregion

                DTO_coDocumentoRevelacion result = null; 
                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_coDocumentoRevelacion(dr);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coDocumentoRevelacion_Get");
                throw exception;
            }
        }

        #endregion 

    }
}
