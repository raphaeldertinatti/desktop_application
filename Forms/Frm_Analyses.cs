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
    public partial class Frm_Analyses : Form
    {
        cls_mysql_conn connection = new cls_mysql_conn();
        cls_populate_views populate = new cls_populate_views();
        public Frm_Analyses()
        {            
            InitializeComponent();                
            lbl_resumo.Text = Frm_Conferencia.instance.EMP.ToString() + " | CNPJ: " + Frm_Conferencia.instance.CNPJ.ToString() + " | " + Frm_Conferencia.instance.Mes.ToString() + "/" + Frm_Conferencia.instance.Ano.ToString();
            string[] headers = {"CFOP","Valor Contábil","Base ICMS","Isentos ICMS","Outras ICMS","Valor ICMS/ST","Base IPI", "Diferença" };
            int[] widths = {60,100,100,100,100,100,100,160 };
            populate.ConstructListView(lsv_analiseCFOP, headers, widths);  
            ListarDivCFOP();
        }

        public void ListarDivCFOP()
        {            
            try
            {
                connection.OpenConnection(); 
                string sql = "select * from (select CFOP, round(sum(VALOR_CONTABIL), 2) as `Valor Contábil`, round(sum(BASE_ICMS), 2) as `Base ICMS`, round(sum(VALOR_ISENTOS_ICMS), 2) as `Isentos ICMS`, round(sum(VALOR_OUTRAS_ICMS), 2) as `Outras ICMS`, round(sum(VALOR_ICMS_ST), 2) as `Valor ICMS / ST`, round(sum(VALOR_IPI), 2) as `Valor IPI`, round((round(sum(VALOR_CONTABIL), 2) - round(sum(BASE_ICMS), 2) - round(sum(VALOR_ISENTOS_ICMS), 2) - round(sum(VALOR_OUTRAS_ICMS), 2) - round(sum(VALOR_ICMS_ST), 2) - round(sum(VALOR_IPI), 2)), 2) as `Diferença` from db_sis.tb_conf_c5 where COD_CLIENTE = @COD_CLI AND COD_EMPRESA = @COD_EMP AND MES = @MES AND ANO = @ANO group by CFOP order by CFOP) A where Diferença <> 0";
                MySqlParameter[] parameters = GetMySqlParameters();                
                MySqlCommand cmd = connection.CreateCommand(sql, parameters);
                lsv_analiseCFOP.Items.Clear();
                int[] columnIndexes = {0,1,2,3,4,5,6,7};
                populate.PopulateListViews(lsv_analiseCFOP, cmd,columnIndexes);                 
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
        private void ListaDivRazaoFiltro(string cFOP)
        {            
            try
            {
                if (!connection.IsConnectionOpen())
                {
                    connection.OpenConnection(); ;
                }
                using (DataTable dt = new DataTable())
                {
                    string sql = "select * from (select CFOP, RAZAO_SOCIAL, NRO_DOCUMENTO, CHAVE_ACESSO as `CHAVE ACESSO`, round(sum(VALOR_CONTABIL), 2) as `Valor Contábil`, round(sum(BASE_ICMS), 2) as `Base ICMS`, round(sum(VALOR_ISENTOS_ICMS), 2) as `Isentos ICMS`, round(sum(VALOR_OUTRAS_ICMS), 2) as `Outras ICMS`, round(sum(VALOR_ICMS_ST), 2) as `Valor ICMS / ST`, round(sum(VALOR_IPI), 2) as `Valor IPI`, round((round(sum(VALOR_CONTABIL), 2) - round(sum(BASE_ICMS), 2) - round(sum(VALOR_ISENTOS_ICMS), 2) - round(sum(VALOR_OUTRAS_ICMS), 2) - round(sum(VALOR_ICMS_ST), 2) - round(sum(VALOR_IPI), 2)), 2) as `Diferença` from db_sis.tb_conf_c5 where COD_CLIENTE = @COD_CLI AND COD_EMPRESA = @COD_EMP AND MES = @MES AND ANO = @ANO group by CFOP,RAZAO_SOCIAL,NRO_DOCUMENTO,CHAVE_ACESSO order by CFOP) A where Diferença <> 0 AND CFOP =" + cFOP;
                    MySqlParameter[] parameters = GetMySqlParameters();                    
                    MySqlCommand cmd = connection.CreateCommand(sql, parameters);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                        if (dt.Rows.Count > 0)
                        {
                            dgv_ap_detalhado.Columns.Clear();
                            dgv_ap_detalhado.DataSource = dt;
                            var chk = new DataGridViewCheckBoxColumn
                            {
                                ValueType = typeof(bool),
                                Name = "Chk",
                                HeaderText = "CONFERIDO"
                            };
                            dgv_ap_detalhado.Columns.Add(chk);
                        }
                    }                  
                }
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

        private void lsv_analiseCFOP_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ListView.SelectedListViewItemCollection itens_selecionados = lsv_analiseCFOP.SelectedItems;

            foreach (ListViewItem item in itens_selecionados)
            {
                string CFOP = item.SubItems[0].Text;
                ListaDivRazaoFiltro(CFOP);
                CapturaFleg();
            }
        }

        private void Frm_Analyses_Load(object sender, EventArgs e)
        {
            
        }

        private void dgv_ap_detalhado_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (!connection.IsConnectionOpen())
                {
                    connection.OpenConnection();
                }
                if (dgv_ap_detalhado.CurrentCell.Value.ToString() == "True")
                {                    
                    string sql = "INSERT INTO db_sis.tb_conferidos (COD_CLIENTE, COD_EMPRESA, MES, ANO, CHAVE_ACESSO, MODULO) VALUES (@COD_CLI, @COD_EMP, @MES, @ANO," + this.dgv_ap_detalhado.CurrentRow.Cells[3].Value.ToString() + ",'ANALISES')";
                    MySqlParameter[] parameters = GetMySqlParameters();
                    MySqlCommand cmd = connection.CreateCommand(sql, parameters);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    reader.Close();
                    dgv_ap_detalhado.CurrentRow.DefaultCellStyle.BackColor = Color.LightGreen;
                }
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

        private void CapturaFleg()
        {
            try
            {
                connection.OpenConnection();                
                using (DataTable dt = new DataTable())
                {
                    string sql = "select COD_CLIENTE, COD_EMPRESA, MES, ANO, CHAVE_ACESSO, MODULO from db_sis.tb_conferidos where COD_CLIENTE = @COD_CLI AND COD_EMPRESA = @COD_EMP AND MES = @MES AND ANO = @ANO AND MODULO = 'ANALISES'";
                    MySqlParameter[] parameters = GetMySqlParameters();
                    MySqlCommand cmd = connection.CreateCommand(sql, parameters);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataGridViewRow linha in dgv_ap_detalhado.Rows)
                            {
                                if (linha.Cells[3].Value != null)
                                {
                                    if (dt.AsEnumerable().Any(row => linha.Cells["CHAVE ACESSO"].Value.ToString() == row.Field<string>("CHAVE_ACESSO")))
                                    {
                                        linha.Cells[11].Value = true;
                                        linha.DefaultCellStyle.BackColor = Color.LightGreen;
                                    }
                                }
                            }
                        }                        
                    }                       
                }                
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
        private MySqlParameter[] GetMySqlParameters()
        {
            return new MySqlParameter[]
            {
                new MySqlParameter("@COD_CLI",Frm_Conferencia.instance.cod_cliente.Text),
                new MySqlParameter("@COD_EMP",Frm_Conferencia.instance.cod_emp.Text),
                new MySqlParameter("@MES",Frm_Conferencia.instance.Mes),
                new MySqlParameter("@ANO",Frm_Conferencia.instance.Ano)
            };
        }

    }
}
