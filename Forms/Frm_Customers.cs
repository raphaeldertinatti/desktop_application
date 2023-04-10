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
            lsv_contacts.View = View.Details;
            lsv_contacts.LabelEdit = true;
            lsv_contacts.AllowColumnReorder = true;
            lsv_contacts.FullRowSelect = true;
            lsv_contacts.GridLines = true;
            lsv_contacts.Columns.Add("Name", 180, HorizontalAlignment.Left);
            lsv_contacts.Columns.Add("Job_Role", 140, HorizontalAlignment.Left);
            lsv_contacts.Columns.Add("e-mail", 180, HorizontalAlignment.Left);
            lsv_contacts.Columns.Add("Telephone",160, HorizontalAlignment.Left);

            lsv_companies.View = View.Details;
            lsv_companies.LabelEdit = true;
            lsv_companies.AllowColumnReorder = true;
            lsv_companies.FullRowSelect = true;
            lsv_companies.GridLines = true;            
            lsv_companies.Columns.Add("Razão Social", 190, HorizontalAlignment.Left);
            lsv_companies.Columns.Add("CNPJ", 110, HorizontalAlignment.Left);
            lsv_companies.Columns.Add("Type", 40, HorizontalAlignment.Left);
            lsv_companies.Columns.Add("I.E.", 130, HorizontalAlignment.Left);
            lsv_companies.Columns.Add("Address", 360, HorizontalAlignment.Left);
            lsv_companies.Columns.Add("City", 120, HorizontalAlignment.Left);
            lsv_companies.Columns.Add("UF", 40, HorizontalAlignment.Left);

            lsv_services.View = View.Details;
            lsv_services.LabelEdit = true;
            lsv_services.AllowColumnReorder = true;
            lsv_services.FullRowSelect = true;
            lsv_services.GridLines = true;
            lsv_services.Columns.Add("Department", 100, HorizontalAlignment.Left);
            lsv_services.Columns.Add("Service", 280, HorizontalAlignment.Left);

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
                    MySqlCommand cmd = new MySqlCommand(sql, connection.conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            txt_CustomerName.Text = reader.GetString("CUSTOMER_NAME");
                            txt_Website.Text = reader.GetString("WEBSITE");
                            txt_Partners.Text = reader.GetString("PARTNERS");
                            txt_status.Text = reader.GetString("STATUS");
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
