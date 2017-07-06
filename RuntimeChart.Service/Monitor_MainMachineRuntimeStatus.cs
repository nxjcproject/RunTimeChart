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
                              ,A.DisplayIndex
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
            //余热发电转速模拟量
            ServiceReference_RealTimeTagValue.AnalogDataGroup_Serialization m_AnalogDataGroup_Serialization = realTimeDataSoapClientTest.GetAnalogDataA(organizationId, boolTagArray, "HTKJ2016_#*?");
            foreach (ServiceReference_RealTimeTagValue.DigitalDataItem_Serialization item in m_DigitalDataGroup_Serialization.DataSet)
            {
                tagDataDic[organizationId + ">" + item.ID] = item.Value;
            }
            //余热发电转速模拟量
            foreach (ServiceReference_RealTimeTagValue.AnalogDataItem_Serialization item in m_AnalogDataGroup_Serialization.DataSet)
            {
                tagDataDic[organizationId + ">" + item.ID] = item.Value > 300 ? true : false;
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
        /// <summary>
        /// 余热发电标签直接写死
        /// </summary>
        /// <param name="myEquipmentParameters"></param>
        public static DataTable GetParametersCogeneration(DataTable myEquipmentParameters)
        {
            if (myEquipmentParameters != null)
            {
                DataRow[] m_EquipmentParametersDataRows = myEquipmentParameters.Select("ProductionLineType <> '余热发电'");
                DataTable m_EquipmentParametersDataTable = m_EquipmentParametersDataRows.CopyToDataTable();

                m_EquipmentParametersDataTable.Rows.Add("Cogeneration01", "1#余热发电", "Generator", "zc_nxjc_byc_byf", "zc_nxjc_byc_byf_cogeneration01", "", "O030101", "1#余热发电", "余热发电", "O03", "白银公司", "F_505SEA_R", "zc_nxjc_byc_byf_dcs02", "ProcessVariable01", "zc_nxjc_byc_byf", 0, 901);
                m_EquipmentParametersDataTable.Rows.Add("Cogeneration02", "1#余热发电", "Generator", "zc_nxjc_ychc_yfcf", "zc_nxjc_ychc_yfcf_cogeneration01", "", "O040106", "1#余热发电", "余热发电", "O04", "银川公司", "TSIS01A_R", "zc_nxjc_ychc_yfcf_dcs03", "ProcessVariable05", "zc_nxjc_ychc_yfcf", 0, 901);
                m_EquipmentParametersDataTable.Rows.Add("Cogeneration03", "2#余热发电", "Generator", "zc_nxjc_ychc_lsf", "zc_nxjc_ychc_lsf_cogeneration02", "", "O040306", "2#余热发电", "余热发电", "O04", "银川公司", "TSIS01A_R", "zc_nxjc_ychc_lsf_dcs05", "ProcessVariable04", "zc_nxjc_ychc_lsf", 0, 902);
                m_EquipmentParametersDataTable.Rows.Add("Cogeneration04", "3#余热发电", "Generator", "zc_nxjc_ychc_lsf", "zc_nxjc_ychc_lsf_cogeneration03", "", "O040307", "3#余热发电", "余热发电", "O04", "银川公司", "F_4TSIS01A_R", "zc_nxjc_ychc_lsf_dcs06", "ProcessVariable01", "zc_nxjc_ychc_lsf", 0, 903);
                m_EquipmentParametersDataTable.Rows.Add("Cogeneration05", "1#余热发电", "Generator", "zc_nxjc_tsc_tsf", "zc_nxjc_tsc_tsf_cogeneration01", "", "O050105", "1#余热发电", "余热发电", "O05", "天水公司", "F_120TR1SE1_R", "zc_nxjc_tsc_tsf_dcs03", "ProcessVariable02", "zc_nxjc_tsc_tsf", 0, 901);
                m_EquipmentParametersDataTable.Rows.Add("Cogeneration06", "1#余热发电", "Generator", "zc_nxjc_klqc_klqf", "zc_nxjc_klqc_klqf_cogeneration01", "", "O070105", "1#余热发电", "余热发电", "O07", "喀喇沁公司", "F_505SEA_R", "zc_nxjc_klqc_klqf_dcs02", "ProcessVariable05", "zc_nxjc_klqc_klqf", 0, 901);
                m_EquipmentParametersDataTable.Rows.Add("Cogeneration07", "1#余热发电", "Generator", "zc_nxjc_whsmc_whsmf", "zc_nxjc_whsmc_whsmf_cogeneration01", "", "O100103", "1#余热发电", "余热发电", "O10", "乌海赛马公司", "F_23SI04_03_R", "zc_nxjc_whsmc_whsmf_dcs02", "ProcessVariable04", "zc_nxjc_whsmc_whsmf", 0, 901);
                m_EquipmentParametersDataTable.Rows.Add("Cogeneration08", "1#余热发电", "Generator", "zc_nxjc_qtx_efc", "zc_nxjc_qtx_efc_cogeneration01", "", "O020102", "1#余热发电", "余热发电", "O02", "青铜峡公司", "TSIA03_AI", "Db_02_01_Cogeneration01", "ProcessVariable04", "Db_02_01", 0, 901);
                m_EquipmentParametersDataTable.Rows.Add("Cogeneration09", "2#余热发电", "Generator", "zc_nxjc_qtx_efc", "zc_nxjc_qtx_efc_cogeneration02", "", "O020104", "2#余热发电", "余热发电", "O02", "青铜峡公司", "AI1_TSE03", "Db_02_01_Cogeneration02", "ProcessVariable02", "Db_02_01", 0, 902);
                m_EquipmentParametersDataTable.Rows.Add("Cogeneration10", "3#余热发电", "Generator", "zc_nxjc_qtx_tys", "zc_nxjc_qtx_tys_cogeneration03", "", "O020202", "3#余热发电", "余热发电", "O02", "青铜峡公司", "TSIS01A_R", "zc_nxjc_qtx_tys_dcs03", "ProcessVariable01", "zc_nxjc_qtx_tys", 0, 903);
                m_EquipmentParametersDataTable.Rows.Add("Cogeneration11", "4#余热发电", "Generator", "zc_nxjc_qtx_tys", "zc_nxjc_qtx_tys_cogeneration04", "", "O020204", "4#余热发电", "余热发电", "O02", "青铜峡公司", "F_2TSE03_R", "zc_nxjc_qtx_tys_dcs04", "ProcessVariable01", "zc_nxjc_qtx_tys", 0, 904);
                
                DataView m_EquipmentParametersDataView = m_EquipmentParametersDataTable.DefaultView;
                m_EquipmentParametersDataView.Sort = "CompanyLevelCode, ProductionLevelCode, DisplayIndex";
                return m_EquipmentParametersDataView.ToTable();
            }
            else
            {
                return myEquipmentParameters;
            }
        }
    }
}
