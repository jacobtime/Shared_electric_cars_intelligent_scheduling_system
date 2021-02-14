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
    public partial class Login : Form
    {
        public static string conString = "server=DESKTOP-DFGSCKR;Integrated Security = true; database=electric_car";

        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = conString;
            conn.Open();
            SqlCommand cmd = new SqlCommand("select * from userid where username='" + username.Text.Trim() + "'", conn);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("用户未注册！");
                return;
            }
            string pass = dt.Rows[0]["password"].ToString();
            string pass_test = password.Text;
            string pass_test_en = "";
            for (int i = 0; i < pass_test.Length; i++)
            {
                //转化为字符的形式处理
                char temp = pass_test[i];
                //针对不同位置，相同字符得到的密文并不相同
                temp = (char)((int)(temp) - i / 2 + 2);
                pass_test_en += temp;
            }
            conn.Close();
            if (pass.Equals(pass_test_en))
            {
                GlobalVar.User = username.Text;
                DialogResult = DialogResult.OK;
                Dispose();
                Close();
            }
            else
            {
                MessageBox.Show("密码不正确");
                password.Text = "";
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dr;
            dr = MessageBox.Show("确认退出吗", "确认对话框", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (dr == DialogResult.Yes)
            {
                Application.ExitThread();
            }
        }
    }
}
