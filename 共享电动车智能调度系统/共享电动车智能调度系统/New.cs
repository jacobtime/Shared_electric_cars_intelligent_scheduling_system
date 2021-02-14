using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 共享电动车智能调度系统
{

    public partial class New : Form
    {
        public static string conString = "server=DESKTOP-DFGSCKR;Integrated Security = true; database=electric_car";
        private int num;
        public New()
        {
            InitializeComponent();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if(int.TryParse(nodeNum.Text,out num) )
            {
                dataGridView2.Rows.Clear();
                for(int i = 0; i < num; i++)
                {
                    dataGridView2.Rows.Add((i + 1).ToString(), "");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                SqlCommand da = new SqlCommand("insert into area values('" + GlobalVar.User + "','" + dbName.Text + "'," + Maxn.Text + ", " + nodeNum.Text + ")", con);
                da.ExecuteNonQuery();
                for(int i = 0; i < dataGridView2.Rows.Count - 1; i++)
                {
                    da = new SqlCommand("insert into now values('" + GlobalVar.User + "','" + dbName.Text + "'," + int.Parse(dataGridView2.Rows[i].Cells[0].Value.ToString()) + ", " + int.Parse(dataGridView2.Rows[i].Cells[1].Value.ToString()) + ")", con);
                    da.ExecuteNonQuery();
                }
                
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    da = new SqlCommand("insert into distance values('" + GlobalVar.User + "','" + dbName.Text + "'," + int.Parse(dataGridView1.Rows[i].Cells[0].Value.ToString()) + ", " + int.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString()) + ", " + int.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString()) + ")", con);
                    da.ExecuteNonQuery();
                }
                
                con.Close();
            }
            MessageBox.Show("添加成功！");
            Dispose();
            Close();
        }
    }
}
