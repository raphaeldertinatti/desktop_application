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
using System.IO;

namespace DesktopApplication
{
    public partial class Frm_Audit_IRS : Form
    {
        cls_mysql_conn connection = new cls_mysql_conn();
        private BindingSource BSource = new BindingSource();
        public Frm_Audit_IRS()
        {
            InitializeComponent();           
            lbl_resumo.Text = Frm_TaxAudit.instance.EMP.ToString() + " | CNPJ: " + Frm_TaxAudit.instance.CNPJ.ToString() + " | " + Frm_TaxAudit.instance.Mes.ToString() + "/" + Frm_TaxAudit.instance.Ano.ToString();
        }

        private void btn_import_Click(object sender, EventArgs e) //IMPORTAR ENTRADA
        {
            string ent_sai = "ENTRADA";
            Import_NDD(ent_sai);
        }
        private void Frm_Audit_IRS_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }

        private void Frm_Audit_IRS_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Antes de importar o arquivo .csv gerado pelo NDD: abra-o no excel, trate os campos IE Emitente e IE Destinatário como campos numéricos, exporte-o em .csv para então importar este novo arquivo no sistema.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            BindData();
        }

        private void BindData()
        {
            try
            {
                connection.OpenConnection();
                dgv_conf_ndd.DataSource = BSource;
                using (DataTable dt = new DataTable())
                {                    
                    string sql = "SELECT ENTRADA_SAIDA,SERIE, NUMERO_DA_NOTA, TIPO, CHAVE_ACESSO, RAZAO_SOCIAL, DTA_EMISSAO, CNPJ_EMITENTE, CNPJ_DESTINATARIO, CPF_DESTINATARIO, RAZAO_SOCIAL_DESTINATARIO, UF, MODELO, COD_NUMERICO, NATUREZA_OPERACAO, IE_EMITENTE, IE_DESTINATARIO, UF_DESTINATARIO, BASE_ICMS, BASE_ICMS_ST, TOTAL_PRODUTOS, VALOR_ICMS, VALOR_ICMS_ST, VALOR_IPI, VALOR_COFINS, BASE_ISS, CNPJ_TRANSPORTADORA, PLACA_TRANSPORTADORA, TIPO_EMISSAO, STATUS_NFE, VALOR_TOTAL_NFE, ULTIMA_ITERACAO, ULTIMO_EVENTO,  DTA_IMPORTACAO FROM db_sis.tb_conf_ndd WHERE COD_CLIENTE = @COD_CLI AND COD_EMPRESA = @COD_EMP AND MES = @MES AND ANO = @ANO";

                    MySqlParameter[] parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@COD_CLI", Frm_TaxAudit.instance.cod_cliente.Text),
                        new MySqlParameter("@COD_EMP", Frm_TaxAudit.instance.cod_emp.Text),
                        new MySqlParameter("@MES", Frm_TaxAudit.instance.Mes),
                        new MySqlParameter("@ANO", Frm_TaxAudit.instance.Ano)
                    };

                    MySqlCommand cmd = connection.CreateCommand(sql, parameters);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    dt.Load(reader);

                    if (dt.Rows.Count > 0)
                    {
                        BSource.DataSource = dt;
                    }                    
                    CapturaUltImport();
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

        public void DeleteNDD(string ent_sai)
        {
            try
            {
                connection.OpenConnection();
                string sql = "DELETE FROM db_sis.tb_conf_ndd WHERE COD_CLIENTE = @COD_CLI AND COD_EMPRESA = @COD_EMP AND MES = @MES AND ANO = @ANO AND ENTRADA_SAIDA = @ENT_SAI";
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                new MySqlParameter("@COD_CLI",Frm_TaxAudit.instance.cod_cliente.Text),
                new MySqlParameter("@COD_EMP",Frm_TaxAudit.instance.cod_emp.Text),
                new MySqlParameter("@MES", Frm_TaxAudit.instance.Mes),
                new MySqlParameter("@ANO",Frm_TaxAudit.instance.Ano),
                new MySqlParameter("@ENT_SAI", ent_sai)
                };

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

        public void CaptureUltImport()
        {
            if (dgv_conf_ndd.Rows.Count > 0)
            {
                try
                {
                    string sql = "SELECT DISTINCT MAX(DTA_IMPORTACAO) FROM db_sis.tb_conf_ndd WHERE COD_CLIENTE = @COD_CLI AND COD_EMPRESA = @COD_EMP AND MES = @MES AND ANO = @ANO";

                    MySqlParameter[] parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@COD_CLI",Frm_TaxAudit.instance.cod_cliente.Text),
                        new MySqlParameter("@COD_EMP",Frm_TaxAudit.instance.cod_emp.Text),
                        new MySqlParameter("@MES",Frm_TaxAudit.instance.Mes),
                        new MySqlParameter("@ANO",Frm_TaxAudit.instance.Ano)
                    };

                    MySqlCommand cmd = connection.CreateCommand(sql, parameters);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            txt_ultimportndd.Text = reader.GetString("MAX(DTA_IMPORTACAO)");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void brn_imp_sai_Click(object sender, EventArgs e) // IMPORTAR SAIDA
        {
            string ent_sai = "SAIDA";
            Import_NDD(ent_sai);
        }

        public void Import_NDD (string ent_sai)
        {
            OpenFileDialog arquivo = new OpenFileDialog();
            arquivo.InitialDirectory = "C:\\";
            arquivo.Filter = "csv|*.csv|txt|*.txt";
            arquivo.Title = "Selecione o arquivo .txt ou .csv";

            if (arquivo.ShowDialog() == DialogResult.OK)
            {
                string nomearquivo = arquivo.FileName;
                try
                {
                    var reader = new StreamReader(File.OpenRead(nomearquivo));
                    var line = reader.ReadLine();
                    var columns = line.Split(';');
                    cls_csv_ndd.Indexes index = cls_csv_ndd.SetColumnsIndex(columns);
                    var consinco = cls_csv_ndd.BuildConfNDD(reader, index);
                    DeleteNDD(ent_sai);
                    connection.OpenConnection();
                    Frm_ProgressBar f = new Frm_ProgressBar();
                    f.Show();
                    f.totalLines = File.ReadAllLines(nomearquivo).Length;
                    f.progressBar1.Maximum = f.totalLines;
                    int currentLine = 0;

                    foreach (var item in consinco)
                    {
                        string sql = "INSERT INTO db_sis.tb_conf_ndd (COD_CLIENTE, COD_EMPRESA, MES, ANO, ENTRADA_SAIDA,SERIE, NUMERO_DA_NOTA, TIPO,  CHAVE_ACESSO, RAZAO_SOCIAL, DTA_EMISSAO, CNPJ_EMITENTE, CNPJ_DESTINATARIO, CPF_DESTINATARIO, RAZAO_SOCIAL_DESTINATARIO, UF, MODELO, COD_NUMERICO, NATUREZA_OPERACAO, IE_EMITENTE, IE_DESTINATARIO, UF_DESTINATARIO, BASE_ICMS, BASE_ICMS_ST, TOTAL_PRODUTOS, VALOR_ICMS, VALOR_ICMS_ST, VALOR_IPI, VALOR_PIS, VALOR_COFINS, BASE_ISS, CNPJ_TRANSPORTADORA, PLACA_TRANSPORTADORA, TIPO_EMISSAO, STATUS_NFE, VALOR_TOTAL_NFE, ULTIMA_ITERACAO, ULTIMO_EVENTO, DTA_IMPORTACAO)" +
                                         " VALUES (@COD_CLI, @COD_EMP, @MES, @ANO, @ENT_SAI, @SERIE, @NRO_NF, SUBSTRING(@CHAV_ACES1,1,3), SUBSTRING(@CHAV_ACES2,4,50), @RAZAOS, @DTAEMISSAO, @CNPJ_EM, @CNPJ_DE, @CPF_DEST, @RAZAOS_D, @UF, @MODELO, @COD_NUM, @NAT_OP, @IE_EM, @IE_DE, @UF_DE, REPLACE(@BC_ICMS,',','.'),REPLACE(@BC_ICMS_ST,',','.'),REPLACE(@TOT_PROD,',','.'),REPLACE(@VLR_ICMS,',','.'),REPLACE(@VLR_ICMS_ST,',','.'),REPLACE(@VLR_IPI,',','.'),REPLACE(@VLR_PIS,',','.'),REPLACE(@VLR_COFINS,',','.'),REPLACE(@BC_ISS,',','.'),@CNPJ_T, @PL_TRANS, @TP_EMISS, @STATUS_NFE, REPLACE(@VLR_TOT_NFE,',','.'), @ULT_ITER, @ULT_EVE, SYSDATE())";
                        string[] param_name = { "@COD_CLI", "@COD_EMP", "@MES", "@ANO","@ENT_SAI", "@SERIE", "@NRO_NF", "@CHAV_ACES1", "@CHAV_ACES2", "@RAZAOS", "@DTAEMISSAO", "@CNPJ_EM", "@CNPJ_DE", "@CPF_DEST", "@RAZAOS_D", "@UF", "@MODELO", "@COD_NUM", "@NAT_OP", "@IE_EM", "@IE_DE", "@UF_DE", "@BC_ICMS", "@BC_ICMS_ST", "@TOT_PROD", "@VLR_ICMS", "@VLR_ICMS_ST", "@VLR_IPI", "@VLR_PIS", "@VLR_COFINS", "@BC_ISS", "@CNPJ_T", "@PL_TRANS", "@TP_EMISS", "@STATUS_NFE", "@VLR_TOT_NFE", "@ULT_ITER", "@ULT_EVE" };
                        dynamic[] dynamics = { Frm_TaxAudit.instance.cod_cliente.Text, Frm_TaxAudit.instance.cod_emp.Text, Frm_TaxAudit.instance.Mes, Frm_TaxAudit.instance.Ano, ent_sai, item.SERIE, item.NUMERO_DA_NOTA, item.CHAVE_ACESSO, item.CHAVE_ACESSO, item.RAZAO_SOCIAL.Replace("'", "''"), item.DTA_EMISSAO, item.CNPJ_EMITENTE, item.CNPJ_DESTINATARIO, item.CPF_DESTINATARIO, item.RAZAO_SOCIAL_DESTINATARIO.Replace("'", "''"), item.UF, item.MODELO, item.COD_NUMERICO, item.NATUREZA_OPERACAO, item.IE_EMITENTE, item.IE_DESTINATARIO, item.UF_DESTINATARIO, item.BASE_ICMS, item.BASE_ICMS_ST, item.TOTAL_PRODUTOS, item.VALOR_ICMS, item.VALOR_ICMS_ST, item.VALOR_IPI, item.VALOR_PIS, item.VALOR_COFINS, item.BASE_ISS, item.CNPJ_TRANSPORTADORA, item.PLACA_TRANSPORTADORA.Replace("'", "''"), item.TIPO_EMISSAO, item.STATUS_NFE, item.VALOR_TOTAL_NFE, item.ULTIMA_ITERACAO, item.ULTIMO_EVENTO};
                        MySqlParameter[] parameters = new MySqlParameter[38];
                        for (int i = 0; i < param_name.Length; i++)
                        {
                            parameters[i] = new MySqlParameter(param_name[i], dynamics[i]);
                        }

                        MySqlCommand cmd = connection.CreateCommand(sql, parameters);
                        MySqlDataReader read = cmd.ExecuteReader();
                        read.Close();
                         currentLine++;
                        f.progressBar1.Value = currentLine;
                        f.progressBar1.Refresh();
                        Cursor.Current = Cursors.WaitCursor;
                    }
                     f.Close();
                    connection.CloseConnection();
                    BindData();
                    CheckEntSai();
                    MessageBox.Show($"Arquivo Importado com sucesso!\n{Frm_TaxAudit.instance.import_ndd.Text}", "Importado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void CheckEntSai()
        {
            try
            {
                using(DataTable dt = new DataTable())
                {
                    connection.OpenConnection();
                    string sql = "SELECT DISTINCT ENTRADA_SAIDA FROM db_sis.tb_conf_ndd WHERE COD_CLIENTE = @COD_CLI AND COD_EMPRESA = @COD_EMP AND MES = @MES AND ANO = @ANO";
                    MySqlParameter[] parameters = new MySqlParameter[]
                       {
                        new MySqlParameter("@COD_CLI",Frm_TaxAudit.instance.cod_cliente.Text),
                        new MySqlParameter("@COD_EMP",Frm_TaxAudit.instance.cod_emp.Text),
                        new MySqlParameter("@MES",Frm_TaxAudit.instance.Mes),
                        new MySqlParameter("@ANO",Frm_TaxAudit.instance.Ano)
                       };
                    MySqlCommand cmd = connection.CreateCommand(sql, parameters);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }                                                   
                    if (dt.Rows.Count == 1)
                    {
                        if (dt.Rows[0].Field<string>("ENTRADA_SAIDA").Contains("ENTRADA"))
                        {
                            Frm_TaxAudit.instance.import_ndd.Text = "- FALTA SAÍDA";
                        }
                        if (dt.Rows[0].Field<string>("ENTRADA_SAIDA").Contains("SAIDA"))
                        {
                            Frm_TaxAudit.instance.import_ndd.Text = "- FALTA ENTRADA";
                        }
                    }
                    else
                    {
                        Frm_TaxAudit.instance.import_ndd.Text = "Importado";
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

        private void lbl_obs_Click(object sender, EventArgs e)
        {

        }

        private void dgv_conf_ndd_FilterStringChanged(object sender, EventArgs e)
        {
            string filterString = dgv_conf_ndd.FilterString;
            BSource.Filter = filterString;
            dgv_conf_ndd.DataSource = BSource;
        }

        private void dgv_conf_ndd_SortStringChanged(object sender, EventArgs e)
        {
            BSource.Sort = dgv_conf_ndd.SortString;
        }

        private void btn_limpar_Click(object sender, EventArgs e)
        {
            dgv_conf_ndd.ClearFilter();
            BSource.Filter = "";
        }
    }
}
