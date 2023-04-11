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
    public partial class Frm_ContactsList : Form
    {
        public static Frm_ContactsList instance;
        cls_mysql_conn connection = new cls_mysql_conn();
        cls_populate_views populate = new cls_populate_views();

        public Frm_ContactsList()
        {
            InitializeComponent();
            instance = this;
            lsv_contacts.View = View.Details;
            lsv_contacts.LabelEdit = true;
            lsv_contacts.AllowColumnReorder = true;
            lsv_contacts.FullRowSelect = true;
            lsv_contacts.GridLines = true;
            lsv_contacts.Columns.Add("Cód", 40, HorizontalAlignment.Left);
            lsv_contacts.Columns.Add("Nome", 160, HorizontalAlignment.Left);
            lsv_contacts.Columns.Add("Cargo", 100, HorizontalAlignment.Left);
            lsv_contacts.Columns.Add("e-mail", 190, HorizontalAlignment.Left);
            lsv_contacts.Columns.Add("Telefone", 100, HorizontalAlignment.Left);
            ListContacts();            
        }

        private void btn_Select_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void ListContacts()
        {
            try
            {
                connection.OpenConnection();
                string sql = "SELECT * FROM db_sis.tb_contacts";                
                MySqlCommand cmd = new MySqlCommand(sql, connection.conn);    
                lsv_contacts.Items.Clear();
                int[] columnIndexes = { 0, 2, 3, 4, 5 };
                populate.PopulateListViews(lsv_contacts, cmd, columnIndexes);
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
                string sql = "SELECT * FROM db_sis.tb_contacts WHERE NAME LIKE @NAME";
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@NAME", "%" + txt_search.Text + "%")
                };
                MySqlCommand cmd = connection.CreateCommand(sql, parameters);
                lsv_contacts.Items.Clear();
                int[] columnIndexes = { 0, 1, 2, 3, 4, 5 };
                populate.PopulateListViews(lsv_contacts, cmd, columnIndexes);
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

        private void Frm_ContactsList_FormClosed(object sender, FormClosedEventArgs e)
        {
            Frm_Contatos.instance.Captura();
        }

        private void lsv_contacts_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ListView.SelectedListViewItemCollection itens_selecionados = lsv_contacts.SelectedItems;

            foreach (ListViewItem item in itens_selecionados)
            {
                Frm_Contacts.instance.cod.Text = item.SubItems[0].Text;
            }
        }
    }
}
