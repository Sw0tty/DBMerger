using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SqlDBManager.DBClasses
{
    abstract public class BaseDBConnector : SQLAdapters
    {
        protected string Source;
        protected string Catalog;
        protected string Login;
        protected string Password;
        protected string connectionString;
        protected SqlConnection connection;
        protected SqlTransaction transaction;

        public BaseDBConnector(string source, string catalog, string login, string password)
        {
            Source = source;
            Catalog = catalog;
            Login = login;
            Password = password;
            connectionString = $@"Data Source={Source};Initial Catalog={Catalog};User ID={Login};Password={Password};Connect Timeout=30";
            connection = new SqlConnection(connectionString);
            transaction = null;
        }

        public void OpenConnection()
        {
            connection.Open();
        }

        public void CloseConnection()
        {
            connection.Close();
        }

        protected SqlConnection ReturnConnection()
        {
            return connection;
        }

        public void StartTransaction()
        {
            transaction = connection.BeginTransaction();
        }

        protected SqlTransaction ReturnTransaction()
        {
            return transaction;
        }

        public void CommitTransaction()
        {
           transaction.Commit();
        }

        public void RollbackTransaction()
        {
            transaction.Rollback();
        }

        public string ReturnCatalogName()
        {
            return Catalog;
        }
    }
}
