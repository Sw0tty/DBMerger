using SqlDBManager.DBClasses;
using System;


namespace SqlDBManager
{
    public class BackupManager : BaseDBConnector
    {
        protected string EditsOnDB { get; }
        protected string BackupPath { get; set; }
        protected string RestoreDBPath { get; set; }
        protected string RestoreLOGPath { get; set; }
        protected string LogicalName { get; set; }
        protected string LogicalLogName { get; set; }
        protected string CountOfCopys { get; set; }

        public BackupManager(string catalogPath, string editsOnDB, string catalogName, string source, string login, string password) : base(source, catalogName, login, password)
        {
            EditsOnDB = editsOnDB;
            BackupPath = CreateBackupPath(catalogPath, editsOnDB);
            RestoreDBPath = CreateDBPath(catalogPath, editsOnDB);
            RestoreLOGPath = CreateLOGPath(catalogPath, editsOnDB);
            LogicalName = null;
            LogicalLogName = null;
            CountOfCopys = null;
        }

        public void SetParams()
        {
            LogicalName = SelectLogicalName();
            LogicalLogName = SelectLogicalLogName();

            int copyCount = SelectCountOfCopys();
            CountOfCopys = (copyCount == 0) ? "" : $"{copyCount + 1}";
        }

        public int SelectCountOfCopys()
        {
            string request = SQLRequests.SelectRequests.CountSysRowsRequest("name", EditsOnDB);
            return SelectCountAdapter(request, ReturnConnection(), ReturnTransaction());
        }

        public string SelectLogicalName()
        {
            string request = SQLRequests.SelectRequests.LogicalNameRequest(EditsOnDB);
            return SelectSingleValueAdapter(request, likeValue: true, ReturnConnection(), ReturnTransaction());
        }

        public string SelectLogicalLogName()
        {
            string request = SQLRequests.SelectRequests.LogicalNameLogRequest(EditsOnDB);
            return SelectSingleValueAdapter(request, likeValue: true, ReturnConnection(), ReturnTransaction());
        }

        public int CreateReserveBackup()
        {
            string request = SQLRequests.BackUpRequests.CreateBackupRequest(EditsOnDB, BackupPath);
            return BackUpAdapter(request, ReturnConnection());
        }

        public int DeleteReserveBackup()
        {
            string request = SQLRequests.BackUpRequests.DeleteBackupRequest(BackupPath);
            return BackUpAdapter(request, ReturnConnection());
        }

        public int DropCatalog()
        {
            string request = SQLRequests.BackUpRequests.DropCatalogRequest(EditsOnDB);
            return BackUpAdapter(request, ReturnConnection());
        }

        public int RestoreFromBackup()
        {
            string request = SQLRequests.BackUpRequests.RestoreBackupRequest(EditsOnDB, BackupPath, LogicalName, LogicalLogName, RestoreDBPath, RestoreLOGPath);
            return BackUpAdapter(request, ReturnConnection());
        }

        static string CreateDBPath(string catalogPath, string catalogName)
        {
            return catalogPath.Replace($"{catalogName}.mdf", $"{catalogName}{Consts.VisualConsts.TAIL_OF_MERGED_FILES}.mdf");
        }

        static string CreateLOGPath(string catalogPath, string catalogName)
        {
            return catalogPath.Replace($"{catalogName}.mdf", $"{catalogName}{Consts.VisualConsts.TAIL_OF_MERGED_FILES}.ldf");
        }

        static string CreateBackupPath(string catalogPath, string catalogName)
        {
            return catalogPath.Replace($"DATA\\{catalogName}.mdf", $"Backup\\{catalogName}_{DateTime.Now.ToString().Replace(':', '-')}_backup.bak");
        }
    }
}
