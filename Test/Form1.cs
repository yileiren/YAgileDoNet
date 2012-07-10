using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using YLR.YAdoNet;

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
            YSQLiteDataBase db = new YSQLiteDataBase();
            db.filePaht = "D:\\test.db";
            db.connectDataBase();
            db.executeSqlWithOutDs("INSERT INTO test (name) VALUES ('ttt')");
            db.disconnectDataBase();
        }
    }
}
