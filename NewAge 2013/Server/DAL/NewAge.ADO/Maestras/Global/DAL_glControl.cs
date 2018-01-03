using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.Resultados;
using NewAge.Librerias.Project;
using System.Reflection;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO
{
    public class DAL_glControl : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_glControl(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones públicas

        /// <summary>
        /// Actualiza glControl
        /// </summary>
        /// <param name="control">control</param> 
        /// <returns>Retorna una respuesta TxResult</returns>
        public void DAL_glControl_Update(DTO_glControl control)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                    mySqlCommand.CommandText =
                      "UPDATE glControl " +
                      "SET Descriptivo = @Descriptivo, Data = @Data " +
                      "WHERE glControlID = @glControlID";

                    mySqlCommand.Parameters.Add("@glControlID", SqlDbType.Int);
                    mySqlCommand.Parameters.Add("@Descriptivo", SqlDbType.VarChar, 1024);
                    mySqlCommand.Parameters.Add("@Data", SqlDbType.VarChar, 255);

                    mySqlCommand.Parameters["@glControlID"].Value = control.glControlID.Value;
                    mySqlCommand.Parameters["@Descriptivo"].Value = control.Descriptivo.Value;
                    mySqlCommand.Parameters["@Data"].Value = control.Data.Value;
                

                mySqlCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glControl_Update");
                throw exception;
            }
        }

        /// <summary>
        /// Trae todos los glControl de una empresa
        /// </summary>
        /// <param name="isBasic">Indica si solo trae la informacion basica</param>
        /// <param name="numEmpresa">Numero de control de una empresa</param>
        /// <returns>enumeracion de glControl</returns>
        public IEnumerable<DTO_glControl> glControl_GetByNumeroEmpresa(bool isBasic, string numEmpresa)
        {
            try
            {
                List<DTO_glControl> controles = new List<DTO_glControl>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                    "select * from glControl where LEN(glControlID) = 2 ";

                if(!isBasic)
                    mySqlCommand.CommandText +=
                        "UNION ALL " +
                        "select * from glControl where glControlID like '" + numEmpresa + "%' ";               

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    DTO_glControl control = new DTO_glControl(dr);
                    controles.Add(control);
                }
                dr.Close();

                return controles;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glControl_GetAll");
                throw exception;
            }
        }

        /// <summary>
        ///  Trae una fila de la tabla de control de acuerdo a un id
        /// </summary>
        /// <param name="controlId">ID de control</param>
        /// <returns>DTO control encontrado</returns>
        public DTO_glControl DAL_glControl_GetById(int controlId)
        {
            try
            {
                List<DTO_glControl> controles = new List<DTO_glControl>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                  "SELECT * FROM glControl WITH(nolock) WHERE glControlID = @controlId";

                mySqlCommand.Parameters.Add("@controlId", SqlDbType.Int);
                mySqlCommand.Parameters["@controlId"].Value = controlId;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                DTO_glControl res = null;

                if (dr.Read())
                {
                    res= new DTO_glControl(dr);
                }
                dr.Close();
                return res;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glControl_GetById");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza una lista de registros 
        /// </summary>
        /// <param name="data">Diccionario con la lista de datos "Llave,Valor"</param>
        public void DAL_glControl_UpdateModuleData(Dictionary<string, string> data)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "UPDATE glControl SET Data=@Data WHERE glControlID = @glControlID";

                mySqlCommand.Parameters.Add("@glControlID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Data", SqlDbType.VarChar, 255);

                foreach (var ctrl in data)
                {
                    mySqlCommand.Parameters["@glControlID"].Value = ctrl.Key;
                    mySqlCommand.Parameters["@Data"].Value = ctrl.Value;

                    mySqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glControl_UpdateModuleData");
                throw exception;
            }
        }

        #endregion

        #region Funciones internas

        /// <summary>
        /// Devuelve un valor segun la empresa
        /// </summary>
        /// <param name="mod">Modulo del sistema</param>
        /// <param name="ctrl">Control que se desea consultar</param>
        /// <returns>Retorna el valor buscado</returns>
        internal string GetControlValueByCompany(ModulesPrefix mod, string ctrl)
        {
            string modValue = ((int)mod).ToString();

            if (modValue.Length == 1)
                modValue = "0" + modValue;

            string empValue = this.Empresa.NumeroControl.Value;
            string key = empValue + modValue + ctrl;

            string result = this.DAL_glControl_GetById(Convert.ToInt32(key)).Data.Value;
            return result;
        }

        #endregion

        #region Funciones del servidor

        /// <summary>
        /// Agrega los valores de una empresa para una tabla de control
        /// </summary>
        /// <param name="id">Identificador de la empresa</param>
        /// <param name="desc">Descripcion de la empresa</param>
        /// <param name="numControl">Numero de control para la empresa</param>
        public void AddCompanyValues(string desc, string numControl, string numControlCopia)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                List<DTO_glControl> controles = new List<DTO_glControl>();
                #region Trae la lista de controles de la empresa copia
                mySqlCommand.CommandText = "select * from glControl with(nolock) where glControlID like @glControlID";

                mySqlCommand.Parameters.Add("@glControlID", SqlDbType.Char);
                mySqlCommand.Parameters["@glControlID"].Value = numControlCopia + "%";

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_glControl control = new DTO_glControl(dr);
                    controles.Add(control);
                }
                dr.Close();
                #endregion
                #region Crea los registros de glControl para la nueva empresa

                mySqlCommand.Parameters.Clear();
                mySqlCommand.CommandText =
                  "INSERT INTO glControl (glControlID, Descriptivo, Data, CtrlVersion) " +
                  "VALUES (@glControlID, @Descriptivo, @Data, @CtrlVersion)";

                mySqlCommand.Parameters.Add("@glControlID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Descriptivo", SqlDbType.VarChar, 1024);
                mySqlCommand.Parameters.Add("@Data", SqlDbType.VarChar, 255);
                mySqlCommand.Parameters.Add("@CtrlVersion", SqlDbType.SmallInt);
                string codID = string.Empty;
                string newCode = string.Empty;
                int i = 0;
                foreach (DTO_glControl ctrl in controles)
                {
                    codID = ctrl.glControlID.Value.Value.ToString();
                    newCode = numControl + codID.Substring(4);

                    mySqlCommand.Parameters["@glControlID"].Value = newCode;
                    mySqlCommand.Parameters["@Data"].Value = ctrl.Data.Value;
                    mySqlCommand.Parameters["@CtrlVersion"].Value = 1;

                    if (i == 0)
                        mySqlCommand.Parameters["@Descriptivo"].Value = desc;
                    else
                        mySqlCommand.Parameters["@Descriptivo"].Value = ctrl.Descriptivo.Value;

                    mySqlCommand.ExecuteNonQuery();
                    ++i;
                }
                #endregion

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddControl, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glControl_AddCompanyValues");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina de la tabla de control los registros de una empresa
        /// </summary>
        /// <param name="numControl">Numero de control para la empresa</param>
        public void DeleteCompanyValues(string numControl)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "DELETE FROM glControl WHERE glControlID LIKE '%" + numControl + "'";

                //mySqlCommand.Parameters.Add("@glControlID", SqlDbType.Int);
                //mySqlCommand.Parameters["@glControlID"].Value = Convert.ToInt32(empKey);

                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteEmpControl, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glControl_DeleteCompanyValues");
                throw exception;
            }
        }

        #endregion
    }
}
