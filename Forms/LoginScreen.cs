using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DesktopApplication
{
    public partial class LoginScreen : Form
    {
        cls_mysql_conn connection = new cls_mysql_conn();
        public bool LoginSucess = false;  
        public static string User { get; set; }

        public LoginScreen()
        {
            InitializeComponent();
        }      

        private void btn_enter_Click(object sender, EventArgs e)
        {
            try
            {                  
                connection.OpenConnection();
                string sql = "SELECT * FROM db_sis.tb_users WHERE "+
                             "USER = @user "+
                             "AND PASS = @pass";

                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@user",txt_user.Text),
                    new MySqlParameter("@pass",txt_pass.Text)
                };              

                MySqlCommand cmd = connection.CreateCommand(sql, parameters);  
                
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows == true)
                {
                    if (txt_pass.Text == "default")
                    {
                        User = txt_user.Text;
                        Frm_NewPass newpass = new Frm_NewPass();
                        newpass.Show();
                    }
                    if (txt_pass.Text != "default")
                    {
                        LoginSucess = true;
                        this.Close();
                    }
                }
                else
                {                       
                    MessageBox.Show("User/Pass incorrect, verify your credentials", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 
             finally
            {
                if (connection != null)
                {
                    connection.CloseConnection();
                }
            }           
        }
        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
