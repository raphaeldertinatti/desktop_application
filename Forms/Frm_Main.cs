using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DesktopApplication.Forms;

namespace DesktopApplication
{
    public partial class Frm_Main : Form
    {
        
        public Frm_Main()
        {
            InitializeComponent();
        }     

        private void customersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_Customers f = new Frm_Customers();
            f.MdiParent = this;
            f.Show();
        }

        private void companiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_Companies f = new Frm_Companies();
            f.MdiParent = this;
            f.Show();
        }

        private void contactsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_Contacts f = new Frm_Contacts();
            f.MdiParent = this;
            f.Show();
        }

        private void departmentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_Departments f = new Frm_Departments();
            f.MdiParent = this;
            f.Show();
        }

        private void servicesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_Services f = new Frm_Services();
            f.MdiParent = this;
            f.Show();
        }

        private void taxesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_Taxes f = new Frm_Taxes();
            f.MdiParent = this;
            f.Show();
        }

        private void searchDocToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_Search_Doc f = new Frm_Search_Doc();
            f.MdiParent = this;
            f.Show();
        }        

        private void searchProductsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_Search_Products f = new Frm_Search_Products();
            f.MdiParent = this;
            f.Show();
        }
    }
}
