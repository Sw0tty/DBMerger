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
using System.Windows.Forms.VisualStyles;


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

                try
                {
                    int recordsCount = mainCatalog.ClearTable(logTable);

                    if (mainCatalog.SelectCountRowsTable(logTable) == 0)
                    {
                        worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + "Пустая таблица.");
                    }
                    else
                    {
                        worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + $"Записей удалено {recordsCount}");
                    }
                    worker.ReportProgress(Consts.MergeProgress.UpdateMainBar(), Consts.WorkerConsts.ITS_MAIN_PROGRESS_BAR);
                }
                catch {
                    return false;
                }
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
                worker.ReportProgress(Consts.MergeProgress.UpdateMainBar(), Consts.WorkerConsts.ITS_MAIN_PROGRESS_BAR);
            }
        }

        public static bool ProcessDefaultTables(DBCatalog mainCatalog, DBCatalog daughterCatalog, BackgroundWorker worker)
        {
            //ReturnProcessStatus(worker, mainCatalog, defaultTablesFunctions, daughterCatalog);


            foreach (string tableName in mainCatalog.SelectDefaultProcessingTables())
            {
                int mainCount = mainCatalog.SelectCountRowsTable(tableName);
                int daughterCount = daughterCatalog.SelectCountRowsTable(tableName);


                worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, $"Обработка {tableName}:");

                if (daughterCatalog.SelectCountRowsTable(tableName) == 0)
                {
                    worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + "Пустая таблица.");
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
                int mainCount = mainCatalog.SelectCountRowsTable(tableName);
                int daughterCount = daughterCatalog.SelectCountRowsTable(tableName);


                worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, $"Обработка {tableName}:");

                if (daughterCatalog.SelectCountRowsTable(tableName) == 0)
                {
                    worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + "Пустая таблица.");
                }
                else if (LinksTablesParams.ContainsKey(tableName))
                {
                    Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>> paramsForProcessing = LinksTablesParams[tableName];
                    // 1. string uniqueValueColumnName         2. string idLikeColumnName         3. List<string> excludeColumns    4. string highLevelColumnName     5. string parentIdColumn         6. string numerateColumn                          bool usedFurther, string foreignIdColumn = null

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
                }
                worker.ReportProgress(Consts.MergeProgress.UpdateMainBar(), Consts.WorkerConsts.ITS_MAIN_PROGRESS_BAR);
            }
            return true;
        }

        static void ReturnProcessStatus(BackgroundWorker worker, DBCatalog mainCatalog, Dictionary<string, Func<DBCatalog, DBCatalog, string, int>> functionsDict = null, DBCatalog daughterCatalog = null)
        {
            foreach (string tableName in ArchiveTables.OrderedCompositeTables)
            {
                int checkEmpty;
                if (daughterCatalog != null)
                {
                    checkEmpty = daughterCatalog.SelectCountRowsTable(tableName);
                }
                else
                {
                    checkEmpty = mainCatalog.SelectCountRowsTable(tableName);
                }

                worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, $"Обработка {tableName}:");

                if (checkEmpty == 0)
                {
                    worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + "Пустая таблица.");
                }
                else if (functionsDict.ContainsKey(tableName))
                {
                    worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + $"Импортировано значений {functionsDict[tableName](mainCatalog, daughterCatalog, tableName)}");
                }
                else
                {
                    worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + "Обработчик отсутствует.");
                }
            }
        }

        // --- Master merge process table ---
        // Для обработки дефолтных таблиц
        static int ProcessDefaultTable(BackgroundWorker worker, DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName, string uniqueValueColumnName, string idLikeColumnName, string highLevelColumnName, List<string> excludeColumns, bool allowsNull)
        {
            // -------- Общий блок инициализации вне зависимости от переданных парпаметров--------
            int countOfImports = 0;

            long lastId = 0;
            if (idLikeColumnName != null)
            {
                string lastIDFromDB = string.Join("", mainCatalog.SelectLastRecord(idLikeColumnName, tableName, idLikeColumnName)).Replace("\'", "");
                lastId = (lastIDFromDB != "") ? Convert.ToInt64(lastIDFromDB) : 0;
            }

            // 1. Берем все записи двух каталогов в виде словарей
            List<Dictionary<string, string>> allFromMainData = mainCatalog.SelectAllFrom(tableName, mainCatalog.SelectColumnsNames(tableName, excludeColumns), allowsNull);
            List<Dictionary<string, string>> allFromDaughterData = daughterCatalog.SelectAllFrom(tableName, daughterCatalog.SelectColumnsNames(tableName, excludeColumns), allowsNull);
            List<Dictionary<string, string>> reservedRows = new List<Dictionary<string, string>>();

            // 2. Берем список значений по уникальному полю из главного каталога
            List<string> mainCatalogValues = ValuesManager.SelectDataFromColumn(allFromMainData, uniqueValueColumnName);

            // 3. Берем таблицы в дальнейшем используемые
            Dictionary<string, string> foreigns = mainCatalog.SelectTablesAndForeignKeyUsage(tableName);

            // 4. Проверяем foreigns на наличие использования. Если True, то добавляем все в RealoadDict
            if (foreigns.Count > 0)
            {
                ValuesManager.AddTablesToRewriteDict(foreigns);
            }
            // ----------------

            Consts.MergeProgress.ClearTasksBlock();
            Consts.MergeProgress.AddTaskInBlock(allFromDaughterData.Count);

            if (highLevelColumnName != null)
            {
                // рекурсия до 50. Если записи так и остались, то меняем на null и закидываем так
                //List<Dictionary<string, string>> copyOfAll = new List<Dictionary<string, string>>(allFromDaughterData);
                int countOfTry = 50;
                //ValuesManager.RecursHighValues(countOfTry, uniqueValueColumnName, idLikeColumnName, highLevelColumnName, new List<Dictionary<string, string>>(allFromDaughterData), new List<string>(mainCatalogValues));

                


                /*while (copyOfAll.Count != 0)
                {
                    foreach (Dictionary<string, string> row in allFromDaughterData)
                    {
                        // Если значение присутствует в главной таблице
                        if (mainCatalogValues.Contains(row[uniqueValueColumnName]))
                        {
                            ValuesManager.AddPairKeysToRewriteDict(foreigns, idLikeColumnName, new Tuple<string, string>(row[idLikeColumnName], ValuesManager.ReturnValue(allFromMainData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName)));
                        }

                        else if (!mainCatalogValues.Contains(row[uniqueValueColumnName]))
                        {
                            // ------- new_block ----
                            // Первым делом взять high и проверить его значение в дочерней с главной
                            //string s = ValuesManager.ReturnValue(allFromDaughterData, highLevelColumnName, row[highLevelColumnName], uniqueValueColumnName);
                            if (mainCatalogValues.Contains(ValuesManager.ReturnValue(allFromDaughterData, highLevelColumnName, row[highLevelColumnName], uniqueValueColumnName)))
                            {
                                string oldKey = row[idLikeColumnName];
                                row[highLevelColumnName] = (ValuesManager.ReturnValue(allFromMainData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName) != "") ? ValuesManager.ReturnValue(allFromMainData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName) : "'null'";
                                row[idLikeColumnName] = $"'{lastId + countOfImports + 1}'";
                                ValuesManager.AddPairKeysToRewriteDict(foreigns, idLikeColumnName, new Tuple<string, string>(oldKey, row[idLikeColumnName]));
                                mainCatalog.InsertValue(tableName, ValuesManager.RemoveUnnecessary(row, excludeColumns));
                                mainCatalogValues.Add(row[uniqueValueColumnName]);
                                countOfImports++;
                            }// Если значения по ключу High нет в записях главного каталога
                            else if (!mainCatalogValues.Contains(ValuesManager.ReturnValue(allFromDaughterData, highLevelColumnName, row[highLevelColumnName], uniqueValueColumnName)))
                            {
                                Consts.MergeProgress.AddTaskInBlock();
                            }
                        }
                        worker.ReportProgress(Consts.MergeProgress.UpdateBlockBar(), WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
                    }
                }*/


                // Если есть highLevelColumnName значит есть и idLikeColumnName
                foreach (Dictionary<string, string> row in allFromDaughterData)
                {                    
                    // Если значение присутствует в главной таблице
                    if (mainCatalogValues.Contains(row[uniqueValueColumnName]))
                    {
                        ValuesManager.AddPairKeysToRewriteDict(foreigns, idLikeColumnName, new Tuple<string, string>(row[idLikeColumnName], ValuesManager.ReturnValue(allFromMainData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName)));
                    }

                    else if (!mainCatalogValues.Contains(row[uniqueValueColumnName]))
                    {
                        // ------- new_block ----
                        // Первым делом взять high и проверить его значение в дочерней с главной
                        //string s = ValuesManager.ReturnValue(allFromDaughterData, highLevelColumnName, row[highLevelColumnName], uniqueValueColumnName);
                        string valueOnHighID = ValuesManager.ReturnValue(allFromDaughterData, highLevelColumnName, row[highLevelColumnName], uniqueValueColumnName);
                        if (mainCatalogValues.Contains(valueOnHighID))
                        {
                            string oldKey = row[idLikeColumnName];
                            //row[highLevelColumnName] = (ValuesManager.ReturnValue(allFromMainData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName) != "") ? ValuesManager.ReturnValue(allFromMainData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName) : "'null'";
                            row[highLevelColumnName] = ValuesManager.ReturnValue(allFromMainData, uniqueValueColumnName, valueOnHighID, idLikeColumnName);                         
                            row[idLikeColumnName] = $"'{lastId + countOfImports + 1}'";
                            ValuesManager.AddPairKeysToRewriteDict(foreigns, idLikeColumnName, new Tuple<string, string>(oldKey, row[idLikeColumnName]));
                            mainCatalog.InsertValue(tableName, ValuesManager.RemoveUnnecessary(row, excludeColumns));
                            allFromMainData.Add(row);
                            mainCatalogValues.Add(row[uniqueValueColumnName]);
                            countOfImports++;
                        }// Если значения по ключу High нет в записях главного каталога
                        else if (!mainCatalogValues.Contains(ValuesManager.ReturnValue(allFromDaughterData, highLevelColumnName, row[highLevelColumnName], uniqueValueColumnName)))
                        {
                            MessageBox.Show(row[uniqueValueColumnName]);
                            reservedRows.Add(row);
                            Consts.MergeProgress.AddTaskInBlock();
                        }
                        // ------- new_block ----
                    }



                    // ------- old_block ----
                    /* // Если значения в главной нет и idLikeColumnName БОЛЬШЕ чем highLevelColumnName 
                     else if (!mainCatalogValues.Contains(row[uniqueValueColumnName]) && Convert.ToInt64(row[idLikeColumnName].Replace("\'", "")) > Convert.ToInt64(row[highLevelColumnName].Replace("\'", "")))
                     {
                         // Первым делом взять high и проверить его значение в дочерней с главной
                         //string s = ValuesManager.ReturnValue(allFromDaughterData, highLevelColumnName, row[highLevelColumnName], uniqueValueColumnName);
                         if (mainCatalogValues.Contains(ValuesManager.ReturnValue(allFromDaughterData, highLevelColumnName, row[highLevelColumnName], uniqueValueColumnName)))
                         {
                             string oldKey = row[idLikeColumnName];
                             row[highLevelColumnName] = (ValuesManager.ReturnValue(allFromMainData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName) != "") ? ValuesManager.ReturnValue(allFromMainData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName) : "'null'";
                             row[idLikeColumnName] = $"'{lastId + countOfImports + 1}'";
                             ValuesManager.AddPairKeysToRewriteDict(foreigns, idLikeColumnName, new Tuple<string, string>(oldKey, row[idLikeColumnName]));
                             mainCatalog.InsertValue(tableName, ValuesManager.RemoveUnnecessary(row, excludeColumns));
                             mainCatalogValues.Add(row[uniqueValueColumnName]);
                             countOfImports++;
                         }// Если значения по ключу High нет в записях главного каталога
                         else if (!mainCatalogValues.Contains(ValuesManager.ReturnValue(allFromDaughterData, highLevelColumnName, row[highLevelColumnName], uniqueValueColumnName)))
                         {
                             reservedRows.Add(row);
                             Consts.AddTaskInBlock();
                         }
                     } // Если значения в главной нет и idLikeColumnName МЕНЬШЕ чем highLevelColumnName 
                     else if (!mainCatalogValues.Contains(row[uniqueValueColumnName]) && Convert.ToInt64(row[idLikeColumnName].Replace("\'", "")) < Convert.ToInt64(row[highLevelColumnName].Replace("\'", "")))
                     {
                         if (mainCatalogValues.Contains(ValuesManager.ReturnValue(allFromDaughterData, highLevelColumnName, row[highLevelColumnName], uniqueValueColumnName)))
                         {
                             string oldKey = row[idLikeColumnName];
                             row[highLevelColumnName] = (ValuesManager.ReturnValue(allFromMainData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName) != "") ? ValuesManager.ReturnValue(allFromMainData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName) : "'null'";
                             row[idLikeColumnName] = $"'{lastId + countOfImports + 1}'";
                             ValuesManager.AddPairKeysToRewriteDict(foreigns, idLikeColumnName, new Tuple<string, string>(oldKey, row[idLikeColumnName]));
                             mainCatalog.InsertValue(tableName, ValuesManager.RemoveUnnecessary(row, excludeColumns));
                             mainCatalogValues.Add(row[uniqueValueColumnName]);
                             countOfImports++;
                         }// Если значения по ключу High нет в записях главного каталога
                         else if (!mainCatalogValues.Contains(ValuesManager.ReturnValue(allFromDaughterData, highLevelColumnName, row[highLevelColumnName], uniqueValueColumnName)))
                         {
                             reservedRows.Add(row);
                             Consts.AddTaskInBlock();
                         }
                     }*/
                    // ------- old_block ----


                    worker.ReportProgress(Consts.MergeProgress.UpdateBlockBar(), Consts.WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
                }
            }
            else // Готовый блок
            {
                // N. Создаем цикл перебора значений дочернего каталога
                foreach (Dictionary<string, string> row in allFromDaughterData)
                {
                    // Если значения нет и не передано уникальное поле idLike
                    if (idLikeColumnName == null && !mainCatalogValues.Contains(row[uniqueValueColumnName]))
                    {
                        if (row.ContainsKey("ID"))
                        {
                            mainCatalog.InsertValue(tableName, ValuesManager.RemoveUnnecessary(row, excludeColumns));
                        }
                        else
                        {
                            mainCatalog.InsertValue(tableName, ValuesManager.RemoveUnnecessary(row, excludeColumns), withoutID: true);
                        }
                        mainCatalogValues.Add(row[uniqueValueColumnName]);
                        countOfImports++;
                    } // Если значение из дочерней БД уже есть в главной
                    else if (idLikeColumnName != null && mainCatalogValues.Contains(row[uniqueValueColumnName]) && foreigns.Count > 0)
                    {
                        ValuesManager.AddPairKeysToRewriteDict(foreigns, idLikeColumnName, new Tuple<string, string>(row[idLikeColumnName], ValuesManager.ReturnValue(allFromMainData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName)));
                    }// Если нашлось новое значения, которого нет в главной БД
                    else if (idLikeColumnName != null && !mainCatalogValues.Contains(row[uniqueValueColumnName]) && row[uniqueValueColumnName] != null)
                    {
                        string oldKey = row[idLikeColumnName];
                        //long lastId = Convert.ToInt64(string.Join("", mainCatalog.SelectLastRecord(idLikeColumnName, tableName, idLikeColumnName)).Replace("\'", ""));
                        row[idLikeColumnName] = $"'{countOfImports + lastId + 1}'";
                        //ValuesManager.AddPairKeysToReloadDict(foreigns, idLikeColumnName, new Tuple<string, string>(ValuesManager.ReturnValue(allFromMainData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName), row[idLikeColumnName]));
                        ValuesManager.AddPairKeysToRewriteDict(foreigns, idLikeColumnName, new Tuple<string, string>(oldKey, row[idLikeColumnName]));
                        mainCatalog.InsertValue(tableName, ValuesManager.RemoveUnnecessary(row, excludeColumns));
                        mainCatalogValues.Add(row[uniqueValueColumnName]);
                        countOfImports++;
                    }
                }
                worker.ReportProgress(Consts.MergeProgress.UpdateBlockBar(), Consts.WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
            }
            // Обработка отложенных записей
            if (reservedRows.Count > 0)
            {
                foreach (Dictionary<string, string> row in reservedRows)
                {
                    if (mainCatalogValues.Contains(ValuesManager.ReturnValue(allFromDaughterData, highLevelColumnName, row[highLevelColumnName], uniqueValueColumnName)))
                    {
                        string oldKey = row[idLikeColumnName];
                        row[highLevelColumnName] = (ValuesManager.ReturnValue(allFromMainData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName) != "") ? ValuesManager.ReturnValue(allFromMainData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName) : "'null'";
                        row[idLikeColumnName] = $"'{lastId + countOfImports + 1}'";
                        ValuesManager.AddPairKeysToRewriteDict(foreigns, idLikeColumnName, new Tuple<string, string>(oldKey, row[idLikeColumnName]));
                        mainCatalog.InsertValue(tableName, ValuesManager.RemoveUnnecessary(row, excludeColumns));
                        mainCatalogValues.Add(row[uniqueValueColumnName]);
                        countOfImports++;
                    }
                    else
                    {
                        string oldKey = row[idLikeColumnName];
                        row[highLevelColumnName] = "'null'";
                        row[idLikeColumnName] = $"'{lastId + countOfImports + 1}'";
                        ValuesManager.AddPairKeysToRewriteDict(foreigns, idLikeColumnName, new Tuple<string, string>(oldKey, row[idLikeColumnName]));
                        mainCatalog.InsertValue(tableName, ValuesManager.RemoveUnnecessary(row, excludeColumns));
                        mainCatalogValues.Add(row[uniqueValueColumnName]);
                        countOfImports++;
                    }
                }
                worker.ReportProgress(Consts.MergeProgress.UpdateBlockBar(), Consts.WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
            }
            return countOfImports;
        }

        // 1. string uniqueValueColumnName         2. string idLikeColumnName         3. List<string> excludeColumns    4. string highLevelColumnName     5. string parentIdColumn         6. string numerateColumn
        // Для обработки таблиц с внешними ключами mainCatalog, daughterCatalog, uniqueValueColumnName: paramsForProcessing.Item1, idLikeColumnName: paramsForProcessing.Item2, tableName: tableName, excludeColumns: paramsForProcessing.Item3, highLevelColumnName: paramsForProcessing.Item4, parentIdColumn: paramsForProcessing.Item5, numerateColumn: paramsForProcessing.Item6
        static int ProcessLinksTable(BackgroundWorker worker, DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName, string uniqueValueColumnName, string idLikeColumnName, string highLevelColumnName, string parentIdColumn, string numerateColumn, List<string> extraFilterColumns, List<string> excludeColumns, bool allowsNull)
        {
            // -------- Общий блок инициализации вне зависимости от переданных парпаметров--------
            int countOfImports = 0;
            int countOfRequests = 0;

            if (Consts.FAST_REQUEST_MOD)
            {
                ValuesManager.ClearRequestsToTable();
            }
            

            long lastId = 0;
            // Инициализиуем ID строки, если был передан как параметр
            if (idLikeColumnName != null)
            {
                string lastIDFromDB = string.Join("", mainCatalog.SelectLastRecord(idLikeColumnName, tableName, idLikeColumnName)).Replace("\'", "");
                lastId = (lastIDFromDB != "") ? Convert.ToInt64(lastIDFromDB) : 0;
            }


            int lastNumeric = 0;
            // Инициализиуем НУМЕРАЦИЮ строки, если был передан как параметр
            if (numerateColumn != null)
            {
                string lastNumricFromDB = string.Join("", mainCatalog.SelectLastRecord(numerateColumn, tableName, numerateColumn)).Replace("\'", "");
                lastNumeric = (lastNumricFromDB != "") ? Convert.ToInt32(lastNumricFromDB) : 0;
            }
/*            
            if (tableName == "tblFUND")
            {
                MessageBox.Show(lastNumeric.ToString());
            }*/

            // 3. Берем таблицы в дальнейшем используемые
            Dictionary<string, string> foreigns = mainCatalog.SelectTablesAndForeignKeyUsage(tableName);

            // 4. Проверяем foreigns на наличие использования. Если True, то добавляем все в ReservDict
            if (foreigns.Count > 0)
            {
                ValuesManager.AddNewTableToReserve(foreigns);
            }
            // ----------------

            Consts.MergeProgress.ClearTasksBlock();

            // MessageBox.Show(tableName + "\n" + string.Join("  ", foreigns.Keys));


            /*if (uniqueValueColumnName == null && highLevelColumnName == null && parentIdColumn == null)
            {
                List<Dictionary<string, string>> allFromMainData = mainCatalog.SelectAllFrom(tableName);
                List<Dictionary<string, string>> allFromDaughterData = daughterCatalog.SelectAllFrom(tableName);

                if (foreigns.Count != 0)
                { 
                    ValuesManager.AddTablesToRewriteDict(foreigns);
                    ValuesManager.AddPairKeysToRewriteDict(foreigns, idLikeColumnName, new Tuple<string, string>(allFromDaughterData[0][idLikeColumnName], allFromMainData[0][idLikeColumnName]));
                }
            }*/

            List<Dictionary<string, string>> allFromMainCatalog = mainCatalog.SelectAllFrom(tableName, mainCatalog.SelectColumnsNames(tableName, excludeColumns), allowsNull);
            List<Dictionary<string, string>> allFromDaughterCatalog = daughterCatalog.SelectAllFrom(tableName, daughterCatalog.SelectColumnsNames(tableName, excludeColumns), allowsNull);



            if (tableName == "tblDOCUMENT_STATS")
            {
                //List<Dictionary<string, string>> allFromMainCatalog = mainCatalog.SelectAllFrom(tableName, mainCatalog.SelectColumnsNames(tableName, excludeColumns), allowsNull);
                //List<Dictionary<string, string>> allFromDaughterCatalog = daughterCatalog.SelectAllFrom(tableName, daughterCatalog.SelectColumnsNames(tableName, excludeColumns), allowsNull);
                Dictionary<string, List<Tuple<string, string>>> tableReservedValues = ValuesManager.ReturnTableValuesReserveDict(tableName);

                string secondParent = "ISN_INVENTORY";
                


                List<Tuple<string, string>> secondParentTuplesKeys = ValuesManager.ReturnTuplesValuesSpecialDict(tableName, secondParent);

                List<Tuple<string, string>> oldTwoParents = ValuesManager.ReturnFilteredTuples(allFromDaughterCatalog, parentIdColumn, secondParent);






                Consts.MergeProgress.AddTaskInBlock(allFromDaughterCatalog.Count);


                // parentIdTuple = ISN_FUND
                foreach (Tuple<string, string> parentIdTuple in tableReservedValues[parentIdColumn])
                {
                    List<Dictionary<string, string>> allFromMainDataWhere = new List<Dictionary<string, string>>();

                    // Получаем значения по родителю.
                    allFromMainDataWhere = ValuesManager.FilterRecordsFrom(allFromMainCatalog, parentIdColumn, parentIdTuple.Item2);
                    if (allFromMainDataWhere.Count > 0)
                    {
                        // Возникает, когда добавились только описи.
                        // доп проверку на вхождения всех записей. Что-то типа чекнуть по второму родителю. Если таких записей нет, то тот же цикл когда нету, но с доп поиском.

                        //allFromMainDataWhere = ValuesManager.FilterRecordsFrom(allFromMainDataWhere, secondParent, parentIdTuple.Item2);

                        continue;
                    }
                    else
                    {
                        string docID = null;
                        // Если есть новые фонды или фонды и описи в них
                        foreach (Dictionary<string, string> row in allFromDaughterCatalog)
                        {
                            ValuesManager.UpdateCheck(worker);

                            if (row[parentIdColumn] == parentIdTuple.Item1)
                            {
                                row[parentIdColumn] = parentIdTuple.Item2;
                                row[idLikeColumnName] = $"'{lastId + countOfImports + 1}'";
                                //row["DocID"] = mainCatalog.SelectIDFrom("tblFUND", parentIdColumn, parentIdTuple.Item2);
                                
                                if (row[secondParent] == "'null'")
                                {
                                    //"tblFUND"
                                    row["DocID"] = mainCatalog.SelectIDFrom(mainCatalog.SelectReferenceTableName(tableName, parentIdColumn), parentIdColumn, parentIdTuple.Item2);
                                    //MessageBox.Show("FUND   " + row["DocID"]);

                                }
                                else
                                {
                                    foreach (Tuple<string, string> twoParents in oldTwoParents)
                                    {
                                        if (parentIdTuple.Item1 == twoParents.Item1)
                                        {
                                            foreach (Tuple<string, string> secondPair in secondParentTuplesKeys)
                                            {
                                                if (twoParents.Item1 == secondPair.Item1)
                                                {
                                                    row[secondParent] = secondPair.Item2;

                                                    row["DocID"] = mainCatalog.SelectIDFrom(mainCatalog.SelectReferenceTableName(tableName, secondParent), secondParent, parentIdTuple.Item2);

                                                    if (row["DocID"] == null)
                                                        MessageBox.Show(row["DocID"].ToString());
                                                    //row["DocID"] = mainCatalog.SelectIDFrom("tblINVENTORY", "ISN_INVENTORY", secondPair.Item2);
                                                    //MessageBox.Show("INV   " + row["DocID"]);
                                                }
                                            }
                                        }
                                    }
                                }







                                /*if (Consts.FAST_REQUEST_MOD)
                                {
                                    countOfRequests++;
                                    ValuesManager.AddToRequest(mainCatalog.ReturnValues(ValuesManager.MakeEditsInRow(row, tableName)));
                                }
                                else
                                {
                                    mainCatalog.InsertValue(tableName, ValuesManager.MakeEditsInRow(row, tableName));
                                }*/



                                ValuesManager.SelectImportMethod(mainCatalog, row, tableName, worker);

                                //mainCatalog.InsertValue(tableName, ValuesManager.MakeEditsInRow(row, tableName));

                                countOfImports++;
                            }
                            
                        }
                    }


                }






                // Старый не рабочий

                /*Consts.AddTaskInBlock(allFromDaughterCatalog.Count);


                // parentIdTuple = ISN_FUND
                foreach (Tuple<string, string> parentIdTuple in tableReservedValues[parentIdColumn])
                {
                    List<Dictionary<string, string>> allFromMainDataWhere = new List<Dictionary<string, string>>();

                    foreach (Tuple<string, string> pairParants in oldTwoParents)
                    {
                        if (parentIdTuple.Item1 == pairParants.Item1)
                        {
                            if (pairParants.Item2 != "'null'")
                            {
                                foreach (Tuple<string, string> secondParentKeys in secondParentTuplesKeys)
                                {
                                    if (pairParants.Item2 == secondParentKeys.Item1)
                                    {
                                        allFromMainDataWhere = ValuesManager.FilterRecordsFrom(allFromMainCatalog, parentIdColumn, parentIdTuple.Item2, secondParent, secondParentKeys.Item2);
                                        if (allFromMainDataWhere.Count > 0)
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            foreach (Dictionary<string, string> row in allFromDaughterCatalog)
                                            {
                                                if (row[parentIdColumn] == parentIdTuple.Item1)
                                                {
                                                    row[parentIdColumn] = parentIdTuple.Item2;
                                                    row[secondParent] = secondParentKeys.Item2;
                                                    row[idLikeColumnName] = $"'{lastId + countOfImports + 1}'";

                                                    mainCatalog.InsertValue(tableName, ValuesManager.MakeEditsInRow(row, tableName));

                                                    countOfImports++;
                                                }
                                            }
                                        }
                                        goto exitloop;
                                    }
                                }
                            }
                            allFromMainDataWhere = ValuesManager.FilterRecordsFrom(allFromMainCatalog, parentIdColumn, parentIdTuple.Item2);
                            if (allFromMainDataWhere.Count > 0)
                            {
                                continue;
                            }
                            else
                            {
                                foreach (Dictionary<string, string> row in allFromDaughterCatalog)
                                {
                                    if (row[parentIdColumn] == parentIdTuple.Item1)
                                    {
                                        row[parentIdColumn] = parentIdTuple.Item2;
                                        row[idLikeColumnName] = $"'{lastId + countOfImports + 1}'";

                                        mainCatalog.InsertValue(tableName, ValuesManager.MakeEditsInRow(row, tableName));

                                        countOfImports++;
                                    }
                                }
                            }
                            goto exitloop;
                        }
                    }

                //List<Dictionary<string, string>> allFromMainDataWhere = ValuesManager.FilterRecordsFrom(allFromMainCatalog, parentIdColumn, tuple.Item2);
                exitloop:
                    continue;

                    //worker.ReportProgress(Consts.UpdateBlockBar(), WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
                }*/
            }
            else if (tableName == "tblARCHIVE")
            {
                //List<Dictionary<string, string>> allFromMainCatalog = mainCatalog.SelectAllFrom(tableName, mainCatalog.SelectColumnsNames(tableName, excludeColumns), allowsNull);
                //List<Dictionary<string, string>> allFromDaughterCatalog = daughterCatalog.SelectAllFrom(tableName, daughterCatalog.SelectColumnsNames(tableName, excludeColumns), allowsNull);

                ValuesManager.AddTablesToRewriteDict(foreigns);
                ValuesManager.AddPairKeysToRewriteDict(foreigns, idLikeColumnName, new Tuple<string, string>(allFromDaughterCatalog[0][idLikeColumnName], allFromMainCatalog[0][idLikeColumnName]));

            }
            // Если нет родителя к которому цепляются записи, то обработка по всем значениям таблицы
            // Данный IF работает, все значения передаются в резерв.
            else if (uniqueValueColumnName != null && idLikeColumnName != null && highLevelColumnName == null && parentIdColumn == null)
            {
                // 1. Берем все записи двух каталогов в виде словарей
                //List<Dictionary<string, string>> allFromMainCatalog = mainCatalog.SelectAllFrom(tableName, mainCatalog.SelectColumnsNames(tableName, excludeColumns), allowsNull);
                //List<Dictionary<string, string>> allFromDaughterCatalog = daughterCatalog.SelectAllFrom(tableName, daughterCatalog.SelectColumnsNames(tableName, excludeColumns), allowsNull);
                List<Dictionary<string, string>> reservedRows = new List<Dictionary<string, string>>();



                // 2. Берем список значений по уникальному полю из главного каталога
                List<string> mainCatalogValues = ValuesManager.SelectDataFromColumn(allFromMainCatalog, uniqueValueColumnName);

                Consts.MergeProgress.AddTaskInBlock(allFromDaughterCatalog.Count);

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
                            //MessageBox.Show("old: " + oldKey + " new: "  + row[idLikeColumnName]);
                            ValuesManager.AddPairKeysToReserve(foreigns, idLikeColumnName, new Tuple<string, string>(oldKey, row[idLikeColumnName]));
                        }

                        

/*                        if (Consts.FAST_REQUEST_MOD)
                        {
                            countOfRequests++;
                            ValuesManager.AddToRequest(mainCatalog.ReturnValues(ValuesManager.MakeEditsInRow(row, tableName)));
                        }
                        else
                        {
                            mainCatalog.InsertValue(tableName, ValuesManager.MakeEditsInRow(row, tableName));
                        }*/


                        ValuesManager.SelectImportMethod(mainCatalog, row, tableName, worker);

                        countOfImports++;
                    }
                    worker.ReportProgress(Consts.MergeProgress.UpdateBlockBar(), Consts.WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
                }
            } // Если есть родителей, то отбор данных совершенно иной
            else if (uniqueValueColumnName != null && idLikeColumnName != null && highLevelColumnName == null && parentIdColumn != null)
            {
                if (tableName == SpecialTablesValues.SpecialTablePair.Item1)
                    ValuesManager.AddNewTableToSpecialRewrite(foreigns);

                //List<Dictionary<string, string>> allFromMainCatalog = mainCatalog.SelectAllFrom(tableName, mainCatalog.SelectColumnsNames(tableName, excludeColumns), allowsNull);
                //List<Dictionary<string, string>> allFromDaughterCatalog = daughterCatalog.SelectAllFrom(tableName, daughterCatalog.SelectColumnsNames(tableName, excludeColumns), allowsNull);

                Dictionary<string, List<Tuple<string, string>>> tableReservedValues = ValuesManager.ReturnTableValuesReserveDict(tableName);
                // List<Dictionary<string, string>> reservedRows = new List<Dictionary<string, string>>();

                Consts.MergeProgress.AddTaskInBlock(tableReservedValues[parentIdColumn].Count);

                foreach (Tuple<string, string> pairKeys in tableReservedValues[parentIdColumn])
                {
                    /*if (tableName == "tblACT")
                    {
                        MessageBox.Show((pairKeys.Item1 == pairKeys.Item2).ToString() + pairKeys.Item1 + "    " + pairKeys.Item2);
                    }*/
                   
                    //pairKeys.Item1; // значения ключей, которые были в дочерней
                    //pairKeys.Item2; // значения ключей, которые теперь в главной

                    // List<Dictionary<string, string>> filterFromMainData = mainCatalog.SelectRecordsWhere(new List<string>(), tableName, parentIdColumn, pairKeys.Item2);
                    // List<Dictionary<string, string>> filterFromDaughterData = daughterCatalog.SelectRecordsWhere(new List<string>(), tableName, parentIdColumn, pairKeys.Item1);

                    List<Dictionary<string, string>> filterFromMainData = ValuesManager.FilterRecordsFrom(allFromMainCatalog, parentIdColumn, pairKeys.Item2);
                    List<Dictionary<string, string>> filterFromDaughterData = ValuesManager.FilterRecordsFrom(allFromDaughterCatalog, parentIdColumn, pairKeys.Item1);

                    List<string> mainCatalogValues = ValuesManager.SelectDataFromColumn(filterFromMainData, uniqueValueColumnName);

                    

                    foreach (Dictionary<string, string> row in filterFromDaughterData)
                    {
                        ValuesManager.UpdateCheck(worker);

                        //MessageBox.Show(mainCatalogValues.Contains(row[uniqueValueColumnName]).ToString() + "   % " + row[uniqueValueColumnName] + " %" + string.Join(" ", mainCatalogValues));
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

                                

/*                                if (tableName == "tblACT")
                                {
                                    MessageBox.Show((row[idLikeColumnName] == ValuesManager.ReturnValue(filterFromMainData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName)).ToString() + " " + row[idLikeColumnName] + "    " + ValuesManager.ReturnValue(filterFromMainData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName));
                                }*/

                                if (tableName == SpecialTablesValues.SpecialTablePair.Item1)
                                    ValuesManager.AddPairKeysToSpecialRewrite(foreigns, idLikeColumnName, new Tuple<string, string>(row[idLikeColumnName], ValuesManager.ReturnValue(filterFromMainData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName)));
                            }
                        }
                        else if (!mainCatalogValues.Contains(row[uniqueValueColumnName]))
                        {
                            string oldKey = row[idLikeColumnName];
                            row[idLikeColumnName] = $"'{lastId + countOfImports + 1}'";
                            row[parentIdColumn] = pairKeys.Item2;

                            if (numerateColumn != null)
                            {
                                row[numerateColumn] = $"'{lastNumeric + countOfImports + 1}'";
                            }

                            if (foreigns.Count > 0)
                            {


                                ValuesManager.AddPairKeysToReserve(foreigns, idLikeColumnName, new Tuple<string, string>(oldKey, row[idLikeColumnName]));




                                if (tableName == SpecialTablesValues.SpecialTablePair.Item1)
                                    ValuesManager.AddPairKeysToSpecialRewrite(foreigns, idLikeColumnName, new Tuple<string, string>(oldKey, row[idLikeColumnName]));
                            }

                            /*if (Consts.FAST_REQUEST_MOD)
                            {
                                countOfRequests++;
                                ValuesManager.AddToRequest(mainCatalog.ReturnValues(ValuesManager.MakeEditsInRow(row, tableName)));
                            }
                            else
                            {
                                mainCatalog.InsertValue(tableName, ValuesManager.MakeEditsInRow(row, tableName));
                            }*/

                            ValuesManager.SelectImportMethod(mainCatalog, row, tableName, worker);

                            countOfImports++;
                        }
                    }
                    
                    /*if (Consts.FAST_REQUEST_MOD && ValuesManager.ReturnRequestsToTable() != "")
                    {
                        mainCatalog.InsertListOfValues(ValuesManager.ReturnRequestsToTable());
                        ValuesManager.ClearRequestsToTable();
                    }*/
                    worker.ReportProgress(Consts.MergeProgress.UpdateBlockBar(), Consts.WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
                }
                // ValuesManager.DeleteTableFromReserve(tableName);
            } // Если из параметров только родитель
            else if (uniqueValueColumnName == null && idLikeColumnName == null && highLevelColumnName == null && parentIdColumn != null)
            {
                //List<Dictionary<string, string>> allFromMainCatalog = mainCatalog.SelectAllFrom(tableName, mainCatalog.SelectColumnsNames(tableName, excludeColumns), allowsNull);
                //List<Dictionary<string, string>> allFromDaughterCatalog = daughterCatalog.SelectAllFrom(tableName, daughterCatalog.SelectColumnsNames(tableName, excludeColumns), allowsNull);
                Dictionary<string, List<Tuple<string, string>>> tableReservedValues = ValuesManager.ReturnTableValuesReserveDict(tableName);

                Consts.MergeProgress.AddTaskInBlock(allFromDaughterCatalog.Count);

/*                if (tableName == "tblFUND_CHECK")
                {
                    MessageBox.Show(tableReservedValues[parentIdColumn].Count.ToString());
                }*/

                foreach (Tuple<string, string> tuple in tableReservedValues[parentIdColumn])
                {
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
                                row["DocID"] = mainCatalog.SelectIDFrom(mainCatalog.SelectReferenceTableName(tableName, parentIdColumn), parentIdColumn, tuple.Item2);

                                /*if (Consts.FAST_REQUEST_MOD)
                                {
                                    countOfRequests++;
                                    ValuesManager.AddToRequest(mainCatalog.ReturnValues(ValuesManager.MakeEditsInRow(row, tableName)));
                                }
                                else
                                {
                                    mainCatalog.InsertValue(tableName, ValuesManager.MakeEditsInRow(row, tableName));
                                }*/

                                ValuesManager.SelectImportMethod(mainCatalog, row, tableName, worker);

                                countOfImports++;
                            }
                        }
                    }
                    //worker.ReportProgress(Consts.UpdateBlockBar(), WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
                }
            }
            else if (uniqueValueColumnName != null && idLikeColumnName != null && highLevelColumnName != null && parentIdColumn == null)
            {
                //List<Dictionary<string, string>> allFromMainCatalog = mainCatalog.SelectAllFrom(tableName, mainCatalog.SelectColumnsNames(tableName, excludeColumns), allowsNull);
                //List<Dictionary<string, string>> allFromDaughterCatalog = daughterCatalog.SelectAllFrom(tableName, daughterCatalog.SelectColumnsNames(tableName, excludeColumns), allowsNull);
                List<string> mainCatalogValues = ValuesManager.SelectDataFromColumn(allFromMainCatalog, uniqueValueColumnName);
                List<Dictionary<string, string>> reservedRows = new List<Dictionary<string, string>>();

                if (foreigns.Count > 0)
                {
                    ValuesManager.AddTablesToRewriteDict(foreigns);
                }

                foreach (Dictionary<string, string> row in allFromDaughterCatalog)
                {
                    ValuesManager.UpdateCheck(worker);

                    if (mainCatalogValues.Contains(row[uniqueValueColumnName]))
                    {
                        ValuesManager.AddPairKeysToRewriteDict(foreigns, idLikeColumnName, new Tuple<string, string>(row[idLikeColumnName], ValuesManager.ReturnValue(allFromMainCatalog, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName)));
                    }
                    // Если значения в главной нет и idLikeColumnName БОЛЬШЕ чем highLevelColumnName 
                    else if (!mainCatalogValues.Contains(row[uniqueValueColumnName]))
                    {
                        // Первым делом взять high и проверить его значение в дочерней с главной
                        //string s = ValuesManager.ReturnValue(allFromDaughterData, highLevelColumnName, row[highLevelColumnName], uniqueValueColumnName);
                        if (mainCatalogValues.Contains(ValuesManager.ReturnValue(allFromDaughterCatalog, highLevelColumnName, row[highLevelColumnName], uniqueValueColumnName)))
                        {
                            string oldKey = row[idLikeColumnName];
                            row[highLevelColumnName] = (ValuesManager.ReturnValue(allFromMainCatalog, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName) != "") ? ValuesManager.ReturnValue(allFromMainCatalog, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName) : "'null'";
                            row[idLikeColumnName] = $"'{lastId + countOfImports + 1}'";
                            ValuesManager.AddPairKeysToRewriteDict(foreigns, idLikeColumnName, new Tuple<string, string>(oldKey, row[idLikeColumnName]));

                            /*if (Consts.FAST_REQUEST_MOD)
                            {
                                countOfRequests++;
                                ValuesManager.AddToRequest(mainCatalog.ReturnValues(ValuesManager.MakeEditsInRow(row, tableName)));
                            }
                            else
                            {
                                mainCatalog.InsertValue(tableName, ValuesManager.MakeEditsInRow(row, tableName));
                            }*/

                            ValuesManager.SelectImportMethod(mainCatalog, row, tableName, worker);

                            mainCatalogValues.Add(row[uniqueValueColumnName]);
                            countOfImports++;
                        }// Если значения по ключу High нет в записях главного каталога
                        else if (!mainCatalogValues.Contains(ValuesManager.ReturnValue(allFromDaughterCatalog, highLevelColumnName, row[highLevelColumnName], uniqueValueColumnName)))
                        {
                            reservedRows.Add(row);
                            Consts.MergeProgress.AddTaskInBlock();
                        }
                    }
                }
                if (reservedRows.Count > 0)
                {
                    foreach (Dictionary<string, string> row in reservedRows)
                    {
                        ValuesManager.UpdateCheck(worker);

                        if (mainCatalogValues.Contains(ValuesManager.ReturnValue(allFromDaughterCatalog, highLevelColumnName, row[highLevelColumnName], uniqueValueColumnName)))
                        {
                            string oldKey = row[idLikeColumnName];
                            row[highLevelColumnName] = (ValuesManager.ReturnValue(allFromMainCatalog, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName) != "") ? ValuesManager.ReturnValue(allFromMainCatalog, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName) : "'null'";
                            MessageBox.Show(row[highLevelColumnName]);
                            row[idLikeColumnName] = $"'{lastId + countOfImports + 1}'";
                            ValuesManager.AddPairKeysToRewriteDict(foreigns, idLikeColumnName, new Tuple<string, string>(oldKey, row[idLikeColumnName]));



                            ValuesManager.SelectImportMethod(mainCatalog, row, tableName, worker);

                            /*if (Consts.FAST_REQUEST_MOD)
                            {
                                countOfRequests++;
                                ValuesManager.AddToRequest(mainCatalog.ReturnValues(ValuesManager.MakeEditsInRow(row, tableName)));
                            }
                            else
                            {
                                mainCatalog.InsertValue(tableName, ValuesManager.MakeEditsInRow(row, tableName));
                            }*/


                            mainCatalogValues.Add(row[uniqueValueColumnName]);
                            countOfImports++;
                        }
                        else
                        {
                            string oldKey = row[idLikeColumnName];
                            row[highLevelColumnName] = "'null'";
                            row[idLikeColumnName] = $"'{lastId + countOfImports + 1}'";
                            ValuesManager.AddPairKeysToRewriteDict(foreigns, idLikeColumnName, new Tuple<string, string>(oldKey, row[idLikeColumnName]));

/*                            if (Consts.FAST_REQUEST_MOD)
                            {
                                countOfRequests++;
                                ValuesManager.AddToRequest(mainCatalog.ReturnValues(ValuesManager.MakeEditsInRow(row, tableName)));
                            }
                            else
                            {
                                mainCatalog.InsertValue(tableName, ValuesManager.MakeEditsInRow(row, tableName));
                            }*/

                            ValuesManager.SelectImportMethod(mainCatalog, row, tableName, worker);

                            

                            mainCatalogValues.Add(row[uniqueValueColumnName]);
                            countOfImports++;
                        }
                    }
                    worker.ReportProgress(Consts.MergeProgress.UpdateBlockBar(), Consts.WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
                }
            }
            else if (uniqueValueColumnName == null && idLikeColumnName != null && highLevelColumnName == null && parentIdColumn != null)
            {
                //List<Dictionary<string, string>> allFromMainCatalog = mainCatalog.SelectAllFrom(tableName, mainCatalog.SelectColumnsNames(tableName, excludeColumns), allowsNull);
                //List<Dictionary<string, string>> allFromDaughterCatalog = daughterCatalog.SelectAllFrom(tableName, daughterCatalog.SelectColumnsNames(tableName, excludeColumns), allowsNull);
                Dictionary<string, List<Tuple<string, string>>> tableReservedValues = ValuesManager.ReturnTableValuesReserveDict(tableName);


                Consts.MergeProgress.AddTaskInBlock(allFromDaughterCatalog.Count);

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



                                /*if (Consts.FAST_REQUEST_MOD)
                                {
                                    countOfRequests++;
                                    ValuesManager.AddToRequest(mainCatalog.ReturnValues(ValuesManager.MakeEditsInRow(row, tableName)));
                                }
                                else
                                {
                                    mainCatalog.InsertValue(tableName, ValuesManager.MakeEditsInRow(row, tableName));
                                }*/

                                ValuesManager.SelectImportMethod(mainCatalog, row, tableName, worker);

                                countOfImports++;
                            }
                        }
                    }
                    //worker.ReportProgress(Consts.UpdateBlockBar(), WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
                }

            }
            else if (uniqueValueColumnName == null && idLikeColumnName != null && highLevelColumnName != null && parentIdColumn != null)
            {
                //List<Dictionary<string, string>> allFromMainCatalog = mainCatalog.SelectAllFrom(tableName, mainCatalog.SelectColumnsNames(tableName, excludeColumns), allowsNull);
                //List<Dictionary<string, string>> allFromDaughterCatalog = daughterCatalog.SelectAllFrom(tableName, daughterCatalog.SelectColumnsNames(tableName, excludeColumns), allowsNull);

                Dictionary<string, List<Tuple<string, string>>> tableReservedValues = ValuesManager.ReturnTableValuesReserveDict(tableName);
                List<string> mainCatalogValues = ValuesManager.SelectDataFromColumn(allFromMainCatalog, highLevelColumnName);

                // List<Dictionary<string, string>> allFromDaughterDataWhere = daughterCatalog.SelectRecordsWhere(new List<string>, tableName, parentIdColumn, );

                Consts.MergeProgress.AddTaskInBlock(allFromDaughterCatalog.Count);

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
/*
                                if (Consts.FAST_REQUEST_MOD)
                                {
                                    countOfRequests++;
                                    ValuesManager.AddToRequest(mainCatalog.ReturnValues(ValuesManager.MakeEditsInRow(row, tableName)));
                                }
                                else
                                {
                                    mainCatalog.InsertValue(tableName, ValuesManager.MakeEditsInRow(row, tableName));
                                }*/

                                ValuesManager.SelectImportMethod(mainCatalog, row, tableName, worker);


                                ValuesManager.AddPairKeysToRewriteDict(foreigns, idLikeColumnName, new Tuple<string, string>(oldKey, row[idLikeColumnName]));
                                countOfImports++;
                            }
                        }

                    }
                    //worker.ReportProgress(Consts.UpdateBlockBar(), WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
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
