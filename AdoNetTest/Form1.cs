using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using YLR.YAdoNet;

namespace AdoNetTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            YDataBase db = YDataBaseConfigFile.createDataBase("D:/Projects/YAgileDoNet/YAdoNet/DataBaseConfig.xml", "SQLServer", "");
            if (db.connectDataBase())
            {
                MessageBox.Show("true");
                db.disconnectDataBase();
            }
            else
            {
                MessageBox.Show(db.errorText);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            YDataBase db = YDataBaseConfigFile.createDataBase("D:/Projects/YAgileDoNet/YAdoNet/DataBaseConfig.xml", "SQLite", "");
            if (db.connectDataBase())
            {
                MessageBox.Show("connect");
                if (null != db.executeSqlReturnDt("SELECT * FROM sys_users"))
                {
                    MessageBox.Show("yes");
                }
                else
                {
                    MessageBox.Show("no");
                }
                db.disconnectDataBase();
            }
            else
            {
                MessageBox.Show(db.errorText);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            YDataBase db = YDataBaseConfigFile.createDataBase("D:/Projects/YAgileDoNet/YAdoNet/DataBaseConfig.xml", "Access2007", "");
            if (db.connectDataBase())
            {
                MessageBox.Show("connect");
                if (null != db.executeSqlReturnDt("SELECT * FROM tb_test"))
                {
                    MessageBox.Show("yes");
                }
                else
                {
                    MessageBox.Show("no");
                }
                db.disconnectDataBase();
            }
            else
            {
                MessageBox.Show(db.errorText);
            }
        }
    }
}
