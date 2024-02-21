using SqlDBManager;
using SqlDBManager.DBClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace NotesNamespace
{
    public static class HelpFunction
    {
        public static List<string> Exclude(List<string> columns, List<string> excludeClumns)
        {
            foreach (string excludeColumn in excludeClumns)
            {
                columns.Remove(excludeColumn);
            }
            return columns;
        }

        public static string CreateSpace(int spaceSize)
        {
            string space = "";
            for (int i = 1; i <= spaceSize; i++)
                space += " ";
            return space;
        }
    }

    public class MergerPreSetting
    {
        private string ShortArchiveName { get; }
        private string FullArchiveName { get; }
        private string ArchiveAddress { get; }
        private string Description { get; }

        public MergerPreSetting(string shortArchiveName, string fullArchiveName, string archiveAddress, string description)
        {
            ShortArchiveName = shortArchiveName;
            FullArchiveName = fullArchiveName;
            ArchiveAddress = archiveAddress;
            Description = description;
        }
    }

    public static class DocStats
    {
        public static string SearchSecondParent(string nowSecondParentID, List<Tuple<string, string>> pairOfSecondParentID)
        {
            foreach (Tuple<string, string> pairID in pairOfSecondParentID)
            {
                if (nowSecondParentID == pairID.Item1)
                    return pairID.Item2;
            }
            return null;
        }
        public static string SearchDocID()
        {
            return null;
        }
    }
    public class S : BaseDBConnector
    {
        public S(string source, string catalog, string login, string password) : base(source, catalog, login, password){}
    }
    
    public static class RecalculationConsts
    {
        public static char Electronic = 'E';
        public static char Traditional = 'T';

        public static List<string> PaperFunds = new List<string>()
        {
            "'1'", "'2'", "'3'", "'4'",
        };

        public static List<string> TraditionalFunds = new List<string>()
        {
            "'5'", "'6'", "'7'", "'8'",
        };

        public static List<string> MicroformFunds = new List<string>()
        {
            "'9'",
        };
    }
    

    public static class SpecialTablesValues
    {
        public static Tuple<string, string> SpecialTablePair = new Tuple<string, string>("tblINVENTORY", "tblDOCUMENT_STATS");

        /// <summary>
        /// Наименование таблицы - (дефолтное значение для корректировки, фильтруемая колонка) - ключи дефолтных значений
        /// </summary>
        public static Dictionary<string, Tuple<string, Dictionary<string, List<string>>>> DefaultTables { get; } = new Dictionary<string, Tuple<string, Dictionary<string, List<string>>>>()
        {
            { "tblPERIOD", new Tuple<string, Dictionary<string, List<string>>>("3", new Dictionary<string, List<string>>() { { "ISN_PERIOD", new List<string>() { "1", "2", "3" } } }) },
            { "tblSECURLEVEL", new Tuple<string, Dictionary<string, List<string>>>("1", new Dictionary<string, List<string>>() { { "ISN_SECURLEVEL", new List<string>() { "1", "2", "3" } } }) },
            { "tblSECURITY_REASON", new Tuple<string, Dictionary<string, List<string>>>("null", new Dictionary<string, List<string>>() { { "ISN_SECURITY_REASON", new List<string>() { "1", "2", "3", "4", "8" } } }) },
        };

        public static Dictionary<string, Tuple<string, string>> WithoutKeysTables { get; } = new Dictionary<string, Tuple<string, string>>()
        {
            { "tblACT", new Tuple<string, string>("tblFUND", "ISN_FUND") },
            { "tblINVENTORY_STRUCTURE", new Tuple<string, string>("tblINVENTORY", "ISN_INVENTORY") },
        };

        public static Dictionary<string, Tuple<Tuple<string, string>, Tuple<string, string>>> RenamedColumns { get; } = new Dictionary<string, Tuple<Tuple<string, string>, Tuple<string, string>>>()
        { // Before Merge, After Merge
            { "tblINVENTORY", new Tuple<Tuple<string, string>, Tuple<string, string>>(new Tuple<string, string>("ISN_INVENTORY_STORAGE", "ISN_STORAGE_MEDIUM"), new Tuple<string, string>("ISN_STORAGE_MEDIUM", "ISN_INVENTORY_STORAGE")) },
            //{ "tblREF_FILE", new Tuple<Tuple<string, string>, Tuple<string, string>>(new Tuple<string, string>("ISN_OBJ", "ISN_FUND"), new Tuple<string, string>("ISN_FUND", "ISN_OBJ")) },
        };

        public static List<string> DefaultUsers { get; } = new List<string>() { "sa", "anonymous", "admin", "reader", "arch", "tech" };
    }


    //   FROM [TestDB].[dbo].[tblFUND] where CreationDateTime <= '{passportYear}0101 00:00:00.000'

    /*public class CatalogInfo
    {
        protected List<string> logTables;
        protected List<string> defaultTables;
        protected List<string> linksTables;

        public CatalogInfo()
        {
            logTables = SelectLogTables();
            defaultTables = SelectDefaultTables();
            linksTables = SelectLinksTables();
        }

        public List<string> SelectLogTables()
        {
            List<string> logTables = new List<string>();
            return new List<string>();
        }

        public List<string> SelectDefaultTables()
        {
            List<string> list = new List<string>();
            return new List<string>();
        }

        public List<string> SelectLinksTables()
        {
            List<string> list = new List<string>();
            return new List<string>();
        }
    }*/

    // System.Data.SqlClient.SqlException

    /*public class DBCatalog
    {
        protected string Source;
        protected string Catalog;
        protected string Login;
        protected string Password;
        protected string connectionString;
        protected SqlConnection connection;

        public DBCatalog(string source, string catalog, string login, string password)
        {
            Source = source;
            Catalog = catalog;
            Login = login;
            Password = password;
            connectionString = $@"Data Source={Source};Initial Catalog={Catalog};User ID={Login};Password={Password};Connect Timeout=30";
            connection = new SqlConnection(connectionString);
        }

        public void OpenConnection()
        {
            connection.Open();
        }

        public void CloseConnection()
        {
            connection.Close();
        }

        public int SelectCountTables()
        {
            string request = SQLRequests.CountTablesRequest(Catalog);
            SqlCommand command = new SqlCommand(request, connection);
            SqlDataReader reader = command.ExecuteReader();

            reader.Read();
            int count = Convert.ToInt32(reader.GetValue(0));
            reader.Close();
            command.Dispose();
            return count;
        }

        public int SelectCountRowsTable(string table)
        {
            string request = SQLRequests.CountRowsRequest(Catalog, table);
            SqlCommand command = new SqlCommand(request, connection);
            SqlDataReader reader = command.ExecuteReader();

            reader.Read();
            int count = Convert.ToInt32(reader.GetValue(0));
            reader.Close();
            command.Dispose();
            return count;
        }

        public List<string> SelectTablesNames()
        {
            string request = SQLRequests.AllTablesRequest(Catalog);
            return ReturnListFromDB(request);
        }

        public List<string> SelectLogTables()
        {
            string request = SQLRequests.LogTablesRequest(Catalog);
            return ReturnListFromDB(request);
        }

        public List<string> SelectDefaultSkipTables()
        {
            // Дефолтные таблицы на скип
            string request = SQLRequests.SkipRequest(Catalog);
            return ReturnListFromDB(request);
        }

        public List<string> SelectDefaultProcessingTables()
        {
            // Дефолтные таблицы на обработку
            string request = SQLRequests.ProcessingRequest(Catalog);
            return ReturnListFromDB(request);
        }

*//*        public List<string> SelectDefaultTables()
        {
            string request = $"SELECT name FROM sys.tables WHERE OBJECTPROPERTY(object_id, 'TableHasForeignKey') = 0 and name not like '%log' and name not in ('tblPUBLICATION_CL', 'tblUNIT_FOTO_EX', 'tblUNIT_MOVIE_EX', 'tblUNIT_VIDEO_EX') or name in ('tblFEATURE', 'tblDOC_KIND_CL', 'tblSTATE_CL', 'tblSTORAGE_MEDIUM_CL', 'tblSUBJECT_CL', 'tblCLS', 'tblABSENCE_REASON_CL', 'tblQUESTION') ORDER BY name;";
            return ReturnListFromDB(request, connection);
        }*//*

        public List<string> ReturnLinksTables()
        {
            List<string> linksTables = new List<string>()
            {
                "tblORGANIZ_RENAME",
                "tblARCHIVE",
                "tblLOCATION",
                "tblARCHIVE_STORAGE",
                "tblARCHIVE_PASSPORT",
                "tblARCHIVE_STATS",
                "tblFUND",
                "tblFUND_RENAME",
                "tblFUND_CHECK",
                "tblFUND_DOC_TYPE",
                "tblFUND_INCLUSION",
                "tblFUND_PAPER_CLS",
                "tblPUBLICATION_CL",
                "tblFUND_PUBLICATION",
                "tblFUND_RECEIPT_REASON",
                "tblFUND_RECEIPT_SOURCE",
                "tblFUND_OAF",
                "tblFUND_OAF_REASON",
                "tblFUND_COLLECTION_REASONS",
                "tblFUND_CREATOR",
                "tblUNDOCUMENTED_PERIOD",
                "tblDEPOSIT",
                "tblDEPOSIT_DOC_TYPE",
                "tblACT",
                "tblINVENTORY",
                "tblINVENTORY_CHECK",
                "tblINVENTORY_DOC_STORAGE",
                "tblINVENTORY_DOC_TYPE",
                "tblINVENTORY_CLS_ATTR",
                "tblINVENTORY_GROUPING_ATTRIBUTE",
                "tblINVENTORY_PAPER_CLS",
                "tblINVENTORY_REQUIRED_WORK",
                "tblINVENTORY_STRUCTURE",
                "tblDOCUMENT_STATS",
                "tblREF_ACT",
                "tblREF_CLS",
                "tblREF_FEATURE",
                "tblREF_LANGUAGE",
                "tblREF_LOCATION",
                "tblREF_QUESTION",
                "tblUNIT",
                "tblUNIT_ELECTRONIC",
                "tblUNIT_FOTO",
                "tblUNIT_FOTO_EX",
                "tblUNIT_MICROFORM",
                "tblUNIT_MOVIE",
                "tblUNIT_MOVIE_EX",
                "tblUNIT_NTD",
                "tblUNIT_PHONO",
                "tblUNIT_REQUIRED_WORK",
                "tblUNIT_STATE",
                "tblUNIT_VIDEO",
                "tblUNIT_VIDEO_EX",
                "tblUNIT_WORK",
                "tblDOCUMENT"
            };
            return linksTables;
        }

        public string ClearTable(string tableName)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            string request = $"DELETE [{Catalog}].[dbo].[{tableName}]";

            //SqlCommand command = new SqlCommand(request, connection);

            adapter.DeleteCommand = new SqlCommand(request, connection);
            string deletedCount = adapter.DeleteCommand.ExecuteNonQuery().ToString();

            adapter.Dispose();
            //command.Dispose();
            return deletedCount;
        }

        public List<string> ReturnListFromDB(string request)
        {
            SqlCommand command = new SqlCommand(request, connection);
            SqlDataReader reader = command.ExecuteReader();
            List<string> listTablesNames = new List<string>();

            while (reader.Read())
            {
                listTablesNames.Add(reader.GetString(0));
            }

            reader.Close();
            command.Dispose();
            return listTablesNames;
        }

        public bool ValidateCountTables(int countAnotherCatalogTables)
        {
            if (countAnotherCatalogTables == SelectCountTables())
                return true;
            return false;
        }

        public bool ValidateNamesTables(List<string> anotherCatalogNamesTables)
        {
            List<string> mainCatalogNamesTables = SelectTablesNames();

            foreach (string anotherTablesName in anotherCatalogNamesTables)
            {
                if (mainCatalogNamesTables.Contains(anotherTablesName))
                {
                    continue;
                }
                return false;
            }
            return true;
        }
    }*/
}


/*

Запросы

--Таблицы С внешними ключами

SELECT name AS [Tables]
FROM sys.tables
WHERE OBJECTPROPERTY(object_id, 'TableHasForeignKey') = 1
ORDER BY[Tables];


--Таблицы БЕЗ внешних ключей

SELECT name AS [Tables]
FROM sys.tables
WHERE OBJECTPROPERTY(object_id, 'TableHasForeignKey') = 0
ORDER BY[Tables];


-- Все таблицы из выбранного каталога БД

$"SELECT TABLE_NAME FROM {catalog}.INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' order by TABLE_NAME";


-- Отсортированный список обработки таблиц
1. Таблицы на очистку (всего 6)
SELECT TABLE_NAME FROM {catalog}.INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'  and TABLE_NAME like '%log' order by TABLE_NAME

2. Таблицы без внешних ключей и с ключами сами на себя (всего 101)
SELECT name AS [Tables]
FROM sys.tables
WHERE OBJECTPROPERTY(object_id, 'TableHasForeignKey') = 0 and name not like '%log' and name not in ('tblPUBLICATION_CL', 'tblUNIT_FOTO_EX', 'tblUNIT_MOVIE_EX', 'tblUNIT_VIDEO_EX') or name in ('tblFEATURE', 'tblDOC_KIND_CL', 'tblSTATE_CL', 'tblSTORAGE_MEDIUM_CL', 'tblSUBJECT_CL', 'tblCLS', 'tblABSENCE_REASON_CL', 'tblQUESTION')
ORDER BY [Tables];


3. Таблицы с ключами на дефолтные таблицы (всего 55)

tblORGANIZ_RENAME - После tblORGANIZ_CL . Добавить новые записи, если такие есть в дочерней.
tblARCHIVE - обработка после tblSUBJECT_CL. Берется название из основной таблицы (предложить переменование перед слиянием).
tblLOCATION - обработка после tblARCHIVE
tblARCHIVE_STORAGE - обработка после tblLOCATION (?)
tblARCHIVE_PASSPORT - обработка после tblARCHIVE. Вероятно стереть. Пока что оставить все из главной
tblARCHIVE_STATS - обработка после tblARCHIVE_PASSPORT, tblDOC_TYPE_CL пока оставить записи из главной БД
tblFUND - обработка после таблиц tblARCHIVE, tblDOC_TYPE_CL, tblPERIOD, tblSECURITY_REASON, tblSECURLEVEL
tblFUND_RENAME - обработка после tblFUND
tblFUND_CHECK - обработка после tblFUND
tblFUND_DOC_TYPE - обработка после tblFUND, tblDOC_TYPE_CL 
tblFUND_INCLUSION - (?) обработка после tblFUND, tblCITIZEN_CL, tblORGANIZ_CL
tblFUND_PAPER_CLS - обработка после tblFUND, tblPAPER_CLS
tblPUBLICATION_CL - обработка после tblPUBLICATION_TYPE_CL Добавить новые записи, если такие есть в дочерней.
tblFUND_PUBLICATION - обработка после tblFUND, tblPUBLICATION_CL
tblFUND_RECEIPT_REASON - обработка после tblFUND, tblRECEIPT_REASON_CL
tblFUND_RECEIPT_SOURCE - обработка после tblFUND, tblRECEIPT_SOURCE_CL
tblFUND_OAF - (?) обработка после tblFUND
tblFUND_OAF_REASON - (?) обработка после tblFUND, tblOAF_REASON_CL
tblFUND_COLLECTION_REASONS - обработка после tblFUND, tblCOLLECTION_REASON_CL
tblFUND_CREATOR - (?) обработка после tblCITIZEN_CL, tblFUND, tblORGANIZ_CL
tblUNDOCUMENTED_PERIOD - обработка после tblFUND, tblABSENCE_REASON_CL
tblDEPOSIT - (?) обработка после tblFUND
tblDEPOSIT_DOC_TYPE - (?)  обработка после tblDEPOSIT, tblDOC_TYPE_CL
tblACT - обработка после tblFUND, tblACT_TYPE_CL. Уникальность по 
tblINVENTORY - обработка после tblFUND, tblDOC_KIND_CL, tblDOC_TYPE_CL, tblINV_REQUIRED_WORK_CL, tblREPRODUCTION_METHOD_CL, tblRECEIPT_REASON_CL, tblSTORAGE_MEDIUM_CL, tblRECEIPT_SOURCE_CL, tblSECURLEVEL, tblSECURITY_REASON
tblINVENTORY_CHECK - обработка после tblINVENTORY 
tblINVENTORY_DOC_STORAGE - обработка после tblINVENTORY, tblSTORAGE_MEDIUM_CL
tblINVENTORY_DOC_TYPE - обработка после tblINVENTORY, tblDOC_TYPE_CL 
tblINVENTORY_CLS_ATTR - обработка после tblINVENTORY, tblCLS
tblINVENTORY_GROUPING_ATTRIBUTE - обработка после tblINVENTORY, tblGROUPING_ATTRIBUTE_CL
tblINVENTORY_PAPER_CLS - обработка после tblINVENTORY, tblPAPER_CLS_INV
tblINVENTORY_REQUIRED_WORK - обработка после tblINVENTORY, tblINV_REQUIRED_WORK_CL
tblINVENTORY_STRUCTURE - обработка после tblINVENTORY
tblDOCUMENT_STATS - обработка после tblFUND, tblDOC_TYPE_CL, tblINVENTORY 
tblREF_ACT - обработка после tblACT Добавить новые записи, если такие есть
tblREF_CLS - обработка после tblCLS
tblREF_FEATURE - обработка после tblFEATURE
tblREF_LANGUAGE - обработка после tblLANGUAGE_CL
tblREF_LOCATION - обработка после tblLOCATION
tblREF_QUESTION - (?) обработка после tblQUESTION, tblSUBJECT_CL Добавить новые записи, если такие есть в дочерней.
tblUNIT - обработка после tblDOC_KIND_CL, tblDOC_TYPE_CL, tblINVENTORY, tblINVENTORY_STRUCTURE, tblLOCATION, tblSECURITY_REASON, tblSECURLEVEL, tblSTORAGE_MEDIUM_CL
tblUNIT_ELECTRONIC - обработка после tblUNIT
tblUNIT_FOTO - обработка после tblUNIT, tblDOC_KIND_CL
tblUNIT_FOTO_EX - (?) обработка после tblUNIT
tblUNIT_MICROFORM - обработка после tblUNIT
tblUNIT_MOVIE - обработка после tblUNIT, tblDOC_KIND_CL
tblUNIT_MOVIE_EX - (?) обработка после tblUNIT
tblUNIT_NTD - обработка после tblUNIT, tblDOC_KIND_CL
tblUNIT_PHONO - обработка после tblUNIT, tblDOC_KIND_CL
tblUNIT_REQUIRED_WORK - обработка после tblUNIT, tblREF_ACT, tblWORK_CL 
tblUNIT_STATE - обработка после tblUNIT, tblREF_ACT, tblSTATE_CL
tblUNIT_VIDEO - обработка после tblUNIT, tblDOC_KIND_CL 
tblUNIT_VIDEO_EX - (?) после tblUNIT
tblUNIT_WORK - обработка после tblUNIT, tblWORK_CL Добавить новые записи, если такие есть в дочерней.
tblDOCUMENT - обработка после tblSECURLEVEL, tblDOC_KIND_CL, tblUNIT, tblREPRODUCTION_METHOD_CL


*/

/*


------Tables_to_clean_up------

1. Таблицы в названии которых есть LOG



------Default_tables------

Запросом у которых нет внешних ключей


------Tables_with_links------

 
 
 */

/*

// Словарь дефолтных (без внешних ключей) таблиц и функций для их обработки
public static Dictionary<string, Func<DBCatalog, DBCatalog, string, int>> defaultTablesFunctions = new Dictionary<string, Func<DBCatalog, DBCatalog, string, int>> {
            { "eqUsers", ProcessUsers }, // ProcessTBLUsers
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
            { "tblSUBJECT_CL", ProcessSUBJECT_CL },
            { "tblTREE_SUPPORT", ProcessTREE_SUPPORT },
            { "tblWORK_CL", ProcessWORK_CL },
            { "rptFUND_PAPER", ProcessFUND_PAPER },
            { "rptFUND_UNIT_REG_STATS", ProcessFUND_UNIT_REG_STATS },
        };


// --- Process Functions for DafaultTables ---
static int ProcessUsers(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
{
    string idLikeColumnName = null;
    string uniqueColumnName = "Login";
    return ProcessDefaultTable(mainCatalog, daughterCatalog, uniqueColumnName, idLikeColumnName, tableName, excludeColumns: new List<string>() { "DisplayName" });
}

static int ProcessActTypeCL(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
{
    string idLikeColumnName = "ISN_ACT_TYPE";
    string uniqueColumnName = "NAME";
    //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_ACT_TYPE]", "[CODE]", "[NAME]", "[NOTE]", "[PROTECTED]", "[WEIGHT]"  };
    return ProcessDefaultTable(mainCatalog, daughterCatalog, uniqueColumnName, idLikeColumnName, tableName);
}

static int ProcessAuthorizedDep(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
{
    string idLikeColumnName = "ISN_AuthorizedDep";
    string uniqueColumnName = "ShortName";
    //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_AuthorizedDep]", "[ShortName]", "[FullName]", "[Address]", "[District]" };
    return ProcessDefaultTable(mainCatalog, daughterCatalog, uniqueColumnName, idLikeColumnName, tableName);
}

static int ProcessCLS(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
{
    string highLevelColumnName = "ISN_HIGH_CLS";
    string idLikeColumnName = "ISN_CLS";
    string uniqueColumnName = "NAME";
    //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_CLS]", "[ISN_HIGH_CLS]", "[CODE]", "[WEIGHT]", "[NAME]", "[OBJ_KIND]", "[MULTISELECT]", "[NOTE]", "[FOREST_ELEM]", "[PROTECTED]" };
    return ProcessDefaultTable(mainCatalog, daughterCatalog, uniqueColumnName, idLikeColumnName, tableName, highLevelColumnName: highLevelColumnName);
}

static int ProcessDataExport(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
{
    string idLikeColumnName = null;
    string uniqueColumnName = "fcDbName";
    //List<string> forImportColumns = new List<string>() { "[ID]", "[StatusID]", "[OwnerID]", "[CreationDateTime]", "[Deleted]", "[fcDbName]", "[fcDbBacPath]", "[isZiped]" };
    return ProcessDefaultTable(mainCatalog, daughterCatalog, uniqueColumnName, idLikeColumnName, tableName);
}

static int ProcessDECL_COMMISSION_CL(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
{
    string idLikeColumnName = "ISN_COMMISSION";
    string uniqueColumnName = "NAME";
    //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_COMMISSION]", "[CODE]", "[NAME_SHORT]", "[NAME]", "[CREATE_DATE]", "[DELETE_DATE]", "[NOTE]", "[PROTECTED]", "[WEIGHT]" };
    return ProcessDefaultTable(mainCatalog, daughterCatalog, uniqueColumnName, idLikeColumnName, tableName);
}

static int ProcessGROUPING_ATTRIBUTE_CL(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
{
    string idLikeColumnName = "ISN_GROUPING_ATTRIBUTE";
    string uniqueColumnName = "NAME";
    //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_GROUPING_ATTRIBUTE]", "[CODE]", "[NAME]", "[NOTE]", "[PROTECTED]", "[WEIGHT]" };
    return ProcessDefaultTable(mainCatalog, daughterCatalog, uniqueColumnName, idLikeColumnName, tableName);
}

static int ProcessINV_REQUIRED_WORK_CL(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
{
    string idLikeColumnName = "ISN_REQUIRED_WORK";
    string uniqueColumnName = "NAME";
    //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_REQUIRED_WORK]", "[CODE]", "[NAME]", "[NOTE]", "[PROTECTED]", "[WEIGHT]" };
    return ProcessDefaultTable(mainCatalog, daughterCatalog, uniqueColumnName, idLikeColumnName, tableName);
}

static int ProcessLANGUAGE_CL(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
{
    string idLikeColumnName = "ISN_LANGUAGE";
    string uniqueColumnName = "NAME";
    //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_LANGUAGE]", "[CODE]", "[NAME]", "[NOTE]", "[PROTECTED]", "[WEIGHT]" };
    return ProcessDefaultTable(mainCatalog, daughterCatalog, uniqueColumnName, idLikeColumnName, tableName);
}

static int ProcessFEATURE(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
{
    string highLevelColumnName = "ISN_HIGH_FEATURE";
    string idLikeColumnName = "ISN_FEATURE";
    string uniqueColumnName = "NAME";
    //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_FEATURE]", "[ISN_HIGH_FEATURE]", "[CODE]", "[NAME]", "[NOTE]", "[FOREST_ELEM]", "[PROTECTED]", "[WEIGHT]" };
    return ProcessDefaultTable(mainCatalog, daughterCatalog, uniqueColumnName, idLikeColumnName, tableName, highLevelColumnName: highLevelColumnName);
}

static int ProcessCITIZEN_CL(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
{
    string idLikeColumnName = "ISN_CITIZEN";
    string uniqueColumnName = "NAME";
    //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_CITIZEN]", "[NAME]", "[TITLE]", "[RELATIONSHIP]", "[LAST_FAMILY_NAME]", "[NICKNAME]", "[BIRTH_DATE]", "[DEATH_DATE]", "[PROFESSION]", "[POST]", "[DEGREE]", "[MILITARY_RANK]", "[HONORARY_RANK]", "[NOTE]", "[WEIGHT]" };
    return ProcessDefaultTable(mainCatalog, daughterCatalog, uniqueColumnName, idLikeColumnName, tableName);
}

static int ProcessORGANIZ_CL(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
{
    string idLikeColumnName = "ISN_ORGANIZ";
    string uniqueColumnName = "NAME";
    //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_ORGANIZ]", "[NAME]", "[CODE]", "[CREATE_YEAR]", "[CREATE_YEAR_INEXACT]", "[DELETE_YEAR]", "[DELETE_YEAR_INEXACT]", "[ADDRESS]", "[CEO_NAME]", "[ARCHIVIST_NAME]", "[CEO_PHONE]", "[ARCHIVIST_PHONE]", "[ARCHIVE_REGULATIONS]", "[HAS_EPK]", "[DELO_INSTRUCTION_YEAR]", "[WORKER_COUNT]", "[APPROVED_NOM]", "[KEEPING_PLACE]", "[NOTE]", "[WEIGHT]" };
    return ProcessDefaultTable(mainCatalog, daughterCatalog, uniqueColumnName, idLikeColumnName, tableName);
}

static int ProcessPAPER_CLS(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
{
    string idLikeColumnName = "ISN_PAPER_CLS";
    string uniqueColumnName = "NAME";
    //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_PAPER_CLS]", "[CLS_TYPE]", "[CODE]", "[NAME]", "[TYPEID]", "[SCOPE]", "[NOTE]", "[PROTECTED]", "[WEIGHT]" };
    return ProcessDefaultTable(mainCatalog, daughterCatalog, uniqueColumnName, idLikeColumnName, tableName);
}

static int ProcessPAPER_CLS_INV(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
{
    string idLikeColumnName = "ISN_PAPER_CLS_INV";
    string uniqueColumnName = "NAME";
    //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_PAPER_CLS_INV]", "[CODE]", "[NAME]", "[NOTE]", "[PROTECTED]", "[WEIGHT]" };
    return ProcessDefaultTable(mainCatalog, daughterCatalog, uniqueColumnName, idLikeColumnName, tableName);
}

static int ProcessPUBLICATION_TYPE_CL(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
{
    string idLikeColumnName = "ISN_PUBLICATION_TYPE";
    string uniqueColumnName = "NAME";
    //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_PUBLICATION_TYPE]", "[CODE]", "[NAME]", "[NOTE]", "[PROTECTED]", "[WEIGHT]" };
    return ProcessDefaultTable(mainCatalog, daughterCatalog, uniqueColumnName, idLikeColumnName, tableName);
}

static int ProcessQUESTION(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
{
    string idLikeColumnName = "ISN_QUESTION";
    string uniqueColumnName = "NAME";
    //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_QUESTION]", "[NAME]" };
    return ProcessDefaultTable(mainCatalog, daughterCatalog, uniqueColumnName, idLikeColumnName, tableName);
}

static int ProcessRECEIPT_REASON_CL(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
{
    string idLikeColumnName = "ISN_RECEIPT_REASON";
    string uniqueColumnName = "NAME";
    //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_RECEIPT_REASON]", "[CODE]", "[NAME]", "[NOTE]", "[PROTECTED]", "[WEIGHT]" };
    return ProcessDefaultTable(mainCatalog, daughterCatalog, uniqueColumnName, idLikeColumnName, tableName);
}

static int ProcessRECEIPT_SOURCE_CL(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
{
    string idLikeColumnName = "ISN_RECEIPT_SOURCE";
    string uniqueColumnName = "NAME";
    //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_RECEIPT_SOURCE]", "[CODE]", "[NAME]", "[NOTE]", "[PROTECTED]", "[WEIGHT]" };
    return ProcessDefaultTable(mainCatalog, daughterCatalog, uniqueColumnName, idLikeColumnName, tableName);
}

static int ProcessREF_FILE(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
{
    string idLikeColumnName = "ISN_REF_FILE";
    string uniqueColumnName = "NAME";
    //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[DocID]", "[RowID]", "[ISN_REF_FILE]", "[ISN_OBJ]", "[KIND]", "[GR_STORAGE]", "[NAME]", "[CATEGORY]", "[WEIGHT]" };
    return ProcessDefaultTable(mainCatalog, daughterCatalog, uniqueColumnName, idLikeColumnName, tableName);
}

static int ProcessREPRODUCTION_METHOD_CL(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
{
    string idLikeColumnName = "ISN_REPRODUCTION_METHOD";
    string uniqueColumnName = "NAME";
    //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_REPRODUCTION_METHOD]", "[CODE]", "[NAME]", "[NOTE]", "[PROTECTED]", "[WEIGHT]" };
    return ProcessDefaultTable(mainCatalog, daughterCatalog, uniqueColumnName, idLikeColumnName, tableName);
}

static int ProcessSTATE_CL(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
{
    string highLevelColumnName = "ISN_HIGH_STATE";
    string idLikeColumnName = "ISN_STATE";
    string uniqueColumnName = "NAME";
    //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_STATE]", "[ISN_HIGH_STATE]", "[CODE]", "[NAME]", "[NOTE]", "[FOREST_ELEM]", "[PROTECTED]", "[WEIGHT]" };
    return ProcessDefaultTable(mainCatalog, daughterCatalog, uniqueColumnName, idLikeColumnName, tableName, highLevelColumnName: highLevelColumnName);
}

static int ProcessSTORAGE_MEDIUM_CL(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
{
    string highLevelColumnName = "ISN_HIGH_STORAGE_MEDIUM";
    string idLikeColumnName = "ISN_STORAGE_MEDIUM";
    string uniqueColumnName = "NAME";
    //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_STORAGE_MEDIUM]", "[ISN_HIGH_STORAGE_MEDIUM]", "[CODE]", "[NAME]", "[FOREST_ELEM]", "[NOTE]", "[PROTECTED]", "[WEIGHT]" };
    return ProcessDefaultTable(mainCatalog, daughterCatalog, uniqueColumnName, idLikeColumnName, tableName, highLevelColumnName: highLevelColumnName);
}

static int ProcessSUBJECT_CL(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
{
    string highLevelColumnName = "ISN_HIGH_SUBJECT";
    string idLikeColumnName = "ISN_SUBJECT";
    string uniqueColumnName = "NAME";
    //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_SUBJECT]", "[ISN_HIGH_SUBJECT]", "[CODE]", "[NAME]", "[NOTE]", "[FOREST_ELEM]", "[PROTECTED]", "[WEIGHT]" };
    return ProcessDefaultTable(mainCatalog, daughterCatalog, uniqueColumnName, idLikeColumnName, tableName, highLevelColumnName: highLevelColumnName);
}

static int ProcessTREE_SUPPORT(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
{
    string idLikeColumnName = null;
    string uniqueColumnName = "ISN";
    //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN]", "[DUE]", "[WDUE]" };
    return ProcessDefaultTable(mainCatalog, daughterCatalog, uniqueColumnName, idLikeColumnName, tableName);
}

static int ProcessWORK_CL(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
{
    string idLikeColumnName = "ISN_WORK";
    string uniqueColumnName = "NAME";
    //List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_WORK]", "[CODE]", "[NAME]", "[NOTE]", "[PROTECTED]", "[WEIGHT]" };
    return ProcessDefaultTable(mainCatalog, daughterCatalog, uniqueColumnName, idLikeColumnName, tableName);
}

static int ProcessFUND_PAPER(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
{
    string idLikeColumnName = null;
    string uniqueColumnName = "ISN_FUND";
    //List<string> forImportColumns = new List<string>() { "[ISN_FUND]", "[SORT_ORDER]", "[TEXT]" };
    return ProcessDefaultTable(mainCatalog, daughterCatalog, uniqueColumnName, idLikeColumnName, tableName);
}

static int ProcessFUND_UNIT_REG_STATS(DBCatalog mainCatalog, DBCatalog daughterCatalog, string tableName)
{
    string idLikeColumnName = null;
    string uniqueColumnName = "ISN_FUND";
    //List<string> forImportColumns = new List<string>() { "[ISN_FUND]", "[UNIT_COUNT]", "[REG_UNIT]", "[TEXT]" };
    return ProcessDefaultTable(mainCatalog, daughterCatalog, uniqueColumnName, idLikeColumnName, tableName);
}
// -------------------------------------
// last check, with_high: 5*/



/*
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
        };*/

/*
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
    *//* List<string> forImportColumns = new List<string>() { "[ID]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Deleted]", "[ISN_FUND]", "[ISN_ARCHIVE]", "[FUND_NUM_1]", "[FUND_NUM_2]", "[FUND_NUM_3]", "[ISN_DOC_TYPE]", "[FUND_CATEGORY]", "[ISN_PERIOD]", "[FUND_KIND]", "[FUND_NAME_SHORT]", "[FUND_NAME_FULL]", "[INVENTORY_COUNT]", "[AUTO_INVENTORY_COUNT]", "[DOC_START_YEAR]", "[DOC_START_YEAR_INEXACT]", "[DOC_END_YEAR]", "[DOC_END_YEAR_INEXACT]", "[DOC_RECEIPT_YEAR]", "[LAST_CHECKED_YEAR]", "[LAST_DOC_CHECK_YEAR]", "[IS_IN_SEARCH]", "[IS_LOST]", "[ANNOTATE]", "[PROPERTY]", "[PRESENCE_FLAG]", "[ABSENCE_REASON]", "[MOVEMENT_NOTE]", "[HAS_MUSEUM_ITEMS]", "[TREASURE_UNITS_COUNT]", "[HAS_UNDOCUMENTED_PERIODS]", "[HAS_INCLUSIONS]", "[WAS_RENAMED]", "[WEIGHT]", "[KEEP_PERIOD]", "[ISN_SECURLEVEL]", "[SECURITY_CHAR]", "[SECURITY_REASON]", "[ISN_OAF]", "[OAF_NOTE]", "[CARD_COUNT]", "[ARCHIVE_DB_COUNT]", "[FUND_DB_COUNT]", "[INNER_DB_COUNT]", "[LIST_COUNT]", "[PERSONAL_UNDESCRIBED_DOC_COUNT]", "[HAS_ELECTRONIC_DOCS]", "[HAS_TRADITIONAL_DOCS]", "[CARRIER_TYPE]", "[UNDESCRIBED_DOC_COUNT]",
"[UNDECSRIBED_PAGE_COUNT]", "[JOIN_REASON]", "[ADDITIONAL_NSA]", "[KEYWORDS]", "[FUND_HISTORY]", "[NOTE]", "[INVENTORY_STATE]", "[ISN_SECURITY_REASON]", "[ForbidRecalc]" };
*//*
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
*/






