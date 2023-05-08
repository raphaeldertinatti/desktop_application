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
    public partial class Frm_Transferences : Form
    {
        public static Frm_Natureza_Operacao instance;
        cls_mysql_conn connection = new cls_mysql_conn();
        public Frm_Transferences()
        {
            InitializeComponent();           
            lbl_resumo.Text = Frm_Conferencia.instance.EMP.ToString() + " | CNPJ: " + Frm_Conferencia.instance.CNPJ.ToString() + " | " + Frm_Conferencia.instance.Mes.ToString() + "/" + Frm_Conferencia.instance.Ano.ToString();
        }

        private void ListaCFOPTransf()
        {
            try
            {
                if (!(txt_pessoas.Text == null))
                {
                    connection.OpenConnection();                   
                    using (DataTable dt = new DataTable())
                    {
                        string sql = "select CFOP, PESSOA, round(sum(VALOR_CONTABIL),2) as `Valor Contábil`, round(sum(BASE_ICMS),2) as `Base ICMS`, round(sum(VALOR_ICMS),2) as `Valor ICMS`, round(sum(VALOR_OUTRAS_ICMS),2) as `Outras ICMS`, round(sum(VALOR_ISENTOS_ICMS),2) as `Isentos` from db_sis.tb_conf_c5 where COD_CLIENTE = @COD_CLI AND COD_EMPRESA = @COD_EMP AND MES = @MES AND ANO = @ANO AND PESSOA in (" + txt_pessoas.Text + ") group by CFOP,PESSOA order by CFOP,PESSOA";
                        MySqlParameter[] parameters = new MySqlParameter[]
                         {
                            new MySqlParameter("@COD_CLI",Frm_Conferencia.instance.cod_cliente.Text),
                            new MySqlParameter("@COD_EMP",Frm_Conferencia.instance.cod_emp.Text),
                            new MySqlParameter("@MES",Frm_Conferencia.instance.Mes),
                            new MySqlParameter("@ANO",Frm_Conferencia.instance.Ano)
                         };
                        MySqlCommand cmd = connection.CreateCommand(sql,parameters);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            dt.Load(reader);
                            if (dt.Rows.Count > 0)
                            {
                                dgv_CFOP_Transf.DataSource = dt;
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

        private void Frm_Transferences_Load(object sender, EventArgs e)
        {

        }

        private void btn_buscar_Click(object sender, EventArgs e)
        {
            if (!(txt_pessoas.Text == ""))
            {
                ListaCFOPTransf();
            }                   
           
        }

        private void dgv_CFOP_Transf_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
          
        }

        private void dgv_CFOP_Transf_DoubleClick(object sender, EventArgs e)
        {
            Frm_Transf_Detalhado f = new Frm_Transf_Detalhado();
            try
            {
                f.txt_CFOP.Text = this.dgv_CFOP_Transf.CurrentRow.Cells[0].Value.ToString();
                f.txt_Pessoa.Text = this.dgv_CFOP_Transf.CurrentRow.Cells[1].Value.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            f.ShowDialog();
        }
    }
}
