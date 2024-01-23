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
        // Словарь сборных (с внешними ключами) таблиц и функций для их обработки
        public static Dictionary<string, Func<DBCatalog, DBCatalog, string, int>> linksTablesFunctions = new Dictionary<string, Func<DBCatalog, DBCatalog, string, int>> {
            { "tblORGANIZ_RENAME", ProcessORGANIZ_RENAME },
            // { "tblARCHIVE", ProcessARCHIVE } // уникальная фунция запроса/обновления данных
            { "tblLOCATION", ProcessLOCATION },
            { "tblARCHIVE_STORAGE", ProcessARCHIVE_STORAGE },
            // { "tblARCHIVE_PASSPORT", ProcessARCHIVE_PASSPORT }, // Пока что скип
            // { "tblARCHIVE_STATS", ProcessARCHIVE_STATS }, // Пока что скип
            { "tblFUND", ProcessFUND },
            { "tblFUND_RENAME", ProcessFUND_RENAME },
            { "tblFUND_CHECK", ProcessFUND_CHECK },
            { "tblFUND_DOC_TYPE", ProcessFUND_DOC_TYPE },
            { "tblFUND_INCLUSION", ProcessFUND_INCLUSION },
            { "tblFUND_PAPER_CLS", ProcessFUND_PAPER_CLS },
            { "tblPUBLICATION_CL", ProcessPUBLICATION_CL },
            { "tblFUND_PUBLICATION", ProcessFUND_PUBLICATION },
            { "tblFUND_RECEIPT_REASON", ProcessFUND_RECEIPT_REASON },
            { "tblFUND_RECEIPT_SOURCE", ProcessFUND_RECEIPT_SOURCE },
            { "tblFUND_OAF", ProcessFUND_OAF },
            { "tblFUND_OAF_REASON", ProcessFUND_OAF_REASON },
            { "tblFUND_COLLECTION_REASONS", ProcessFUND_COLLECTION_REASONS },
            //{ "tblFUND_CREATOR", ProcessFUND_CREATOR }, хз пока как фильтровать
            { "tblUNDOCUMENTED_PERIOD", ProcessUNDOCUMENTED_PERIOD },
            { "tblDEPOSIT", ProcessDEPOSIT },
            { "tblDEPOSIT_DOC_TYPE", ProcessDEPOSIT_DOC_TYPE },
            { "tblACT", ProcessACT },
            { "tblINVENTORY", ProcessINVENTORY },
            { "tblINVENTORY_CHECK", ProcessINVENTORY_CHECK },
            { "tblINVENTORY_DOC_STORAGE", ProcessINVENTORY_DOC_STORAGE },
            { "tblINVENTORY_DOC_TYPE", ProcessINVENTORY_DOC_TYPE },
            { "tblINVENTORY_CLS_ATTR", ProcessINVENTORY_CLS_ATTR },
            { "tblINVENTORY_GROUPING_ATTRIBUTE", ProcessINVENTORY_GROUPING_ATTRIBUTE },
            { "tblINVENTORY_PAPER_CLS", ProcessINVENTORY_PAPER_CLS },
            { "tblINVENTORY_REQUIRED_WORK", ProcessINVENTORY_REQUIRED_WORK },
            { "tblINVENTORY_STRUCTURE", ProcessINVENTORY_STRUCTURE },
            { "tblDOCUMENT_STATS", ProcessDOCUMENT_STATS },
            { "tblREF_ACT", ProcessREF_ACT },
            { "tblREF_CLS", ProcessREF_CLS },
            { "tblREF_FEATURE", ProcessREF_FEATURE },
            { "tblREF_LANGUAGE", ProcessREF_LANGUAGE },
            { "tblREF_LOCATION", ProcessREF_LOCATION },
            { "tblREF_QUESTION", ProcessREF_QUESTION },
            { "tblUNIT", ProcessUNIT },
            { "tblUNIT_ELECTRONIC", ProcessUNIT_ELECTRONIC },
            { "tblUNIT_FOTO", ProcessUNIT_FOTO },
            { "tblUNIT_FOTO_EX", ProcessUNIT_FOTO_EX },
            { "tblUNIT_MICROFORM", ProcessUNIT_MICROFORM },
            { "tblUNIT_MOVIE", ProcessUNIT_MOVIE },
            { "tblUNIT_MOVIE_EX", ProcessUNIT_MOVIE_EX },
            { "tblUNIT_NTD", ProcessUNIT_NTD },
            { "tblUNIT_PHONO", ProcessUNIT_PHONO },
            { "tblUNIT_REQUIRED_WORK", ProcessUNIT_REQUIRED_WORK },
            { "tblUNIT_STATE", ProcessUNIT_STATE },
            { "tblUNIT_VIDEO", ProcessUNIT_VIDEO },
            { "tblUNIT_VIDEO_EX", ProcessUNIT_VIDEO_EX },
            { "tblUNIT_WORK", ProcessUNIT_WORK },
            { "tblDOCUMENT", ProcessDOCUMENT },
        };

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
                new Tuple<string, string, List<string>, string, string, string>(null, null, null, null, "ISN_FUND", null) },

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
                new Tuple<string, string, List<string>, string, string, string>(null, "ISN_INVENTORY_CLS", null, "ISN_HIGH_INVENTORY_CLS", "ISN_INVENTORY", null) },

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


// --- Process Functions for LinksTables ---
        static int ProcessORGANIZ_RENAME(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            bool usedFurther = true;
            string idLikeColumnName = "ISN_ORGANIZ_RENAME";
            string uniqueValueColumnName = "ISN_ORGANIZ_RENAME";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_ORGANIZ_RENAME]", "[ISN_ORGANIZ]", "[CODE]", "[CREATE_DATE]", "[CREATE_DATE_INEXACT]", "[DELETE_DATE]", "[DELETE_DATE_INEXACT]", "[NAME]", "[NOTE]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessARCHIVE(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            // Переделать на проверку замены именования. Таблица содержит в себе всегда один архив

            bool usedFurther = true;
            string idLikeColumnName = "";
            string uniqueValueColumnName = "";
            //List<string> forImportColumns = new List<string>() { };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessLOCATION(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_LOCATION";
            string uniqueValueColumnName = "ISN_LOCATION";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_LOCATION]", "[ISN_HIGH_LOCATION]", "[ISN_ARCHIVE]", "[CODE]", "[NAME]", "[NOTE]", "[FOREST_ELEM]", "[PROTECTED]", "[WEIGHT]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessARCHIVE_STORAGE(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_LOCATION";
            string uniqueValueColumnName = "ISN_LOCATION";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_LOCATION]", "[ROOM_CNT]", "[SPECIAL_ROOM_CNT]", "[ADAPTED_ROOM_CNT]", "[TOTAL_SPACE]", "[SPACE_WITH_ALARM]", "[SPACE_WITHOUT_ALARM]", "[SHELF_LENGTH]", "[METAL_SHELF_LENGTH]", "[FREE_SHELF_LENGTH]", "[STORAGE]", "[FLOOR]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessFUND(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_FUND";
            string uniqueValueColumnName = "FUND_NAME_SHORT";
            /* List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_FUND]", "[ISN_ARCHIVE]", "[FUND_NUM_1]", "[FUND_NUM_2]", "[FUND_NUM_3]", "[ISN_DOC_TYPE]", "[FUND_CATEGORY]", "[ISN_PERIOD]", "[FUND_KIND]", "[FUND_NAME_SHORT]", "[FUND_NAME_FULL]", "[INVENTORY_COUNT]", "[AUTO_INVENTORY_COUNT]", "[DOC_START_YEAR]", "[DOC_START_YEAR_INEXACT]", "[DOC_END_YEAR]", "[DOC_END_YEAR_INEXACT]", "[DOC_RECEIPT_YEAR]", "[LAST_CHECKED_YEAR]", "[LAST_DOC_CHECK_YEAR]", "[IS_IN_SEARCH]", "[IS_LOST]", "[ANNOTATE]", "[PROPERTY]", "[PRESENCE_FLAG]", "[ABSENCE_REASON]", "[MOVEMENT_NOTE]", "[HAS_MUSEUM_ITEMS]", "[TREASURE_UNITS_COUNT]", "[HAS_UNDOCUMENTED_PERIODS]", "[HAS_INCLUSIONS]", "[WAS_RENAMED]", "[WEIGHT]", "[KEEP_PERIOD]", "[ISN_SECURLEVEL]", "[SECURITY_CHAR]", "[SECURITY_REASON]", "[ISN_OAF]", "[OAF_NOTE]", "[CARD_COUNT]", "[ARCHIVE_DB_COUNT]", "[FUND_DB_COUNT]", "[INNER_DB_COUNT]", "[LIST_COUNT]", "[PERSONAL_UNDESCRIBED_DOC_COUNT]", "[HAS_ELECTRONIC_DOCS]", "[HAS_TRADITIONAL_DOCS]", "[CARRIER_TYPE]", "[UNDESCRIBED_DOC_COUNT]",
 "[UNDECSRIBED_PAGE_COUNT]", "[JOIN_REASON]", "[ADDITIONAL_NSA]", "[KEYWORDS]", "[FUND_HISTORY]", "[NOTE]", "[INVENTORY_STATE]", "[ISN_SECURITY_REASON]", "[ForbidRecalc]" };
    */
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessFUND_RENAME(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_FUND_RENAME";
            string uniqueValueColumnName = "ISN_FUND_RENAME";

            string foreignIdColumn = "ISN_FUND";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_FUND_RENAME]", "[ISN_FUND]", "[CREATE_DATE]", "[CREATE_DATE_INEXACT]", "[DELETE_DATE]", "[DELETE_DATE_INEXACT]", "[FUND_NUM_1]", "[FUND_NUM_2]", "[FUND_NUM_3]", "[FUND_NAME_SHORT]", "[FUND_NAME_FULL]", "[NOTE]", "[NAME_SAVED]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther, foreignIdColumn);
        }

        static int ProcessFUND_CHECK(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_FUND";
            string uniqueValueColumnName = "ISN_FUND";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_FUND]", "[CARDBOARDED]", "[UNITS_NEED_CARDBOARDED]", "[UNITS_DBR]", "[UNITS_NEED_RESTORATION]", "[UNITS_NEED_BINDING]", "[UNITS_NEED_DISINFECTION]", "[UNITS_NEED_DISINSECTION]", "[FADING_PAGES]", "[UNITS_NEED_ENCIPHERING]", "[UNITS_NEED_COVER_CHANGE]", "[UNITS_INFLAMMABLE]", "[UNITS_NEED_KPO]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessFUND_DOC_TYPE(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = false;
            string idLikeColumnName = "ISN_FUND";
            string uniqueValueColumnName = "";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_FUND]", "[ISN_DOC_TYPE]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessFUND_INCLUSION(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_INCLUSION";
            string uniqueValueColumnName = "ISN_INCLUSION";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_INCLUSION]", "[ISN_FUND]", "[ISN_CITIZEN]", "[ISN_ORGANIZ]", "[DOC_TYPES]", "[START_YEAR]", "[START_YEAR_INEXACT]", "[END_YEAR]", "[END_YEAR_INEXACT]", "[NOTE]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessFUND_PAPER_CLS(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_FUND";
            string uniqueValueColumnName = "ISN_FUND";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_FUND]", "[ISN_PAPER_CLS]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessPUBLICATION_CL(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_PUBLICATION";
            string uniqueValueColumnName = "ISN_PUBLICATION";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_PUBLICATION]", "[ISN_PUBLICATION_TYPE]", "[PUBLICATION_NUM]", "[AUTHORS]", "[PUBLICATION_NAME]", "[PUBLICATION_PLACE]", "[PUBLICATION_YEAR]", "[SHEET_COUNT]", "[PUBLISHER]", "[PROTECTED]", "[NOTE]", "[WEIGHT]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessFUND_PUBLICATION(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_FUND";
            string uniqueValueColumnName = "ISN_FUND";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_FUND]", "[ISN_PUBLICATION]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessFUND_RECEIPT_REASON(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_FUND";
            string uniqueValueColumnName = "ISN_FUND";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_FUND]", "[ISN_RECEIPT_REASON]", "[ORDER_NUM]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessFUND_RECEIPT_SOURCE(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_FUND";
            string uniqueValueColumnName = "ISN_FUND";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_FUND]", "[ISN_RECEIPT_SOURCE]", "[ORDER_NUM]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessFUND_OAF(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_OAF";
            string uniqueValueColumnName = "ISN_OAF";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[Deleted]", "[ISN_OAF]", "[ISN_FUND]", "[ISN_CHILD_FUND]", "[FUND_NUM_1]", "[FUND_NUM_2]", "[FUND_NUM_3]", "[FUND_CATEGORY]", "[FUND_NAME_SHORT]", "[FUND_NAME_FULL]", "[DOC_START_YEAR]", "[DOC_END_YEAR]", "[NOTE]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessFUND_OAF_REASON(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_FUND";
            string uniqueValueColumnName = "ISN_FUND";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_FUND]", "[ISN_OAF_REASON]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessFUND_COLLECTION_REASONS(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_FUND";
            string uniqueValueColumnName = "ISN_FUND";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_FUND]", "[ISN_COLLECTION_REASON]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }


        static int ProcessFUND_CREATOR(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = false;
            string idLikeColumnName = "ISN_FUND_CREATOR";
            string uniqueValueColumnName = "ISN_FUND_CREATOR";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_FUND_CREATOR]", "[ISN_CITIZEN]", "[ISN_ORGANIZ]", "[ISN_FUND]", "[KIND]", "[IS_PRIMARY]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }
        
        static int ProcessUNDOCUMENTED_PERIOD(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_PERIOD";
            string uniqueValueColumnName = "ISN_PERIOD";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_PERIOD]", "[ISN_FUND]", "[PERIOD_START_YEAR]", "[PERIOD_END_YEAR]", "[ISN_ABSENCE_REASON]", "[INFO_PLACE]", "[NOTE]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessDEPOSIT(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_DEPOSIT";
            string uniqueValueColumnName = "ISN_DEPOSIT";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_DEPOSIT]", "[ISN_FUND]", "[DEPOSIT_NUM]", "[DEPOSIT_NAME]", "[CONSISTS]", "[UNIT_COUNT]", "[DOC_COUNT]", "[PERSONAL_DOC_COUNT]", "[PAGE_COUNT]", "[NOTE]", "[PAPER_DOC_COUNT]", "[AV_TRAD_COUNT]", "[ELECTRONIC_DOC_COUNT]", "[PROCESSED]", "[PROCESSED_DOC]", "[PROCESSED_PERSONAL_DOC]", "[PROCESSED_PAGE]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessDEPOSIT_DOC_TYPE(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_DEPOSIT_DOC_TYPE";
            string uniqueValueColumnName = "ISN_DEPOSIT_DOC_TYPE";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_DEPOSIT_DOC_TYPE]", "[ISN_DEPOSIT]", "[ISN_DOC_TYPE]", "[UNIT_COUNT]", "[PROCESSED_COUNT]", "[CARRIER_TYPE]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessACT(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {


            bool usedFurther = true;
            string idLikeColumnName = "ISN_ACT";
            string uniqueValueColumnName = "ISN_ACT";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_ACT]", "[ISN_ACT_TYPE]", "[ISN_COMMISSION]", "[ISN_ESTIMATE_REASON]", "[ISN_FUND]", "[ACT_NUM]", "[ACT_DATE]", "[ACT_NAME]", "[MOVEMENT_FLAG]", "[ACT_OBJ]", "[ACT_PERSONS]", "[NOTE]", "[ACT_WORK]", "[UNIT_COUNT]", "[PAGE_NUMBERS]", "[DOC_DATES]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessINVENTORY(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_INVENTORY";
            string uniqueValueColumnName = "ISN_INVENTORY";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_INVENTORY]", "[ISN_FUND]", "[ISN_SECURLEVEL]", "[SECURITY_CHAR]", "[SECURITY_REASON]", "[ISN_INVENTORY_TYPE]", "[ISN_RECEIPT_SOURCE]", "[ISN_RECEIPT_REASON]", "[ISN_DOC_KIND]", "[ISN_INVENTORY_STORAGE]", "[ISN_REPRODUCTION_METHOD]", "[ISN_REQUIRED_WORK]", "[INVENTORY_NUM_1]", "[INVENTORY_NUM_2]", "[INVENTORY_NUM_3]", "[INVENTORY_NAME]", "[INVENTORY_KIND]", "[INVENTORY_KEEP_PERIOD]", "[COPY_COUNT]", "[DOC_START_YEAR]", "[DOC_START_YEAR_INEXACT]", "[DOC_END_YEAR]", "[DOC_END_YEAR_INEXACT]", "[CATALOGUING]", "[PRESENCE_FLAG]", "[ABSENCE_REASON]", "[MOVEMENT_NOTE]", "[UNITS_WITH_TREASURES_COUNT]", "[MUSEUM_UNITS_COUNT]", "[ANNOTATE]", "[WEIGHT]", "[INVENTORY_DOC_WORK]", "[NOTE]", "[VOL_NUM]", "[HAS_TRADITIONAL_DOCS]", "[HAS_ELECTRONIC_DOCS]", "[CARRIER_TYPE]", "[ADDITIONAL_NSA]", "[KEYWORDS]", "[ISN_SECURITY_REASON]", "[ForbidRecalc]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessINVENTORY_CHECK(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_INVENTORY";
            string uniqueValueColumnName = "ISN_INVENTORY";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_INVENTORY]", "[CARDBOARDED]", "[UNITS_NEED_CARDBOARDED]", "[UNITS_DBR]", "[UNITS_NEED_RESTORATION]", "[UNITS_NEED_BINDING]", "[UNITS_NEED_DISINFECTION]", "[UNITS_NEED_DISINSECTION]", "[FADING_PAGES]", "[UNITS_NEED_ENCIPHERING]", "[UNITS_NEED_COVER_CHANGE]", "[UNITS_INFLAMMABLE]", "[UNITS_NEED_KPO]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessINVENTORY_DOC_STORAGE(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_INVENTORY";
            string uniqueValueColumnName = "ISN_INVENTORY";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_INVENTORY]", "[ISN_STORAGE_MEDIUM]", "[ORDER_NUM]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessINVENTORY_DOC_TYPE(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_INVENTORY";
            string uniqueValueColumnName = "ISN_INVENTORY";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_INVENTORY]", "[ISN_DOC_TYPE]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessINVENTORY_CLS_ATTR(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_INVENTORY";
            string uniqueValueColumnName = "ISN_INVENTORY";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_INVENTORY]", "[ISN_CLS]", "[ORDER_NUM]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessINVENTORY_GROUPING_ATTRIBUTE(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_INVENTORY";
            string uniqueValueColumnName = "ISN_INVENTORY";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_INVENTORY]", "[ISN_GROUPING_ATTRIBUTE]", "[ORDER_NUM]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessINVENTORY_PAPER_CLS(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_INVENTORY";
            string uniqueValueColumnName = "ISN_INVENTORY";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_INVENTORY]", "[ISN_PAPER_CLS_INV]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessINVENTORY_REQUIRED_WORK(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_INVENTORY";
            string uniqueValueColumnName = "ISN_INVENTORY";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_INVENTORY]", "[ISN_REQUIRED_WORK]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessINVENTORY_STRUCTURE(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_INVENTORY_CLS";
            string uniqueValueColumnName = "ISN_INVENTORY_CLS";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[StatusID]", "[Deleted]", "[ISN_INVENTORY_CLS]", "[ISN_HIGH_INVENTORY_CLS]", "[ISN_ARCHIVE]", "[ISN_FUND]", "[ISN_INVENTORY]", "[CODE]", "[NAME]", "[NOTE]", "[FOREST_ELEM]", "[PROTECTED]", "[WEIGHT]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessDOCUMENT_STATS(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_DOCUMENT_STATS";
            string uniqueValueColumnName = "ISN_DOCUMENT_STATS";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_DOCUMENT_STATS]", "[ISN_FUND]", "[ISN_DOC_TYPE]", "[ISN_INVENTORY]", "[CARRIER_TYPE]", "[UNIT_COUNT]", "[UNIT_INVENTORY]", "[UNIT_REGISTERED]", "[REG_UNIT]", "[REG_UNIT_INVENTORY]", "[REG_UNIT_REGISTERED]", "[UNIT_UNDESCRIBED]", "[REG_UNIT_UNDESCRIBED]", "[REG_UNIT_OC]", "[UNIT_OC_COUNT]", "[REG_UNIT_HAS_SF]", "[UNIT_HAS_SF]", "[REG_UNIT_HAS_FP]", "[UNIT_HAS_FP]", "[REG_UNITS_NOT_FOUND]", "[UNITS_NOT_FOUND]", "[SECRET_REG_UNITS]", "[SECRET_UNITS]", "[REG_UNITS_SEARCH]", "[UNITS_SEARCH]", "[REG_UNITS_UNIQUE]", "[UNITS_UNIQUE]", "[REG_UNITS_CTALOGUE]", "[UNITS_CATALOGUED]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessREF_ACT(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_REF_ACT";
            string uniqueValueColumnName = "ISN_REF_ACT";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_REF_ACT]", "[ISN_ACT]", "[ISN_OBJ]", "[KIND]", "[UNIT_COUNT]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessREF_CLS(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_REF_CLS";
            string uniqueValueColumnName = "ISN_REF_CLS";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_REF_CLS]", "[ISN_CLS]", "[ISN_CLSID]", "[ISN_TREE]", "[ISN_OBJ]", "[ORDER_NUM]", "[KIND]", "[NOTE]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessREF_FEATURE(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_REF_FEATURE";
            string uniqueValueColumnName = "ISN_REF_FEATURE";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_REF_FEATURE]", "[ISN_FEATURE]", "[ISN_OBJ]", "[KIND]", "[ORDER_NUM]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessREF_LANGUAGE(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_REF_LANGUAGE";
            string uniqueValueColumnName = "ISN_REF_LANGUAGE";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_REF_LANGUAGE]", "[ISN_LANGUAGE]", "[ISN_OBJ]", "[KIND]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessREF_LOCATION(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_REF_LOCATION";
            string uniqueValueColumnName = "ISN_REF_LOCATION";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_REF_LOCATION]", "[ISN_LOCATION]", "[ISN_OBJ]", "[UNIT_NUM_FROM]", "[UNIT_NUM_TO]", "[KIND]", "[NOTE]", "[UNIT_NUM_OLD_FROM]", "[UNIT_NUM_OLD_TO]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessREF_QUESTION(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_REF_QUESTION";
            string uniqueValueColumnName = "ISN_REF_QUESTION";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_REF_QUESTION]", "[ISN_QUESTION]", "[ISN_OBJ]", "[KIND]", "[Pages]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessUNIT(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_UNIT";
            string uniqueValueColumnName = "ISN_UNIT";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_UNIT]", "[ISN_HIGH_UNIT]", "[ISN_INVENTORY]", "[ISN_DOC_TYPE]", "[ISN_LOCATION]", "[ISN_SECURLEVEL]", "[SECURITY_CHAR]", "[SECURITY_REASON]", "[ISN_INVENTORY_CLS]", "[ISN_STORAGE_MEDIUM]", "[ISN_DOC_KIND]", "[UNIT_KIND]", "[UNIT_NUM_1]", "[UNIT_NUM_2]", "[VOL_NUM]", "[NAME]", "[ANNOTATE]", "[DELO_INDEX]", "[PRODUCTION_NUM]", "[UNIT_CATEGORY]", "[NOTE]", "[IS_IN_SEARCH]", "[IS_LOST]", "[HAS_SF]", "[HAS_FP]", "[HAS_DEFECTS]", "[ARCHIVE_CODE]", "[CATALOGUED]", "[WEIGHT]", "[UNIT_CNT]", "[START_YEAR]", "[START_YEAR_INEXACT]", "[END_YEAR]", "[END_YEAR_INEXACT]", "[MEDIUM_TYPE]", "[BACKUP_COPY_CNT]", "[HAS_TREASURES]", "[IS_MUSEUM_ITEM]", "[PAGE_COUNT]", "[CARDBOARDED]", "[ADDITIONAL_CLS]", "[ALL_DATE]", "[ISN_SECURITY_REASON]", "[UNIT_NUM_TXT]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessUNIT_ELECTRONIC(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_UNIT";
            string uniqueValueColumnName = "ISN_UNIT";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_UNIT]", "[DATA_FORMAT]", "[SIZE]", "[CARRIER_DESC]", "[ACCOMP_DOC]", "[UNIT_CNT]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessUNIT_FOTO(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_UNIT";
            string uniqueValueColumnName = "ISN_UNIT";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_UNIT]", "[ISN_DOC_KIND]", "[PLACE]", "[AUTHOR]", "[BASE_KIND]", "[COLOR]", "[SIZE]", "[STORAGE_INFO]", "[NEGATIVE_COUNT]", "[POSITIVE_COUNT]", "[PRINT_COUNT]", "[DUP_NEGATIVE_COUNT]", "[SLIDE_COUNT]", "[BACKUP_COUNT]", "[SHOT_COUNT]", "[ORIGINAL]", "[ACCOMP_DOC]", "[FOTO_DATE]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessUNIT_FOTO_EX(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_UNIT";
            string uniqueValueColumnName = "ISN_UNIT";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_UNIT]", "[CODE]", "[RUBRIC]", "[SOURCE]", "[START_YEAR]", "[END_YEAR]", "[COUNT]", "[COMPILER]", "[FOTO_DATE]", "[PUBLICATION]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessUNIT_MICROFORM(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_UNIT";
            string uniqueValueColumnName = "ISN_UNIT";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_UNIT]", "[FRAME_COUNT]", "[BACKUP_COUNT]", "[CARRIER_DESC]", "[ACCOMP_DOC]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessUNIT_MOVIE(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_UNIT";
            string uniqueValueColumnName = "ISN_UNIT";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_UNIT]", "[ISN_DOC_KIND]", "[UNIT_TYPE]", "[BASE]", "[FILM_TYPE]", "[FILM_FORMAT]", "[FOOTAGE]", "[START_DATE]", "[END_DATE]", "[PART_COUNT]", "[PLAYBACK_TIME]", "[SOUND]", "[FORMAT]", "[COLOR]", "[AUTHOR]", "[SHOOTING_PLACE]", "[PRODUCTION_PLACE]", "[NEGATIVE_COUNT]", "[NEGATIVE_LENGTH]", "[DUP_NEGATIVE_COUNT]", "[DUP_NEGATIVE_LENGTH]", "[PHONO_NEGATIVE_COUNT]", "[PHONO_NEGATIVE_LENGTH]", "[PHONO_MAG_COUNT]", "[PHONO_MAG_LENGTH]", "[PHONO_COMBINED_COUNT]", "[PHONO_COMBINED_LENGTH]", "[INTERPOSITIVE_COUNT]", "[INTERPOSITIVE_LENGTH]", "[POSITIVE_COUNT]", "[POSITIVE_LENGTH]", "[DUP_BACKUP_COUNT]", "[DUP_BACKUP_LENGTH]", "[INTERPOSITIVE_BACKUP_COUNT]", "[INTERPOSITIVE_BACKUP_LENGTH]", "[CINEX_COUNT]", "[COLOR_PASS_COUNT]", "[CARRIER_DESC]", "[ACCOMP_DOC]", "[UNIT_CNT]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }
        static int ProcessUNIT_MOVIE_EX(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_UNIT";
            string uniqueValueColumnName = "ISN_UNIT";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_UNIT]", "[STORY_NUM]", "[STUDIO]", "[DIRECTOR]", "[OPERATOR]", "[CREATOR]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessUNIT_NTD(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_UNIT";
            string uniqueValueColumnName = "ISN_UNIT";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_UNIT]", "[ISN_NTD_KIND]", "[COMPLEX_NUM_1]", "[COMPLEX_NUM_2]", "[DEV_STAGE]", "[AUTHOR]", "[ORGANIZ]", "[REQUEST_DATE]", "[REQUEST_NUM]", "[PATENT_NUM]", "[COPYRIGHT_NUM]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessUNIT_PHONO(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_UNIT";
            string uniqueValueColumnName = "ISN_UNIT";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_UNIT]", "[ISN_DOC_KIND]", "[BASE]", "[REC_SPEED]", "[REC_DATE]", "[PHONO_TYPE]", "[LENGTH]", "[REC_PLACE]", "[PLAYBACK_TIME]", "[PRODUCTION_PLACE]", "[AUTHOR]", "[PERFORMER]", "[ORIGINAL_COUNT]", "[DISK_COUNT]", "[NEGATIVE_COUNT]", "[COPY_COUNT]", "[BACKUP_COUNT]", "[NEGATIVE_LENGTH]", "[COPY_LENGTH]", "[BACKUP_LENGTH]", "[UNIT_CNT]", "[CARRIER_DESC]", "[ACCOMP_DOC]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessUNIT_REQUIRED_WORK(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_UNIT_REQUIRED_WORK";
            string uniqueValueColumnName = "ISN_UNIT_REQUIRED_WORK";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_UNIT_REQUIRED_WORK]", "[ISN_WORK]", "[ISN_UNIT]", "[ISN_REF_ACT]", "[NOTE]", "[CHECK_DATE]", "[IS_ACTUAL]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessUNIT_STATE(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {

            bool usedFurther = true;
            string idLikeColumnName = "ISN_UNIT_STATE";
            string uniqueValueColumnName = "ISN_UNIT_STATE";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_UNIT_STATE]", "[ISN_UNIT]", "[ISN_STATE]", "[ISN_REF_ACT]", "[PAGE_NUMS]", "[PAGE_COUNT]", "[STATE_DATE]", "[NOTE]", "[IS_ACTUAL]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessUNIT_VIDEO(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            bool usedFurther = true;
            string idLikeColumnName = "ISN_UNIT";
            string uniqueValueColumnName = "ISN_UNIT";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_UNIT]", "[ISN_DOC_KIND]", "[REC_DATE]", "[REC_TYPE]", "[REC_FORMAT]", "[BASE]", "[ORIGINAL]", "[START_DATE]", "[END_DATE]", "[PART_COUNT]", "[PLAYBACK_TIME]", "[SOUND]", "[COLOR]", "[AUTHOR]", "[SHOOTING_PLACE]", "[PRODUCTION_PLACE]", "[ORIGINAL_COUNT]", "[COPY_COUNT]", "[BACKUP_COUNT]", "[UNIT_CNT]", "[CARRIER_DESC]", "[ACCOMP_DOC]", "[PERFORMER]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessUNIT_VIDEO_EX(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            bool usedFurther = true;
            string idLikeColumnName = "ISN_UNIT";
            string uniqueValueColumnName = "ISN_UNIT";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_UNIT]", "[STORY_NUM]", "[STUDIO]", "[DIRECTOR]", "[OPERATOR]", "[CREATOR]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessUNIT_WORK(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            bool usedFurther = true;
            string idLikeColumnName = "ISN_UNIT_WORK";
            string uniqueValueColumnName = "ISN_UNIT_WORK";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_UNIT_WORK]", "[ISN_UNIT]", "[ISN_WORK]", "[WORK_DATE]", "[NOTE]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }

        static int ProcessDOCUMENT(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            bool usedFurther = true;
            string idLikeColumnName = "ISN_DOCUM";
            string uniqueValueColumnName = "ISN_DOCUM";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_DOCUM]", "[ISN_REPRODUCTION_METHOD]", "[ISN_SECURLEVEL]", "[SECURITY_CHAR]", "[ISN_UNIT]", "[ISN_DOC_KIND]", "[DOC_NUM]", "[WEIGHT]", "[PAGE_FROM]", "[PAGE_TO]", "[PAGE_COUNT]", "[NAME]", "[ANNOTATE]", "[DOCUM_DATE]", "[INEXACT_DOCUM_DATE]", "[EVENT_DATE]", "[INEXACT_EVENT_DATE]", "[EVENT_PLACE]", "[ENCLOSURES]", "[NOTE]", "[IS_ORIGINAL]", "[ADDITIONAL_CLS]", "[KEYWORDS]", "[AUTHORS]" };
            return ProcessLinksTable(mainCatalog, daughterCatalog, uniqueValueColumnName, idLikeColumnName, tableName, usedFurther);
        }
        // -------------------------------------


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
                            MessageBox.Show((row[idLikeColumnName] + "   new: " + ValuesManager.ReturnValue(allFromMainData, uniqueValueColumnName, row[uniqueValueColumnName], idLikeColumnName)));
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
                    MessageBox.Show("old:" + pairKeys.Item1 + "  new:" + pairKeys.Item2);
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

                            mainCatalog.InsertValue(tableName, row);
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
