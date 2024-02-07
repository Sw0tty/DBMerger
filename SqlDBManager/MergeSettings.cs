using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDBManager
{
    abstract public class MergeSettings
    {
        /// <summary>
        /// Params (columns name in DB) for processing simple tables <para/>
        /// 1. (string) uniqueValueColumnName <br/>
        /// 2. (string) idLikeColumnName <br/>
        /// 3. (string) highLevelColumnName <br/>
        /// 4. List(string) excludeColumns <br/>
        /// </summary>
        public static Dictionary<string, Tuple<string, string, string, List<string>>> DefaultTablesParams { get; } = new Dictionary<string, Tuple<string, string, string, List<string>>>
        {
            // 1. string uniqueValueColumnName         2. string idLikeColumnName    3. string highLevelColumnName     4. List<string> excludeColumns         
            { "eqUsers",
                new Tuple<string, string, string, List<string>>("Login", null, null, new List<string>(){ "DisplayName" }) },

            { "tblACT_TYPE_CL",
                new Tuple<string, string, string, List<string>>("NAME", "ISN_ACT_TYPE", null, null) },

            { "tblAuthorizedDep",
                new Tuple<string, string, string, List<string>>("ShortName", "ISN_AuthorizedDep", null, null) },

            { "tblCLS",
                new Tuple<string, string, string, List<string>>("NAME", "ISN_CLS", "ISN_HIGH_CLS", null) },

            { "tblDataExport",
                new Tuple<string, string, string, List<string>>("fcDbName", null, null, null) },

            { "tblDECL_COMMISSION_CL",
                new Tuple<string, string, string, List<string>>("NAME", "ISN_COMMISSION", null, null) },
            
            //{ "tblConstantsSpec", ProcessConstantsSpec },
            
            { "tblGROUPING_ATTRIBUTE_CL",
                new Tuple<string, string, string, List<string>>("NAME", "ISN_GROUPING_ATTRIBUTE", null, null) },

            { "tblINV_REQUIRED_WORK_CL",
                new Tuple<string, string, string, List<string>>("NAME", "ISN_REQUIRED_WORK", null, null) },

            { "tblLANGUAGE_CL",
                new Tuple<string, string, string, List<string>>("NAME", "ISN_LANGUAGE", null, null) },

            { "tblFEATURE",
                new Tuple<string, string, string, List<string>>("NAME", "ISN_FEATURE", "ISN_HIGH_FEATURE", null) },

            { "tblCITIZEN_CL",
                new Tuple<string, string, string, List<string>>("NAME", "ISN_CITIZEN", null, null) },

            { "tblORGANIZ_CL",
                new Tuple<string, string, string, List<string>>("NAME", "ISN_ORGANIZ", null, null) },

            { "tblPAPER_CLS",
                new Tuple<string, string, string, List<string>>("NAME", "ISN_PAPER_CLS", null, null) },

            { "tblPAPER_CLS_INV",
                new Tuple<string, string, string, List<string>>("NAME", "ISN_PAPER_CLS_INV", null, null) },

            { "tblPUBLICATION_TYPE_CL",
                new Tuple<string, string, string, List<string>>("NAME", "ISN_PUBLICATION_TYPE", null, null) },

            { "tblQUESTION",
                new Tuple<string, string, string, List<string>>("NAME", "ISN_QUESTION", null, null) },

            { "tblRECEIPT_REASON_CL",
                new Tuple<string, string, string, List<string>>("NAME", "ISN_RECEIPT_REASON", null, null) },

            { "tblRECEIPT_SOURCE_CL",
                new Tuple<string, string, string, List<string>>("NAME", "ISN_RECEIPT_SOURCE", null, null) },

            { "tblREF_FILE",
                new Tuple<string, string, string, List<string>>("NAME", "ISN_REF_FILE", null, null) },

            { "tblREPRODUCTION_METHOD_CL",
                new Tuple<string, string, string, List<string>>("NAME", "ISN_REPRODUCTION_METHOD", null, null) },
            
            // { "tblService", ProcessService },
            
            { "tblSTATE_CL",
                new Tuple<string, string, string, List<string>>("NAME", "ISN_STATE", "ISN_HIGH_STATE", null) },

            { "tblSTORAGE_MEDIUM_CL",
                new Tuple<string, string, string, List<string>>("NAME", "ISN_STORAGE_MEDIUM", "ISN_HIGH_STORAGE_MEDIUM", null) },

            { "tblSUBJECT_CL",
                new Tuple<string, string, string, List<string>>("NAME", "ISN_SUBJECT", "ISN_HIGH_SUBJECT", null) },

            { "tblTREE_SUPPORT",
                new Tuple<string, string, string, List<string>>("ISN", null, null, null) },

            { "tblWORK_CL",
                new Tuple<string, string, string, List<string>>("NAME", "ISN_WORK", null, null) },

            { "rptFUND_PAPER",
                new Tuple<string, string, string, List<string>>("ISN_FUND", null, null, null) },

            { "rptFUND_UNIT_REG_STATS",
                new Tuple<string, string, string, List<string>>("ISN_FUND", null, null, null) },
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
        /// </summary>
        public static Dictionary<string, Tuple<string, string, string, string, string, List<string>, List<string>>> LinksTablesParams { get; } = new Dictionary<string, Tuple<string, string, string, string, string, List<string>, List<string>>>
        {
            { "tblORGANIZ_RENAME",
                new Tuple<string, string, string, string, string, List<string>, List<string>>("NAME", "ISN_ORGANIZ_RENAME", null, "ISN_ORGANIZ", null, null, null) },

            { "tblARCHIVE",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, "ISN_ARCHIVE", null, null, null, null, null) },

            { "tblLOCATION",
                new Tuple<string, string, string, string, string, List<string>, List<string>>("NOTE", "ISN_LOCATION", "ISN_HIGH_LOCATION", null, null, null, null) },

            { "tblARCHIVE_STORAGE",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, null, null, null, null, null, null) },

            //{ "tblARCHIVE_PASSPORT",
            //    new Tuple<string, string, string, string, string, List<string>, List<string>>("PASS_YEAR", "ISN_PASSPORT", null, null, null, null, null) },

            // ---In recalc---
            //{ "tblARCHIVE_STATS",
            //    new Tuple<string, string, string, string, string, List<string>, List<string>>(null, "ISN_ARCHIVE_STATS", null, "ISN_PASSPORT", null, null, null) },
            // -------

            { "tblFUND",
                new Tuple<string, string, string, string, string, List<string>, List<string>>("FUND_NAME_SHORT", "ISN_FUND", null, null, "FUND_NUM_2", null, null) },

            { "tblFUND_RENAME",
                new Tuple<string, string, string, string, string, List<string>, List<string>>("FUND_NAME_SHORT", "ISN_FUND_RENAME", null, "ISN_FUND", null, null, null) },

            { "tblFUND_CHECK",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, null, null, "ISN_FUND", null, null, null) },

            { "tblFUND_DOC_TYPE",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, null, null, "ISN_FUND", null, null, null) },

            { "tblFUND_INCLUSION",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, "ISN_INCLUSION", null, "ISN_FUND", null, null, null) },

            { "tblFUND_PAPER_CLS",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, null, null, "ISN_FUND", null, null, null) },

            { "tblPUBLICATION_CL",
                new Tuple<string, string, string, string, string, List<string>, List<string>>("PUBLICATION_NAME", "ISN_PUBLICATION", null, null, "PUBLICATION_NUM", null, null) },

            { "tblFUND_PUBLICATION",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, "ISN_PUBLICATION", null, "ISN_FUND", null, null, null) },

            { "tblFUND_RECEIPT_REASON",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, null, null, "ISN_FUND", null, null, null) },

            { "tblFUND_RECEIPT_SOURCE",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, null, null, "ISN_FUND", null, null, null) },

            { "tblFUND_OAF",
                new Tuple<string, string, string, string, string, List<string>, List<string>>("FUND_NAME_SHORT", "ISN_OAF", null, "ISN_FUND", "FUND_NUM_2", null, null) },

            { "tblFUND_OAF_REASON",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, null, null, "ISN_FUND", null, null, null) },

            { "tblFUND_COLLECTION_REASONS",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, "ISN_COLLECTION_REASON", null, "ISN_FUND", null, null, null) },

            { "tblFUND_CREATOR",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, "ISN_FUND_CREATOR", null, "ISN_FUND", null, null, null) },

            { "tblUNDOCUMENTED_PERIOD",
                new Tuple<string, string, string, string, string, List<string>, List<string>>("PERIOD_START_YEAR", "ISN_PERIOD", null, "ISN_FUND", null, null, null) },

            { "tblDEPOSIT",
                new Tuple<string, string, string, string, string, List<string>, List<string>>("DEPOSIT_NAME", "ISN_DEPOSIT", null, "ISN_FUND", "DEPOSIT_NUM", null, null) },

            { "tblDEPOSIT_DOC_TYPE",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, "ISN_DEPOSIT_DOC_TYPE", null, "ISN_DEPOSIT", null, null, null) },

            { "tblACT",
                new Tuple<string, string, string, string, string, List<string>, List<string>>("ACT_NAME", "ISN_ACT", null, "ISN_FUND", null, new List<string>() { "ACT_DATE", "ACT_NUM", "ACT_OBJ", "ISN_ACT_TYPE", "MOVEMENT_FLAG" }, null) },

            { "tblINVENTORY",
                new Tuple<string, string, string, string, string, List<string>, List<string>>("INVENTORY_NAME", "ISN_INVENTORY", null, "ISN_FUND", "INVENTORY_NUM_1", null, null) },

            { "tblINVENTORY_CHECK",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, null, null, "ISN_INVENTORY", null, null, null) },

            { "tblINVENTORY_DOC_STORAGE",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, null, null, "ISN_INVENTORY", null, null, null) },

            { "tblINVENTORY_DOC_TYPE",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, null, null, "ISN_INVENTORY", null, null, null) },

            { "tblINVENTORY_CLS_ATTR",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, null, null, "ISN_INVENTORY", null, null, null) },

            { "tblINVENTORY_GROUPING_ATTRIBUTE",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, null, null, "ISN_INVENTORY", null, null, null) },

            { "tblINVENTORY_PAPER_CLS",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, null, null, "ISN_INVENTORY", null, null, null) },

            { "tblINVENTORY_REQUIRED_WORK",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, null, null, "ISN_INVENTORY", null, null, null) },

            { "tblINVENTORY_STRUCTURE", // Есть уникальное по NAME, но оно не заполняется пользователем
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, "ISN_INVENTORY_CLS", "ISN_HIGH_INVENTORY_CLS", "ISN_INVENTORY", null, null, null) },

            { "tblDOCUMENT_STATS",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, "ISN_DOCUMENT_STATS", null, "ISN_FUND", null, null, null) },

            { "tblREF_ACT",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, "ISN_REF_ACT", null, "ISN_ACT", null, null, null) },

            { "tblREF_CLS",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, "ISN_REF_CLS", null, null, null, null, null) },

            { "tblREF_FEATURE",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, "ISN_REF_FEATURE", null, null, null, null, null) },

            { "tblREF_LANGUAGE",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, "ISN_REF_LANGUAGE", null, null, null, null, null) },

            { "tblREF_LOCATION",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, "ISN_REF_LOCATION", null, "ISN_LOCATION", null, null, null) },

            { "tblREF_QUESTION",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, "ISN_REF_QUESTION", null, "ISN_QUESTION", null, null, null) },

            { "tblUNIT", // "ISN_HIGH_UNIT"
                new Tuple<string, string, string, string, string, List<string>, List<string>>("NAME", "ISN_UNIT", null, "ISN_INVENTORY", null, new List<string>() { "UNIT_NUM_1", "UNIT_NUM_2", "WEIGHT", "PAGE_COUNT", "ALL_DATE" }, null) },

            { "tblUNIT_ELECTRONIC",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, null, null, "ISN_UNIT", null, null, null) },

            { "tblUNIT_FOTO",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, null, null, "ISN_UNIT", null, null, null) },

            { "tblUNIT_FOTO_EX",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, null, null, "ISN_UNIT", null, null, null) },

            { "tblUNIT_MICROFORM",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, null, null, "ISN_UNIT", null, null, null) },

            { "tblUNIT_MOVIE",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, null, null, "ISN_UNIT", null, null, null) },

            { "tblUNIT_MOVIE_EX",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, null, null, "ISN_UNIT", null, null, null) },

            { "tblUNIT_NTD",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, null, null, "ISN_UNIT", null, null, null) },

            { "tblUNIT_PHONO",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, null, null, "ISN_UNIT", null, null, null) },

            { "tblUNIT_REQUIRED_WORK",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, "ISN_UNIT_REQUIRED_WORK", null, "ISN_UNIT", null, null, null) },

            { "tblUNIT_STATE",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, "ISN_UNIT_STATE", null, "ISN_UNIT", null, new List<string>() { "STATE_DATE", "PAGE_NUMS", "PAGE_COUNT" }, null) },

            { "tblUNIT_VIDEO",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, null, null, "ISN_UNIT", null, null, null) },

            { "tblUNIT_VIDEO_EX",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, null, null, "ISN_UNIT", null, null, null) },

            { "tblUNIT_WORK",
                new Tuple<string, string, string, string, string, List<string>, List<string>>(null, "ISN_UNIT_WORK", null, "ISN_UNIT", null, null, null) },

            { "tblDOCUMENT",
                new Tuple<string, string, string, string, string, List<string>, List<string>>("NAME", "ISN_DOCUM", null, "ISN_UNIT", null, null, null) },
        };

        public static Dictionary<string, Tuple<string, string, string, string, string, List<string>>> RecalcTablesParams { get; } = new Dictionary<string, Tuple<string, string, string, string, string, List<string>>>
        {
            { "tblARCHIVE_STATS",
                new Tuple<string, string, string, string, string, List<string>>(null, "ISN_ARCHIVE_STATS", null, "ISN_PASSPORT", null, null) },
        };
    }
}
