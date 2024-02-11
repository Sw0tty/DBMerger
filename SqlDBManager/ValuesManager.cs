using NotesNamespace;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlDBManager
{
    /// <summary>
    /// Вспомогательный класс для MergeManager. Содержит зарезервированные данные и методы по их манипуляции.
    /// </summary>
    public static class ValuesManager
    {
        /// <summary>
        /// Зарезервированные данные, на основе которых строятся и обновляются более сложные записи.
        /// </summary>
        static Dictionary<string, Dictionary<string, List<Tuple<string, string>>>> RewriteDict { get; set; } = new Dictionary<string, Dictionary<string, List<Tuple<string, string>>>>();

        // Наименование таблицы в которой есть внешние ключи на дефолтные таблицы - Список словарей, у которых Ключ это наименование колонки с внешним ключом - Которая содержит список кортежей (старый ключ, и ключ на который нужно обновить)
        // "tblINVENTORY": [
        //      "SECURY_LVL": [ ('10023', '10067'), ('10001', '10432') ],
        //      "REASON": [ ('10003', '10007'), ('10001', '10032') ]
        // ]
        /// <summary>
        /// Зарезервированные данные, на основе которых идет фильтрация для импортируемых записей.
        /// </summary>
        static Dictionary<string, Dictionary<string, List<Tuple<string, string>>>> ReserveDict { get; set; } = new Dictionary<string, Dictionary<string, List<Tuple<string, string>>>>();
        // "tblINVENTORY": [{"old_key": "new_key"}, {"old_key": "new_key"}, {"old_key": "new_key"}]
        // 1. Ищем в словаре по таблице. Если нету, то делаем поиск по уникальности. В итоге след пункт
        // 2. Берем

        static Dictionary<string, Dictionary<string, List<Tuple<string, string>>>> DocStatsRewriteDict { get; set; } = new Dictionary<string, Dictionary<string, List<Tuple<string, string>>>>();

        static List<string> ListInsertRequestInTable { get; set; } = new List<string>();

        public static Dictionary<string, List<Tuple<string, string>>> ReturnTableValuesReserveDict(string tableName)
        {
            return ReserveDict[tableName];
        }

        public static void DeleteTableFromReserve(string tableName)
        {
            ReserveDict.Remove(tableName);
        }

        public static Dictionary<string, List<Tuple<string, string>>> ReturnTableValuesSpecialRewriteDict(string tableName)
        {
            return DocStatsRewriteDict[tableName];
        }

        public static List<Tuple<string, string>> ReturnTuplesValuesSpecialDict(string tableName, string columnName)
        {
            return DocStatsRewriteDict[tableName][columnName];
        }

        public static Dictionary<string, List<Tuple<string, string>>> ReturnTableValuesRewriteDict(string tableName)
        {
            return RewriteDict[tableName];
        }

        public static void DeleteTableFromRewrite(string tableName)
        {
            RewriteDict.Remove(tableName);
        }

        public static bool ContainsInRewrite(string tableName)
        {
            return RewriteDict.ContainsKey(tableName);
        }

        public static int CountOfInsertValues()
        {
            return ListInsertRequestInTable.Count;
        }

        public static void ClearRequestsToTable()
        {
            ListInsertRequestInTable.Clear();
        }

        public static string ReturnRequestsToTable(int countOfValues)
        {

            List<string> copy = new List<string>(ListInsertRequestInTable);


            List<string> s = new List<string>();
            for (int i = 0; i < countOfValues; i++)
            {
                try
                {
                    s.Add(copy[i]);
                    ListInsertRequestInTable.Remove(copy[i]);

                }
                catch (ArgumentOutOfRangeException)
                {

                    break;
                }
            }

            //ListInsertRequestInTable = copy;


            return string.Join(", ", s);
        }

        public static void UpdateCheck(BackgroundWorker worker)
        {
            Consts.ALL_OF_CHECK++;
            //worker.ReportProgress(WorkerConsts.UPDATE_COUNT_OF_CHECK);
        }

        public static void AddToRequest(string rowInsertRequest)
        {
            //InsertRequestInTable += rowInsertRequest + "\n";

            ListInsertRequestInTable.Add(rowInsertRequest);

        }

        public static int CheckDefaultUsers(List<string> usersLogins, int countImports)
        {
            foreach (string userLogin in SpecialTablesValues.DefaultUsers)
            {
                if (usersLogins.Contains(userLogin))
                    continue;
                RestoreUser(userLogin); // В процессе
                countImports++;
            }

            return countImports;
        }

        public static Dictionary<string, string> MakeEditsInRow(Dictionary<string, string> row, string tableName, List<string> excludeColumns = null)
        {
            row = RemoveUnnecessary(row, excludeColumns);
            if (ContainsInRewrite(tableName))
                row = RepareColumnsValues(row, tableName);

            /*if (tableName == SpecialTablesValues.SpecailTablePair.Item2)
                row = SpecialRepareColumnsValues(row, tableName);*/

            row = EscapeSymbolsInRow(row);
            return row;
        }

        public static Dictionary<string, string> RemoveUnnecessary(Dictionary<string, string> row, List<string> excludeColumns)
        {
            if (excludeColumns != null)
            {
                foreach (string excludeColumnName in excludeColumns)
                {
                    row.Remove(excludeColumnName);
                }
            }
            row.Remove("ID");
            return row;
        }

        public static Dictionary<string, string> SpecialRepareColumnsValues(Dictionary<string, string> row, string tableName)
        {
            Dictionary<string, List<Tuple<string, string>>> rewriteDict = ReturnTableValuesSpecialRewriteDict(tableName);
            foreach (string columnName in rewriteDict.Keys)
            {
                foreach (Tuple<string, string> keysTuple in rewriteDict[columnName])
                {
                    if (row[columnName] == keysTuple.Item1)
                    {
                        row[columnName] = keysTuple.Item2;
                        break;
                    }
                }
            }
            return row;
        }

        public static Dictionary<string, string> RepareColumnsValues(Dictionary<string, string> row, string tableName)
        {
            Dictionary<string, List<Tuple<string, string>>> rewriteDict = ReturnTableValuesRewriteDict(tableName);
            foreach (string columnName in rewriteDict.Keys)
            {
                foreach (Tuple<string, string> keysTuple in rewriteDict[columnName])
                {
                    if (row[columnName] == keysTuple.Item1)
                    {
                        row[columnName] = keysTuple.Item2;
                        break;
                    }
                }
            }
            return row;
        }

        public static List<string> SelectDataFromColumn(List<Dictionary<string, string>> allFromMainData, string columnName)
        {
            List<string> listData = new List<string>();
            foreach (Dictionary<string, string> dataDict in allFromMainData)
                listData.Add(dataDict[columnName]);
            return listData;
        }

        public static List<Tuple<string, string>> ReturnFilteredTuples(List<Dictionary<string, string>> allFromTable, string firstParent, string secondParent)
        {
            List<Tuple<string, string>> parents = new List<Tuple<string, string>>();

            foreach (Dictionary<string, string> row in allFromTable)
            {
                Tuple<string, string> mayAddTuple = new Tuple<string, string>(row[firstParent], row[secondParent]);

                if (parents.Contains(mayAddTuple))
                {
                    continue;
                }
                parents.Add(new Tuple<string, string>(row[firstParent], row[secondParent]));
            }
            return parents;
        }

        public static void AddTablesToRewriteDict(Dictionary<string, string> foreigns)
        {
            foreach (string inTable in foreigns.Keys)
            {
                if (!RewriteDict.ContainsKey(inTable))
                {
                    RewriteDict[inTable] = new Dictionary<string, List<Tuple<string, string>>>();
                }
                RewriteDict[inTable][foreigns[inTable]] = new List<Tuple<string, string>>();
            }
        }

        public static void AddPairKeysToRewriteDict(Dictionary<string, string> foreigns, string idLikeColumnName, Tuple<string, string> pairKeys)
        {
            foreach (string inTable in foreigns.Keys)
            {
                foreach (string foreignKey in RewriteDict[inTable].Keys)
                {
                    if (foreignKey == idLikeColumnName)
                    {
                        RewriteDict[inTable][idLikeColumnName].Add(pairKeys);
                    }
                    continue;
                }
            }
        }

        public static void AddNewTableToSpecialRewrite(Dictionary<string, string> foreigns)
        {
            foreach (string inTable in foreigns.Keys)
            {
                if (!DocStatsRewriteDict.ContainsKey(inTable))
                {
                    DocStatsRewriteDict[inTable] = new Dictionary<string, List<Tuple<string, string>>>();
                }
                DocStatsRewriteDict[inTable][foreigns[inTable]] = new List<Tuple<string, string>>();
            }
        }

        /// <summary>
        /// Добавляет в резервный словарь наименование таблицы и наименое столбца на которые используется ссылка переданной таблицы
        /// </summary>
        public static void AddNewTableToReserve(Dictionary<string, string> foreigns)
        {
            foreach (string inTable in foreigns.Keys)
            {
                if (!ReserveDict.ContainsKey(inTable))
                {
                    ReserveDict[inTable] = new Dictionary<string, List<Tuple<string, string>>>();
                }
                ReserveDict[inTable][foreigns[inTable]] = new List<Tuple<string, string>>();
            }
        }

        public static void AddPairKeysToSpecialRewrite(Dictionary<string, string> foreigns, string idLikeColumnName, Tuple<string, string> pairKeys)
        {
            foreach (string inTable in foreigns.Keys)
            {
                foreach (string foreignKey in DocStatsRewriteDict[inTable].Keys)
                {
                    if (foreignKey == idLikeColumnName)
                    {
                        DocStatsRewriteDict[inTable][idLikeColumnName].Add(pairKeys);
                    }
                    continue;
                }
            }
        }

        /// <summary>
        /// Добавляет пару ключей к таблице в резервной копии
        /// </summary>
        public static void AddPairKeysToReserve(Dictionary<string, string> foreigns, string idLikeColumnName, Tuple<string, string> pairKeys)
        {
            foreach (string inTable in foreigns.Keys)
            {
                foreach (string foreignKey in ReserveDict[inTable].Keys)
                {
                    if (foreignKey == idLikeColumnName)
                    {
                        ReserveDict[inTable][idLikeColumnName].Add(pairKeys);
                    }
                    continue;
                }
            }
        }

        /// <summary>
        /// Возвращает список разницы между двумя списками
        /// </summary>
        public static List<string> CheckUniqueValue(List<string> mainList, List<string> daughterList)
        {
            List<string> uniqueValues = new List<string>();

            foreach (string value in daughterList)
            {
                if (!mainList.Contains(value))
                {
                    uniqueValues.Add(value);
                }
            }
            return uniqueValues;
        }

        /// <summary>
        /// (not relevant because of a bug. It will be replaced on RecursionFilterValue ) Ищет в словаре значение и возвращает по переданным параметрам
        /// </summary>
        public static string ReturnValue(List<Dictionary<string, string>> mainRecordsFromTable, string searchColumn, string searchValue, string returnTheColumnValue)
        {
            foreach (Dictionary<string, string> rowData in mainRecordsFromTable)
            {
                if (rowData[searchColumn] == searchValue)
                {
                    return rowData[returnTheColumnValue];
                }
            }
            return "";
        }

        public static string RecursionFilterValue(Dictionary<string, string> searchingRow, List<Dictionary<string, string>> filteringRecords, List<string> columnsFilters, string returnColumnValue)
        {
            if (filteringRecords.Count == 1)
                return filteringRecords[0][returnColumnValue];

            List<Dictionary<string, string>> newFilteringRecords = new List<Dictionary<string, string>>();

            foreach (Dictionary<string, string> row in filteringRecords)
            {
                if (row[columnsFilters[0]] == searchingRow[columnsFilters[0]])
                {
                    newFilteringRecords.Add(row);
                }
            }
            columnsFilters.Remove(columnsFilters[0]);

            return RecursionFilterValue(searchingRow, newFilteringRecords, columnsFilters, returnColumnValue);
        }

        static Dictionary<string, string> EscapeSymbolsInRow(Dictionary<string, string> row)
        {
            Dictionary<string, string> escapedRow = new Dictionary<string, string>(row);

            foreach (string key in row.Keys)
            {
                if (escapedRow[key].Contains("'"))
                {
                    escapedRow[key] = $"'{row[key].Substring(1, row[key].Length - 2).Replace("'", "\''")}'";
                }
            }
            return escapedRow;
        }

        /// <summary>
        /// Вносит изменения в БД. Корректирует невалидные данные дефолтных таблиц
        /// </summary>
        public static void RebuildCatalog(DBCatalog catalog, BackgroundWorker worker, Dictionary<string, List<Dictionary<string, string>>> problemTables, Dictionary<string, Tuple<string, Dictionary<string, List<string>>>> defaultTables)
        {
            foreach (string tableName in problemTables.Keys)
            {
                foreach (Dictionary<string, string> row in problemTables[tableName])
                {
                    Dictionary<string, string> foreignsDict = catalog.SelectTablesAndForeignKeyUsage(tableName);

                    foreach (string updateTable in foreignsDict.Keys)
                    {
                        catalog.UpdateValue(updateTable, foreignsDict[updateTable], defaultTables[tableName].Item1, foreignsDict[updateTable], row[foreignsDict[updateTable]]);
                    }
                    catalog.DeleteValue(tableName, string.Join("", defaultTables[tableName].Item2.Keys), row[string.Join("", defaultTables[tableName].Item2.Keys)]);
                }
            }
            worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, $"Каталог '{catalog.ReturnCatalogName()}' скорректирован.");
        }

        public static void SelectImportMethod(DBCatalog catalog, Dictionary<string, string> row, string tableName, BackgroundWorker worker)
        {
            if (Consts.FAST_REQUEST_MOD)
            {
                ValuesManager.AddToRequest(catalog.ReturnValues(ValuesManager.MakeEditsInRow(row, tableName)));           
                
            }
            else
            {
                catalog.InsertValue(tableName, ValuesManager.MakeEditsInRow(row, tableName));
                worker.ReportProgress(WorkerConsts.UPDATE_COUNT_OF_IMPORT);
            }
            Consts.ALL_OF_IMPORT++;
        }

        public static List<Dictionary<string, string>> FilterRecordsFrom(List<Dictionary<string, string>> allFromTable, string filterColumn, string filterValue, string filterColumn2 = null, string filterValue2 = null)
        {
            List<Dictionary<string, string>> filteredData = new List<Dictionary<string, string>>();

            if (filterColumn2 == null)
            {
                foreach (Dictionary<string, string> row in allFromTable)
                {
                    if (row[filterColumn] == filterValue)
                    {
                        filteredData.Add(row);
                    }
                }
            }
            else
            {
                foreach (Dictionary<string, string> row in allFromTable)
                {
                    if (row[filterColumn] == filterValue && row[filterColumn2] == filterValue2)
                    {
                        filteredData.Add(row);
                    }
                }
            }
            return filteredData;
        }

        public static void RestoreUser(string userLogin)
        {

        }
    }
}
