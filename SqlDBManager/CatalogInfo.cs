using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlDBManager
{
    public class DBCatalog
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

        public Dictionary<int, List<string>> SelectColumnsData(List<string> columns, string table)
        {
            string request = SQLRequests.ColumnsRequest(columns, Catalog, table);
            return ReturnDictFromDB(request);
        }

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
            string request = SQLRequests.ClearTableRequest(Catalog, tableName);

            //SqlCommand command = new SqlCommand(request, connection);

            adapter.DeleteCommand = new SqlCommand(request, connection);
            string deletedCount = adapter.DeleteCommand.ExecuteNonQuery().ToString();

            adapter.Dispose();
            //command.Dispose();
            return deletedCount;
        }

        public Dictionary<int, List<string>> ReturnDictFromDB(string request)
        {
            SqlCommand command = new SqlCommand(request, connection);
            SqlDataReader reader = command.ExecuteReader();
            Dictionary<int, List<string>> dictTableData = new Dictionary<int, List<string>>();
            List<string> tableRowData = new List<string>();
            int rowNumber = 1;

            while (reader.Read())
            {
                for(int i = 0; i < reader.FieldCount; i++)
                {
                    tableRowData.Add(reader.GetString(i));
                }
                dictTableData.Add(rowNumber, tableRowData);
                rowNumber++;
            }

            reader.Close();
            command.Dispose();
            return dictTableData;
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
    }
}
