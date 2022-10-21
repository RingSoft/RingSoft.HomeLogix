using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.EfCore;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;

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

        public static bool CopyData(LookupContext lookupContext, DbDataProcessor destinationDataProcessor, ITwoTierProcedure procedure)
        {
            var tables = lookupContext.TableDefinitions.OrderBy(p => p.PriorityLevel);
            var tableCount = tables.Count();
            var tableItem = 0;
            var pageSize = 100;
            foreach (var table in tables)
            {
                tableItem++;
                procedure.UpdateTopTier($"Processing Table {table.Description} {tableItem}/{tableCount}", tableCount, tableItem);
                var count = 0;
                var selectQuery = new SelectQuery(table.TableName);
                var countQuery = new CountQuery(selectQuery, "Count");
                var countResult = lookupContext.DataProcessor.GetData(countQuery, true, false);
                if (countResult.ResultCode == GetDataResultCodes.Success)
                {
                    count = countResult.DataSet.Tables[0].Rows[0].GetRowValue("Count").ToInt();
                }
                else
                {
                    procedure.ShowError(countResult.Message, "Database Connection Error");
                    return false;
                }
                var processingRecord = 0;
                var chunk = table.GetChunk(pageSize);
                if (chunk.Chunk.Rows.Count >= pageSize)
                {
                    while (chunk.Chunk.Rows.Count >= pageSize)
                    {
                        processingRecord += chunk.Chunk.Rows.Count;
                        procedure.UpdateBottomTier($"Processing Record {processingRecord}/{count}", count, processingRecord);
                        ProcessChunk(destinationDataProcessor, chunk, table);
                        chunk = table.GetChunk(pageSize, chunk.BottomPrimaryKey);
                        if (chunk.Chunk.Rows.Count < pageSize)
                        {
                            processingRecord += chunk.Chunk.Rows.Count;
                            procedure.UpdateBottomTier($"Processing Record {processingRecord}/{count}", count, processingRecord);
                            ProcessChunk(destinationDataProcessor, chunk, table);
                        }
                    }
                }
                else if (chunk.Chunk.Rows.Count > 0)
                {
                    processingRecord += chunk.Chunk.Rows.Count;
                    procedure.UpdateBottomTier($"Processing Record {processingRecord}/{count}", count, processingRecord);
                    ProcessChunk(destinationDataProcessor, chunk, table);
                }
            }
            return true;
        }

        private static void ProcessChunk(DbDataProcessor destinationDataProcessor, ChunkResult chunk, TableDefinitionBase table)
        {
            var sqlList = new List<string>();
            if (table.PrimaryKeyFields.Count == 1 && table.PrimaryKeyFields[0].FieldDataType == FieldDataTypes.Integer)
            {
                var sql = destinationDataProcessor.GetIdentityInsertSql(table.TableName);
                if (!sql.IsNullOrEmpty())
                {
                    sqlList.Add(sql);
                }
            }
            
            foreach (DataRow chunkRow in chunk.Chunk.Rows)
            {
                var insertFields = string.Empty;
                var values = string.Empty;
                foreach (var fieldDefinition in table.FieldDefinitions)
                {
                    var dateType = DbDateTypes.DateOnly;
                    if (fieldDefinition is DateFieldDefinition dateFieldDefinition)
                    {
                        dateType = dateFieldDefinition.DateType;
                    }
                    insertFields +=
                        $"{destinationDataProcessor.SqlGenerator.FormatSqlObject(fieldDefinition.FieldName)}, ";
                    values +=
                        $"{destinationDataProcessor.SqlGenerator.ConvertValueToSqlText(chunkRow.GetRowValue(fieldDefinition.FieldName), fieldDefinition.ValueType, dateType)}, ";
                }

                insertFields = insertFields.LeftStr(insertFields.Length - 2);
                values = values.LeftStr(values.Length - 2);
                var sql =
                    $"INSERT INTO {destinationDataProcessor.SqlGenerator.FormatSqlObject(table.TableName)} ({insertFields}) VALUES ({values})";
                sqlList.Add(sql);
            }

            var result = destinationDataProcessor.ExecuteSqls(sqlList, true);
        }
    }
}
