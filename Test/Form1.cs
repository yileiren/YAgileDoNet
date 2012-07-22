using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using YLR.YAdoNet;
using YLR.YCrypto;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fileName = System.Environment.CurrentDirectory + "\\DataBaseConfig.xml";

            YDataBase db = YDataBaseConfigFile.createDataBase(fileName,"SQLServer");

            if (db == null)
            {
                MessageBox.Show("");
            }
            else
            {
                if (db.connectDataBase())
                {
                    MessageBox.Show("con");
                    db.disconnectDataBase();
                }
            }
        }
    }
}
