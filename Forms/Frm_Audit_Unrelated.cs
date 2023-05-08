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
    public partial class Frm_Audit_Unrelated : Form
    {
        cls_mysql_conn connection = new cls_mysql_conn();
       
        public Frm_Audit_Unrelated()
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
                string sql = "select DISTINCT NRO_DOCUMENTO as `Número NF`, SERIE as `Serie`,ESPECIE as `Tipo`, DTA_ENT_SAIDA `Data`, CHAVE_ACESSO `Chave de Acesso`, RAZAO_SOCIAL as `Razão Social`, CGO, CFOP from db_sis.tb_conf_c5 where COD_CLIENTE = @COD_CLI AND COD_EMPRESA = @COD_EMP AND MES = @MES AND ANO = @ANO AND CHAVE_ACESSO not in (select CHAVE_ACESSO from db_sis.tb_conf_ndd where COD_CLIENTE = @COD_CLI AND COD_EMPRESA = @COD_EMP AND MES = @MES AND ANO = @ANO) order by `Data`";
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@COD_CLI", Frm_Conferencia.instance.cod_cliente.Text),
                    new MySqlParameter("@COD_EMP", Frm_Conferencia.instance.cod_emp.Text),
                    new MySqlParameter("@MES", Frm_Conferencia.instance.Mes),
                    new MySqlParameter("@ANO", Frm_Conferencia.instance.Ano )
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
        }
        private void Frm_Audit_Unrelated_Load(object sender, EventArgs e)
        {
            BindData(); 
        }
    }
}
