﻿using System;
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
            YAccessDataBase db = new YAccessDataBase();
            //db.databaseType = DataBaseType.Access2007;
            db.filePath = "D:\\test.mdb";
            if (db.connectDataBase())
            {
                MessageBox.Show("ok");
                db.disconnectDataBase();
            }
            else
            {
                MessageBox.Show("error||" + db.errorText);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
