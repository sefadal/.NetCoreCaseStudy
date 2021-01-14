using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Utility;

namespace WepApi.DataAccessLayer
{
    public class SqlDataBlock : IDisposable
    {
        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataAdapter da;
        private DataSet ds;

        public SqlDataBlock()
        {
            var configuration = GetConfiguration();
            cnn = new SqlConnection(configuration.GetSection("ConnectionStrings").GetSection("dbConnection").Value);
        }

        IConfiguration GetConfiguration()   
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true);

            return builder.Build();
        }

        public int ExecuteNonQuery(CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(commandType, commandText, null);
        }

        public SqlDataReader ExecuteDataReader(CommandType commandType, string commandText)
        {
            return ExecuteDataReader(commandType, commandText, null);
        }

        public DataTable ExecuteDataTable(CommandType commandType, string commandText)
        {
            return ExecuteDataTable(commandType, commandText, null);
        }

        public DataSet ExecuteDataSet(CommandType commandType, string commandText)
        {
            return ExecuteDataSet(commandType, commandText, null);
        }

        public int ExecuteNonQuery(CommandType commandType, string commandText, params SqlParameter[] parameters)
        {
            int result = 0;

            try
            {
                cmd = new SqlCommand(commandText, cnn);
                cmd.CommandType = commandType;

                if (parameters != null)
                {
                    foreach (SqlParameter parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }
                }

                OpenConnection();

                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                FileProccess.WriteLog(ex.Message);
            }
            finally
            {
                CloseConnection();
            }

            return result;
        }

        public int ExecuteNonQuery(CommandType commandType, string commandText, SqlTransaction tran, SqlConnection con, params SqlParameter[] parameters)
        {
            int result = 0;

            try
            {
                cmd = new SqlCommand(commandText, con, tran);
                cmd.CommandType = commandType;

                if (parameters != null)
                {
                    foreach (SqlParameter parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }
                }

                con.Open();

                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                FileProccess.WriteLog(ex.Message);
            }
            finally
            {
                con.Close();
            }

            return result;
        }

        public object ExecuteScalar(CommandType commandType, string commandText, params SqlParameter[] parameters)
        {
            object result = new object();

            try
            {
                cmd = new SqlCommand(commandText, cnn);
                cmd.CommandType = commandType;

                if (parameters != null)
                {
                    foreach (SqlParameter parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }
                }

                OpenConnection();

                result = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                FileProccess.WriteLog(ex.Message);
            }
            finally
            {
                CloseConnection();
            }

            return result;
        }

        public object ExecuteScalar(CommandType commandType, string commandText, SqlTransaction tran, SqlConnection con, params SqlParameter[] parameters)
        {
            object result = new object();

            try
            {
                cmd = new SqlCommand(commandText, con, tran);
                cmd.CommandType = commandType;

                if (parameters != null)
                {
                    foreach (SqlParameter parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }
                }

                con.Open();

                result = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                FileProccess.WriteLog(ex.Message);
            }
            finally
            {
                con.Close();
            }

            return result;
        }

        public SqlDataReader ExecuteDataReader(CommandType commandType, string commandText, params SqlParameter[] parameters)
        {
            SqlDataReader result = null;

            try
            {
                cmd = new SqlCommand(commandText, cnn);
                cmd.CommandType = commandType;

                if (parameters != null)
                {
                    foreach (SqlParameter parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }
                }

                OpenConnection();

                result = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                FileProccess.WriteLog(ex.Message);
            }

            return result;
        }

        public DataTable ExecuteDataTable(CommandType commandType, string commandText, params SqlParameter[] parameters)
        {
            DataTable result = new DataTable();

            try
            {
                cmd = new SqlCommand(commandText, cnn);
                cmd.CommandType = commandType;

                if (parameters != null)
                {
                    foreach (SqlParameter parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }
                }

                ds = new DataSet();
                da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                result = ds.Tables[0];
            }
            catch (Exception ex)
            {
                FileProccess.WriteLog(ex.Message);
            }

            return result;
        }

        public DataSet ExecuteDataSet(CommandType commandType, string commandText, params SqlParameter[] parameters)
        {
            DataSet result = new DataSet();

            try
            {
                cmd = new SqlCommand(commandText, cnn);
                cmd.CommandType = commandType;

                if (parameters != null)
                {
                    foreach (SqlParameter parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }
                }

                ds = new DataSet();
                da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                result = ds;
            }
            catch (Exception ex)
            {
                FileProccess.WriteLog(ex.Message);
            }

            return result;
        }

        private void OpenConnection()
        {
            if (cnn.State == ConnectionState.Closed) cnn.Open();
        }

        private void CloseConnection()
        {
            if (cnn.State == ConnectionState.Open) cnn.Close();
        }

        private void cnn_stateChange(object sender, StateChangeEventArgs e)
        {
            FileProccess.WriteLog(DateTime.Now.ToString() + "=> Bağlantı Önnceki Durumu : " + e.OriginalState + " Bağlantı Durumu : " + e.CurrentState);
        }

        public void Dispose()
        {
            CloseConnection();
        }
    }
}
