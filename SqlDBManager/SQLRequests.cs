using NotesNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDBManager
{
    public static class SQLRequests
    {
        // таблицы на обработку
        public static string ProcessingRequest(string catalog)
        {
            return $"SELECT name FROM [{catalog}].sys.tables WHERE name in ('eqUsers', 'rptFUND_PAPER', 'rptFUND_UNIT_REG_STATS', 'tblACT_TYPE_CL', 'tblAuthorizedDep', 'tblCLS', 'tblDataExport', 'tblDECL_COMMISSION_CL', 'tblConstantsSpec', 'tblGROUPING_ATTRIBUTE_CL', 'tblINV_REQUIRED_WORK_CL', 'tblLANGUAGE_CL', 'tblFEATURE', 'tblCITIZEN_CL', 'tblORGANIZ_CL', 'tblPAPER_CLS', 'tblPAPER_CLS_INV', 'tblPUBLICATION_TYPE_CL', 'tblQUESTION', 'tblRECEIPT_REASON_CL', 'tblRECEIPT_SOURCE_CL', 'tblREF_FILE', 'tblREPRODUCTION_METHOD_CL', 'tblService', 'tblSTATE_CL', 'tblSTORAGE_MEDIUM_CL', 'tblSUBJECT_CL', 'tblTREE_SUPPORT', 'tblWORK_CL') ORDER BY name;";
        }

        // Таблицы на скип
        public static string SkipRequest(string catalog)
        {
            return $"SELECT name FROM [{catalog}].sys.tables WHERE OBJECTPROPERTY(object_id, 'TableHasForeignKey') = 0 and name not like '%log' and name not in ('eqUsers', 'rptFUND_PAPER', 'rptFUND_UNIT_REG_STATS', 'tblACT_TYPE_CL', 'tblAuthorizedDep', 'tblCLS', 'tblDataExport', 'tblDECL_COMMISSION_CL', 'tblConstantsSpec', 'tblGROUPING_ATTRIBUTE_CL', 'tblINV_REQUIRED_WORK_CL', 'tblLANGUAGE_CL', 'tblFEATURE', 'tblCITIZEN_CL', 'tblORGANIZ_CL', 'tblPAPER_CLS', 'tblPAPER_CLS_INV', 'tblPUBLICATION_TYPE_CL', 'tblQUESTION', 'tblRECEIPT_REASON_CL', 'tblRECEIPT_SOURCE_CL', 'tblREF_FILE', 'tblREPRODUCTION_METHOD_CL', 'tblService', 'tblSTATE_CL', 'tblSTORAGE_MEDIUM_CL', 'tblSUBJECT_CL', 'tblTREE_SUPPORT', 'tblWORK_CL', 'tblPUBLICATION_CL', 'tblUNIT_FOTO_EX', 'tblUNIT_MOVIE_EX', 'tblUNIT_VIDEO_EX') or name in ('tblDOC_KIND_CL', 'tblABSENCE_REASON_CL') ORDER BY name;";
        }

        // Запрос количества таблиц в каталоге
        public static string CountTablesRequest(string catalog)
        {
            return $"SELECT COUNT(TABLE_NAME) FROM [{catalog}].INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
        }

        // Запрос наименований всех таблиц
        public static string AllTablesRequest(string catalog)
        {
            return $"SELECT TABLE_NAME FROM [{catalog}].INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME";
        }

        // Запрос таблицы содержащие логи
        public static string LogTablesRequest(string catalog)
        {
            return $"SELECT TABLE_NAME FROM [{catalog}].INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' and TABLE_NAME like '%log' ORDER BY TABLE_NAME";
        }

        // Запрос на полную очистку таблицы
        public static string ClearTableRequest(string catalog, string table)
        {
            return $"DELETE [{catalog}].[dbo].[{table}]";
        }

        // Запрос количества значений в таблице
        public static string CountRowsRequest(string catalog, string table)
        {
            return $"SELECT COUNT(*) FROM [{catalog}].[dbo].[{table}]";
        }

        // Запрос на обновление ID
        public static string UpdateIDRequest(string catalog, string table, string filterColumn, string filterValue)
        {
            return $"UPDATE [{catalog}].[dbo].[{table}] SET ID = NEWID() WHERE {filterColumn} = '{filterValue}'";
        }

        // Запрос на добавление записи
       /* public static string InsertRequest(string catalog, string table)
        {
            return $"INSERT INTO [{catalog}].[dbo].[{table}](some_columns) VALUES()";
        }*/

        public static string InsertFromRequest(string inCatalog, string inTable, List<string> columns, string fromCatalog, string fromTable, string filterColumn, string filterValue)
        {
            return $"INSERT INTO [{inCatalog}].[dbo].[{inTable}] SELECT {string.Join(", ", columns).Replace('\"', '\'')} FROM [{fromCatalog}].[dbo].[{fromTable}] WHERE {filterColumn} = '{filterValue}'";
        }

        public static string SelectWhereRequest(string catalog, string table, string result)
        {
            return $"SELECT column FROM FROM [{catalog}].[dbo].[{table}] WHERE column_from_table = '{result}'";
        }

        // Запрос на получение опредленных колонок из таблицы
        public static string ColumnsDataRequest(List<string> columns, string catalog, string tableName)
        {
            return $"SELECT {string.Join(", ", columns)} FROM [{catalog}].[dbo].[{tableName}]";
        }

        // Получение наименований столбцов переданной таблицы
        public static string ColumnsNamesRequest(string catalog, string tableName)
        {
            return $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_CATALOG = '{catalog}' and TABLE_SCHEMA = 'dbo' and TABLE_NAME = '{tableName}'";
        }

        public static string OneColumnRequest(string column, string catalog, string table)
        {
            return $"SELECT {column} FROM [{catalog}].[dbo].[{table}]";
        }
        
    }
}
