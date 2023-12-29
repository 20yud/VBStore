using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBStore
{
    public class dbhelper
    {
        private string connectionString = "Data Source=DESKTOP-59ANJ2R\\SQLEXPRESS03;Initial Catalog=CNPM;Integrated Security=True";

        public string ConnectionString
        {
            get { return connectionString; }
        }
    }
}