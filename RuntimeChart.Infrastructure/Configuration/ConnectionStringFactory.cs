using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
namespace RuntimeChart.Infrastructure.Configuration
{
    public class ConnectionStringFactory
    {
        public static string NXJCConnectionString { get { return ConfigurationManager.ConnectionStrings["ConnNXJC"].ToString(); } }
    }
}
