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
    public partial class Frm_Audit_Values_Detailed : Form
    {
        cls_mysql_conn connection = new cls_mysql_conn();
        public Frm_Audit_Values_Detailed()
        {            
            InitializeComponent();
            TextBox[] TBoxes = { txt_numdoc, txt_especie, txt_data, txt_chaveacesso, txt_serie, txt_razaosocial, txt_UF, txt_pessoa, txt_valor_total_c5, txt_valor_total_ndd, txt_base_icms_c5,  txt_base_icms_ndd, txt_valor_icms_c5, txt_valor_icms_ndd, txt_base_icms_st_c5, txt_base_icms_st_ndd, txt_valor_icms_st_c5, txt_valor_icms_st_ndd, txt_CGO, txt_natureza  };

            for (int i = 0; i < TBoxes.Length; i++)
            {
                TBoxes[i].Enabled = false;
            }
        }

        private void BindData()
        {
            try
            {
                connection.OpenConnection();                
                string sql = "SELECT COD_PRODUTO as `Cod`, DESC_PRODUTO as `Produto`, NCM, VALOR_ITEM as Valor, CFOP, CST_ICMS as CST, COD_TRIBUTACAO as Trib, BASE_ICMS as `Base Icms`, ALIQ_ICMS as `Aliq Icms`, VALOR_ICMS as `Valor Icms`, VALOR_OUTRAS_ICMS as `Outras Icms`, VALOR_ISENTOS_ICMS as `Isentos Icms`, MVA_ICMS_ST as MVA, BASE_ICMS_ST as `Base Icms St`, ALIQ_ICMS_ST as `Aliq Icms St`, VALOR_ICMS_ST as `Valor Icms St`, VALOR_OUTRAS_ICMS_ST as `Outras Icms St`,VALOR_ISENTOS_ICMS_ST as `Isentos Icms St`, CSOSN, BASE_SIMPLES_NAC as `Base Simples`, ALIQ_SIMPLES_NAC as `Aliq Simples`, VALOR_SIMPLES_NAC as `Valor Simples` FROM db_sis.tb_conf_c5 WHERE COD_CLIENTE = @COD_CLI AND COD_EMPRESA = @COD_EMP AND MES = @MES AND ANO = @ANO AND NRO_DOCUMENTO= @NRO_DOC";
                MySqlParameter[] parameters = GetSqlParameters();  
                using (DataTable dt = new DataTable())
                {
                    MySqlCommand cmd = connection.CreateCommand(sql, parameters);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);                       
                        if (dt.Rows.Count > 0)
                        {
                            dgv_itens.DataSource = dt;
                        }                        
                    }                        
                }
                Capture();
                CaptureNatureza();
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
        public void Capture()
        {
            try
            {               
                string sql = "SELECT DISTINCT SERIE, DTA_ENT_SAIDA, CHAVE_ACESSO, PESSOA, RAZAO_SOCIAL, UF, CGO FROM db_sis.tb_conf_c5 WHERE COD_CLIENTE = @COD_CLI AND COD_EMPRESA = @COD_EMP AND MES = @MES AND ANO = @ANO AND NRO_DOCUMENTO = @NRO_DOC";
                MySqlParameter[] parameters = GetSqlParameters();
                TextBox[] textBoxes = { txt_serie, txt_data, txt_UF, txt_CGO, txt_pessoa, txt_razaosocial, txt_chaveacesso };
                string[] columns = { "SERIE","DTA_ENT_SAIDA","UF","CGO","PESSOA","RAZAO_SOCIAL","CHAVE_ACESSO"};
                MySqlCommand cmd = connection.CreateCommand(sql, parameters);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        for (int i = 0; i < textBoxes.Length; i++)
                        {
                            textBoxes[i].Text = reader.GetString(columns[i]);
                        }                       
                    }                    
                }                 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void PrintText()
        {
            void CompareTB(TextBox TB1, TextBox TB2, Color _true, Color _false)
            {
                if (TB1.Text.Replace(".", ",") == TB2.Text.Replace(".", ","))
                {
                    TB1.BackColor = _true;
                    TB2.BackColor = _true;
                }
                else
                {
                    TB1.BackColor = _false;
                    TB2.BackColor = _false;
                }
            }

            CompareTB(txt_valor_total_c5, txt_valor_total_ndd, Color.LightGreen, Color.LightSalmon);
            CompareTB(txt_base_icms_c5, txt_base_icms_ndd, Color.LightGreen, Color.LightSalmon);
            CompareTB(txt_valor_icms_c5, txt_valor_icms_ndd, Color.LightGreen, Color.LightSalmon);
            CompareTB(txt_base_icms_st_c5, txt_base_icms_st_ndd, Color.LightGreen, Color.LightSalmon);
            CompareTB(txt_valor_icms_st_c5, txt_valor_icms_st_ndd, Color.LightGreen, Color.LightSalmon);

            TextBox[] textBoxes = { txt_numdoc, txt_especie, txt_data, txt_chaveacesso, txt_serie, txt_razaosocial, txt_UF, txt_pessoa, txt_CGO, txt_natureza };

            for (int i = 0; i < textBoxes.Length; i++)
            {
                textBoxes[i].BackColor = Color.White;
            }
        }

        public void CaptureNatureza()
        {
            try
            {               
                string sql = "SELECT DISTINCT NATUREZA_OPERACAO FROM db_sis.tb_conf_ndd WHERE COD_CLIENTE = @COD_CLI AND COD_EMPRESA = @COD_EMP AND MES = @MES AND ANO = ANO AND NUMERO_DA_NOTA= @NRO_DOC";
                MySqlParameter[] parameters = GetSqlParameters();
                MySqlCommand cmd = connection.CreateCommand(sql, parameters);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        txt_natureza.Text = reader.GetString("NATUREZA_OPERACAO");
                    }                    
                    PrintText();
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
                new MySqlParameter("@COD_CLI",Frm_Conferencia.instance.cod_cliente.Text),
                new MySqlParameter("@COD_EMP",Frm_Conferencia.instance.cod_emp.Text),
                new MySqlParameter("@MES", Frm_Conferencia.instance.Mes),
                new MySqlParameter("@ANO",Frm_Conferencia.instance.Ano),
                new MySqlParameter("@NRO_DOC",txt_numdoc.Text)
            };
        }

        private void Frm_Audit_Values_Detailed_Load(object sender, EventArgs e)
        {
            BindData();
        }
    }
}
