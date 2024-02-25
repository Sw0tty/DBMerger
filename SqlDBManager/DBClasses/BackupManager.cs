﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDBManager
{
    public class BackupManager
    {
        protected string BackupPath {  get; set; }
        SqlConnection masterСonnection;

        public BackupManager(string catalogPath, string catalogName, string source, string login, string password)
        {
            BackupPath = CreateBackupPath(catalogPath, catalogName);
            masterСonnection = new SqlConnection($@"Data Source={source};Initial Catalog=master;User ID={login};Password={password};Connect Timeout=30");
        }

        public void OpenConnection()
        {
            masterСonnection.Open();
        }

        public void CloseConnection()
        {
            masterСonnection.Close();
        }

        public void CreateReserveBackup(string catalog)
        {
            string request = SQLRequests.BackUpRequests.CreateBackupRequest(catalog, BackupPath);
            SqlCommand command = new SqlCommand(request, masterСonnection);

            command.ExecuteNonQuery();
            command.Dispose();
        }

        public void DeleteReserveBackup()
        {
            string request = SQLRequests.BackUpRequests.DeleteBackupRequest(BackupPath);
            SqlCommand command = new SqlCommand(request, masterСonnection);

            command.ExecuteNonQuery();
            command.Dispose();
        }

        public void RestoreFromBackup(string catalog)
        {
            string request = SQLRequests.BackUpRequests.RestoreBackupRequest(catalog, BackupPath);
            SqlCommand command = new SqlCommand(request, masterСonnection);

            command.ExecuteNonQuery();
            command.Dispose();
        }

        static string CreateBackupPath(string catalogPath, string catalogName)
        {
            //catalogPath = catalogPath.Replace($"DATA\\{catalogName}.mdf", $"Backup\\{catalogName}_reserve.bak");
            return catalogPath.Replace($"DATA\\{catalogName}.mdf", $"Backup\\{catalogName}_reserve.bak"); ;
        }
    }
}
