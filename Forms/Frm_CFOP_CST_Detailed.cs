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
    public partial class Frm_CFOP_CST_Detailed : Form
    {
        public static Frm_CFOP_CST_Detailed instance;
        cls_mysql_conn connection = new cls_mysql_conn();
        cls_populate_views populate = new cls_populate_views();
        public Frm_CFOP_CST_Detailed()
        {            
            InitializeComponent();
            string[] headers = {"Entrada/Saída","Tipo","Documento","Chave de Acesso","Razão Social","Data Emissão"};
            int[] widths = {80,40,80,260,180,80};
            populate.ConstructListView(lsv_nat_detalhado, headers, widths);
        }
        private void ListarDetalhado()
        {            
            try
            {
                connection.OpenConnection();                 
                string sql = "select DISTINCT a.ENTRADA_SAIDA, a.TIPO, a.NUMERO_DA_NOTA, a.CHAVE_ACESSO, a.RAZAO_SOCIAL, a.DTA_EMISSAO from db_sis.tb_conf_ndd a inner join db_sis.tb_conf_c5 b on a.CHAVE_ACESSO = b.CHAVE_ACESSO where a.COD_CLIENTE = @COD_CLI AND a.COD_EMPRESA = @COD_EMP AND a.MES = @MES AND a.ANO = @ANO AND b.CFOP = @CFOP AND b.CST_ICMS = @CST";
                int[] columnIndexes = {0,1,2,3,4,5};
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@COD_CLI", Frm_Conferencia.instance.cod_cliente.Text),
                    new MySqlParameter("@COD_EMP", Frm_Conferencia.instance.cod_emp.Text),
                    new MySqlParameter("@MES", Frm_Conferencia.instance.Mes),
                    new MySqlParameter("@ANO", Frm_Conferencia.instance.Ano),
                    new MySqlParameter("@CFOP", txt_CFOP.Text),
                    new MySqlParameter("@CST", txt_CST.Text)
                };
                MySqlCommand cmd = connection.CreateCommand(sql, parameters);              
                lsv_nat_detalhado.Items.Clear();
                populate.PopulateListViews(lsv_nat_detalhado, cmd, columnIndexes);
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

        private void Frm_CFOP_CST_Detailed_Load(object sender, EventArgs e)
        {
            try
            {
                ListarDetalhado();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }       

        public void CopyListBox(ListView list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in list.SelectedItems)
            {
                ListViewItem l = item as ListViewItem;
                if (l != null)
                    foreach (ListViewItem.ListViewSubItem sub in l.SubItems)
                        sb.Append(sub.Text + "\t");
                sb.AppendLine();
            }
            Clipboard.SetDataObject(sb.ToString());

        }

        private void lsv_nat_detalhado_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                CopyListBox(lsv_nat_detalhado);
            }
        }
    }
}
