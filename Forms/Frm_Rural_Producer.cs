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
    public partial class Frm_Rural_Producer : Form
    {
        public static Frm_Rural_Producer instance;
        cls_mysql_conn connection = new cls_mysql_conn();
        cls_populate_views populate = new cls_populate_views();

        public Frm_Rural_Producer()
        {
            InitializeComponent();            
            lbl_resumo.Text = Frm_Conferencia.instance.EMP.ToString() + " | CNPJ: " + Frm_Conferencia.instance.CNPJ.ToString() + " | " + Frm_Conferencia.instance.Mes.ToString() + "/" + Frm_Conferencia.instance.Ano.ToString();           
            lsv_CFOP.View = View.Details;           
            lsv_CFOP.AllowColumnReorder = true;
            lsv_CFOP.FullRowSelect = true;
            lsv_CFOP.GridLines = true;
            lsv_CFOP.Columns.Add("CGO", 60, HorizontalAlignment.Left);            
        }

        private void ListarCFOP()
        {
            try
            {
                connection.OpenConnection();
                string sql = "call db_sis.sp_Select_ProdRural(@COD_CLI,@COD_EMP,@MES,@ANO)";
                int[] columns = { 0 };
                MySqlParameter[] parameters = GetMySqlParameters();
                MySqlCommand cmd = connection.CreateCommand(sql, parameters); 
                lsv_CFOP.Items.Clear();
                populate.PopulateListViews(lsv_CFOP, cmd, columns);
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

        private void lsv_CFOP_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ListView.SelectedListViewItemCollection itens_selecionados = lsv_CFOP.SelectedItems;

            foreach (ListViewItem item in itens_selecionados)
            {
                string cFOP = item.SubItems[0].Text;
                ListaNatureza(cFOP);
            }
        }

        private void ListaNatureza(string cFOP)
        {
            try
            {
                connection.OpenConnection();
                this.Cursor = Cursors.WaitCursor;
                using (DataTable dt = new DataTable())
                {
                    string sql = "call db_sis.sp_Conf_Prod_Rural(@COD_CLI,@COD_EMP,@MES,@ANO," + cFOP + ")";
                    MySqlParameter[] parameters = GetMySqlParameters();
                    MySqlCommand cmd = connection.CreateCommand(sql, parameters);                   
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                        if (dt.Rows.Count > 0)
                        {
                            dgv_Natureza.DataSource = dt;
                            dgv_Natureza.CurrentCell = null;
                        }                        
                    }                        
                }
                this.Cursor = Cursors.Default;
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

        private void dgv_Natureza_DoubleClick(object sender, EventArgs e)
        {
            Frm_Rural_Producer_Detalhado f = new Frm_Rural_Producer_Detalhado();
            try
            {
                f.txt_CFOP.Text = this.lsv_CFOP.SelectedItems[0].Text;
                f.txt_Natureza.Text = this.dgv_Natureza.CurrentRow.Cells[1].Value.ToString();              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            f.ShowDialog();
        }
        private MySqlParameter[] GetMySqlParameters()
        {
            return new MySqlParameter[]
            {
                new MySqlParameter("@COD_CLI",Frm_Conferencia.instance.cod_cliente.Text),
                new MySqlParameter("@COD_EMP",Frm_Conferencia.instance.cod_emp.Text),
                new MySqlParameter("@MES", Frm_Conferencia.instance.Mes),
                new MySqlParameter("@ANO", Frm_Conferencia.instance.Ano)
            };
        }
        private void Frm_Rural_Producer_Load(object sender, EventArgs e)
        {
            ListarCFOP();
            lsv_CFOP.Items[0].Selected = true;
        }
    }
}
