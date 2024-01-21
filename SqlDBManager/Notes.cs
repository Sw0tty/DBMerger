using SqlDBManager;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NotesNamespace
{
    public static class HelpFunction
    {
        public static string CreateSpace(int spaceSize)
        {
            string space = "";
            for (int i = 1; i <= spaceSize; i++)
                space += " ";
            return space;
        }
    }

    public static class VisualConsts
    {
        public const int SPACE_SIZE = 4;
    }

    public static class DefaultTablesValues
    {
        /// <summary>
        /// Наименование таблицы - фильтруемая колонка - ключи дефолтных значений
        /// </summary>
/*        public static Dictionary<string, Dictionary<string, List<string>>> DefaultTables { get; } = new Dictionary<string, Dictionary<string, List<string>>>()
        {
            { "tblPERIOD", new Dictionary<string, List<string>>() { { "ISN_PERIOD", new List<string>() { "1", "2", "3" } } } },
            { "tblSECURLEVEL", new Dictionary<string, List<string>>() { { "ISN_SECURLEVEL", new List<string>() { "1", "2", "3" } } } },
            { "tblSECURITY_REASON", new Dictionary<string, List<string>>() { { "ISN_SECURITY_REASON", new List<string>() { "1", "2", "3", "4", "8" } } } },
        };*/

        /// <summary>
        /// Наименование таблицы - (дефолтное значение для корректировки, фильтруемая колонка) - ключи дефолтных значений
        /// </summary>
        public static Dictionary<string, Tuple<string, Dictionary<string, List<string>>>> DefaultTables { get; } = new Dictionary<string, Tuple<string, Dictionary<string, List<string>>>>()
        {
            { "tblPERIOD", new Tuple<string, Dictionary<string, List<string>>>("3", new Dictionary<string, List<string>>() { { "ISN_PERIOD", new List<string>() { "1", "2", "3" } } }) },
            { "tblSECURLEVEL", new Tuple<string, Dictionary<string, List<string>>>("1", new Dictionary<string, List<string>>() { { "ISN_SECURLEVEL", new List<string>() { "1", "2", "3" } } }) },
            { "tblSECURITY_REASON", new Tuple<string, Dictionary<string, List<string>>>("null", new Dictionary<string, List<string>>() { { "ISN_SECURITY_REASON", new List<string>() { "1", "2", "3", "4", "8" } } }) },
        };
    }

    static public class Visualizator
    {
        const int VISUAL_LVL_TWO = 8;
        const int VISUAL_LVL_THREE = 12;
        const int VISUAL_LVL_FOUR = 16;

        /// <summary>
        /// Визуализирует входящие данные в резервный словарь в момент вызова визуализации
        /// </summary>
        static public void VisualizateReserv(Dictionary<string, List<Dictionary<string, List<Tuple<string, string>>>>> reloadDict)
        {
            //Dictionary<string, List<Dictionary<string, List<Tuple<string, string>>>>> reloadDict


            /*Dictionary<string, List<Dictionary<string, List<Tuple<string, string>>>>> reloadDict = new Dictionary<string, List<Dictionary<string, List<Tuple<string, string>>>>>();
            reloadDict["SOME_TABLE"] = new List<Dictionary<string, List<Tuple<string, string>>>>();
            reloadDict["SOME_TABLE"].Add(new Dictionary<string, List<Tuple<string, string>>>() { { "ISN_SOME", new List<Tuple<string, string>>() } });
            reloadDict["SOME_TABLE"].Add(new Dictionary<string, List<Tuple<string, string>>>() { { "ISN_SOME_2", new List<Tuple<string, string>>() { new Tuple<string, string>("1232132", "dfgdfgf"), new Tuple<string, string>("1232132", "dfgdfgf") } } });
*/

            if (reloadDict.Count == 0)
            {
                MessageBox.Show("словарь пуст");
            }
            else
            {
                //string wrapperKey = "";
                //string insideDictKey = "";
                //string insideDict = $"{insideDictKey}: " + "[\n" + "\n]";


                List<string> insideDicts = new List<string>();
                List<string> insideTuples = new List<string>();

                string dictWrapper = null;

                foreach (string key in reloadDict.Keys)
                {
                    string wrapperKey = key;
                    foreach (Dictionary<string, List<Tuple<string, string>>> keyColumn in reloadDict[key])
                    {
                        foreach (string insideKey in keyColumn.Keys)
                        {
                            string insideDictKey = insideKey;
                            foreach(Tuple<string, string> tuple in keyColumn[insideKey])
                            {
                                insideTuples.Add($"{HelpFunction.CreateSpace(VISUAL_LVL_THREE)}({tuple.Item1}, {tuple.Item2})");
                            }
                            string insideDict = $"{HelpFunction.CreateSpace(VISUAL_LVL_TWO)}{insideDictKey}: " + "[\n" + $"{string.Join(",\n", insideTuples)}" + $"\n{HelpFunction.CreateSpace(VISUAL_LVL_TWO)}]";
                            insideDicts.Add(insideDict);
                        }
                        dictWrapper = $"{wrapperKey}: " + "[\n" + $"{string.Join("\n", insideDicts)}" + "\n]";
                    }
                    MessageBox.Show(dictWrapper);
                }
            }
        }

        static public void VisualizateDefaultTables()
        { 

        }
    }


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