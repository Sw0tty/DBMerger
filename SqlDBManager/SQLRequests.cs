using NotesNamespace;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        /// <summary>
        /// Запрос очистики таблицы
        /// </summary>
        public static string ClearTableRequest(string catalog, string table)
        {
            return $"DELETE [{catalog}].[dbo].[{table}]";
        }

        /// <summary>
        /// Запрос удаления определенной строки
        /// </summary>
        public static string DeleteRowRequest(string catalog, string tableName, string filterColumn, string filterValue)
        {
            return $"DELETE [{catalog}].[dbo].[{tableName}] WHERE {filterColumn} = {filterValue}";
        }

        /// <summary>
        /// Возвращает значение последней записи в переданной таблице
        /// </summary>
        public static string LastInsertRecordRequest(string catalog, string columns, string tableName, string orderByColumn)
        {
            return $"SELECT TOP 1 {columns} FROM [{catalog}].[dbo].[{tableName}] ORDER BY {orderByColumn} DESC";
        }

        /// <summary>
        /// Возвращает количество значений в переданной таблице
        /// </summary>
        public static string CountRowsRequest(string catalog, string table)
        {
            return $"SELECT COUNT(*) FROM [{catalog}].[dbo].[{table}]";
        }

        // Запрос на обновление ID
        public static string UpdateIDRequest(string catalog, string table, string filterColumn, string filterValue)
        {
            return $"UPDATE [{catalog}].[dbo].[{table}] SET ID = NEWID() WHERE {filterColumn} = '{filterValue}'";
        }

        public static string UpdateRowRequest(string catalog, string tableName, string updateColumn, string updateValue, string filterColumn, string filterValue)
        {
            return $"UPDATE [{catalog}].[dbo].[{tableName}] SET {updateColumn} = {updateValue} WHERE {filterColumn} = {filterValue}";
        }

        public static string InsertFromRequest(string inCatalog, string inTable, List<string> columns, string fromCatalog, string fromTable, string filterColumn, string filterValue)
        {
            return $"INSERT INTO [{inCatalog}].[dbo].[{inTable}] SELECT {string.Join(", ", columns)} FROM [{fromCatalog}].[dbo].[{fromTable}] WHERE {filterColumn} = '{filterValue}'";
        }

        /// <summary>
        /// Запрос на вставку записи в таблицу
        /// </summary>
        public static string InsertDictValueRequst(string catalog, string tableName, Dictionary<string, string> data, bool withoutID = false)
        {
            if (tableName == "")
            {
                return $"INSERT INTO [{catalog}].[dbo].[{tableName}](ID, {string.Join(", ", data.Keys).Replace('\"', '\'')}) VALUES (NEWID(), {string.Join(", ", data.Values).Replace("'null'", "''")})";
            }
            else if (withoutID)
            {
                return $"INSERT INTO [{catalog}].[dbo].[{tableName}]({string.Join(", ", data.Keys).Replace('\"', '\'')}) VALUES ({string.Join(", ", data.Values).Replace("'null'", "null")})";
            }
            return $"INSERT INTO [{catalog}].[dbo].[{tableName}](ID, {string.Join(", ", data.Keys).Replace('\"', '\'')}) VALUES (NEWID(), {string.Join(", ", data.Values).Replace("'null'", "null")})";
        }

        /// <summary>
        /// Запрос получения записей по переданному фильтру
        /// Фильтрация по одному параметру WHERE key = value
        /// </summary>
        public static string SelectWhereRequest(List<string> columns, string catalog, string tableName, string filterColumn, string filterValue)
        {
            if (columns.Count > 0 && !columns.Contains("Deleted"))
                return $"SELECT {string.Join(", ", columns).Replace('\"', ' ')} FROM [{catalog}].[dbo].[{tableName}] WHERE {filterColumn} = {filterValue}";
            if (columns.Count > 0 && columns.Contains("Deleted"))
                return $"SELECT {string.Join(", ", columns).Replace('\"', ' ')} FROM [{catalog}].[dbo].[{tableName}] WHERE {filterColumn} = {filterValue} and Deleted = '0'";
            if (columns.Count == 0 && columns.Contains("Deleted"))
                return $"SELECT * FROM [{catalog}].[dbo].[{tableName}] WHERE {filterColumn} = {filterValue} and Deleted = '0'";
            return $"SELECT * FROM [{catalog}].[dbo].[{tableName}] WHERE {filterColumn} = {filterValue}";
        }

        /// <summary>
        /// Запрос на получение всех записей переданной таблицы
        /// Содержит фильтрацию WHERE key IN (value, value, ...) и фильтрацию WHERE key NOT IN (value, value, ...) в зависимости от переданных параметров
        /// Опционально принимает список необходимых столбцов
        /// </summary>
        public static string AllRecordsRequest(string catalog, string tableName, Dictionary<string, List<string>> filter = null, bool filterIN = true, List<string> columns = null)
        {
            string strColumns = (columns == null) ? "*" : string.Join(", ", columns);

            if (filter != null && filterIN && !strColumns.Contains("Deleted"))
                return $"SELECT {strColumns} FROM [{catalog}].[dbo].[{tableName}] WHERE {string.Join("", filter.Keys)} IN ({string.Join(", ", filter[string.Join("", filter.Keys)])})";
            if (filter != null && filterIN && strColumns.Contains("Deleted"))
                return $"SELECT {strColumns} FROM [{catalog}].[dbo].[{tableName}] WHERE {string.Join("", filter.Keys)} IN ({string.Join(", ", filter[string.Join("", filter.Keys)])}) and Deleted = '0'";
            if (filter != null && !filterIN && !strColumns.Contains("Deleted"))
                return $"SELECT {strColumns} FROM [{catalog}].[dbo].[{tableName}] WHERE {string.Join("", filter.Keys)} NOT IN ({string.Join(", ", filter[string.Join("", filter.Keys)])})";
            if (filter != null && !filterIN && strColumns.Contains("Deleted"))
                return $"SELECT {strColumns} FROM [{catalog}].[dbo].[{tableName}] WHERE {string.Join("", filter.Keys)} NOT IN ({string.Join(", ", filter[string.Join("", filter.Keys)])}) and Deleted = '0'";
            return $"SELECT {strColumns} FROM [{catalog}].[dbo].[{tableName}]";
        }

        // Получение наименований столбцов переданной таблицы
        public static string ColumnsNamesRequest(string catalog, string tableName)
        {
            return $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_CATALOG = '{catalog}' and TABLE_SCHEMA = 'dbo' and TABLE_NAME = '{tableName}'";
        }

        /// <summary>
        /// Запрос получения списока значений по одной колонке
        /// </summary>
        public static string OneColumnRequest(string column, string catalog, string tableName)
        {
            return $"SELECT {column} FROM [{catalog}].[dbo].[{tableName}]";
        }

        /// <summary>
        /// Запрос расположения каталога
        /// </summary>
        public static string CatalogPathRequest(string catalog)
        {
            return $"SELECT TOP 1 physical_name FROM [{catalog}].sys.database_files";
        }
        
        public static string CreateBackupRequest(string catalog, string path)
        {
            return $"BACKUP DATABASE [{catalog}] TO DISK = '{path}' ";
        }

        public static string DeleteBackupRequest(string path)
        {
            return $"EXECUTE master.dbo.xp_delete_file 0, N'{path}'";
        }

        public static string RestoreBackupRequest(string catalog, string path)
        {
            return $"ALTER DATABASE [{catalog}] SET single_user WITH rollback immediate; DROP DATABASE [{catalog}] RESTORE DATABASE [{catalog}] FROM DISK = '{path}'";
        }

        /// <summary>
        /// Запрос таблиц в которых используется ссылка на переданную таблицу
        /// </summary>
        public static string RecordsUsingAsForeignKeyRequest(string catalog, string tableName)
        {
            return "SELECT t.name AS TableWithForeignKey, c.name AS ForeignKeyColumnName " +
                   $"FROM [{catalog}].sys.foreign_key_columns AS fk " +
                   $"JOIN [{catalog}].sys.tables AS t ON fk.parent_object_id = t.object_id " +
                   $"JOIN [{catalog}].sys.columns AS c ON fk.parent_object_id = c.object_id and fk.parent_column_id = c.column_id " +
                   $"WHERE fk.referenced_object_id = (SELECT object_id FROM [{catalog}].sys.tables WHERE name = '{tableName}') and c.name != 'ID' and t.name != '{tableName}'";
        }

        public static string RenameTableColumnRequest(string catalog, string tableName, string oldColumnName, string newColumnName)
        {
            // EXEC [5572_verh].[dbo].sp_rename 'tblINVENTORY.ISN_INVENTORY_STORAGE', 'ISN_STORAGE_MEDIUM', 'COLUMN';
            return $"EXEC [{catalog}].[dbo].sp_rename '{tableName}.{oldColumnName}', '{newColumnName}', 'COLUMN';";
        }

        public static string AddForeignKeyOnTable_old(string catalog, string repairTableName, string referenceTableName, string linkColumn)
        {
            return $"ALTER TABLE[{catalog}].[dbo].[{repairTableName}] ADD FOREIGN KEY({linkColumn}) REFERENCES[{catalog}].[dbo].[{referenceTableName}]({linkColumn});";
        }

        public static string AddForeignKeyOnTable(string catalog, string repairTableName, string referenceTableName, string linkColumn)
        {
            return $"IF NOT EXISTS (SELECT t.name AS TableWithForeignKey " +
                $"FROM [{catalog}].sys.foreign_key_columns AS fk " +
                $"JOIN [{catalog}].sys.tables AS t ON fk.parent_object_id = t.object_id " +
                $"JOIN [{catalog}].sys.columns AS c ON fk.parent_object_id = c.object_id and fk.parent_column_id = c.column_id " +
                $"WHERE fk.referenced_object_id = (SELECT object_id FROM [{catalog}].sys.tables WHERE name = '{referenceTableName}') and c.name != 'ID' and t.name = '{repairTableName}') " +
                $"ALTER TABLE[{catalog}].[dbo].[{repairTableName}] ADD FOREIGN KEY({linkColumn}) REFERENCES[{catalog}].[dbo].[{referenceTableName}]({linkColumn});";
        }

/*        IF NOT EXISTS(SELECT t.name AS TableWithForeignKey
                FROM [TestDB].sys.foreign_key_columns AS fk
                JOIN [TestDB].sys.tables AS t ON fk.parent_object_id = t.object_id
                JOIN[TestDB].sys.columns AS c ON fk.parent_object_id = c.object_id and fk.parent_column_id = c.column_id
                WHERE fk.referenced_object_id = (SELECT object_id FROM [TestDB].sys.tables WHERE name = 'tblFUND') and c.name != 'ID' and t.name = 'tblACT')
  ALTER TABLE[TestDB].[dbo].[tblACT] ADD FOREIGN KEY(ISN_FUND) REFERENCES[TestDB].[dbo].[tblFUND] (ISN_FUND);*/
    }
}
