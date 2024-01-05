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
        public static string ClearString(string str)
        {
            return str.Trim(' ');
        }
    }

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
    }

    public static class SQLManager
    {
/*        protected string Catalog;

        public SQLManager(string catalog)
        {
            Catalog = catalog;
        }*/

        public static bool CheckConnection(string source, string catalog, string login, string password)
        {
            SqlCommand command;
            SqlDataReader reader;
            string request, connectionString, response = "";

            connectionString = $@"Data Source={source};Initial Catalog={catalog};User ID={login};Password={password};Connect Timeout=30";

            SqlConnection cnn = new SqlConnection(connectionString);

            try
            {
                SqlExtensions.QuickOpen(cnn, 30);

                if (catalog == "")
                {
                    request = "SELECT name FROM sys.databases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb');";

                    command = new SqlCommand(request, cnn);
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        response += Convert.ToString(reader.GetValue(0)) + " ";
                    }

                    reader.Close();
                    command.Dispose();
                    cnn.Close();
                    return false;
                }
                cnn.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static DialogResult CheckConnectionMessage(string source, string catalog, string login, string password)
        {

            SqlCommand command;
            SqlDataReader reader;
            string request, connectionString, response = "";

            connectionString = $@"Data Source={source};Initial Catalog={catalog};User ID={login};Password={password};Connect Timeout=30";

            SqlConnection cnn = new SqlConnection(connectionString);

            try
            {
                SqlExtensions.QuickOpen(cnn, 30);
                
                if (catalog == "")
                {
                    request = "SELECT name FROM sys.databases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb');";

                    command = new SqlCommand(request, cnn);
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        response += Convert.ToString(reader.GetValue(0)) + " ";
                    }

                    reader.Close();
                    command.Dispose();
                    cnn.Close();
                    return MessageBox.Show($"Соединение установлено, но не выбран каталог!\nДоступные каталоги: {response}",
                                            "Предупреждение",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Warning);
                }
                cnn.Close();
                return MessageBox.Show("Соединение установлено!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                return MessageBox.Show("Соединение не удалось!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
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