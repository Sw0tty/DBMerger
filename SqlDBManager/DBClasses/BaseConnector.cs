using System.Data.SqlClient;


namespace SqlDBManager.DBClasses
{
    abstract public class BaseDBConnector : SQLAdapters
    {
        private string Source { get; }
        private string Catalog { get; }
        private string Login { get; }
        private string Password { get; }
        private string ConnectionString { get; }
        private SqlConnection Connection { get; }
        private SqlTransaction Transaction { get; set; }

        public BaseDBConnector(string source, string catalog, string login, string password)
        {
            Source = source;
            Catalog = catalog;
            Login = login;
            Password = password;
            ConnectionString = $@"Data Source={Source};Initial Catalog={Catalog};User ID={Login};Password={Password};Connect Timeout=30";
            Connection = new SqlConnection(ConnectionString);
            Transaction = null;
        }

        public void OpenConnection()
        {
            Connection.Open();
        }

        public void CloseConnection()
        {
            Connection.Close();
        }

        protected SqlConnection ReturnConnection()
        {
            return Connection;
        }

        public void StartTransaction()
        {
            Transaction = Connection.BeginTransaction();
        }

        protected SqlTransaction ReturnTransaction()
        {
            return Transaction;
        }

        public void CommitTransaction()
        {
           Transaction.Commit();
        }

        public void RollbackTransaction()
        {
            Transaction.Rollback();
        }

        public string ReturnCatalogName()
        {
            return Catalog;
        }
    }
}
