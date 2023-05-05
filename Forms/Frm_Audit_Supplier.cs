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

namespace SistemaEtccom
{
    public partial class Frm_Audit_Supplier : Form
    {
        public static Frm_Audit_Supplier instance;
        cls_mysql_conn connection = new cls_mysql_conn();
        private BindingSource BSource = new BindingSource();
        public Frm_Audit_Supplier()
        {
            InitializeComponent();
            lbl_resumo.Text = Frm_TaxAudit.instance.EMP.ToString() + " | CNPJ: " + Frm_TaxAudit.instance.CNPJ.ToString() + " | " + Frm_TaxAudit.instance.Mes.ToString() + "/" + Frm_TaxAudit.instance.Ano.ToString();
        }

        private void btn_import_Click(object sender, EventArgs e)
        {
            OpenFileDialog arquivo = new OpenFileDialog();
            arquivo.InitialDirectory = "C:\\";
            arquivo.Filter = "txt|*.txt|csv|*.csv";
            arquivo.Title = "Selecione o arquivo .txt ou .csv";

            if (arquivo.ShowDialog() == DialogResult.OK)
            {
                string nomearquivo = arquivo.FileName;
                try
                {
                    var reader = new StreamReader(File.OpenRead(nomearquivo));
                    var line = reader.ReadLine();
                    var columns = line.Split(';');
                    cls_csv_fornec.Indexes index = cls_csv_fornec.SetColumnsIndex(columns);
                    var consinco = cls_csv_fornec.BuildConfC5(reader, index);
                    Delete();
                    connection.OpenConnection(); 
                    Frm_ProgressBar f = new Frm_ProgressBar();
                    f.Show();
                    f.totalLines = File.ReadAllLines(nomearquivo).Length;
                    f.progressBar1.Maximum = f.totalLines;
                    int currentLine = 0;                   

                    foreach (var item in consinco)
                    {
                        string sql = "INSERT INTO db_sis.tb_conf_fornec (COD_CLIENTE, SEQFORNECEDOR, NOMERAZAO, CNPJ, UF,TIPFORNEC, NROREGTRIB, MICROEMPRESA, PRODRURAL,DTA_IMPORTACAO)" +
                                        " VALUES (@COD_CLI, @SEQ_FORNEC, @RAZAO, @CNPJ, @UF, @TP_FORNEC, @NRO_REG, @MICRO_EMP, @PROD_RURAL, SYSDATE())";
                        string[] param_name = {"@COD_CLI","@SEQ_FORNEC","@RAZAO", "@CNPJ", "@UF", "@TP_FORNEC", "@NRO_REG", "@MICRO_EMP", "@PROD_RURAL" };
                        string[] dynamics = { Frm_TaxAudit.instance.cod_cliente.Text, item.SEQ_FORNECEDOR, item.NOME_RAZAO.Replace("'", "''"), item.CNPJ, item.UF, item.TIPOFORNEC, item.NRO_REGTRIB, item.MICROEMPRESA, item.PROD_RURAL };
                        MySqlParameter[] parameters = new MySqlParameter[9];
                        for (int i = 0; i < param_name.Length; i++)
                        {
                            parameters[i] = new MySqlParameter(param_name[i], dynamics[i]);
                        }

                        MySqlCommand cmd = connection.CreateCommand(sql,parameters);
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
                    Frm_TaxAudit.instance.import_fornec.Text = "Importado";
                    MessageBox.Show("Arquivo Importado com sucesso!", "Importado", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void Frm_Audit_Supplier_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }

        private void Frm_Audit_Supplier_Load(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindData()
        {
            try
            {
                connection.OpenConnection();
                dgv_conf_supplier.DataSource = BSource;
                using (DataTable dt = new DataTable())
                {
                    string sql = "SELECT SEQFORNECEDOR, NOMERAZAO, CNPJ, UF, TIPFORNEC, NROREGTRIB, MICROEMPRESA, PRODRURAL, DTA_IMPORTACAO FROM db_sis.tb_conf_fornec WHERE COD_CLIENTE =" + Frm_TaxAudit.instance.cod_cliente.Text;
                    MySqlCommand cmd = new MySqlCommand(sql, connection.conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                        if (dt.Rows.Count > 0)
                        {
                            BSource.DataSource = dt;
                        }                        
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

        public void Delete()
        {
            try
            {
                connection.OpenConnection();
                string sql = "DELETE FROM db_sis.tb_conf_fornec WHERE COD_CLIENTE =" + Frm_TaxAudit.instance.cod_cliente.Text;
                MySqlCommand cmd = new MySqlCommand(sql, connection.conn);
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

        public void CapturaUltImport()
        {
            if (dgv_conf_supplier.Rows.Count > 0)
            {
                try
                {                                     
                    string sql = "SELECT DISTINCT MAX(DTA_IMPORTACAO) FROM db_sis.tb_conf_fornec WHERE COD_CLIENTE =" + Frm_TaxAudit.instance.cod_cliente.Text;                   
                    MySqlCommand cmd = new MySqlCommand(sql, connection.conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            txt_ultimport.Text = reader.GetString("MAX(DTA_IMPORTACAO)");
                        }                        
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }

        }

        private void lbl_resumo_Click(object sender, EventArgs e)
        {

        }      

        private void dgv_conf_supplier_FilterStringChanged(object sender, EventArgs e)
        {
            string filterString = dgv_conf_supplier.FilterString;
            BSource.Filter = filterString;
            dgv_conf_supplier.DataSource = BSource;
        }

        private void dgv_conf_supplier_SortStringChanged(object sender, EventArgs e)
        {
            BSource.Sort = dgv_conf_supplier.SortString;
        }

        private void btn_limpar_Click(object sender, EventArgs e)
        {
            dgv_conf_supplier.ClearFilter();
            BSource.Filter = "";
        }
    }
}
