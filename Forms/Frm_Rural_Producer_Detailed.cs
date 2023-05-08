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
    public partial class Frm_Rural_Producer_Detailed : Form
    {
        public static Frm_Rural_Producer_Detailed instance;
        cls_mysql_conn connection = new cls_mysql_conn();
        cls_populate_views populate = new cls_populate_views();
        public Frm_Rural_Producer_Detailed()
        {
            InitializeComponent();

            string[] headers = {"Pessoa","Razão Social","CFOP","CST","Cod.Produto","Produto","Vlr.Contábil","Base ICMS","Aliq.ICMS","Vlr.ICMS","Outras","Isentos" };
            int[] widths = {50,130,40,40,60,130,80,80,80,80,80,80};
            populate.ConstructListView(lsv_nat_detalhado, headers, widths);            
        }
        private void ListarDetalhado()
        {
            try
            {
                connection.OpenConnection();                               
                string sql = "select a.PESSOA, a.RAZAO_SOCIAL, a.CFOP, a.CST_ICMS, a.COD_PRODUTO, a.DESC_PRODUTO, a.VALOR_CONTABIL, a.BASE_ICMS, a.ALIQ_ICMS, a.VALOR_ICMS, a.VALOR_OUTRAS_ICMS, a.VALOR_ISENTOS_ICMS from db_sis.tb_conf_c5 a inner join db_sis.tb_conf_fornec b on a.PESSOA = b.SEQFORNECEDOR  where a.COD_CLIENTE = @COD_CLI AND a.COD_EMPRESA = @COD_EMP AND a.MES = @MES AND a.ANO = @ANO AND a.CGO = @CFOP AND a.NRO_DOCUMENTO = @NAT AND b.PRODRURAL = 'SIM'";
                int[] columnindexes = {0,1,2,3,4,5,6,7,8,9,10,11};
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@COD_CLI", Frm_Conferencia.instance.cod_cliente.Text),
                    new MySqlParameter("@COD_EMP", Frm_Conferencia.instance.cod_emp.Text),
                    new MySqlParameter("@MES", Frm_Conferencia.instance.Mes),
                    new MySqlParameter("@ANO", Frm_Conferencia.instance.Ano),
                    new MySqlParameter("@CFOP", txt_CFOP.Text),
                    new MySqlParameter("@NAT", txt_Natureza.Text)
                };
                MySqlCommand cmd = connection.CreateCommand(sql, parameters);                
                lsv_nat_detalhado.Items.Clear();
                populate.PopulateListViews(lsv_nat_detalhado, cmd, columnindexes); 
                Captura();
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
        public void Captura()
        {
            try
            {
                string sql = "select a.PESSOA, a.RAZAO_SOCIAL, a.CFOP, a.CST_ICMS, a.COD_PRODUTO, a.DESC_PRODUTO, a.VALOR_CONTABIL, a.BASE_ICMS, a.ALIQ_ICMS, a.VALOR_ICMS, a.VALOR_OUTRAS_ICMS, a.VALOR_ISENTOS_ICMS, a.CHAVE_ACESSO from db_sis.tb_conf_c5 a inner join db_sis.tb_conf_fornec b on a.PESSOA = b.SEQFORNECEDOR  where a.COD_CLIENTE = @COD_CLI AND a.COD_EMPRESA = @COD_EMP AND a.MES = @MES AND a.ANO = @ANO AND a.CGO = @CFOP AND a.NRO_DOCUMENTO = @NAT AND b.PRODRURAL = 'SIM'";
                MySqlParameter[] parameters = new MySqlParameter[]
                  {
                    new MySqlParameter("@COD_CLI", Frm_Conferencia.instance.cod_cliente.Text),
                    new MySqlParameter("@COD_EMP", Frm_Conferencia.instance.cod_emp.Text),
                    new MySqlParameter("@MES", Frm_Conferencia.instance.Mes),
                    new MySqlParameter("@ANO", Frm_Conferencia.instance.Ano),
                    new MySqlParameter("@CFOP", txt_CFOP.Text),
                    new MySqlParameter("@NAT", txt_Natureza.Text)
                  };
                MySqlCommand cmd = connection.CreateCommand(sql,parameters);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        txt_chave_acesso.Text = reader.GetString("CHAVE_ACESSO");
                    }                 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Frm_Rural_Producer_Detailed_Load(object sender, EventArgs e)
        {
            ListarDetalhado();
        }

    }

}
