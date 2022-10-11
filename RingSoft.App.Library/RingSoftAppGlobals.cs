using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using RingSoft.DbLookup;
using RingSoft.DbLookup.DataProcessor;

namespace RingSoft.App.Library
{
    public enum DbPlatforms
    {
        Sqlite = 0,
        SqlServer = 1,
        MySql = 2,
    }
    public class RingSoftAppGlobals
    {
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static string AppTitle { get; set; }

        public static string AppVersion { get; set; }

        public static string AppCopyright { get; set; }

        public static double CalculateMonthsInTimeSpan(DateTime startDate, DateTime endDate)
        {
            var result = endDate.Subtract(startDate).Days / (365.25 / 12);
            return result;
        }

        public static List<string> GetSqlServerDatabaseList(string serverName)
        {
            var dbList = new List<string>();
            var databaseProcessor = new SqlServerDataProcessor();
            databaseProcessor.Server = serverName;
            var result = databaseProcessor.GetListOfDatabases();
            if (result.ResultCode == GetDataResultCodes.Success)
            {
                var dataTable = result.DataSet.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    var text = dataRow.GetRowValue(dataTable.Columns[0].ColumnName);
                    dbList.Add(text);
                }

                return dbList.OrderBy(o => o).ToList();
            }

            return new List<string>();

        }
    }
}
