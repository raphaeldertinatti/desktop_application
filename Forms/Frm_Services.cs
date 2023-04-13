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
    public partial class Frm_Services : Form
    {
        cls_mysql_conn connection = new cls_mysql_conn();
        cls_populate_views populate = new cls_populate_views();
        public static Frm_Services instance;
        public TextBox cod_dep;
        public TextBox desc_dep;
        public TextBox cod_cliente;
        public TextBox desc_cliente;
        public Frm_Services()
        {            
            InitializeComponent();
            instance = this;
            cod_dep = txt_coddep;
            desc_dep = txt_department;
            cod_cliente = txt_codcustomer;
            desc_cliente = txt_Customer;

            lsv_servicos.View = View.Details;
            lsv_servicos.LabelEdit = true;
            lsv_servicos.AllowColumnReorder = true;
            lsv_servicos.FullRowSelect = true;
            lsv_servicos.GridLines = true;
            lsv_servicos.Columns.Add("Cód.Serviço", 80, HorizontalAlignment.Left);
            lsv_servicos.Columns.Add("Serviço", 510, HorizontalAlignment.Left);
            lsv_servicos.Columns.Add("Departamento", 120, HorizontalAlignment.Left);

            lsv_servclient.View = View.Details;
            lsv_servclient.LabelEdit = true;
            lsv_servclient.AllowColumnReorder = true;
            lsv_servclient.FullRowSelect = true;
            lsv_servclient.GridLines = true;
            lsv_servclient.Columns.Add("Cód.Cliente", 80, HorizontalAlignment.Left);
            lsv_servclient.Columns.Add("Nome Cliente", 120, HorizontalAlignment.Left);
            lsv_servclient.Columns.Add("Departamento", 100, HorizontalAlignment.Left);
            lsv_servclient.Columns.Add("Serviço", 400, HorizontalAlignment.Left);             
            ListServices();
            ListServCustomers();
        }
        public void ListServices()
        {
            try
            {
                connection.OpenConnection();               
                string sql = "SELECT * FROM db_sis.vw_serv_dep order by COD_SERVICO";                
                MySqlCommand cmd = new MySqlCommand(sql, connection.conn);    
                lsv_servicos.Items.Clear();
                int[] columnIndex = { 0, 1, 2 };
                populate.PopulateListViews(lsv_servicos, cmd, columnIndex);
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
        public void ListServicesDEP()
        {
            if (!(txt_coddep.Text == ""))
            {
                try
                {
                    connection.OpenConnection();
                    string sql = "SELECT * FROM db_sis.vw_serv_dep WHERE COD_DEPARTAMENTO =" + txt_coddep.Text + " order by COD_SERVICO;";
                    MySqlCommand cmd = new MySqlCommand(sql, connection.conn);                    
                    lsv_servicos.Items.Clear();
                    int[] columnIndex = { 0, 1, 2 };
                    populate.PopulateListViews(lsv_servicos, cmd, columnIndex);
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

        public void ListServCustomers()
        {
            try
            {
                connection.OpenConnection();  
                string sql = "SELECT * FROM db_sis.vw_serv_cliente2 WHERE COD_SERVICO =" + txt_codserv.Text;
                string sql2 = "SELECT * FROM db_sis.vw_serv_cliente2"; 
                MySqlCommand cmd = new MySqlCommand(sql, connection.conn);
                MySqlCommand cmd2 = new MySqlCommand(sql2, connection.conn);
                int[] columnIndex = { 0, 1, 3, 5 };

                if (txt_codserv.Text == "")
                {
                    lsv_servclient.Items.Clear();
                    populate.PopulateListViews(lsv_servclient, cmd2, columnIndex);                   
                }
                else 
                {
                    lsv_servclient.Items.Clear();
                    populate.PopulateListViews(lsv_servclient, cmd, columnIndex);                    
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

        public void ListServCustomersDEP()
        {
            try
            {
                connection.OpenConnection();
                string sql = "SELECT * FROM db_sis.vw_serv_cliente2 WHERE COD_DEPARTAMENTO = " + txt_coddep.Text;               
                MySqlCommand cmd = new MySqlCommand(sql, connection.conn);
                int[] columnIndex = { 0, 1, 3, 5 };

                if (!(txt_coddep.Text == ""))
                {                    
                    lsv_servclient.Items.Clear();
                    populate.PopulateListViews(lsv_servclient, cmd, columnIndex);
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

        private void lsv_services_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ListView.SelectedListViewItemCollection itens_selecionados = lsv_services.SelectedItems;

            foreach (ListViewItem item in itens_selecionados)
            {
                txt_codserv.Text = item.SubItems[0].Text;
                txt_department.Text = item.SubItems[2].Text;
                txt_servico.Text = item.SubItems[1].Text;              
                ListServCustomers();
                btn_departamento.Enabled = false;
                txt_coddep.Text = "";                
            }
        }
        private void tsb_clear_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void btn_department_Click(object sender, EventArgs e)
        {
            Frm_Lista_DEP list = new Frm_Lista_DEP();
            list.ShowDialog();
        }
        private void tsb_add_Click(object sender, EventArgs e)
        {
            if (txt_codserv.Text == "")
            {
                if ((txt_coddep.Text == "") || (txt_servico.Text == ""))
                {
                    MessageBox.Show("Please inform the Service Name and the Department.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    try
                    {
                        connection.OpenConnection();
                        string sql = "INSERT INTO db_sis.tb_servicos (DESCRICAO, COD_DEPARTAMENTO) VALUES (@SERV, @COD)";
                        MySqlParameter[] parameter = new MySqlParameter[]
                        {
                            new MySqlParameter("@SERV",txt_servico.Text),
                            new MySqlParameter("@COD",txt_coddep.Text)
                        };
                        MySqlCommand cmd = connection.CreateCommand(sql, parameter);
                        MySqlDataReader reader = cmd.ExecuteReader();
                        MessageBox.Show("Service successfully included!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        connection.CloseConnection();
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
                MessageBox.Show("Clear the form to insert a new service.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void tsb_save_Click(object sender, EventArgs e)
        {
            if (!(txt_codserv.Text == ""))
            {
                try
                {
                    connection.OpenConnection();
                    string sql = "UPDATE db_sis.tb_servicos SET DESCRICAO = @SERV WHERE COD_SERVICO = @COD";
                    MySqlParameter[] parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@SERV", txt_servico.Text),
                        new MySqlParameter("@COD", int.Parse(txt_codserv.Text))
                    };
                    MySqlCommand cmd = connection.CreateCommand(sql,parameters);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    MessageBox.Show("Update successfully performed.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    connection.CloseConnection();
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
        private void btn_associa_Click(object sender, EventArgs e)
        {
            Frm_Clientes_SERV list = new Frm_Clientes_SERV();
            list.ShowDialog();
        }
        private void btn_vincula_Click(object sender, EventArgs e)
        {
            if (!((txt_codserv.Text == "") || (txt_codcustomer.Text =="")))
            {
                DialogResult = MessageBox.Show($"Are you sure you want to link the Service {txt_servico.Text} to the customer {txt_Customer.Text} ?", "Link Services", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DialogResult == DialogResult.Yes)
                {
                    try
                    {
                        connection.OpenConnection();
                        string sql = "INSERT INTO db_sis.tb_servico_cliente (COD_SERVICO, COD_CLIENTE) VALUES (@COD_SERV, @COD_CLIENTE)";

                        MySqlParameter[] parameters = new MySqlParameter[]
                        {
                            new MySqlParameter("@COD_SERV", txt_codserv.Text),
                            new MySqlParameter("@COD_CLIENTE", txt_codcustomer.Text)
                        };

                        MySqlCommand cmd = connection.CreateCommand(sql, parameters);
                        MySqlDataReader reader = cmd.ExecuteReader();                        
                        MessageBox.Show("Service successfully linked to the customer!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        connection.CloseConnection();
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
                MessageBox.Show("Please inform the service and the customer to link.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Clear()
        {
            btn_departamento.Enabled = true;
            txt_codserv.Text = "";
            txt_department.Text = "";
            txt_servico.Text = "";
            txt_coddep.Text = "";
            txt_codcustomer.Text = "";
            txt_Customer.Text = "";
            ListServices();
            ListServCustomers();
        }
    }
}
