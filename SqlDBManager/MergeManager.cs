﻿using NotesNamespace;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
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
        // Словарь дефолтных (без внешних ключей) таблиц и функций для их обработки
        public static Dictionary<string, Func<DBCatalog, DBCatalog, string, int>> defaultTablesFunctions = new Dictionary<string, Func<DBCatalog, DBCatalog, string, int>> {
            { "eqUsers", ProcessTBLUsers },
            { "tblACT_TYPE_CL", ProcessActTypeCL },
            { "tblAuthorizedDep", ProcessAuthorizedDep },
            { "tblCLS", ProcessCLS },
            { "tblDataExport", ProcessDataExport },
            { "tblDECL_COMMISSION_CL", ProcessDECL_COMMISSION_CL },
            //{ "tblConstantsSpec", ProcessConstantsSpec },
            { "tblGROUPING_ATTRIBUTE_CL", ProcessGROUPING_ATTRIBUTE_CL },
            { "tblINV_REQUIRED_WORK_CL", ProcessINV_REQUIRED_WORK_CL },
            { "tblLANGUAGE_CL", ProcessLANGUAGE_CL },
            { "tblFEATURE", ProcessFEATURE },
            { "tblCITIZEN_CL", ProcessCITIZEN_CL },
            { "tblORGANIZ_CL", ProcessORGANIZ_CL },
            { "tblPAPER_CLS", ProcessPAPER_CLS },
            { "tblPAPER_CLS_INV", ProcessPAPER_CLS_INV },
            { "tblPUBLICATION_TYPE_CL", ProcessPUBLICATION_TYPE_CL },
            { "tblQUESTION", ProcessQUESTION },
            { "tblRECEIPT_REASON_CL", ProcessRECEIPT_REASON_CL },
            { "tblRECEIPT_SOURCE_CL", ProcessRECEIPT_SOURCE_CL },
            { "tblREF_FILE", ProcessREF_FILE },
            { "tblREPRODUCTION_METHOD_CL", ProcessREPRODUCTION_METHOD_CL },
            // { "tblService", ProcessService },
            { "tblSTATE_CL", ProcessSTATE_CL },
            { "tblSTORAGE_MEDIUM_CL", ProcessSTORAGE_MEDIUM_CL },
            { "tblSUBJECT_CL", ProcessSUBJECT_CL },  // Может быть, что ISN_HIGH быть больше, чем основной ISN_SUBJECT (добавить новый обработчик отдельный. Или выбор вызова функции). В данных случаях, необходимо отложить импорт записи, пока не будет импортирована запись с указанным ID. Или найти по данному ID и сразу испортировать
            { "tblTREE_SUPPORT", ProcessTREE_SUPPORT },
            { "tblWORK_CL", ProcessWORK_CL },
            { "rptFUND_PAPER", ProcessFUND_PAPER },
            { "rptFUND_UNIT_REG_STATS", ProcessFUND_UNIT_REG_STATS },
        };

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
            { "tblFUND_CREATOR", ProcessFUND_CREATOR },
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

        public static void ClearLogs(DBCatalog mainCatalog, BackgroundWorker worker)
        {          
            foreach (string logTable in mainCatalog.SelectLogTables())
            {
                worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, $"Очистка {logTable}: {mainCatalog.ClearTable(logTable)} записей удалено.");
            }
        }

        public static void ProcessDefaultTables(DBCatalog mainCatalog, DBCatalog daughterCatalog, BackgroundWorker worker)
        {
            foreach (string defaultTable in mainCatalog.SelectDefaultSkipTables())
            {
                worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, $"Обработка {defaultTable}: default table");
            }

            CheckDefaultTables(mainCatalog, daughterCatalog, worker);
        }

        static void CheckDefaultTables(DBCatalog mainCatalog, DBCatalog daughterCatalog, BackgroundWorker worker)
        {
            foreach (string tableName in mainCatalog.SelectDefaultProcessingTables())
            {
                int mainCount = mainCatalog.SelectCountRowsTable(tableName);
                int daughterCount = daughterCatalog.SelectCountRowsTable(tableName);

                if (defaultTablesFunctions.ContainsKey(tableName))
                {
                    worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, $"Обработка {tableName}: Импортировано значений {defaultTablesFunctions[tableName](mainCatalog, daughterCatalog, tableName)}");
                }

                else if (mainCount != daughterCount)
                {
                    worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, $"{tableName}: {mainCount} <- M -- D -> {daughterCount}");
                }
                else
                {
                    worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, $"Обработка {tableName}: Equal");
                }
            }
        }

        public static void ProcessLinksTables(DBCatalog mainCatalog, DBCatalog daughterCatalog, BackgroundWorker worker)
        {
            foreach (string tableName in mainCatalog.SelectLinksTables())
            {
                int mainCount = mainCatalog.SelectCountRowsTable(tableName);
                int daughterCount = daughterCatalog.SelectCountRowsTable(tableName);

                if (linksTablesFunctions.ContainsKey(tableName))
                {
                    worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, $"Обработка {tableName}: Импортировано значений {linksTablesFunctions[tableName](mainCatalog, daughterCatalog, tableName)}");
                }
                else if (mainCount != daughterCount)
                {
                    worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, $"{tableName}: {mainCount} <- M -- D -> {daughterCount}");
                }
                else
                {
                    worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, $"{tableName}: Equal");
                }
            }
        }

        static void ProcessSomeTable() // Template process
        {
            // Наименование функции = наименование обрабатываемой таблицы
            // Принимает оба каталога, инициализирует необходимые объекты для опознования уникальности записи.
            // Передает все в функцию обработчик ProcessTable, которая найдет уникальные записи и запишит их в главную БД
            // Возвращается количество импортированных записей
        }

        // --- Process Functions for DafaultTables ---
        static int ProcessActTypeCL(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string columnName = "ISN_ACT_TYPE";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_ACT_TYPE]", "[CODE]", "[NAME]", "[NOTE]", "[PROTECTED]", "[WEIGHT]"  };
            return ProcessTable(mainCatalog, daughterCatalog, columnName, tableName);
        }

        static int ProcessAuthorizedDep(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string columnName = "ISN_AuthorizedDep";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_AuthorizedDep]", "[ShortName]", "[FullName]", "[Address]", "[District]" };
            return ProcessTable(mainCatalog, daughterCatalog, columnName, tableName);
        }

        static int ProcessCLS(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_CLS";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_CLS]", "[ISN_HIGH_CLS]", "[CODE]", "[WEIGHT]", "[NAME]", "[OBJ_KIND]", "[MULTISELECT]", "[NOTE]", "[FOREST_ELEM]", "[PROTECTED]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessDataExport(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "fcDbName";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[StatusID]", "[OwnerID]", "[CreationDateTime]", "[Deleted]", "[fcDbName]", "[fcDbBacPath]", "[isZiped]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }
        static int ProcessDECL_COMMISSION_CL(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_COMMISSION";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_COMMISSION]", "[CODE]", "[NAME_SHORT]", "[NAME]", "[CREATE_DATE]", "[DELETE_DATE]", "[NOTE]", "[PROTECTED]", "[WEIGHT]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessGROUPING_ATTRIBUTE_CL(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_GROUPING_ATTRIBUTE";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_GROUPING_ATTRIBUTE]", "[CODE]", "[NAME]", "[NOTE]", "[PROTECTED]", "[WEIGHT]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessINV_REQUIRED_WORK_CL(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_REQUIRED_WORK";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_REQUIRED_WORK]", "[CODE]", "[NAME]", "[NOTE]", "[PROTECTED]", "[WEIGHT]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessLANGUAGE_CL(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_LANGUAGE";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_LANGUAGE]", "[CODE]", "[NAME]", "[NOTE]", "[PROTECTED]", "[WEIGHT]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessFEATURE(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_FEATURE";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_FEATURE]", "[ISN_HIGH_FEATURE]", "[CODE]", "[NAME]", "[NOTE]", "[FOREST_ELEM]", "[PROTECTED]", "[WEIGHT]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessCITIZEN_CL(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_CITIZEN";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_CITIZEN]", "[NAME]", "[TITLE]", "[RELATIONSHIP]", "[LAST_FAMILY_NAME]", "[NICKNAME]", "[BIRTH_DATE]", "[DEATH_DATE]", "[PROFESSION]", "[POST]", "[DEGREE]", "[MILITARY_RANK]", "[HONORARY_RANK]", "[NOTE]", "[WEIGHT]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessORGANIZ_CL(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_ORGANIZ";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_ORGANIZ]", "[NAME]", "[CODE]", "[CREATE_YEAR]", "[CREATE_YEAR_INEXACT]", "[DELETE_YEAR]", "[DELETE_YEAR_INEXACT]", "[ADDRESS]", "[CEO_NAME]", "[ARCHIVIST_NAME]", "[CEO_PHONE]", "[ARCHIVIST_PHONE]", "[ARCHIVE_REGULATIONS]", "[HAS_EPK]", "[DELO_INSTRUCTION_YEAR]", "[WORKER_COUNT]", "[APPROVED_NOM]", "[KEEPING_PLACE]", "[NOTE]", "[WEIGHT]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessPAPER_CLS(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_PAPER_CLS";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_PAPER_CLS]", "[CLS_TYPE]", "[CODE]", "[NAME]", "[TYPEID]", "[SCOPE]", "[NOTE]", "[PROTECTED]", "[WEIGHT]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessPAPER_CLS_INV(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_PAPER_CLS_INV";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_PAPER_CLS_INV]", "[CODE]", "[NAME]", "[NOTE]", "[PROTECTED]", "[WEIGHT]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessPUBLICATION_TYPE_CL(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_PUBLICATION_TYPE";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_PUBLICATION_TYPE]", "[CODE]", "[NAME]", "[NOTE]", "[PROTECTED]", "[WEIGHT]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessQUESTION(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_QUESTION";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_QUESTION]", "[NAME]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessRECEIPT_REASON_CL(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_RECEIPT_REASON";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_RECEIPT_REASON]", "[CODE]", "[NAME]", "[NOTE]", "[PROTECTED]", "[WEIGHT]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessRECEIPT_SOURCE_CL(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_RECEIPT_SOURCE";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_RECEIPT_SOURCE]", "[CODE]", "[NAME]", "[NOTE]", "[PROTECTED]", "[WEIGHT]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessREF_FILE(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_REF_FILE";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_REF_FILE]", "[ISN_OBJ]", "[KIND]", "[GR_STORAGE]", "[NAME]", "[CATEGORY]", "[WEIGHT]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessREPRODUCTION_METHOD_CL(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_REPRODUCTION_METHOD";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_REPRODUCTION_METHOD]", "[CODE]", "[NAME]", "[NOTE]", "[PROTECTED]", "[WEIGHT]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessSTATE_CL(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_STATE";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_STATE]", "[ISN_HIGH_STATE]", "[CODE]", "[NAME]", "[NOTE]", "[FOREST_ELEM]", "[PROTECTED]", "[WEIGHT]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessSTORAGE_MEDIUM_CL(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_STORAGE_MEDIUM";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_STORAGE_MEDIUM]", "[ISN_HIGH_STORAGE_MEDIUM]", "[CODE]", "[NAME]", "[FOREST_ELEM]", "[NOTE]", "[PROTECTED]", "[WEIGHT]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessSUBJECT_CL(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "NAME";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_SUBJECT]", "[ISN_HIGH_SUBJECT]", "[CODE]", "[NAME]", "[NOTE]", "[FOREST_ELEM]", "[PROTECTED]", "[WEIGHT]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessSUBJECT_CL_NEW(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "NAME";
            return ProcessWithHIGHTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessTREE_SUPPORT(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN]", "[DUE]", "[WDUE]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessWORK_CL(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_WORK";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_WORK]", "[CODE]", "[NAME]", "[NOTE]", "[PROTECTED]", "[WEIGHT]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessFUND_PAPER(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_FUND";
            //List<string> forImportColumns = new List<string>() { "[ISN_FUND]", "[SORT_ORDER]", "[TEXT]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessFUND_UNIT_REG_STATS(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_FUND";
            //List<string> forImportColumns = new List<string>() { "[ISN_FUND]", "[UNIT_COUNT]", "[REG_UNIT]", "[TEXT]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }
        // -------------------------------------

        // --- Process Functions for LinksTables ---
        static int ProcessORGANIZ_RENAME(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_ORGANIZ_RENAME";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_ORGANIZ_RENAME]", "[ISN_ORGANIZ]", "[CODE]", "[CREATE_DATE]", "[CREATE_DATE_INEXACT]", "[DELETE_DATE]", "[DELETE_DATE_INEXACT]", "[NAME]", "[NOTE]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessARCHIVE(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            // Переделать на проверку замены именования. Таблица содержит в себе всегда один архив
            string uniqueColumnName = "";
            //List<string> forImportColumns = new List<string>() { };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessLOCATION(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_LOCATION";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_LOCATION]", "[ISN_HIGH_LOCATION]", "[ISN_ARCHIVE]", "[CODE]", "[NAME]", "[NOTE]", "[FOREST_ELEM]", "[PROTECTED]", "[WEIGHT]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessARCHIVE_STORAGE(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_LOCATION";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_LOCATION]", "[ROOM_CNT]", "[SPECIAL_ROOM_CNT]", "[ADAPTED_ROOM_CNT]", "[TOTAL_SPACE]", "[SPACE_WITH_ALARM]", "[SPACE_WITHOUT_ALARM]", "[SHELF_LENGTH]", "[METAL_SHELF_LENGTH]", "[FREE_SHELF_LENGTH]", "[STORAGE]", "[FLOOR]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessFUND(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_FUND";
           /* List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_FUND]", "[ISN_ARCHIVE]", "[FUND_NUM_1]", "[FUND_NUM_2]", "[FUND_NUM_3]", "[ISN_DOC_TYPE]", "[FUND_CATEGORY]", "[ISN_PERIOD]", "[FUND_KIND]", "[FUND_NAME_SHORT]", "[FUND_NAME_FULL]", "[INVENTORY_COUNT]", "[AUTO_INVENTORY_COUNT]", "[DOC_START_YEAR]", "[DOC_START_YEAR_INEXACT]", "[DOC_END_YEAR]", "[DOC_END_YEAR_INEXACT]", "[DOC_RECEIPT_YEAR]", "[LAST_CHECKED_YEAR]", "[LAST_DOC_CHECK_YEAR]", "[IS_IN_SEARCH]", "[IS_LOST]", "[ANNOTATE]", "[PROPERTY]", "[PRESENCE_FLAG]", "[ABSENCE_REASON]", "[MOVEMENT_NOTE]", "[HAS_MUSEUM_ITEMS]", "[TREASURE_UNITS_COUNT]", "[HAS_UNDOCUMENTED_PERIODS]", "[HAS_INCLUSIONS]", "[WAS_RENAMED]", "[WEIGHT]", "[KEEP_PERIOD]", "[ISN_SECURLEVEL]", "[SECURITY_CHAR]", "[SECURITY_REASON]", "[ISN_OAF]", "[OAF_NOTE]", "[CARD_COUNT]", "[ARCHIVE_DB_COUNT]", "[FUND_DB_COUNT]", "[INNER_DB_COUNT]", "[LIST_COUNT]", "[PERSONAL_UNDESCRIBED_DOC_COUNT]", "[HAS_ELECTRONIC_DOCS]", "[HAS_TRADITIONAL_DOCS]", "[CARRIER_TYPE]", "[UNDESCRIBED_DOC_COUNT]",
"[UNDECSRIBED_PAGE_COUNT]", "[JOIN_REASON]", "[ADDITIONAL_NSA]", "[KEYWORDS]", "[FUND_HISTORY]", "[NOTE]", "[INVENTORY_STATE]", "[ISN_SECURITY_REASON]", "[ForbidRecalc]" };
   */       
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessFUND_RENAME(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_FUND_RENAME";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_FUND_RENAME]", "[ISN_FUND]", "[CREATE_DATE]", "[CREATE_DATE_INEXACT]", "[DELETE_DATE]", "[DELETE_DATE_INEXACT]", "[FUND_NUM_1]", "[FUND_NUM_2]", "[FUND_NUM_3]", "[FUND_NAME_SHORT]", "[FUND_NAME_FULL]", "[NOTE]", "[NAME_SAVED]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessFUND_CHECK(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_FUND";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_FUND]", "[CARDBOARDED]", "[UNITS_NEED_CARDBOARDED]", "[UNITS_DBR]", "[UNITS_NEED_RESTORATION]", "[UNITS_NEED_BINDING]", "[UNITS_NEED_DISINFECTION]", "[UNITS_NEED_DISINSECTION]", "[FADING_PAGES]", "[UNITS_NEED_ENCIPHERING]", "[UNITS_NEED_COVER_CHANGE]", "[UNITS_INFLAMMABLE]", "[UNITS_NEED_KPO]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessFUND_DOC_TYPE(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ID";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_FUND]", "[ISN_DOC_TYPE]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessFUND_INCLUSION(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_INCLUSION";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_INCLUSION]", "[ISN_FUND]", "[ISN_CITIZEN]", "[ISN_ORGANIZ]", "[DOC_TYPES]", "[START_YEAR]", "[START_YEAR_INEXACT]", "[END_YEAR]", "[END_YEAR_INEXACT]", "[NOTE]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessFUND_PAPER_CLS(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_FUND";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_FUND]", "[ISN_PAPER_CLS]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessPUBLICATION_CL(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_PUBLICATION";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_PUBLICATION]", "[ISN_PUBLICATION_TYPE]", "[PUBLICATION_NUM]", "[AUTHORS]", "[PUBLICATION_NAME]", "[PUBLICATION_PLACE]", "[PUBLICATION_YEAR]", "[SHEET_COUNT]", "[PUBLISHER]", "[PROTECTED]", "[NOTE]", "[WEIGHT]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessFUND_PUBLICATION(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_FUND";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_FUND]", "[ISN_PUBLICATION]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessFUND_RECEIPT_REASON(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_FUND";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_FUND]", "[ISN_RECEIPT_REASON]", "[ORDER_NUM]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessFUND_RECEIPT_SOURCE(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_FUND";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_FUND]", "[ISN_RECEIPT_SOURCE]", "[ORDER_NUM]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessFUND_OAF(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_OAF";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[Deleted]", "[ISN_OAF]", "[ISN_FUND]", "[ISN_CHILD_FUND]", "[FUND_NUM_1]", "[FUND_NUM_2]", "[FUND_NUM_3]", "[FUND_CATEGORY]", "[FUND_NAME_SHORT]", "[FUND_NAME_FULL]", "[DOC_START_YEAR]", "[DOC_END_YEAR]", "[NOTE]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessFUND_OAF_REASON(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_FUND";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_FUND]", "[ISN_OAF_REASON]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessFUND_COLLECTION_REASONS(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_FUND";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_FUND]", "[ISN_COLLECTION_REASON]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }


        static int ProcessFUND_CREATOR(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_FUND_CREATOR";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_FUND_CREATOR]", "[ISN_CITIZEN]", "[ISN_ORGANIZ]", "[ISN_FUND]", "[KIND]", "[IS_PRIMARY]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }
        
        static int ProcessUNDOCUMENTED_PERIOD(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_PERIOD";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_PERIOD]", "[ISN_FUND]", "[PERIOD_START_YEAR]", "[PERIOD_END_YEAR]", "[ISN_ABSENCE_REASON]", "[INFO_PLACE]", "[NOTE]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessDEPOSIT(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_DEPOSIT";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_DEPOSIT]", "[ISN_FUND]", "[DEPOSIT_NUM]", "[DEPOSIT_NAME]", "[CONSISTS]", "[UNIT_COUNT]", "[DOC_COUNT]", "[PERSONAL_DOC_COUNT]", "[PAGE_COUNT]", "[NOTE]", "[PAPER_DOC_COUNT]", "[AV_TRAD_COUNT]", "[ELECTRONIC_DOC_COUNT]", "[PROCESSED]", "[PROCESSED_DOC]", "[PROCESSED_PERSONAL_DOC]", "[PROCESSED_PAGE]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessDEPOSIT_DOC_TYPE(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_DEPOSIT_DOC_TYPE";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_DEPOSIT_DOC_TYPE]", "[ISN_DEPOSIT]", "[ISN_DOC_TYPE]", "[UNIT_COUNT]", "[PROCESSED_COUNT]", "[CARRIER_TYPE]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessACT(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_ACT";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_ACT]", "[ISN_ACT_TYPE]", "[ISN_COMMISSION]", "[ISN_ESTIMATE_REASON]", "[ISN_FUND]", "[ACT_NUM]", "[ACT_DATE]", "[ACT_NAME]", "[MOVEMENT_FLAG]", "[ACT_OBJ]", "[ACT_PERSONS]", "[NOTE]", "[ACT_WORK]", "[UNIT_COUNT]", "[PAGE_NUMBERS]", "[DOC_DATES]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessINVENTORY(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_INVENTORY";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_INVENTORY]", "[ISN_FUND]", "[ISN_SECURLEVEL]", "[SECURITY_CHAR]", "[SECURITY_REASON]", "[ISN_INVENTORY_TYPE]", "[ISN_RECEIPT_SOURCE]", "[ISN_RECEIPT_REASON]", "[ISN_DOC_KIND]", "[ISN_INVENTORY_STORAGE]", "[ISN_REPRODUCTION_METHOD]", "[ISN_REQUIRED_WORK]", "[INVENTORY_NUM_1]", "[INVENTORY_NUM_2]", "[INVENTORY_NUM_3]", "[INVENTORY_NAME]", "[INVENTORY_KIND]", "[INVENTORY_KEEP_PERIOD]", "[COPY_COUNT]", "[DOC_START_YEAR]", "[DOC_START_YEAR_INEXACT]", "[DOC_END_YEAR]", "[DOC_END_YEAR_INEXACT]", "[CATALOGUING]", "[PRESENCE_FLAG]", "[ABSENCE_REASON]", "[MOVEMENT_NOTE]", "[UNITS_WITH_TREASURES_COUNT]", "[MUSEUM_UNITS_COUNT]", "[ANNOTATE]", "[WEIGHT]", "[INVENTORY_DOC_WORK]", "[NOTE]", "[VOL_NUM]", "[HAS_TRADITIONAL_DOCS]", "[HAS_ELECTRONIC_DOCS]", "[CARRIER_TYPE]", "[ADDITIONAL_NSA]", "[KEYWORDS]", "[ISN_SECURITY_REASON]", "[ForbidRecalc]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessINVENTORY_CHECK(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_INVENTORY";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_INVENTORY]", "[CARDBOARDED]", "[UNITS_NEED_CARDBOARDED]", "[UNITS_DBR]", "[UNITS_NEED_RESTORATION]", "[UNITS_NEED_BINDING]", "[UNITS_NEED_DISINFECTION]", "[UNITS_NEED_DISINSECTION]", "[FADING_PAGES]", "[UNITS_NEED_ENCIPHERING]", "[UNITS_NEED_COVER_CHANGE]", "[UNITS_INFLAMMABLE]", "[UNITS_NEED_KPO]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessINVENTORY_DOC_STORAGE(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_INVENTORY";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_INVENTORY]", "[ISN_STORAGE_MEDIUM]", "[ORDER_NUM]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessINVENTORY_DOC_TYPE(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_INVENTORY";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_INVENTORY]", "[ISN_DOC_TYPE]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessINVENTORY_CLS_ATTR(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_INVENTORY";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_INVENTORY]", "[ISN_CLS]", "[ORDER_NUM]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessINVENTORY_GROUPING_ATTRIBUTE(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_INVENTORY";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_INVENTORY]", "[ISN_GROUPING_ATTRIBUTE]", "[ORDER_NUM]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessINVENTORY_PAPER_CLS(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_INVENTORY";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_INVENTORY]", "[ISN_PAPER_CLS_INV]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessINVENTORY_REQUIRED_WORK(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_INVENTORY";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_INVENTORY]", "[ISN_REQUIRED_WORK]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessINVENTORY_STRUCTURE(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_INVENTORY_CLS";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[StatusID]", "[Deleted]", "[ISN_INVENTORY_CLS]", "[ISN_HIGH_INVENTORY_CLS]", "[ISN_ARCHIVE]", "[ISN_FUND]", "[ISN_INVENTORY]", "[CODE]", "[NAME]", "[NOTE]", "[FOREST_ELEM]", "[PROTECTED]", "[WEIGHT]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessDOCUMENT_STATS(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_DOCUMENT_STATS";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_DOCUMENT_STATS]", "[ISN_FUND]", "[ISN_DOC_TYPE]", "[ISN_INVENTORY]", "[CARRIER_TYPE]", "[UNIT_COUNT]", "[UNIT_INVENTORY]", "[UNIT_REGISTERED]", "[REG_UNIT]", "[REG_UNIT_INVENTORY]", "[REG_UNIT_REGISTERED]", "[UNIT_UNDESCRIBED]", "[REG_UNIT_UNDESCRIBED]", "[REG_UNIT_OC]", "[UNIT_OC_COUNT]", "[REG_UNIT_HAS_SF]", "[UNIT_HAS_SF]", "[REG_UNIT_HAS_FP]", "[UNIT_HAS_FP]", "[REG_UNITS_NOT_FOUND]", "[UNITS_NOT_FOUND]", "[SECRET_REG_UNITS]", "[SECRET_UNITS]", "[REG_UNITS_SEARCH]", "[UNITS_SEARCH]", "[REG_UNITS_UNIQUE]", "[UNITS_UNIQUE]", "[REG_UNITS_CTALOGUE]", "[UNITS_CATALOGUED]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessREF_ACT(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_REF_ACT";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_REF_ACT]", "[ISN_ACT]", "[ISN_OBJ]", "[KIND]", "[UNIT_COUNT]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessREF_CLS(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_REF_CLS";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_REF_CLS]", "[ISN_CLS]", "[ISN_CLSID]", "[ISN_TREE]", "[ISN_OBJ]", "[ORDER_NUM]", "[KIND]", "[NOTE]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessREF_FEATURE(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_REF_FEATURE";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_REF_FEATURE]", "[ISN_FEATURE]", "[ISN_OBJ]", "[KIND]", "[ORDER_NUM]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessREF_LANGUAGE(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_REF_LANGUAGE";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_REF_LANGUAGE]", "[ISN_LANGUAGE]", "[ISN_OBJ]", "[KIND]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessREF_LOCATION(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_REF_LOCATION";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_REF_LOCATION]", "[ISN_LOCATION]", "[ISN_OBJ]", "[UNIT_NUM_FROM]", "[UNIT_NUM_TO]", "[KIND]", "[NOTE]", "[UNIT_NUM_OLD_FROM]", "[UNIT_NUM_OLD_TO]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessREF_QUESTION(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_REF_QUESTION";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_REF_QUESTION]", "[ISN_QUESTION]", "[ISN_OBJ]", "[KIND]", "[Pages]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessUNIT(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_UNIT";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_UNIT]", "[ISN_HIGH_UNIT]", "[ISN_INVENTORY]", "[ISN_DOC_TYPE]", "[ISN_LOCATION]", "[ISN_SECURLEVEL]", "[SECURITY_CHAR]", "[SECURITY_REASON]", "[ISN_INVENTORY_CLS]", "[ISN_STORAGE_MEDIUM]", "[ISN_DOC_KIND]", "[UNIT_KIND]", "[UNIT_NUM_1]", "[UNIT_NUM_2]", "[VOL_NUM]", "[NAME]", "[ANNOTATE]", "[DELO_INDEX]", "[PRODUCTION_NUM]", "[UNIT_CATEGORY]", "[NOTE]", "[IS_IN_SEARCH]", "[IS_LOST]", "[HAS_SF]", "[HAS_FP]", "[HAS_DEFECTS]", "[ARCHIVE_CODE]", "[CATALOGUED]", "[WEIGHT]", "[UNIT_CNT]", "[START_YEAR]", "[START_YEAR_INEXACT]", "[END_YEAR]", "[END_YEAR_INEXACT]", "[MEDIUM_TYPE]", "[BACKUP_COPY_CNT]", "[HAS_TREASURES]", "[IS_MUSEUM_ITEM]", "[PAGE_COUNT]", "[CARDBOARDED]", "[ADDITIONAL_CLS]", "[ALL_DATE]", "[ISN_SECURITY_REASON]", "[UNIT_NUM_TXT]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessUNIT_ELECTRONIC(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_UNIT";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_UNIT]", "[DATA_FORMAT]", "[SIZE]", "[CARRIER_DESC]", "[ACCOMP_DOC]", "[UNIT_CNT]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessUNIT_FOTO(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_UNIT";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_UNIT]", "[ISN_DOC_KIND]", "[PLACE]", "[AUTHOR]", "[BASE_KIND]", "[COLOR]", "[SIZE]", "[STORAGE_INFO]", "[NEGATIVE_COUNT]", "[POSITIVE_COUNT]", "[PRINT_COUNT]", "[DUP_NEGATIVE_COUNT]", "[SLIDE_COUNT]", "[BACKUP_COUNT]", "[SHOT_COUNT]", "[ORIGINAL]", "[ACCOMP_DOC]", "[FOTO_DATE]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessUNIT_FOTO_EX(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_UNIT";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_UNIT]", "[CODE]", "[RUBRIC]", "[SOURCE]", "[START_YEAR]", "[END_YEAR]", "[COUNT]", "[COMPILER]", "[FOTO_DATE]", "[PUBLICATION]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessUNIT_MICROFORM(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_UNIT";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_UNIT]", "[FRAME_COUNT]", "[BACKUP_COUNT]", "[CARRIER_DESC]", "[ACCOMP_DOC]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessUNIT_MOVIE(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_UNIT";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_UNIT]", "[ISN_DOC_KIND]", "[UNIT_TYPE]", "[BASE]", "[FILM_TYPE]", "[FILM_FORMAT]", "[FOOTAGE]", "[START_DATE]", "[END_DATE]", "[PART_COUNT]", "[PLAYBACK_TIME]", "[SOUND]", "[FORMAT]", "[COLOR]", "[AUTHOR]", "[SHOOTING_PLACE]", "[PRODUCTION_PLACE]", "[NEGATIVE_COUNT]", "[NEGATIVE_LENGTH]", "[DUP_NEGATIVE_COUNT]", "[DUP_NEGATIVE_LENGTH]", "[PHONO_NEGATIVE_COUNT]", "[PHONO_NEGATIVE_LENGTH]", "[PHONO_MAG_COUNT]", "[PHONO_MAG_LENGTH]", "[PHONO_COMBINED_COUNT]", "[PHONO_COMBINED_LENGTH]", "[INTERPOSITIVE_COUNT]", "[INTERPOSITIVE_LENGTH]", "[POSITIVE_COUNT]", "[POSITIVE_LENGTH]", "[DUP_BACKUP_COUNT]", "[DUP_BACKUP_LENGTH]", "[INTERPOSITIVE_BACKUP_COUNT]", "[INTERPOSITIVE_BACKUP_LENGTH]", "[CINEX_COUNT]", "[COLOR_PASS_COUNT]", "[CARRIER_DESC]", "[ACCOMP_DOC]", "[UNIT_CNT]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }
        static int ProcessUNIT_MOVIE_EX(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_UNIT";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_UNIT]", "[STORY_NUM]", "[STUDIO]", "[DIRECTOR]", "[OPERATOR]", "[CREATOR]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessUNIT_NTD(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_UNIT";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_UNIT]", "[ISN_NTD_KIND]", "[COMPLEX_NUM_1]", "[COMPLEX_NUM_2]", "[DEV_STAGE]", "[AUTHOR]", "[ORGANIZ]", "[REQUEST_DATE]", "[REQUEST_NUM]", "[PATENT_NUM]", "[COPYRIGHT_NUM]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessUNIT_PHONO(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_UNIT";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_UNIT]", "[ISN_DOC_KIND]", "[BASE]", "[REC_SPEED]", "[REC_DATE]", "[PHONO_TYPE]", "[LENGTH]", "[REC_PLACE]", "[PLAYBACK_TIME]", "[PRODUCTION_PLACE]", "[AUTHOR]", "[PERFORMER]", "[ORIGINAL_COUNT]", "[DISK_COUNT]", "[NEGATIVE_COUNT]", "[COPY_COUNT]", "[BACKUP_COUNT]", "[NEGATIVE_LENGTH]", "[COPY_LENGTH]", "[BACKUP_LENGTH]", "[UNIT_CNT]", "[CARRIER_DESC]", "[ACCOMP_DOC]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessUNIT_REQUIRED_WORK(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_UNIT_REQUIRED_WORK";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_UNIT_REQUIRED_WORK]", "[ISN_WORK]", "[ISN_UNIT]", "[ISN_REF_ACT]", "[NOTE]", "[CHECK_DATE]", "[IS_ACTUAL]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessUNIT_STATE(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_UNIT_STATE";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_UNIT_STATE]", "[ISN_UNIT]", "[ISN_STATE]", "[ISN_REF_ACT]", "[PAGE_NUMS]", "[PAGE_COUNT]", "[STATE_DATE]", "[NOTE]", "[IS_ACTUAL]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessUNIT_VIDEO(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_UNIT";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_UNIT]", "[ISN_DOC_KIND]", "[REC_DATE]", "[REC_TYPE]", "[REC_FORMAT]", "[BASE]", "[ORIGINAL]", "[START_DATE]", "[END_DATE]", "[PART_COUNT]", "[PLAYBACK_TIME]", "[SOUND]", "[COLOR]", "[AUTHOR]", "[SHOOTING_PLACE]", "[PRODUCTION_PLACE]", "[ORIGINAL_COUNT]", "[COPY_COUNT]", "[BACKUP_COUNT]", "[UNIT_CNT]", "[CARRIER_DESC]", "[ACCOMP_DOC]", "[PERFORMER]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessUNIT_VIDEO_EX(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_UNIT";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_UNIT]", "[STORY_NUM]", "[STUDIO]", "[DIRECTOR]", "[OPERATOR]", "[CREATOR]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessUNIT_WORK(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_UNIT_WORK";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_UNIT_WORK]", "[ISN_UNIT]", "[ISN_WORK]", "[WORK_DATE]", "[NOTE]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }

        static int ProcessDOCUMENT(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
        {
            string uniqueColumnName = "ISN_DOCUM";
            //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_DOCUM]", "[ISN_REPRODUCTION_METHOD]", "[ISN_SECURLEVEL]", "[SECURITY_CHAR]", "[ISN_UNIT]", "[ISN_DOC_KIND]", "[DOC_NUM]", "[WEIGHT]", "[PAGE_FROM]", "[PAGE_TO]", "[PAGE_COUNT]", "[NAME]", "[ANNOTATE]", "[DOCUM_DATE]", "[INEXACT_DOCUM_DATE]", "[EVENT_DATE]", "[INEXACT_EVENT_DATE]", "[EVENT_PLACE]", "[ENCLOSURES]", "[NOTE]", "[IS_ORIGINAL]", "[ADDITIONAL_CLS]", "[KEYWORDS]", "[AUTHORS]" };
            return ProcessTable(mainCatalog, daughterCatalog, uniqueColumnName, tableName);
        }
        // -------------------------------------

        static int ProcessWithHIGHTable(DBCatalog mainCatalog, DBCatalog daughterCatalog, string columnName, string tableName)
        {
            int countImports = 0;
            List<string> mainListCheckData = mainCatalog.SelectListColumnsData(columnName, tableName);
            List<string> daughterListCheckData = daughterCatalog.SelectListColumnsData(columnName, tableName);
            List<string> uniqueValues = ValuesManager.CheckUniqueValue(mainListCheckData, daughterListCheckData);
            List<string> forImportColumns = mainCatalog.SelectColumnsNames(tableName);

            List<string> isnSubjects = mainCatalog.SelectListColumnsData("ISN_SUBJECT", tableName);

            /*if (isnSubjects.Contains("повтор isn_key") "ISN_SUBJECT" in) {

                new "ISN_SUBJECT" = mainCatalog.SelectCountRowsTable(tableName) + 1;
            }*/


            if (tableName == "eqUsers")
            {
                forImportColumns.Remove("[DisplayName]");
            }

            if (uniqueValues.Count > 0)
            {
                foreach (string value in uniqueValues)
                {
                    ValuesManager.AddUniqueValue(tableName, forImportColumns, columnName, value, mainCatalog, daughterCatalog);
                }
                countImports += uniqueValues.Count;
            }

            return countImports;
        }

        // --- Master merge process table ---
        static int ProcessTable(DBCatalog mainCatalog, DBCatalog daughterCatalog, string columnName, string tableName)
        {
            int countImports = 0;
            List<string> mainListCheckData = mainCatalog.SelectListColumnsData(columnName, tableName);
            List<string> daughterListCheckData = daughterCatalog.SelectListColumnsData(columnName, tableName);
            List<string> uniqueValues = ValuesManager.CheckUniqueValue(mainListCheckData, daughterListCheckData);


            // ---
            List<string> forImportColumns = mainCatalog.SelectColumnsNames(tableName);
            /*for (int i = 0; i < forImportColumns.Count; i++)
            {
                forImportColumns[i] = "[" + forImportColumns[i] + "]";
            }*/
            // ---

            if (tableName == "eqUsers")
            {
                forImportColumns.Remove("[DisplayName]");
            }

            if (uniqueValues.Count > 0)
            {
                foreach (string value in uniqueValues)
                {
                    ValuesManager.AddUniqueValue(tableName, forImportColumns, columnName, value, mainCatalog, daughterCatalog);
                }
                countImports += uniqueValues.Count;
            }

            return countImports;
        }
        // --------------------------------------

        static int ProcessTBLUsers(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
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


            


            /*
            // [DisplayName] лишнее значение, так это вычисляемая строка
              INSERT INTO [test].[dbo].[eqUsers] SELECT [ID]
                  ,[Login]
                  ,[Department]
                  ,[Deleted]
                  ,[OwnerID]
                  ,[CreationDateTime]
                  ,[StatusID]
                  ,[Email]
                  ,[patronymic]
                  ,[Position]
                  ,[Phone]
                  ,[Room_Number]
                  ,[Description]
                  ,[surname]
                  ,[AccessGranted]
                  ,[Supervisor]
                  ,[FirstName]
                  ,[BUILD_IN_ACCOUNT]

                  ,[Pass]
                  ,[Cookie]
                  ,[UserTheme] FROM [main].[dbo].[eqUsers] WHERE Login = 'reader'


             */


            //DefaultValuesManager.CheckUsers(mainUsersLogins);

            /*            for (int i = 1; i <= mainUsersLogins.Count; i++)
                        {
                            foreach (string data in mainUsersLogins[1])
                            {

                            }
                        }*/

            //3. Выводим в листбокс количество импортированных пользователей
            return countImports;
        }
    }

    public static class ValuesManager
    {
        static List<string> defaultUsers = new List<string>() { "sa", "anonymous", "admin", "reader", "arch", "tech" };

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

        public static void RestoreUser(string userLogin)
        {

        }

        public static void AddUniqueValue(string tableName, List<string> forImportColumns, string filterColumn, string filterValue, DBCatalog maincatalog, DBCatalog daughterCatalog)
        {
            

            daughterCatalog.UpdateID(tableName, filterColumn, filterValue);
            maincatalog.InsertFromUniqueValue(tableName, forImportColumns, daughterCatalog.ReturnCatalog(), tableName, filterColumn, filterValue);
        }

    }
}
