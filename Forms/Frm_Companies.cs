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
            string[] UF = { "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA", "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI", "RJ", "RN", "RS", "RO", "RR", "SC", "SP", "SE", "TO" };

            cbb_matriz.Items.Clear();
            cbb_matriz.Items.Add("Matriz");
            cbb_matriz.Items.Add("Filial");

            cbb_UF.Items.Clear();
            for (int i = 0; i < UF.Length; i++)
            {
                cbb_UF.Items.Add(UF[i]);    
            }             
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
                    string[] column = { "COD_CLIENTE", "NOME_CLIENTE", "NOME_FANTASIA", "RAZAO_SOCIAL", "CNPJ", "INSCRICAO_ESTADUAL", "ENDERECO", "CIDADE", "STATUS" };
                    TextBox[] textBoxes = { txt_codcliente, txt_Cliente, txt_nomefantasia, txt_razaoSocial, txt_cnpj, txt_IE, txt_endereco, txt_cidade, txt_status };
                    MySqlCommand cmd = new MySqlCommand(sql, connection.conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            for (int i = 0; i < column.Length; i++)
                            {
                                textBoxes[i].Text = reader.GetString(column[i]);
                            }
                            estado = reader.GetString("UF");
                            tipo = reader.GetString("TIPO_CNPJ");                            
                            CapturaCBB();
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
                        string[] param_name = { "@COD", "@NOME", "@RAZAOS", "@CNPJ", "@MATRIZ", "@IE", "@ENDERECO", "@CIDADE", "@CBB_UF" };
                        dynamic[] dynamics = { txt_codcliente, txt_nomefantasia, txt_razaoSocial, txt_cnpj, cbb_matriz, txt_IE, txt_endereco, txt_cidade, cbb_UF };
                        MySqlParameter[] parameters = new MySqlParameter[9];

                        for (int i = 0; i < param_name.Length; i++)
                        {
                            parameters[i] = new MySqlParameter(param_name[i], dynamics[i].Text);
                        }  

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
