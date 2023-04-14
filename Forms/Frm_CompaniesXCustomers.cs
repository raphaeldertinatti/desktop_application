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
    public partial class Frm_CompaniesXCustomers : Form
    {
        public static Frm_CompaniesXCustomers instance;
        cls_mysql_conn connection = new cls_mysql_conn();
        cls_populate_views populate = new cls_populate_views();
        public Frm_CompaniesXCustomers()        
        {
            InitializeComponent();
            instance = this;
            string[] headers = { "Código Cliente", "Nome Cliente", "Status" };
            int[] widths = { 80, 270, 270 };
            populate.ConstructListView(lsv_clientes2, headers, widths);
            ListCustomers();
            cbb_status.SelectedIndex = 0;
        }
      
        public void ListCustomers()
        {
            try
            {
                connection.OpenConnection();
                string sql = "SELECT * FROM db_sis.tb_cliente";                
                MySqlCommand cmd = new MySqlCommand(sql, connection.conn);               
                lsv_customers2.Items.Clear();
                int[] columnIndexes = { 0, 1, 4 };
                populate.PopulateListViews(lsv_customers2,cmd,columnIndexes);
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
        private void lsv_clientes_ItemSelectionChanged_1(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ListView.SelectedListViewItemCollection itens_selecionados = lsv_customers2.SelectedItems;

            foreach (ListViewItem item in itens_selecionados)
            {
                Frm_Companies.instance.cod_cliente.Text = item.SubItems[0].Text;   
                Frm_Companies.instance.name_customer.Text = item.SubItems[1].Text;             
            }
        }

        private void btn_Selecionar_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_buscar_Click_1(object sender, EventArgs e)
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
