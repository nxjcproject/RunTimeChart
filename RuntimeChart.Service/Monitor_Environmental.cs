using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SqlServerDataAdapter;
using RuntimeChart.Infrastructure.Configuration;
namespace RuntimeChart.Service
{
    public class Monitor_Environmental
    {
        private static readonly string _connStr = ConnectionStringFactory.NXJCConnectionString;
        private static readonly ISqlServerDataFactory _dataFactory = new SqlServerDataFactory(_connStr);
        public static string GetOrganizationTree(string[] myOrganizationIdArray)
        {
            string m_OrganizationString = "";
            string m_Sql = @"Select A.OrganizationID, A.Name, A.LevelCode, A.Type, A.LevelType from system_Organization A, system_Organization B
                                where B.OrganizationID in ({0})
                                and (A.LevelCode like B.LevelCode + '%' or CHARINDEX(A.LevelCode, B.LevelCode) > 0)
                                and A.LevelType = 'Company'
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
                    //m_OrganizationTable.Rows.Add(new string[]{"All","全部","O00","","Company"});
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
    }
}
