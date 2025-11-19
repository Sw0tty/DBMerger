using SqlDBManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;


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

        public static int ConventToInt(string digitalFromDB)
        {
            return digitalFromDB.Contains("null") ? 0 : Convert.ToInt32(digitalFromDB.Replace("\'", ""));
        }

        public static string CreateSpace(int spaceSize)
        {
            string space = "";
            for (int i = 1; i <= spaceSize; i++)
                space += " ";
            return space;
        }

        public static void SaveMergeLog(string textBlock)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = "c:\\";
                saveFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.FileName = "MergeLog";
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                    {
                        writer.WriteLine($"Merge log of '{Consts.LAST_MAIN_CATALOG}' and '{Consts.LAST_DAUGHTER_CATALOG}' catalogs. \n");

                        foreach (char s in textBlock)
                            writer.Write(s);
                    }
                    Consts.LOG_SAVED = true;
                }
            }
        }
    }

    public static class SpecialTablesValues
    {
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
    }
}
