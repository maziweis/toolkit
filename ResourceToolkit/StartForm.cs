using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResourceToolkit
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            TemplateForm1 f = new TemplateForm1();
            f.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ResouceSetForm f = new ResouceSetForm();
            f.ShowDialog();
        }



        private void button8_Click(object sender, EventArgs e)
        {
            ResourceCheckForm f = new ResourceCheckForm();
            f.ShowDialog();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void StartForm_Load(object sender, EventArgs e)
        {

        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            UptResourceForm upt = new UptResourceForm();
            upt.ShowDialog();
        }

        private void NWJson_Click(object sender, EventArgs e)
        {
            NWForm nw = new NWForm();
            nw.ShowDialog();
        }
    }
}
