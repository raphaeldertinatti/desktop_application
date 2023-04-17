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
    public partial class Frm_TaxAudit : Form
    {
        public static Frm_TaxAudit instance;
        public TextBox cod_cliente;
        public TextBox cod_emp;
        public TextBox import_c5;
        public TextBox import_ndd;
        public TextBox import_fornec;
        public string EMP;
        public string CNPJ;
        public string Mes;
        public string Ano;
        cls_mysql_conn connection = new cls_mysql_conn();     
        
        public Frm_TaxAudit()
        {
            InitializeComponent();
            instance = this;
            cod_cliente = txt_codcliente;
            cod_emp = txt_codempresa;
            import_c5 = txt_ultimportc5;
            import_ndd = txt_ultimportndd;
            import_fornec = txt_ultimportfornec;
            dynamic[] dynamics = { btn_selemp, btn_limpar, btn_NDD, btn_consinco, btn_basefornec, btn_valores, btn_canceladas, btn_nao_relacionadas, btn_analises, btn_nat_operacao, btn_CFOP_CST, btn_prod_rural, btn_transf, btn_salvar, cbb_mês, cbb_ano, txt_anotacoes };
            string[] mês = { "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro" };
            string[] ano = { "2022", "2023", "2024", "2025", "2026", "2027", "2028", "2029", "2030" };
           
            for (int i = 0; i < dynamics.Length; i++)
            {
                dynamics[i].Enabled = false;
            }
            for (int i = 0; i < mês.Length; i++)
            {
                cbb_mês.Items.Add(mês[i]);
            }
            for (int i = 0; i < ano.Length; i++)
            {
                cbb_ano.Items.Add(ano[i]);
            }
        }
        private void btn_selcli_Click(object sender, EventArgs e)
        {
            Frm_Lst_Cst_Audit lista = new Frm_Lst_Cst_Audit();
            lista.ShowDialog();
        }
        public void CapturaCli()
        {
            if (!(txt_codcliente.Text == ""))
            {
                try
                {
                    connection.OpenConnection();
                    string sql = "SELECT * FROM db_sis.tb_cliente WHERE COD_CLIENTE=" + int.Parse(txt_codcliente.Text);
                    MySqlCommand cmd = new MySqlCommand(sql, connection.conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            txt_cliente.Text = reader.GetString("NOME_CLIENTE");
                        }
                        lbl_Cliente.Text = txt_cliente.Text;
                        btn_selcli.Enabled = false;
                        btn_limpar.Enabled = true;
                        btn_selemp.Enabled = true;                        
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
        }
        public void CapturaEmp()
        {
            if (!(txt_codempresa.Text == ""))
            {
                try
                {
                    connection.OpenConnection();
                    string sql = "SELECT * FROM db_sis.tb_empresa WHERE COD_EMPRESA=" + int.Parse(txt_codempresa.Text);                   
                    MySqlCommand cmd = new MySqlCommand(sql, connection.conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            txt_empresa.Text = reader.GetString("RAZAO_SOCIAL");
                            txt_cnpj.Text = reader.GetString("CNPJ");
                        }
                    }
                    lbl_emp.Text = txt_empresa.Text;
                    EMP = lbl_emp.Text;
                    lbl_cnpj.Text = txt_cnpj.Text;
                    CNPJ = lbl_cnpj.Text;
                    btn_selemp.Enabled = false;
                    cbb_mês.Enabled = true;                    
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

        private void btn_limpar_Click(object sender, EventArgs e)
        {
            dynamic[] txt_clean = { txt_codcliente, txt_cliente, txt_codempresa, txt_empresa, txt_cnpj, cbb_mês, cbb_ano, lbl_Cliente, lbl_emp, lbl_cnpj, lbl_Mes, lbl_ano, txt_ultimportc5, txt_ultimportndd, txt_ultimportfornec, txt_anotacoes };
            dynamic[] bool_clean = { btn_selemp, btn_limpar, cbb_mês, cbb_ano, btn_NDD, btn_consinco, btn_basefornec, btn_valores, btn_canceladas, btn_nao_relacionadas, btn_analises, btn_nat_operacao, btn_CFOP_CST, btn_prod_rural, btn_transf, btn_salvar, txt_anotacoes };

            for (int i = 0; i < txt_clean.Length; i++)
            {
                txt_clean[i].Text = ""; 
            }
            for (int i = 0; i < bool_clean.Length; i++)
            {
                bool_clean[i].Enabled = false;
            }
            btn_selcli.Enabled = true;
        }

        private void btn_selemp_Click(object sender, EventArgs e)
        {
            Lst_Cmp_Audit lista = new Lst_Cmp_Audit();
            lista.ShowDialog();
        }

        private void cbb_mês_SelectionChangeCommitted(object sender, EventArgs e)
        {
            lbl_Mes.Text = cbb_mês.SelectedItem.ToString();
            Mes = cbb_mês.SelectedItem.ToString();
            btn_selemp.Enabled = false;            
            cbb_mês.Enabled = false;
            cbb_ano.Enabled = true;
        }

        private void cbb_ano_SelectionChangeCommitted(object sender, EventArgs e)
        {
            lbl_ano.Text = cbb_ano.SelectedItem.ToString();
            Ano = cbb_ano.SelectedItem.ToString();
            btn_selemp.Enabled = false;
            cbb_ano.Enabled = false;
            dynamic[] btn_true = { btn_NDD, btn_consinco, btn_basefornec, btn_valores, btn_canceladas, btn_nao_relacionadas, btn_analises, btn_nat_operacao, btn_CFOP_CST, btn_prod_rural, btn_transf, btn_salvar, txt_anotacoes };
            for (int i = 0; i < btn_true.Length; i++)
            {
                btn_true[i].Enabled = true;
            }
            CheckRowsC5();
            CheckRowsNDD();
            CheckRowsNFornec();
            CapturaObs();
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void btn_system_Click(object sender, EventArgs e)
        {
            Frm_Audit_System f = new Frm_Audit_System();
            f.ShowDialog();
        }

        private void btn_IRS_Click(object sender, EventArgs e)
        {
            Frm_Audit_IRS f = new Frm_Audit_IRS();
            f.ShowDialog();
        }
        private void CheckRowsC5()
        {
            try
            {
                connection.OpenConnection();
                string sql = "SELECT DTA_IMPORTACAO FROM db_sis.tb_conf_c5 WHERE COD_CLIENTE = @COD_CLI AND COD_EMPRESA = @COD_EMP AND MES = @MES AND ANO = @ANO";
                Check_Importado(sql, txt_ultimportc5);                        
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

        private void CheckRowsNDD()
        {
            try
            {
                connection.OpenConnection();
                string sql = "SELECT DTA_IMPORTACAO FROM db_sis.tb_conf_ndd WHERE COD_CLIENTE = @COD_CLI AND COD_EMPRESA = @COD_EMP AND MES = @MES AND ANO = @ANO";
                Check_Importado(sql, txt_ultimportndd);                  
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

        private void CheckRowsNFornec()
        {
            try
            {
                connection.OpenConnection();
                string sql = "SELECT DTA_IMPORTACAO FROM db_sis.tb_conf_fornec WHERE COD_CLIENTE =" + int.Parse(txt_codcliente.Text);
                Check_Importado(sql, txt_ultimportfornec);
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

        private void Frm_TaxAudit_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void btn_basesupplier_Click(object sender, EventArgs e)
        {
            Frm_Audit_Supplier f = new Frm_Audit_Supplier();
            f.ShowDialog();
        }

        private void btn_valores_Click(object sender, EventArgs e)
        {
            Frm_Audit_Values f = new Frm_Audit_Values();
            f.ShowDialog();
        }

        private void brn_canceladas_Click(object sender, EventArgs e)
        {
            try
            {
                connection.OpenConnection();
                string sql = "select DISTINCT a.TIPO, a.NUMERO_DA_NOTA, a.SERIE, a.ENTRADA_SAIDA, a.NATUREZA_OPERACAO, a.CHAVE_ACESSO, a.RAZAO_SOCIAL, a.STATUS_NFE, a.DTA_EMISSAO, a.ULTIMA_ITERACAO from db_sis.tb_conf_ndd a inner join db_sis.tb_conf_c5 b on a.CHAVE_ACESSO = b.CHAVE_ACESSO where a.STATUS_NFE like '%Cancelado%' and a.COD_CLIENTE = @COD_CLI AND a.COD_EMPRESA = @COD_EMP AND a.MES = @MES AND a.ANO = @ANO order by a.NUMERO_DA_NOTA";
                string msg = "Não foram encontradas notas canceladas na base consinco.";
                string title = "Notas Canceladas";
                Frm_Canceladas form = new Frm_Canceladas();
                Check_OpenForm(sql, form, msg, title);                   
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

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btn_nao_relacionadas_Click(object sender, EventArgs e)
        {
            try
            {
                connection.OpenConnection();
                string sql = "select DISTINCT ESPECIE as `Tipo`, NRO_DOCUMENTO as `Número NF`, SERIE as `Serie`, CHAVE_ACESSO `Chave de Acesso`, RAZAO_SOCIAL as `Razão Social` from db_sis.tb_conf_c5 where COD_CLIENTE = @COD_CLI AND COD_EMPRESA = @COD_EMP AND MES = @MES AND ANO = @ANO AND CHAVE_ACESSO not in (select CHAVE_ACESSO from db_sis.tb_conf_ndd where COD_CLIENTE = @COD_CLI AND COD_EMPRESA = @COD_EMP AND MES = @MES AND ANO = @ANO)";
                string msg = "Não foram encontradas notas não relacionadas.";
                string title = "Notas Não Relacionadas.";
                Frm_Nao_Relacionadas Form = new Frm_Nao_Relacionadas();
                Check_OpenForm(sql, Form, msg, title);             
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

        private void btn_analises_Click(object sender, EventArgs e)
        {
            try
            {
                connection.OpenConnection();
                string sql = "select * from (select CFOP, round(sum(VALOR_CONTABIL), 2) as `Valor Contábil`, round(sum(BASE_ICMS), 2) as `Base ICMS`, round(sum(VALOR_ISENTOS_ICMS), 2) as `Isentos ICMS`, round(sum(VALOR_OUTRAS_ICMS), 2) as `Outras ICMS`, round(sum(VALOR_ICMS_ST), 2) as `Valor ICMS / ST`, round(sum(VALOR_IPI), 2) as `Valor IPI`, round((round(sum(VALOR_CONTABIL), 2) - round(sum(BASE_ICMS), 2) - round(sum(VALOR_ISENTOS_ICMS), 2) - round(sum(VALOR_OUTRAS_ICMS), 2) - round(sum(VALOR_ICMS_ST), 2) - round(sum(VALOR_IPI), 2)), 2) as `Diferença` from db_sis.tb_conf_c5 where COD_CLIENTE = @COD_CLI AND COD_EMPRESA = @COD_EMP AND MES = @MES AND ANO = @ANO group by CFOP order by CFOP) A where Diferença <> 0";
                string msg = "Não foram encontradas notas com divergência para análise.";
                string title = "Notas Divergentes";
                Frm_Analises form = new Frm_Analises();
                Check_OpenForm(sql, form, msg, title);
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

        private void btn_nat_operacao_Click(object sender, EventArgs e)
        {
            try
            {
                connection.OpenConnection();
                string sql = "select DISTINCT a.CFOP from db_sis.tb_conf_c5 a inner join db_sis.tb_conf_ndd b on a.CHAVE_ACESSO = b.CHAVE_ACESSO where a.COD_CLIENTE = @COD_CLI AND a.COD_EMPRESA = @COD_EMP AND a.MES = @MES AND a.ANO = @ANO order by a.CFOP";
                string msg = "Não foram encontrados registros para este cliente, certifique-se de que as bases estejam importadas..";
                string title = "Natureza Operação";
                Frm_Natureza_Operacao Form = new Frm_Natureza_Operacao();
                Check_OpenForm(sql, Form, msg, title);          
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
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                connection.OpenConnection();
                string sql = "select DISTINCT a.CFOP from db_sis.tb_conf_c5 a inner join db_sis.tb_conf_ndd b on a.CHAVE_ACESSO = b.CHAVE_ACESSO where a.COD_CLIENTE = @COD_CLI AND a.COD_EMPRESA = @COD_EMP AND a.MES = @MES AND a.ANO = @ANO order by a.CFOP";
                string msg = "Não foram encontrados registros para este cliente, certifique-se de que as bases estejam importadas..";
                string title = "Natureza Operação";
                Frm_CFOP_CST Form = new Frm_CFOP_CST();
                Check_OpenForm(sql,Form,msg,title);       
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
        private void btn_prod_rural_Click(object sender, EventArgs e)
        {
            try
            {
                connection.OpenConnection();
                string sql = "select DISTINCT a.CGO from db_sis.tb_conf_c5 a inner join db_sis.tb_conf_fornec b on a.PESSOA = b.SEQFORNECEDOR where a.COD_CLIENTE =" + Frm_TaxAudit.instance.cod_cliente.Text + " AND b.PRODRURAL = 'SIM' order by CGO";
                string msg = "Não foram encontrados registros para este cliente, certifique-se de que as bases estejam importadas..";
                string title = "Natureza Operação";
                Frm_Prod_Rural form = new Frm_Prod_Rural();
                Check_OpenForm(sql, form, msg, title);                  
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

        private void btn_transf_Click(object sender, EventArgs e)
        {
            Frm_Transferencia f = new Frm_Transferencia();
            f.ShowDialog();
        }

        private void btn_salvar_Click(object sender, EventArgs e)
        {
            try
            {
                connection.OpenConnection();

                using (DataTable dt = new DataTable())
                {
                    string sql = "select * from db_sis.tb_conf_obs where COD_CLIENTE = @COD_CLI AND COD_EMPRESA = @COD_EMP AND MES = @MES AND ANO = @ANO";

                    MySqlParameter[] parameters = GetSqlParameters();
                    MySqlCommand cdm = connection.CreateCommand(sql, parameters);
                    using (MySqlDataReader reader = cdm.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                    connection.CloseConnection();
                    if (dt.Rows.Count > 0)
                    {
                        UpdateObs();
                    }
                    else
                    {
                        InsertObs();
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
        private void UpdateObs()
        {
            try
            {
                connection.OpenConnection();               
                string sql = "UPDATE db_sis.tb_conf_obs set OBS = @OBS where COD_CLIENTE = @COD_CLI AND COD_EMPRESA = @COD_EMP AND MES = @MES AND ANO = @ANO";
                MySqlParameter[] parameters = GetSqlParameters();
                MySqlCommand cmd = connection.CreateCommand(sql, parameters);
                MySqlDataReader reader = cmd.ExecuteReader();                
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
        private void InsertObs()
        {           
            try
            {
                connection.OpenConnection();
                string sql = "INSERT INTO db_sis.tb_conf_obs (COD_CLIENTE, COD_EMPRESA, MES, ANO, OBS) VALUES (@COD_CLI, @COD_EMP, @MES, @ANO, @OBS)";
                MySqlParameter[] parameters = GetSqlParameters();
                MySqlCommand cmd = connection.CreateCommand(sql, parameters);
                MySqlDataReader reader = cmd.ExecuteReader();                
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
        public void CapturaObs()
        {     
            try
            {
                connection.OpenConnection();                
                string sql = "SELECT OBS FROM db_sis.tb_conf_obs WHERE COD_CLIENTE = @COD_CLI AND COD_EMPRESA = @COD_EMP AND MES = @MES AND ANO = @ANO";
                MySqlParameter[] parameters = GetSqlParameters();
                MySqlCommand cmd = connection.CreateCommand(sql, parameters);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        txt_anotacoes.Text = reader.GetString("OBS");
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
        private void Check_OpenForm(string sql, Form form, string msg, string title)
        {
            try
            {
                using (DataTable dt = new DataTable())
                {
                    MySqlParameter[] parameters = GetSqlParameters(); 
                    MySqlCommand cmd = connection.CreateCommand(sql, parameters);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                    if (dt.Rows.Count > 0)
                    {
                        form.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Check_Importado(string sql, TextBox TB)
        {
            try
            {
                using (DataTable dt = new DataTable())
                {
                    MySqlParameter[] parameters = GetSqlParameters();
                    MySqlCommand cmd = connection.CreateCommand(sql, parameters);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                    if (dt.Rows.Count > 0)
                    {
                        TB.Text = "Importado";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private MySqlParameter[] GetSqlParameters()
        {
            return new MySqlParameter[]
            {
            new MySqlParameter("@MES", Frm_TaxAudit.instance.Mes),
            new MySqlParameter("@COD_CLI",Frm_TaxAudit.instance.cod_cliente.Text),
            new MySqlParameter("@COD_EMP",Frm_TaxAudit.instance.cod_emp.Text),
            new MySqlParameter("@ANO",Frm_TaxAudit.instance.Ano),
            new MySqlParameter("@OBS",txt_anotacoes.Text)
            };
        }
    }
}
