using NotesNamespace;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SqlDBManager
{
    public static class MergeManager
    {
        public static Dictionary<string, Func<DBCatalog, DBCatalog, int>> tablesFunctions = new Dictionary<string, Func<DBCatalog, DBCatalog, int>> {
            { "eqUsers", ProcessTBLUsers },
        };

        public static void ClearLogs(DBCatalog mainCatalog, ListBox listBox1)
        {
            listBox1.Items.Add("--- Очистка логов ---");
            foreach(string logTable in mainCatalog.SelectLogTables())
            {
                listBox1.Items.Add($"Очистка {logTable}: {mainCatalog.ClearTable(logTable)} записей удалено.");
            }
        }

        public static void ProcessDefaultTables(DBCatalog mainCatalog, DBCatalog daughterCatalog, ListBox listBox1)
        {
            listBox1.Items.Add("--- Обработка дефолтных таблиц ---");
            foreach (string defaultTable in mainCatalog.SelectDefaultSkipTables())
            {
                listBox1.Items.Add($"{defaultTable}: default");
            }

            CheckDefaultTables(mainCatalog, daughterCatalog, listBox1);
        }

        static void CheckDefaultTables(DBCatalog mainCatalog, DBCatalog daughterCatalog, ListBox listBox1)
        {
            int mainCount = 0;
            int daughterCount = 0;

            foreach (string defaultTable in mainCatalog.SelectDefaultProcessingTables())
            {
                mainCount = mainCatalog.SelectCountRowsTable(defaultTable);
                daughterCount = daughterCatalog.SelectCountRowsTable(defaultTable);

                if (tablesFunctions.ContainsKey(defaultTable))
                {
                    listBox1.Items.Add($"Обработка {defaultTable}: Импортировано {tablesFunctions[defaultTable](mainCatalog, daughterCatalog)} значений");
                }

                else if (mainCount != daughterCount)
                {
                    listBox1.Items.Add($"{defaultTable}: {mainCount} <- M -- D -> {daughterCount}");
                }
                else
                {
                    listBox1.Items.Add($"{defaultTable}: Equal");
                }
            }
        }

        public static void ProcessLinksTables(DBCatalog mainCatalog, ListBox listBox1)
        {
            listBox1.Items.Add("--- Обработка таблиц с внешними ключами ---");
            foreach (string logTable in mainCatalog.SelectDefaultSkipTables())
            {
                listBox1.Items.Add(logTable);
                mainCatalog.ClearTable(logTable);
            }
        }

        static int ProcessTBLUsers(DBCatalog mainCatalog, DBCatalog daughterCatalog)
        {
            List<string> mainListUsersLogins = mainCatalog.SelectListColumnsData("Login", "eqUsers");
            List<string> daughterListUsersLogins = daughterCatalog.SelectListColumnsData("Login", "eqUsers");
            List<string> columns = new List<string>() { "[ID]", "[Login]", "[Department]", "[Deleted]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Email]", "[patronymic]", "[Position]", "[Phone]", "[Room_Number]", "[Description]", "[surname]", "[AccessGranted]", "[Supervisor]", "[FirstName]", "[BUILD_IN_ACCOUNT]", "[Pass]", "[Cookie]", "[UserTheme]" };
            List<string> uniqueValues = ValuesManager.CheckUniqueValue(mainListUsersLogins, daughterListUsersLogins);
            int countImports = 0;

            //1. Берем список логинов из главной и проверяем, что все дефолтные пользователи на месте. Добавляем, если не хватает
            countImports += ValuesManager.CheckDefaultUsers(mainListUsersLogins, countImports);

             //2. Берем список логинов из дочерней и сравниваем с главными. Если есть уникальный, то делаем запрос на получение данных этого пользователя и добавляем его в главную
             if (uniqueValues.Count > 0)
            {
                foreach (string value in uniqueValues)
                {
                    ValuesManager.AddUniqueValue("eqUsers", columns, "Login", value, mainCatalog, daughterCatalog);
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
                RestoreUser(userLogin);
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

        public static void AddUniqueValue(string table, List<string> columns, string filterColumn, string filterValue, DBCatalog maincatalog, DBCatalog daughterCatalog)
        {
            daughterCatalog.UpdateID(table, filterColumn, filterValue);
            maincatalog.InsertFromUniqueValue(table, columns, daughterCatalog.ReturnCatalog(), table, filterColumn, filterValue);
        }

    }
}
