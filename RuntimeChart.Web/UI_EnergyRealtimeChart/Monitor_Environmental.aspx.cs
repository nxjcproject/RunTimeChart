using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using WebStyleBaseForEnergy;

namespace RuntimeChart.Web.UI_EnergyRealtimeChart
{
    public partial class Monitor_Environmental : WebStyleBaseForEnergy.webStyleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.InitComponts();
            if (!IsPostBack)
            {
#if DEBUG
                ////////////////////调试用,自定义的数据授权
                List<string> m_DataValidIdItems = new List<string>() { "zc_nxjc_klqc", "zc_nxjc_tsc", "zc_nxjc_znc" };
                AddDataValidIdGroup("ProductionOrganization", m_DataValidIdItems);
                //Hiddenfield_PageId.Value = "EnvironmentalMonitor";
#elif RELEASE
                //string m_PageId = Request.QueryString["PageId"] != null? Request.QueryString["PageId"]: "";
                //Hiddenfield_PageId.Value = m_PageId;
#endif
                Hiddenfield_PageId.Value = "EnvironmentalMonitor";
            }
        }
        
        [WebMethod]
        public static string GetOrganizationTree()
        {
            string m_ReturnString = "";
            List<string> m_OrganizationIdArray = GetDataValidIdGroup("ProductionOrganization");
            m_ReturnString = RuntimeChart.Service.Monitor_Environmental.GetOrganizationTree(m_OrganizationIdArray.ToArray());
            return m_ReturnString;
        }
    }
}