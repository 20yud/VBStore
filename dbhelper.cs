using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBStore
{
    public class dbhelper
    {
        private string connectionString = "Data Source=DESKTOP-C753LHT\\SQLEXPRESS;Initial Catalog=CNPM_DB;Integrated Security=True";

        public string ConnectionString
        {
            get { return connectionString; }
        }
    }
}