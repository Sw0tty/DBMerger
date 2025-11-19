using System.Collections.Generic;
using System.ComponentModel;
using System;


namespace SqlDBManager
{
    /// <summary>
    /// Вспомогательный класс для MergeManager. Содержит зарезервированные данные и методы по их манипуляции.
    /// </summary>
    public static class ValuesManager
    {
        /// <summary>
        /// Selects values by column
        /// </summary>
        /// <returns>List of string values</returns>
        public static List<string> SelectDataFromColumn(List<Dictionary<string, string>> allDataSomeTable, string columnName)
        {
            List<string> listData = new List<string>();
            foreach (Dictionary<string, string> dataDict in allDataSomeTable)
                listData.Add(dataDict[columnName]);
            return listData;
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
            worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, $"Каталог '{catalog.ReturnCatalogName()}' скорректирован.");
        }
    }
}
