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
using System.Security.Cryptography;

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
            YCryptoTest w = new YCryptoTest();
            w.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            YDataBaseConfigFile.createDataBase("D:\\Projects\\YAgileDoNet\\YAdoNet\\DataBaseConfig.xml", "SQLServer","");
        }
    }
}
