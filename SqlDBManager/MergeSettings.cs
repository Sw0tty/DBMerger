using System;
using System.Collections.Generic;


namespace SqlDBManager
{
    abstract public class MergeSettings
    {
        public static string ExtraIDColumn = "DocID";

        /// <summary>
        /// Tables columns to update on second tab. <para/>
        /// 1. List(string) update columns <br/>
        /// </summary>
        public static Dictionary<string, List<string>> UpdateTables { get; } = new Dictionary<string, List<string>>
        {
            { "tblARCHIVE",
                new List<string>() { "NAME_SHORT", "NAME", "ADDRESS", "AUTHORITY" } },
        };

        /// <summary>
        /// Params for processing simple tables <para/>
        /// 1. (string) uniqueValueColumnName <br/>
        /// 2. (string) idLikeColumnName <br/>
        /// 3. (string) highLevelColumnName <br/>
        /// 4. List(string) excludeColumns <br/>
        /// 5. (bool) allowsNull <br/>
        /// </summary>
        public static Dictionary<string, Tuple<string, string, string, List<string>, bool>> DefaultTablesParams { get; } = new Dictionary<string, Tuple<string, string, string, List<string>, bool>>
        {
            // 1. string uniqueValueColumnName         2. string idLikeColumnName    3. string highLevelColumnName     4. List<string> excludeColumns         
            { "eqUsers",
                new Tuple<string, string, string, List<string>, bool>("Login", null, null, new List<string>() { "DisplayName" }, false) },

            { "tblACT_TYPE_CL",
                new Tuple<string, string, string, List<string>, bool>("NAME", "ISN_ACT_TYPE", null, null, true) },

            { "tblAuthorizedDep",
                new Tuple<string, string, string, List<string>, bool>("ShortName", "ISN_AuthorizedDep", null, null, true) },

            { "tblCLS",
                new Tuple<string, string, string, List<string>, bool>("NAME", "ISN_CLS", "ISN_HIGH_CLS", null, true) },

            { "tblDataExport",
                new Tuple<string, string, string, List<string>, bool>("fcDbName", null, null, null, true) },

            { "tblDECL_COMMISSION_CL",
                new Tuple<string, string, string, List<string>, bool>("NAME", "ISN_COMMISSION", null, null, true) },
            
            //{ "tblConstantsSpec", ProcessConstantsSpec },
            
            { "tblGROUPING_ATTRIBUTE_CL",
                new Tuple<string, string, string, List<string>, bool>("NAME", "ISN_GROUPING_ATTRIBUTE", null, null, true) },

            { "tblINV_REQUIRED_WORK_CL",
                new Tuple<string, string, string, List<string>, bool>("NAME", "ISN_REQUIRED_WORK", null, null, true) },

            { "tblLANGUAGE_CL",
                new Tuple<string, string, string, List<string>, bool>("NAME", "ISN_LANGUAGE", null, null, true) },

            { "tblFEATURE",
                new Tuple<string, string, string, List<string>, bool>("NAME", "ISN_FEATURE", "ISN_HIGH_FEATURE", null, true) },

            { "tblCITIZEN_CL",
                new Tuple<string, string, string, List<string>, bool>("NAME", "ISN_CITIZEN", null, null, true) },

            { "tblORGANIZ_CL",
                new Tuple<string, string, string, List<string>, bool>("NAME", "ISN_ORGANIZ", null, null, true) },

            { "tblPAPER_CLS",
                new Tuple<string, string, string, List<string>, bool>("NAME", "ISN_PAPER_CLS", null, null, true) },

            { "tblPAPER_CLS_INV",
                new Tuple<string, string, string, List<string>, bool>("NAME", "ISN_PAPER_CLS_INV", null, null, true) },

            { "tblPUBLICATION_TYPE_CL",
                new Tuple<string, string, string, List<string>, bool>("NAME", "ISN_PUBLICATION_TYPE", null, null, true) },

            { "tblQUESTION",
                new Tuple<string, string, string, List<string>, bool>("NAME", "ISN_QUESTION", null, null, true) },

            { "tblRECEIPT_REASON_CL",
                new Tuple<string, string, string, List<string>, bool>("NAME", "ISN_RECEIPT_REASON", null, null, true) },

            { "tblRECEIPT_SOURCE_CL",
                new Tuple<string, string, string, List<string>, bool>("NAME", "ISN_RECEIPT_SOURCE", null, null, true) },

            { "tblREF_FILE",
                new Tuple<string, string, string, List<string>, bool>("NAME", "ISN_REF_FILE", null, null, true) },

            { "tblREPRODUCTION_METHOD_CL",
                new Tuple<string, string, string, List<string>, bool>("NAME", "ISN_REPRODUCTION_METHOD", null, null, true) },
            
            // { "tblService", ProcessService },
            
            { "tblSTATE_CL",
                new Tuple<string, string, string, List<string>, bool>("NAME", "ISN_STATE", "ISN_HIGH_STATE", null, true) },

            { "tblSTORAGE_MEDIUM_CL",
                new Tuple<string, string, string, List<string>, bool>("NAME", "ISN_STORAGE_MEDIUM", "ISN_HIGH_STORAGE_MEDIUM", null, true) },

            { "tblSUBJECT_CL",
                new Tuple<string, string, string, List<string>, bool>("NAME", "ISN_SUBJECT", "ISN_HIGH_SUBJECT", null, true) },

            { "tblTREE_SUPPORT",
                new Tuple<string, string, string, List<string>, bool>("ISN", null, null, null, true) },

            { "tblWORK_CL",
                new Tuple<string, string, string, List<string>, bool>("NAME", "ISN_WORK", null, null, true) },

            { "rptFUND_PAPER",
                new Tuple<string, string, string, List<string>, bool>("ISN_FUND", null, null, null, true) },

            { "rptFUND_UNIT_REG_STATS",
                new Tuple<string, string, string, List<string>, bool>("ISN_FUND", null, null, null, true) },
        };

        /// <summary>
        /// Params for processing composite tables <para/>
        /// 1. (string) uniqueValueColumnName <br/>
        /// 2. (string) idLikeColumnName <br/>
        /// 3. (string) highLevelColumnName <br/>
        /// 4. (string) parentIdColumn <br/>
        /// 5. (string) numerateColumn <br/>
        /// 6. List(string) extraFilterColumns <br/>
        /// 7. List(string) excludeColumns <br/>
        /// 8. (bool) allowsNull <br/>
        /// </summary>
        public static Dictionary<string, Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>> LinksTablesParams { get; } = new Dictionary<string, Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>>
        {
            { "tblORGANIZ_RENAME",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>("NAME", "ISN_ORGANIZ_RENAME", null, "ISN_ORGANIZ", null, null, null, new Tuple<bool>(true)) },

            { "tblARCHIVE",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, "ISN_ARCHIVE", null, null, null, null, null, new Tuple<bool>(true)) },

            { "tblLOCATION",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>("NOTE", "ISN_LOCATION", "ISN_HIGH_LOCATION", null, null, null, null, new Tuple<bool>(true)) },

            { "tblARCHIVE_STORAGE",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, null, null, null, null, null, null, new Tuple<bool>(true)) },

            // ---In recalc---
            //{ "tblARCHIVE_PASSPORT",
            //    new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>("PASS_YEAR", "ISN_PASSPORT", null, null, null, null, null, new Tuple<bool>(true)) },
            // -------

            // ---In recalc---
            //{ "tblARCHIVE_STATS",
            //    new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, "ISN_ARCHIVE_STATS", null, "ISN_PASSPORT", null, null, null, new Tuple<bool>(true)) },
            // -------

            { "tblFUND",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>("FUND_NAME_SHORT", "ISN_FUND", null, null, "FUND_NUM_2", null, null, new Tuple<bool>(true)) },

            { "tblFUND_RENAME",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>("FUND_NAME_SHORT", "ISN_FUND_RENAME", null, "ISN_FUND", null, null, null, new Tuple<bool>(true)) },

            { "tblFUND_CHECK",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, null, null, "ISN_FUND", null, null, null, new Tuple<bool>(true)) },

            { "tblFUND_DOC_TYPE",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, null, null, "ISN_FUND", null, null, null, new Tuple<bool>(true)) },

            { "tblFUND_INCLUSION",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, "ISN_INCLUSION", null, "ISN_FUND", null, null, null, new Tuple<bool>(true)) },

            { "tblFUND_PAPER_CLS",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, null, null, "ISN_FUND", null, null, null, new Tuple<bool>(true)) },

            { "tblPUBLICATION_CL",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>("PUBLICATION_NAME", "ISN_PUBLICATION", null, null, "PUBLICATION_NUM", null, null, new Tuple<bool>(true)) },

            { "tblFUND_PUBLICATION",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, "ISN_PUBLICATION", null, "ISN_FUND", null, null, null, new Tuple<bool>(true)) },

            { "tblFUND_RECEIPT_REASON",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, null, null, "ISN_FUND", null, null, null, new Tuple<bool>(true)) },

            { "tblFUND_RECEIPT_SOURCE",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, null, null, "ISN_FUND", null, null, null, new Tuple<bool>(true)) },

            { "tblFUND_OAF",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>("FUND_NAME_SHORT", "ISN_OAF", null, "ISN_FUND", "FUND_NUM_2", null, null, new Tuple<bool>(true)) },

            { "tblFUND_OAF_REASON",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, null, null, "ISN_FUND", null, null, null, new Tuple<bool>(true)) },

            { "tblFUND_COLLECTION_REASONS",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, "ISN_COLLECTION_REASON", null, "ISN_FUND", null, null, null, new Tuple<bool>(true)) },

            { "tblFUND_CREATOR",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, "ISN_FUND_CREATOR", null, "ISN_FUND", null, null, null, new Tuple<bool>(true)) },

            { "tblUNDOCUMENTED_PERIOD",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>("PERIOD_START_YEAR", "ISN_PERIOD", null, "ISN_FUND", null, null, null, new Tuple<bool>(true)) },

            { "tblDEPOSIT",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>("DEPOSIT_NAME", "ISN_DEPOSIT", null, "ISN_FUND", "DEPOSIT_NUM", null, null, new Tuple<bool>(true)) },

            { "tblDEPOSIT_DOC_TYPE",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, "ISN_DEPOSIT_DOC_TYPE", null, "ISN_DEPOSIT", null, null, null, new Tuple<bool>(true)) },

            { "tblACT",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>("ACT_NAME", "ISN_ACT", null, "ISN_FUND", null, new List<string>() { "ACT_DATE", "ACT_NUM", "ACT_OBJ", "ISN_ACT_TYPE", "MOVEMENT_FLAG", "DOC_DATES", "UNIT_COUNT" }, null, new Tuple<bool>(true)) },

            { "tblINVENTORY",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>("INVENTORY_NAME", "ISN_INVENTORY", null, "ISN_FUND", "INVENTORY_NUM_1", null, null, new Tuple<bool>(true)) },

            { "tblINVENTORY_CHECK",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, null, null, "ISN_INVENTORY", null, null, null, new Tuple<bool>(true)) },

            { "tblINVENTORY_DOC_STORAGE",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, null, null, "ISN_INVENTORY", null, null, null, new Tuple<bool>(true)) },

            { "tblINVENTORY_DOC_TYPE",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, null, null, "ISN_INVENTORY", null, null, null, new Tuple<bool>(true)) },

            { "tblINVENTORY_CLS_ATTR",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, null, null, "ISN_INVENTORY", null, null, null, new Tuple<bool>(true)) },

            { "tblINVENTORY_GROUPING_ATTRIBUTE",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, null, null, "ISN_INVENTORY", null, null, null, new Tuple<bool>(true)) },

            { "tblINVENTORY_PAPER_CLS",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, null, null, "ISN_INVENTORY", null, null, null, new Tuple<bool>(true)) },

            { "tblINVENTORY_REQUIRED_WORK",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, null, null, "ISN_INVENTORY", null, null, null, new Tuple<bool>(true)) },

            { "tblINVENTORY_STRUCTURE", // Есть уникальное по NAME, но оно не заполняется пользователем
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, "ISN_INVENTORY_CLS", "ISN_HIGH_INVENTORY_CLS", "ISN_INVENTORY", null, null, null, new Tuple<bool>(true)) },

            { "tblDOCUMENT_STATS",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, "ISN_DOCUMENT_STATS", null, "ISN_FUND", null, null, null, new Tuple<bool>(true)) },

            { "tblREF_ACT",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, "ISN_REF_ACT", null, "ISN_ACT", null, null, null, new Tuple<bool>(true)) },

            { "tblREF_CLS",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, "ISN_REF_CLS", null, null, null, null, null, new Tuple<bool>(true)) },

            { "tblREF_FEATURE",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, "ISN_REF_FEATURE", null, null, null, null, null, new Tuple<bool>(true)) },

            { "tblREF_LANGUAGE",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, "ISN_REF_LANGUAGE", null, null, null, null, null, new Tuple<bool>(true)) },

            { "tblREF_LOCATION",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, "ISN_REF_LOCATION", null, "ISN_LOCATION", null, null, null, new Tuple<bool>(true)) },

            { "tblREF_QUESTION",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, "ISN_REF_QUESTION", null, "ISN_QUESTION", null, null, null, new Tuple<bool>(true)) },

            { "tblUNIT", // "ISN_HIGH_UNIT"
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>("NAME", "ISN_UNIT", null, "ISN_INVENTORY", null, new List<string>() { "UNIT_NUM_1", "UNIT_NUM_2", "WEIGHT", "PAGE_COUNT", "ALL_DATE", "START_YEAR", "END_YEAR" }, null, new Tuple<bool>(true)) },

            { "tblUNIT_ELECTRONIC",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, null, null, "ISN_UNIT", null, null, null, new Tuple<bool>(true)) },

            { "tblUNIT_FOTO",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, null, null, "ISN_UNIT", null, null, null, new Tuple<bool>(true)) },

            { "tblUNIT_FOTO_EX",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, null, null, "ISN_UNIT", null, null, null, new Tuple<bool>(true)) },

            { "tblUNIT_MICROFORM",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, null, null, "ISN_UNIT", null, null, null, new Tuple<bool>(true)) },

            { "tblUNIT_MOVIE",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, null, null, "ISN_UNIT", null, null, null, new Tuple<bool>(true)) },

            { "tblUNIT_MOVIE_EX",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, null, null, "ISN_UNIT", null, null, null, new Tuple<bool>(true)) },

            { "tblUNIT_NTD",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, null, null, "ISN_UNIT", null, null, null, new Tuple<bool>(true)) },

            { "tblUNIT_PHONO",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, null, null, "ISN_UNIT", null, null, null, new Tuple<bool>(true)) },

            { "tblUNIT_REQUIRED_WORK",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, "ISN_UNIT_REQUIRED_WORK", null, "ISN_UNIT", null, null, null, new Tuple<bool>(true)) },

            { "tblUNIT_STATE",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, "ISN_UNIT_STATE", null, "ISN_UNIT", null, new List<string>() { "STATE_DATE", "PAGE_NUMS", "PAGE_COUNT" }, null, new Tuple<bool>(true)) },

            { "tblUNIT_VIDEO",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, null, null, "ISN_UNIT", null, null, null, new Tuple<bool>(true)) },

            { "tblUNIT_VIDEO_EX",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, null, null, "ISN_UNIT", null, null, null, new Tuple<bool>(true)) },

            { "tblUNIT_WORK",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>(null, "ISN_UNIT_WORK", null, "ISN_UNIT", null, null, null, new Tuple<bool>(true)) },

            { "tblDOCUMENT",
                new Tuple<string, string, string, string, string, List<string>, List<string>, Tuple<bool>>("NAME", "ISN_DOCUM", null, "ISN_UNIT", null, null, null, new Tuple<bool>(true)) },
        };

        public static Dictionary<string, Tuple<string, string, string, string, string, List<string>>> RecalcTablesParams { get; } = new Dictionary<string, Tuple<string, string, string, string, string, List<string>>>
        {
            { "tblARCHIVE_STATS",
                new Tuple<string, string, string, string, string, List<string>>(null, "ISN_ARCHIVE_STATS", null, "ISN_PASSPORT", null, null) },
        };
    }
}
