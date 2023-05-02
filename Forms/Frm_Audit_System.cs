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
using System.Globalization;

namespace DesktopApplication
{
    public partial class Frm_Audit_System : Form
    {
        public static Frm_Audit_System instance;
        cls_mysql_conn connection = new cls_mysql_conn();
        private BindingSource BSource = new BindingSource();

        public Frm_Audit_System()
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
                    cls_csv_c5.Indexes index = cls_csv_c5.SetColumnsIndex(columns);
                    var consinco = cls_csv_c5.BuildConfC5(reader, index);                    
                    Delete();
                    connection.OpenConnection();

                    foreach (var item in consinco)
                    {
                        string sql = "INSERT INTO db_sis.tb_conf_c5 (COD_CLIENTE, COD_EMPRESA, MES, ANO, DTA_ENT_SAIDA,ESPECIE, NRO_DOCUMENTO, SERIE, CHAVE_ACESSO, PESSOA, RAZAO_SOCIAL, UF, CGO, CFOP, COD_TRIBUTACAO, NCM, COD_PRODUTO, DESC_PRODUTO, QUANTIDADE, VALOR_ITEM, VALOR_CONTABIL, CST_ICMS, BASE_ICMS, ALIQ_ICMS, VALOR_ICMS, VALOR_OUTRAS_ICMS, VALOR_ISENTOS_ICMS, MVA_ICMS_ST, BASE_ICMS_ST, ALIQ_ICMS_ST,VALOR_ICMS_ST,VALOR_OUTRAS_ICMS_ST,VALOR_ISENTOS_ICMS_ST,CST_IPI,BASE_IPI,ALIQ_IPI,VALOR_IPI,VALOR_OUTRAS_IPI,VALOR_ISENTOS_IPI,CST_PIS,BASE_PIS,ALIQ_PIS,VALOR_PIS,CST_COFINS,BASE_COFINS,ALIQ_COFINS,VALOR_COFINS,CSOSN,BASE_SIMPLES_NAC,ALIQ_SIMPLES_NAC,VALOR_SIMPLES_NAC,BASE_FCP_ICMS,VALOR_FCP_ICMS,BASE_FCP_ICMS_ST,VALOR_FCP_ICMS_ST,DTA_IMPORTACAO)" +
                                        " VALUES (@COD_CLI, @COD_EMP, @MES, @ANO, @DTA_ES, @ESP, @NRO_DOC, @SERIE, @CHAV, @PESSOA, @RAZAOS, @UF, @CGO, @CFOP, @COD_TRIB, @NCM, @COD_PROD, @DESC_PROD, @QTD, REPLACE(@VLR_IT,',','.'),REPLACE(@VLR_CT,',','.'), @CST_ICMS, REPLACE(@BC_ICMS,',','.'),REPLACE(@ALIQ_ICMS,',','.'),REPLACE(@VLR_ICMS,',','.'),REPLACE(@VLR_O_ICMS,',','.'),REPLACE(@VLR_I_ICMS,',','.'),REPLACE(@MVA_ST,',','.'),REPLACE(@BC_ICMS_ST,',','.'),REPLACE(@ALIQ_ICMS_ST,',','.'),REPLACE(@VLR_ICMS_ST,',','.'),REPLACE(@VLR_O_ICMS_ST,',','.'),REPLACE(@VLR_I_ICMS_ST,',','.'),@CST_IPI,REPLACE(@BC_IPI,',','.'),REPLACE(@ALIQ_IPI,',','.'),REPLACE(@VLR_IPI,',','.'),REPLACE(@VLR_O_IPI,',','.'),REPLACE(@VLR_I_IPI,',','.'), @CST_PIS, REPLACE(@BC_PIS,',','.'),REPLACE(@ALIQ_PIS,',','.'),REPLACE(@VLR_PIS,',','.'),@CST_COFINS, REPLACE(@BC_COFINS,',','.'),REPLACE(@ALIQ_COFINS,',','.'),REPLACE(@VLR_COFINS,',','.'),@CSOSN, REPLACE(@BC_SN,',','.'),REPLACE(@ALIQ_SN,',','.'),REPLACE(@VLR_SN,',','.'),REPLACE(@BC_FCP,',','.'),REPLACE(@VLR_FCP,',','.'),REPLACE(@BC_FCP_ST,',','.'),REPLACE(@VLR_FCP_ST,',','.'), SYSDATE())";
                        string[] param_name = {"@COD_CLI","@COD_EMP","@MES","@ANO","@DTA_ES","@ESP","@NRO_DOC","@SERIE","@CHAV","@PESSOA","@RAZAOS","@UF","@CGO","@CFOP","@COD_TRIB","@NCM","@COD_PROD","@DESC_PROD","@QTD","@VLR_IT","@VLR_CT","@CST_ICMS","@BC_ICMS","@ALIQ_ICMS","@VLR_ICMS","@VLR_O_ICMS", "@VLR_I_ICMS","@MVA_ST","@BC_ICMS_ST","@ALIQ_ICMS_ST","@VLR_ICMS_ST","@VLR_O_ICMS_ST","@VLR_I_ICMS_ST","@CST_IPI","@BC_IPI","@ALIQ_IPI","@VLR_IPI","@VLR_O_IPI","@VLR_I_IPI","@CST_PIS","@BC_PIS","@ALIQ_PIS","@VLR_PIS","@CST_COFINS","@BC_COFINS","@ALIQ_COFINS","@VLR_COFINS", "@CSOSN","@BC_SN","@ALIQ_SN","@VLR_SN","@BC_FCP","@VLR_FCP","@BC_FCP_ST","@VLR_FCP_ST"};
                        dynamic[] dynamics = { Frm_TaxAudit.instance.cod_cliente.Text, Frm_TaxAudit.instance.cod_emp.Text, Frm_TaxAudit.instance.Mes, Frm_TaxAudit.instance.Ano, item.DTA_ENT_SAIDA, item.ESPECIE, item.NRO_DOCUMENTO, item.SERIE, item.CHAVE_ACESSO, item.PESSOA.Replace("'", "''"), item.RAZAO_SOCIAL.Replace("'", "''"), item.UF, item.CGO, item.CFOP, item.COD_TRIBUTACAO, item.NCM, item.COD_PRODUTO, item.DESC_PRODUTO.Replace("'", "''"), item.QUANTIDADE, item.VALOR_ITEM, item.VALOR_CONTABIL, item.CST_ICMS, item.BASE_ICMS, item.ALIQ_ICMS, item.VALOR_ICMS, item.VALOR_OUTRAS_ICMS, item.VALOR_ISENTOS_ICMS, item.MVA_ICMS_ST, item.BASE_ICMS_ST, item.ALIQ_ICMS_ST, item.VALOR_ICMS_ST, item.VALOR_OUTRAS_ICMS_ST, item.VALOR_ISENTOS_ICMS_ST, item.CST_IPI, item.BASE_IPI, item.ALIQ_IPI, item.VALOR_IPI, item.VALOR_OUTRAS_IPI, item.VALOR_ISENTOS_IPI, item.CST_PIS, item.BASE_PIS, item.ALIQ_PIS, item.VALOR_PIS, item.CST_COFINS, item.BASE_COFINS, item.ALIQ_COFINS, item.VALOR_COFINS, item.CSOSN, item.BASE_SIMPLES_NAC, item.ALIQ_SIMPLES_NAC, item.VALOR_SIMPLES_NAC, item.BASE_FCP_ICMS, item.VALOR_FCP_ICMS, item.BASE_FCP_ICMS_ST, item.VALOR_FCP_ICMS_ST };
                        MySqlParameter[] parameters = new MySqlParameter[55];
                        for (int i = 0; i < param_name.Length; i++)
                        {
                            parameters[i] = new MySqlParameter(param_name[i], dynamics[i]);
                        }

                        MySqlCommand cmd = connection.CreateCommand(sql, parameters);
                        MySqlDataReader read = cmd.ExecuteReader();
                        read.Close();
                        Cursor.Current = Cursors.WaitCursor;
                    }

                    connection.CloseConnection();
                    BindData();
                    Frm_TaxAudit.instance.import_c5.Text = "Importado";
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

        private void Frm_Audit_System_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }
        
        private void Frm_Audit_System_Load(object sender, EventArgs e)
        {
            BindData();
        }
        private void BindData()
        {
            try
            {
                connection.OpenConnection();
                dgv_conf_system.DataSource = BSource;
                using (DataTable dt = new DataTable())
                {
                    string sql = "SELECT DTA_ENT_SAIDA,ESPECIE, NRO_DOCUMENTO, SERIE, CHAVE_ACESSO, PESSOA, RAZAO_SOCIAL, UF, CGO, CFOP, COD_TRIBUTACAO, NCM, COD_PRODUTO, DESC_PRODUTO, QUANTIDADE, VALOR_ITEM, VALOR_CONTABIL, CST_ICMS, BASE_ICMS, ALIQ_ICMS, VALOR_ICMS, VALOR_OUTRAS_ICMS, VALOR_ISENTOS_ICMS, MVA_ICMS_ST, BASE_ICMS_ST, ALIQ_ICMS_ST,VALOR_ICMS_ST,VALOR_OUTRAS_ICMS_ST,VALOR_ISENTOS_ICMS_ST,CST_IPI,BASE_IPI,ALIQ_IPI,VALOR_IPI,VALOR_OUTRAS_IPI,VALOR_ISENTOS_IPI,CST_PIS,BASE_PIS,ALIQ_PIS,VALOR_PIS,CST_COFINS,BASE_COFINS,ALIQ_COFINS,VALOR_COFINS,CSOSN,BASE_SIMPLES_NAC,ALIQ_SIMPLES_NAC,VALOR_SIMPLES_NAC,BASE_FCP_ICMS,VALOR_FCP_ICMS,BASE_FCP_ICMS_ST,VALOR_FCP_ICMS_ST, DTA_IMPORTACAO FROM db_sis.tb_conf_c5 WHERE COD_CLIENTE = @COD_CLI AND COD_EMPRESA = @COD_EMP AND MES = @MES AND ANO = @ANO";

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
                string sql = "DELETE FROM db_sis.tb_conf_c5 WHERE COD_CLIENTE = @COD_CLI AND COD_EMPRESA = @COD_EMP AND MES = @MES AND ANO = @ANO";
                MySqlParameter[] parameters = new MySqlParameter[]
                    {
                    new MySqlParameter("@COD_CLI",Frm_TaxAudit.instance.cod_cliente.Text),
                    new MySqlParameter("@COD_EMP",Frm_TaxAudit.instance.cod_emp.Text),
                    new MySqlParameter("@MES",Frm_TaxAudit.instance.Mes),
                    new MySqlParameter("@ANO",Frm_TaxAudit.instance.Ano)
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
            if (dgv_conf_system.RowCount > 0)
            {
                try
                {
                    string sql = "SELECT DISTINCT MAX(DTA_IMPORTACAO) FROM db_sis.tb_conf_c5 WHERE COD_CLIENTE = @COD_CLI AND COD_EMPRESA = @COD_EMP AND MES = @MES AND ANO = @ANO";
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
        private void dgv_conf_system_FilterStringChanged(object sender, EventArgs e)
        {
            string filterString = dgv_conf_system.FilterString;
            BSource.Filter = filterString;
            dgv_conf_system.DataSource = BSource;           
        }

        private void dgv_conf_system_SortStringChanged(object sender, EventArgs e)
        {
            BSource.Sort = dgv_conf_system.SortString;
        }

        private void btn_limpar_Click(object sender, EventArgs e)
        {
           dgv_conf_system.ClearFilter();
           BSource.Filter = "";  
        }
    }    
}
