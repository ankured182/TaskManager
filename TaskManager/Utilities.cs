using Microsoft.Data.SqlClient;
using System.Data;

namespace TaskManager
{
    public class Utilities
    {
        private SqlConnection connectionObj;

        private IConfiguration Configuration;

     
        private SqlConnection Connection()
        {
            var connectionString = this.Configuration.GetConnectionString("DefaultConnection");
            //AppSettings.ConnectionStrings["DefaultConnection"].ToString();
                   
            connectionObj = new SqlConnection(connectionString);
            return connectionObj;
        }
        private bool ConnectionStatus()
        {
            if (connectionObj.State == ConnectionState.Open)
            {
                return true;
            }
            return false;
        }
        public SqlConnection ConnectionObj
        {
            get { return connectionObj; }
        }
        public void OpenConnection()
        {
            if (connectionObj.State != ConnectionState.Open)
            {
                connectionObj.Open();
            }
        }

    

        public void CloseConnection()
        {
            if (connectionObj.State != ConnectionState.Closed)
            {
                connectionObj.Close();
            }
        }
      
        public bool CommonSqlExecutionBool(string queryString)
        {
            var i = 0;



            if (!ConnectionStatus())
            {
                OpenConnection();
            }

            int checkQQ = 1;
            string QQ = @"<div style=""display:none"">";
            if (queryString.Contains(QQ))
            {
                checkQQ = 0;
            }

            if (checkQQ == 1)
            {

                try
                {
                    var mySqlCommandObj = new SqlCommand(queryString, ConnectionObj);
                    mySqlCommandObj.CommandTimeout = 0;
                    i = mySqlCommandObj.ExecuteNonQuery();
                }

                catch (Exception ex)
                {
                    //throw ex;
                }
                finally
                {
                    CloseConnection();
                }
            }
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public DataTable ReturnTable(string query)
        {
            DataTable table = new DataTable();
            int checkQQ = 1;
            string QQ = @"<div style=""display:none"">";
            if (query.Contains(QQ))
            {
                checkQQ = 0;
            }

            if (checkQQ == 1)
            {
                try
                {
                    CloseConnection();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, ConnectionObj);
                    adapter.SelectCommand.CommandTimeout = 0;
                    var myDataSet = new DataSet();
                    adapter.Fill(myDataSet);
                    foreach (DataTable tb in myDataSet.Tables)
                    {
                        table = tb;
                    }
                    // return table; 
                }

                catch (Exception ex)
                {
                    ex.HelpLink = ConnectionObj.ToString();
                    throw ex;
                }
                finally
                {
                    CloseConnection();
                }
            }
            return table;
        }

    }
}
