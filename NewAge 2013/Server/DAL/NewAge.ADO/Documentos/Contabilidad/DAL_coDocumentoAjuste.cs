using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL Documento Ajuste
    /// </summary>
    public class DAL_coDocumentoAjuste : DAL_Base
    {
       /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_coDocumentoAjuste(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }
        
        #region Funciones Públicas

        /// <summary>
        /// Consulta una cuenta por pagar segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Control</param>
        /// <returns></returns>
        public DTO_coDocumentoAjuste DAL_coDocumentoAjuste_Get(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from coDocumentoAjuste with(nolock) where NumeroDoc = @NumeroDoc ";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                DTO_coDocumentoAjuste result = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_coDocumentoAjuste(dr);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coDocumentoAjuste_Get");
                throw exception;
            }
        }

        /// <summary>
        /// adiciona en tabla coDocumentoAjuste 
        /// </summary>
        /// <param name="doc">documento</param>
        /// <param name="userId">usuario digitador</param>
        /// <returns></returns>
        public void DAL_coDocumentoAjuste_Add(DTO_coDocumentoAjuste doc)
        {
            try
            {
            string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
            // string msg_FkNotFound = DictionaryMessages.FkNotFound;

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
   
                #region CommandText
                mySqlCommand.CommandText = "INSERT INTO coDocumentoAjuste (EmpresaID, NumeroDoc, IdentificadorTR, Valor) " +
                                           "VALUES (@EmpresaID, @NumeroDoc, @IdentificadorTR, @Valor)";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@IdentificadorTR", SqlDbType.BigInt);
                mySqlCommand.Parameters.Add("@Valor", SqlDbType.Decimal);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = doc.NumeroDoc.Value;
                mySqlCommand.Parameters["@IdentificadorTR"].Value = doc.IdentificadorTR.Value;
                mySqlCommand.Parameters["@Valor"].Value = doc.Valor.Value;               
                #endregion

                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coDocumentoAjuste_Add");
                throw exception;
            }          
        
        }          

        /// <summary>
        /// Actualizar el documento en tabla coDocumentoAjuste y asociar en glDocumentoControl
        /// </summary>
        /// <param name="doc">documento</param>
        /// <param name="userId">usuario digitador</param>
        /// <returns></returns>
        //public void DAL_coDocumentoAjuste_Upd(DTO_coDocumentoAjuste doc)
        //{            
        //    try
        //    {
        //        string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
        //        string msg_FkNotFound = DictionaryMessages.FkNotFound;

        //        //Actualiza Tabla coDocumentoAjuste
        //        #region CommandText
        //        SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
        //        mySqlCommand.Transaction = base.MySqlConnectionTx;

        //        mySqlCommand.CommandText = "UPDATE coDocumentoAjuste " +
        //                                   "SET EmpresaID = @EmpresaID, IdentificadorTR = @IdentificadorTR, Valor = @Valor" +                                              
        //                                   "WHERE NumeroDoc = @NumeroDoc";
        //        #endregion
        //        #region Creacion de parametros
        //            mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
        //            mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
        //            mySqlCommand.Parameters.Add("@IdentificadorTR", SqlDbType.Int);
        //            mySqlCommand.Parameters.Add("@Valor", SqlDbType.Decimal);           
        //        #endregion
        //        #region Asignacion de valores
        //            mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
        //            mySqlCommand.Parameters["@NumeroDoc"].Value = doc.NumeroDoc.Value;
        //            mySqlCommand.Parameters["@IdentificadorTR"].Value = doc.IdentificadorTR.Value;
        //            mySqlCommand.Parameters["@Valor"].Value = doc.Valor.Value;
        //        #endregion
        //        mySqlCommand.ExecuteNonQuery();   
          
        //    }
        //    catch (Exception ex)
        //    {
        //        var exception = new Exception(DictionaryMessages.Err_UpdateDocument, ex);
        //        Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coDocumentoAjuste_Upd");
        //        throw exception;
        //    }           

        //}

        #endregion

    }
}
