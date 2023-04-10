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
    public partial class Frm_CompaniesList : Form
    {
        public static Frm_CompaniesList instance;

        cls_mysql_conn connection = new cls_mysql_conn();
        cls_populate_views populate = new cls_populate_views();        
        
        public Frm_CompaniesList()
        {
            InitializeComponent();
            instance = this;            
            lsv_companies.View = View.Details;
            lsv_companies.LabelEdit = true;
            lsv_companies.AllowColumnReorder = true;
            lsv_companies.FullRowSelect = true;
            lsv_companies.GridLines = true;
            lsv_companies.Columns.Add("COD", 35, HorizontalAlignment.Left);
            lsv_companies.Columns.Add("Razão Social", 190, HorizontalAlignment.Left);
            lsv_companies.Columns.Add("CNPJ", 110, HorizontalAlignment.Left);
            lsv_companies.Columns.Add("Tipo", 40, HorizontalAlignment.Left);
            lsv_companies.Columns.Add("I.E.", 100, HorizontalAlignment.Left);            
            lsv_companies.Columns.Add("Cidade", 120, HorizontalAlignment.Left);
            lsv_companies.Columns.Add("UF", 40, HorizontalAlignment.Left);
            lsv_companies.Columns.Add("STATUS", 70, HorizontalAlignment.Left);
            ListCompanies();
            cbb_status.SelectedIndex = 0;
        }
        public void btn_Select_Click(object sender, EventArgs e)
        {
            this.Close();
        }
		
        public void ListCompanies()
        {
            try
            {
                connection.OpenConnection();
                string sql = "SELECT * FROM db_sis.tb_empresa";                
                MySqlCommand cmd = new MySqlCommand(sql, connection.conn);   
                lsv_companies.Items.Clear();
                int[] columnIndexes = { 0, 3, 4, 5, 6, 8, 9, 10 };
                populate.PopulateListViews(lsv_companies,cmd,columnIndexes);                
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
        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                connection.OpenConnection(); 
                string sql = "SELECT * FROM db_sis.tb_empresa WHERE RAZAO_SOCIAL LIKE @RAZAO_S AND STATUS LIKE @STATUS";

                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@RAZAO_S","%" + txt_buscar.Text + "%"),
                    new MySqlParameter("@STATUS", cbb_status.Text)
                };

                MySqlCommand cmd = connection.CreateCommand(sql, parameters);
                lsv_companies.Items.Clear();
                int[] columnIndexes = { 0, 3, 4, 5, 6, 8, 9, 10 };
                populate.PopulateListViews(lsv_companies, cmd, columnIndexes);
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

        private void Frm_CompaniesList_FormClosed(object sender, FormClosedEventArgs e)
        {
            Frm_Companies.instance.Captura();
        }

        private void lsv_companies_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ListView.SelectedListViewItemCollection itens_selecionados = lsv_companies.SelectedItems;

            foreach (ListViewItem item in itens_selecionados)
            {
                Frm_Companies.instance.cod.Text = item.SubItems[0].Text;
            }
        }
    }

}
