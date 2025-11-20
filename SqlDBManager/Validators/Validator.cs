using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using NotesNamespace;
using System;


namespace SqlDBManager
{
    public static class Validator
    {
        public static Tuple<int, int> TablesCount = null;
        public static List<string> TablesNamesUncontains = new List<string>();

        public static bool ValidateVersion(DBCatalog сatalog)
        {
            try
            {
                return Consts.ReturnSupportingVersions().Contains(сatalog.SelectCatalogVersion());
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool ValidateApplication(DBCatalog сatalog)
        {
            try
            {
                return сatalog.SelectCatalogApplication() == Consts.ReturnSupportingApplication();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool ValidateCountTables(DBCatalog mainCatalog, DBCatalog daughterCatalog)
        {
            TablesCount = new Tuple<int, int>(mainCatalog.SelectCountTables(), daughterCatalog.SelectCountTables());
            return TablesCount.Item1 == TablesCount.Item2;
        }

        public static bool ValidateNamesTables(DBCatalog mainCatalog, DBCatalog daughterCatalog)
        {
            List<string> mainCatalogNamesTables = mainCatalog.SelectTablesNames();

            foreach (string daughterTablesName in daughterCatalog.SelectTablesNames())
            {
                if (!mainCatalogNamesTables.Contains(daughterTablesName))
                    TablesNamesUncontains.Add(daughterTablesName);
            }
            return TablesNamesUncontains.Count == 0;
        }

        public static bool ValidateDefaultTablesValues(DBCatalog mainCatalog, DBCatalog daughterCatalog, BackgroundWorker worker)
        {
            Dictionary<string, Tuple<string, Dictionary<string, List<string>>>> defaultTables = SpecialTablesValues.DefaultTables;

            Dictionary<string, List<Dictionary<string, string>>> mainProblemTables = ValidateValues(mainCatalog, defaultTables, worker);          
            if (mainProblemTables.Count == 0)
            {
                worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + "Ошибок не обнаружено.");
            }

            Dictionary<string, List<Dictionary<string, string>>> daughterProblemTables = ValidateValues(daughterCatalog, defaultTables, worker);
            if (daughterProblemTables.Count == 0)
            {
                worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + "Ошибок не обнаружено.");
            }

            if (mainProblemTables.Count > 0 || daughterProblemTables.Count > 0)
            {
                Thread.Sleep(2000);
                if (ProgramMessages.MakeEdits() == DialogResult.Yes)
                {
                    worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, "Приступаем к корректировке данных...");
                    if (mainProblemTables.Count > 0)
                    {
                        ValuesManager.RebuildCatalog(mainCatalog, worker, mainProblemTables, defaultTables);
                    }
                    if (daughterProblemTables.Count > 0)
                    {
                        ValuesManager.RebuildCatalog(daughterCatalog, worker, daughterProblemTables, defaultTables);
                    }
                    return true;
                }
                return false;
            }
            return true;
        }

        static Dictionary<string, List<Dictionary<string, string>>> ValidateValues(DBCatalog catalog, Dictionary<string, Tuple<string, Dictionary<string, List<string>>>> defaultTables, BackgroundWorker worker)
        {
            Dictionary<string, List<Dictionary<string, string>>> catalogProblemTables = new Dictionary<string, List<Dictionary<string, string>>>();

            worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, $"Проверяем каталог '{catalog.ReturnCatalogName()}':");
            foreach (string tableName in defaultTables.Keys)
            {
                List<Dictionary<string, string>> catalogInvalidRows = catalog.SelectAllFrom(tableName, catalog.SelectColumnsNames(tableName, null), true, filter: defaultTables[tableName].Item2, filterIN: false);;

                if (catalogInvalidRows.Count > 0)
                {
                    catalogProblemTables.Add(tableName, new List<Dictionary<string, string>>(catalogInvalidRows));

                    worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, $"Недопустимые значения в {tableName}, в колонке {string.Join("", defaultTables[tableName].Item2.Keys)}:");
                    string invalidUniqueValues = "";
                    foreach (Dictionary<string, string> invalidRow in catalogInvalidRows)
                    {
                        invalidUniqueValues += invalidRow[string.Join("", defaultTables[tableName].Item2.Keys)] + " ";
                    }
                    worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + $"{invalidUniqueValues}");
                }
            }
            return catalogProblemTables;
        }
    }
}
