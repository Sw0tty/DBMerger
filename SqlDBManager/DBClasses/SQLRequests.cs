using System.Collections.Generic;
using System.Windows.Forms;
using static SqlDBManager.Consts.RecalcConsts;


namespace SqlDBManager
{
    public static class SQLRequests
    {
        public static class SelectRequests
        {
            public static string AllAllowedDataBasesRequest()
            {
                return "SELECT name FROM sys.databases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb');";
            }

            public static string ValuesOnUpdate(string catalogName, List<string> columns, string tableName)
            {
                return $"SELECT {string.Join(", ", columns)} FROM [{catalogName}].[dbo].[{tableName}]";
            }

            public static string CatalogPathRequest(string catalog)
            {
                return $"SELECT TOP 1 physical_name FROM [{catalog}].sys.database_files;";
            }

            public static string LogicalNameRequest(string catalogName)
            {
                return $"SELECT name FROM [{catalogName}].sys.database_files WHERE type_desc = 'ROWS'";
            }

            public static string LogicalNameLogRequest(string catalogName)
            {
                return $"SELECT name FROM [{catalogName}].sys.database_files WHERE type_desc = 'LOG'";
            }

            public static string IDFromRequest(string catalog, string tableName, string filterColumn, string filterValue)
            {
                return $"SELECT ID FROM [{catalog}].[dbo].[{tableName}] WHERE {filterColumn} = {filterValue};";
            }

            /// <summary>
            /// Request of taking table name on the foreign key referenced
            /// </summary>
            /// <returns>String request</returns>
            public static string ReferenceTableNameRequest(string catalog, string currentTableName, string foreignColumnName)
            {
                return $"USE [{catalog}]; " +
                       "SELECT OBJECT_NAME (fk.referenced_object_id) " +
                       "FROM sys.foreign_keys AS fk " +
                       "INNER JOIN sys.foreign_key_columns AS fk_c " +
                       "ON fk.object_id = fk_c.constraint_object_id " +
                       $"WHERE OBJECT_NAME(fk.parent_object_id) = '{currentTableName}' " +
                       $"and COL_NAME(fk_c.parent_object_id, fk_c.parent_column_id) = '{foreignColumnName}'";
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
                       $"WHERE fk.referenced_object_id = (SELECT object_id FROM [{catalog}].sys.tables WHERE name = '{tableName}') and c.name != 'ID' and t.name != '{tableName}';";
            }

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

            /// <summary>
            /// Request of the number of tables in catalog
            /// </summary>
            /// <returns>String request</returns>
            public static string CountTablesRequest(string catalog)
            {
                return $"SELECT COUNT(TABLE_NAME) FROM [{catalog}].INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE';";
            }

            /// <summary>
            /// Request of the names tables in catalog
            /// </summary>
            /// <returns>String request</returns>
            public static string AllTablesNamesRequest(string catalog)
            {
                return $"SELECT TABLE_NAME FROM [{catalog}].INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME;";
            }

            /// <summary>
            /// Request of the log tables names
            /// </summary>
            /// <returns>String request</returns>
            public static string LogTablesRequest(string catalog)
            {
                return $"SELECT TABLE_NAME FROM [{catalog}].INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' and TABLE_NAME like '%log' ORDER BY TABLE_NAME;";
            }

            /// <summary>
            /// Column of last record
            /// </summary>
            /// <returns>String request</returns>
            public static string LastInsertRecordRequest(string catalogName, string column, string tableName, string orderByColumn)
            {
                return $"SELECT TOP 1 {column} FROM [{catalogName}].[dbo].[{tableName}] ORDER BY {orderByColumn} * 1 DESC;";
            }

            /// <summary>
            /// Count of records in table request
            /// </summary>
            /// <returns>String request</returns>
            public static string CountRowsRequest(string catalogName, string tableName, string column = "*", string filterValue = null)
            {
                if (filterValue == null)
                    return $"SELECT COUNT({column}) FROM [{catalogName}].[dbo].[{tableName}];";
                return $"SELECT COUNT({column}) FROM [{catalogName}].[dbo].[{tableName}] WHERE {column} = {filterValue};";
            }

            public static string CountSysRowsRequest(string column, string catalogName)
            {
                return $"SELECT COUNT({column}) FROM sys.databases WHERE {column} like '{catalogName}%';";
            }

            public static string SelectVersionRequest(string catalog)
            {
                return $"SELECT [Value], [Text] FROM [{catalog}].[dbo].[tblConstantsSpec] WHERE Value = 'Version'";
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
                    return $"SELECT {strColumns} FROM [{catalog}].[dbo].[{tableName}] WHERE {string.Join("", filter.Keys)} IN ({string.Join(", ", filter[string.Join("", filter.Keys)])});";
                if (filter != null && filterIN && strColumns.Contains("Deleted"))
                    return $"SELECT {strColumns} FROM [{catalog}].[dbo].[{tableName}] WHERE {string.Join("", filter.Keys)} IN ({string.Join(", ", filter[string.Join("", filter.Keys)])}) and Deleted = '0';";
                if (filter != null && !filterIN && !strColumns.Contains("Deleted"))
                    return $"SELECT {strColumns} FROM [{catalog}].[dbo].[{tableName}] WHERE {string.Join("", filter.Keys)} NOT IN ({string.Join(", ", filter[string.Join("", filter.Keys)])});";
                if (filter != null && !filterIN && strColumns.Contains("Deleted"))
                    return $"SELECT {strColumns} FROM [{catalog}].[dbo].[{tableName}] WHERE {string.Join("", filter.Keys)} NOT IN ({string.Join(", ", filter[string.Join("", filter.Keys)])}) and Deleted = '0';";
                if (filter == null && filterIN && strColumns.Contains("Deleted"))
                    return $"SELECT {strColumns} FROM [{catalog}].[dbo].[{tableName}] WHERE Deleted = '0';";
                return $"SELECT {strColumns} FROM [{catalog}].[dbo].[{tableName}];";
            }

            /// <summary>
            /// Request for table column names
            /// </summary>
            /// <returns>String request</returns>
            public static string ColumnsNamesRequest(string catalog, string tableName)
            {
                return $"USE [{catalog}]; " +
                       $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'dbo' and TABLE_NAME = '{tableName}';";
            }
        }

        public static class InsertRequests
        {
            /// <summary>
            /// Request on insert row
            /// </summary>
            /// <returns>String request</returns>
            public static string InsertDictValueRequst(string catalogName, string tableName, Dictionary<string, string> data, bool withoutID = false)
            {
                if (withoutID)
                {
                    return $"INSERT INTO [{catalogName}].[dbo].[{tableName}]({string.Join(", ", data.Keys).Replace('\"', '\'')}) VALUES ({string.Join(", ", data.Values).Replace("'null'", "null")});";
                }
                return $"INSERT INTO [{catalogName}].[dbo].[{tableName}](ID, {string.Join(", ", data.Keys).Replace('\"', '\'')}) VALUES (NEWID(), {string.Join(", ", data.Values).Replace("'null'", "null")});";
            }

            /// <summary>
            /// Form big request to table
            /// </summary>
            /// <returns>String request</returns>
            public static string FastFormerInsertValueRequst(List<string> columns, string catalogName, string tableName, string values)
            {
                return $"INSERT INTO [{catalogName}].[dbo].[{tableName}]({string.Join(", ", columns).Replace('\"', '\'')}) VALUES {values};";
            }
        }

        public static class UpdateRequests
        {
            /// <summary>
            /// Update filtered records request
            /// </summary>
            /// <returns>String request</returns>
            public static string UpdateRowRequest(string catalogName, string tableName, string updateColumn, string updateValue, string filterColumn, string filterValue)
            {
                return $"UPDATE [{catalogName}].[dbo].[{tableName}] SET {updateColumn} = {updateValue} WHERE {filterColumn} = {filterValue};";
            }

            /// <summary>
            /// Update full table request
            /// </summary>
            /// <returns>String request</returns>
            public static string UpdateTableRequest(string catalogName, string tableName, List<string> updateSet)
            {
                //, string fullName, string shortName, string address, string description
                //return $"UPDATE [{catalogName}].[dbo].[{tableName}] SET NAME = '{fullName}', NAME_SHORT = '{shortName}', ADDRESS = '{address}', AUTHORITY = '{description}';";
                return $"UPDATE [{catalogName}].[dbo].[{tableName}] SET {string.Join(", ", updateSet)};";
            }

            /// <summary>
            /// Request to renames same column
            /// </summary>
            /// <returns>String request</returns>
            public static string RenameTableColumnRequest(string catalogName, string tableName, string oldColumnName, string newColumnName)
            {
                return $"EXEC [{catalogName}].[dbo].sp_rename '{tableName}.{oldColumnName}', '{newColumnName}', 'COLUMN';";
            }

            /// <summary>
            /// Request to add missing foreign keys
            /// </summary>
            /// <returns>String request</returns>
            public static string AddForeignKeyOnTable(string catalogName, string repairTableName, string referenceTableName, string linkColumn)
            {
                return $"IF NOT EXISTS (SELECT t.name AS TableWithForeignKey " +
                       $"FROM [{catalogName}].sys.foreign_key_columns AS fk " +
                       $"JOIN [{catalogName}].sys.tables AS t ON fk.parent_object_id = t.object_id " +
                       $"JOIN [{catalogName}].sys.columns AS c ON fk.parent_object_id = c.object_id and fk.parent_column_id = c.column_id " +
                       $"WHERE fk.referenced_object_id = (SELECT object_id FROM [{catalogName}].sys.tables WHERE name = '{referenceTableName}') and c.name != 'ID' and t.name = '{repairTableName}') " +
                       $"ALTER TABLE[{catalogName}].[dbo].[{repairTableName}] ADD FOREIGN KEY({linkColumn}) REFERENCES[{catalogName}].[dbo].[{referenceTableName}]({linkColumn});";
            }
        }

        public static class DeleteRequests
        {
            /// <summary>
            /// Clear table request
            /// </summary>
            /// <returns>String request</returns>
            public static string ClearTableRequest(string catalogName, string table)
            {
                return $"DELETE [{catalogName}].[dbo].[{table}];";
            }

            /// <summary>
            /// Delete records request
            /// </summary>
            /// <returns>String request</returns>
            public static string DeleteRowRequest(string catalogName, string tableName, string filterColumn, string filterValue)
            {
                return $"DELETE [{catalogName}].[dbo].[{tableName}] WHERE {filterColumn} = {filterValue};";
            }
        }
  
        public static class BackUpRequests
        {
            public static string DropCatalogRequest(string catalogName)
            {
                return $"ALTER DATABASE [{catalogName}] SET single_user WITH rollback immediate; DROP DATABASE [{catalogName}]";
            }

            public static string RestoreBackupRequest(string catalogName, string bakupPath, string logicalName, string logicalNameLog, string logicalPath, string logicalLogPath)
            {
                return "USE master; " +
                       $"RESTORE DATABASE [{catalogName}{Consts.VisualConsts.TAIL_OF_MERGED_FILES}] FROM DISK = {bakupPath} " +
                       $"WITH MOVE {logicalName} TO {logicalPath}, " +
                       $"MOVE {logicalNameLog} TO {logicalLogPath}";
            }

            public static string CreateBackupRequest(string catalogName, string path)
            {
                return $"BACKUP DATABASE [{catalogName}] TO DISK = {path};";
            }

            public static string DeleteBackupRequest(string path)
            {
                return $"EXECUTE master.dbo.xp_delete_file 0, N{path};";
            }
        }

        public static class RecalculationRequests
        {
            public static string UpdateInventoryUnitCountRequest(string catalogName, string inventoryID, string docType, string carrierType, int unitCount)
            {
                string docEqual = (docType == null) ? "is null" : $"= '{docType}'";
                string carrierEqual = (carrierType == null) ? "is null" : $"= '{carrierType}'";

                return $"USE [{catalogName}]; " +
                       $"UPDATE [tblDOCUMENT_STATS] SET UNIT_COUNT = {unitCount}, UNIT_REGISTERED = '{unitCount}' WHERE ISN_INVENTORY = {inventoryID} and ISN_DOC_TYPE {docEqual} and CARRIER_TYPE {carrierEqual};";
            }

/*            public static string SelectInventoryUnitCountRequest()
            {
                return "";
            }*/

            public static string UpdateInventoryDocStatsRequest(string catalogName, string inventoryID, string docType, string carrierType, bool withAccountingUnits,
                                                                int registered, int ocUnits, int unique, int hasSF, int hasFP, int notFound, int secret, int catalogued,
                                                                int regRegistered = 0, int regOC = 0, int regUnique = 0, int regHasSF = 0, int regHasFP = 0, int regNotFound = 0, int regSecret = 0, int regCatalogued = 0)
            {
                string docEqual = (docType == null) ? "is null" : $"= '{docType}'";
                string carrierEqual = (carrierType == null) ? "is null" : $"= '{carrierType}'";

                return $"USE [{catalogName}]; " +
                       "UPDATE [tblDOCUMENT_STATS] " +
                       $"SET UNIT_COUNT = '{registered}', UNIT_REGISTERED = '{registered}', UNIT_OC_COUNT = '{ocUnits}', UNITS_UNIQUE = '{unique}', UNIT_HAS_SF = '{hasSF}', UNIT_HAS_FP = '{hasFP}', UNITS_NOT_FOUND = '{notFound}', SECRET_UNITS = '{secret}', UNITS_CATALOGUED = '{catalogued}' " +
                       (withAccountingUnits ? $", REG_UNIT = '{regRegistered}', REG_UNIT_REGISTERED = '{regRegistered}', REG_UNIT_OC = '{regOC}', REG_UNITS_UNIQUE = '{regUnique}', REG_UNIT_HAS_SF = '{regHasSF}', REG_UNIT_HAS_FP = '{regHasFP}', REG_UNITS_NOT_FOUND = '{regNotFound}', SECRET_REG_UNITS = '{regSecret}', REG_UNITS_CTALOGUE = '{regCatalogued}' " : "") +
                       $"WHERE ISN_INVENTORY = {inventoryID} and ISN_DOC_TYPE {docEqual} and CARRIER_TYPE {carrierEqual};";
            }

            public static string UpdateCheckRequest(string catalogName, string tableCheck, string idLikeColumn, string filteredID, int unitCount)
            {
                return $"USE [{catalogName}]; " +
                       $"UPDATE [{tableCheck}] SET CARDBOARDED = '{unitCount}' WHERE {idLikeColumn} = {filteredID};";
            }

            public static string UpdateInventoryCheckRequest(string catalogName, string inventoryID, int cardboarded, int needCardboarded, int damaged, int needRestoration, int needBinding, int needDisinfection, int needDisinsection, int fading, int needEnciphering, int needCoverChange, int flamed, int needKPO)
            {
/*                return $"USE [{catalogName}]; " +
                       $"UPDATE [{tableCheck}] SET CARDBOARDED = '{unitCount}' WHERE {idLikeColumn} = {filteredID};" +*/

                return $"USE [{catalogName}]; " +
                       $"UPDATE [tblINVENTORY_CHECK] SET CARDBOARDED = '{cardboarded}', UNITS_NEED_CARDBOARDED = '{needCardboarded}', UNITS_DBR = '{damaged}', UNITS_NEED_RESTORATION = '{needRestoration}', UNITS_NEED_BINDING = '{needBinding}', UNITS_NEED_DISINFECTION = '{needDisinfection}', UNITS_NEED_DISINSECTION = '{needDisinsection}', FADING_PAGES = '{fading}', UNITS_NEED_ENCIPHERING = '{needEnciphering}', UNITS_NEED_COVER_CHANGE = '{needCoverChange}', UNITS_INFLAMMABLE = '{flamed}', UNITS_NEED_KPO = '{needKPO}' " +
                       $"WHERE ISN_INVENTORY = {inventoryID};";
            }

            public static string UpdateObjectCheckRequest(string catalogName, string tableName, string idLikeColumn, string objectID, int cardboarded, int needCardboarded, int damaged, int needRestoration, int needBinding, int needDisinfection, int needDisinsection, int fading, int needEnciphering, int needCoverChange, int flamed, int needKPO)
            {
                return $"USE [{catalogName}]; " +
                       $"UPDATE [{tableName}] SET CARDBOARDED = '{cardboarded}', UNITS_NEED_CARDBOARDED = '{needCardboarded}', UNITS_DBR = '{damaged}', UNITS_NEED_RESTORATION = '{needRestoration}', UNITS_NEED_BINDING = '{needBinding}', UNITS_NEED_DISINFECTION = '{needDisinfection}', UNITS_NEED_DISINSECTION = '{needDisinsection}', FADING_PAGES = '{fading}', UNITS_NEED_ENCIPHERING = '{needEnciphering}', UNITS_NEED_COVER_CHANGE = '{needCoverChange}', UNITS_INFLAMMABLE = '{flamed}', UNITS_NEED_KPO = '{needKPO}' " +
                       $"WHERE {idLikeColumn} = {objectID};";
            }

            public static string InventoryUnitCountRequest(string catalogName, string unitKind, string inventoryID, string docType)
            {
                return $"USE [{catalogName}]; " +
                       $"SELECT COUNT(*) FROM [tblUNIT] WHERE Deleted = '0' and UNIT_KIND = '{unitKind}' and MEDIUM_TYPE = (SELECT CARRIER_TYPE FROM [tblINVENTORY] WHERE ISN_INVENTORY = {inventoryID}) and ISN_INVENTORY = {inventoryID} and ISN_DOC_TYPE = '{docType}';";
            }

            public static string Ultra_InventoryUnitCountRequest(string catalogName, string unitKind, string inventoryID, string docType, string signColumn, string sign, string signValue)
            {
                return $"USE [{catalogName}]; " +
                       $"SELECT COUNT(*) FROM [tblUNIT] WHERE Deleted = '0' and UNIT_KIND = '{unitKind}' and MEDIUM_TYPE = (SELECT CARRIER_TYPE FROM [tblINVENTORY] WHERE ISN_INVENTORY = {inventoryID}) and ISN_INVENTORY = {inventoryID} and ISN_DOC_TYPE = '{docType}' and {signColumn} {sign} '{signValue}';";
            }

            public static string FeaturesUnitsRequest(string catalogName, string inventoryID, string featureID)
            {
                return $"USE [{catalogName}]; " +
                       "SELECT COUNT(*) FROM [tblREF_FEATURE] AS ref_feature " +
                       $"JOIN [tblUNIT] AS unit ON ISN_OBJ = unit.ISN_UNIT WHERE unit.Deleted = '0' and unit.ISN_INVENTORY = {inventoryID} and ref_feature.KIND = '703' and ref_feature.ISN_FEATURE = '{featureID}';";
            }

            public static string WorksUnitsRequest(string catalogName, string inventoryID, string workID)
            {
                return $"USE [{catalogName}]; " +
                       $"SElECT COUNT(*) FROM [tblUNIT_REQUIRED_WORK] AS req_work JOIN [tblUNIT] AS unit ON unit.ISN_UNIT = req_work.ISN_UNIT WHERE unit.Deleted = '0' and unit.ISN_INVENTORY = {inventoryID} and req_work.IS_ACTUAL = 'Y' and req_work.ISN_WORK = '{workID}'";
            }

            public static string CardboardedUnitRequest(string catalogName, string idLikeColumn, string filteredID)
            {
                return $"SELECT COUNT(*) FROM [{catalogName}].[dbo].[tblUNIT] WHERE {idLikeColumn} = {filteredID} and CARDBOARDED = 'Y' and Deleted = '0';";
            }

            public static string UpdateFundInventoryCountRequest(string catalogName, string fundID, int inventoryCount)
            {
                return $"UPDATE [{catalogName}].[dbo].[tblFUND] SET INVENTORY_COUNT = '{inventoryCount}', AUTO_INVENTORY_COUNT = '{inventoryCount}' WHERE ISN_FUND = {fundID};";
            }

            public static string FundAttachedInventoryCheckRequest(string catalogName, string fundID)
            {
                return $"USE [{catalogName}]; " +
                       $"SELECT inv_check.* FROM [tblINVENTORY_CHECK] AS inv_check JOIN [tblINVENTORY] AS inv ON inv_check.ISN_INVENTORY = inv.ISN_INVENTORY WHERE inv.Deleted = '0' and inv.ISN_FUND = {fundID}";
            }

            public static string CountAllFunds(string catalogName, int passportYear)
            {
                return $"SELECT COUNT(ISN_FUND) FROM [{catalogName}].[dbo].[tblFUND] where CreationDateTime <= '{passportYear + 1}0101 00:00:00.000' and Deleted = '0';";
            }

            public static string CountTypeFunds(string catalogName, int passportYear, string fundType)
            {
                return $"SELECT COUNT(ISN_FUND) FROM [{catalogName}].[dbo].[tblFUND] where CreationDateTime <= '{passportYear + 1}0101 00:00:00.000' and ISN_DOC_TYPE = '{fundType}' and Deleted = '0';";
            }

            public static string CountAllInCategoryFunds(string catalogName, int passportYear, List<string> docsFundTypes, char fundType)
            {
                return $"SELECT COUNT(ISN_FUND) FROM [{catalogName}].[dbo].[tblFUND] where CreationDateTime <= '{passportYear + 1}0101 00:00:00.000' and ISN_DOC_TYPE in ({string.Join(", ", docsFundTypes)}) and CARRIER_TYPE = '{fundType}' and Deleted = '0';";
            }

            public static string CountAllInCategoryInventory(string catalogName, int passportYear, List<string> docsFundTypes, char fundType)
            {
                return $"SELECT COUNT(ISN_INVENTORY) FROM [{catalogName}].[dbo].[tblINVENTORY] where CreationDateTime <= '{passportYear + 1}0101 00:00:00.000' and ISN_DOC_TYPE in ({string.Join(", ", docsFundTypes)}) and CARRIER_TYPE = '{fundType}' and Deleted = '0';";
            }
        }
    }
}
