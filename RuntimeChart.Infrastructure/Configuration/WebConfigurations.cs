using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
namespace RuntimeChart.Infrastructure.Configuration
{
    public class WebConfigurations
    {
        public static string StationId
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["StationId"] != null ? System.Configuration.ConfigurationManager.AppSettings["StationId"].ToString() : "";
            }
        }
    }
}
