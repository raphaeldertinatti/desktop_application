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
    public partial class Frm_CustomersList : Form   
    {
        public static Frm_CustomersList instance;
        cls_mysql_conn connection = new cls_mysql_conn();
        cls_populate_views populate = new cls_populate_views();

        public Frm_CustomersList()
        {
            InitializeComponent();
            instance = this;
            string[] headers = { "Código Cliente", "Nome Cliente", "Status" };
            int[] widths = { 80, 270, 270 };
            populate.ConstructListView(lsv_customers, headers, widths);         
            cbb_status.SelectedIndex = 0;
            ListCustomers();
        }        

        public void btn_Select_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void ListCustomers()
        {
            try
            {
                connection.OpenConnection();
                string sql = "SELECT * FROM db_sis.tb_customers WHERE STATUS IN ('ACTIVE','INACTIVE')";               
                MySqlCommand cmd = connection.CreateCommand(sql, new MySqlParameter[0]);                
                lsv_customers.Items.Clear();
                int[] columnIndexes = { 0, 1, 4 };
                populate.PopulateListViews(lsv_customers,cmd,columnIndexes);                          
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

        public void lsv_customers_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ListView.SelectedListViewItemCollection itens_selecionados = lsv_customers.SelectedItems;

            foreach(ListViewItem item in itens_selecionados)
            {
                if (Frm_Customers.instance != null)
                {
                    Frm_Customers.instance.cod.Text = item.SubItems[0].Text;
                }
                if (Frm_TaxAudit.instance != null)
                {
                    Frm_TaxAudit.instance.cod_cliente.Text = item.SubItems[0].Text;
                }
            }
        }

        public void Frm_CustomersList_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Frm_Customers.instance != null)
            {
                Frm_Customers.instance.Captura();
            }
            if (Frm_TaxAudit.instance != null)
            {
                Frm_TaxAudit.instance.CapturaCli();
            }    
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                cls_mysql_conn connection = new cls_mysql_conn();
                connection.OpenConnection();
                string sql = "SELECT * FROM db_sis.tb_customers WHERE CUSTOMER_NAME LIKE @NAME AND STATUS LIKE @STATUS";

                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@NAME","%" + txt_search.Text + "%"),
                    new MySqlParameter("@STATUS", cbb_status.Text)
                };

                MySqlCommand cmd = connection.CreateCommand(sql, parameters);               
                lsv_customers.Items.Clear();
                int[] columnIndexes = { 0, 1, 4 };

                populate.PopulateListViews(lsv_customers, cmd, columnIndexes);                           
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
