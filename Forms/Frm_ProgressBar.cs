using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopApplication
{
    public partial class Frm_ProgressBar : Form
    {
        public static Frm_ProgressBar instance;
        public int totalLines;
        public Frm_ProgressBar()
        {
            InitializeComponent();
        }
    }
}
