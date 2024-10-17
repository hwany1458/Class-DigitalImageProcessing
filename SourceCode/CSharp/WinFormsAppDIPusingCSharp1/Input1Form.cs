using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsAppDIPusingCSharp1
{
    public partial class Input1Form : Form
    {
        public Input1Form()
        {
            InitializeComponent();
        }

        // 확인 버튼
        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        // 취소 버튼
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        // --------- user defined function

    }
}
