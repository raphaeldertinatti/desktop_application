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
    public partial class Frm_ContactsXCustomers : Form
    {
        public static Frm_ContactsXCustomers instance;
        cls_mysql_conn connection = new cls_mysql_conn();
        cls_populate_views populate = new cls_populate_views();
        
        public Frm_ContactsXCustomers()
        {
            InitializeComponent();
            instance = this;
            string[] headers = { "Código Cliente", "Nome Cliente", "Status" };
            int[] widths = { 80, 270, 270 };
            populate.ConstructListView(lsv_clientes2, headers, widths);     
            ListsCustomers();
            cbb_status.SelectedIndex = 0;
        }

        public void ListsCustomers()
        {
            try
            {
                connection.OpenConnection();
                string sql = "SELECT * FROM db_sis.tb_customer";               
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
                Frm_Contacts.instance.cod_customer.Text = item.SubItems[0].Text;
                Frm_Contacts.instance.name_customer.Text = item.SubItems[1].Text;
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
                string sql = "SELECT * FROM db_sis.tb_customer WHERE NOME_CLIENTE LIKE @NAME AND STATUS LIKE @STATUS";
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@NAME","%" + txt_buscar.Text + "%"),
                    new MySqlParameter("@STATUS", cbb_status.Text)
                };

                MySqlCommand cmd = connection.CreateCommand(sql,parameters); 
                lsv_customers2.Items.Clear();
                int[] columnIndexes = { 0, 1, 4 };
                populate.PopulateListViews(lsv_customers2, cmd, columnIndexes);                
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
}
