using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CAD_Utils
{
    public partial class MessageBoxRichTextBox : Form
    {
        public MessageBoxRichTextBox(string s)
        {
            InitializeComponent();
            rtbRes.Text = s;
        }
    }
}
