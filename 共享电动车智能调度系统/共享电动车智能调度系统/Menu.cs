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
    public partial class Menu : Form
    {
        public static string conString = "server=DESKTOP-DFGSCKR;Integrated Security = true; database=electric_car";
        public static string name;
        public static int maxn;
        public Menu()
        {
            InitializeComponent();
            ReFresh();
        }
        public void ReFresh()
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                Area.Items.Clear();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter("select * from area where username = '" + GlobalVar.User + "'", con);
                da.Fill(dt);
                foreach (DataRow r in dt.Rows)
                {
                    Area.Items.Add(r["name"]);
                }
            }
        }
        private void showinf()
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter("select * from now where username = '" + GlobalVar.User + "' and name = '" + name + "'", con);
                da.Fill(dt);
                dataGridView1.DataSource = dt.DefaultView;
                ID.Items.Clear();
                foreach (DataRow r in dt.Rows)
                {
                    ID.Items.Add(r["id"]);
                }
            }
            using (SqlConnection con = new SqlConnection(conString))
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter("select * from area  where username = '" + GlobalVar.User + "' and name = '" + name + "'", con);
                da.Fill(dt);
                maxn = int.Parse(dt.Rows[0]["maxn"].ToString());
            }
        }

        private void 操作_Load(object sender, EventArgs e)
        {
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            name = dataGridView1.SelectedCells[1].Value.ToString();

        }

        private void 打开ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            name = Area.SelectedItem.ToString();
            showinf();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int now = 0, num = 0;
            bool flag = false;
            Solve solve;
            if (Add.Text != "" && Cut.Text != "")
            {
                MessageBox.Show("请不要同时在两个区域内填入内容！");
            }
            else if(Add.Text != "")
            {
                if (!int.TryParse(Add.Text, out num))
                {
                    MessageBox.Show("请填入数字！");
                }
                else
                {
                    now = int.Parse(ID.Text);
                    flag = true;
                }
            }
            else
            {
                if (!int.TryParse(Cut.Text, out num))
                {
                    MessageBox.Show("请填入数字！");
                }
                else
                {
                    now = int.Parse(ID.Text);
                    num *= -1;
                    flag = true;
                }
            }
            if (flag)
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    SqlCommand da = new SqlCommand("update now set num += " + num + " where username = '" + GlobalVar.User + "' and name = '" + name + "' and id = '" + now + "'", con);
                    da.ExecuteNonQuery();
                    con.Close();
                }
                showinf();
                if (num < maxn / 2)
                {
                    solve = new Solve(name, now, maxn);
                    Solution.Text = solve.say();
                }
            }
            showinf();
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            New newForm = new New();
            newForm.ShowDialog();
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr;
            dr = MessageBox.Show("确认删除吗", "确认对话框", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (dr == DialogResult.Yes)
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    SqlCommand da = new SqlCommand("delete from now where username = '" + GlobalVar.User + "' and name = '" + name + "'", con);
                    da.ExecuteNonQuery();
                    da = new SqlCommand("delete from area where username = '" + GlobalVar.User + "' and name = '" + name + "'", con);
                    da.ExecuteNonQuery();
                    da = new SqlCommand("delete from distance where username = '" + GlobalVar.User + "' and name = '" + name + "'", con);
                    da.ExecuteNonQuery();
                    con.Close();
                }
            }
            ReFresh();
        }

        private void 退出系统ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr;
            dr = MessageBox.Show("确认退出吗", "确认对话框", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (dr == DialogResult.Yes)
            {
                Application.ExitThread();
            }
        }

        private void 重命名ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rename output = new Rename();
            output.ShowDialog();
            ReFresh();
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("保存成功！");
        }

        private void 编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.ReadOnly = false;
        }

        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help help = new Help();
            help.ShowDialog();
        }
        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            New news = new New();
            news.ShowDialog();
            ReFresh();
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }

        private void Area_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void ID_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
