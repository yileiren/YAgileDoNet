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
            YAccessDataBase db = new YAccessDataBase();
            db.databaseType = DataBaseType.Access2007;
            db.filePath = "D:\\DataBase\\test.accdb";
            if (db.connectDataBase())
            {
                MessageBox.Show("ok");
                YParameters p = new YParameters();
                p.add("@value1","text1");
                p.add("@value2","text2");
                DataTable dt = db.executeSqlReturnDt("SELECT * FROM tb_test WHERE text1 = @value1 AND text2 = @value2", p);
                if (null != dt)
                {
                    MessageBox.Show("row count " + dt.Rows.Count.ToString());
                }
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
