using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using YLR.YCrypto;
using System.Security.Cryptography;

namespace Test
{
    public partial class YCryptoTest : Form
    {
        public YCryptoTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.textBox6.Text = Encoding.UTF8.GetString(AESEncrypt.decrypt(Convert.FromBase64String(this.textBox4.Text), this.textBox5.Text));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = Convert.ToBase64String(AESEncrypt.encrypt(this.textBox1.Text, this.textBox3.Text));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.textBox11.Text = Convert.ToBase64String(DESEncrypt.encrypt(this.textBox12.Text, this.textBox10.Text));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.textBox7.Text = Encoding.UTF8.GetString(DESEncrypt.decrypt(Convert.FromBase64String(this.textBox9.Text), this.textBox8.Text));
        }
    }
}
