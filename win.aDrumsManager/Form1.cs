using aDrumsLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aDrum
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = String.Join("\n", DrumManager.getCOMPorts());

        }
  
        private void button1_Click(object sender, EventArgs e)
        {
            new DrumManager().Connect();
        }

    }
}
