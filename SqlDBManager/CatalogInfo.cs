using NotesNamespace;
using SqlDBManager.DBClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;


namespace SqlDBManager
{
    public class DBCatalog : BaseDBConnector
    {
/*        protected string Source;
        protected string Catalog;
        protected string Login;
        protected string Password;
        protected string connectionString;
        protected SqlConnection connection;
        private SqlTransaction transaction;*/

        public DBCatalog(string source, string catalog, string login, string password) : base(source, catalog, login, password) { }

        /// <summary>
        /// Возвращает путь расположения каталога
        /// </summary>
        public string SelectCatalogPath()
        {
            string request = SQLRequests.BackUpRequests.CatalogPathRequest(ReturnCatalogName());
            return SelectSingleValueAdapter(request, likeValue: true, ReturnConnection(), ReturnTransaction());
            //return ReturnStringFromDB(request, ReturnConnection(), ReturnTransaction(), itsValue: true);
        }

        public string ReturnValues(Dictionary<string, string> row, bool withoutID = false)
        {
            if (withoutID)
                return $"({string.Join(", ", row.Values).Replace("'null'", "null")})";
            return $"(NEWID(), {string.Join(", ", row.Values).Replace("'null'", "null")})";
        }

        public int SelectCountTables()
        {
            string request = SQLRequests.SelectRequests.CountTablesRequest(ReturnCatalogName());
            SqlCommand command = new SqlCommand(request, ReturnConnection());
            command.Transaction = ReturnTransaction();
            SqlDataReader reader = command.ExecuteReader();

            reader.Read();
            int count = Convert.ToInt32(reader.GetValue(0));
            reader.Close();
            command.Dispose();
            return count;
/*            string request = SQLRequests.CountTablesRequest(Catalog);
            return SelectAdapter(request, connection, ReturnTransaction());*/
        }

        public string SelectLastRecord(string columns, string tableName, string orderByColumn)
        {
            string request = SQLRequests.SelectRequests.LastInsertRecordRequest(ReturnCatalogName(), columns, tableName, orderByColumn);
            return SelectSingleValueAdapter(request, likeValue: true, ReturnConnection(), ReturnTransaction());
        }

        /// <summary>
        /// Возвращает количество записей в переданной таблице
        /// </summary>
        public int SelectCountRowsTable(string tableName)
        {
            string request = SQLRequests.SelectRequests.CountRowsRequest(ReturnCatalogName(), tableName);
            return SelectCountAdapter(request, ReturnConnection(), ReturnTransaction());
/*            SqlCommand command = new SqlCommand(request, ReturnConnection());
            command.Transaction = ReturnTransaction();
            SqlDataReader reader = command.ExecuteReader();

            reader.Read();
            int count = Convert.ToInt32(reader.GetValue(0));
            reader.Close();
            command.Dispose();
            return count;*/
        }

        /// <summary>
        /// Возвращает список найденых записей в виде словаря (колонка - значение)
        /// </summary>
        public List<Dictionary<string, string>> SelectAllFrom(string tableName, List<string> columns, bool allowsNull, Dictionary<string, List<string>> filter = null, bool filterIN = true)
        {
            string request = SQLRequests.SelectRequests.AllRecordsRequest(ReturnCatalogName(), tableName, filter, filterIN, columns);
            return ReturnListDictsFromDB(request, columns, allowsNull, ReturnConnection(), ReturnTransaction());
        }

/*        public string SelectNewID()
        {
            string request = SQLRequests.NewIDRequest(Catalog);
            return ReturnStringFromDB(request, ReturnConnection(), ReturnTransaction(), itsValue: true);
        }*/

        public string SelectIDFrom(string tableName, string idLikeColumn, string filterValue)
        {
            string request = SQLRequests.SelectRequests.IDFromRequest(ReturnCatalogName(), tableName, idLikeColumn, filterValue);
            return SelectSingleValueAdapter(request, likeValue: true, ReturnConnection(), ReturnTransaction());
            //return ReturnStringFromDB(request, ReturnConnection(), ReturnTransaction(), itsValue: true);
        }

        public string SelectReferenceTableName(string currentTableName, string foreignColumnName)
        {
            string request = SQLRequests.SelectRequests.ReferenceTableNameRequest(ReturnCatalogName(), currentTableName, foreignColumnName);
            return SelectSingleValueAdapter(request, likeValue: false, ReturnConnection(), ReturnTransaction());
            //return ReturnStringFromDB(request, ReturnConnection(), ReturnTransaction(), itsValue: false);
        }

        public List<string> SelectTablesNames()
        {
            string request = SQLRequests.SelectRequests.AllTablesNamesRequest(ReturnCatalogName());
            return ReturnListFromDB(request, ReturnConnection(), ReturnTransaction());
        }

        /// <summary>
        /// Returning list of tables with logs data
        /// </summary>
        public List<string> SelectLogTables()
        {
            string request = SQLRequests.SelectRequests.LogTablesRequest(ReturnCatalogName());
            return ReturnListFromDB(request, ReturnConnection(), ReturnTransaction());
        }

        public List<string> SelectDefaultSkipTables()
        {
            string request = SQLRequests.SelectRequests.SkipRequest(ReturnCatalogName());
            return ReturnListFromDB(request, ReturnConnection(), ReturnTransaction());
        }

        public List<string> SelectDefaultProcessingTables()
        {
            // Дефолтные таблицы на обработку
            string request = SQLRequests.SelectRequests.ProcessingRequest(ReturnCatalogName());
            return ReturnListFromDB(request, ReturnConnection(), ReturnTransaction());
        }

        /// <summary>
        /// Возвращает список полученных значений по фильтру
        /// </summary>
/*        public List<Dictionary<string, string>> SelectRecordsWhere(List<string> columns, string tableName, string filterColumn, string filterData)
        {
            string request = SQLRequests.SelectRequests.SelectWhereRequest(columns, Catalog, tableName, filterColumn, filterData);
            return ReturnListDictsFromDB(request, SelectColumnsNames(tableName), ReturnConnection(), ReturnTransaction());
        }*/

        /// <summary>
        /// Возвращает словарь значений где ключ - таблица в которой используются значения из переданной таблицы.
        /// Значение - наименование столбца через который осуществляется связь
        /// </summary>
        public Dictionary<string, string> SelectTablesAndForeignKeyUsage(string tableName)
        {
            string request = SQLRequests.SelectRequests.RecordsUsingAsForeignKeyRequest(ReturnCatalogName(), tableName);
            return ReturnDictFromDB(request, ReturnConnection(), ReturnTransaction());
        }

        public Dictionary<string, string> SelectCatalogVersion()
        {
            string request = SQLRequests.SelectRequests.SelectVersionRequest(ReturnCatalogName());
            return ReturnDictFromDB(request, ReturnConnection(), ReturnTransaction());
        }

        /// <summary>
        /// Возвращает список наименований столбцов переданной таблицы
        /// </summary>
        public List<string> SelectColumnsNames(string tableName, List<string> excludeColumns)
        {
            string request = SQLRequests.SelectRequests.ColumnsNamesRequest(ReturnCatalogName(), tableName);
            return ReturnListFromDB(request, ReturnConnection(), ReturnTransaction(), excludeColumns);
        }

        /// <summary>
        /// Очищает переданную таблицу
        /// </summary>
        /// <returns>Количество удаленных записей</returns>
        public int ClearTable(string tableName)
        {
            string request = SQLRequests.DeleteRequests.ClearTableRequest(ReturnCatalogName(), tableName);
            return DeleteAdapter(request, ReturnConnection(), ReturnTransaction());
        }

        public void AddReference(string repairTableName, string referenceTableName, string linkColumn)
        {
            string request = SQLRequests.UpdateRequests.AddForeignKeyOnTable(ReturnCatalogName(), repairTableName, referenceTableName, linkColumn);
            AlterAdapter(request, ReturnConnection(), ReturnTransaction());
        }

        public void RenameColumn(string tableName, string oldColumnName, string newColumnName)
        {
            string request = SQLRequests.UpdateRequests.RenameTableColumnRequest(ReturnCatalogName(), tableName, oldColumnName, newColumnName);
            AlterAdapter(request, ReturnConnection(), ReturnTransaction());
        }

        /// <summary>
        /// Вставляет переданные данные в указанную таблицу (ID формируется средствами SQL)
        /// </summary>
        public void InsertValue(string tableName, Dictionary<string, string> data, bool withoutID = false)
        {
            string request = SQLRequests.InsertRequests.InsertDictValueRequst(ReturnCatalogName(), tableName, data, withoutID);
            InsertAdapter(request, ReturnConnection(), ReturnTransaction());
        }

        public void SpecialInsertListOfValues(string tableName, string values, List<string> excludeColumns)
        {
            string request = SQLRequests.InsertRequests.FastFormerInsertValueRequst(SelectColumnsNames(tableName, excludeColumns), ReturnCatalogName(), tableName, values);
            InsertAdapter(request, ReturnConnection(), ReturnTransaction());
        }

        public void UpdateValue(string tableName, string updateColumn, string updateValue, string filterColumn, string filterValue)
        {
            string request = SQLRequests.UpdateRequests.UpdateRowRequest(ReturnCatalogName(), tableName, updateColumn, updateValue, filterColumn, filterValue);
            UpdateAdapter(request, ReturnConnection(), ReturnTransaction());
        }

        public void DeleteValue(string tableName, string filterColumn, string filterValue)
        {
            string request = SQLRequests.DeleteRequests.DeleteRowRequest(ReturnCatalogName(), tableName, filterColumn, filterValue);
            DeleteAdapter(request, ReturnConnection(), ReturnTransaction());
        }

        /// <summary>
        /// Возвращает список словарей значений таблицы
        /// </summary>
        static List<Dictionary<string, string>> ReturnListDictsFromDB(string request, List<string> columnsNames, bool allowsNull, SqlConnection connection, SqlTransaction transaction)
        {
            // on SELECT Adapter
            SqlCommand command = new SqlCommand(request, connection);
            command.Transaction = transaction;
            SqlDataReader reader = command.ExecuteReader();
            
            List<Dictionary<string, string>> tableData = new List<Dictionary<string, string>>();

            while (reader.Read())
            {
                Dictionary<string, string> rowData = new Dictionary<string, string>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader[i].ToString().Trim(' ') == "")
                    {
                        if (allowsNull)
                        {
                            rowData[columnsNames[i]] = "'null'";
                        }
                        else
                        {
                            rowData[columnsNames[i]] = "''";
                        }
                    }
                    else
                    {
                        rowData[columnsNames[i]] = "'" + reader[i].ToString() + "'";
                    }
                }
                tableData.Add(new Dictionary<string, string>(rowData));
            }

            reader.Close();
            command.Dispose();
            return tableData;
        }

        /// <summary>
        /// Возвращает словарь значений таблицы
        /// </summary>
        static Dictionary<string, string> ReturnDictFromDB(string request, SqlConnection connection, SqlTransaction transaction)
        {
            SqlCommand command = new SqlCommand(request, connection);
            command.Transaction = transaction;
            SqlDataReader reader = command.ExecuteReader();
            Dictionary<string, string> dictTableData = new Dictionary<string, string>();

            while (reader.Read())
            {
                dictTableData.Add(reader.GetString(0), reader.GetString(1));
            }

            reader.Close();
            command.Dispose();
            return dictTableData;
        }

        static List<string> ReturnListFromDB(string request, SqlConnection connection, SqlTransaction transaction, List<string> excludeColumns = null, bool likeDBString = false)
        {
            SqlCommand command = new SqlCommand(request, connection);
            command.Transaction = transaction;
            SqlDataReader reader = command.ExecuteReader();
            List<string> listTablesNames = new List<string>();

            if (likeDBString)
            {
                while (reader.Read())
                {
                    listTablesNames.Add("'" + reader.GetValue(0).ToString() + "'");
                }
            }
/*            else if (itsRow)
            {
                MessageBox.Show("row");
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                        listTablesNames.Add("'" + reader.GetValue(i).ToString() + "'");
                }
            }*/
            else
            {
                while (reader.Read())
                {
                    listTablesNames.Add(reader.GetValue(0).ToString());
                }
            }

            if (excludeColumns != null)
                listTablesNames = HelpFunction.Exclude(listTablesNames, excludeColumns);
            
            reader.Close();
            command.Dispose();
            return listTablesNames;
        }
    }
}
