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

        public static void PrintSkipDefaultTables(DBCatalog mainCatalog, System.Windows.Forms.ListBox listBox1)
        {
            listBox1.Items.Add("--- Обработка дефолтных таблиц ---");
            foreach (string logTable in mainCatalog.SelectDefaultSkipTables())
            {
                listBox1.Items.Add(logTable);
                mainCatalog.ClearTable(logTable);
            }
        }
    }
}
