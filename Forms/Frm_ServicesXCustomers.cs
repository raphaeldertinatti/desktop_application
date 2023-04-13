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
    public partial class Frm_ServicesXCustomers : Form
    {
        cls_mysql_conn connection = new cls_mysql_conn();
        cls_populate_views populate = new cls_populate_views();
        public static Frm_ServicesXCustomers instance;       
        public Frm_ServicesXCustomers()
        {
            InitializeComponent();
            instance = this;
            lsv_customers2.View = View.Details;
            lsv_customers2.LabelEdit = true;
            lsv_customers2.AllowColumnReorder = true;
            lsv_customers2.FullRowSelect = true;
            lsv_customers2.GridLines = true;
            lsv_customers2.Columns.Add("Código Cliente", 80, HorizontalAlignment.Left);
            lsv_customers2.Columns.Add("Nome Cliente", 270, HorizontalAlignment.Left);
            lsv_customers2.Columns.Add("Status", 270, HorizontalAlignment.Left);
            ListClients();
            cbb_status.SelectedIndex = 0;
        }

        public void ListClients()
        {
            try
            {
                connection.OpenConnection();
                string sql = "SELECT * FROM db_sis.tb_cliente";               
                MySqlCommand cmd = new MySqlCommand(sql, connection.conn);
                lsv_customers2.Items.Clear();
                int[] columnIndex = { 0, 1, 4 };
                populate.PopulateListViews(lsv_customers2, cmd, columnIndex);
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

        private void lsv_customers2_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ListView.SelectedListViewItemCollection itens_selecionados = lsv_customers2.SelectedItems;

            foreach (ListViewItem item in itens_selecionados)
            {
                Frm_Services.instance.cod_cliente.Text = item.SubItems[0].Text;
                Frm_Services.instance.desc_cliente.Text = item.SubItems[1].Text;
            }
        }

        private void btn_Select_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                connection.OpenConnection();                               
                string sql = "SELECT * FROM db_sis.tb_cliente WHERE NOME_CLIENTE LIKE @NOME AND STATUS LIKE @STATUS";
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@NOME","%" + txt_buscar.Text + "%"),
                    new MySqlParameter("@STATUS", cbb_status.Text)
                };
                MySqlCommand cmd = connection.CreateCommand(sql, parameters);
                lsv_customers2.Items.Clear();
                int[] columnIndex = { 0, 1, 4 };
                populate.PopulateListViews(lsv_customers2,cmd,columnIndex);
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
        private void Frm_ServicesXCustomers_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
    }
}
