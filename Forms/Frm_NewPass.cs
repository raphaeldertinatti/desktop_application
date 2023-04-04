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
    public partial class Frm_NewPass : Form
    {
        cls_mysql_conn connection = new cls_mysql_conn();
        public Frm_NewPass()
        {
            InitializeComponent();
            lbl_UserName.Text = LoginScreen.User;
        }

        private void button_Confirm_Click(object sender, EventArgs e)
        {
            try
            {
                if(txt_pass1.Text == txt_pass2.Text)
                {
                    if (txt_pass1.Text == "" || txt_pass2.Text == "")

                    { 
                        MessageBox.Show("Password cannot be null, please inform a new password.", "", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                    }
                    else
                    {
                        try 
                        {
                            connection.OpenConnection();   
                            string sql = "UPDATE " +
                                         "db_sis.tb_users " +
                                         "SET PASS = @pass " +
                                         "WHERE USER = @user " +
                                         "AND PASS = 'etccom'";                           

                            MySqlParameter[] parameters = new MySqlParameter[]
                            {
                                new MySqlParameter("user",lbl_UserName.Text),
                                new MySqlParameter("pass",txt_senha1.Text)
                            };
                            
                            MySqlCommand cmd = connection.CreateCommand(sql,parameters);
                            cmd.ExecuteNonQuery();
                            connection.CloseConnection();
                            MessageBox.Show("Password update succeed. Login again with the new password.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            connection.CloseConnection();
                        }
                    }                   
                }                
                else
                {
                    MessageBox.Show("Password is different, check again.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }
    }
}
