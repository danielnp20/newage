using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using SentenceTransformer;
using NewAge.DTO.UDT;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO
{
    public class DAL_aplBitacora : DAL_Base
    {
            
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_aplBitacora(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones publicas

        /// <summary>
        /// Inserta una bitcora
        /// </summary>
        /// <param name="empresaID">Empresa en la que se esta trabajando</param>
        /// <param name="documentoID">formaID</param>
        /// <param name="accionID">accionID</param>
        /// <param name="fecha">fecha</param>
        /// <param name="seUsuarioID">seUsuarioID</param>
        /// <param name="llp01">llp01</param>
        /// <param name="llp02">llp02</param>
        /// <param name="llp03">llp03</param>
        /// <param name="llp04">llp04</param>
        /// <param name="llp04">llp05</param>
        /// <param name="llp04">llp06</param>
        /// <param name="bitacoraOrigenID">bitacoraOrigenID</param>
        /// <param name="bitacoraPadreID">bitacoraPadreID</param>
        /// <param name="bitacoraAnulacionID">bitacoraAnulacionID</param>
        /// <returns>True si se ingreso False de lo contrario</returns>
        public int DAL_aplBitacora_Add(string empresaID, int documentoID, short accionID, DateTime fecha, int seUsuarioID, string llp01, string llp02, string llp03, string llp04, string llp05, string llp06, int bitacoraOrigenID, int bitacoraPadreID, int bitacoraAnulacionID)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                  "INSERT INTO aplBitacora (" +
                  "  EmpresaID, DocumentoID, AccionID, Fecha, seUsuarioID, llp01, llp02, llp03, llp04, llp05, llp06, BitacoraOrigenID, BitacoraPadreID, BitacoraAnulacionID" +
                  ") VALUES (" +
                  "  @EmpresaID, @DocumentoID, @AccionID, @Fecha, @seUsuarioID, @llp01, @llp02, @llp03, @llp04, @llp05, @llp06, @BitacoraOrigenID, @BitacoraPadreID, @BitacoraAnulacionID" +
                  ") SET @ID = SCOPE_IDENTITY()";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.VarChar, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@AccionID", SqlDbType.SmallInt);
                mySqlCommand.Parameters.Add("@Fecha", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@seUsuarioID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@llp01", SqlDbType.VarChar, 50);
                mySqlCommand.Parameters.Add("@llp02", SqlDbType.VarChar, 50);
                mySqlCommand.Parameters.Add("@llp03", SqlDbType.VarChar, 50);
                mySqlCommand.Parameters.Add("@llp04", SqlDbType.VarChar, 50);
                mySqlCommand.Parameters.Add("@llp05", SqlDbType.VarChar, 50);
                mySqlCommand.Parameters.Add("@llp06", SqlDbType.VarChar, 50);
                mySqlCommand.Parameters.Add("@BitacoraOrigenID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@BitacoraPadreID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@BitacoraAnulacionID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ID", SqlDbType.Int);

                mySqlCommand.Parameters["@EmpresaID"].Value = empresaID;
                mySqlCommand.Parameters["@DocumentoID"].Value = documentoID;
                mySqlCommand.Parameters["@AccionID"].Value = accionID;
                mySqlCommand.Parameters["@Fecha"].Value = fecha;
                mySqlCommand.Parameters["@seUsuarioID"].Value = seUsuarioID;
                mySqlCommand.Parameters["@llp01"].Value = llp01;
                mySqlCommand.Parameters["@llp02"].Value = llp02;
                mySqlCommand.Parameters["@llp03"].Value = llp03;
                mySqlCommand.Parameters["@llp04"].Value = llp04;
                mySqlCommand.Parameters["@llp05"].Value = llp05;
                mySqlCommand.Parameters["@llp06"].Value = llp06;
                mySqlCommand.Parameters["@BitacoraOrigenID"].Value = bitacoraOrigenID;
                mySqlCommand.Parameters["@BitacoraPadreID"].Value = bitacoraPadreID;
                mySqlCommand.Parameters["@BitacoraAnulacionID"].Value = bitacoraAnulacionID;
                mySqlCommand.Parameters["@ID"].Direction = ParameterDirection.Output;

                mySqlCommand.ExecuteNonQuery();

                return Convert.ToInt32(mySqlCommand.Parameters["@ID"].Value);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddBita, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_aplBitacora_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Inserta una bitcora
        /// </summary>
        /// <param name="documentoTipoID">documentoTipoID</param>
        /// <param name="formaID">formaID</param>
        /// <param name="accionID">accionID</param>
        /// <param name="fecha">fecha</param>
        /// <param name="seUsuarioID">seUsuarioID</param>
        /// <param name="pkVals">lista de valores con as llaves primarias</param>
        /// <param name="bitacoraOrigenID">bitacoraOrigenID</param>
        /// <param name="bitacoraPadreID">bitacoraPadreID</param>
        /// <param name="bitacoraAnulacionID">bitacoraAnulacionID</param>
        /// <returns>True si se ingreso False de lo contrario</returns>
        public int DAL_aplBitacora_Add(string empresaID, int documentoID, short accionID, DateTime fecha, int seUsuarioID, string[] pkVals, int bitacoraOrigenID, int bitacoraPadreID, int bitacoraAnulacionID)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                  "INSERT INTO aplBitacora (" +
                  "  EmpresaID, DocumentoID, AccionID, Fecha, seUsuarioID, llp01, llp02, llp03, llp04, llp05, llp06, BitacoraOrigenID, BitacoraPadreID, BitacoraAnulacionID" +
                  ") VALUES (" +
                  "  @EmpresaID, @DocumentoID, @AccionID, @Fecha, @seUsuarioID, @llp01, @llp02, @llp03, @llp04, @llp05, @llp06, @BitacoraOrigenID, @BitacoraPadreID, @BitacoraAnulacionID" +
                  ") SET @ID = SCOPE_IDENTITY()";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.VarChar, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@AccionID", SqlDbType.SmallInt);
                mySqlCommand.Parameters.Add("@Fecha", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@seUsuarioID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@llp01", SqlDbType.VarChar, 50);
                mySqlCommand.Parameters.Add("@llp02", SqlDbType.VarChar, 50);
                mySqlCommand.Parameters.Add("@llp03", SqlDbType.VarChar, 50);
                mySqlCommand.Parameters.Add("@llp04", SqlDbType.VarChar, 50);
                mySqlCommand.Parameters.Add("@llp05", SqlDbType.VarChar, 50);
                mySqlCommand.Parameters.Add("@llp06", SqlDbType.VarChar, 50);
                mySqlCommand.Parameters.Add("@BitacoraOrigenID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@BitacoraPadreID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@BitacoraAnulacionID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ID", SqlDbType.Int);

                mySqlCommand.Parameters["@EmpresaID"].Value = empresaID;
                mySqlCommand.Parameters["@DocumentoID"].Value = documentoID;
                mySqlCommand.Parameters["@AccionID"].Value = accionID;
                mySqlCommand.Parameters["@Fecha"].Value = fecha;
                mySqlCommand.Parameters["@seUsuarioID"].Value = seUsuarioID;
                mySqlCommand.Parameters["@llp01"].Value = pkVals[0];
                mySqlCommand.Parameters["@llp02"].Value = pkVals[1];
                mySqlCommand.Parameters["@llp03"].Value = pkVals[2];
                mySqlCommand.Parameters["@llp04"].Value = pkVals[3];
                mySqlCommand.Parameters["@llp05"].Value = pkVals[4];
                mySqlCommand.Parameters["@llp06"].Value = pkVals[5];
                mySqlCommand.Parameters["@BitacoraOrigenID"].Value = bitacoraOrigenID;
                mySqlCommand.Parameters["@BitacoraPadreID"].Value = bitacoraPadreID;
                mySqlCommand.Parameters["@BitacoraAnulacionID"].Value = bitacoraAnulacionID;
                mySqlCommand.Parameters["@ID"].Direction = ParameterDirection.Output;

                mySqlCommand.ExecuteNonQuery();

                return Convert.ToInt32(mySqlCommand.Parameters["@ID"].Value);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddBita, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_aplBitacora_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Consulta la bitácora dado un filtro por páginas
        /// </summary>
        /// <param name="pageSize">Tamano de la pagina</param>
        /// <param name="pageNum">Numero de la pagina</param>
        /// <param name="consulta">dto con el filtro</param>
        /// <param name="ini">numero pagina inicial</param>
        /// <param name="fin">numero pagina final</param>
        /// <param name="where">string de condiciones para el query</param>
        /// <returns>enumeracion de dtos de bitacora</returns>
        public IEnumerable<DTO_aplBitacora> DAL_aplBitacoraGetFilteredPaged(int pageSize, int pageNum, DTO_glConsulta consulta, int ini, int fin, string where)
        {
            try
            {
                Dictionary<int, DTO_aplBitacora> bitacora = new Dictionary<int, DTO_aplBitacora>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = 
                    "SELECT * FROM "+
                    "( " + 
                    "   SELECT ROW_NUMBER()Over(Order by baseTable.Fecha DESC) As RowNum, baseTable.BitacoraID "+
                    "   FROM aplBitacora baseTable with (nolock) " + where + 
                    ") AS ResultadoPaginado " +
                    "WHERE RowNum BETWEEN " + ini + " AND " + fin;

                SqlDataReader drSel;

                string bicatoraIDs = string.Empty;
                
                drSel = mySqlCommand.ExecuteReader();
                while (drSel.Read())
                {
                    int bitId = Convert.ToInt32(drSel["BitacoraID"]);
                    if (string.IsNullOrEmpty(bicatoraIDs))
                        bicatoraIDs += bitId.ToString();
                    else
                        bicatoraIDs += ", "+bitId.ToString();
                }
                drSel.Close();

                if (!string.IsNullOrWhiteSpace(bicatoraIDs))
                {
                    mySqlCommand.CommandText = 
                        "SELECT temp.*,glEmpresa.Descriptivo EmpresaDesc, seUsuario.UsuarioID UsuarioDesc, glDocumento.Descriptivo DocumentoDesc " + 
                        "FROM " + 
                        "( " + 
                        "   SELECT aplBitacora.*, aplBitacoraAct.NombreCampo, aplBitacoraAct.Valor " +
                        "   FROM aplBitacora with (nolock) " +
                        "       LEFT JOIN aplBitacoraAct with (nolock) ON aplBitacora.BitacoraID=aplBitacoraAct.BitacoraID " +
                        "   WHERE aplBitacora.BitacoraID in (" + bicatoraIDs + ") " +
                        ") temp " +
                        "   Inner Join glEmpresa with (nolock) on temp.EmpresaID = glEmpresa.EmpresaID " +
                        "   Inner Join seUsuario with (nolock) on temp.seUsuarioID=seUsuario.ReplicaID" +
                        "   Inner Join glDocumento  with (nolock) on temp.DocumentoID=glDocumento.DocumentoID " + 
                        "WHERE BitacoraID in (" + bicatoraIDs + ") " +
                        "order by Fecha desc";

                    drSel = mySqlCommand.ExecuteReader();
                    while (drSel.Read())
                    {
                        DTO_aplBitacoraAct btAct = new DTO_aplBitacoraAct(drSel);
                        if (!bitacora.Keys.Contains(btAct.BitacoraID.Value.Value))
                        {
                            DTO_aplBitacora bit = new DTO_aplBitacora(drSel);
                            if (!string.IsNullOrWhiteSpace(btAct.NombreCampo.Value))
                                bit.Actualizaciones.Add(btAct);
                            bitacora.Add(bit.BitacoraID.Value.Value, bit);
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(btAct.NombreCampo.Value))
                                bitacora[btAct.BitacoraID.Value.Value].Actualizaciones.Add(btAct);
                        }
                    }
                    drSel.Close();
                }

                return bitacora.Values.ToList();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_aplBitacoraGetFilteredPaged");
                throw exception;          
            }
        }

        /// <summary>
        /// Trae datos de la bitacora filtrada
        /// </summary>
        /// <param name="consulta">Filtros a aplicar</param>
        /// <param name="where">condiciones del query</param>
        /// <returns>Retorna los registros de la bitacora</returns>
        public IEnumerable<DTO_aplBitacora> DAL_aplBitacoraGetFiltered(DTO_glConsulta consulta, string where)
        {
            try
            {
                Dictionary<int, DTO_aplBitacora> bitacora = new Dictionary<int, DTO_aplBitacora>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "SELECT temp.* FROM (SELECT aplBitacora.*, aplBitacoraAct.NombreCampo, aplBitacoraAct.Valor, glEmpresa.Descriptivo EmpresaDesc, seUsuario.UsuarioID UsuarioDesc, glDocumento.Descriptivo DocumentoDesc FROM aplBitacora LEFT JOIN aplBitacoraAct ON aplBitacora.BitacoraID=aplBitacoraAct.BitacoraID ,glEmpresa,seUsuario,glDocumento WHERE aplBitacora.EmpresaID=glEmpresa.EmpresaID AND aplBitacora.seUsuarioID=seUsuario.ReplicaID AND aplBitacora.DocumentoID=glDocumento.DocumentoID) temp " + where + " order by Fecha desc";

                SqlDataReader drSel;

                drSel = mySqlCommand.ExecuteReader();
                while (drSel.Read())
                {
                    DTO_aplBitacoraAct btAct = new DTO_aplBitacoraAct(drSel);
                    if (!bitacora.Keys.Contains(btAct.BitacoraID.Value.Value))
                    {
                        DTO_aplBitacora bit = new DTO_aplBitacora(drSel);
                        if (!string.IsNullOrWhiteSpace(btAct.NombreCampo.Value))
                        {
                            bit.Actualizaciones.Add(btAct);
                        }
                        bitacora.Add(bit.BitacoraID.Value.Value, bit);
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(btAct.NombreCampo.Value))
                        {
                            bitacora[btAct.BitacoraID.Value.Value].Actualizaciones.Add(btAct);
                        }
                    }
                }
                drSel.Close();

                return bitacora.Values.ToList();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_aplBitacoraGetFiltered");
                throw exception;        
            }
        }

        /// <summary>
        /// Trae la cantidad de registros de la bitacora despues de aplicar un filtro
        /// </summary>
        /// <param name="consulta">Filtros a aplicar</param>
        /// <param name="where">condiciones del query</param>
        /// <returns>Retorna el numero de registros encontrados</returns>
        public long DAL_aplBitacoraCountFiltered(DTO_glConsulta consulta, string where)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "SELECT COUNT(*) res FROM aplBitacora " + where + "";

                SqlDataReader drSel;
                long res = 0;

                drSel = mySqlCommand.ExecuteReader();
                while (drSel.Read())
                {
                    res = Convert.ToInt64(drSel["res"]);
                }
                drSel.Close();

                return res;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_aplBitacoraCountFiltered");
                throw exception;   
            }
        }

        /// <summary>
        /// Dice si una empresa ha sido eliminada
        /// </summary>
        /// <param name="empresaID">empresa a eliminar</param>
        /// <returns>devuelve true si la empresa se elimino correctamente sino devuelve false</returns>
        public bool DAL_aplBitacora_DeleteCompany(string empresaID)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select count(*) from aplBitacora with(nolock) where EmpresaID = @EmpresaID"; 

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.VarChar, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = empresaID;

                int res = mySqlCommand.ExecuteNonQuery();
                return res == 0 ? true : false;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_aplBitacora_DeleteCompany");
                throw exception;  
            }
        }

        #endregion

    }

    public class UDTComparer : IEqualityComparer<UDT>
    {
        public bool Equals(UDT b1, UDT b2)
        {
            return b1.ToString().Equals(b2.ToString());
        }

        public int GetHashCode(UDT bx)
        {
            return bx.ToString().GetHashCode();
        }
    }
}
