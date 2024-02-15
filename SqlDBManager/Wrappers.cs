using System;
using System.ComponentModel;


namespace SqlDBManager
{
    public static class Wrappers
    {
        public static bool WrapValidator(Func<DBCatalog, DBCatalog, bool> function, DBCatalog mainCatalog, DBCatalog daughterCatalog, BackgroundWorker worker)
        {
            Consts.MergeProgress.AddToAllTasks(1);
            bool status = function(mainCatalog, daughterCatalog);
            worker.ReportProgress(Consts.MergeProgress.UpdateMainBar(), Consts.WorkerConsts.ITS_MAIN_PROGRESS_BAR);
            return status;
        }

        public static bool WrapCustomValidator(Func<DBCatalog, DBCatalog, BackgroundWorker, bool> function, DBCatalog mainCatalog, DBCatalog daughterCatalog, BackgroundWorker worker)
        {
            Consts.MergeProgress.AddToAllTasks(1);
            bool status = function(mainCatalog, daughterCatalog, worker);
            worker.ReportProgress(Consts.MergeProgress.UpdateMainBar(), Consts.WorkerConsts.ITS_MAIN_PROGRESS_BAR);
            return status;
        }

        public static bool WrapSimpleMergeFunc(Func<DBCatalog, BackgroundWorker, bool> function, DBCatalog catalog, BackgroundWorker worker)
        {
            Consts.MergeProgress.AddToAllTasks(1);
            bool status = function(catalog, worker);
            worker.ReportProgress(Consts.MergeProgress.UpdateMainBar(), Consts.WorkerConsts.ITS_MAIN_PROGRESS_BAR);
            return status;
        }
    }
}
