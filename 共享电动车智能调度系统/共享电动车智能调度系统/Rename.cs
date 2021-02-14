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
    public partial class Rename : Form
    {
        public static string conString = "server=DESKTOP-DFGSCKR;Integrated Security = true; database=electric_car";
        public static string old_name, new_name;
        public Rename()
        {
            InitializeComponent(); 
            using (SqlConnection con = new SqlConnection(conString))
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter("select * from area where username = '" + GlobalVar.User + "'", con);
                da.Fill(dt);
                foreach (DataRow r in dt.Rows)
                {
                    comboBox1.Items.Add(r["name"]);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                SqlCommand da = new SqlCommand("update now set name = '" + new_name + "' where username = '" + GlobalVar.User + "' and name = '" + old_name + "'", con);
                da.ExecuteNonQuery();
                da = new SqlCommand("update distance set name = '" + new_name + "' where username = '" + GlobalVar.User + "' and name = '" + old_name + "'", con);
                da.ExecuteNonQuery();
                da = new SqlCommand("update area set name = '" + new_name + "' where username = '" + GlobalVar.User + "' and name = '" + old_name + "'", con);
                da.ExecuteNonQuery();
                con.Close();
            }
            Dispose();
            Close();
            MessageBox.Show("修改成功！");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Dispose();
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            new_name = textBox1.Text;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            old_name = comboBox1.SelectedItem.ToString();
        }
    }
}
