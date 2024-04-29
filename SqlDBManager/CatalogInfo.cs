using System.Collections.Generic;
using SqlDBManager.DBClasses;
using System.Data.SqlClient;
using NotesNamespace;
using System;


namespace SqlDBManager
{
    public class DBCatalog : BaseDBConnector
    {
        public DBCatalog(string source, string catalog, string login, string password) : base(source, catalog, login, password) { }

        /// <summary>
        /// Возвращает путь расположения каталога
        /// </summary>
        public string SelectCatalogPath()
        {
            string request = SQLRequests.SelectRequests.CatalogPathRequest(ReturnCatalogName());
            return SelectSingleValueAdapter(request, likeValue: true, ReturnConnection(), ReturnTransaction());
            //return ReturnStringFromDB(request, ReturnConnection(), ReturnTransaction(), itsValue: true);
        }

        public string ReturnValues(Dictionary<string, string> row, bool withoutID = false)
        {
            if (withoutID)
                return $"({string.Join(", ", row.Values).Replace("'null'", "null")})";
            return $"(NEWID(), {string.Join(", ", row.Values).Replace("'null'", "null")})";
        }

        public int SelectCountTables()
        {
            string request = SQLRequests.SelectRequests.CountTablesRequest(ReturnCatalogName());
            SqlCommand command = new SqlCommand(request, ReturnConnection());
            command.Transaction = ReturnTransaction();
            SqlDataReader reader = command.ExecuteReader();

            reader.Read();
            int count = Convert.ToInt32(reader.GetValue(0));
            reader.Close();
            command.Dispose();
            return count;
/*            string request = SQLRequests.CountTablesRequest(Catalog);
            return SelectAdapter(request, connection, ReturnTransaction());*/
        }

        public string SelectLastRecord(string columns, string tableName, string orderByColumn)
        {
            string request = SQLRequests.SelectRequests.LastInsertRecordRequest(ReturnCatalogName(), columns, tableName, orderByColumn);
            return SelectSingleValueAdapter(request, likeValue: true, ReturnConnection(), ReturnTransaction());
        }

        /// <summary>
        /// Возвращает количество записей в переданной таблице
        /// </summary>
        public int SelectCountRowsTable(string tableName)
        {
            string request = SQLRequests.SelectRequests.CountRowsRequest(ReturnCatalogName(), tableName);
            return SelectCountAdapter(request, ReturnConnection(), ReturnTransaction());
/*            SqlCommand command = new SqlCommand(request, ReturnConnection());
            command.Transaction = ReturnTransaction();
            SqlDataReader reader = command.ExecuteReader();

            reader.Read();
            int count = Convert.ToInt32(reader.GetValue(0));
            reader.Close();
            command.Dispose();
            return count;*/
        }

        public int UpdateArchive(string tableName, List<string> updateSet)
        {
            string request = SQLRequests.UpdateRequests.UpdateTableRequest(ReturnCatalogName(), tableName, updateSet);
            return UpdateAdapter(request, ReturnConnection(), ReturnTransaction());
        }

        /// <summary>
        /// Возвращает список найденых записей в виде словаря (колонка - значение)
        /// </summary>
        public List<Dictionary<string, string>> SelectAllFrom(string tableName, List<string> columns, bool allowsNull, Dictionary<string, List<string>> filter = null, bool filterIN = true)
        {
            string request = SQLRequests.SelectRequests.AllRecordsRequest(ReturnCatalogName(), tableName, filter, filterIN, columns);
            return ReturnListDictsFromDB(request, columns, allowsNull, ReturnConnection(), ReturnTransaction());
        }

        public string SelectIDFrom(string tableName, string idLikeColumn, string filterValue)
        {
            string request = SQLRequests.SelectRequests.IDFromRequest(ReturnCatalogName(), tableName, idLikeColumn, filterValue);
            return SelectSingleValueAdapter(request, likeValue: true, ReturnConnection(), ReturnTransaction());
            //return ReturnStringFromDB(request, ReturnConnection(), ReturnTransaction(), itsValue: true);
        }

        public string SelectReferenceTableName(string currentTableName, string foreignColumnName)
        {
            string request = SQLRequests.SelectRequests.ReferenceTableNameRequest(ReturnCatalogName(), currentTableName, foreignColumnName);
            return SelectSingleValueAdapter(request, likeValue: false, ReturnConnection(), ReturnTransaction());
        }

        public List<string> SelectTablesNames()
        {
            string request = SQLRequests.SelectRequests.AllTablesNamesRequest(ReturnCatalogName());
            return ReturnListFromDB(request, ReturnConnection(), ReturnTransaction());
        }

        /// <summary>
        /// Returning list of tables with logs data
        /// </summary>
        public List<string> SelectLogTables()
        {
            string request = SQLRequests.SelectRequests.LogTablesRequest(ReturnCatalogName());
            return ReturnListFromDB(request, ReturnConnection(), ReturnTransaction());
        }

        public List<string> SelectDefaultSkipTables()
        {
            string request = SQLRequests.SelectRequests.SkipRequest(ReturnCatalogName());
            return ReturnListFromDB(request, ReturnConnection(), ReturnTransaction());
        }

        public List<string> SelectDefaultProcessingTables()
        {
            // Дефолтные таблицы на обработку
            string request = SQLRequests.SelectRequests.ProcessingRequest(ReturnCatalogName());
            return ReturnListFromDB(request, ReturnConnection(), ReturnTransaction());
        }

        /// <summary>
        /// Возвращает словарь значений где ключ - таблица в которой используются значения из переданной таблицы.
        /// Значение - наименование столбца через который осуществляется связь
        /// </summary>
        public Dictionary<string, string> SelectTablesAndForeignKeyUsage(string tableName)
        {
            string request = SQLRequests.SelectRequests.RecordsUsingAsForeignKeyRequest(ReturnCatalogName(), tableName);
            return ReturnDictFromDB(request, ReturnConnection(), ReturnTransaction());
        }

/*        public Dictionary<string, string> SelectCatalogVersion_old()
        {
            string request = SQLRequests.SelectRequests.SelectVersionRequest(ReturnCatalogName());
            return ReturnDictFromDB(request, ReturnConnection(), ReturnTransaction());
        }*/

        public string SelectCatalogVersion()
        {
            string request = SQLRequests.SelectRequests.SelectVersionRequest_new(ReturnCatalogName());
            return SelectSingleValueAdapter(request, likeValue: false, ReturnConnection(), ReturnTransaction());
        }

        /// <summary>
        /// Возвращает список наименований столбцов переданной таблицы
        /// </summary>
        public List<string> SelectColumnsNames(string tableName, List<string> excludeColumns)
        {
            string request = SQLRequests.SelectRequests.ColumnsNamesRequest(ReturnCatalogName(), tableName);
            return ReturnListFromDB(request, ReturnConnection(), ReturnTransaction(), excludeColumns);
        }

        /// <summary>
        /// Очищает переданную таблицу
        /// </summary>
        /// <returns>Количество удаленных записей</returns>
        public int ClearTable(string tableName)
        {
            string request = SQLRequests.DeleteRequests.ClearTableRequest(ReturnCatalogName(), tableName);
            return DeleteAdapter(request, ReturnConnection(), ReturnTransaction());
        }

        public int AddReference(string repairTableName, string referenceTableName, string linkColumn)
        {
            string request = SQLRequests.UpdateRequests.AddForeignKeyOnTable(ReturnCatalogName(), repairTableName, referenceTableName, linkColumn);
            return AlterAdapter(request, ReturnConnection(), ReturnTransaction());
        }

        public int RenameColumn(string tableName, string oldColumnName, string newColumnName)
        {
            string request = SQLRequests.UpdateRequests.RenameTableColumnRequest(ReturnCatalogName(), tableName, oldColumnName, newColumnName);
            return AlterAdapter(request, ReturnConnection(), ReturnTransaction());
        }

        /// <summary>
        /// Вставляет переданные данные в указанную таблицу (ID формируется средствами SQL)
        /// </summary>
        public int InsertValue(string tableName, Dictionary<string, string> data, bool withoutID = false)
        {
            string request = SQLRequests.InsertRequests.InsertDictValueRequst(ReturnCatalogName(), tableName, data, withoutID);
            return InsertAdapter(request, ReturnConnection(), ReturnTransaction());
        }

        public int SpecialInsertListOfValues(string tableName, string values, List<string> excludeColumns)
        {
            string request = SQLRequests.InsertRequests.FastFormerInsertValueRequst(SelectColumnsNames(tableName, excludeColumns), ReturnCatalogName(), tableName, values);
            return InsertAdapter(request, ReturnConnection(), ReturnTransaction());
        }

        public int UpdateValue(string tableName, string updateColumn, string updateValue, string filterColumn, string filterValue)
        {
            string request = SQLRequests.UpdateRequests.UpdateRowRequest(ReturnCatalogName(), tableName, updateColumn, updateValue, filterColumn, filterValue);
            return UpdateAdapter(request, ReturnConnection(), ReturnTransaction());
        }

        public int DeleteValue(string tableName, string filterColumn, string filterValue)
        {
            string request = SQLRequests.DeleteRequests.DeleteRowRequest(ReturnCatalogName(), tableName, filterColumn, filterValue);
            return DeleteAdapter(request, ReturnConnection(), ReturnTransaction());
        }

        public int SelectInventoryUnitCount(string unitKind, string inventoryID, string docType)
        {
            string request = SQLRequests.RecalculationRequests.InventoryUnitCountRequest(ReturnCatalogName(), unitKind, inventoryID, docType);
            return SelectCountAdapter(request, ReturnConnection(), ReturnTransaction());
        }

        public int Ultra_SelectInventoryUnitCount(string unitKind, string inventoryID, string docType, string signColumn, string sign, string signValue)
        {
            string request = SQLRequests.RecalculationRequests.Ultra_InventoryUnitCountRequest(ReturnCatalogName(), unitKind, inventoryID, docType, signColumn, sign, signValue);
            return SelectCountAdapter(request, ReturnConnection(), ReturnTransaction());
        }

        public string SelectFirstYear()
        {
            string request = SQLRequests.RecalculationRequests.YearOfFirstRecordRequest(ReturnCatalogName());
            return SelectSingleValueAdapter(request, likeValue: false, ReturnConnection(), ReturnTransaction());
        }

        public int CreateAndRecalcPassport(string request)
        {
            return InsertAdapter(request, ReturnConnection(), ReturnTransaction());
        }

        public int DeleteArchivePassports()
        {
            string request = SQLRequests.DeleteRequests.DeleteArchivePassportsRequest(ReturnCatalogName());
            return DeleteAdapter(request, ReturnConnection(), ReturnTransaction());
        }

/*        public int CreatePassport(string IDLastRecord, string passportYear)
        {
            string request = SQLRequests.RecalculationRequests.CreatePassportRequest(ReturnCatalogName(), IDLastRecord, passportYear);
            return InsertAdapter(request, ReturnConnection(), ReturnTransaction());
        }*/

/*        public int CreatePassportStat(string passportID, string statID, string docType, string carrierType)
        {
            string request = SQLRequests.RecalculationRequests.CreatePassportStatRequest(ReturnCatalogName(), passportID, statID, docType, carrierType);
            return InsertAdapter(request, ReturnConnection(), ReturnTransaction());
        }

        public int UpdateInventoryCount(string inventoryID, string docType, string carrierType, int unitCount)
        {
            string request = SQLRequests.RecalculationRequests.UpdateInventoryUnitCountRequest(ReturnCatalogName(), inventoryID, docType, carrierType, unitCount);
            return UpdateAdapter(request, ReturnConnection(), ReturnTransaction());
        }*/

        public int UpdateInventoryDocStats(string inventoryID, string docType, string carrierType, bool withAccountingUnits,
                                           int registered, int ocUnits, int unique, int hasSF, int hasFP, int notFound, int secret, int catalogued,
                                           int regRegistered = 0, int regOC = 0, int regUnique = 0, int regHasSF = 0, int regHasFP = 0, int regNotFound = 0, int regSecret = 0, int regCatalogued = 0)
        {
            string request = SQLRequests.RecalculationRequests.UpdateInventoryDocStatsRequest(ReturnCatalogName(), inventoryID, docType, carrierType, withAccountingUnits,
                                                                                              registered, ocUnits, unique, hasSF, hasFP, notFound, secret, catalogued,
                                                                                              regRegistered, regOC, regUnique, regHasSF, regHasFP, regNotFound, regSecret, regCatalogued);
            return UpdateAdapter(request, ReturnConnection(), ReturnTransaction());
        }

        public int UpdateFundDocStats(string fundID, string docType, string carrierType, bool withAccountingUnits,
                                      int registered, int ocUnits, int unique, int hasSF, int hasFP, int notFound, int secret, int catalogued,
                                      int regRegistered = 0, int regOC = 0, int regUnique = 0, int regHasSF = 0, int regHasFP = 0, int regNotFound = 0, int regSecret = 0, int regCatalogued = 0)
        {
            string request = SQLRequests.RecalculationRequests.UpdateFundDocStatsRequest(ReturnCatalogName(), fundID, docType, carrierType, withAccountingUnits,
                                                                                         registered, ocUnits, unique, hasSF, hasFP, notFound, secret, catalogued,
                                                                                         regRegistered, regOC, regUnique, regHasSF, regHasFP, regNotFound, regSecret, regCatalogued);
            return UpdateAdapter(request, ReturnConnection(), ReturnTransaction());
        }

        public int UpdateInventoryCheck(string inventoryID)
        {
            string request = SQLRequests.RecalculationRequests.UpdateInventoryCheckRequest(ReturnCatalogName(), inventoryID);
            return UpdateAdapter(request, ReturnConnection(), ReturnTransaction());
        }

        public int UpdateFundCheck(string tableName, string idLikeColumn, string objectID, int cardboarded, int needCardboarded, int damaged, int needRestoration, int needBinding, int needDisinfection, int needDisinsection, int fading, int needEnciphering, int needCoverChange, int flamed, int needKPO)
        {
            string request = SQLRequests.RecalculationRequests.UpdateObjectCheckRequest(ReturnCatalogName(), tableName, idLikeColumn, objectID, cardboarded, needCardboarded, damaged, needRestoration, needBinding, needDisinfection, needDisinsection, fading, needEnciphering, needCoverChange, flamed, needKPO);
            return UpdateAdapter(request, ReturnConnection(), ReturnTransaction());
        }
/*
        public int SelectCountFeaturesUnits(string inventoryID, string featureID)
        {
            string request = SQLRequests.RecalculationRequests.FeaturesUnitsRequest(ReturnCatalogName(), inventoryID, featureID);
            return SelectCountAdapter(request, ReturnConnection(), ReturnTransaction());
        }

        public int SelectCountWorksUnits(string inventoryID, string workID)
        {
            string request = SQLRequests.RecalculationRequests.WorksUnitsRequest(ReturnCatalogName(), inventoryID, workID);
            return SelectCountAdapter(request, ReturnConnection(), ReturnTransaction());
        }

        public int SelectCarboarderedUnit(string idLikeColumn, string filteredID)
        {
            string request = SQLRequests.RecalculationRequests.CardboardedUnitRequest(ReturnCatalogName(), idLikeColumn, filteredID);
            return SelectCountAdapter(request, ReturnConnection(), ReturnTransaction());
        }*/

        public List<Dictionary<string, string>> SelectFundAttachedInventoryCheck(string fundID)
        {
            string request = SQLRequests.RecalculationRequests.FundAttachedInventoryCheckRequest(ReturnCatalogName(), fundID);
            return SelectAdapter(request, allowsNull: true, ReturnConnection(), ReturnTransaction());
        }

        public List<Dictionary<string, string>> SelectFundAttachedInventoryDocStats(string fundID)
        {
            string request = SQLRequests.RecalculationRequests.FundAttachedInventoryDocStatsRequest(ReturnCatalogName(), fundID);
            return SelectAdapter(request, allowsNull: true, ReturnConnection(), ReturnTransaction());
        }

        public int UpdateFundInventoryCount(string fundID, int inventoryCount)
        {
            string request = SQLRequests.RecalculationRequests.UpdateFundInventoryCountRequest(ReturnCatalogName(), fundID, inventoryCount);
            return UpdateAdapter(request, ReturnConnection(), ReturnTransaction());
        }

        /// <summary>
        /// Возвращает список словарей значений таблицы
        /// </summary>
        static List<Dictionary<string, string>> ReturnListDictsFromDB(string request, List<string> columnsNames, bool allowsNull, SqlConnection connection, SqlTransaction transaction)
        {
            // on SELECT Adapter
            SqlCommand command = new SqlCommand(request, connection);
            command.Transaction = transaction;
            SqlDataReader reader = command.ExecuteReader();
            
            List<Dictionary<string, string>> tableData = new List<Dictionary<string, string>>();

            while (reader.Read())
            {
                Dictionary<string, string> rowData = new Dictionary<string, string>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader[i].ToString().Trim(' ') == "")
                    {
                        if (allowsNull)
                        {
                            rowData[columnsNames[i]] = "'null'";
                        }
                        else
                        {
                            rowData[columnsNames[i]] = "''";
                        }
                    }
                    else
                    {
                        rowData[columnsNames[i]] = "'" + reader[i].ToString() + "'";
                    }
                }
                tableData.Add(new Dictionary<string, string>(rowData));
            }

            reader.Close();
            command.Dispose();
            return tableData;
        }

        /// <summary>
        /// Возвращает словарь значений таблицы
        /// </summary>
        static Dictionary<string, string> ReturnDictFromDB(string request, SqlConnection connection, SqlTransaction transaction)
        {
            SqlCommand command = new SqlCommand(request, connection);
            command.Transaction = transaction;
            SqlDataReader reader = command.ExecuteReader();
            Dictionary<string, string> dictTableData = new Dictionary<string, string>();

            while (reader.Read())
            {
                dictTableData.Add(reader.GetString(0), reader.GetString(1));
            }

            reader.Close();
            command.Dispose();
            return dictTableData;
        }

        static List<string> ReturnListFromDB(string request, SqlConnection connection, SqlTransaction transaction, List<string> excludeColumns = null, bool likeDBString = false)
        {
            SqlCommand command = new SqlCommand(request, connection);
            command.Transaction = transaction;
            SqlDataReader reader = command.ExecuteReader();
            List<string> listTablesNames = new List<string>();

            if (likeDBString)
            {
                while (reader.Read())
                {
                    listTablesNames.Add("'" + reader.GetValue(0).ToString() + "'");
                }
            }
            else
            {
                while (reader.Read())
                {
                    listTablesNames.Add(reader.GetValue(0).ToString());
                }
            }

            if (excludeColumns != null)
                listTablesNames = HelpFunction.Exclude(listTablesNames, excludeColumns);
            
            reader.Close();
            command.Dispose();
            return listTablesNames;
        }
    }
}
