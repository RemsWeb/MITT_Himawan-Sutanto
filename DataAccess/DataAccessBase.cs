using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
namespace MITT_HIMAWAN_SUTANTO.DataAccess
{

    public class DataAccessBase
    {
        private string _connectionString = "";
        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
            set
            {
                _connectionString = value;
            }
        }


        private void AssignParameters(SqlCommand cmd, object[] parameterValues)
        {
            if (!(cmd.Parameters.Count - 1 == parameterValues.Length))
                throw new ApplicationException("Stored procedure's parameters and parameter values does not match.");
            int i = 0;
            foreach (SqlParameter param in cmd.Parameters)
            {
                if (!(param.Direction == ParameterDirection.Output) && !(param.Direction == ParameterDirection.ReturnValue))
                {
                    param.Value = parameterValues[i];
                    i += 1;
                }
            }
        }

        private void AssignParameters(SqlCommand cmd, SqlParameter[] cmdParameters)
        {
            if ((cmdParameters == null))
                return;
            foreach (SqlParameter p in cmdParameters)
                cmd.Parameters.Add(p);
        }

        /// <summary>
        ///     ''' Adds or refreshes rows in the System.Data.DataSet to match those in the data source using the System.Data.DataSet name, and creates a System.Data.DataTable named "Table."
        ///     ''' </summary>
        ///     ''' <param name="cmd">The Transact-SQL statement or stored procedure to execute at the data source.</param>
        ///     ''' <param name="cmdType">A value indicating how the System.Data.SqlClient.SqlCommand.CommandText property is to be interpreted.</param>
        ///     ''' <param name="parameters">The parameters of the Transact-SQL statement or stored procedure.</param>
        ///     ''' <returns>A System.Data.Dataset object.</returns>
        public DataTable FillDataset(string conn, string cmd, CommandType cmdType, SqlParameter[] parameters = null)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataAdapter sqlda = null;
            DataTable res = new DataTable();
            try
            {
                connection = new SqlConnection(conn);
                command = new SqlCommand(cmd, connection);
                command.CommandType = cmdType;
                AssignParameters(command, parameters);
                sqlda = new SqlDataAdapter(command);

                command.CommandTimeout = 7200;

                sqlda.Fill(res);
            }
            catch (Exception ex)
            {
                throw new System.Data.DataException(ex.Message, ex.InnerException);
            }
            finally
            {
                if (!(connection == null))
                    connection.Dispose();
                if (!(command == null))
                    command.Dispose();
                if (!(sqlda == null))
                    sqlda.Dispose();
            }
            return res;
        }



        public int ExecuteNonQuery(string conn, string cmd, CommandType cmdType, SqlParameter[] parameters = null)
        {
            SqlConnection connection = null;
            SqlTransaction transaction = null;
            SqlCommand command = null;
            int res = -1;
            try
            {
                connection = new SqlConnection(conn);
                command = new SqlCommand(cmd, connection);
                command.CommandType = cmdType;
                this.AssignParameters(command, parameters);
                connection.Open();
                // transaction = connection.BeginTransaction()
                // command.Transaction = transaction
                res = command.ExecuteNonQuery();
            }
            // transaction.Commit()
            catch (Exception ex)
            {
                // If Not (transaction Is Nothing) Then
                // transaction.Rollback()
                // End If
                // Throw New Data.DataException(ex.Message, ex.InnerException)
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                if (!(connection == null) && (connection.State == ConnectionState.Open))
                    connection.Close();
                if (!(command == null))
                    command.Dispose();
                if (!(transaction == null))
                    transaction.Dispose();
            }
            return res;
        }
    }
}