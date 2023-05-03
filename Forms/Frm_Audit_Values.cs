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
    public partial class Frm_Audit_Values : Form
    {
        public static Frm_Audit_Values instance;
        cls_mysql_conn connection = new cls_mysql_conn();
        int offset = 0;
        int totPG = 0;
        int inPG = 1;

        public Frm_Audit_Values()
        {   
            InitializeComponent();          
            lbl_resumo.Text = Frm_Conferencia.instance.EMP.ToString() + " | CNPJ: " + Frm_Conferencia.instance.CNPJ.ToString() + " | " + Frm_Conferencia.instance.Mes.ToString() + "/" + Frm_Conferencia.instance.Ano.ToString();           
        }

        private void BindData(int OFFSET)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                connection.OpenConnection();
                using (DataTable dt = new DataTable())
                {
                    string sql = "call db_sis.sp_Conf_Values (@COD_CLI, @COD_EMP, @MES, @ANO, @OFFS)";
                    MySqlParameter[] parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@COD_CLI",Frm_Conferencia.instance.cod_cliente.Text),
                        new MySqlParameter("@COD_EMP",Frm_Conferencia.instance.cod_emp.Text),
                        new MySqlParameter("@MES", Frm_Conferencia.instance.Mes),
                        new MySqlParameter("@ANO", Frm_Conferencia.instance.Ano),
                        new MySqlParameter("@OFFS", OFFSET)
                    };

                    MySqlCommand cmd = connection.CreateCommand(sql, parameters);                    
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }                    

                    if (dt.Rows.Count > 0)
                    {                       
                        dgv_conf_values.DataSource = dt;
                        int[] LightGray = { 0, 1, 2 };
                        int[] Bisque = { 8, 9, 10, 11, 12 };
                        foreach (int index in LightGray)
                        {
                            dgv_conf_values.Columns[index].DefaultCellStyle.BackColor = Color.LightGray;
                        }
                        foreach (int index in Bisque)
                        {
                            dgv_conf_values.Columns[index].DefaultCellStyle.BackColor = Color.Bisque;
                        }
                        var chk = new DataGridViewCheckBoxColumn
                        { 
                        ValueType = typeof(bool),
                        Name = "Chk",
                        HeaderText = "CONFERIDO"
                        };   
                        dgv_conf_values.Columns.Add(chk);                        
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
        private void countRows()
        {
            try
            {
                connection.OpenConnection();
                using (DataTable dt = new DataTable())
                {
                    string sql = "call db_sis.sp_Conf_Values_COUNT(@COD_CLI, @COD_EMP, @MES, @ANO)";
                    MySqlParameter[] parameters = GetMySqlParameters();
                    MySqlCommand cmd = connection.CreateCommand(sql, parameters);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                    if (dt.Rows.Count > 0)
                    {
                        totPG = ((dt.Rows.Count) / 26) +1;
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

        private void Frm_Audit_Values_Load(object sender, EventArgs e)
        {            
            BindData(0);
            CaptureFleg();
            countRows();
            lbl_pgnum.Text = $"{inPG} / {totPG}";
        }

        private void Frm_Audit_Values_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void dgv_conf_values_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {                
                foreach (DataGridViewRow row in dgv_conf_values.Rows)
                {
                    decimal valorTotal = Convert.ToDecimal(row.Cells["VALOR TOTAL"].Value);
                    decimal baseICMS = Convert.ToDecimal(row.Cells["BASE ICMS"].Value);
                    decimal valorICMS = Convert.ToDecimal(row.Cells["VALOR ICMS"].Value);
                    decimal baseICMSST = Convert.ToDecimal(row.Cells["BASE ICMS/ST"].Value);
                    decimal valorICMSST = Convert.ToDecimal(row.Cells["VALOR ICMS/ST"].Value); 

                    decimal valorTotalNDD = Convert.ToDecimal(row.Cells["VALOR TOTAL NDD"].Value.ToString().Replace('.', ','));
                    decimal baseICMSNDD = Convert.ToDecimal(row.Cells["BASE ICMS NDD"].Value.ToString().Replace('.', ','));
                    decimal valorICMSNDD = Convert.ToDecimal(row.Cells["VALOR ICMS NDD"].Value.ToString().Replace('.', ','));
                    decimal baseICMSSTNDD = Convert.ToDecimal(row.Cells["BASE ICMS/ST NDD"].Value.ToString().Replace('.', ','));
                    decimal valorICMSSTNDD = Convert.ToDecimal(row.Cells["VALOR ICMS/ST NDD"].Value.ToString().Replace('.', ','));                    
                    
                    row.Cells["VALOR TOTAL"].Style.ForeColor = (valorTotal == valorTotalNDD) ? Color.Black : Color.Red;
                    row.Cells["BASE ICMS"].Style.ForeColor = (baseICMS == baseICMSNDD) ? Color.Black : Color.Red;
                    row.Cells["VALOR ICMS"].Style.ForeColor = (valorICMS == valorICMSNDD) ? Color.Black : Color.Red;
                    row.Cells["BASE ICMS/ST"].Style.ForeColor = (baseICMSST == baseICMSSTNDD) ? Color.Black : Color.Red;
                    row.Cells["VALOR ICMS/ST"].Style.ForeColor = (valorICMSST == valorICMSSTNDD) ? Color.Black : Color.Red;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }          
        }

        private void dgv_conf_values_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Frm_Audit_Values_Detailed f = new Frm_Audit_Values_Detailed();
            try
            {                
                f.txt_numdoc.Text = this.dgv_conf_values.CurrentRow.Cells[1].Value.ToString();
                f.txt_especie.Text = this.dgv_conf_values.CurrentRow.Cells[0].Value.ToString();
                f.txt_valor_total_c5.Text = this.dgv_conf_values.CurrentRow.Cells[3].Value.ToString();
                f.txt_valor_total_ndd.Text = this.dgv_conf_values.CurrentRow.Cells[8].Value.ToString();
                f.txt_base_icms_c5.Text = this.dgv_conf_values.CurrentRow.Cells[4].Value.ToString();
                f.txt_base_icms_ndd.Text = this.dgv_conf_values.CurrentRow.Cells[9].Value.ToString();
                f.txt_valor_icms_c5.Text = this.dgv_conf_values.CurrentRow.Cells[5].Value.ToString();
                f.txt_valor_icms_ndd.Text = this.dgv_conf_values.CurrentRow.Cells[10].Value.ToString();
                f.txt_base_icms_st_c5.Text = this.dgv_conf_values.CurrentRow.Cells[6].Value.ToString();
                f.txt_base_icms_st_ndd.Text = this.dgv_conf_values.CurrentRow.Cells[11].Value.ToString();
                f.txt_valor_icms_st_c5.Text = this.dgv_conf_values.CurrentRow.Cells[7].Value.ToString();
                f.txt_valor_icms_st_ndd.Text = this.dgv_conf_values.CurrentRow.Cells[12].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            f.ShowDialog();
        }

        private void dgv_conf_values_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (!connection.IsConnectionOpen())
                {
                    connection.OpenConnection(); ;
                }                
                if (dgv_conf_values.CurrentCell.Value.ToString() == "True")
                {                    
                    string sql = "INSERT INTO db_sis.tb_conferidos (COD_CLIENTE, COD_EMPRESA, MES, ANO, CHAVE_ACESSO, MODULO) VALUES (@COD_CLI, @COD_EMP, @MES, @ANO," + this.dgv_conf_values.CurrentRow.Cells[2].Value.ToString() + ", 'VALORES')";
                    MySqlParameter[] parameters = GetMySqlParameters();
                    MySqlCommand cmd = connection.CreateCommand(sql, parameters);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    reader.Close();
                    dgv_conf_values.CurrentRow.DefaultCellStyle.BackColor = Color.LightGreen;
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
     
        private void CaptureFleg()
        {
            try
            {
                connection.OpenConnection();                
                using (DataTable dt = new DataTable())
                {
                    string sql = "select COD_CLIENTE, COD_EMPRESA, MES, ANO, CHAVE_ACESSO from db_sis.tb_conferidos where COD_CLIENTE = @COD_CLI AND COD_EMPRESA = @COD_EMP AND MES = @MES AND ANO = @ANO AND MODULO = 'VALORES'";
                    MySqlParameter[] parameters = GetMySqlParameters();
                    MySqlCommand cmd = connection.CreateCommand(sql, parameters);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataGridViewRow linha in dgv_conf_values.Rows)
                            {
                                if (linha.Cells[2].Value != null)
                                {
                                    if (dt.AsEnumerable().Any(row => linha.Cells["CHAVE ACESSO"].Value.ToString() == row.Field<string>("CHAVE_ACESSO")))
                                    {
                                        linha.Cells[13].Value = true;
                                        linha.DefaultCellStyle.BackColor = Color.LightGreen;
                                    }
                                }
                            }
                        }
                    }
                    Cursor.Current = Cursors.Default;
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

        private MySqlParameter[] GetMySqlParameters()
        {
            return new MySqlParameter[]
            {
                new MySqlParameter("@COD_CLI",Frm_Conferencia.instance.cod_cliente.Text),
                new MySqlParameter("@COD_EMP",Frm_Conferencia.instance.cod_emp.Text),
                new MySqlParameter("@MES", Frm_Conferencia.instance.Mes),
                new MySqlParameter("@ANO", Frm_Conferencia.instance.Ano)
            };
        }

        private void btn_Next_Click(object sender, EventArgs e)
        {            
            if (inPG >= totPG)
            {
                return;
            }
            inPG += 1;
            offset += 26;
            dgv_conf_values.Columns.Clear();
            BindData(offset);
            CaptureFleg();
            lbl_pgnum.Text = $"{inPG} / {totPG}";
        }
        private void btn_previous_Click(object sender, EventArgs e)
        {            
            if (inPG <= 1)
            {
                return;
            }
            inPG -= 1;
            offset -= 26;
            dgv_conf_values.Columns.Clear();
            BindData(offset);
            CaptureFleg();
            lbl_pgnum.Text = $"{inPG} / {totPG}";
        }
    }
}
