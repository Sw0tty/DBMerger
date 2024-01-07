using NotesNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SqlDBManager
{
    public static class MergeManager
    {
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
    }
}
