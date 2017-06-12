using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SqlServerDataAdapter;
using RuntimeChart.Infrastructure.Configuration;
using StaffAssessment;
namespace RuntimeChart.Service
{
    public class Monitor_KeyIndicators
    {
        private static readonly string _connStr = ConnectionStringFactory.NXJCConnectionString;
        private static readonly ISqlServerDataFactory _dataFactory = new SqlServerDataFactory(_connStr);
        public static string GetOrganizationTree(string[] myOrganizationIdArray)
        {
            string m_OrganizationString = "";
            string m_Sql = @"Select A.OrganizationID, A.Name, A.LevelCode, A.Type, A.LevelType from system_Organization A, system_Organization B
                                where B.OrganizationID in ({0})
                                and (A.LevelCode like B.LevelCode + '%' or CHARINDEX(A.LevelCode, B.LevelCode) > 0)
                                and ((A.LevelType = 'ProductionLine' and A.Type in ('熟料','水泥磨')) or A.LevelType <> 'ProductionLine')
                                order by A.LevelCode";
            if (myOrganizationIdArray != null)
            {
                for (int i = 0; i < myOrganizationIdArray.Length; i++)
                {
                    if (i == 0)
                    {
                        m_OrganizationString = "'" + myOrganizationIdArray[i] + "'";
                    }
                    else
                    {
                        m_OrganizationString = m_OrganizationString + ",'" + myOrganizationIdArray[i] + "'";
                    }
                }
                m_Sql = string.Format(m_Sql, m_OrganizationString);
                try
                {
                    DataTable m_OrganizationTable = _dataFactory.Query(m_Sql);
                    string m_OrganizationTableString = EasyUIJsonParser.TreeJsonParser.DataTableToJsonByLevelCodeWithIdColumn(m_OrganizationTable, "LevelCode", "OrganizationID", "Name", new string[] { "LevelType" });
                    return m_OrganizationTableString;
                }
                catch
                {
                    return "{\"rows\":[],\"total\":0}";
                }
            }
            else
            {
                return "{\"rows\":[],\"total\":0}";
            }
        }

        public static DataTable GetTagItemsInfoTable(string[] myOrganizationIds, string myPageId)
        {
            string m_Sql = @"SELECT A.ItemId, A.ItemName, A.Unit, A.ValueType, A.CaculateType, A.PageId, A.GroupId, A.OrganizationID, rtrim(A.Tags) as Tags, rtrim(A.SubtrahendTags) as SubtrahendTags, A.Min, A.Max, A.AlarmH, A.AlarmHH, A.DisplayIndex, 0.0 as Value
                                FROM realtime_KeyIndicatorsMonitorContrast A, system_Organization B, system_Organization C
                                where A.OrganizationID = B.OrganizationID 
                                   and A.PageId = '{1}' and A.Enabled = 1
                                   and C.OrganizationID in ({0})
                                   and B.LevelCode like C.LevelCode + '%'
                                order by A.OrganizationID, A.DisplayIndex";
            string m_OrganizationIdString = "''";
            if (myOrganizationIds != null)
            {
                for (int i = 0; i < myOrganizationIds.Length; i++)
                {
                    if (i == 0)
                    {
                        m_OrganizationIdString = "'" + myOrganizationIds[i] + "'";
                    }
                    else
                    {
                        m_OrganizationIdString = m_OrganizationIdString + ",'" + myOrganizationIds[i] + "'";
                    }
                }
            }
            m_Sql = string.Format(m_Sql, m_OrganizationIdString, myPageId);
            try
            {
                DataTable m_TagItemsInfoTable = _dataFactory.Query(m_Sql);
                return m_TagItemsInfoTable;
            }
            catch
            {
                return null;
            }

        }
        public static void GetChartData(ref DataTable myTagItemsInfoTable, DataTable myDateTimeTable)
        {
            Dictionary<string, StaffAssessment.Model_CaculateItems> m_CaculateItems = new Dictionary<string, Model_CaculateItems>();
            //因为是按照组织机构区分的,因此即使出现一个组织机构多个DCS，标签理论上也具有唯一性，但要注意处理同一个对照表中出现两个相同的标签的情况
            if (myTagItemsInfoTable != null)
            {
                for (int i = 0; i < myTagItemsInfoTable.Rows.Count; i++)
                {
                    string[] m_AddTagsArray = myTagItemsInfoTable.Rows[i]["Tags"] != DBNull.Value ? myTagItemsInfoTable.Rows[i]["Tags"].ToString().Split(',') : new string[0];
                    string[] m_SubtrahendTagsArray = myTagItemsInfoTable.Rows[i]["SubtrahendTags"] != DBNull.Value ? myTagItemsInfoTable.Rows[i]["SubtrahendTags"].ToString().Split(',') : new string[0];
                    string m_DictionaryKey = myTagItemsInfoTable.Rows[i]["OrganizationID"].ToString() + "_" + myTagItemsInfoTable.Rows[i]["CaculateType"].ToString() + "_" + myTagItemsInfoTable.Rows[i]["ValueType"].ToString();
                    if (m_CaculateItems.ContainsKey(m_DictionaryKey))
                    {
                        for (int j = 0; j < m_AddTagsArray.Length; j++)
                        {
                            StaffAssessment.Model_CaculateItemDetail m_CaculateItemDetail = new Model_CaculateItemDetail();
                            m_CaculateItemDetail.Id = myTagItemsInfoTable.Rows[i]["ItemId"].ToString() + "Add";
                            m_CaculateItemDetail.ObjectId = m_AddTagsArray[j].Replace(" ", ""); ;
                            m_CaculateItemDetail.AssessmentId = "D" + myTagItemsInfoTable.Rows[i]["ItemId"].ToString().Replace("-", "") + j.ToString();

                            m_CaculateItems[m_DictionaryKey].CaculateItemDetail.Add(m_CaculateItemDetail);
                        }
                        for (int j = 0; j < m_SubtrahendTagsArray.Length; j++)
                        {
                            StaffAssessment.Model_CaculateItemDetail m_CaculateItemDetail = new Model_CaculateItemDetail();
                            m_CaculateItemDetail.Id = myTagItemsInfoTable.Rows[i]["ItemId"].ToString() + "Sub";
                            m_CaculateItemDetail.ObjectId = m_SubtrahendTagsArray[j].Replace(" ", ""); ;
                            m_CaculateItemDetail.AssessmentId = "D" + myTagItemsInfoTable.Rows[i]["ItemId"].ToString().Replace("-", "") + (m_AddTagsArray.Length + j).ToString();

                            m_CaculateItems[m_DictionaryKey].CaculateItemDetail.Add(m_CaculateItemDetail);
                        }
                    }
                    else
                    {
                        StaffAssessment.Model_CaculateItems m_CaculateItemsTemp = new Model_CaculateItems();
                        m_CaculateItemsTemp.ValueType = myTagItemsInfoTable.Rows[i]["ValueType"].ToString();
                        m_CaculateItemsTemp.Type = myTagItemsInfoTable.Rows[i]["CaculateType"].ToString();
                        m_CaculateItemsTemp.OrganizaitonId = myTagItemsInfoTable.Rows[i]["OrganizationID"].ToString();

                        for (int j = 0; j < m_AddTagsArray.Length; j++)
                        {
                            StaffAssessment.Model_CaculateItemDetail m_CaculateItemDetail = new Model_CaculateItemDetail();
                            m_CaculateItemDetail.Id = myTagItemsInfoTable.Rows[i]["ItemId"].ToString() + "Add";
                            m_CaculateItemDetail.ObjectId = m_AddTagsArray[j].Replace(" ", "");
                            m_CaculateItemDetail.AssessmentId = "D" + myTagItemsInfoTable.Rows[i]["ItemId"].ToString().Replace("-", "") + j.ToString();
                            m_CaculateItemsTemp.CaculateItemDetail.Add(m_CaculateItemDetail);
                        }
                        for (int j = 0; j < m_SubtrahendTagsArray.Length; j++)
                        {
                            StaffAssessment.Model_CaculateItemDetail m_CaculateItemDetail = new Model_CaculateItemDetail();
                            m_CaculateItemDetail.Id = myTagItemsInfoTable.Rows[i]["ItemId"].ToString() + "Sub";
                            m_CaculateItemDetail.ObjectId = m_SubtrahendTagsArray[j].Replace(" ", "");
                            m_CaculateItemDetail.AssessmentId = "D" + myTagItemsInfoTable.Rows[i]["ItemId"].ToString().Replace("-", "") + (m_AddTagsArray.Length + j).ToString();
                            m_CaculateItemsTemp.CaculateItemDetail.Add(m_CaculateItemDetail);
                        }
                        m_CaculateItems.Add(m_DictionaryKey, m_CaculateItemsTemp);
                    }
                }
            }

            StaffAssessment.Function_AssessmentCaculate.CaculateItemValue(ref m_CaculateItems, myDateTimeTable, _dataFactory);

            ///////////////////////把值放入DataTable中//////////////////////
            for (int i = 0; i < myTagItemsInfoTable.Rows.Count; i++)
            {
                string m_Id = myTagItemsInfoTable.Rows[i]["ItemId"].ToString();
                string m_OrganizationId = myTagItemsInfoTable.Rows[i]["OrganizationID"].ToString();
                string m_ValueType = myTagItemsInfoTable.Rows[i]["ValueType"].ToString();
                string m_CaculateType = myTagItemsInfoTable.Rows[i]["CaculateType"].ToString();
                decimal m_Value = 0.0m;
                foreach (Model_CaculateItems m_CaculateValueTemp in m_CaculateItems.Values)
                {
                    if (m_CaculateValueTemp.OrganizaitonId == m_OrganizationId && m_CaculateValueTemp.ValueType == m_ValueType && m_CaculateValueTemp.Type == m_CaculateType)
                    {
                        for (int j = 0; j < m_CaculateValueTemp.CaculateItemDetail.Count; j++)
                        {
                            if (m_CaculateValueTemp.CaculateItemDetail[j].Id == m_Id + "Add")
                            {
                                m_Value = m_Value + m_CaculateValueTemp.CaculateItemDetail[j].CaculateValue;
                            }
                            else if (m_CaculateValueTemp.CaculateItemDetail[j].Id == m_Id + "Sub")
                            {
                                m_Value = m_Value - m_CaculateValueTemp.CaculateItemDetail[j].CaculateValue;
                            }
                        }
                    }
                }
                myTagItemsInfoTable.Rows[i]["Value"] = m_Value;
            }
        }
        public static string GetChartDataJson(DataTable myChartValueTable)
        {
            string m_ReturnJson = "";
            for (int i = 0; i < myChartValueTable.Rows.Count; i++)
            {
                string m_JsonTemplate = "{\"max\":{0},\"min\":{1},\"value\":{2},\"intervals\":[{3}],\"label\":\"{4}({5})\"}";
                string m_MaxScale = myChartValueTable.Rows[i]["Max"] != DBNull.Value ? ((decimal)myChartValueTable.Rows[i]["Max"]).ToString("0.00") : "100";
                m_JsonTemplate = m_JsonTemplate.Replace("{0}", m_MaxScale);
                m_JsonTemplate = m_JsonTemplate.Replace("{1}", myChartValueTable.Rows[i]["Min"] != DBNull.Value ? ((decimal)myChartValueTable.Rows[i]["Min"]).ToString("0.00") : "0");
                m_JsonTemplate = m_JsonTemplate.Replace("{2}", myChartValueTable.Rows[i]["Value"] != DBNull.Value ? ((decimal)myChartValueTable.Rows[i]["Value"]).ToString("0.00") : "0.00");
                string m_IntervalValues = m_MaxScale;
                m_IntervalValues = myChartValueTable.Rows[i]["AlarmHH"] != DBNull.Value ? myChartValueTable.Rows[i]["AlarmHH"].ToString() + "," + m_IntervalValues : m_IntervalValues;
                m_IntervalValues = myChartValueTable.Rows[i]["AlarmH"] != DBNull.Value ? myChartValueTable.Rows[i]["AlarmH"].ToString() + "," + m_IntervalValues : m_IntervalValues;

                m_JsonTemplate = m_JsonTemplate.Replace("{3}", m_IntervalValues);
                m_JsonTemplate = m_JsonTemplate.Replace("{4}", myChartValueTable.Rows[i]["ItemName"] != DBNull.Value ? myChartValueTable.Rows[i]["ItemName"].ToString() : "");
                m_JsonTemplate = m_JsonTemplate.Replace("{5}", myChartValueTable.Rows[i]["Unit"] != DBNull.Value ? myChartValueTable.Rows[i]["Unit"].ToString() : "");

                if (i == 0)
                {
                    m_ReturnJson = m_JsonTemplate;
                }
                else
                {
                    m_ReturnJson = m_ReturnJson + "," + m_JsonTemplate;
                }

            }
            return "[" + m_ReturnJson + "]";
        }
    }
}
