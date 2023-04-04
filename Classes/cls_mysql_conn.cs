using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DesktopApplication
{
    public class cls_mysql_conn
    {
        public MySqlConnection conn;
        private string server;        
        private string user;
        private string pass;

        public cls_mysql_conn()
        {
            server = "00.0.000.000";
            user = "system_user";
            pass = "@aeiou0011*ABC";

            string connectionString = $"datasource={server};username={user};password={pass};";

            conn = new MySqlConnection(connectionString);
        }

        public bool OpenConnection()
        {
            try
            {
                conn.Open();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool CloseConnection()
        {
            try
            {
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                conn.Dispose();
            }
        }

        public MySqlCommand CreateCommand(string query, params MySqlParameter[] parameters)
        {
            if (!IsConnectionOpen())
            {
                throw new InvalidOperationException("Não está conectado ao banco.");
            }

            try
            {   
                MySqlCommand cmd = new MySqlCommand(query,conn);               
                foreach(MySqlParameter parameter in parameters)
                {
                    cmd.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);                    
                }
                cmd.Prepare();
                return cmd;
            }
            catch (Exception ex )
            {
                MessageBox.Show(ex.Message);
                return null;
            }            
        }

        public bool IsConnectionOpen()
        {
            return conn != null && conn.State == System.Data.ConnectionState.Open;
        }

    }
}
