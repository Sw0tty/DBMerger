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
            foreach(string logTable in mainCatalog.SelectLogTables())
            {
                listBox1.Items.Add($"Очистка {logTable}: {mainCatalog.ClearTable(logTable)} записей удалено.");
            }
        }

        public static void CheckDefaultTables(DBCatalog mainCatalog)
        {
            foreach (string logTable in mainCatalog.ReturnLogTables())
            {
                mainCatalog.ClearTable(logTable);
            }
        }
    }
}
