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
    public partial class Frm_DepList : Form
    {
        public static Frm_DepList instance;
        cls_mysql_conn connection = new cls_mysql_conn();
        cls_populate_views populate = new cls_populate_views();
		
        public Frm_DepList()
        {
            InitializeComponent();
            instance = this;
            lsv_dep.View = View.Details;
            lsv_dep.LabelEdit = true;
            lsv_dep.AllowColumnReorder = true;
            lsv_dep.FullRowSelect = true;
            lsv_dep.GridLines = true;
            lsv_dep.Columns.Add("Código", 60, HorizontalAlignment.Left);
            lsv_dep.Columns.Add("Departamento", 180, HorizontalAlignment.Left);            
            ListDEP();            
        }
        public void ListDEP()
        {
            try
            {
                connection.OpenConnection();                
                string sql = "SELECT * FROM db_sis.tb_departments";                
                MySqlCommand cmd = new MySqlCommand(sql, connection.conn);  
                lsv_dep.Items.Clear();
                int[] columnIndexes = { 0, 1 };
                populate.PopulateListViews(lsv_dep, cmd, columnIndexes);
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

        private void btn_Select_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lsv_dep_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ListView.SelectedListViewItemCollection itens_selecionados = lsv_dep.SelectedItems;

            foreach (ListViewItem item in itens_selecionados)
            {
                Frm_Services.instance.cod_dep.Text = item.SubItems[0].Text;
                Frm_Services.instance.desc_dep.Text = item.SubItems[1].Text;
            }
        }

        private void Frm_DepList_FormClosed(object sender, FormClosedEventArgs e)
        {            
            Frm_Services.instance.ListarServicosDEP();
            Frm_Services.instance.ListarServClientesDEP();
        }
    }
}
