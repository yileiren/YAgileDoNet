using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using YLR.YCrypto;

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
            string bbb = Convert.ToBase64String(AESEncrypt.encrypt("AES加密算法测试数据", "dongbinhuiasxiny"));
            MessageBox.Show(bbb);
            MessageBox.Show(Encoding.UTF8.GetString(AESEncrypt.decrypt(Convert.FromBase64String(bbb), "dongbinhuiasxiny")));
        }
    }
}
