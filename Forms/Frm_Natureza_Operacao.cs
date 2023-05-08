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
    public partial class Frm_Natureza_Operacao : Form
    {
        public static Frm_Natureza_Operacao instance;
        cls_mysql_conn connection = new cls_mysql_conn();
        cls_populate_views populate = new cls_populate_views();
        
        public Frm_Natureza_Operacao()
        {
            InitializeComponent();           
            lbl_resumo.Text = Frm_Conferencia.instance.EMP.ToString() + " | CNPJ: " + Frm_Conferencia.instance.CNPJ.ToString() + " | " + Frm_Conferencia.instance.Mes.ToString() + "/" + Frm_Conferencia.instance.Ano.ToString();

            lsv_CFOP.View = View.Details;           
            lsv_CFOP.AllowColumnReorder = true;
            lsv_CFOP.FullRowSelect = true;
            lsv_CFOP.GridLines = true;
            lsv_CFOP.Columns.Add("CFOP", 60, HorizontalAlignment.Left);           
        }

        private void ListarCFOP()
        {
            try
            {                
                connection.OpenConnection();                
                string sql = "select DISTINCT a.CFOP from db_sis.tb_conf_c5 a inner join db_sis.tb_conf_ndd b on a.CHAVE_ACESSO = b.CHAVE_ACESSO where a.COD_CLIENTE = @COD_CLI AND a.COD_EMPRESA = @COD_EMP AND a.MES = @MES AND a.ANO = @ANO order by a.CFOP";
                int[] columnindexes = { 0 };
                MySqlParameter[] parameters = GetMySqlParameters();
                MySqlCommand cmd = connection.CreateCommand(sql, parameters);              
                lsv_CFOP.Items.Clear();
                populate.PopulateListViews(lsv_CFOP, cmd, columnindexes);
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
                string CFOP = item.SubItems[0].Text;
                ListaNatureza(CFOP); 
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
                    string sql = "select DISTINCT b.NATUREZA_OPERACAO as `NATUREZA DA OPERAÇÃO` from db_sis.tb_conf_c5 a inner join db_sis.tb_conf_ndd b on a.CHAVE_ACESSO = b.CHAVE_ACESSO where a.COD_CLIENTE = @COD_CLI AND a.COD_EMPRESA = @COD_EMP AND a.MES = @MES AND a.ANO = @ANO AND a.CFOP =" + cFOP + " order by 'NATUREZA DA OPERAÇÃO'";
                    MySqlParameter[] parameters = GetMySqlParameters();
                    MySqlCommand cmd = connection.CreateCommand(sql,parameters);
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
            Frm_Natureza_Detalhado f = new Frm_Natureza_Detalhado();
            try
            {
                f.txt_CFOP.Text = this.lsv_CFOP.SelectedItems[0].Text;
                f.txt_Natureza.Text = this.dgv_Natureza.CurrentRow.Cells[0].Value.ToString(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            f.ShowDialog();
        }

        private void dgv_Natureza_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                DataGridViewRow row = dgv_Natureza.Rows[e.RowIndex];
                string selectedCFOP = lsv_CFOP.SelectedItems[0].Text;
                bool needsFormatting = false;
                Color backColor = Color.Red;
                List<string> cfopsCompra = new List<string> {"1101","1102","1113","1118","1121","1253","1303","1353","1401","1403","1406","1407","1551","1556","2101","2102","2113","2120","2121","2253","2303","2353","2401","2403","2406","2407","2551","2556" };
                List<string> cfopsVenda = new List<string> { "5101","5102","5103","5104","5105","5106","5113","5114","5116","5401","5402","5403","5405","5551","6101","6102","6103","6104","6105","6107","6108","6113","6117"};
                List<string> cfopsBonif = new List<string> { "1910","1911","2910","2911","3910","3911","4910","4911","5910","5911","6910","6911"};
                List<string> cfopsTransf = new List<string> { "1551","1152","1552","1557","2152","2408","2409","2552","2557","5151","5152","5408","5409","5552","5557","6151","6152","6408","6409","6552","6557"};
                if (cfopsCompra.Contains(selectedCFOP))
                {
                    string value = row.Cells["NATUREZA DA OPERAÇÃO"].Value?.ToString() ?? "";
                    if (value.Contains("BONIFICA") || value.Contains("TRANSFER") || value.Contains("DEVOLU") || value.Contains("REMESSA") || value.Contains("RETORNO"))
                    {
                        needsFormatting = true;
                    }
                }
                else if (cfopsVenda.Contains(selectedCFOP))
                {
                    string value = row.Cells["NATUREZA DA OPERAÇÃO"].Value?.ToString() ?? "";
                    if (value.Contains("BONIFICA") || value.Contains("TRANSFER") || value.Contains("DEVOLU") || value.Contains("REMESSA") || value.Contains("RETORNO"))
                    {
                        needsFormatting = true;
                    }
                }
                else if (cfopsBonif.Contains(selectedCFOP))
                {
                    string value = row.Cells["NATUREZA DA OPERAÇÃO"].Value?.ToString() ?? "";
                    if (value.Contains("VENDA") || value.Contains("COMPRA") || value.Contains("VASILHAME"))
                    {
                        needsFormatting = true;
                    }
                }
                else if (cfopsTransf.Contains(selectedCFOP))
                {
                    string value = row.Cells["NATUREZA DA OPERAÇÃO"].Value?.ToString() ?? "";
                    if (value.Contains("VENDA") || value.Contains("COMPRA") || value.Contains("VASILHAME") || value.Contains("BONIFIC") || value.Contains("REMESSA") || value.Contains("RETORNO") || value.Contains("DEVOLU"))
                    {
                        needsFormatting = true;
                    }
                }
                if (needsFormatting)
                {
                    row.DefaultCellStyle.BackColor = backColor;
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgv_Natureza_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void dgv_Natureza_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void dgv_Natureza_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
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
        private void Frm_Natureza_Operacao_Load(object sender, EventArgs e)
        {
            ListarCFOP();
            lsv_CFOP.Items[0].Selected = true;
        }
    }
}
