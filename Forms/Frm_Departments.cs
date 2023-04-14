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
    public partial class Frm_Departments : Form
    {
        cls_mysql_conn connection = new cls_mysql_conn();
        cls_populate_views populate = new cls_populate_views();
        
        public Frm_Departments()
        {
            InitializeComponent();            
            string[] headers = { "Cód", "Departamento" };
            int[] widths = { 40, 200 };
            populate.ConstructListView(lsv_departamento, headers, widths);       
            ListarDepartamentos();
        }
        public void ListarDepartamentos()
        {
            try
            {
                connection.OpenConnection();                
                string sql = "SELECT * FROM db_sis.tb_department";               
                MySqlCommand cmd = new MySqlCommand(sql, connection.conn);                
                lsv_department.Items.Clear();
                int[] columnIndexes = { 0, 1 };
                populate.PopulateListViews(lsv_department, cmd, columnIndexes);                
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

        private void lsv_department_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ListView.SelectedListViewItemCollection itens_selecionados = lsv_department.SelectedItems;

            foreach (ListViewItem item in itens_selecionados)
            {
                txt_coddep.Text = item.SubItems[0].Text;
                txt_desc.Text = item.SubItems[1].Text;
            }
        }

        private void tsb_clear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void tsb_add_Click(object sender, EventArgs e)
        {
            if (txt_coddep.Text == "")
            {
                if (txt_desc.Text == "")
                {
                    MessageBox.Show("Department name cannot be null.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    try
                    {
                        connection.OpenConnection();
                        string sql = "INSERT INTO db_sis.tb_department (DESCRICAO) VALUES ('" + txt_desc.Text + "')";                        
                        MySqlCommand cmd = new MySqlCommand(sql, connection.conn);
                        MySqlDataReader reader = cmd.ExecuteReader();
                        MessageBox.Show("Department added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Clear the form to insert a new department.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Clear()
        {
            txt_coddep.Text = "";
            txt_desc.Text = "";
        }
    }
}
