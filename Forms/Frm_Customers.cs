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
   
    public partial class Frm_Customers : Form    
    {
        public static Frm_Customers instance;
        public TextBox cod; 
        cls_mysql_conn connection = new cls_mysql_conn();
        cls_populate_views populate = new cls_populate_views();


        public Frm_Customers()
        {
            InitializeComponent();
            instance = this; 
            cod = txt_codcustomer;

            string[] headers_c = { "Nome", "Cargo", "e-mail", "Telefone" };
            int[] widths_c = { 180, 140, 180, 160 };
            string[] headers_e = { "Razão Social", "CNPJ", "Tipo", "I.E.", "Endereço", "Cidade", "UF" };
            int[] widths_e = { 190, 110, 40, 130, 360, 120, 40 };
            string[] headers_s = {"Departamento","Serviço"};
            int[] widths_s = { 100, 280 };

            populate.ConstructListView(lsv_contatos, headers_c, widths_c);            
            populate.ConstructListView(lsv_empresas, headers_e, widths_e);
            populate.ConstructListView(lsv_servicos, headers_s, widths_s); 

            cbb_dep.Enabled = false;           
        }        

        private void tsb_search_Click(object sender, EventArgs e)
        {
            Frm_CustomersList List = new Frm_CustomersList();            
            List.ShowDialog();            
        }       

        public void Capture()
        {
            if (!(txt_codcustomer.Text == ""))
            {
                try
                {
                    connection.OpenConnection();
                    string sql = "SELECT * FROM db_sis.tb_customers WHERE " +
                             "COD_CUSTOMER = " + int.Parse(txt_codcustomer.Text);
                    string[] column = {"NOME_CLIENTE","WEBSITE","SOCIOS","STATUS"};
                    TextBox[] TextBoxes = {txt_NomeCliente,txt_Website,txt_Socios,txt_status};
                    MySqlCommand cmd = new MySqlCommand(sql, connection.conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            for (int i = 0; i < column.Length; i++)
                            {
                                TextBoxes[i].Text = reader.GetString(column[i]);
                            }
                        }
                    }
                    ListContacts();
                    ListCompanies();
                    ListServices();
                    cbb_dep.Enabled = true;
                    cbb_dep.SelectedIndex = 0;  
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

        private void tsb_clean_Click(object sender, EventArgs e)
        {
           Clear();
        }

        private void tsb_add_Click(object sender, EventArgs e)
        {
            if (txt_codcustomer.Text == "")
            {
                if (txt_CustomerName.Text == "")
                {
                    MessageBox.Show("Customer Name cannot be null", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    try
                    {
                        connection.OpenConnection();
                        string sql = "INSERT INTO db_sis.tb_customers (CUSTOMER_NAME, WEBSITE, PARTNERS, STATUS) VALUES (@NAME, @WEB, @PART," + "'ACTIVE'" + ")";

                        MySqlParameter[] parameters = new MySqlParameter[]
                        {
                            new MySqlParameter("@NAME",txt_CustomerName.Text),
                            new MySqlParameter("@WEB",txt_Website.Text),
                            new MySqlParameter("@PART",txt_Partners.Text)
                        };     
                        
                        MySqlCommand cmd = connection.CreateCommand(sql, parameters);
                        MySqlDataReader reader = cmd.ExecuteReader();
                        MessageBox.Show("Customer added with success! To add companies, contacts and services for this customer access the respective forms.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
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
                MessageBox.Show("Clean the form to add a new customer.","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            } 
        }
        public void ListContacts()
        {
            try
            {
                string sql = "SELECT * FROM db_sis.tb_contacts WHERE "+
                             "COD_CUSTOMER = " + int.Parse(txt_codcustomer.Text);
                MySqlCommand cmd = new MySqlCommand(sql, connection.conn);
                lsv_contacts.Items.Clear();
                int[] columnIndexes = {2,3,4,5};
                populate.PopulateListViews(lsv_contacts,cmd,columnIndexes);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 
        }
        public void ListCompanies()
        {
            try
            {
                string sql = "SELECT * FROM db_sis.tb_companies WHERE " +
                             "COD_CUSTOMER = " + int.Parse(txt_codcustomer.Text);
                MySqlCommand cmd = new MySqlCommand(sql, connection.conn);
                lsv_companies.Items.Clear();
                int[] columnIndexes = {3,4,5,6,7,8,9};
                populate.PopulateListViews(lsv_companies,cmd,columnIndexes);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }                     

        }
        public void ListServices()
        {            
            try
            {           
                string sql = "SELECT * FROM db_sis.vw_serv_customer WHERE " +
                             "COD_CUSTOMER = " + int.Parse(txt_codcustomer.Text);
                MySqlCommand cmd = new MySqlCommand(sql, connection.conn);
                lsv_services.Items.Clear();
                int[] columnIndexes = {2,3};
                populate.PopulateListViews(lsv_services,cmd,columnIndexes);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 
        }
        private void tsb_save_Click(object sender, EventArgs e)
        {
            if (!(txt_codcustomer.Text == ""))
            {
                try
                {
                    connection.OpenConnection();
                    string sql = "UPDATE db_sis.tb_customers SET NOME_CLIENTE = @NAME, WEBSITE = @WEB, PARTNERS = @PART WHERE COD_CUSTOMER = @COD";

                    MySqlParameter[] parameters = new MySqlParameter[]
                     {
                            new MySqlParameter("@NOME", txt_CustomerName.Text),
                            new MySqlParameter("@WEB", txt_Website.Text),
                            new MySqlParameter("@PART", txt_Partners.Text),
                            new MySqlParameter("@COD", int.Parse(txt_codcustomer.Text))
                     };

                    MySqlCommand cmd = connection.CreateCommand(sql, parameters);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    MessageBox.Show("Update Succeeded", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
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

        
        private void cbb_dep_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            try
            {
                if (cbb_dep.Text == "TODOS")
                {
                    if (!connection.IsConnectionOpen())
                    {
                        connection.OpenConnection();
                        string sql = "SELECT * FROM db_sis.vw_serv_customer WHERE " +
                                                       "COD_CUSTOMER = " + int.Parse(txt_codcustomer.Text);
                        MySqlCommand cmd = new MySqlCommand(sql, connection.conn);
                        lsv_services.Items.Clear();
                        int[] columnIndexes = { 2, 3 };
                        populate.PopulateListViews(lsv_services,cmd,columnIndexes);
                    }
                    else
                    {
                        string sql = "SELECT * FROM db_sis.vw_serv_customer WHERE " +
                                                        "COD_CUSTOMER = " + int.Parse(txt_codcustomer.Text);
                        MySqlCommand cmd = new MySqlCommand(sql, connection.conn);
                        lsv_services.Items.Clear();
                        int[] columnIndexes = { 2, 3 };
                        populate.PopulateListViews(lsv_services, cmd, columnIndexes);
                    }
                    
                }
                else
                {
                    connection.OpenConnection();             
                    string sql = "SELECT * FROM db_sis.vw_serv_customer WHERE COD_CUSTOMER = @COD AND DEPARTAMENTO LIKE @DEP";

                    MySqlParameter[] parameters = new MySqlParameter[]
                     {
                        new MySqlParameter("@COD", int.Parse(txt_codcustomer.Text)),
                        new MySqlParameter("@DEP","%" + cbb_dep.Text + "%")
                     };

                    MySqlCommand cmd = connection.CreateCommand(sql, parameters);
                    lsv_services.Items.Clear();
                    int[] columnIndexes = { 2, 3 };
                    populate.PopulateListViews(lsv_services, cmd, columnIndexes);
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

        private void Clear()
        {
            txt_codcliente.Text = "";
            txt_NomeCliente.Text = "";
            txt_Socios.Text = "";
            txt_Website.Text = "";
            txt_status.Text = "";
            lsv_contatos.Items.Clear();
            lsv_empresas.Items.Clear();
            lsv_servicos.Items.Clear();
            cbb_dep.Text = "";
            cbb_dep.Enabled = false;
        }       
    }
    
}
