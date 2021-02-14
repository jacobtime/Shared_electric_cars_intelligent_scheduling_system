using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 共享电动车智能调度系统
{
    public class Node : IComparable
    {
        public int id;
        public int value;
        public Node(int a, int b)
        {
            id = a;
            value = b;
        }

        public int CompareTo(Object o)
        {
            Node y = (Node)o;
            int result = value < y.value ? -1 : 1;
            return result;
        }
    }
}
