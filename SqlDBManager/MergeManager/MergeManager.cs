using System.Collections.Generic;
using System.ComponentModel;
using NotesNamespace;
using System;
using System.IO;
using System.Text.Json;
using System.Linq;


namespace SqlDBManager
{
    public class MergeManager : MergeSettings
    {
        public static bool FastRequestMod;
        public string MergeRulesFilePath;

        public MergeManager(bool fastRequestModValue, string mergeRulesFilePathValue)
        {
            FastRequestMod = fastRequestModValue;
            MergeRulesFilePath = mergeRulesFilePathValue;
        }

        static MergeRulesDeserializer MergeRules;
        static long LastIdMainCatalog = 0;

        public void LoadMergeRules()
        {
            //AppDomain.CurrentDomain.BaseDirectory + "/MergeRules/" + "af_rules.json"
            using (StreamReader reader = new StreamReader(MergeRulesFilePath))
            {
                string jsonString = reader.ReadToEnd();
                MergeRulesDeserializer deserializedJson = JsonSerializer.Deserialize<MergeRulesDeserializer>(jsonString);
                MergeRules = deserializedJson;
            }
        }

        public bool RepairDBKeys(DBCatalog mainCatalog, BackgroundWorker worker)
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
                catch (MergerExceptions.StopMergeException error)
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

        public bool ProcessOnlyIdsTables(DBCatalog mainCatalog, DBCatalog daughterCatalog, BackgroundWorker worker)
        {
            if (MergeRules.onlyIdsTables == null)
            {
                worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + "Таблиц на получение ID не найдено.");
                return true;
            }

            foreach (string tableName in MergeRules.onlyIdsTables.Keys)
            {
                worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, $"Обработка {tableName}:");

                if (mainCatalog.SelectCountRowsTable(tableName) == 0)
                {
                    worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + "Пустая таблица.");
                    worker.ReportProgress(100, Consts.WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
                }
                else
                {
                    if (Consts.DEBUG_MOD)
                    {
                        //MergeManager.ProcessGetPairIds(tableName, MergeRules.onlyIdsTables[tableName], mainCatalog, daughterCatalog, worker);
                        //worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + $"{0}");
                    }
                    else
                    {
                        try
                        {
                            MergeManager.ProcessGetPairIds(tableName, MergeRules.onlyIdsTables[tableName], mainCatalog, daughterCatalog, worker);
                            //worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + $"{0}");
                        }
                        catch (MergerExceptions.StopMergeException error)
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
            };
            return true;
        }

        /// <summary>
        /// Очищает выделенные под очистку таблицы
        /// </summary>
        /// <returns>Успешность выполнения транзакций</returns>
        public bool ProcessClearingTables(DBCatalog mainCatalog, BackgroundWorker worker)
        {
            if (MergeRules.tablesForCleaning == null)
            {
                worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + "Таблиц на очистку не найдено.");
                return true;
            }

            foreach (string tableName in MergeRules.tablesForCleaning)
            {
                worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, $"Очистка {tableName}:");

                if (mainCatalog.SelectCountRowsTable(tableName) == 0)
                {
                    worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + "Пустая таблица.");
                    worker.ReportProgress(100, Consts.WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
                }
                else
                {
                    if (Consts.DEBUG_MOD)
                    {
                        worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + $"Записей удалено {mainCatalog.ClearTable(tableName)}");
                    }
                    else
                    {
                        try
                        {
                            worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + $"Записей удалено {mainCatalog.ClearTable(tableName)}");
                        }
                        catch (MergerExceptions.StopMergeException error)
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
            };
            return true;
        }

        public bool ProcessSimpleTables(DBCatalog mainCatalog, DBCatalog daughterCatalog, BackgroundWorker worker)
        {
            foreach (string tableName in MergeRules.simpleTables.Keys)
            {
                worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, $"Обработка {tableName}:");

                if (daughterCatalog.SelectCountRowsTable(tableName) == 0)
                {
                    worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + "Пустая таблица.");
                    worker.ReportProgress(100, Consts.WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
                }
                else
                {
                    if (Consts.DEBUG_MOD)
                    {
                        //worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + $"Импортировано значений {ProcessSimpleTable(tableName, MergeRules.simpleTables[tableName], mainCatalog, daughterCatalog, worker)}");
                    }
                    else
                    {
                        try
                        {
                            worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + $"Импортировано значений {ProcessSimpleTable(tableName, MergeRules.simpleTables[tableName], mainCatalog, daughterCatalog, worker)}");
                        }
                        catch (MergerExceptions.StopMergeException error)
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

                worker.ReportProgress(Consts.MergeProgress.UpdateMainBar(), Consts.WorkerConsts.ITS_MAIN_PROGRESS_BAR);
            }
            return true;
        }

        public bool ProcessLinksTables_NEW(DBCatalog mainCatalog, DBCatalog daughterCatalog, BackgroundWorker worker)
        {
            //ReturnProcessStatus(worker, mainCatalog, linksTablesFunctions, daughterCatalog);


            foreach (string tableName in MergeRules.linksTables.Keys)
            {
                string processedTableName = tableName;

                if (processedTableName.Contains(Consts.MergeManager.REPEAT_TABLE_DELIMITER))
                {
                    processedTableName = processedTableName.Substring(0, processedTableName.IndexOf(Consts.MergeManager.REPEAT_TABLE_DELIMITER));
                }

                worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, $"Обработка {processedTableName}:");

                if (daughterCatalog.SelectCountRowsTable(processedTableName) == 0)
                {
                    worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + "Пустая таблица.");
                    worker.ReportProgress(100, Consts.WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
                }
                else
                {
                    if (Consts.DEBUG_MOD)
                    {
                        worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + $"Импортировано значений {MergeManager.ProcessLinkTable(processedTableName, MergeRules.linksTables[tableName], mainCatalog, daughterCatalog, worker)}");
                    }
                    else
                    {
                        try
                        {
                            worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + $"Импортировано значений {MergeManager.ProcessLinkTable(processedTableName, MergeRules.linksTables[tableName], mainCatalog, daughterCatalog, worker)}");
                        }
                        catch (MergerExceptions.StopMergeException error)
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
                
                worker.ReportProgress(Consts.MergeProgress.UpdateMainBar(), Consts.WorkerConsts.ITS_MAIN_PROGRESS_BAR);
            }
            return true;
        }
        
        public bool ProcessTables(string tablesStackName, DBCatalog mainCatalog, DBCatalog daughterCatalog, BackgroundWorker worker)
        {
            dynamic iterableData = null;
            DBCatalog checkedOnEmpty = null;

            if (tablesStackName == Consts.MergeManager.TableStackNames.REFERENCES_STACK)
            {
                iterableData = MergeRules.tablesReferences.Keys;
            }
            else if (tablesStackName == Consts.MergeManager.TableStackNames.CLEANING_TABLES_STACK)
            {
                iterableData = MergeRules.tablesForCleaning;
                checkedOnEmpty = mainCatalog;
            }
            else if (tablesStackName == Consts.MergeManager.TableStackNames.ONLY_IDS_STACK)
            {
                iterableData = MergeRules.onlyIdsTables.Keys;
                checkedOnEmpty = daughterCatalog;
            }
            else if (tablesStackName == Consts.MergeManager.TableStackNames.SIMPLE_TABLES_STACK)
            {
                iterableData = MergeRules.simpleTables.Keys;
                checkedOnEmpty = daughterCatalog;
            }
            else if (tablesStackName == Consts.MergeManager.TableStackNames.LINK_TABLES_STACK)
            {
                iterableData = MergeRules.linksTables.Keys;
                checkedOnEmpty = daughterCatalog;
            }

            if (iterableData == null)
            {
                worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + "Таблиц на обработку не обнаружено.");
                return true;
            }

            foreach (string rawTableNameFromRules in iterableData)
            {
                string processedTableName = rawTableNameFromRules;
                if (rawTableNameFromRules.Contains(Consts.MergeManager.REPEAT_TABLE_DELIMITER))
                {
                    processedTableName = rawTableNameFromRules.Substring(0, rawTableNameFromRules.IndexOf(Consts.MergeManager.REPEAT_TABLE_DELIMITER));
                }

                worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, $"Обработка {processedTableName}:");

                /*if (mainCatalog.)
                {
                    worker.ReportProgress(Consts.WorkerConsts.ERROR_STATUS_CODE, $"Несуществующая таблица: {processedTableName}");
                }*/


                if (mainCatalog.SelectCountRowsTable(processedTableName) == 0)
                {
                    worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + "Пустая таблица.");
                }
                else
                {
                    if (Consts.DEBUG_MOD)
                    {
                        //MergeManager.ProcessGetPairIds(tableName, MergeRules.onlyIdsTables[tableName], mainCatalog, daughterCatalog, worker);
                        //worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + $"{0}");
                    }
                    else
                    {
                        try
                        {
                            MergeManager.ProcessGetPairIds(processedTableName, MergeRules.onlyIdsTables[rawTableNameFromRules], mainCatalog, daughterCatalog, worker);
                            //worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + $"{0}");
                        }
                        catch (MergerExceptions.StopMergeException error)
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
            };
            return true;
        }

        // -----------------------------------------

        static void ProcessGetPairIds(string tableName, OnlyIdsTableRulesDeserializer mergeRules, DBCatalog mainCatalog, DBCatalog daughterCatalog, BackgroundWorker worker)
        {
            List<Dictionary<string, string>> mainCatalogTableData = mainCatalog.SelectAllFrom(tableName, mainCatalog.SelectColumnsNames(tableName, null), false);
            List<Dictionary<string, string>> daughterCatalogTableData = daughterCatalog.SelectAllFrom(tableName, daughterCatalog.SelectColumnsNames(tableName, null), false);

            Dictionary<string, string> foreignKeys = mainCatalog.SelectTablesAndForeignKeyUsage(tableName);
            if (foreignKeys.Count > 0)
            {
                ReservedValuesManager.AddForeignKeys(foreignKeys, tableName);
            }

            if (mainCatalogTableData.Count == 1 && daughterCatalogTableData.Count == 1)
            {
                ReservedValuesManager.AddPairToReservedPairs(tableName, mainCatalogTableData[0][mergeRules.idColumnName], daughterCatalogTableData[0][mergeRules.idColumnName], true);
            }
            else
            {
                foreach (var dRow in daughterCatalogTableData)
                {
                    foreach (var mRow in mainCatalogTableData)
                    {
                        if (dRow[mergeRules.idColumnName] == mRow[mergeRules.idColumnName])
                        {
                            ReservedValuesManager.AddPairToReservedPairs(tableName, mRow[mergeRules.idColumnName], dRow[mergeRules.idColumnName], true);
                            break;
                        }
                    }
                }
            }
        }

        static bool CheckRowColumnsValuesOnInsert(Dictionary<string, string> row, List<CheckColumnValueDeserializer> columnValuesRules)
        {
            bool passedState = true;

            if (columnValuesRules == null)
            {
                return passedState;
            }
            
            foreach (CheckColumnValueDeserializer rule in columnValuesRules)
            {
                if (rule.equalAnyTo != null && !rule.equalAnyTo.Contains(row[rule.columnName]))
                {
                    passedState = false;
                    break;
                }
                else if (rule.equalTo != null && row[rule.columnName] != rule.equalTo)
                {
                    passedState = false;
                    break;
                }
                else if (rule.notEqualTo != null && row[rule.columnName] == rule.notEqualTo)
                {
                    passedState = false;
                    break;
                }
            }
            return passedState;
        }

        static void RecurseLinkHighTable(string tableName, List<Dictionary<string, string>> iterData, List<Dictionary<string, string>> allDaughterData, List<Dictionary<string, string>> allMainData, LinkTableRulesDeserializer mergeRules, List<ReservedValuesManager.ForeignKey> tableReservedForeignKeys, DBCatalog catalog, BackgroundWorker worker)
        {
            foreach (var root in iterData)
            {
                // Получаем список дочерних элементов относительно рута для проверки уровня выше
                List<Dictionary<string, string>> rootChilds = new List<Dictionary<string, string>>();
                foreach (var el in allDaughterData)
                {
                    if (el[mergeRules.parentIdColumnName] == root[mergeRules.secondIdColumnName])
                    {
                        rootChilds.Add(new Dictionary<string, string>(el));
                    }
                }

                // Копируем нынешнего рута для дальнейшей работы с ним
                Dictionary<string, string> rootCopy = new Dictionary<string, string>(root);
                bool valueExisting = true;

                if (root[mergeRules.parentIdColumnName] == null || root[mergeRules.parentIdColumnName].ToLower() == "'null'" || root[mergeRules.parentIdColumnName].ToLower() == "null")
                {
                    // Отбираем все имена рутов с учетом, что могли появится новые
                    List<string> mainZeroLevelNames = new List<string>();
                    foreach (var row in allMainData)
                    {
                        if (row[mergeRules.parentIdColumnName] == null || row[mergeRules.parentIdColumnName].ToLower() == "'null'" || row[mergeRules.parentIdColumnName].ToLower() == "null")
                        {
                            mainZeroLevelNames.Add(row[mergeRules.uniqueValueColumnName]);
                        }
                    }

                    // Если значения нет, то добавляем значение в список главной таблицы с новым id и добавляем пару ключей стало-было
                    if (!mainZeroLevelNames.Contains(root[mergeRules.uniqueValueColumnName]))
                    {
                        rootCopy[mergeRules.secondIdColumnName] = $"{++LastIdMainCatalog}";
                        allMainData.Add(rootCopy);
                        valueExisting = false;

                        if (tableReservedForeignKeys.Count > 0)
                        {
                            rootCopy = MergeManager.UpdateRowForeignKeys(rootCopy, tableReservedForeignKeys);
                        }

                        ReservedValuesManager.AddValueToListOfInserts(rootCopy);
                    }
                }
                else
                { // Проверяем, что работаем не с рутом
                    foreach (var pair in ReservedValuesManager.GetReservedKeyPairs()[tableName])
                    {
                        // Ищем родителя нынешнего рута для сопоставления его с главной таблицей
                        if (pair.idDaughterCatalog == root[mergeRules.parentIdColumnName])
                        {
                            List<string> daughtersNames = new List<string>();

                            foreach (var el in allMainData)
                            {
                                if (el[mergeRules.parentIdColumnName] == pair.idMainCatalog)
                                {
                                    daughtersNames.Add(el[mergeRules.uniqueValueColumnName]);
                                }
                            }

                            // При новом значении обновляем у элемента ID и даем нового родителя, который будет в главной
                            if (!daughtersNames.Contains(root[mergeRules.uniqueValueColumnName]))
                            {
                                rootCopy[mergeRules.secondIdColumnName] = $"{++LastIdMainCatalog}";
                                rootCopy[mergeRules.parentIdColumnName] = pair.idMainCatalog;
                                allMainData.Add(rootCopy);
                                valueExisting = false;

                                if (tableReservedForeignKeys.Count > 0)
                                {
                                    rootCopy = MergeManager.UpdateRowForeignKeys(rootCopy, tableReservedForeignKeys);
                                }

                                ReservedValuesManager.AddValueToListOfInserts(rootCopy);
                            }
                            break;
                        }
                    }
                }

                ReservedValuesManager.AddPairToReservedPairs(tableName, rootCopy[mergeRules.secondIdColumnName], root[mergeRules.secondIdColumnName], valueExisting);

                if (rootChilds.Count > 0)
                {
                    RecurseLinkHighTable(tableName, rootChilds, allDaughterData, allMainData, mergeRules, tableReservedForeignKeys, catalog, worker);
                }
            }
        }

        static void RecurseSimpleHighTable(string tableName, List<Dictionary<string, string>> iterData, List<Dictionary<string, string>> allDaughterData, List<Dictionary<string, string>> allMainData, SimpleTableRulesDeserializer mergeRules, DBCatalog catalog, BackgroundWorker worker)
        {
            foreach (var root in iterData)
            {
                // Получаем список дочерних элементов относительно рута для проверки уровня выше
                List<Dictionary<string, string>> rootChilds = new List<Dictionary<string, string>>();
                foreach (var el in allDaughterData)
                {
                    if (el[mergeRules.parentIdColumnName] == root[mergeRules.secondIdColumnName])
                    {
                        rootChilds.Add(new Dictionary<string, string>(el));
                    }
                }

                // Копируем нынешнего рута для дальнейшей работы с ним
                Dictionary<string, string> rootCopy = new Dictionary<string, string>(root);
                bool valueExisting = true;

                if (root[mergeRules.parentIdColumnName] == null || root[mergeRules.parentIdColumnName].ToLower() == "'null'" || root[mergeRules.parentIdColumnName].ToLower() == "null")
                {
                    // Отбираем все имена рутов с учетом, что могли появится новые
                    List<string> mainZeroLevelNames = new List<string>();
                    HashSet<string> setValues = new HashSet<string>(mainZeroLevelNames);
                    if (mainZeroLevelNames.Count != setValues.Count)
                    {
                        throw new Exception($"{tableName} Root with ID: {root[mergeRules.parentIdColumnName]} Error: {mainZeroLevelNames.Count} != {setValues.Count}");
                    }

                    foreach (var row in allMainData)
                    {
                        if (row[mergeRules.parentIdColumnName] == null || row[mergeRules.parentIdColumnName].ToLower() == "'null'" || row[mergeRules.parentIdColumnName].ToLower() == "null")
                        {
                            mainZeroLevelNames.Add(row[mergeRules.uniqueValueColumnName]);
                        }
                    }

                    // Если значения нет, то добавляем значение в список главной таблицы с новым id и добавляем пару ключей стало-было
                    if (!mainZeroLevelNames.Contains(root[mergeRules.uniqueValueColumnName]))
                    {
                        rootCopy[mergeRules.secondIdColumnName] = $"{++LastIdMainCatalog}";
                        allMainData.Add(rootCopy);
                        valueExisting = false;

                        ReservedValuesManager.AddValueToListOfInserts(rootCopy);
                    }
                }
                else //else if (root[mergeRules.parentIdColumnName] != null)
                { // Проверяем, что работаем не с рутом
                    foreach (var pair in ReservedValuesManager.GetReservedKeyPairs()[tableName])
                    {
                        // Ищем родителя нынешнего рута для сопоставления его с главной таблицей
                        if (pair.idDaughterCatalog == root[mergeRules.parentIdColumnName]) // pair["idDaughterCatalog"]
                        {
                            List<string> daughtersNames = new List<string>();
                            HashSet<string> setValues = new HashSet<string>(daughtersNames);
                            if (daughtersNames.Count != setValues.Count)
                            {
                                throw new Exception($"{tableName} Root with ID: {root[mergeRules.parentIdColumnName]} Error: {daughtersNames.Count} != {setValues.Count}");
                            }

                            foreach (var el in allMainData)
                            {
                                if (el[mergeRules.parentIdColumnName] == pair.idMainCatalog)
                                {
                                    daughtersNames.Add(el[mergeRules.uniqueValueColumnName]);
                                }
                            }

                            // При новом значении обновляем у элемента ID и даем нового родителя, который будет в главной
                            if (!daughtersNames.Contains(root[mergeRules.uniqueValueColumnName]))
                            {
                                rootCopy[mergeRules.secondIdColumnName] = $"{++LastIdMainCatalog}";
                                rootCopy[mergeRules.parentIdColumnName] = pair.idMainCatalog;
                                allMainData.Add(rootCopy);
                                valueExisting = false;

                                ReservedValuesManager.AddValueToListOfInserts(rootCopy);
                            }
                            break;
                        }
                    }
                }

                ReservedValuesManager.AddPairToReservedPairs(tableName, rootCopy[mergeRules.secondIdColumnName], root[mergeRules.secondIdColumnName], valueExisting);

                if (rootChilds.Count > 0)
                {
                    RecurseSimpleHighTable(tableName, rootChilds, allDaughterData, allMainData, mergeRules, catalog, worker);
                }
            }
        }

        static Dictionary<string, string> UpdateRowForeignKeys(Dictionary<string, string> row, List<ReservedValuesManager.ForeignKey> reservedForeignKeys)
        {
            foreach (var foreignInfo in reservedForeignKeys)
            {
                if (row[foreignInfo.columnWithForeignKey] == null || row[foreignInfo.columnWithForeignKey].ToLower() == "'null'" || row[foreignInfo.columnWithForeignKey].ToLower() == "null")
                    continue;

                row[foreignInfo.columnWithForeignKey] = ReservedValuesManager.FindReservedKeyPair(foreignInfo.linkOnTable, idDaughterCatalog: row[foreignInfo.columnWithForeignKey]).idMainCatalog;
            }
            return row;
        }

        static Dictionary<string, string> EscapeSymbolsInRow(Dictionary<string, string> row)
        {
            Dictionary<string, string> escapedRow = new Dictionary<string, string>(row);

            foreach (string key in row.Keys)
            {
                if (escapedRow[key].Contains("'"))
                    escapedRow[key] = $"'{row[key].Substring(1, row[key].Length - 2).Replace("'", "\''")}'";
            }
            return escapedRow;
        }

        /// <summary>
        /// Процесс слияния простых таблиц, которые не содержат ссылок на другие таблицы
        /// </summary>
        /// <returns></returns>
        static int ProcessSimpleTable(string tableName, SimpleTableRulesDeserializer mergeRules, DBCatalog mainCatalog, DBCatalog daughterCatalog, BackgroundWorker worker)
        {
            ReservedValuesManager.ClearValuesToInsert();

            long lastId = 0;
            if (mergeRules.secondIdColumnName != null)
            {
                string lastIDFromDB = string.Join("", mainCatalog.SelectLastRecord(mergeRules.secondIdColumnName, tableName, mergeRules.secondIdColumnName)).Replace("\'", "");
                lastId = (lastIDFromDB != "") ? Convert.ToInt64(lastIDFromDB) : 0;
                LastIdMainCatalog = (lastIDFromDB != "") ? Convert.ToInt64(lastIDFromDB) : 0;
            }

            List<Dictionary<string, string>> mainCatalogTableData = mainCatalog.SelectAllFrom(tableName, mainCatalog.SelectColumnsNames(tableName, mergeRules.excludeSpecialsColumnsNames), mergeRules.allowsNullValues);
            List<Dictionary<string, string>> daughterCatalogTableData = daughterCatalog.SelectAllFrom(tableName, daughterCatalog.SelectColumnsNames(tableName, mergeRules.excludeSpecialsColumnsNames), mergeRules.allowsNullValues);

            Dictionary<string, string> foreigns = mainCatalog.SelectTablesAndForeignKeyUsage(tableName);
            if (foreigns.Count > 0)
            {
                ReservedValuesManager.AddForeignKeys(foreigns, tableName);
            }

            Consts.MergeProgress.ClearTasksBlock();
            Consts.MergeProgress.AddTaskInBlock(daughterCatalogTableData.Count);

            if (mergeRules.parentIdColumnName != null)
            {
                // При заходе в обработку нужно выполнить запросы и обновить классовые переменные с данными Главной и Дочерней БД. Также обновить последний ID и номер, если такой столбец есть
                // После получения данных и вхождение в условие иерархической таблицы, необходимо определить рутовые элементы
                List<Dictionary<string, string>> daughterZeroLevel = new List<Dictionary<string, string>>();
                foreach (var el in daughterCatalogTableData)
                {
                    if (el[mergeRules.parentIdColumnName] == null || el[mergeRules.parentIdColumnName].ToLower() == "'null'" || el[mergeRules.parentIdColumnName].ToLower() == "null")
                    {
                        daughterZeroLevel.Add(new Dictionary<string, string>(el));
                    }
                }

                // Передаем на обработку в рекурсию рутовые элементы Дочерней таблицы и все данные Дочерней таблицы
                RecurseSimpleHighTable(tableName, daughterZeroLevel, daughterCatalogTableData, mainCatalogTableData, mergeRules, mainCatalog, worker);

                worker.ReportProgress(Consts.MergeProgress.UpdateBlockBar(), Consts.WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
            }
            else
            {
                // Обычная таблица со значениями. Обработка происходит по очереди
                List<string> mainCatalogValues = ValuesManager.SelectDataFromColumn(mainCatalogTableData, mergeRules.uniqueValueColumnName);

                foreach (Dictionary<string, string> row in daughterCatalogTableData)
                {
                    if (mergeRules.secondIdColumnName == null && !mainCatalogValues.Contains(row[mergeRules.uniqueValueColumnName]))
                    {
                        ReservedValuesManager.AddValueToListOfInserts(row);

                        mainCatalogValues.Add(row[mergeRules.uniqueValueColumnName]);
                        mainCatalogTableData.Add(new Dictionary<string, string>(row));
                    }
                    else if (mergeRules.secondIdColumnName != null && mainCatalogValues.Contains(row[mergeRules.uniqueValueColumnName]) && foreigns.Count > 0)
                    {
                        string idMainCatalog = mainCatalogTableData.Find(el => el[mergeRules.uniqueValueColumnName] == row[mergeRules.uniqueValueColumnName])[mergeRules.secondIdColumnName];
                        ReservedValuesManager.AddPairToReservedPairs(tableName, idMainCatalog, row[mergeRules.secondIdColumnName], true);
                    }
                    else if (mergeRules.secondIdColumnName != null && !mainCatalogValues.Contains(row[mergeRules.uniqueValueColumnName]) && row[mergeRules.uniqueValueColumnName] != null)
                    {
                        Dictionary<string, string> rowCopy = new Dictionary<string, string>(row);
                        //string oldKey = row[mergeRules.secondIdColumnName];
                        //rowCopy[mergeRules.secondIdColumnName] = $"'{countOfImportsByTable + lastId + 1}'";
                        rowCopy[mergeRules.secondIdColumnName] = $"'{++lastId}'";

                        ReservedValuesManager.AddPairToReservedPairs(tableName, rowCopy[mergeRules.secondIdColumnName], row[mergeRules.secondIdColumnName], true);

                        ReservedValuesManager.AddValueToListOfInserts(rowCopy);

                        mainCatalogValues.Add(rowCopy[mergeRules.uniqueValueColumnName]);
                        mainCatalogTableData.Add(rowCopy);
                    }
                    worker.ReportProgress(Consts.MergeProgress.UpdateBlockBar(), Consts.WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
                }
            }

            // Импорт уникальных значений
            return MakeInsertsUniqueData(mainCatalog, tableName, ReservedValuesManager.GetValuesToInsert(), mergeRules.excludeSpecialsColumnsNames, worker);
        }

        static int ProcessLinkTable(string tableName, LinkTableRulesDeserializer mergeRules, DBCatalog mainCatalog, DBCatalog daughterCatalog, BackgroundWorker worker)
        {
            ReservedValuesManager.ClearValuesToInsert();
            long lastId = 0;

            if (mergeRules.secondIdColumnName != null)
            {
                string lastIDFromDB = string.Join("", mainCatalog.SelectLastRecord(mergeRules.secondIdColumnName, tableName, mergeRules.secondIdColumnName)).Replace("\'", "");
                lastId = (lastIDFromDB != "") ? Convert.ToInt64(lastIDFromDB) : 0;
            }

            int lastNumeric = 0;
            if (mergeRules.numerateColumn != null)
            {
                string lastNumericFromDB = string.Join("", mainCatalog.SelectLastRecord(mergeRules.numerateColumn, tableName, mergeRules.numerateColumn)).Replace("\'", "");
                lastNumeric = (lastNumericFromDB != "") ? Convert.ToInt32(lastNumericFromDB) : 0;
            }

            // 3. Берем таблицы в дальнейшем используемые
            Dictionary<string, string> foreignKeys = mainCatalog.SelectTablesAndForeignKeyUsage(tableName);

            // 4. Проверяем foreigns на наличие использования. Если True, то добавляем все в ReservDict
            if (foreignKeys.Count > 0)
            {
                ReservedValuesManager.AddForeignKeys(foreignKeys, tableName);
            }

            // Собираем все данных с таблиц
            List<Dictionary<string, string>> mainCatalogTableData = mainCatalog.SelectAllFrom(tableName, mainCatalog.SelectColumnsNames(tableName, mergeRules.excludeSpecialsColumnsNames), mergeRules.allowsNullValues);
            List<Dictionary<string, string>> daughterCatalogTableData = daughterCatalog.SelectAllFrom(tableName, daughterCatalog.SelectColumnsNames(tableName, mergeRules.excludeSpecialsColumnsNames), mergeRules.allowsNullValues);

            // Берем все связи, которые имеет таблица
            List<ReservedValuesManager.ForeignKey> tableReservedForeignKeys = ReservedValuesManager.GetForeignKeysByTable(tableName);

            
            if (mergeRules.parentIdColumnName != null)
            {
                // При заходе в обработку нужно выполнить запросы и обновить классовые переменные с данными Главной и Дочерней БД. Также обновить последний ID и номер, если такой столбец есть
                // После получения данных и вхождение в условие иерархической таблицы, необходимо определить рутовые элементы
                List<Dictionary<string, string>> daughterZeroLevel = new List<Dictionary<string, string>>();
                foreach (var el in daughterCatalogTableData)
                {
                    if (el[mergeRules.parentIdColumnName] == null || el[mergeRules.parentIdColumnName].ToLower() == "'null'" || el[mergeRules.parentIdColumnName].ToLower() == "null")
                    {
                        daughterZeroLevel.Add(new Dictionary<string, string>(el));
                    }
                }

                // Передаем на обработку в рекурсию рутовые элементы Дочерней таблицы и все данные Дочерней таблицы
                RecurseLinkHighTable(tableName, daughterZeroLevel, daughterCatalogTableData, mainCatalogTableData, mergeRules, tableReservedForeignKeys, mainCatalog, worker);

                //worker.ReportProgress(Consts.MergeProgress.UpdateBlockBar(), Consts.WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
            }
            else if (mergeRules.uniqueValueColumnName != null && mergeRules.secondIdColumnName != null && mergeRules.parentIdColumnName == null && mergeRules.existingDaughterValuesForParent == null)
            {
                List<string> mainCatalogValues = ValuesManager.SelectDataFromColumn(mainCatalogTableData, mergeRules.uniqueValueColumnName);

                foreach (Dictionary<string, string> row in daughterCatalogTableData)
                {
                    if (mainCatalogValues.Contains(row[mergeRules.uniqueValueColumnName]) && foreignKeys.Count > 0)
                    {
                        string idMainCatalog = mainCatalogTableData.Find(el => el[mergeRules.uniqueValueColumnName] == row[mergeRules.uniqueValueColumnName])[mergeRules.secondIdColumnName];
                        ReservedValuesManager.AddPairToReservedPairs(tableName, idMainCatalog, row[mergeRules.secondIdColumnName], true);
                    }
                    else if (!mainCatalogValues.Contains(row[mergeRules.uniqueValueColumnName]) && row[mergeRules.uniqueValueColumnName] != null)
                    {
                        Dictionary<string, string> rowCopy = new Dictionary<string, string>(row);

                        if (tableReservedForeignKeys.Count > 0)
                        {
                            rowCopy = MergeManager.UpdateRowForeignKeys(rowCopy, tableReservedForeignKeys);
                        }

                        if (mergeRules.numerateColumn != null)
                        {
                            rowCopy[mergeRules.numerateColumn] = $"'{++lastNumeric}'";
                        }

                        //string oldKey = row[mergeRules.secondIdColumnName];
                        //rowCopy[mergeRules.secondIdColumnName] = $"'{countOfImportsByTable + lastId + 1}'";
                        rowCopy[mergeRules.secondIdColumnName] = $"'{++lastId}'";

                        string newID = $"'{Guid.NewGuid()}'";
                        if (rowCopy.ContainsKey("DocID"))
                        {
                            rowCopy["DocID"] = mainCatalog.SelectIDFrom(mergeRules.existingDaughterValuesForParent.toTableName, mergeRules.existingDaughterValuesForParent.fromColumnName, rowCopy[mergeRules.existingDaughterValuesForParent.fromColumnName]);
                        }
                        rowCopy["ID"] = newID;

                        ReservedValuesManager.AddPairToReservedPairs(tableName, rowCopy[mergeRules.secondIdColumnName], row[mergeRules.secondIdColumnName], false);

                        ReservedValuesManager.AddValueToListOfInserts(rowCopy);

                        mainCatalogValues.Add(rowCopy[mergeRules.uniqueValueColumnName]);
                        mainCatalogTableData.Add(rowCopy);
                    }
                    //worker.ReportProgress(Consts.MergeProgress.UpdateBlockBar(), Consts.WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
                }
            }
            else if (mergeRules.parentIdColumnName == null && mergeRules.existingDaughterValuesForParent != null) // Берет все значения родителя и без разбора делает импорт всех нашедшихся
            {
                // при таком условии нужно по existingDaughterValuesForParent.toTableName получить все пары ключей
                // после чего проверить, необходимо ли затрагивать все значения или только новые
                List<ReservedValuesManager.KeyPair> tablePairs = ReservedValuesManager.GetReservedKeyPairsByTable(mergeRules.existingDaughterValuesForParent.toTableName, mergeRules.existingDaughterValuesForParent.onlyNew);

                foreach (var row in daughterCatalogTableData)
                {
                    foreach (var keyPair in tablePairs)
                    {
                        if (row[mergeRules.existingDaughterValuesForParent.fromColumnName] == keyPair.idDaughterCatalog)
                        {
                            Dictionary<string, string> rowCopy = new Dictionary<string, string>(row);

                            // Тут добавить проверку по  uniqueValueColumnName к примеру для tblFUND_RENAME
                            // Берем список значений по уникальному полю из главного каталога
                            if (mergeRules.uniqueValueColumnName != null)
                            {
                                List<Dictionary<string, string>> mainCatalogDataByParent = mainCatalogTableData.Where(r => r[mergeRules.existingDaughterValuesForParent.fromColumnName] == keyPair.idMainCatalog).ToList();
                                List<string> mainCatalogDataByParentValues = ValuesManager.SelectDataFromColumn(mainCatalogDataByParent, mergeRules.uniqueValueColumnName);

                                if (mainCatalogDataByParentValues.Contains(row[mergeRules.uniqueValueColumnName]) && foreignKeys.Count > 0)
                                {
                                    string idMainCatalog = mainCatalogDataByParent.Find(el => el[mergeRules.uniqueValueColumnName] == row[mergeRules.uniqueValueColumnName])[mergeRules.secondIdColumnName];
                                    ReservedValuesManager.AddPairToReservedPairs(tableName, idMainCatalog, row[mergeRules.secondIdColumnName], true);
                                }
                                else if (!mainCatalogDataByParentValues.Contains(row[mergeRules.uniqueValueColumnName]))
                                {
                                    if (tableReservedForeignKeys.Count > 0)
                                    {
                                        rowCopy = MergeManager.UpdateRowForeignKeys(rowCopy, tableReservedForeignKeys);
                                    }

                                    if (mergeRules.secondIdColumnName != null)
                                    {
                                        rowCopy[mergeRules.secondIdColumnName] = $"'{++lastId}'";
                                    }

                                    string newID = $"'{Guid.NewGuid()}'";
                                    if (rowCopy.ContainsKey("DocID"))
                                    {
                                        rowCopy["DocID"] = mainCatalog.SelectIDFrom(mergeRules.existingDaughterValuesForParent.toTableName, mergeRules.existingDaughterValuesForParent.fromColumnName, rowCopy[mergeRules.existingDaughterValuesForParent.fromColumnName]);
                                    }
                                    rowCopy["ID"] = newID;

                                    ReservedValuesManager.AddValueToListOfInserts(rowCopy);
                                    mainCatalogTableData.Add(rowCopy);

                                    if (foreignKeys.Count > 0)
                                    {
                                        ReservedValuesManager.AddPairToReservedPairs(tableName, rowCopy[mergeRules.secondIdColumnName], row[mergeRules.secondIdColumnName], false);
                                    }
                                }
                            }
                            else
                            {
                                if (tableReservedForeignKeys.Count > 0)
                                {
                                    rowCopy = MergeManager.UpdateRowForeignKeys(rowCopy, tableReservedForeignKeys);
                                }

                                if (mergeRules.secondIdColumnName != null)
                                {
                                    rowCopy[mergeRules.secondIdColumnName] = $"'{++lastId}'";
                                }

                                string newID = $"'{Guid.NewGuid()}'";
                                if (rowCopy.ContainsKey("DocID"))
                                {
                                    if (tableName == "tblDOCUMENT_STATS")
                                    {
                                        if (rowCopy["ISN_INVENTORY"].ToLower() == "null" || rowCopy["ISN_INVENTORY"].ToLower() == "'null'" || rowCopy["ISN_INVENTORY"] == null)
                                        {
                                            rowCopy["DocID"] = mainCatalog.SelectIDFrom(mergeRules.existingDaughterValuesForParent.toTableName, mergeRules.existingDaughterValuesForParent.fromColumnName, rowCopy[mergeRules.existingDaughterValuesForParent.fromColumnName]);
                                        }
                                        else
                                        {
                                            rowCopy["DocID"] = mainCatalog.SelectIDFrom("tblINVENTORY", "ISN_INVENTORY", rowCopy["ISN_INVENTORY"]);
                                        }
                                    }
                                    else
                                    {
                                        rowCopy["DocID"] = mainCatalog.SelectIDFrom(mergeRules.existingDaughterValuesForParent.toTableName, mergeRules.existingDaughterValuesForParent.fromColumnName, rowCopy[mergeRules.existingDaughterValuesForParent.fromColumnName]);
                                    }


                                }
                                rowCopy["ID"] = newID;

                                bool passedFotInsert = MergeManager.CheckRowColumnsValuesOnInsert(rowCopy, mergeRules.checkOnValue);

                                if (passedFotInsert)
                                {
                                    ReservedValuesManager.AddValueToListOfInserts(rowCopy);
                                    mainCatalogTableData.Add(rowCopy);
                                }

                                if (foreignKeys.Count > 0)
                                {
                                    ReservedValuesManager.AddPairToReservedPairs(tableName, rowCopy[mergeRules.secondIdColumnName], row[mergeRules.secondIdColumnName], false);
                                }
                            }
                            break;
                        }
                    }
                    //worker.ReportProgress(Consts.MergeProgress.UpdateBlockBar(), Consts.WorkerConsts.ITS_BLOCK_PROGRESS_BAR);
                }


                // Пройтись по этим парам. Найти по дочернему ключу значения проверяемой таблицы, далее заменить existingDaughterValuesForParent.fromColumnName на значений из главной и сделать импорт

            }

            // Импорт уникальных значений
            return MakeInsertsUniqueData(mainCatalog, tableName, ReservedValuesManager.GetValuesToInsert(), mergeRules.excludeSpecialsColumnsNames, worker);
        }

        static int MakeInsertsUniqueData(DBCatalog catalog, string tableName, List<Dictionary<string, string>> uniqueData, List<string> excludeSpecialsColumnsNames, BackgroundWorker worker)
        {
            List<Dictionary<string, string>> fastRequestData = new List<Dictionary<string, string>>();
            int rowNumber = 0;

            foreach (var importValue in uniqueData)
            {
                if (FastRequestMod)
                {
                    rowNumber++;
                    fastRequestData.Add(new Dictionary<string, string>(importValue));

                    if (fastRequestData.Count == 1000 || uniqueData.Count == rowNumber)
                    {
                        catalog.InsertListOfValues(tableName, fastRequestData, excludeSpecialsColumnsNames);
                        fastRequestData.Clear();

                        Consts.ALL_OF_IMPORT += fastRequestData.Count;
                        worker.ReportProgress(Consts.WorkerConsts.UPDATE_COUNT_OF_IMPORT);
                    }
                }
                else
                {
                    catalog.InsertValue(tableName, MergeManager.EscapeSymbolsInRow(importValue));

                    Consts.ALL_OF_IMPORT++;
                    worker.ReportProgress(Consts.WorkerConsts.UPDATE_COUNT_OF_IMPORT);
                }
            }

            return uniqueData.Count;
        }
    }
}
