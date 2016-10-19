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
            using (var dm = new DrumManager())
            {
                dm.Connect();
                dm.Triggers.ElementAt(2).Threshold = 100;
                dm.SaveSettings();
                dm.LoadSettings();
                MessageBox.Show(dm.Triggers.ElementAt(2).Threshold.ToString());
                
                
            } 
        }

    }
}
