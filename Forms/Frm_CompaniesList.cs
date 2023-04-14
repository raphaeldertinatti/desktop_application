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
            string[] headers = { "COD", "Razão Social", "CNPJ", "Tipo", "I.E", "Cidade", "UF", "STATUS" };
            int[] widths = { 35, 190, 110, 40, 100, 120, 40, 70 };
            populate.ConstructListView(lsv_empresas, headers, widths);
            if (Frm_Companies.instance != null)
            {
                ListCompanies();
            }
            if (Frm_TaxAudit.instance != null)
            {
                ListCompaniesAudit();
            }
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
         public void ListCompaniesAudit()
        {
            try
            {
                connection.OpenConnection();
                string sql = "SELECT * FROM db_sis.tb_empresa WHERE COD_CLIENTE = "+Frm_TaxAudit.instance.cod_cliente.Text;
                MySqlCommand cmd = new MySqlCommand(sql, connection.conn);
                lsv_empresas.Items.Clear();
                int[] columnIndexes = { 0, 3, 4, 5, 6, 8, 9, 10 };
                populate.PopulateListViews(lsv_empresas, cmd, columnIndexes);
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
            if (Frm_Companies.instance != null)
            {
                Frm_Companies.instance.Captura();
            }
            if (Frm_TaxAudit.instance != null)
            {
                Frm_TaxAudit.instance.CapturaEmp();
            }    
        }

        private void lsv_companies_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ListView.SelectedListViewItemCollection itens_selecionados = lsv_companies.SelectedItems;

            foreach (ListViewItem item in itens_selecionados)
            {
                if (Frm_Companies.instance != null)
                {
                    Frm_Companies.instance.cod.Text = item.SubItems[0].Text;
                }
                if (Frm_TaxAudit.instance !=null)
                {
                    Frm_TaxAudit.instance.cod_emp.Text = item.SubItems[0].Text;
                }
            }
        }
    }

}
