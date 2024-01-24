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
    public static class MergeManager
    {


        public static Dictionary<string, Tuple<string, string, List<string>, string>> defaultTablesParams = new Dictionary<string, Tuple<string, string, List<string>, string>>
        {
            // 1. string uniqueValueColumnName         2. string idLikeColumnName         3. List<string> excludeColumns         4. string highLevelColumnName
            { "eqUsers",
                new Tuple<string, string, List<string>, string>("Login", null, new List<string>(){ "DisplayName" }, null) },
            
            { "tblACT_TYPE_CL",
                new Tuple<string, string, List<string>, string>("NAME", "ISN_ACT_TYPE", null, null) },
            
            { "tblAuthorizedDep",
                new Tuple<string, string, List<string>, string>("ShortName", "ISN_AuthorizedDep", null, null) },
            
            { "tblCLS",
                new Tuple<string, string, List<string>, string>("NAME", "ISN_CLS", null, "ISN_HIGH_CLS") },
            
            { "tblDataExport",
                new Tuple<string, string, List<string>, string>("fcDbName", null, null, null) },
            
            { "tblDECL_COMMISSION_CL",
                new Tuple<string, string, List<string>, string>("NAME", "ISN_COMMISSION", null, null) },
            
            //{ "tblConstantsSpec", ProcessConstantsSpec },
            
            { "tblGROUPING_ATTRIBUTE_CL",
                new Tuple<string, string, List<string>, string>("NAME", "ISN_GROUPING_ATTRIBUTE", null, null) },
            
            { "tblINV_REQUIRED_WORK_CL",
                new Tuple<string, string, List<string>, string>("NAME", "ISN_REQUIRED_WORK", null, null) },
            
            { "tblLANGUAGE_CL",
                new Tuple<string, string, List<string>, string>("NAME", "ISN_LANGUAGE", null, null) },
            
            { "tblFEATURE",
                new Tuple<string, string, List<string>, string>("NAME", "ISN_FEATURE", null, "ISN_HIGH_FEATURE") },
            
            { "tblCITIZEN_CL",
                new Tuple<string, string, List<string>, string>("NAME", "ISN_CITIZEN", null, null) },
            
            { "tblORGANIZ_CL",
                new Tuple<string, string, List<string>, string>("NAME", "ISN_ORGANIZ", null, null) },
            
            { "tblPAPER_CLS",
                new Tuple<string, string, List<string>, string>("NAME", "ISN_PAPER_CLS", null, null) },
            
            { "tblPAPER_CLS_INV",
                new Tuple<string, string, List<string>, string>("NAME", "ISN_PAPER_CLS_INV", null, null) },
            
            { "tblPUBLICATION_TYPE_CL",
                new Tuple<string, string, List<string>, string>("NAME", "ISN_PUBLICATION_TYPE", null, null) },
            
            { "tblQUESTION",
                new Tuple<string, string, List<string>, string>("NAME", "ISN_QUESTION", null, null) },
            
            { "tblRECEIPT_REASON_CL",
                new Tuple<string, string, List<string>, string>("NAME", "ISN_RECEIPT_REASON", null, null) },
            
            { "tblRECEIPT_SOURCE_CL",
                new Tuple<string, string, List<string>, string>("NAME", "ISN_RECEIPT_SOURCE", null, null) },
            
            { "tblREF_FILE",
                new Tuple<string, string, List<string>, string>("NAME", "ISN_REF_FILE", null, null) },
            
            { "tblREPRODUCTION_METHOD_CL",
                new Tuple<string, string, List<string>, string>("NAME", "ISN_REPRODUCTION_METHOD", null, null) },
            
            // { "tblService", ProcessService },
            
            { "tblSTATE_CL",
                new Tuple<string, string, List<string>, string>("NAME", "ISN_STATE", null, "ISN_HIGH_STATE") },
            
            { "tblSTORAGE_MEDIUM_CL",
                new Tuple<string, string, List<string>, string>("NAME", "ISN_STORAGE_MEDIUM", null, "ISN_HIGH_STORAGE_MEDIUM") },
            
            { "tblSUBJECT_CL",
                new Tuple<string, string, List<string>, string>("NAME", "ISN_SUBJECT", null, "ISN_HIGH_SUBJECT") },
            
            { "tblTREE_SUPPORT",
                new Tuple<string, string, List<string>, string>("ISN", null, null, null) },
            
            { "tblWORK_CL",
                new Tuple<string, string, List<string>, string>("NAME", "ISN_WORK", null, null) },
            
            { "rptFUND_PAPER",
                new Tuple<string, string, List<string>, string>("ISN_FUND", null, null, null) },
            
            { "rptFUND_UNIT_REG_STATS",
                new Tuple<string, string, List<string>, string>("ISN_FUND", null, null, null) },
        };

        public static Dictionary<string, Tuple<string, string, List<string>, string, string, string>> linksTablesParams = new Dictionary<string, Tuple<string, string, List<string>, string, string, string>>
        {
            // 1. string uniqueValueColumnName         2. string idLikeColumnName         3. List<string> excludeColumns    4. string highLevelColumnName     5. string parentIdColumn         6. string numerateColumn                          bool usedFurther, string foreignIdColumn = null
            { "tblORGANIZ_RENAME",
                new Tuple<string, string, List<string>, string, string, string>("NAME", "ISN_ORGANIZ_RENAME", null, null, "ISN_ORGANIZ", null) },

            //{ "tblARCHIVE",
            //    new Tuple<string, string, List<string>, string>("", null, null, null, null) },

            { "tblLOCATION",
                new Tuple<string, string, List<string>, string, string, string>("NAME", "ISN_LOCATION", null, "ISN_HIGH_LOCATION", null, null) },

/*            { "tblARCHIVE_STORAGE",
                new Tuple<string, string, List<string>, string, string, string>("", null, null, null, null, null) },*/

            //{ "tblARCHIVE_PASSPORT",
            //    new Tuple<string, string, List<string>, string, string, string>("", null, null, null, null, null) },

            //{ "tblARCHIVE_STATS",
            //    new Tuple<string, string, List<string>, string, string, string>("", null, null, null, null, null) },

            { "tblFUND",
                new Tuple<string, string, List<string>, string, string, string>("FUND_NAME_SHORT", "ISN_FUND", null, null, null, "FUND_NUM_2") },

            { "tblFUND_RENAME",
                new Tuple<string, string, List<string>, string, string, string>("FUND_NAME_SHORT", "ISN_FUND_RENAME", null, null, "ISN_FUND", null) },

            { "tblFUND_CHECK",
                new Tuple<string, string, List<string>, string, string, string>("ISN_FUND", null, null, null, "ISN_FUND", null) },

            { "tblFUND_DOC_TYPE",
                new Tuple<string, string, List<string>, string, string, string>(null, null, null, null, "ISN_FUND", null) },

            { "tblFUND_INCLUSION",
                new Tuple<string, string, List<string>, string, string, string>(null, "ISN_INCLUSION", null, null, "ISN_FUND", null) },

            { "tblFUND_PAPER_CLS",
                new Tuple<string, string, List<string>, string, string, string>(null, "ISN_PAPER_CLS", null, null, "ISN_FUND", null) },

            { "tblPUBLICATION_CL",
                new Tuple<string, string, List<string>, string, string, string>("PUBLICATION_NAME", "ISN_PUBLICATION", null, null, null, "PUBLICATION_NUM") },

            { "tblFUND_PUBLICATION",
                new Tuple<string, string, List<string>, string, string, string>(null, "ISN_PUBLICATION", null, null, "ISN_FUND", null) },

            { "tblFUND_RECEIPT_REASON",
                new Tuple<string, string, List<string>, string, string, string>(null, null, null, null, "ISN_FUND", null) },

            { "tblFUND_RECEIPT_SOURCE",
                new Tuple<string, string, List<string>, string, string, string>(null, null, null, null, "ISN_FUND", null) },

            { "tblFUND_OAF",
                new Tuple<string, string, List<string>, string, string, string>("FUND_NAME_SHORT", "ISN_OAF", null, null, "ISN_FUND", "FUND_NUM_2") },

            { "tblFUND_OAF_REASON",
                new Tuple<string, string, List<string>, string, string, string>(null, "ISN_OAF_REASON", null, null, "ISN_FUND", null) },

            { "tblFUND_COLLECTION_REASONS",
                new Tuple<string, string, List<string>, string, string, string>(null, "ISN_COLLECTION_REASON", null, null, "ISN_FUND", null) },

            { "tblFUND_CREATOR",
                new Tuple<string, string, List<string>, string, string, string>(null, "ISN_FUND_CREATOR", null, null, "ISN_FUND", null) },

            { "tblUNDOCUMENTED_PERIOD",
                new Tuple<string, string, List<string>, string, string, string>("PERIOD_START_YEAR", "ISN_PERIOD", null, null, "ISN_FUND", null) },

            { "tblDEPOSIT",
                new Tuple<string, string, List<string>, string, string, string>("DEPOSIT_NAME", "ISN_DEPOSIT", null, null, "ISN_FUND", "DEPOSIT_NUM") },

            { "tblDEPOSIT_DOC_TYPE",
                new Tuple<string, string, List<string>, string, string, string>(null, "ISN_DEPOSIT_DOC_TYPE", null, null, "ISN_DEPOSIT", null) },

            { "tblACT",
                new Tuple<string, string, List<string>, string, string, string>("ACT_DATE", "ISN_ACT", null, null, "ISN_FUND", null) },

            { "tblINVENTORY",
                new Tuple<string, string, List<string>, string, string, string>("INVENTORY_NAME", "ISN_INVENTORY", null, null, "ISN_FUND", "INVENTORY_NUM_1") },

            { "tblINVENTORY_CHECK",
                new Tuple<string, string, List<string>, string, string, string>(null, null, null, null, "ISN_INVENTORY", null) },

            { "tblINVENTORY_DOC_STORAGE",
                new Tuple<string, string, List<string>, string, string, string>(null, null, null, null, "ISN_INVENTORY", null) },

            { "tblINVENTORY_DOC_TYPE",
                new Tuple<string, string, List<string>, string, string, string>(null, null, null, null, "ISN_INVENTORY", null) },
            
            { "tblINVENTORY_CLS_ATTR",
                new Tuple<string, string, List<string>, string, string, string>(null, null, null, null, "ISN_INVENTORY", null) },

            { "tblINVENTORY_GROUPING_ATTRIBUTE",
                new Tuple<string, string, List<string>, string, string, string>(null, "ISN_GROUPING_ATTRIBUTE", null, null, "ISN_INVENTORY", null) },

            { "tblINVENTORY_PAPER_CLS",
                new Tuple<string, string, List<string>, string, string, string>(null, "ISN_PAPER_CLS_INV", null, null, "ISN_INVENTORY", null) },

            { "tblINVENTORY_REQUIRED_WORK",
                new Tuple<string, string, List<string>, string, string, string>(null, "ISN_REQUIRED_WORK", null, null, "ISN_INVENTORY", null) },

            { "tblINVENTORY_STRUCTURE",
                new Tuple<string, string, List<string>, string, string, string>("NAME", "ISN_INVENTORY_CLS", null, "ISN_HIGH_INVENTORY_CLS", "ISN_INVENTORY", null) },

            /*{ "tblDOCUMENT_STATS",
                new Tuple<string, string, List<string>, string, string, string>(null, "ISN_DOCUMENT_STATS", null, null, "ISN_INVENTORY", null) },
*/
            { "tblREF_ACT",
                new Tuple<string, string, List<string>, string, string, string>(null, "ISN_REF_ACT", null, null, null, null) },

            { "tblREF_CLS",
                new Tuple<string, string, List<string>, string, string, string>(null, "ISN_REF_CLS", null, null, null, null) },

            { "tblREF_FEATURE",
                new Tuple<string, string, List<string>, string, string, string>(null, "ISN_REF_FEATURE", null, null, null, null) },

            { "tblREF_LANGUAGE",
                new Tuple<string, string, List<string>, string, string, string>(null, "ISN_REF_LANGUAGE", null, null, null, null) },

            { "tblREF_LOCATION",
                new Tuple<string, string, List<string>, string, string, string>(null, "ISN_REF_LOCATION", null, null, null, null) },

            { "tblREF_QUESTION",
                new Tuple<string, string, List<string>, string, string, string>(null, "ISN_REF_QUESTION", null, null, null, null) },

            { "tblUNIT",
                new Tuple<string, string, List<string>, string, string, string>("NAME", "ISN_UNIT", null, null, "ISN_INVENTORY", null) },

            { "tblUNIT_ELECTRONIC",
                new Tuple<string, string, List<string>, string, string, string>("ISN_UNIT", null, null, null, "ISN_UNIT", null) },

            { "tblUNIT_FOTO",
                new Tuple<string, string, List<string>, string, string, string>("ISN_UNIT", null, null, null, "ISN_UNIT", null) },

            /*{ "tblUNIT_FOTO_EX",
                new Tuple<string, string, List<string>, string, string, string>("", null, null, null, null, null) },
*/
            { "tblUNIT_MICROFORM",
                new Tuple<string, string, List<string>, string, string, string>("ISN_UNIT", null, null, null, "ISN_UNIT", null) },

            { "tblUNIT_MOVIE",
                new Tuple<string, string, List<string>, string, string, string>("ISN_UNIT", null, null, null, "ISN_UNIT", null) },

            /*{ "tblUNIT_MOVIE_EX",
                new Tuple<string, string, List<string>, string, string, string>("", null, null, null, null, null) },
*/
            { "tblUNIT_NTD",
                new Tuple<string, string, List<string>, string, string, string>("ISN_UNIT", null, null, null, "ISN_UNIT", null) },

            { "tblUNIT_PHONO",
                new Tuple<string, string, List<string>, string, string, string>("ISN_UNIT", null, null, null, "ISN_UNIT", null) },

            { "tblUNIT_REQUIRED_WORK",
                new Tuple<string, string, List<string>, string, string, string>(null, "ISN_UNIT_REQUIRED_WORK", null, null, "ISN_UNIT", null) },

            { "tblUNIT_STATE",
                new Tuple<string, string, List<string>, string, string, string>(null, "ISN_UNIT_STATE", null, null, "ISN_UNIT", null) },

            { "tblUNIT_VIDEO",
                new Tuple<string, string, List<string>, string, string, string>("ISN_UNIT", null, null, null, "ISN_UNIT", null) },

            /*{ "tblUNIT_VIDEO_EX",
                new Tuple<string, string, List<string>, string, string, string>("", null, null, null, null, null) },
*/
            { "tblUNIT_WORK",
                new Tuple<string, string, List<string>, string, string, string>(null, "ISN_UNIT_WORK", null, null, "ISN_UNIT", null) },

            { "tblDOCUMENT",
                new Tuple<string, string, List<string>, string, string, string>("NAME", "ISN_DOCUM", null, null, "ISN_UNIT", null) },
        };

        /// <summary>
        /// Очищает таблицы с логами
        /// </summary>
        /// <returns>Успешность выполнения транзакций</returns>
        public static bool ClearLogs(DBCatalog mainCatalog, BackgroundWorker worker)
        {   
            foreach (string logTable in mainCatalog.SelectLogTables())
            {
                worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, $"Очистка {logTable}:");

                try
                {
                    int recordsCount = mainCatalog.ClearTable(logTable);

                    if (mainCatalog.SelectCountRowsTable(logTable) == 0)
                    {
                        worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(VisualConsts.SPACE_SIZE) + "Пустая таблица.");
                    }
                    else
                    {
                        worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(VisualConsts.SPACE_SIZE) + $"Записей удалено {recordsCount}");
                    }
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
                worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, $"Обработка {defaultTable}:");

                if (mainCatalog.SelectCountRowsTable(defaultTable) == 0)
                {
                    worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(VisualConsts.SPACE_SIZE) + "Пустая таблица.");
                }
                else
                {
                    worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(VisualConsts.SPACE_SIZE) + "Таблица с дефолтными значениями.");
                }
            }
        }

        public static void ProcessDefaultTables(DBCatalog mainCatalog, DBCatalog daughterCatalog, BackgroundWorker worker)
        {
            //ReturnProcessStatus(worker, mainCatalog, defaultTablesFunctions, daughterCatalog);


            foreach (string tableName in mainCatalog.SelectDefaultProcessingTables())
            {
                int mainCount = mainCatalog.SelectCountRowsTable(tableName);
                int daughterCount = daughterCatalog.SelectCountRowsTable(tableName);


                worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, $"Обработка {tableName}:");

                if (daughterCatalog.SelectCountRowsTable(tableName) == 0)
                {
                    worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(VisualConsts.SPACE_SIZE) + "Пустая таблица.");
                }
                // старый принцип. Испольщовал взятия функции из словаря, далее формировния данных в этих функция (более не актуально, удалится после написания логики составных таблиц)
/*                else if (defaultTablesFunctions.ContainsKey(tableName))
                {
                    worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(VisualConsts.SPACE_SIZE) + $"Импортировано значений {defaultTablesFunctions[tableName](mainCatalog, daughterCatalog, tableName)}");

                }*/
                // Вызывается функция обработчик, которая собирает данные из кортежа
                else if (defaultTablesParams.ContainsKey(tableName))
                {
                    Tuple<string, string, List<string>, string> paramsForProcessing = defaultTablesParams[tableName];
                    worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(VisualConsts.SPACE_SIZE) + $"Импортировано значений {ProcessDefaultTable(mainCatalog, daughterCatalog, uniqueValueColumnName: paramsForProcessing.Item1, idLikeColumnName: paramsForProcessing.Item2, tableName: tableName, excludeColumns: paramsForProcessing.Item3, highLevelColumnName: paramsForProcessing.Item4)}");
                }
                else
                {
                    worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(VisualConsts.SPACE_SIZE) + "Обработчик отсутствует.");
                }
            }
        }

        public static void ProcessLinksTables(DBCatalog mainCatalog, DBCatalog daughterCatalog, BackgroundWorker worker)
        {
            //ReturnProcessStatus(worker, mainCatalog, linksTablesFunctions, daughterCatalog);


            foreach (string tableName in mainCatalog.SelectLinksTables())
            {
                int mainCount = mainCatalog.SelectCountRowsTable(tableName);
                int daughterCount = daughterCatalog.SelectCountRowsTable(tableName);


                worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, $"Обработка {tableName}:");

                if (daughterCatalog.SelectCountRowsTable(tableName) == 0)
                {
                    worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(VisualConsts.SPACE_SIZE) + "Пустая таблица.");
                }
/*                else if (linksTablesFunctions.ContainsKey(tableName))
                {
                    worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(VisualConsts.SPACE_SIZE) + $"Импортировано значений {linksTablesFunctions[tableName](mainCatalog, daughterCatalog, tableName)}");
                }*/
                else if (linksTablesParams.ContainsKey(tableName))
                {
                    Tuple<string, string, List<string>, string, string, string> paramsForProcessing = linksTablesParams[tableName];
                    // 1. string uniqueValueColumnName         2. string idLikeColumnName         3. List<string> excludeColumns    4. string highLevelColumnName     5. string parentIdColumn         6. string numerateColumn                          bool usedFurther, string foreignIdColumn = null

                    worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(VisualConsts.SPACE_SIZE) + $"Импортировано значений {ProcessLinksTable_new(mainCatalog, daughterCatalog, uniqueValueColumnName: paramsForProcessing.Item1, idLikeColumnName: paramsForProcessing.Item2, tableName: tableName, excludeColumns: paramsForProcessing.Item3, highLevelColumnName: paramsForProcessing.Item4, parentIdColumn: paramsForProcessing.Item5, numerateColumn: paramsForProcessing.Item6)}");
                }
                else
                {
                    worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(VisualConsts.SPACE_SIZE) + "Обработчик отсутствует.");
                }
            }
        }

        static void ReturnProcessStatus(BackgroundWorker worker, DBCatalog mainCatalog, Dictionary<string, Func<DBCatalog, DBCatalog, string, int>> functionsDict = null, DBCatalog daughterCatalog = null)
        {
            foreach (string tableName in mainCatalog.SelectLinksTables())
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

                worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, $"Обработка {tableName}:");

                if (checkEmpty == 0)
                {
                    worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(VisualConsts.SPACE_SIZE) + "Пустая таблица.");
                }
                else if (functionsDict.ContainsKey(tableName))
                {
                    worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(VisualConsts.SPACE_SIZE) + $"Импортировано значений {functionsDict[tableName](mainCatalog, daughterCatalog, tableName)}");
                }
                else
                {
                    worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(VisualConsts.SPACE_SIZE) + "Обработчик отсутствует.");
                }
            }
        }



        // --- Master merge process table ---

        // Для обработки дефолтных таблиц (дописать логику позже)
        static int ProcessDefaultTable(DBCatalog mainCatalog, DBCatalog daughterCatalog, string uniqueValueColumnName, string idLikeColumnName, string tableName, List<string> excludeColumns = null, string highLevelColumnName = null)
        {
            // -------- Общий блок инициализации вне зависимости от переданных парпаметров--------
            int countOfImports = 0;

            long lastId = 0;
            if (idLikeColumnName != null)
            {
                string lastIDFromDB = string.Join("", mainCatalog.SelectLastRecord(idLikeColumnName, tableName, idLikeColumnName)).Replace("\'", "");
                lastId = (lastIDFromDB != "") ? Convert.ToInt64(lastIDFromDB) : 0;
            }
            //long lastId = Convert.ToInt64(string.Join("", mainCatalog.SelectLastRecord(idLikeColumnName, tableName, idLikeColumnName)).Replace("\'", ""));
            // 1. Берем все записи двух каталогов в виде словарей
            List<Dictionary<string, string>> allFromMainData = mainCatalog.SelectAllFrom(tableName);
            List<Dictionary<string, string>> allFromDaughterData = daughterCatalog.SelectAllFrom(tableName);
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


            if (highLevelColumnName != null)
            {
                // Если есть highLevelColumnName значит есть и idLikeColumnName


                foreach (Dictionary<string, string> row in allFromDaughterData)
                {
                    // Если значение присутствует в главной таблице
                    if (mainCatalogValues.Contains(row[uniqueValueColumnName]))
                    {
                        ValuesManager.AddPairKeysToRewriteDict(foreigns, idLikeColumnName, new Tuple<string, string>(row[idLikeColumnName], ValuesManager.ReturnValue(allFromMainData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName)));
                    }
                    // Если значения в главной нет и idLikeColumnName БОЛЬШЕ чем highLevelColumnName 
                    else if (!mainCatalogValues.Contains(row[uniqueValueColumnName]) && Convert.ToInt64(row[idLikeColumnName].Replace("\'", "")) > Convert.ToInt64(row[highLevelColumnName].Replace("\'", "")))
                    {
                        // Первым делом взять high и проверить его значение в дочерней с главной
                        //string s = ValuesManager.ReturnValue(allFromDaughterData, highLevelColumnName, row[highLevelColumnName], uniqueValueColumnName);
                        if (mainCatalogValues.Contains(ValuesManager.ReturnValue(allFromDaughterData, highLevelColumnName, row[highLevelColumnName], uniqueValueColumnName)))
                        {
                            row[highLevelColumnName] = (ValuesManager.ReturnValue(allFromMainData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName) != "") ? ValuesManager.ReturnValue(allFromMainData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName) : "'null'";
                            row[idLikeColumnName] = $"'{lastId + countOfImports + 1}'";
                            ValuesManager.AddPairKeysToRewriteDict(foreigns, idLikeColumnName, new Tuple<string, string>(ValuesManager.ReturnValue(allFromDaughterData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName), row[idLikeColumnName]));
                            mainCatalog.InsertValue(tableName, ValuesManager.RemoveUnnecessary(row, excludeColumns));
                            mainCatalogValues.Add(row[uniqueValueColumnName]);
                            countOfImports++;
                        }// Если значения по ключу High нет в записях главного каталога
                        else if (!mainCatalogValues.Contains(ValuesManager.ReturnValue(allFromDaughterData, highLevelColumnName, row[highLevelColumnName], uniqueValueColumnName)))
                        {
                            // Добавляем в конец очереди остальных
                            reservedRows.Add(row);
                        }
                    } // Если значения в главной нет и idLikeColumnName МЕНЬШЕ чем highLevelColumnName 
                    else if (!mainCatalogValues.Contains(row[uniqueValueColumnName]) && Convert.ToInt64(row[idLikeColumnName].Replace("\'", "")) < Convert.ToInt64(row[highLevelColumnName].Replace("\'", "")))
                    {
                        if (mainCatalogValues.Contains(ValuesManager.ReturnValue(allFromDaughterData, highLevelColumnName, row[highLevelColumnName], uniqueValueColumnName)))
                        {
                            row[highLevelColumnName] = (ValuesManager.ReturnValue(allFromMainData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName) != "") ? ValuesManager.ReturnValue(allFromMainData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName) : "'null'";
                            row[idLikeColumnName] = $"'{lastId + countOfImports + 1}'";
                            ValuesManager.AddPairKeysToRewriteDict(foreigns, idLikeColumnName, new Tuple<string, string>(ValuesManager.ReturnValue(allFromDaughterData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName), row[idLikeColumnName]));
                            mainCatalog.InsertValue(tableName, ValuesManager.RemoveUnnecessary(row, excludeColumns));
                            mainCatalogValues.Add(row[uniqueValueColumnName]);
                            countOfImports++;
                        }// Если значения по ключу High нет в записях главного каталога
                        else if (!mainCatalogValues.Contains(ValuesManager.ReturnValue(allFromDaughterData, highLevelColumnName, row[highLevelColumnName], uniqueValueColumnName)))
                        {
                            // Добавляем в конец очереди остальных
                            reservedRows.Add(row);
                        }
                    }
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
                        //long lastId = Convert.ToInt64(string.Join("", mainCatalog.SelectLastRecord(idLikeColumnName, tableName, idLikeColumnName)).Replace("\'", ""));
                        row[idLikeColumnName] = $"'{countOfImports + lastId + 1}'";
                        //ValuesManager.AddPairKeysToReloadDict(foreigns, idLikeColumnName, new Tuple<string, string>(ValuesManager.ReturnValue(allFromMainData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName), row[idLikeColumnName]));
                        ValuesManager.AddPairKeysToRewriteDict(foreigns, idLikeColumnName, new Tuple<string, string>(ValuesManager.ReturnValue(allFromDaughterData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName), row[idLikeColumnName]));
                        mainCatalog.InsertValue(tableName, ValuesManager.RemoveUnnecessary(row, excludeColumns));
                        mainCatalogValues.Add(row[uniqueValueColumnName]);
                        countOfImports++;
                    }
                }
            }
            // Обработка отложенных записей
            if (reservedRows.Count > 0)
            {
                foreach (Dictionary<string, string> row in reservedRows)
                {
                    if (mainCatalogValues.Contains(ValuesManager.ReturnValue(allFromDaughterData, highLevelColumnName, row[highLevelColumnName], uniqueValueColumnName)))
                    {
                        row[highLevelColumnName] = (ValuesManager.ReturnValue(allFromMainData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName) != "") ? ValuesManager.ReturnValue(allFromMainData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName) : "'null'";
                        row[idLikeColumnName] = $"'{lastId + countOfImports + 1}'";
                        ValuesManager.AddPairKeysToRewriteDict(foreigns, idLikeColumnName, new Tuple<string, string>(ValuesManager.ReturnValue(allFromDaughterData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName), row[idLikeColumnName]));
                        mainCatalog.InsertValue(tableName, ValuesManager.RemoveUnnecessary(row, excludeColumns));
                        mainCatalogValues.Add(row[uniqueValueColumnName]);
                        countOfImports++;
                    }
                    else
                    {
                        row[highLevelColumnName] = "'null'";
                        row[idLikeColumnName] = $"'{lastId + countOfImports + 1}'";
                        ValuesManager.AddPairKeysToRewriteDict(foreigns, idLikeColumnName, new Tuple<string, string>(ValuesManager.ReturnValue(allFromDaughterData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName), row[idLikeColumnName]));
                        mainCatalog.InsertValue(tableName, ValuesManager.RemoveUnnecessary(row, excludeColumns));
                        mainCatalogValues.Add(row[uniqueValueColumnName]);
                        countOfImports++;
                    }
                }
            }
            return countOfImports;
        }

        static int ProcessLinksTable(DBCatalog mainCatalog, DBCatalog daughterCatalog, string uniqueValueColumnName, string idLikeColumnName, string tableName, bool usedFurther, string foreignIdColumn = null, List<string> excludeColumns = null, string parentIdColumn = null, string numerateColumn = null)
        {
            return 1;
        }

        // 1. string uniqueValueColumnName         2. string idLikeColumnName         3. List<string> excludeColumns    4. string highLevelColumnName     5. string parentIdColumn         6. string numerateColumn
        // Для обработки таблиц с внешними ключами mainCatalog, daughterCatalog, uniqueValueColumnName: paramsForProcessing.Item1, idLikeColumnName: paramsForProcessing.Item2, tableName: tableName, excludeColumns: paramsForProcessing.Item3, highLevelColumnName: paramsForProcessing.Item4, parentIdColumn: paramsForProcessing.Item5, numerateColumn: paramsForProcessing.Item6
        static int ProcessLinksTable_new(DBCatalog mainCatalog, DBCatalog daughterCatalog, string uniqueValueColumnName, string idLikeColumnName, string tableName, List<string> excludeColumns, string highLevelColumnName, string parentIdColumn, string numerateColumn)
        {
            // -------- Общий блок инициализации вне зависимости от переданных парпаметров--------
            int countOfImports = 0;


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
            

            // 3. Берем таблицы в дальнейшем используемые
            Dictionary<string, string> foreigns = mainCatalog.SelectTablesAndForeignKeyUsage(tableName);

            // 4. Проверяем foreigns на наличие использования. Если True, то добавляем все в ReservDict
            if (foreigns.Count > 0)
            {
                ValuesManager.AddNewTableToReserve(foreigns);
            }
            // ----------------

            MessageBox.Show(tableName + "\n" + string.Join("  ", foreigns.Keys));

            // Если нет родителя к которому цепляются записи, то обработка по всем значениям таблицы
            if (parentIdColumn == null && uniqueValueColumnName != null)
            {
                // 1. Берем все записи двух каталогов в виде словарей
                List<Dictionary<string, string>> allFromMainData = mainCatalog.SelectAllFrom(tableName);
                List<Dictionary<string, string>> allFromDaughterData = daughterCatalog.SelectAllFrom(tableName);
                List<Dictionary<string, string>> reservedRows = new List<Dictionary<string, string>>();

                // 2. Берем список значений по уникальному полю из главного каталога
                List<string> mainCatalogValues = ValuesManager.SelectDataFromColumn(allFromMainData, uniqueValueColumnName);


                foreach (Dictionary<string, string> row in allFromDaughterData)
                {
                    // Если значение ИМЕЕТСЯ в главном каталоге
                    if (mainCatalogValues.Contains(row[uniqueValueColumnName]))
                    {
                        // и используется дальше, то добавляем 
                        if (foreigns.Count > 0)
                        {
                            //MessageBox.Show((row[idLikeColumnName] + "   new: " + ValuesManager.ReturnValue(allFromMainData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName)));
                            ValuesManager.AddPairKeysToReserve(foreigns, idLikeColumnName, new Tuple<string, string>(row[idLikeColumnName], ValuesManager.ReturnValue(allFromMainData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName)));
                        }
                    } // Если значения НЕТ в главном каталоге
                    else if (!mainCatalogValues.Contains(row[uniqueValueColumnName]))
                    {
                        row[idLikeColumnName] = $"'{lastId + countOfImports + 1}'";

                        if (numerateColumn != null)
                        {
                            row[numerateColumn] = $"'{lastNumeric + countOfImports + 1}'";
                        }



                        if (foreigns.Count > 0)
                        {
                            ValuesManager.AddPairKeysToReserve(foreigns, idLikeColumnName, new Tuple<string, string>(ValuesManager.ReturnValue(allFromDaughterData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName), row[idLikeColumnName]));
                        }

                        if (ValuesManager.ContainsInRewrite(tableName))
                        {
                            mainCatalog.InsertValue(tableName, ValuesManager.RepareColumnsValues(ValuesManager.RemoveUnnecessary(row, excludeColumns), tableName));
                        }
                        else
                        {
                            mainCatalog.InsertValue(tableName, ValuesManager.RemoveUnnecessary(row, excludeColumns));
                        }

                        countOfImports++;
                    }



                }
            } // Если есть родителей, то отбор данных совершенно иной
            else if (parentIdColumn != null && uniqueValueColumnName != null)
            {
                Dictionary<string, List<Tuple<string, string>>> tableReservedValues = ValuesManager.ReturnTableValuesReserveDict(tableName);
                List<Dictionary<string, string>> reservedRows = new List<Dictionary<string, string>>();

                foreach (Tuple<string, string> pairKeys in tableReservedValues[parentIdColumn])
                {
                    //pairKeys.Item1; // значения ключей, которые были в дочерней
                    //pairKeys.Item2; // значения ключей, которые теперь в главной
                    //MessageBox.Show("old:" + pairKeys.Item1 + "  new:" + pairKeys.Item2);
                    List<Dictionary<string, string>> filterFromMainData = mainCatalog.SelectRecordsWhere(new List<string>(), tableName, parentIdColumn, pairKeys.Item2);
                    List<Dictionary<string, string>> filterFromDaughterData = daughterCatalog.SelectRecordsWhere(new List<string>(), tableName, parentIdColumn, pairKeys.Item1);
                    List<string> mainCatalogValues = ValuesManager.SelectDataFromColumn(filterFromMainData, uniqueValueColumnName);

                    foreach (Dictionary<string, string> row in filterFromDaughterData)
                    {
                        if (mainCatalogValues.Contains(row[uniqueValueColumnName]))
                        {
                            if (foreigns.Count > 0)
                            {
                                
                                ValuesManager.AddPairKeysToReserve(foreigns, idLikeColumnName, new Tuple<string, string>(row[idLikeColumnName], ValuesManager.ReturnValue(filterFromMainData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName)));
                            }
                        }
                        else if (!mainCatalogValues.Contains(row[uniqueValueColumnName]))
                        {
                            row[idLikeColumnName] = $"'{lastId + countOfImports + 1}'";

                            if (numerateColumn != null)
                            {
                                row[numerateColumn] = $"'{lastNumeric + countOfImports + 1}'";
                            }

                            if (foreigns.Count > 0)
                            {
                                ValuesManager.AddPairKeysToReserve(foreigns, idLikeColumnName, new Tuple<string, string>(ValuesManager.ReturnValue(filterFromDaughterData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName), row[idLikeColumnName]));
                            }

                            if (ValuesManager.ContainsInRewrite(tableName))
                            {
                                mainCatalog.InsertValue(tableName, ValuesManager.RepareColumnsValues(ValuesManager.RemoveUnnecessary(row, excludeColumns), tableName));
                            }
                            else
                            {
                                mainCatalog.InsertValue(tableName, ValuesManager.RemoveUnnecessary(row, excludeColumns));
                            }

                            countOfImports++;
                        }
                    }


                }





            }
            else if (parentIdColumn != null && uniqueValueColumnName == null)
            {
                List<Dictionary<string, string>> allFromDaughterData = daughterCatalog.SelectAllFrom(tableName);
                Dictionary<string, List<Tuple<string, string>>> tableReservedValues = ValuesManager.ReturnTableValuesReserveDict(tableName);

                foreach (Dictionary<string, string> row in allFromDaughterData)
                {
                    
                    foreach (Tuple<string, string> tuple in tableReservedValues[parentIdColumn])
                    {
                        if (row[parentIdColumn] == tuple.Item1)
                        {
                            row[parentIdColumn] = tuple.Item2;

                            /*                            if (foreigns.Count > 0)
                                                        {
                                                            ValuesManager.AddPairKeysToReserve(foreigns, idLikeColumnName, new Tuple<string, string>(tuple.Item1, tuple.Item2));
                                                        }*/

                            if (ValuesManager.ContainsInRewrite(tableName))
                            {
                                mainCatalog.InsertValue(tableName, ValuesManager.RepareColumnsValues(ValuesManager.RemoveUnnecessary(row, excludeColumns), tableName));
                            }
                            else
                            {
                                mainCatalog.InsertValue(tableName, ValuesManager.RemoveUnnecessary(row, excludeColumns));
                            }

                            //mainCatalog.InsertValue(tableName, row);
                            countOfImports++;
                            break;
                        }
                    }
                }
            }
            return countOfImports;

            /*int countImports = 0; // Переменная для подсчета импортированных записей

            // остановка на "tblINVENTORY". Значения для импорта ищутся по ISN_INVENTORY, а должны по ISN_FUND. Также данная таблица используется далее, как внешние значения. Переделать логику составления импорта данных. По типу проверить на внешние ключи, потом составить данные для импорта и проверить данные в резерве. После этого сделать импорт из зарезервированных, а после добавить в резервный словарь внешние ключи переданной таблицы. (Логика перерабатывается в блоке когда нет значений на фильтрацию уникальных)

            int usageCount = ValuesManager.AddNewTableToReserve(mainCatalog, tableName);


            // 1. У нее могут быть ключи на внешние 2. Могут отсутствовть, но может быть, что она в резерве.
            if (usageCount > 0)
            {

            }

            if (ValuesManager.ReturnReserve().ContainsKey(tableName))
            {
                // old --- new
                List<Tuple<string, string>> keysTuples = ValuesManager.ReturnReserve()[tableName][0][foreignIdColumn];

                List<Dictionary<string, string>> daughterRecordsFromTable = new List<Dictionary<string, string>>();

                List<string> oldKeys = new List<string>();

                foreach (Tuple<string, string>  tTuple in keysTuples)
                {
                    oldKeys.Add(tTuple.Item1);
                    // Если таблица была занесена в резерв, то найти значения только оттуда и сделать импорты в главную.
                    //daughterRecordsFromTable = daughterCatalog.SelectAllFrom(tableName, new Dictionary<string, List<string>>() { { "", "" } });

                }

                daughterRecordsFromTable = daughterCatalog.SelectAllFrom(tableName, new Dictionary<string, List<string>>() { { idLikeColumnName, oldKeys } });



                if (daughterRecordsFromTable.Count > 0)
                {
                    foreach (Dictionary<string, string> insertData in daughterRecordsFromTable)
                    {
                        Dictionary<string, string> prepareData = new Dictionary<string, string>(insertData);

                        prepareData.Remove("ID");

                        mainCatalog.InsertValue(tableName, prepareData);
                    }
                    countImports += daughterRecordsFromTable.Count;
                }
                return countImports;
            }
            else
            {
                // Если нет, то ищем по каталогу
                ValuesManager.AddNewTableToReserve(mainCatalog, tableName);
                // В данной функции собрать и сформировать все данные для импорта, чтобы передать на непосредственный импорт

                //List<string> mainListCheckData = mainCatalog.SelectListColumnsData(columnName, tableName);
                //List<string> daughterListCheckData = daughterCatalog.SelectListColumnsData(columnName, tableName);

*//*                if (idLikeColumnName != "")
                {
                    // Доюавдение в резерв используемых далее
                    ValuesManager.AddNewTableToReserve(mainCatalog, tableName);
                }*//*



                // 1. Список в котором словари со всеми значениями из главного каталога по переданной таблице List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
                List<Dictionary<string, string>> mainRecordsFromTable = mainCatalog.SelectAllFrom(tableName);

                // ----- проверка (не используется в программе)
                *//*            foreach (Dictionary<string, string> i in mainRecordsFromTable)
                            {
                                foreach (string j in i.Keys)
                                {
                                    MessageBox.Show(j + " --- " + i[j]);
                                }
                            }*//*
                // ---------


                // 2. Список в котором словари со всеми значениями из дочернего каталога по переданной таблице
                List<Dictionary<string, string>> daughterRecordsFromTable = daughterCatalog.SelectAllFrom(tableName);

                // 3. Обработчик, который принимает списки из  пунктов 1 и 2. Проходит по дочернему списку. Если по уникальному не нашел значения в главной, то в резервный словарь записывает нынешний ключ, как старый и новый формирует на основе последнего в главной + 1. После передает обработчику, который сформирует и добавит в очередь на импорт. Если нашел подобное значение. То в словарь записывает нынешний, как старый ключ. А новый, как он есть в главной. После чего переходит к следующей записи. На выходе возвращает готовый словарь записей, которые будут испортированы
                // ЧТОБЫ добавить вторичный ключ нынешней таблицы, нужно проверить idLikeColumnName. Если его передали, то мы добавляем значение в резервный словарь. Если такого нет. Значит данные из таблицы нигде не используются. А значит нужно только сформировать запись на импорт в гланый каталог
                List<Dictionary<string, string>> onImportData = ValuesManager.SortData(mainCatalog, mainRecordsFromTable, daughterRecordsFromTable, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);



                // 4. Импортируем сформированные записи из пункта 3.

                // Дальше идет обработчик, который берет все записи из главной и дочерней баз. И проверяет вхождения по уникальному ID, который передается при вызове обработчика



                // Список уникальных значений, по которым можно найти необходимые записи в дочерней БД
                *//*List<string> uniqueValues = ValuesManager.CheckUniqueValue(
                    mainCatalog.SelectListColumnsData(uniqueValueColumnName, tableName),
                    daughterCatalog.SelectListColumnsData(uniqueValueColumnName, tableName)
                );*//*

                // Список колонок для формирования запроса на добавление уникальной записи
                //List<string> forImportColumns = mainCatalog.SelectColumnsNames(tableName);



                *//*            if (tableName == "eqUsers")
                            {
                                forImportColumns.Remove("DisplayName");
                            }*//*

                if (onImportData.Count > 0)
                {
                    // Цикл импортирования уникальных значений
                    foreach (Dictionary<string, string> uniqueRowData in onImportData)
                    {
                        mainCatalog.InsertValue(tableName, uniqueRowData);

                        // Формирование списка, который содержит в себе набор данных для импорта одного значения
                        //List<string> forImportData = daughterCatalog.SelectRecordsWhere(forImportColumns, tableName, uniqueValueColumnName, filterValue);

                        // ---
                        // Словарь ключ строка, который содержит Список, который содержит Словарь ключ строка со значением пара 
                        // Если передано уникальное поле, то ищем по нему значения на транзакцию.
                        // Если поле не передано, то берется поле из словаря резервных записей и также от-туда берется значение, по которому найти записи
                        // Также в таком раскладе исппользуется другой запрос, нежели если бы было передано изначальное поле для поиска
                        // В случае не нахождения в словаре поля вернуть 0 импортированных значений.
                        //

                        // Где-то в данном блоке необходимо сделать запрос внешних ключей.
                        // После проверки, если такие имеются, то создать запись в reserveDict "таблица, в которой используется" -> "oldKey", "newKey". А в middle заменить его на новый, который равняется: (количество всех записей (запрос имеется в каталоге, а также используется перед входом в данный обработчик) в главной + 1) 
                        *//*Dictionary<string, string> middleRecordDict = new Dictionary<string, string>();

                        for (int i = 0; i < forImportColumns.Count; i++)
                        {
                            middleRecordDict[forImportColumns[i]] = forImportData[i];
                        }

                        middleRecordDict.Remove("ID");*//*
                        //middleRecordDict["unique_column_name"] = $"'{reserveDict[tableName][0][oldKey]}'";

                        // ---

                        //ValuesManager.AddUniqueValue(tableName, forImportColumns, uniqueValueColumnName, filterValue, mainCatalog, daughterCatalog);
                    }
                    countImports += onImportData.Count;
                }
                return countImports;
            }*/
        }
        // --------------------------------------

        /*static int ProcessTBLUsers(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            List<string> mainListUsersLogins = mainCatalog.SelectListColumnsData("Login", tableName);
            List<string> daughterListUsersLogins = daughterCatalog.SelectListColumnsData("Login", tableName);
            List<string> forImportColumns = new List<string>() { "[ID]", "[Login]", "[Department]", "[Deleted]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Email]", "[patronymic]", "[Position]", "[Phone]", "[Room_Number]", "[Description]", "[surname]", "[AccessGranted]", "[Supervisor]", "[FirstName]", "[BUILD_IN_ACCOUNT]", "[Pass]", "[Cookie]", "[UserTheme]" };
            List<string> uniqueValues = ValuesManager.CheckUniqueValue(mainListUsersLogins, daughterListUsersLogins);
            int countImports = 0;

            //1. Берем список логинов из главной и проверяем, что все дефолтные пользователи на месте. Добавляем, если не хватает
            countImports += ValuesManager.CheckDefaultUsers(mainListUsersLogins, countImports);

             //2. Берем список логинов из дочерней и сравниваем с главными. Если есть уникальный, то делаем запрос на получение данных этого пользователя и добавляем его в главную
             if (uniqueValues.Count > 0)
            {
                foreach (string value in uniqueValues)
                {
                    ValuesManager.AddUniqueValue(tableName, forImportColumns, "Login", value, mainCatalog, daughterCatalog);
                }
                countImports += uniqueValues.Count;
            }
            return countImports;
        }*/
    }

    public static class ValuesManager
    {
        static List<string> defaultUsers = new List<string>() { "sa", "anonymous", "admin", "reader", "arch", "tech" };
        //static Dictionary<string, List<Dictionary<string, string>>> reserveDict = new Dictionary<string, List<Dictionary<string, string>>>();

        // Словарь для дефолтных таблиц. Содержит ключи из дефолтных, которые нкжно будет перезаписать в процессе формирования импортируемой записи в обработчике линкованных таблиц
        static Dictionary<string, Dictionary<string, List<Tuple<string, string>>>> rewriteDict = new Dictionary<string, Dictionary<string, List<Tuple<string, string>>>>();



        // Наименование таблицы в которой есть внешние ключи на дефолтные таблицы - Список словарей, у которых Ключ это наименование колонки с внешним ключом - Которая содержит список кортежей (старый ключ, и ключ на который нужно обновить)
        // "tblINVENTORY": [
        //      "SECURY_LVL": [ ('10023', '10067'), ('10001', '10432') ],
        //      "REASON": [ ('10003', '10007'), ('10001', '10032') ]
        // ]
        static Dictionary<string, Dictionary<string, List<Tuple<string, string>>>> reserveDict = new Dictionary<string, Dictionary<string, List<Tuple<string, string>>>>();
        // "tblINVENTORY": [{"old_key": "new_key"}, {"old_key": "new_key"}, {"old_key": "new_key"}]
        // 1. Ищем в словаре по таблице. Если нету, то делаем поиск по уникальности. В итоге след пункт
        // 2. Берем

        public static Dictionary<string, Dictionary<string, List<Tuple<string, string>>>> ReturnReserve()
        {
            return reserveDict;
        }

        public static Dictionary<string, List<Tuple<string, string>>> ReturnTableValuesReserveDict(string tableName)
        {
            return reserveDict[tableName];
        }

        public static Dictionary<string, Dictionary<string, List<Tuple<string, string>>>> ReturnRewriteDict()
        {
            return rewriteDict;
        }

        public static Dictionary<string, List<Tuple<string, string>>> ReturnTableValuesRewriteDict(string tableName)
        {
            return rewriteDict[tableName];
        }

        public static bool ContainsInRewrite(string tableName)
        {
            return rewriteDict.ContainsKey(tableName);
        }

        public static int CheckDefaultUsers(List<string> usersLogins, int countImports)
        {
            foreach (string userLogin in defaultUsers)
            {
                if (usersLogins.Contains(userLogin))
                    continue;
                RestoreUser(userLogin); // В процессе
                countImports++;
            }

            return countImports;
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

        public static void AddTablesToRewriteDict(Dictionary<string, string> foreigns)
        {
            foreach (string inTable in foreigns.Keys)
            {
                if (!rewriteDict.ContainsKey(inTable))
                {
                    rewriteDict[inTable] = new Dictionary<string, List<Tuple<string, string>>>();
                }
                rewriteDict[inTable][foreigns[inTable]] = new List<Tuple<string, string>>();
            }
        }

        public static void AddPairKeysToRewriteDict(Dictionary<string, string> foreigns, string idLikeColumnName, Tuple<string, string> pairKeys)
        {
            foreach (string inTable in foreigns.Keys)
            {
                foreach(string foreignKey in rewriteDict[inTable].Keys)
                {
                    if (foreignKey == idLikeColumnName)
                    {
                        rewriteDict[inTable][idLikeColumnName].Add(pairKeys);
                    }
                    continue;
                }
            }
        }

        /// <summary>
        /// Добавляет в резервный словарь наименование таблицы и наименое столбца на которые используется ссылка переданной таблицы
        /// </summary>
        /// <returns>Количество найденых таблиц</returns>
        public static void AddNewTableToReserve(Dictionary<string, string> foreigns)
        {
            foreach (string inTable in foreigns.Keys)
            {
                if (!reserveDict.ContainsKey(inTable))
                {
                    reserveDict[inTable] = new Dictionary<string, List<Tuple<string, string>>>();
                }
                reserveDict[inTable][foreigns[inTable]] = new List<Tuple<string, string>>();
            }

            // Пример пришедших данных
            // "FUND" -> "ISN_FUND"
/*            if (inUsage.Count > 0)
            {
                foreach (string inTable in inUsage.Keys)
                {
                    // Если еще нет таблицы в словаре
                    if (!reserveDictNew.ContainsKey(inTable))
                    {
                        reserveDictNew[inTable] = new List<Dictionary<string, List<Tuple<string, string>>>>();
                    }
                    // Добавляем в список таблицы наименование колонки, которая содержит ключ
                    reserveDictNew[inTable].Add(new Dictionary<string, List<Tuple<string, string>>>() { { inUsage[inTable], new List<Tuple<string, string>>() } });
                }
            }
            return inUsage.Count;*/
        }

        /// <summary>
        /// Добавляет пару ключей к таблице в резервной копии
        /// </summary>
        public static void AddPairKeysToReserve(Dictionary<string, string> foreigns, string idLikeColumnName, Tuple<string, string> pairKeys)
        {
            foreach (string inTable in foreigns.Keys)
            {
                foreach (string foreignKey in reserveDict[inTable].Keys)
                {
                    if (foreignKey == idLikeColumnName)
                    {
                        reserveDict[inTable][idLikeColumnName].Add(pairKeys);
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
        /// Сортирует данные из главной таблицы и дочерней
        /// </summary>
        /// <returns>Записи готовые к импорту в главный каталог</returns>
        public static List<Dictionary<string, string>> SortData(DBCatalog mainCatalog, List<Dictionary<string, string>> mainRecordsFromTable, List<Dictionary<string, string>> daughterRecordsFromTable, string uniqueValueColumnName, string idLikeColumnName, string tableName, bool usedFurther)
        {
            List<Dictionary<string, string>> onImportData = new List<Dictionary<string, string>>();
            /*List<string> mainData = new List<string>();
            List<string> daughterData = new List<string>();
            List<string> uniqueRecords = new List<string>();
*//*            string ssssssss = ;
            MessageBox.Show(ssssssss);*//*
            long lastIdLikeInMain = Convert.ToInt64(mainCatalog.SelectLastRecord(idLikeColumnName, tableName, idLikeColumnName)[0].Replace("\'", ""));

            // Отбираем записи по уникальному переданному полю для дальнейшей сортировки
            foreach (Dictionary<string, string> rowDataInMain in mainRecordsFromTable)
            {
                mainData.Add(rowDataInMain[uniqueValueColumnName]);
            }


            foreach (Dictionary<string, string> rowDataInDaughter in daughterRecordsFromTable)
            {
                // Логика, если значение имеется. Мы добавляем в резерм пару ключей как они есть
                if (mainData.Contains(rowDataInDaughter[uniqueValueColumnName]) && usedFurther)
                {
                    AddPairKeysToReserve(mainCatalog, tableName, idLikeColumnName, new Tuple<string, string>(rowDataInDaughter[idLikeColumnName], ReturnValue(mainRecordsFromTable, uniqueValueColumnName, rowDataInDaughter[uniqueValueColumnName], idLikeColumnName)));
                }
                else
                {
                    // Логика, если значение не найдено. Мы добавляем в резерм пару ключей: старый ключ из дочерней - новый ключ сформированный на основе последнего ключа из главной
                    Dictionary<string, string> onAdd = new Dictionary<string, string>(rowDataInDaughter);

                    onAdd.Remove("ID");
                    onAdd[idLikeColumnName] = $"{lastIdLikeInMain + onImportData.Count + 1}";
                    onImportData.Add(onAdd);

                    if (usedFurther)
                    {
                        AddPairKeysToReserve(mainCatalog, tableName, idLikeColumnName, new Tuple<string, string>(rowDataInDaughter[idLikeColumnName], onAdd[idLikeColumnName]));
                    }
                }
            }*/
            return onImportData;
        }

        /// <summary>
        /// Ищет в словаре значение и возвращает по переданным параметрам
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

        public static void RestoreUser(string userLogin)
        {

        }

        public static void AddUniqueValue(string tableName, List<string> forImportColumns, string filterColumn, string filterValue, DBCatalog maincatalog, DBCatalog daughterCatalog)
        {
            /*if (reserveDict.ContainsKey(tableName))
            {
                foreach (string oldKey in reserveDict[tableName][0].Keys)
                {
                    Dictionary<string, string> middleRecordDict = new Dictionary<string, string>();
                    List<string> tableColumns = daughterCatalog.SelectColumnsNames(tableName);
                    List<string> tableValues = daughterCatalog.SelectRecordsWhere(tableColumns, tableName, "ISN_CITIZEN", oldKey);

                    for (int i = 0; i < tableColumns.Count; i++)
                    {
                        middleRecordDict[tableColumns[i]] = tableValues[i];
                    }

                    middleRecordDict.Remove("ID");
                    middleRecordDict["unique_column_name"] = $"'{reserveDict[tableName][0][oldKey]}'";

                    maincatalog.InsertValue(tableName, middleRecordDict);
                    reserveDict[tableName][0].Remove(oldKey);
                }
            }
            else
            {
                daughterCatalog.UpdateID(tableName, filterColumn, filterValue);
                maincatalog.InsertFromUniqueValue(tableName, forImportColumns, daughterCatalog.ReturnCatalog(), tableName, filterColumn, filterValue);

            }*/
        }

    }
}
