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
    public partial class Frm_Audit_Cancelled : Form    
    {
        cls_mysql_conn connection = new cls_mysql_conn();
        public Frm_Audit_Cancelled()
        {           
            InitializeComponent();               
            lbl_resumo.Text = Frm_Conferencia.instance.EMP.ToString() + " | CNPJ: " + Frm_Conferencia.instance.CNPJ.ToString() + " | " + Frm_Conferencia.instance.Mes.ToString() + "/" + Frm_Conferencia.instance.Ano.ToString();                       
        }
        private void BindData()
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                connection.OpenConnection();                
                string sql = "select DISTINCT a.TIPO as Tipo, a.NUMERO_DA_NOTA as `Número NF`, a.SERIE as `Série`, a.ENTRADA_SAIDA as `Entrada/Saída`, a.NATUREZA_OPERACAO as `Natureza da Operação`, a.CHAVE_ACESSO as `Chave de Acesso`, a.RAZAO_SOCIAL as `Razão Social`, a.STATUS_NFE as `Status`, a.DTA_EMISSAO as `Dta Emissão`, a.ULTIMA_ITERACAO as `Última Iteração` from db_sis.tb_conf_ndd a inner join db_sis.tb_conf_c5 b on a.CHAVE_ACESSO = b.CHAVE_ACESSO where a.STATUS_NFE like '%Cancelad%' and a.COD_CLIENTE = @COD_CLI AND a.COD_EMPRESA = @COD_EMP AND a.MES = @MES AND a.ANO = @ANO order by a.NUMERO_DA_NOTA";
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@COD_CLI",Frm_Conferencia.instance.cod_cliente.Text),
                    new MySqlParameter("@COD_EMP",Frm_Conferencia.instance.cod_emp.Text),
                    new MySqlParameter("@MES",Frm_Conferencia.instance.Mes),
                    new MySqlParameter("@ANO",Frm_Conferencia.instance.Ano)
                };
                using (DataTable dt = new DataTable())
                {
                    MySqlCommand cmd = connection.CreateCommand(sql, parameters);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                        if (dt.Rows.Count > 0)
                        {
                            dgv_conf_valores.DataSource = dt;                           
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
        private void Frm_Audit_Cancelled_Load(object sender, EventArgs e)
        {            
            BindData();
        }
    }
    
}
