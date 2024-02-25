using NotesNamespace;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SqlDBManager
{
    public class MergeManager : MergeSettings
    {
        public static bool RepeirDBKeys(DBCatalog mainCatalog, BackgroundWorker worker)
        {
            if (Consts.DEBUG_MOD)
            {
                RepairTables(mainCatalog);
            }
            else
            {
                try
                {
                    RepairTables(mainCatalog);
                }
                catch (StopMergeException error)
                {
                    worker.ReportProgress(Consts.WorkerConsts.ERROR_STATUS_CODE, error.Message);
                    return false;
                }
                catch (Exception error)
                {
                    worker.ReportProgress(Consts.WorkerConsts.ERROR_STATUS_CODE, error.Message);
                    return false;
                }
            }
            return true;
        }

        static void RepairTables(DBCatalog mainCatalog)
        {
            foreach (string tableName in SpecialTablesValues.WithoutKeysTables.Keys)
            {
                Tuple<string, string> tupleParams = SpecialTablesValues.WithoutKeysTables[tableName];
                mainCatalog.AddReference(tableName, tupleParams.Item1, tupleParams.Item2);
            }
        }

        /// <summary>
        /// Очищает таблицы с логами
        /// </summary>
        /// <returns>Успешность выполнения транзакций</returns>
        public static bool ClearLogs(DBCatalog mainCatalog, BackgroundWorker worker)
        {   
            foreach (string logTable in mainCatalog.SelectLogTables())
            {
                worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, $"Очистка {logTable}:");

                if (mainCatalog.SelectCountRowsTable(logTable) == 0)
                {
                    worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + "Пустая таблица.");
                    worker.ReportProgress(100, Consts.WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
                }
                else
                {
                    if (Consts.DEBUG_MOD)
                    {
                        worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + $"Записей удалено {mainCatalog.ClearTable(logTable)}");
                    }
                    else
                    {
                        try
                        {
                            worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + $"Записей удалено {mainCatalog.ClearTable(logTable)}");
                        }
                        catch (StopMergeException error)
                        {
                            worker.ReportProgress(Consts.WorkerConsts.ERROR_STATUS_CODE, error.Message);
                            return false;
                        }
                        catch (Exception error)
                        {
                            worker.ReportProgress(Consts.WorkerConsts.ERROR_STATUS_CODE, error.Message);
                            return false;
                        }
                    }                   
                }
                worker.ReportProgress(100, Consts.WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
                worker.ReportProgress(Consts.MergeProgress.UpdateMainBar(), Consts.WorkerConsts.ITS_MAIN_PROGRESS_BAR);              
            }
            return true;
        }

        public static void ProcessSkipTables(DBCatalog mainCatalog, DBCatalog daughterCatalog, BackgroundWorker worker)
        {
            //ReturnProcessStatus(worker, mainCatalog);

            foreach (string defaultTable in mainCatalog.SelectDefaultSkipTables())
            {
                worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, $"Обработка {defaultTable}:");

                if (mainCatalog.SelectCountRowsTable(defaultTable) == 0)
                {
                    worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + "Пустая таблица.");
                }
                else
                {
                    worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + "Таблица с дефолтными значениями.");
                }
                worker.ReportProgress(100, Consts.WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
                worker.ReportProgress(Consts.MergeProgress.UpdateMainBar(), Consts.WorkerConsts.ITS_MAIN_PROGRESS_BAR);
            }
        }

        public static bool ProcessDefaultTables(DBCatalog mainCatalog, DBCatalog daughterCatalog, BackgroundWorker worker)
        {
            //ReturnProcessStatus(worker, mainCatalog, defaultTablesFunctions, daughterCatalog);


            foreach (string tableName in mainCatalog.SelectDefaultProcessingTables())
            {
                worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, $"Обработка {tableName}:");

                if (daughterCatalog.SelectCountRowsTable(tableName) == 0)
                {
                    worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + "Пустая таблица.");
                    worker.ReportProgress(100, Consts.WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
                }
                // Вызывается функция обработчик, которая собирает данные из кортежа
                else if (DefaultTablesParams.ContainsKey(tableName))
                {
                    Tuple<string, string, string, List<string>, bool> paramsForProcessing = DefaultTablesParams[tableName];

                    if (Consts.DEBUG_MOD)
                    {
                        worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + $"Импортировано значений {ProcessDefaultTable(worker, mainCatalog, daughterCatalog, tableName: tableName, uniqueValueColumnName: paramsForProcessing.Item1, idLikeColumnName: paramsForProcessing.Item2, highLevelColumnName: paramsForProcessing.Item3, excludeColumns: paramsForProcessing.Item4, allowsNull: paramsForProcessing.Item5)}");
                    }
                    else
                    {
                        try
                        {
                            worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + $"Импортировано значений {ProcessDefaultTable(worker, mainCatalog, daughterCatalog, tableName: tableName, uniqueValueColumnName: paramsForProcessing.Item1, idLikeColumnName: paramsForProcessing.Item2, highLevelColumnName: paramsForProcessing.Item3, excludeColumns: paramsForProcessing.Item4, allowsNull: paramsForProcessing.Item5)}");
                        }
                        catch (StopMergeException error)
                        {
                            worker.ReportProgress(Consts.WorkerConsts.ERROR_STATUS_CODE, error.Message);
                            return false;
                        }
                        catch (Exception error)
                        {
                            worker.ReportProgress(Consts.WorkerConsts.ERROR_STATUS_CODE, error.Message);
                            return false;
                        }
                    }
                }
                else
                {
                    worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + "Обработчик отсутствует.");
                    worker.ReportProgress(100, Consts.WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
                }
                worker.ReportProgress(Consts.MergeProgress.UpdateMainBar(), Consts.WorkerConsts.ITS_MAIN_PROGRESS_BAR);
            }
            return true;
        }

        public static bool ProcessLinksTables(DBCatalog mainCatalog, DBCatalog daughterCatalog, BackgroundWorker worker)
        {
            //ReturnProcessStatus(worker, mainCatalog, linksTablesFunctions, daughterCatalog);


            foreach (string tableName in ArchiveTables.OrderedCompositeTables)
            {
                worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, $"Обработка {tableName}:");

                if (daughterCatalog.SelectCountRowsTable(tableName) == 0)
                {
                    worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + "Пустая таблица.");
                    worker.ReportProgress(100, Consts.WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
                }
                else if (LinksTablesParams.ContainsKey(tableName))
                {
                    Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>> paramsForProcessing = LinksTablesParams[tableName];

                    if (Consts.DEBUG_MOD)
                    {
                        worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + $"Импортировано значений {ProcessLinksTable(worker, mainCatalog, daughterCatalog, tableName: tableName, uniqueValueColumnName: paramsForProcessing.Item1, idLikeColumnName: paramsForProcessing.Item2, highLevelColumnName: paramsForProcessing.Item3, parentIdColumn: paramsForProcessing.Item4, numerateColumn: paramsForProcessing.Item5, extraFilterColumns: paramsForProcessing.Item6, excludeColumns: paramsForProcessing.Item7, allowsNull: paramsForProcessing.Rest.Item1)}");
                    }
                    else
                    {
                        try
                        {
                            worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + $"Импортировано значений {ProcessLinksTable(worker, mainCatalog, daughterCatalog, tableName: tableName, uniqueValueColumnName: paramsForProcessing.Item1, idLikeColumnName: paramsForProcessing.Item2, highLevelColumnName: paramsForProcessing.Item3, parentIdColumn: paramsForProcessing.Item4, numerateColumn: paramsForProcessing.Item5, extraFilterColumns: paramsForProcessing.Item6, excludeColumns: paramsForProcessing.Item7, allowsNull: paramsForProcessing.Rest.Item1)}");
                        }
                        catch (StopMergeException error)
                        {
                            worker.ReportProgress(Consts.WorkerConsts.ERROR_STATUS_CODE, error.Message);
                            return false;
                        }
                        catch (Exception error)
                        {
                            worker.ReportProgress(Consts.WorkerConsts.ERROR_STATUS_CODE, error.Message);
                            return false;
                        }
                    }                    
                }
                else
                {
                    worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + "Обработчик отсутствует.");
                    worker.ReportProgress(100, Consts.WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
                }
                worker.ReportProgress(Consts.MergeProgress.UpdateMainBar(), Consts.WorkerConsts.ITS_MAIN_PROGRESS_BAR);
            }
            return true;
        }

        static bool ReturnProcessStatus(DBCatalog mainCatalog, List<string> processTables, string mergeWork, string workNameMessage, string workDoneMessage, string tableEmptyMessage, string funcUndefinedMessage, BackgroundWorker worker, DBCatalog daughterCatalog = null)
        {
            foreach (string tableName in processTables)
            {
                worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, $"{workNameMessage} {tableName}:");
                int checkEmpty = (daughterCatalog == null) ? mainCatalog.SelectCountRowsTable(tableName) : daughterCatalog.SelectCountRowsTable(tableName);

                if (checkEmpty == 0)
                {
                    worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + tableEmptyMessage);
                    worker.ReportProgress(100, Consts.WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
                }
                else if (mergeWork == Consts.MergeWorks.DEFAULT_TABLE && DefaultTablesParams.ContainsKey(tableName))
                {

                }
                else if (mergeWork == Consts.MergeWorks.COMPOSITE_TABLE && LinksTablesParams.ContainsKey(tableName))
                {

                }
                else if (LinksTablesParams.ContainsKey(tableName))
                {
                    /*if (Consts.DEBUG_MOD)
                    {
                        worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + $"{workDoneMessage} + {SelectMergeWork(mergeWork)}");
                    }
                    else
                    {
                        try
                        {
                            worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + $"{workDoneMessage} + {SelectMergeWork(mergeWork)}");
                        }
                        catch (StopMergeException error)
                        {
                            worker.ReportProgress(Consts.WorkerConsts.ERROR_STATUS_CODE, error.Message);
                            return false;
                        }
                        catch (Exception error)
                        {
                            worker.ReportProgress(Consts.WorkerConsts.ERROR_STATUS_CODE, error.Message);
                            return false;
                        }
                    }*/
                }
                else
                {
                    worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + funcUndefinedMessage);
                    worker.ReportProgress(100, Consts.WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
                }
            }
            return true;
        }

        static int SelectMergeWork(string mergeWork, DBCatalog catalog, string tableName)
        {
            switch (mergeWork)
            {
                case Consts.MergeWorks.SKIP:
                    return 1;
                case Consts.MergeWorks.CLEARING:
                    return catalog.ClearTable(tableName);
                case Consts.MergeWorks.DEFAULT_TABLE:
                    return 1;
                case Consts.MergeWorks.COMPOSITE_TABLE:
                    return 1;
                default: return 0;
            }
        }

        static int UpdateTable(DBCatalog catalog, BackgroundWorker worker)
        {
            foreach (string tableName in UpdateTables.Keys)
            {
                
            }
            return 1;
        }

        // --- Master merge process table ---
        // Для обработки дефолтных таблиц
        static int ProcessDefaultTable(BackgroundWorker worker, DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName, string uniqueValueColumnName, string idLikeColumnName, string highLevelColumnName, List<string> excludeColumns, bool allowsNull)
        {
            if (Consts.FAST_REQUEST_MOD)
                ValuesManager.ClearRequestsToTable();

            int countOfImports = 0;

            long lastId = 0;
            if (idLikeColumnName != null)
            {
                string lastIDFromDB = string.Join("", mainCatalog.SelectLastRecord(idLikeColumnName, tableName, idLikeColumnName)).Replace("\'", "");
                lastId = (lastIDFromDB != "") ? Convert.ToInt64(lastIDFromDB) : 0;
            }

            List<Dictionary<string, string>> tableDataFromMainCatalog = mainCatalog.SelectAllFrom(tableName, mainCatalog.SelectColumnsNames(tableName, excludeColumns), allowsNull);
            List<Dictionary<string, string>> tableDataFromDaughterCatalog = daughterCatalog.SelectAllFrom(tableName, daughterCatalog.SelectColumnsNames(tableName, excludeColumns), allowsNull);
            List<string> mainCatalogValues = ValuesManager.SelectDataFromColumn(tableDataFromMainCatalog, uniqueValueColumnName);
            Dictionary<string, string> foreigns = mainCatalog.SelectTablesAndForeignKeyUsage(tableName);

            if (foreigns.Count > 0)
            {
                ValuesManager.AddTablesToRewriteDict(foreigns);
            }

            Consts.MergeProgress.ClearTasksBlock();
            Consts.MergeProgress.AddTaskInBlock(tableDataFromDaughterCatalog.Count);

            if (highLevelColumnName != null)
            {
                List<Dictionary<string, string>> copyOfAll = new List<Dictionary<string, string>>();
                foreach (Dictionary<string, string> row in tableDataFromDaughterCatalog)
                    copyOfAll.Add(new Dictionary<string, string>(row));

                List<Dictionary<string, string>> copyOfcopyOfAll = new List<Dictionary<string, string>>(copyOfAll);

                while (copyOfAll.Count != 0)
                {                    
                    foreach (Dictionary<string, string> row in copyOfAll)
                    {                       
                        if (mainCatalogValues.Contains(row[uniqueValueColumnName]))
                        {
                            ValuesManager.AddPairKeysToRewriteDict(foreigns, idLikeColumnName, new Tuple<string, string>(row[idLikeColumnName], ValuesManager.ReturnValue(tableDataFromMainCatalog, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName)));
                            copyOfcopyOfAll.Remove(row);
                        }
                        else if (!mainCatalogValues.Contains(row[uniqueValueColumnName]))
                        {
                            string valueOnHighID = ValuesManager.ReturnValue(tableDataFromDaughterCatalog, idLikeColumnName, row[highLevelColumnName], uniqueValueColumnName);
                            if (mainCatalogValues.Contains(valueOnHighID))
                            {
                                string oldKey = row[idLikeColumnName];
                                if (row[highLevelColumnName] != "'null'")
                                    row[highLevelColumnName] = ValuesManager.ReturnValue(tableDataFromMainCatalog, uniqueValueColumnName, valueOnHighID, idLikeColumnName);
                                row[idLikeColumnName] = $"'{lastId + countOfImports + 1}'";
                                ValuesManager.AddPairKeysToRewriteDict(foreigns, idLikeColumnName, new Tuple<string, string>(oldKey, row[idLikeColumnName]));
                                ValuesManager.SelectImportMethod(mainCatalog, new Dictionary<string, string>(row), tableName, worker);
                                mainCatalogValues.Add(row[uniqueValueColumnName]);
                                tableDataFromMainCatalog.Add(new Dictionary<string, string>(row));
                                copyOfcopyOfAll.Remove(row);
                                countOfImports++;
                            }
                            else if (valueOnHighID == "")
                            {
                                string oldKey = row[idLikeColumnName];
                                row[idLikeColumnName] = $"'{lastId + countOfImports + 1}'";
                                ValuesManager.AddPairKeysToRewriteDict(foreigns, idLikeColumnName, new Tuple<string, string>(oldKey, row[idLikeColumnName]));
                                ValuesManager.SelectImportMethod(mainCatalog, new Dictionary<string, string>(row), tableName, worker);
                                mainCatalogValues.Add(row[uniqueValueColumnName]);
                                tableDataFromMainCatalog.Add(new Dictionary<string, string>(row));
                                copyOfcopyOfAll.Remove(row);
                                countOfImports++;
                            }
                            else if (!mainCatalogValues.Contains(ValuesManager.ReturnValue(tableDataFromDaughterCatalog, idLikeColumnName, row[highLevelColumnName], uniqueValueColumnName)))
                            {
                                Consts.MergeProgress.AddTaskInBlock();
                            }
                        }
                        worker.ReportProgress(Consts.MergeProgress.UpdateBlockBar(), Consts.WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
                    }
                    copyOfAll.Clear();
                    foreach (Dictionary<string, string> row in copyOfcopyOfAll)
                        copyOfAll.Add(row);
                }
            }
            else
            {
                foreach (Dictionary<string, string> row in tableDataFromDaughterCatalog)
                {
                    if (idLikeColumnName == null && !mainCatalogValues.Contains(row[uniqueValueColumnName]))
                    {
                        ValuesManager.SelectImportMethod(mainCatalog, new Dictionary<string, string>(row), tableName, worker);
                        mainCatalogValues.Add(row[uniqueValueColumnName]);
                        countOfImports++;
                    }
                    else if (idLikeColumnName != null && mainCatalogValues.Contains(row[uniqueValueColumnName]) && foreigns.Count > 0)
                    {
                        ValuesManager.AddPairKeysToRewriteDict(foreigns, idLikeColumnName, new Tuple<string, string>(row[idLikeColumnName], ValuesManager.ReturnValue(tableDataFromMainCatalog, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName)));
                    }
                    else if (idLikeColumnName != null && !mainCatalogValues.Contains(row[uniqueValueColumnName]) && row[uniqueValueColumnName] != null)
                    {
                        string oldKey = row[idLikeColumnName];
                        row[idLikeColumnName] = $"'{countOfImports + lastId + 1}'";
                        ValuesManager.AddPairKeysToRewriteDict(foreigns, idLikeColumnName, new Tuple<string, string>(oldKey, row[idLikeColumnName]));
                        ValuesManager.SelectImportMethod(mainCatalog, new Dictionary<string, string>(row), tableName, worker);
                        mainCatalogValues.Add(row[uniqueValueColumnName]);
                        countOfImports++;
                    }
                    worker.ReportProgress(Consts.MergeProgress.UpdateBlockBar(), Consts.WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
                }
            }

            if (Consts.FAST_REQUEST_MOD && countOfImports > 0)
            {
                if (ValuesManager.CountOfInsertValues() > Consts.MAX_COUNT_OF_IMPORTS)
                {
                    while (ValuesManager.CountOfInsertValues() != 0)
                        mainCatalog.SpecialInsertListOfValues(tableName, ValuesManager.ReturnRequestsToTable(Consts.MAX_COUNT_OF_IMPORTS), excludeColumns);
                }
                else
                {
                    mainCatalog.SpecialInsertListOfValues(tableName, ValuesManager.ReturnRequestsToTable(Consts.MAX_COUNT_OF_IMPORTS), excludeColumns);
                }
            }

            return countOfImports;
        }

        static int ProcessLinksTable(BackgroundWorker worker, DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName, string uniqueValueColumnName, string idLikeColumnName, string highLevelColumnName, string parentIdColumn, string numerateColumn, List<string> extraFilterColumns, List<string> excludeColumns, bool allowsNull)
        {
            int countOfImports = 0;

            if (Consts.FAST_REQUEST_MOD)
            {
                ValuesManager.ClearRequestsToTable();
            }
            
            long lastId = 0;
            if (idLikeColumnName != null)
            {
                string lastIDFromDB = string.Join("", mainCatalog.SelectLastRecord(idLikeColumnName, tableName, idLikeColumnName)).Replace("\'", "");
                lastId = (lastIDFromDB != "") ? Convert.ToInt64(lastIDFromDB) : 0;
            }

            int lastNumeric = 0;
            if (numerateColumn != null)
            {
                string lastNumricFromDB = string.Join("", mainCatalog.SelectLastRecord(numerateColumn, tableName, numerateColumn)).Replace("\'", "");
                lastNumeric = (lastNumricFromDB != "") ? Convert.ToInt32(lastNumricFromDB) : 0;
            }

            // 3. Берем таблицы в дальнейшем используемые
            Dictionary<string, string> foreigns = mainCatalog.SelectTablesAndForeignKeyUsage(tableName);

            // 4. Проверяем foreigns на наличие использования. Если True, то добавляем все в ReservDict
            if (foreigns.Count > 0)
            {
                ValuesManager.AddNewTableToReserve(foreigns);
            }

            List<Dictionary<string, string>> allFromMainCatalog = mainCatalog.SelectAllFrom(tableName, mainCatalog.SelectColumnsNames(tableName, excludeColumns), allowsNull);
            List<Dictionary<string, string>> allFromDaughterCatalog = daughterCatalog.SelectAllFrom(tableName, daughterCatalog.SelectColumnsNames(tableName, excludeColumns), allowsNull);
            Dictionary<string, List<Tuple<string, string>>> tableReservedValues = new Dictionary<string, List<Tuple<string, string>>>();

            Consts.MergeProgress.ClearTasksBlock();
            worker.ReportProgress(Consts.MergeProgress.COUNT_OF_ALL_BLOCK_TASKS, Consts.WorkerConsts.CLEAN_PROGRESS_BAR);
            if (parentIdColumn != null)
            {
                tableReservedValues = ValuesManager.ReturnTableValuesReserveDict(tableName);
                Consts.MergeProgress.AddTaskInBlock(tableReservedValues[parentIdColumn].Count);
            }

            Consts.MergeProgress.AddTaskInBlock(allFromDaughterCatalog.Count);
            


            if (tableName == "tblDOCUMENT_STATS")
            {
                string secondParent = "ISN_INVENTORY";
                


                List<Tuple<string, string>> secondParentTuplesKeys = ValuesManager.ReturnTuplesValuesSpecialDict(tableName, secondParent);

                List<Tuple<string, string>> oldTwoParents = ValuesManager.ReturnFilteredTuples(allFromDaughterCatalog, parentIdColumn, secondParent);
                

                // parentIdTuple = ISN_FUND
                foreach (Tuple<string, string> parentIdTuple in tableReservedValues[parentIdColumn])
                {
                    // если фонд был новый, то и записей не будет. 
                    List<Dictionary<string, string>> documentsOnFundInMain = ValuesManager.FilterRecordsFrom(allFromMainCatalog, parentIdColumn, parentIdTuple.Item2);
                    
                    // если DocStats уже присутсвуют для parentid
                    if (documentsOnFundInMain.Count > 0)
                    {
                        // ----new block----

                        // documentsOnFundInMain содержит записи для фонда и для описей. Можно убрать записи, у которых 'null'
                        // Возникает, когда фонд не присутствовал в главной. Если данный фонд уже есть, то получаем значения. Для поиска новых нужно придумать фильтрацию

                        // доп проверку на вхождения всех записей. Что-то типа чекнуть по второму родителю. Если таких записей нет, то тот же цикл когда нету, но с доп поиском.



                        // поулчение записей по тому же фонду, но который в дочерней
                        List<Dictionary<string, string>> documentsOnFundInDaughter = ValuesManager.FilterRecordsFrom(allFromDaughterCatalog, parentIdColumn, parentIdTuple.Item2);
                        List<Dictionary<string, string>> filteringByOnlyInventoryInMain = new List<Dictionary<string, string>>();
                        List<Dictionary<string, string>> filteringByOnlyInventoryInDaughter = new List<Dictionary<string, string>>();

                        // отбираем записи, которые отвечают за пересчет по описям
                        foreach (Dictionary<string, string> row in documentsOnFundInMain)
                        {
                            if (row[secondParent] != "'null'")
                                filteringByOnlyInventoryInMain.Add(new Dictionary<string, string>(row));
                        }

                        // если по второму родителю записей нет, то тут все описи новые, можно передавать на перезапись и в импорт
                        if (filteringByOnlyInventoryInMain.Count == 0)
                        {
                            // отбираем нужные записи на импорт
                            List<string> existsSecondParents = new List<string>();
                            foreach (Dictionary<string, string> row in documentsOnFundInDaughter)
                            {
                                if (row[secondParent] != "'null'")
                                    filteringByOnlyInventoryInDaughter.Add(new Dictionary<string, string>(row));
                            }

                            /*foreach (Dictionary<string, string> row in filteringByOnlyInventoryInDaughter)
                            {
                                MessageBox.Show(row[secondParent]);
                            }*/

                            // -------
                            /*
                                                        foreach (Dictionary<string, string> row in filteringByOnlyInventoryInDaughter)
                                                        {
                                                            MessageBox.Show(row[parentIdColumn] + "   " + row[secondParent]);
                                                        }*/


                            foreach (Dictionary<string, string> row in filteringByOnlyInventoryInDaughter)
                            {
                                Dictionary<string, string> onImportRow = new Dictionary<string, string>(row);
                                onImportRow[parentIdColumn] = parentIdTuple.Item2;
                                onImportRow[idLikeColumnName] = $"'{lastId + countOfImports + 1}'";
                                onImportRow[secondParent] = HelpFunction.SearchSecondParent(onImportRow[secondParent], secondParentTuplesKeys);
                                onImportRow[ExtraIDColumn] = mainCatalog.SelectIDFrom(mainCatalog.SelectReferenceTableName(tableName, secondParent), secondParent, onImportRow[secondParent]);



                                // --------------!!----------was been deleted after pass tests-----!!------
/*                                foreach (Tuple<string, string> twoParents in oldTwoParents)
                                {
                                    
                                    if (parentIdTuple.Item1 == twoParents.Item1 && twoParents.Item2 != "'null'")
                                    {
                                        // 133 + 133 + 250 или 133 + 133 + 249
                                        //MessageBox.Show(parentIdTuple.Item1 + "    " + twoParents.Item1 + "   " + twoParents.Item2);
                                        // тут определили первого родителя parentIdTuple.Item1

                                        foreach (Tuple<string, string> secondPair in secondParentTuplesKeys)
                                        {
                                            if (twoParents.Item2 == secondPair.Item1)
                                            {
                                                // 250 + 250 + 249 или 249 + 249 + 250
                                                //MessageBox.Show(twoParents.Item2 + "    " + secondPair.Item1 + "   " + secondPair.Item2);
                                                onImportRow[secondParent] = secondPair.Item2;


                                                //MessageBox.Show(onImportRow["DocID"] + "    " + onImportRow[secondParent] + "   sec1  "  + secondPair.Item2);



                                                onImportRow["DocID"] = mainCatalog.SelectIDFrom(mainCatalog.SelectReferenceTableName(tableName, secondParent), secondParent, secondPair.Item2);

                                                MessageBox.Show(onImportRow[idLikeColumnName] + "    " + onImportRow["DocID"] + "    " + onImportRow[secondParent] + "   sec2  " + secondPair.Item2);
                                                //MessageBox.Show(row[secondParent] + "   " + row[secondParent] + "    " + secondPair.Item1);
                                                //MessageBox.Show(onImportRow["DocID"] + "    " + onImportRow[secondParent]);
                                                asd.Add(new Dictionary<string, string>(onImportRow));
                                            }
                                        }
                                    }
                                }*/
                                // ------------------------!!!!-----------

                                ValuesManager.SelectImportMethod(mainCatalog, new Dictionary<string, string>(onImportRow), tableName, worker);
                                countOfImports++;                               
                            }
                        }
                        // в фонде есть описи, которые уже присутствуют, но так же есть и новые.
                        else
                        {
                            List<string> existingID = new List<string>();
                            foreach (Dictionary<string, string> row in filteringByOnlyInventoryInMain)
                            {
                                if (row[secondParent] != "'null'" && !existingID.Contains(row[secondParent]))
                                    existingID.Add(row[secondParent]);
                            }

                            foreach (Dictionary<string, string> row in documentsOnFundInDaughter)
                            {
                                if (row[secondParent] != "'null'")
                                    filteringByOnlyInventoryInDaughter.Add(new Dictionary<string, string>(row));
                            }

                            List<Dictionary<string, string>> byOnlyNewInventory = new List<Dictionary<string, string>>();
                            foreach (Dictionary<string, string> row in filteringByOnlyInventoryInDaughter)
                            {
                                if (!existingID.Contains(row[secondParent]))
                                    byOnlyNewInventory.Add(new Dictionary<string, string>(row));
                            }

                            //List<Dictionary<string, string>> documentsOnFundInDaughter = ValuesManager.FilterRecordsFrom(allFromDaughterCatalog, parentIdColumn, parentIdTuple.Item2);
                            foreach (Dictionary<string, string> row in byOnlyNewInventory)
                            {
                                ValuesManager.UpdateCheck(worker);
                                Dictionary<string, string> onImportRow = new Dictionary<string, string>(row);

                                onImportRow[parentIdColumn] = parentIdTuple.Item2;
                                onImportRow[idLikeColumnName] = $"'{lastId + countOfImports + 1}'";

                                if (onImportRow[secondParent] == "'null'")
                                {
                                    onImportRow[ExtraIDColumn] = mainCatalog.SelectIDFrom(mainCatalog.SelectReferenceTableName(tableName, parentIdColumn), parentIdColumn, parentIdTuple.Item2);
                                }
                                else
                                {
                                    onImportRow[secondParent] = HelpFunction.SearchSecondParent(onImportRow[secondParent], secondParentTuplesKeys);
                                    onImportRow[ExtraIDColumn] = mainCatalog.SelectIDFrom(mainCatalog.SelectReferenceTableName(tableName, secondParent), secondParent, onImportRow[secondParent]);
                                }

                                ValuesManager.SelectImportMethod(mainCatalog, new Dictionary<string, string>(onImportRow), tableName, worker);
                                countOfImports++;
                            }
                        }                      
                        // ----new block----
                    }
                    else
                    {
                        List<Dictionary<string, string>> documentsOnFundInDaughter = ValuesManager.FilterRecordsFrom(allFromDaughterCatalog, parentIdColumn, parentIdTuple.Item2);
                        // Если есть новые фонды или фонды и описи в них
                        foreach (Dictionary<string, string> row in documentsOnFundInDaughter)
                        {
                            ValuesManager.UpdateCheck(worker);
                            Dictionary<string, string> onImportRow = new Dictionary<string, string>(row);

                            onImportRow[parentIdColumn] = parentIdTuple.Item2;
                            onImportRow[idLikeColumnName] = $"'{lastId + countOfImports + 1}'";

                            if (onImportRow[secondParent] == "'null'")
                            {
                                onImportRow[ExtraIDColumn] = mainCatalog.SelectIDFrom(mainCatalog.SelectReferenceTableName(tableName, parentIdColumn), parentIdColumn, parentIdTuple.Item2);
                            }
                            else
                            {
                                onImportRow[secondParent] = HelpFunction.SearchSecondParent(onImportRow[secondParent], secondParentTuplesKeys);
                                onImportRow[ExtraIDColumn] = mainCatalog.SelectIDFrom(mainCatalog.SelectReferenceTableName(tableName, secondParent), secondParent, onImportRow[secondParent]);

                                /*foreach (Tuple<string, string> twoParents in oldTwoParents)
                                {
                                    if (parentIdTuple.Item1 == twoParents.Item1)
                                    {
                                        foreach (Tuple<string, string> secondPair in secondParentTuplesKeys)
                                        {
                                            if (twoParents.Item1 == secondPair.Item1)
                                            {
                                                onImportRow[secondParent] = secondPair.Item2;

                                                onImportRow["DocID"] = mainCatalog.SelectIDFrom(mainCatalog.SelectReferenceTableName(tableName, secondParent), secondParent, parentIdTuple.Item2);

                                                if (onImportRow["DocID"] == null)
                                                    MessageBox.Show(onImportRow["DocID"].ToString());
                                                //row["DocID"] = mainCatalog.SelectIDFrom("tblINVENTORY", "ISN_INVENTORY", secondPair.Item2);
                                                //MessageBox.Show("INV   " + row["DocID"]);
                                            }
                                        }
                                    }
                                }*/
                            }

                            ValuesManager.SelectImportMethod(mainCatalog, new Dictionary<string, string>(onImportRow), tableName, worker);
                            countOfImports++;
                        }
                    }

                    //worker.ReportProgress(Consts.MergeProgress.UpdateBlockBar(), Consts.WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
                }
            }
            else if (tableName == "tblARCHIVE")
            {
                //List<Dictionary<string, string>> allFromMainCatalog = mainCatalog.SelectAllFrom(tableName, mainCatalog.SelectColumnsNames(tableName, excludeColumns), allowsNull);
                //List<Dictionary<string, string>> allFromDaughterCatalog = daughterCatalog.SelectAllFrom(tableName, daughterCatalog.SelectColumnsNames(tableName, excludeColumns), allowsNull);

                ValuesManager.AddTablesToRewriteDict(foreigns);
                ValuesManager.AddPairKeysToRewriteDict(foreigns, idLikeColumnName, new Tuple<string, string>(allFromDaughterCatalog[0][idLikeColumnName], allFromMainCatalog[0][idLikeColumnName]));

/*                if (Consts.MAKE_EDITS)
                {
                    
                }*/
                
            }
            else if (uniqueValueColumnName != null && idLikeColumnName != null && highLevelColumnName == null && parentIdColumn == null)
            {
                // 2. Берем список значений по уникальному полю из главного каталога
                List<string> mainCatalogValues = ValuesManager.SelectDataFromColumn(allFromMainCatalog, uniqueValueColumnName);

                //Consts.MergeProgress.AddTaskInBlock(allFromDaughterCatalog.Count);

                foreach (Dictionary<string, string> row in allFromDaughterCatalog)
                {
                    ValuesManager.UpdateCheck(worker);

                    // Если значение ИМЕЕТСЯ в главном каталоге
                    if (mainCatalogValues.Contains(row[uniqueValueColumnName]))
                    {
                        // и используется дальше, то добавляем 
                        if (foreigns.Count > 0)
                        {
                            if (extraFilterColumns != null)
                            {
                                ValuesManager.AddPairKeysToReserve(foreigns, idLikeColumnName, new Tuple<string, string>(row[idLikeColumnName], ValuesManager.RecursionFilterValue(row, allFromMainCatalog, new List<string>(extraFilterColumns), idLikeColumnName)));
                            }
                            else
                            {
                                ValuesManager.AddPairKeysToReserve(foreigns, idLikeColumnName, new Tuple<string, string>(row[idLikeColumnName], ValuesManager.ReturnValue(allFromMainCatalog, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName)));
                            }
                            // MessageBox.Show("old: " + row[idLikeColumnName] + "   new: " + ValuesManager.ReturnValue(allFromMainData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName));
                            //ValuesManager.AddPairKeysToReserve(foreigns, idLikeColumnName, new Tuple<string, string>(row[idLikeColumnName], ValuesManager.ReturnValue(allFromMainData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName)));
                        }
                    } // Если значения НЕТ в главном каталоге
                    else if (!mainCatalogValues.Contains(row[uniqueValueColumnName]))
                    {
                        string oldKey = row[idLikeColumnName];
                        row[idLikeColumnName] = $"'{lastId + countOfImports + 1}'";

                        if (numerateColumn != null)
                        {
                            row[numerateColumn] = $"'{lastNumeric + countOfImports + 1}'";
                        }

                        if (foreigns.Count > 0)
                        {
                            ValuesManager.AddPairKeysToReserve(foreigns, idLikeColumnName, new Tuple<string, string>(oldKey, row[idLikeColumnName]));
                        }

                        ValuesManager.SelectImportMethod(mainCatalog, new Dictionary<string, string>(row), tableName, worker);

                        // new string
                        mainCatalogValues.Add(row[uniqueValueColumnName]);


                        countOfImports++;
                    }
                    worker.ReportProgress(Consts.MergeProgress.UpdateBlockBar(), Consts.WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
                }
            }
            else if (uniqueValueColumnName != null && idLikeColumnName != null && highLevelColumnName == null && parentIdColumn != null)
            {
                if (tableName == SpecialTablesValues.SpecialTablePair.Item1)
                    ValuesManager.AddNewTableToSpecialRewrite(foreigns);

                //Dictionary<string, List<Tuple<string, string>>> tableReservedValues = ValuesManager.ReturnTableValuesReserveDict(tableName);
                // List<Dictionary<string, string>> reservedRows = new List<Dictionary<string, string>>();

                //Consts.MergeProgress.AddTaskInBlock(tableReservedValues[parentIdColumn].Count);


                /*if (tableName == "tblUNIT")
                    MessageBox.Show(tableName);*/


                //Consts.MergeProgress.ClearTasksBlock();
                //Consts.MergeProgress.AddTaskInBlock(tableReservedValues[parentIdColumn].Count);





                foreach (Tuple<string, string> pairKeys in tableReservedValues[parentIdColumn])
                {

                    //pairKeys.Item1; // значения ключей, которые были в дочерней
                    //pairKeys.Item2; // значения ключей, которые теперь в главной


                    List<Dictionary<string, string>> filterFromMainData = ValuesManager.FilterRecordsFrom(new List<Dictionary<string, string>>(allFromMainCatalog), parentIdColumn, pairKeys.Item2);
                    List<Dictionary<string, string>> filterFromDaughterData = ValuesManager.FilterRecordsFrom(new List<Dictionary<string, string>>(allFromDaughterCatalog), parentIdColumn, pairKeys.Item1);

                    List<string> mainCatalogValues = ValuesManager.SelectDataFromColumn(filterFromMainData, uniqueValueColumnName);

                    if (numerateColumn != null)
                    {
                        lastNumeric = filterFromMainData.Count;
                    }

                    foreach (Dictionary<string, string> row in filterFromDaughterData)
                    {
                        ValuesManager.UpdateCheck(worker);

                        if (mainCatalogValues.Contains(row[uniqueValueColumnName]))
                        {
                            if (foreigns.Count > 0)
                            {
                                if (extraFilterColumns != null)
                                {
                                    ValuesManager.AddPairKeysToReserve(foreigns, idLikeColumnName, new Tuple<string, string>(row[idLikeColumnName], ValuesManager.RecursionFilterValue(row, filterFromMainData, new List<string>(extraFilterColumns), idLikeColumnName)));
                                }
                                else
                                {
                                    ValuesManager.AddPairKeysToReserve(foreigns, idLikeColumnName, new Tuple<string, string>(row[idLikeColumnName], ValuesManager.ReturnValue(filterFromMainData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName)));
                                }

                                if (tableName == SpecialTablesValues.SpecialTablePair.Item1)
                                    ValuesManager.AddPairKeysToSpecialRewrite(foreigns, idLikeColumnName, new Tuple<string, string>(row[idLikeColumnName], ValuesManager.ReturnValue(filterFromMainData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName)));
                            }
                        }
                        else if (!mainCatalogValues.Contains(row[uniqueValueColumnName]))
                        {
                            Dictionary<string, string> onImportRow = new Dictionary<string, string>(row);
                            string oldKey = onImportRow[idLikeColumnName];
                            onImportRow[idLikeColumnName] = $"'{lastId + countOfImports + 1}'";
                            onImportRow[parentIdColumn] = pairKeys.Item2;

                            if (numerateColumn != null)
                            {
                                onImportRow[numerateColumn] = $"'{lastNumeric + countOfImports + 1}'";
                            }

                            if (foreigns.Count > 0)
                            {
                                ValuesManager.AddPairKeysToReserve(foreigns, idLikeColumnName, new Tuple<string, string>(oldKey, onImportRow[idLikeColumnName]));
                                if (tableName == SpecialTablesValues.SpecialTablePair.Item1)
                                    ValuesManager.AddPairKeysToSpecialRewrite(foreigns, idLikeColumnName, new Tuple<string, string>(oldKey, onImportRow[idLikeColumnName]));
                            }

                            if (onImportRow.ContainsKey(MergeSettings.ExtraIDColumn))
                                onImportRow[ExtraIDColumn] = mainCatalog.SelectIDFrom(mainCatalog.SelectReferenceTableName(tableName, parentIdColumn), parentIdColumn, pairKeys.Item2);

                            ValuesManager.SelectImportMethod(mainCatalog, new Dictionary<string, string>(onImportRow), tableName, worker);

                            // new string
                            //mainCatalogValues.Add(row[uniqueValueColumnName]);

                            countOfImports++;
                        }
                        
                    }
                    worker.ReportProgress(Consts.MergeProgress.UpdateBlockBar(), Consts.WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
/*                    if (tableName == "tblUNIT")
                        MessageBox.Show(Consts.MergeProgress.COUNT_OF_ALL_BLOCK_TASKS.ToString() + "   " + Consts.MergeProgress.BLOCK_PROGRESS_NOW.ToString());*/
                }
            }
            else if (uniqueValueColumnName == null && idLikeColumnName == null && highLevelColumnName == null && parentIdColumn != null)
            {
                //Dictionary<string, List<Tuple<string, string>>> tableReservedValues = ValuesManager.ReturnTableValuesReserveDict(tableName);

                //Consts.MergeProgress.AddTaskInBlock(allFromDaughterCatalog.Count);

                //Consts.MergeProgress.ClearTasksBlock();
                //Consts.MergeProgress.AddTaskInBlock(tableReservedValues[parentIdColumn].Count);
               /* if (tableName == "tblINVENTORY_CHECK")
                    MessageBox.Show(tableReservedValues[parentIdColumn].Count.ToString() + "      "+ allFromDaughterCatalog.Count.ToString());*/
                foreach (Tuple<string, string> tuple in tableReservedValues[parentIdColumn])
                {
                    List<Dictionary<string, string>> allFromMainDataWhere = ValuesManager.FilterRecordsFrom(allFromMainCatalog, parentIdColumn, tuple.Item2);

                    if (allFromMainDataWhere.Count > 0)
                    {
                        continue;
                    }
                    else
                    {
/*                        if (tableName == "tblINVENTORY_CHECK")
                            MessageBox.Show(tuple.Item1 + "    " + tuple.Item2);*/
                        foreach (Dictionary<string, string> row in allFromDaughterCatalog)
                        {
/*                            if (tableName == "tblINVENTORY_CHECK")
                                MessageBox.Show(row[parentIdColumn]);
                            ValuesManager.UpdateCheck(worker);*/

                            if (row[parentIdColumn] == tuple.Item1)
                            {
                                Dictionary<string, string> onImportRow = new Dictionary<string, string>(row);
                                //MessageBox.Show(row[uniqueValueColumnName] + " " + row[parentIdColumn] + " " + tuple.Item1);
                                onImportRow[parentIdColumn] = tuple.Item2;
                                //row["DocID"] = mainCatalog.SelectIDFrom(mainCatalog.SelectReferenceTableName(tableName, parentIdColumn), parentIdColumn, tuple.Item2);

                                if (onImportRow.ContainsKey(MergeSettings.ExtraIDColumn))
                                    onImportRow[ExtraIDColumn] = mainCatalog.SelectIDFrom(mainCatalog.SelectReferenceTableName(tableName, parentIdColumn), parentIdColumn, tuple.Item2);

                                //MessageBox.Show(row[parentIdColumn]);
                                ValuesManager.SelectImportMethod(mainCatalog, new Dictionary<string, string>(onImportRow), tableName, worker);

                                countOfImports++;
                            }
                        }
                    }
                    worker.ReportProgress(Consts.MergeProgress.UpdateBlockBar(), Consts.WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
                }
            }
            else if (uniqueValueColumnName != null && idLikeColumnName != null && highLevelColumnName != null && parentIdColumn == null)
            {
                List<string> mainCatalogValues = ValuesManager.SelectDataFromColumn(allFromMainCatalog, uniqueValueColumnName);
               

                if (foreigns.Count > 0)
                {
                    ValuesManager.AddTablesToRewriteDict(foreigns);
                }

                //Consts.MergeProgress.AddTaskInBlock(allFromDaughterCatalog.Count);
                //MessageBox.Show(Consts.MergeProgress.COUNT_OF_ALL_BLOCK_TASKS.ToString());
                // ----
                List<Dictionary<string, string>> copyOfAll = new List<Dictionary<string, string>>();
                foreach (Dictionary<string, string> row in allFromDaughterCatalog)
                    copyOfAll.Add(new Dictionary<string, string>(row));

                List<Dictionary<string, string>> copyOfcopyOfAll = new List<Dictionary<string, string>>(copyOfAll);
                //Consts.MergeProgress.ClearTasksBlock();
                //Consts.MergeProgress.AddTaskInBlock(allFromDaughterCatalog.Count);
                //MessageBox.Show(Consts.MergeProgress.COUNT_OF_ALL_BLOCK_TASKS.ToString());
                while (copyOfAll.Count != 0)
                {
                    foreach (Dictionary<string, string> row in copyOfAll)
                    {
                        ValuesManager.UpdateCheck(worker);

                        if (mainCatalogValues.Contains(row[uniqueValueColumnName]))
                        {
                            ValuesManager.AddPairKeysToRewriteDict(foreigns, idLikeColumnName, new Tuple<string, string>(row[idLikeColumnName], ValuesManager.ReturnValue(allFromMainCatalog, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName)));
                            copyOfcopyOfAll.Remove(row);
                        }
                        else if (!mainCatalogValues.Contains(row[uniqueValueColumnName]))
                        {
                            string valueOnHighID = ValuesManager.ReturnValue(allFromDaughterCatalog, highLevelColumnName, row[highLevelColumnName], uniqueValueColumnName);
                            if (mainCatalogValues.Contains(valueOnHighID))
                            {                              
                                string oldKey = row[idLikeColumnName];
                                if (row[highLevelColumnName] != "'null'")
                                    row[highLevelColumnName] = ValuesManager.ReturnValue(allFromMainCatalog, uniqueValueColumnName, valueOnHighID, idLikeColumnName);
                                row[idLikeColumnName] = $"'{lastId + countOfImports + 1}'";
                                ValuesManager.AddPairKeysToRewriteDict(foreigns, idLikeColumnName, new Tuple<string, string>(oldKey, row[idLikeColumnName]));
                                ValuesManager.SelectImportMethod(mainCatalog, new Dictionary<string, string>(row), tableName, worker);
                                mainCatalogValues.Add(row[uniqueValueColumnName]);
                                allFromMainCatalog.Add(new Dictionary<string, string>(row));
                                copyOfcopyOfAll.Remove(row);
                                countOfImports++;
                            }
                            else if (valueOnHighID == "")
                            {
                                string oldKey = row[idLikeColumnName];
                                row[idLikeColumnName] = $"'{lastId + countOfImports + 1}'";
                                ValuesManager.AddPairKeysToRewriteDict(foreigns, idLikeColumnName, new Tuple<string, string>(oldKey, row[idLikeColumnName]));
                                ValuesManager.SelectImportMethod(mainCatalog, new Dictionary<string, string>(row), tableName, worker);
                                mainCatalogValues.Add(row[uniqueValueColumnName]);
                                allFromMainCatalog.Add(new Dictionary<string, string>(row));
                                copyOfcopyOfAll.Remove(row);
                                countOfImports++;
                            }
                            else if (!mainCatalogValues.Contains(ValuesManager.ReturnValue(allFromDaughterCatalog, highLevelColumnName, row[highLevelColumnName], uniqueValueColumnName)))
                            {
                                Consts.MergeProgress.AddTaskInBlock();
                            }
                        }
                        worker.ReportProgress(Consts.MergeProgress.UpdateBlockBar(), Consts.WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
                    }
                    copyOfAll.Clear();
                    foreach (Dictionary<string, string> row in copyOfcopyOfAll)
                        copyOfAll.Add(row);
                }
            }
            else if (uniqueValueColumnName == null && idLikeColumnName != null && highLevelColumnName == null && parentIdColumn != null)
            {
                //List<Dictionary<string, string>> allFromMainCatalog = mainCatalog.SelectAllFrom(tableName, mainCatalog.SelectColumnsNames(tableName, excludeColumns), allowsNull);
                //List<Dictionary<string, string>> allFromDaughterCatalog = daughterCatalog.SelectAllFrom(tableName, daughterCatalog.SelectColumnsNames(tableName, excludeColumns), allowsNull);
                //Dictionary<string, List<Tuple<string, string>>> tableReservedValues = ValuesManager.ReturnTableValuesReserveDict(tableName);


                //Consts.MergeProgress.ClearTasksBlock();
                //Consts.MergeProgress.AddTaskInBlock(tableReservedValues[parentIdColumn].Count);

                //Consts.MergeProgress.AddTaskInBlock(allFromDaughterCatalog.Count);

                //MessageBox.Show(tableName);

                foreach (Tuple<string, string> tuple in tableReservedValues[parentIdColumn])
                {
                    
                    /*if (tableName == "tblUNIT_STATE")
                    {
                        MessageBox.Show(tuple.Item1 + "   " + tuple.Item2);
                    }*/
                    List<Dictionary<string, string>> allFromMainDataWhere = ValuesManager.FilterRecordsFrom(allFromMainCatalog, parentIdColumn, tuple.Item2);

                    if (allFromMainDataWhere.Count > 0)
                    {
                        continue;
                    }
                    else
                    {
                        foreach (Dictionary<string, string> row in allFromDaughterCatalog)
                        {
                            ValuesManager.UpdateCheck(worker);

                            if (row[parentIdColumn] == tuple.Item1)
                            {
                                row[parentIdColumn] = tuple.Item2;
                                row[idLikeColumnName] = $"'{lastId + countOfImports + 1}'";

                                if (row.ContainsKey(MergeSettings.ExtraIDColumn))
                                    row[ExtraIDColumn] = mainCatalog.SelectIDFrom(mainCatalog.SelectReferenceTableName(tableName, parentIdColumn), parentIdColumn, tuple.Item2);

                                ValuesManager.SelectImportMethod(mainCatalog, new Dictionary<string, string>(row), tableName, worker);

                                countOfImports++;
                            }
                        }
                    }
                    worker.ReportProgress(Consts.MergeProgress.UpdateBlockBar(), Consts.WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
                }

            }
            else if (uniqueValueColumnName == null && idLikeColumnName != null && highLevelColumnName != null && parentIdColumn != null)
            {
                List<string> mainCatalogValues = ValuesManager.SelectDataFromColumn(allFromMainCatalog, highLevelColumnName);

                // List<Dictionary<string, string>> allFromDaughterDataWhere = daughterCatalog.SelectRecordsWhere(new List<string>, tableName, parentIdColumn, );

                //Consts.MergeProgress.ClearTasksBlock();
                //Consts.MergeProgress.AddTaskInBlock(tableReservedValues[parentIdColumn].Count);

                if (foreigns.Count > 0)
                {
                    ValuesManager.AddTablesToRewriteDict(foreigns);
                }


                foreach (Tuple<string, string> tuple in tableReservedValues[parentIdColumn])
                {
                    //List<Dictionary<string, string>> allFromMainDataWhere = mainCatalog.SelectRecordsWhere(new List<string>(), tableName, parentIdColumn, tuple.Item2);

                    List<Dictionary<string, string>> allFromMainDataWhere = ValuesManager.FilterRecordsFrom(allFromMainCatalog, parentIdColumn, tuple.Item2);

                    if (allFromMainDataWhere.Count > 0)
                    {
                        continue;
                    }
                    else
                    {
                        foreach (Dictionary<string, string> row in allFromDaughterCatalog)
                        {
                            ValuesManager.UpdateCheck(worker);

                            if (row[parentIdColumn] == tuple.Item1)
                            {
                                string oldKey = row[idLikeColumnName];
                                row[parentIdColumn] = tuple.Item2;
                                row[idLikeColumnName] = $"'{lastId + countOfImports + 1}'";

                                if (!mainCatalogValues.Contains(row[highLevelColumnName]))
                                {
                                    row[highLevelColumnName] = "'null'";
                                    //foreach (Dictionary<string, string> sameRow in allFromDaughterData)
                                    //{
                                    //    if (sameRow[idLikeColumnName] == row[highLevelColumnName] && ValuesManager.ContainsInRewrite(tableName))
                                    //    {
                                    //        mainCatalog.InsertValue(tableName, ValuesManager.RepareColumnsValues(ValuesManager.RemoveUnnecessary(sameRow, excludeColumns), tableName));
                                    //        mainCatalogValues.Add(sameRow[idLikeColumnName]);
                                    //        break;
                                    //    }
                                    //    else if (sameRow[idLikeColumnName] == row[highLevelColumnName] && !ValuesManager.ContainsInRewrite(tableName))
                                    //    {
                                    //        mainCatalog.InsertValue(tableName, ValuesManager.RemoveUnnecessary(sameRow, excludeColumns));
                                    //        mainCatalogValues.Add(sameRow[idLikeColumnName]);
                                    //        break;
                                    //    }
                                    //}
                                }


                                /*if (ValuesManager.ContainsInRewrite(tableName))
                                {
                                    mainCatalog.InsertValue(tableName, ValuesManager.RepareColumnsValues(ValuesManager.RemoveUnnecessary(row, excludeColumns), tableName));
                                }
                                else
                                {
                                    mainCatalog.InsertValue(tableName, ValuesManager.RemoveUnnecessary(row, excludeColumns));
                                }*/

                                //ValuesManager.InsertNewValue(mainCatalog, row, tableName, excludeColumns);

                                if (row.ContainsKey(MergeSettings.ExtraIDColumn))
                                    row[ExtraIDColumn] = mainCatalog.SelectIDFrom(mainCatalog.SelectReferenceTableName(tableName, parentIdColumn), parentIdColumn, tuple.Item2);

                                ValuesManager.SelectImportMethod(mainCatalog, new Dictionary<string, string>(row), tableName, worker);


                                ValuesManager.AddPairKeysToRewriteDict(foreigns, idLikeColumnName, new Tuple<string, string>(oldKey, row[idLikeColumnName]));
                                countOfImports++;
                            }
                        }

                    }
                    worker.ReportProgress(Consts.MergeProgress.UpdateBlockBar(), Consts.WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
                }



            }












            //else if (uniqueValueColumnName != null && idLikeColumnName == null && parentIdColumn != null && highLevelColumnName != null)
            //{
            //    ValuesManager.AddTablesToRewriteDict(foreigns);




            //    List<Dictionary<string, string>> allFromMainData = mainCatalog.SelectAllFrom(tableName);
            //    List<string> mainCatalogValues = ValuesManager.SelectDataFromColumn(allFromMainData, uniqueValueColumnName);
            //    List<Dictionary<string, string>> allFromDaughterData = daughterCatalog.SelectAllFrom(tableName);
            //    Dictionary<string, List<Tuple<string, string>>> tableReservedValues = ValuesManager.ReturnTableValuesReserveDict(tableName);

            //    Consts.AddTaskInBlock(allFromDaughterData.Count);





            //    foreach (Dictionary<string, string> row in allFromDaughterData)
            //    {

            //        foreach (Tuple<string, string> tuple in tableReservedValues[parentIdColumn])
            //        {
            //            if (row[parentIdColumn] == tuple.Item1)
            //            {
            //                row[parentIdColumn] = tuple.Item2;


            //                if (mainCatalogValues.Contains(row[uniqueValueColumnName]))
            //                {
            //                    ValuesManager.AddPairKeysToRewriteDict(foreigns, idLikeColumnName, new Tuple<string, string>(row[idLikeColumnName], ValuesManager.ReturnValue(allFromMainData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName)));
            //                }
            //                else if (!mainCatalogValues.Contains(row[uniqueValueColumnName]))
            //                {
            //                    row[idLikeColumnName] = $"'{lastId + countOfImports + 1}'";

            //                    ValuesManager.AddPairKeysToRewriteDict(foreigns, idLikeColumnName, new Tuple<string, string>(ValuesManager.ReturnValue(allFromDaughterData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName), row[idLikeColumnName]));

            //                    if (ValuesManager.ContainsInRewrite(tableName))
            //                    {
            //                        mainCatalog.InsertValue(tableName, ValuesManager.RepareColumnsValues(ValuesManager.RemoveUnnecessary(row, excludeColumns), tableName));
            //                    }
            //                    else
            //                    {
            //                        mainCatalog.InsertValue(tableName, ValuesManager.RemoveUnnecessary(row, excludeColumns));
            //                    }

            //                    mainCatalogValues.Add(row[uniqueValueColumnName]);
            //                    countOfImports++;
            //                }
            //                break;
            //            }
            //        }
            //        worker.ReportProgress(Consts.UpdateBlockBar(), WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
            //    }
            //    // ValuesManager.DeleteTableFromReserve(tableName);
            //}

            //MessageBox.Show(tableName + "   " + mainCatalog.SelectCountRowsTable(tableName));

            //MessageBox.Show(countOfImports + "      " + countOfRequests);

            if (Consts.FAST_REQUEST_MOD && countOfImports > 0)
            {
                if (ValuesManager.CountOfInsertValues() > Consts.MAX_COUNT_OF_IMPORTS)
                {
                    //string requests = "";
                    while (ValuesManager.CountOfInsertValues() != 0)
                    {
                        mainCatalog.SpecialInsertListOfValues(tableName, ValuesManager.ReturnRequestsToTable(Consts.MAX_COUNT_OF_IMPORTS), excludeColumns);
                        //requests += mainCatalog.ListOfValues(tableName, ValuesManager.ReturnRequestsToTable(Consts.MAX_COUNT_OF_IMPORTS));
                    }
                    //mainCatalog.InsertListOfValues(requests);
                }
                else
                {
                    mainCatalog.SpecialInsertListOfValues(tableName, ValuesManager.ReturnRequestsToTable(Consts.MAX_COUNT_OF_IMPORTS), excludeColumns);
                }
            }
            //MessageBox.Show(tableName + " "  + ValuesManager.CountOfInsertValues().ToString());
            return countOfImports;
        }

        public static bool RenameBeforeMergeTableColumn(DBCatalog mainCatalog, DBCatalog daughterCatalog, BackgroundWorker worker)
        {
            if (Consts.DEBUG_MOD)
            {
                foreach (string tableName in SpecialTablesValues.RenamedColumns.Keys)
                {
                    mainCatalog.RenameColumn(tableName, SpecialTablesValues.RenamedColumns[tableName].Item1.Item1, SpecialTablesValues.RenamedColumns[tableName].Item1.Item2);
                    daughterCatalog.RenameColumn(tableName, SpecialTablesValues.RenamedColumns[tableName].Item1.Item1, SpecialTablesValues.RenamedColumns[tableName].Item1.Item2);
                }
            }
            else
            {
                try
                {
                    foreach (string tableName in SpecialTablesValues.RenamedColumns.Keys)
                    {
                        mainCatalog.RenameColumn(tableName, SpecialTablesValues.RenamedColumns[tableName].Item1.Item1, SpecialTablesValues.RenamedColumns[tableName].Item1.Item2);
                        daughterCatalog.RenameColumn(tableName, SpecialTablesValues.RenamedColumns[tableName].Item1.Item1, SpecialTablesValues.RenamedColumns[tableName].Item1.Item2);
                    }
                }
                catch (Exception error)
                {
                    worker.ReportProgress(Consts.WorkerConsts.ERROR_STATUS_CODE, error.Message);
                    return false;
                }
            }
            return true;
        }

        public static bool RenameAfterMergeTableColumn(DBCatalog mainCatalog, BackgroundWorker worker)
        {
            if (Consts.DEBUG_MOD)
            {
                foreach (string tableName in SpecialTablesValues.RenamedColumns.Keys)
                {
                    mainCatalog.RenameColumn(tableName, SpecialTablesValues.RenamedColumns[tableName].Item2.Item1, SpecialTablesValues.RenamedColumns[tableName].Item2.Item2);
                }
            }
            else
            {
                try
                {
                    foreach (string tableName in SpecialTablesValues.RenamedColumns.Keys)
                    {
                        mainCatalog.RenameColumn(tableName, SpecialTablesValues.RenamedColumns[tableName].Item2.Item1, SpecialTablesValues.RenamedColumns[tableName].Item2.Item2);
                    }
                }
                catch (Exception error)
                {
                    worker.ReportProgress(Consts.WorkerConsts.ERROR_STATUS_CODE, error.Message);
                    return false;
                }
            }
            return true;
        }
    }
}
