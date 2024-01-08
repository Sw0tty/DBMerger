using NotesNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SqlDBManager
{
    public static class MergeManager
    {
        public static Dictionary<string, Func<DBCatalog, DBCatalog, string>> tablesFunctions = new Dictionary<string, Func<DBCatalog, DBCatalog, string>> {
            { "eqUsers", ProcessTBLUsers },
        };

        public static void ClearLogs(DBCatalog mainCatalog, System.Windows.Forms.ListBox listBox1)
        {
            listBox1.Items.Add("--- Очистка логов ---");
            foreach(string logTable in mainCatalog.SelectLogTables())
            {
                listBox1.Items.Add($"Очистка {logTable}: {mainCatalog.ClearTable(logTable)} записей удалено.");
            }
        }

        public static void ProcessDefaultTables(DBCatalog mainCatalog, DBCatalog daughterCatalog, System.Windows.Forms.ListBox listBox1)
        {
            listBox1.Items.Add("--- Обработка дефолтных таблиц ---");
            foreach (string defaultTable in mainCatalog.SelectDefaultSkipTables())
            {
                listBox1.Items.Add($"{defaultTable}: default");
            }

            CheckDefaultTables(mainCatalog, daughterCatalog, listBox1);
        }

        static void CheckDefaultTables(DBCatalog mainCatalog, DBCatalog daughterCatalog, System.Windows.Forms.ListBox listBox1)
        {
            int mainCount = 0;
            int daughterCount = 0;

            foreach (string defaultTable in mainCatalog.SelectDefaultProcessingTables())
            {
                mainCount = mainCatalog.SelectCountRowsTable(defaultTable);
                daughterCount = daughterCatalog.SelectCountRowsTable(defaultTable);

                /*if (tablesFunctions.ContainsKey(defaultTable))
                {
                    listBox1.Items.Add($"Обработка {defaultTable}: {tablesFunctions[defaultTable](mainCatalog, daughterCatalog)}");
                }*/

                if (mainCount != daughterCount)
                {
                    listBox1.Items.Add($"{defaultTable}: {mainCount} <- M -- D -> {daughterCount}");
                }
                listBox1.Items.Add($"{defaultTable}: Equal");
            }
        }

        public static void ProcessLinksTables(DBCatalog mainCatalog, System.Windows.Forms.ListBox listBox1)
        {
            listBox1.Items.Add("--- Обработка таблиц с внешними ключами ---");
            foreach (string logTable in mainCatalog.SelectDefaultSkipTables())
            {
                listBox1.Items.Add(logTable);
                mainCatalog.ClearTable(logTable);
            }
        }

        static string ProcessTBLUsers(DBCatalog mainCatalog, DBCatalog daughterCatalog)
        {
            Dictionary<int, List<string>> mainUsersLogins = mainCatalog.SelectColumnsData(new List<string> { "Login" }, "eqUsers");
            Dictionary<int, List<string>> daughterUsersLogins = daughterCatalog.SelectColumnsData(new List<string> { "Login" }, "eqUsers");

            //DefaultValuesManager.CheckUsers(mainUsersLogins);

            for (int i = 1; i <= mainUsersLogins.Count; i++)
            {
                foreach (string data in mainUsersLogins[1])
                {

                }
            }

            

            string som = "";
            return som;
        }
    }

    public static class DefaultValuesManager
    {
        static List<string> defaultUsers = new List<string>() { "sa", "anonymous", "admin", "reader", "arch", "tech" };

        public static bool CheckUsers(Dictionary<int, List<string>> users)
        {
            List<string> catalogUsers = new List<string>();

            for (int i = 1; i <= users.Count; i++)
            {
                foreach (string data in users[1])
                {
                    catalogUsers.Add(data);
                }
            }

            foreach (string userLogin in defaultUsers)
            {
                if (catalogUsers.Contains(userLogin))
                {
                    continue;
                }
                RestoreUser(userLogin);
            }

            //List<string> defaultUsers = new List<string>() { "sa", "anonymous", "admin", "reader", "arch", "tech" };

            return false;
        }

        public static void RestoreUser(string userLogin)
        {

        }
    }
}
