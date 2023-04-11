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
    public partial class Frm_Companies : Form
    {
        public static Frm_Companies instance;
        public TextBox cod;
        public TextBox cod_customer;
        public TextBox name_customer;
        public string state;
        public string tipo;
        cls_mysql_conn connection = new cls_mysql_conn();
        cls_populate_views populate = new cls_populate_views();

        public Frm_Companies()
        {
            InitializeComponent();
            instance = this;
            cod = txt_codcompany;
            cod_customer = txt_codcustomer;
            name_customer = txt_Cliente;

            cbb_matriz.Items.Clear();
            cbb_matriz.Items.Add("Matriz");
            cbb_matriz.Items.Add("Filial");

            cbb_state.Items.Clear();
            cbb_state.Items.Add("AC");
            cbb_state.Items.Add("AL");
            cbb_state.Items.Add("AP");
            cbb_state.Items.Add("AM");
            cbb_state.Items.Add("BA");
            cbb_state.Items.Add("CE");
            cbb_state.Items.Add("DF");
            cbb_state.Items.Add("ES");
            cbb_state.Items.Add("GO");
            cbb_state.Items.Add("MA");
            cbb_state.Items.Add("MT");
            cbb_state.Items.Add("MS");
            cbb_state.Items.Add("MG");
            cbb_state.Items.Add("PA");
            cbb_state.Items.Add("PB");
            cbb_state.Items.Add("PR");
            cbb_state.Items.Add("PE");
            cbb_state.Items.Add("PI");
            cbb_state.Items.Add("RJ");
            cbb_state.Items.Add("RN");
            cbb_state.Items.Add("RS");
            cbb_state.Items.Add("RO");
            cbb_state.Items.Add("RR");
            cbb_state.Items.Add("SC");
            cbb_state.Items.Add("SP");
            cbb_state.Items.Add("SE");
            cbb_state.Items.Add("TO");            
        }

        private void tsb_search_Click_1(object sender, EventArgs e)
        {
            Frm_CompaniesList List = new Frm_CompaniesList();
            List.ShowDialog();
        }
        public void Capture()
        {
            if (!(txt_codcompany.Text == ""))
            {
                try
                {
                    connection.OpenConnection();
                    string sql = "SELECT a.COD_CUSTOMER,a.RAZAO_SOCIAL,a.NOME_FANTASIA,a.CNPJ,a.INSCRICAO_ESTADUAL,a.ADDRESS,a.CITY,a.`STATUS`,a.UF,a.TIPO_CNPJ, b.CUSTOMER_NAME FROM db_sis.tb_companies a inner join db_sis.tb_customers b on a.cod_customer = b.cod_customer WHERE COD_EMPRESA=" + int.Parse(txt_codcompany.Text);                   
                    MySqlCommand cmd = new MySqlCommand(sql, connection.conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            txt_codcustomer.Text = reader.GetString("COD_CUSTOMER");
                            txt_customer.Text = reader.GetString("CUSTOMER_NAME");
                            txt_nomefantasia.Text = reader.GetString("NOME_FANTASIA");
                            txt_razaoSocial.Text = reader.GetString("RAZAO_SOCIAL");
                            txt_cnpj.Text = reader.GetString("CNPJ");
                            txt_IE.Text = reader.GetString("INSCRICAO_ESTADUAL");
                            txt_address.Text = reader.GetString("ADDRESS");
                            txt_city.Text = reader.GetString("CITY");
                            txt_status.Text = reader.GetString("STATUS");
                            state = reader.GetString("UF");
                            tipo = reader.GetString("TIPO_CNPJ");
                            CaptureCBB();
                        }
                    }
                        btn_associa.Enabled = false;                    
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

        public void CaptureCBB ()
        {          
            for (int i = 0; i <= cbb_state.Items.Count - 1; i++)
            {
                if (state== cbb_state.Items[i].ToString())
                    {
                     cbb_state.SelectedIndex = i;
                    }
            }

            for (int i = 0; i <= cbb_matriz.Items.Count - 1; i++)
            {
                if (tipo == cbb_matriz.Items[i].ToString())
                {
                    cbb_matriz.SelectedIndex = i;
                }
            }

        }        
		
        private void tsb_clean_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btn_associa_Click(object sender, EventArgs e)
        {
            Frm_CompaniesXCustomers list = new Frm_CompaniesXCustomers();
            list.ShowDialog();
        }

        private void tsb_add_Click(object sender, EventArgs e)
        {
            if (txt_codcompany.Text == "")
            {
                if ((txt_razaoSocial.Text == "") || (txt_cnpj.Text == "") || (cbb_matriz.Text == "") || (txt_address.Text == "") || (txt_city.Text == "") || (cbb_state.Text == "") || (txt_codcustomer.Text == ""))
                {
                    MessageBox.Show("Please fill out all fields and associate the Company with a Customer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    try
                    {
                        connection.OpenConnection();
                        string sql = "INSERT INTO db_sis.tb_companies (cod_customer, NOME_FANTASIA, RAZAO_SOCIAL, CNPJ, TIPO_CNPJ,INSCRICAO_ESTADUAL,ADDRESS,CITY,UF, STATUS) VALUES (@COD, @NOME, @RAZAOS, @CNPJ, @MATRIZ, @IE, @ADDRESS, @CITY, @cbb_state,'" + "ATIVO" + "')";

                        MySqlParameter[] parameters = new MySqlParameter[]
                        {
                            new MySqlParameter("@COD", txt_codcustomer.Text),
                            new MySqlParameter("@NOME", txt_nomefantasia.Text), 
                            new MySqlParameter("@RAZAOS", txt_razaoSocial.Text),
                            new MySqlParameter("@CNPJ", txt_cnpj.Text),
                            new MySqlParameter("@MATRIZ", cbb_matriz.Text),
                            new MySqlParameter("@IE", txt_IE.Text),
                            new MySqlParameter("@ADDRESS",txt_address.Text),
                            new MySqlParameter("@CITY", txt_city.Text),
                            new MySqlParameter("@cbb_state", cbb_state.Text)
                        };

                        MySqlCommand cmd = connection.CreateCommand(sql, parameters);
                        MySqlDataReader reader = cmd.ExecuteReader();
                        MessageBox.Show("Company included successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                       
                        Clear();
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
            else
            {
                MessageBox.Show("Clear the form to insert a new company.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsb_save_Click(object sender, EventArgs e)
        {
            if (!(txt_codcompany.Text == ""))
            {
                try
                {
                    connection.OpenConnection();                    
                    string sql = "UPDATE db_sis.tb_companies SET NOME_FANTASIA = @NOMEF, RAZAO_SOCIAL = @RAZAOS, CNPJ = @CNPJ, TIPO_CNPJ = @TIPO_CNPJ, INSCRICAO_ESTADUAL = @IE, ADDRESS = @ADDRESS, CITY = @CITY, UF = @UF WHERE COD_EMPRESA = @COD";

                    MySqlParameter[] parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@NOMEF",txt_nomefantasia.Text),
                        new MySqlParameter("@RAZAOS", txt_razaoSocial.Text),
                        new MySqlParameter("@CNPJ", txt_cnpj.Text),
                        new MySqlParameter("@TIPO_CNPJ", cbb_matriz.Text),
                        new MySqlParameter("@IE", txt_IE.Text),
                        new MySqlParameter("@ADDRESS", txt_address.Text),
                        new MySqlParameter("@CITY", txt_city.Text),
                        new MySqlParameter("@UF", cbb_state.Text),
                        new MySqlParameter("@COD", int.Parse(txt_codcompany.Text))
                    };

                    MySqlCommand cmd = connection.CreateCommand(sql,parameters);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    MessageBox.Show("Update successfully executed!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

       private void Clear()
        {
            txt_codempresa.Text = "";
            txt_status.Text = "";
            txt_codcliente.Text = "";
            txt_Cliente.Text = "";
            txt_status.Text = "";
            cbb_matriz.SelectedIndex = -1;
            cbb_UF.SelectedIndex = -1;
            txt_razaoSocial.Text = "";
            txt_cnpj.Text = "";
            txt_endereco.Text = "";
            txt_cidade.Text = "";
            txt_IE.Text = "";
            btn_associa.Enabled = true;
            txt_nomefantasia.Text = "";
        }
    }
}
