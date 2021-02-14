using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Numerics;
using System.Data.SqlClient;
using System.Data;

namespace 共享电动车智能调度系统
{
    class Solve
    {
        public static string conString = "server=DESKTOP-DFGSCKR;Integrated Security = true; database=electric_car";
        const int INF = 0x3f3f3f3f;
        private int[] distance = new int[505];
        private ArrayList ansPath = new ArrayList();
        private ArrayList tempPath = new ArrayList();
        private ArrayList[] v = new ArrayList[505];
        private ArrayList[] pre = new ArrayList[505];
        private Priority_queue pq = new Priority_queue();
        private bool[] vis = new bool[505];
        private int[] a = new int[505];
        private int minbring = new int();
        private int minhave = new int();
        private string Say;
        public Solve(string name, int number, int maxn)
        {
            for (int i = 0; i < 505; i++)
            {
                v[i] = new ArrayList();
                pre[i] = new ArrayList();
            }
            minbring = minhave = INF;
            using (SqlConnection con = new SqlConnection(conString))
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter("select * from now where username = '" + GlobalVar.User + "' and name = '" + name + "'", con);
                da.Fill(dt);
                int cnt = 1;
                foreach (DataRow r in dt.Rows)
                {
                    a[cnt++] = int.Parse(r["num"].ToString());
                }
            }
            using (SqlConnection con = new SqlConnection(conString))
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter("select * from distance where username = '" + GlobalVar.User + "' and name = '" + name + "'", con);
                da.Fill(dt);
                foreach (DataRow r in dt.Rows)
                {
                    Node temp = new Node(int.Parse(r["to_id"].ToString()), int.Parse(r["distance"].ToString()));
                    v[int.Parse(r["from_id"].ToString())].Add(temp);
                }
            }
            for (int i = 1; i < 505; i++)
            {
                a[i] -= maxn / 2;
            }
            findMinLoad();
            DFS(number);
            Say = "调度员要从库中调取" + minbring + "辆车，沿着路线 0";
            for (int i = ansPath.Count - 2; i >= 0; i--)
            {
                Say += "->" + ansPath[i];
            }
            Say += " 并且能带回" + minhave + "辆车";
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                for (int i = ansPath.Count - 2; i >= 0; i--)
                {
                    SqlCommand da = new SqlCommand("update now set num = " + maxn / 2 + " where username = '" + GlobalVar.User + "' and name = '" + name + "' and id = '" + ansPath[i] + "'", con);
                    da.ExecuteNonQuery();
                }
            
                con.Close();
            }
        }

        void findMinLoad()
        {
            for (int i = 0; i < 505; i++)
            {
                distance[i] = INF;
                vis[i] = false;
            }
            distance[0] = 0;
            pq.EnQueue(new Node(0, distance[0]));
            while (!pq.isEmpty())
            {
                int z = pq.DeQueue().id;
                if (vis[z])
                    continue;
                vis[z] = true;
                for (int i = 0; i < v[z].Count; i++)
                {
                    Node now = (Node)v[z][i];
                    if (distance[z] + now.value < distance[now.id])
                    {
                        distance[now.id] = distance[z] + now.value;
                        pre[now.id].Clear();
                        pre[now.id].Add(z);
                        pq.EnQueue(new Node(now.id, distance[now.id]));
                    }
                    else if (distance[z] + now.value == distance[now.id])
                    {
                        pre[now.id].Add(z);
                    }

                }
            }
        }

        void DFS(int now)
        {
            tempPath.Add(now);
            if (now == 0)//如果找到头了
            {
                int bring = 0, have = 0;//bring记录本条最短路径所需要带的车，和带走的车
                for (int i = tempPath.Count - 1; i >= 0; i--)//记得我们是递归查找，顺序要反过来
                {
                    have += a[(int)tempPath[i]];//现存先加上当前多/少了的车
                    if (have < 0)//如果小于0，说明之前的车不满足题目条件，得开始就多带些车
                    {
                        bring -= have;//开始要多带的车（因为have为负数，直接减）
                        have = 0;
                    }
                }
                if (bring < minbring)//如果带的车比当前最小还小，更新
                {
                    minbring = bring;//更新最小值
                    minhave = have;//更新带回去的车
                    ansPath = (ArrayList)tempPath.Clone();//更新路径
                }
                else if (bring == minbring && have < minhave)//如果带的车一样多，但是带回去的车少
                {
                    minhave = have;
                    ansPath = (ArrayList)tempPath.Clone();
                }
                tempPath.RemoveAt(tempPath.Count - 1);//DFS回溯 （在临时路径中删掉本节点继续遍历）
                return;
            }
            for (int i = 0; i < pre[now].Count; i++)//如果没到头，就遍历他的前驱节点
            {
                DFS((int)pre[now][i]);
            }
            tempPath.RemoveAt(tempPath.Count - 1);//DFS回溯
        }

        public string say()
        {
            return Say;
        }
    }
}
