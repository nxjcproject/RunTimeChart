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
    public partial class Monitor_KeyIndicators : WebStyleBaseForEnergy.webStyleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.InitComponts();
            if (!IsPostBack)
            {
#if DEBUG
                ////////////////////调试用,自定义的数据授权
                List<string> m_DataValidIdItems = new List<string>() { "zc_nxjc_byc_byf", "zc_nxjc_qtx", "zc_nxjc_znc" };
                AddDataValidIdGroup("ProductionOrganization", m_DataValidIdItems);
                //Hiddenfield_PageId.Value = "EnergyMonitor";
#elif RELEASE
                //string m_PageId = Request.QueryString["PageId"] != null? Request.QueryString["PageId"]: "";
                //Hiddenfield_PageId.Value = m_PageId;
#endif
                Hiddenfield_PageId.Value = "EnergyMonitor";
            }
        }
        [WebMethod]
        public static string GetChartData(string myOrganizationId, string myPageId, string myStaticsCycle)
        {
            //string m_OrganizationIdTemp = "zc_nxjc_qtx_tys_clinker04";
            string m_ReturnString = "";
            int m_StaticsCycle = Int32.Parse(myStaticsCycle);
            DataTable m_TagItemsInfoTable = null;
            if (myOrganizationId != "" && myOrganizationId != "All")
            {
                m_TagItemsInfoTable = RuntimeChart.Service.Monitor_KeyIndicators.GetTagItemsInfoTable(new string[] { myOrganizationId }, myPageId);
            }
            else
            {
                List<string> m_OrganizationIdArray = GetDataValidIdGroup("ProductionOrganization");
                m_TagItemsInfoTable = RuntimeChart.Service.Monitor_KeyIndicators.GetTagItemsInfoTable(m_OrganizationIdArray.ToArray(), myPageId);
            }
            DataTable m_QueryDateTimeTable = new DataTable();
            m_QueryDateTimeTable.Columns.Add("StartTime", typeof(DateTime));
            m_QueryDateTimeTable.Columns.Add("EndTime", typeof(DateTime));

            m_QueryDateTimeTable.Rows.Add(new object[] { DateTime.Now.AddMinutes(-m_StaticsCycle), DateTime.Now.AddMinutes(5) });
            //m_QueryDateTimeTable.Rows.Add(new object[] { "2017-01-20 10:44:12", "2017-02-20 12:44:12" });
            RuntimeChart.Service.Monitor_KeyIndicators.GetChartData(ref m_TagItemsInfoTable, m_QueryDateTimeTable);

            if (m_TagItemsInfoTable != null)
            {
                m_ReturnString = RuntimeChart.Service.Monitor_KeyIndicators.GetChartDataJson(m_TagItemsInfoTable);
            }

            ////////////
            
            //m_Data['max'] = 100;
            //m_Data["value"] = 50;
            //m_Data["intervals"] = [30, 70, 100];
            //m_Data["label"] = "1#水泥磨综合电耗(千瓦时/千克)";
            //string m_ReturnString = "[{\"max\":32,\"min\":29,\"value\":31.5,\"intervals\":[30, 31, 32],\"label\":\"熟料烧成单耗(Kwh/t)\"}," +
            //                         "{\"max\":4,\"min\":2,\"value\":0,\"intervals\":[2.5,3,4],\"label\":\"主电机单耗(Kwh/t)\"}," +
            //                         "{\"max\":12,\"min\":9,\"value\":6.4,\"intervals\":[10, 11, 12],\"label\":\"高温风机单耗(Kwh/t)\"}," +
            //                         "{\"max\":3,\"min\":1,\"value\":2,\"intervals\":[2, 2.5, 3],\"label\":\"排头风机单耗(Kwh/t)\"}," +
            //                         "{\"max\":5,\"min\":3,\"value\":4,\"intervals\":[4, 4.5, 5],\"label\":\"排尾风机单耗(Kwh/t)\"}," +
            //                         "{\"max\":10,\"min\":7,\"value\":0,\"intervals\":[7.7, 8.4, 10],\"label\":\"篦冷系统单耗(Kwh/t)\"}," +
            //                         "{\"max\":1100,\"min\":500,\"value\":0,\"intervals\":[700, 900, 1100],\"label\":\"窑头袋收尘压差(Kpa)\"}," +
            //                         "{\"max\":300,\"min\":-300,\"value\":0,\"intervals\":[-100, 100, 300],\"label\":\"窑头负压(pa)\"}," +
            //                         "{\"max\":6,\"min\":0,\"value\":0,\"intervals\":[2, 4, 6],\"label\":\"辅助设备单耗(Kwh/t)\"}]";
            return m_ReturnString;
        }
        [WebMethod]
        public static string GetOrganizationTree()
        {
            string m_ReturnString = "";
            List<string> m_OrganizationIdArray = GetDataValidIdGroup("ProductionOrganization");
            m_ReturnString = RuntimeChart.Service.Monitor_KeyIndicators.GetOrganizationTree(m_OrganizationIdArray.ToArray());
            return m_ReturnString;
        }
    }
}