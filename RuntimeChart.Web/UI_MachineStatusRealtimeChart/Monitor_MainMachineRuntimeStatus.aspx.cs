using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using WebStyleBaseForEnergy;

namespace RuntimeChart.Web.UI_MachineStatusRealtimeChart
{
    public partial class Monitor_MainMachineRuntimeStatus  : WebStyleBaseForEnergy.webStyleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.InitComponts();
            if (!IsPostBack)
            {
#if DEBUG
                ////////////////////调试用,自定义的数据授权
                List<string> m_DataValidIdItems = new List<string>() {  "zc_nxjc_klqc", "zc_nxjc_znc", "zc_nxjc_lpsc", "zc_nxjc_whsmc" };
                //List<string> m_DataValidIdItems = new List<string>() {"zc_nxjc_byc"};
                AddDataValidIdGroup("ProductionOrganization", m_DataValidIdItems);
#elif RELEASE
                //string m_PageId = Request.QueryString["PageId"] != null? Request.QueryString["PageId"]: "";
                //Hiddenfield_PageId.Value = m_PageId;
#endif
            }
        }
         [WebMethod]
        public static string GetEquipmentParameters(string myEquipmentCommonIds) 
        {
            string m_ReturnString = "";
            List<string> m_OrganizationIdArray = GetDataValidIdGroup("ProductionOrganization");
            DataTable m_EquipmentParameters = RuntimeChart.Service.Monitor_MainMachineRuntimeStatus.GetEquipmentParameters(myEquipmentCommonIds, m_OrganizationIdArray.ToArray());
            DataTable m_EquipmentParametersAll = RuntimeChart.Service.Monitor_MainMachineRuntimeStatus.GetParametersCogeneration(m_EquipmentParameters, m_OrganizationIdArray.ToArray());

            m_ReturnString = RuntimeChart.Service.Monitor_MainMachineRuntimeStatus.GetEquipmentRunTags(m_EquipmentParametersAll);
            return m_ReturnString;
        }
         [WebMethod]
         public static string GetEquipmentRuntimeStatus(string myTags)
         {
             Dictionary<string, List<string>> m_TagsDic = new Dictionary<string, List<string>>();
             Dictionary<string, bool> m_ValueDic = new Dictionary<string, bool>();
             if (myTags != "")
             {
                 string[] m_TagsGroup = myTags.Split(';');
                 for (int i = 0; i < m_TagsGroup.Length; i++)
                 {
                     string[] m_TagInfo = m_TagsGroup[i].Split(',');
                     if (!m_TagsDic.ContainsKey(m_TagInfo[0]))
                     {
                         m_TagsDic.Add(m_TagInfo[0], new List<string>());
                     }
                     if (!m_TagsDic[m_TagInfo[0]].Contains(m_TagInfo[1]))
                     {
                         m_TagsDic[m_TagInfo[0]].Add(m_TagInfo[1]);
                     }
                 }
             }
             foreach (string key in m_TagsDic.Keys)
             {
                 /////////从WebService中获得数据//////////
                 RuntimeChart.Service.Monitor_MainMachineRuntimeStatus.GetBooleanResult(key, m_TagsDic[key].ToArray(), ref m_ValueDic);
             }
             string m_ReturnString = "";
             foreach (string key in m_ValueDic.Keys)
             {
                 if (m_ReturnString == "")
                 {
                     m_ReturnString = "\"" + key + "\":\"" + m_ValueDic[key] + "\"";
                 }
                 else
                 {
                     m_ReturnString = m_ReturnString + ",\"" + key + "\":\"" + m_ValueDic[key] + "\"";
                 }
             }
             m_ReturnString = "{" + m_ReturnString + "}";
             return m_ReturnString;
         }
         [WebMethod]
         public static string GetEquipmentHaltStatus(string myTags)
         {
             Dictionary<string, bool> m_ValueDic = RuntimeChart.Service.Monitor_MainMachineRuntimeStatus.GetEquipmentHaltStatus(myTags);
             string m_ReturnString = "";
             foreach (string key in m_ValueDic.Keys)
             {
                 if (m_ReturnString == "")
                 {
                     m_ReturnString = "\"" + key + "\":\"" + m_ValueDic[key].ToString() + "\"";
                 }
                 else
                 {
                     m_ReturnString = m_ReturnString + ",\"" + key + "\":\"" + m_ValueDic[key].ToString() + "\"";
                 }
             }
             m_ReturnString = "{" + m_ReturnString + "}";
             return m_ReturnString;
         }
         [WebMethod]
         public static string GetMachineHaltRecord(string myOrganizationId, string myEquipmentId)
         {
             string m_ReturnString = "";
             m_ReturnString = RuntimeChart.Service.Monitor_MainMachineRuntimeStatus.GetMachineHaltRecord(myOrganizationId, myEquipmentId);
             return m_ReturnString;
         }

    }
}