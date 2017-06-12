using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SqlServerDataAdapter;
using RuntimeChart.Infrastructure.Configuration;

namespace RuntimeChart.Service
{
    public class Monitor_MainMachineRuntimeStatus
    {
        private static readonly string _stationId = WebConfigurations.StationId;
        private static readonly string _connStr = ConnectionStringFactory.NXJCConnectionString;
        private static readonly ISqlServerDataFactory _dataFactory = new SqlServerDataFactory(_connStr);
        /// <summary>
        /// 获得设备参数表,用来为前台框架提供必要的信息
        /// </summary>
        /// <param name="myEquipmentCommonId">设备大类型ID</param>
        /// <param name="myOrganizationIdArray">权限组织机构列表</param>
        /// <returns>设备参数表</returns>
        public static DataTable GetEquipmentParameters(string myEquipmentCommonId, string[] myOrganizationIdArray)
        {
            string m_OrganizationString = "";
            string m_EquipmentCommonIdString = "";
            string m_StationString = "";
            string m_Sql = @"SELECT A.EquipmentId
                              ,replace(A.EquipmentName,'号','#') as EquipmentName
                              ,A.EquipmentCommonId
	                          ,A.OrganizationID as FactoryOrganizationId
                              ,C.OrganizationID as OrganizationId
                              ,A.Specifications
                              ,D.LevelCode as ProductionLevelCode
	                          ,replace(replace(replace(replace(D.Name,'号','#'),'窑',''),'熟料',''),'线','') as ProductionLineName
                              ,D.Type as ProductionLineType
	                          ,F.LevelCode as CompanyLevelCode
	                          ,F.Name as CompanyName
	                          ,C.VariableName
	                          ,C.DataBaseName
	                          ,C.TableName
	                          ,M.MeterDatabase
	                          ,C.ValidValues
                          FROM equipment_EquipmentDetail A, equipment_EquipmentCommonInfo B, system_MasterMachineDescription C, system_Organization D, system_Database M, system_Organization E
                          left join system_Organization F on CHARINDEX(E.LevelCode, F.LevelCode) > 0 and F.LevelType = 'Company'
                          where A.Enabled = 1
                          and A.EquipmentCommonId = B.EquipmentCommonId
                          and B.EquipmentCommonId in ({0})
                          and A.EquipmentId = C.ID
                          and A.ProductionLineId = D.OrganizationID
                          and E.OrganizationID in ({1})
                          {2}
                          and D.DatabaseID = M.DatabaseID
                          and D.LevelCode like E.LevelCode + '%'
                          order by F.LevelCode, D.LevelCode, A.DisplayIndex";
            if (myOrganizationIdArray != null && myEquipmentCommonId != "")
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
                string[] m_EquipmentCommonIdArray = myEquipmentCommonId.Split(',');
                for (int i = 0; i < m_EquipmentCommonIdArray.Length; i++)
                {
                    if (i == 0)
                    {
                        m_EquipmentCommonIdString = "'" + m_EquipmentCommonIdArray[i] + "'";
                    }
                    else
                    {
                        m_EquipmentCommonIdString = m_EquipmentCommonIdString + ",'" + m_EquipmentCommonIdArray[i] + "'";
                    }
                }
                if (_stationId != "zc_nxjc" && _stationId != "")
                {
                    m_StationString = string.Format(" and E.OrganizationID = '{0}' ", _stationId);
                }
                m_Sql = string.Format(m_Sql, m_EquipmentCommonIdString, m_OrganizationString, m_StationString);
                try
                {
                    DataTable m_EquipmentParametersTable = _dataFactory.Query(m_Sql);
                    return m_EquipmentParametersTable;
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 根据设备参数获得设备运行标签
        /// </summary>
        /// <param name="myEquipmentParametersTable">设备参数表</param>
        /// <returns>设备标签表</returns>
        public static string GetEquipmentRunTags(DataTable myEquipmentParametersTable)
        {
            DataTable m_EquipmentRunTagsTable = GetEquipmentRunTagsTable();
            if (myEquipmentParametersTable != null)
            {
                string m_DataBaseName = "";
                string m_MeterDataBase = "";
                string m_VariableNamePrefix = "";
                for (int i = 0; i < myEquipmentParametersTable.Rows.Count; i++)
                {
                    string m_DataBaseNameTemp = myEquipmentParametersTable.Rows[i]["DataBaseName"].ToString();
                    string m_MeterDataBaseTemp = myEquipmentParametersTable.Rows[i]["MeterDatabase"].ToString();
                    ///////////////通过比较获得DCS标签的前缀/////////////////
                    if (m_DataBaseName != m_DataBaseNameTemp || m_MeterDataBase != m_MeterDataBaseTemp)
                    {
                        m_VariableNamePrefix = GetVariableNamePrefix(m_MeterDataBaseTemp, m_DataBaseNameTemp);
                        m_DataBaseName = m_DataBaseNameTemp;
                        m_MeterDataBase = m_MeterDataBaseTemp;
                    }

                    /////////相关信息添加到新表/////////////
                    DataRow m_NewRowTemp = m_EquipmentRunTagsTable.NewRow();
                    m_NewRowTemp["ItemId"] = myEquipmentParametersTable.Rows[i]["FactoryOrganizationId"].ToString() + ">"
                                          + m_VariableNamePrefix + "_" + myEquipmentParametersTable.Rows[i]["VariableName"].ToString();
                    m_NewRowTemp["EquipmentId"] = myEquipmentParametersTable.Rows[i]["EquipmentId"].ToString().ToLower();
                    m_NewRowTemp["EquipmentName"] = myEquipmentParametersTable.Rows[i]["EquipmentName"].ToString();
                    m_NewRowTemp["EquipmentCommonId"] = myEquipmentParametersTable.Rows[i]["EquipmentCommonId"].ToString();
                    m_NewRowTemp["FactoryOrganizationId"] = myEquipmentParametersTable.Rows[i]["FactoryOrganizationId"].ToString();
                    m_NewRowTemp["OrganizationId"] = myEquipmentParametersTable.Rows[i]["OrganizationId"].ToString();
                    m_NewRowTemp["ProductionLineName"] = myEquipmentParametersTable.Rows[i]["ProductionLineName"].ToString();
                    m_NewRowTemp["ProductionLineType"] = myEquipmentParametersTable.Rows[i]["ProductionLineType"].ToString();
                    m_NewRowTemp["CompanyName"] = myEquipmentParametersTable.Rows[i]["CompanyName"].ToString();
                    m_NewRowTemp["VariableName"] = m_VariableNamePrefix + "_" + myEquipmentParametersTable.Rows[i]["VariableName"].ToString();
                    m_NewRowTemp["ValidValues"] = myEquipmentParametersTable.Rows[i]["ValidValues"].ToString();
                    m_EquipmentRunTagsTable.Rows.Add(m_NewRowTemp);
                }
            }
            string m_ReturnValue = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(m_EquipmentRunTagsTable);
            return m_ReturnValue;
        }
        private static DataTable GetEquipmentRunTagsTable()
        {
            DataTable m_EquipmentRunTagsTable = new DataTable();
             m_EquipmentRunTagsTable.Columns.Add("ItemId", typeof(string));
             m_EquipmentRunTagsTable.Columns.Add("EquipmentId",typeof(string));
             m_EquipmentRunTagsTable.Columns.Add("EquipmentName",typeof(string));
             m_EquipmentRunTagsTable.Columns.Add("EquipmentCommonId", typeof(string));
             m_EquipmentRunTagsTable.Columns.Add("FactoryOrganizationId", typeof(string));
             m_EquipmentRunTagsTable.Columns.Add("OrganizationId", typeof(string));
             m_EquipmentRunTagsTable.Columns.Add("ProductionLineName",typeof(string));
             m_EquipmentRunTagsTable.Columns.Add("ProductionLineType", typeof(string));
             m_EquipmentRunTagsTable.Columns.Add("CompanyName",typeof(string));
             m_EquipmentRunTagsTable.Columns.Add("VariableName",typeof(string));
             m_EquipmentRunTagsTable.Columns.Add("ValidValues",typeof(bool));
            return m_EquipmentRunTagsTable;
        }
        private static string GetVariableNamePrefix(string myMeterDataBase, string myDataBaseName)
        {
            string m_Sql = @"SELECT top 1 A.DCSName
                                FROM {0}.dbo.View_DCSContrast A
                                where A.DBName = '{1}'";
            if (myDataBaseName != "")
            {
                m_Sql = string.Format(m_Sql, myMeterDataBase, myDataBaseName);
                try
                {
                    DataTable m_VariableNamePrefixTable = _dataFactory.Query(m_Sql);
                    if (m_VariableNamePrefixTable != null && m_VariableNamePrefixTable.Rows.Count > 0)
                    {
                        return m_VariableNamePrefixTable.Rows[0]["DCSName"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                catch
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        public static void GetBooleanResult(string organizationId, string[] tagArray, ref Dictionary<string, bool> tagDataDic)
        {
            ServiceReference_RealTimeTagValue.RealTimeDataSoapClient realTimeDataSoapClientTest = new ServiceReference_RealTimeTagValue.RealTimeDataSoapClient();
            //数据字典
            ServiceReference_RealTimeTagValue.ArrayOfString boolTagArray = new ServiceReference_RealTimeTagValue.ArrayOfString();
            foreach (string tag in tagArray)
            {
                if (!tagDataDic.Keys.Contains(organizationId + ">" + tag))
                {
                    tagDataDic.Add(organizationId + ">" + tag, false);//默认为false
                    boolTagArray.Add(tag);
                }
            }
            ServiceReference_RealTimeTagValue.DigitalDataGroup_Serialization m_DigitalDataGroup_Serialization = realTimeDataSoapClientTest.GetDigitalDataA(organizationId, boolTagArray, "HTKJ2016_#*?");
            foreach (ServiceReference_RealTimeTagValue.DigitalDataItem_Serialization item in m_DigitalDataGroup_Serialization.DataSet)
            {
                tagDataDic[organizationId + ">" + item.ID] = item.Value;
            }
        }
        public static Dictionary<string, bool> GetEquipmentHaltStatus(string myTags)
        {
            Dictionary<string, string> m_TagsDic = new Dictionary<string, string>();
            Dictionary<string, bool> m_TagsValue = new Dictionary<string, bool>();
            if (myTags != "")
            {
                string[] m_TagsGroup = myTags.Split(';');
                for (int i = 0; i < m_TagsGroup.Length; i++)
                {
                    string[] m_TagInfo = m_TagsGroup[i].Split(',');
                    string m_DicKeyTemp = m_TagInfo[0] + m_TagInfo[1].ToLower();
                    if (!m_TagsDic.ContainsKey(m_DicKeyTemp))
                    {
                        m_TagsDic.Add(m_DicKeyTemp, m_TagInfo[2]);
                    }
                    else
                    {
                        m_TagsDic[m_DicKeyTemp] = m_TagInfo[2];
                    }
                }
                DataTable m_MachineHaltTable = GetMachineHaltTable(m_TagsDic.Keys.ToArray());
                if (m_MachineHaltTable != null)
                {
                    for (int i = 0; i < m_MachineHaltTable.Rows.Count; i++)
                    {
                        string m_ItemIdTemp = m_MachineHaltTable.Rows[i]["ItemId"].ToString().ToLower();
                        if (m_TagsDic.ContainsKey(m_ItemIdTemp))        //如果字典里有该停机记录
                        {
                            if (!m_TagsValue.ContainsKey(m_ItemIdTemp))
                            {
                                m_TagsValue.Add(m_TagsDic[m_ItemIdTemp], true);
                            }
                            else
                            {
                                m_TagsValue[m_TagsDic[m_ItemIdTemp]] = true;
                            }
                        }
                    }
                }
            }
            return m_TagsValue;
        }
        private static DataTable GetMachineHaltTable(string[] myEquipmentKeyId)
        {
            string m_Sql = @"SELECT A.OrganizationID + convert(varchar(64), A.EquipmentID) as ItemId
                              ,count(0) as value
                          FROM shift_MachineHaltLog A
                          where (A.HaltTime >= '{1}' or A.StartTime >= '{1}')
                          and A.OrganizationID + convert(varchar(64), A.EquipmentID) in ({0})
                          group by A.OrganizationID + convert(varchar(64), A.EquipmentID)";
            string m_Condition = "''";
            for (int i = 0; i < myEquipmentKeyId.Length; i++)
            {
                if (i == 0)
                {
                    m_Condition = "'" + myEquipmentKeyId[i] + "'";
                }
                else
                {
                    m_Condition = m_Condition + ",'" + myEquipmentKeyId[i] + "'";
                }
            }
            m_Sql = string.Format(m_Sql, m_Condition, DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd HH:mm:ss"));
            try
            {
                DataTable m_MachineHaltRecordTable = _dataFactory.Query(m_Sql);
                return m_MachineHaltRecordTable;
            }
            catch
            {
                return null;
            }
        }
        public static string GetMachineHaltRecord(string myOrganizationId, string myEquipmentId)
        {
            string m_ReturnString = "{\"rows\":[],\"total\":0}";
            string m_Sql = @"select B.MachineHaltLogID
                                ,B.HaltTime
                                ,B.RecoverTime
                                ,convert(varchar(32),floor(B.HaltLong / (24 * 60))) + '天' + convert(varchar(32),floor((B.HaltLong % (24 * 60)) / 60)) + '时' + convert(varchar(32),floor(B.HaltLong % 60)) + '分' as HaltLong
                                ,B.ReasonText
                                from 
                                (SELECT top 10 A.MachineHaltLogID
                                      ,A.HaltTime
                                      ,A.RecoverTime
	                                  ,(case when A.RecoverTime is not null then datediff(Minute, A.HaltTime, A.RecoverTime) 
	                                       else datediff(Minute, A.HaltTime, getDate()) end) as HaltLong
                                      ,A.ReasonText
                                  FROM shift_MachineHaltLog A, equipment_EquipmentDetail B
                                  where A.EquipmentID = '{1}'
                                  and A.HaltTime is not null
                                  and A.EquipmentID = B.EquipmentId
                                  and B.OrganizationID = '{0}'
                                  order by A.HaltTime desc) B";
            m_Sql = string.Format(m_Sql, myOrganizationId, myEquipmentId);
            try
            {
                DataTable m_MachineHaltRecordTable = _dataFactory.Query(m_Sql);
                if (m_MachineHaltRecordTable != null)
                {
                    m_ReturnString = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(m_MachineHaltRecordTable);
                }
                return m_ReturnString;
            }
            catch
            {
                return m_ReturnString;
            }
        }
    }
}
