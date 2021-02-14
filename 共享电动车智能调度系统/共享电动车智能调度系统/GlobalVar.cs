using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 共享电动车智能调度系统
{
    class GlobalVar
    {
        private static string user;
        public static string User
        {
            set { user = value; }
            get { return user; }
        }
        
    }
}
