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
using SistemaEtccom.Classes;

namespace DesktopApplication
{
    public partial class Frm_CFOP_CST : Form
    {
        public static Frm_CFOP_CST instance;
        cls_mysql_conn connection = new cls_mysql_conn();
        cls_populate_views populate = new cls_populate_views();
        public Frm_CFOP_CST()
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
                    string sql = "select DISTINCT a.CST_ICMS as `CST`, round(sum(a.VALOR_CONTABIL),2) as `Valor Contábil`, round(sum(a.BASE_ICMS),2) as `Base ICMS`, round(sum(a.VALOR_ICMS),2) as `Valor ICMS`, round(sum(VALOR_ISENTOS_ICMS),2) as `Valor Isentos`, round(sum(VALOR_OUTRAS_ICMS),2) as `Valor Outros`  from db_sis.tb_conf_c5 a inner join db_sis.tb_conf_ndd b on a.CHAVE_ACESSO = b.CHAVE_ACESSO where a.COD_CLIENTE = @COD_CLI AND a.COD_EMPRESA = @COD_EMP AND a.MES = @MES AND a.ANO = @ANO AND a.CFOP =" + cFOP + " group by a.CST_ICMS order by a.CST_ICMS";
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
            Frm_CFOP_CST_Detalhado f = new Frm_CFOP_CST_Detalhado();
            try
            {
                f.txt_CFOP.Text = this.lsv_CFOP.SelectedItems[0].Text;
                f.txt_CST.Text = this.dgv_Natureza.CurrentRow.Cells[0].Value.ToString();

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
                List<string> cfopCompra_Tranf_Dev_Imob = new List<string> {"1102","2102","3102","4102","5102","6102", "1152", "2152", "3152", "4152", "5152", "6152", "1202", "2202", "3202", "4202", "5202", "6202", "1552", "2552", "3552", "4552", "5552", "6552" };                
                List<string> cfopCmpEnerg = new List<string> {"1253","2253","3253","4253","5253","6253"};
                List<string> cfopCmpST_Transf_Dev = new List<string> {"1403","2403","3403","4403","5403","6403", "1409", "2409", "3409", "4409", "5409", "6409", "1411", "2411", "3411", "4411", "5411", "6411" };
                List<string> cfopCmpAtivoST = new List<string> {"1406","2406","3406","4406","5406","6406"};
                List<string> cfopCmpUsoCSt = new List<string> {"1407","2407","3407","4407","5407","6407"};               
                List<string> cfopCmpAtivo_UsoC = new List<string> {"1551","2551","3551","4551","5551","6551", "1556", "2556", "3556", "4556", "5556", "6556" };                
                List<string> cfopTransfUC = new List<string> {"1557","2557","3557","4557","5557","6557"};
                List<string> ent_retVasilhame = new List<string> {"1920","2920","3920","4920","5920","6920", "1921", "2921", "3921", "4921", "5921", "6921" };
               
                if (cfopCompra_Tranf_Dev_Imob.Contains(selectedCFOP))
                {
                    string value = row.Cells[0].Value?.ToString() ?? "";
                    if ((value.Contains("10") && !(value.Contains("100"))) || value.Contains("30") || value.Contains("60") || value.Contains("70"))
                    {
                        needsFormatting = true;
                    }                    
                }
                else if (cfopCmpEnerg.Contains(selectedCFOP))
                {
                    string value = row.Cells[0].Value?.ToString() ?? "";
                    if (!(value.Contains("20") || value.Contains("90") || value.Contains("")))
                    {
                        needsFormatting = true;
                    }
                }
                else if (cfopCmpST_Transf_Dev.Contains(selectedCFOP))
                {
                    string value = row.Cells[0].Value?.ToString() ?? "";
                    if (!(value.Contains("10") || value.Contains("30") || value.Contains("60") || value.Contains("70") || value.Contains("")))
                    {
                        needsFormatting = true;
                    }
                }
                else if (cfopCmpAtivoST.Contains(selectedCFOP))
                {
                    string value = row.Cells[0].Value?.ToString() ?? "";
                    if (!(value.Contains("60")))
                    {
                        needsFormatting = true;
                    }
                }
                else if (cfopCmpUsoCSt.Contains(selectedCFOP))
                {
                    string value = row.Cells[0].Value?.ToString() ?? "";
                    if (!(value.Contains("60") || value.Contains("90")) || value.Contains(""))
                    {
                        needsFormatting = true;
                    }
                }
                else if (cfopCmpAtivo_UsoC.Contains(selectedCFOP))
                {
                    string value = row.Cells[0].Value?.ToString() ?? "";
                    if (!(value.Contains("90") || value.Contains("")))
                    {
                        needsFormatting = true;
                    }
                }
                else if (cfopTransfUC.Contains(selectedCFOP))
                {
                    string value = row.Cells[0].Value?.ToString() ?? "";
                    if (!(value.Contains("41") || value.Contains("")))
                    {
                        needsFormatting = true;
                    }
                }
                else if (ent_retVasilhame.Contains(selectedCFOP))
                {
                    string value = row.Cells[0].Value?.ToString() ?? "";
                    if (!(value.Contains("40") || value.Contains("41") || value.Contains("")))
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
        private void Frm_CFOP_CST_Load(object sender, EventArgs e)
        {
            ListarCFOP();
            lsv_CFOP.Items[0].Selected = true;
        }
    }
}
