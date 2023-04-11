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
    public partial class Frm_Contacts : Form
    {
        cls_mysql_conn connection = new cls_mysql_conn();
        public static Frm_Contacts instance;
        public TextBox cod;
        public TextBox cod_customer;
        
        public Frm_Contacts()
        {
            InitializeComponent();
            instance = this;
            cod = txt_codcontact;
            cod_customer = txt_codcustomer;
        }

        private void tsb_search_Click(object sender, EventArgs e)
        {
            Frm_ContactList List = new Frm_ContactList();
            List.ShowDialog();
        }
        public void Capture()
        {
            if (!(txt_codcontact.Text == ""))
            {
                try
                {
                    connection.OpenConnection();
                    string sql = " SELECT a.COD_CONTACT,a.COD_CUSTOMER,a.NOME,a.ROLE,a.EMAIL,a.TELEFONE,b.CUSTOMER_NAME FROM db_sis.tb_contacts a INNER JOIN db_sis.tb_customer b on a.COD_CUSTOMER = b.COD_CUSTOMER WHERE COD_CONTACT=" + int.Parse(txt_codcontact.Text);                  
                    MySqlCommand cmd = new MySqlCommand(sql, connection.conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            txt_codcustomer.Text = reader.GetString("COD_CUSTOMER");
                            txt_Customer.Text = reader.GetString("CUSTOMER_NAME");
                            txt_nome.Text = reader.GetString("NOME");
                            txt_ROLE.Text = reader.GetString("ROLE");
                            txt_email.Text = reader.GetString("EMAIL");
                            txt_telefone.Text = reader.GetString("TELEFONE");
                        }
                    }
                    btn_associa.Enabled = false;                    
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
        private void tsb_clear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btn_associa_Click(object sender, EventArgs e)
        {
            Frm_ContactsXCustomers list = new Frm_ContactsXCustomers();
            list.ShowDialog();
        }

        public void CaptureCodCustomer()
        {
            try
            {
                connection.OpenConnection();
                string sql = "SELECT * FROM db_sis.tb_customer WHERE COD_CUSTOMER=" + int.Parse(txt_codcustomer.Text);
                MySqlCommand cmd = new MySqlCommand(sql, connection.conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        txt_Customer.Text = reader.GetString("CUSTOMER_NAME");
                    }
                }
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

        private void tsb_add_Click(object sender, EventArgs e)
        {
            if (txt_codcontact.Text == "")
            {
                if ((txt_nome.Text == "") || (txt_email.Text == "") || (txt_telefone.Text == "") || (txt_codcustomer.Text == ""))
                {
                    MessageBox.Show("Name, email, and phone cannot be null. Also, associate the contact with a customer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    try
                    {
                        connection.OpenConnection();
                        string sql = "INSERT INTO db_sis.tb_contacts (COD_CUSTOMER, NOME, ROLE, EMAIL, TELEFONE) VALUES (@COD, @NOME, @ROLE, @EMAIL, @TEL)";

                        MySqlParameter[] parameters = new MySqlParameter[]
                        {
                            new MySqlParameter("@COD",txt_codcustomer.Text),
                            new MySqlParameter("@NOME",txt_nome.Text),
                            new MySqlParameter("@ROLE",txt_ROLE.Text),
                            new MySqlParameter("@EMAIL",txt_email.Text),
                            new MySqlParameter("@TEL",txt_telefone.Text)
                        };                          
                        
                        MySqlCommand cmd = connection.CreateCommand(sql,parameters);
                        MySqlDataReader reader = cmd.ExecuteReader();

                        MessageBox.Show("Contact added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        Clear();
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
                MessageBox.Show("Clear the form to insert a new contact.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsb_salvar_Click(object sender, EventArgs e)
        {
            if (!(txt_codcontact.Text == ""))
            {
                try
                {
                    connection.OpenConnection();
                    string sql = "UPDATE db_sis.tb_contacts SET NOME = @NOME, ROLE = @ROLE, EMAIL = @EMAIL, TELEFONE = @TEL WHERE COD_CONTACT = @COD";

                    MySqlParameter[] parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@NOME", txt_nome.Text),
                        new MySqlParameter("@ROLE", txt_ROLE.Text),
                        new MySqlParameter("@EMAIL", txt_email.Text),
                        new MySqlParameter("@TEL", txt_telefone.Text), 
                        new MySqlParameter("@COD", int.Parse(txt_codcontact.Text))
                    };

                    MySqlCommand cmd = connection.CreateCommand(sql,parameters);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    MessageBox.Show("Update successfully completed", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void tsb_excluir_Click(object sender, EventArgs e)
        {
            if (!(txt_codcontact.Text == ""))
            {
                try
                {
                    connection.OpenConnection();
                    string sql = "DELETE FROM db_sis.tb_contacts WHERE COD_CONTACT = " + int.Parse(txt_codcontact.Text);                          
                    DialogResult result = MessageBox.Show($"Deseja realmente excluir o contato: {txt_nome.Text} ?", "Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        MySqlCommand cmd = new MySqlCommand(sql, connection.conn);
                        MySqlDataReader reader = cmd.ExecuteReader();
                        MessageBox.Show("Contact successfully deleted.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Clear();
                    }
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
        private void Clear()
        {
            txt_codcontact.Text = "";
            txt_codcustomer.Text = "";
            txt_Customer.Text = "";
            txt_nome.Text = "";
            txt_ROLE.Text = "";
            txt_email.Text = "";
            txt_telefone.Text = "";
            btn_associa.Enabled = true;
        }
    }
}
