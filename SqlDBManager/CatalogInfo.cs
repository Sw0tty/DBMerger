using NotesNamespace;
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
    public class DBCatalog : SQLAdapters
    {
        protected string Source;
        protected string Catalog;
        protected string Login;
        protected string Password;
        protected string connectionString;
        protected SqlConnection connection;
        private SqlTransaction transaction;

        public DBCatalog(string source, string catalog, string login, string password)
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

        public System.Data.SqlClient.SqlTransaction StartTransaction()
        {
            transaction = connection.BeginTransaction();
            return transaction;
        }

        /// <summary>
        /// Return catalog name
        /// </summary>
        public string ReturnCatalog()
        {
            return Catalog;
        }

        public System.Data.SqlClient.SqlTransaction ReturnTransaction()
        {
            return transaction;
        }

        /// <summary>
        /// Возвращает путь расположения каталога
        /// </summary>
        public List<string> SelectCatalogPath()
        {
            string request = SQLRequests.CatalogPathRequest(Catalog);
            return ReturnListFromDB(request, connection, ReturnTransaction());
        }

        public SqlConnection ReturnConnection()
        {
            return connection;
        }

        public string ReturnInsertRequest(Dictionary<string, string> row, string tableName)
        {
            return SQLRequests.InsertDictValueRequst(Catalog, tableName, row);
        }

        public string ReturnValues(Dictionary<string, string> row)
        {
            return $"(NEWID(), {string.Join(", ", row.Values).Replace("'null'", "null")})";
        }

        public int SelectCountTables()
        {
            string request = SQLRequests.CountTablesRequest(Catalog);
            SqlCommand command = new SqlCommand(request, connection);
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

        public List<string> SelectLastRecord(string columns, string tableName, string orderByColumn)
        {
            string request = SQLRequests.LastInsertRecordRequest(Catalog, columns, tableName, orderByColumn);
            return ReturnListFromDB(request, connection, ReturnTransaction(), itsRow: true);
        }

        /// <summary>
        /// Возвращает количество записей в переданной таблице
        /// </summary>
        public int SelectCountRowsTable(string tableName)
        {
            string request = SQLRequests.CountRowsRequest(Catalog, tableName);
            SqlCommand command = new SqlCommand(request, connection);
            command.Transaction = ReturnTransaction();
            SqlDataReader reader = command.ExecuteReader();

            reader.Read();
            int count = Convert.ToInt32(reader.GetValue(0));
            reader.Close();
            command.Dispose();
            return count;
/*
            string request = SQLRequests.CountRowsRequest(Catalog, tableName);
            return SelectAdapter(request, connection, ReturnTransaction());*/
        }

        /// <summary>
        /// Возвращает список найденых записей в виде словаря (колонка - значение)
        /// </summary>
        public List<Dictionary<string, string>> SelectAllFrom(string tableName, Dictionary<string, List<string>> filter = null, bool filterIN = true, List<string> columns = null)
        {
            string request = SQLRequests.AllRecordsRequest(Catalog, tableName, filter, filterIN, columns);
            MessageBox.Show(tableName + "       " + request);
            if (columns == null)
            {
                columns = SelectColumnsNames(tableName);
            }
            return ReturnListDictsFromDB(request, columns, connection, ReturnTransaction());
        }

        public List<string> SelectTablesNames(bool likeDBString = false, bool itsRow = false)
        {
            string request = SQLRequests.AllTablesNamesRequest(Catalog);
            return ReturnListFromDB(request, connection, ReturnTransaction(), likeDBString, itsRow);
        }

        public List<string> SelectLogTables(bool likeDBString = false, bool itsRow = false)
        {
            string request = SQLRequests.LogTablesRequest(Catalog);
            return ReturnListFromDB(request, connection, ReturnTransaction(), likeDBString, itsRow);
        }

        public List<string> SelectDefaultSkipTables()
        {
            // Дефолтные таблицы на скип
            string request = SQLRequests.SkipRequest(Catalog);
            return ReturnListFromDB(request, connection, ReturnTransaction());
        }

        public List<string> SelectDefaultProcessingTables()
        {
            // Дефолтные таблицы на обработку
            string request = SQLRequests.ProcessingRequest(Catalog);
            return ReturnListFromDB(request, connection, ReturnTransaction());
        }

        /// <summary>
        /// Возвращает список полученных значений по фильтру
        /// </summary>
        public List<Dictionary<string, string>> SelectRecordsWhere(List<string> columns, string tableName, string filterColumn, string filterData)
        {
            string request = SQLRequests.SelectWhereRequest(columns, Catalog, tableName, filterColumn, filterData);
            return ReturnListDictsFromDB(request, SelectColumnsNames(tableName), connection, ReturnTransaction());
        }

        /// <summary>
        /// Возвращает словарь значений где ключ - таблица в которой используются значения из переданной таблицы.
        /// Значение - наименование столбца через который осуществляется связь
        /// </summary>
        public Dictionary<string, string> SelectTablesAndForeignKeyUsage(string tableName)
        {
            string request = SQLRequests.RecordsUsingAsForeignKeyRequest(Catalog, tableName);
            return ReturnDictFromDB(request, connection, ReturnTransaction());
        }

        public Dictionary<string, string> SelectCatalogVersion()
        {
            string request = SQLRequests.SelectVersionRequest(Catalog);
            return ReturnDictFromDB(request, connection, ReturnTransaction());
        }

        /// <summary>
        /// Возвращает список наименований столбцов переданной таблицы
        /// </summary>
        public List<string> SelectColumnsNames(string tableName, bool likeDBString = false, bool itsRow = false)
        {
            string request = SQLRequests.ColumnsNamesRequest(Catalog, tableName);
            return ReturnListFromDB(request, connection, ReturnTransaction(), likeDBString, itsRow);
        }

        /// <summary>
        /// Очищает переданную таблицу
        /// </summary>
        /// <returns>Количество удаленных записей</returns>
        public int ClearTable(string tableName)
        {
            string request = SQLRequests.ClearTableRequest(Catalog, tableName);
            return DeleteAdapter(request, connection, ReturnTransaction());
        }

        public void AddReference(string repairTableName, string referenceTableName, string linkColumn)
        {
            string request = SQLRequests.AddForeignKeyOnTable(Catalog, repairTableName, referenceTableName, linkColumn);
            AnotherRequest(request, connection, ReturnTransaction());
        }

        public void RenameColumn(string tableName, string oldColumnName, string newColumnName)
        {
            string request = SQLRequests.RenameTableColumnRequest(Catalog, tableName, oldColumnName, newColumnName);
            AnotherRequest(request, connection, ReturnTransaction());
        }

        /// <summary>
        /// Вставляет переданные данные в указанную таблицу (ID формируется средствами SQL)
        /// </summary>
        public void InsertValue(string tableName, Dictionary<string, string> data, bool withoutID = false)
        {
            string request = SQLRequests.InsertDictValueRequst(Catalog, tableName, data, withoutID);
            InsertAdapter(request, connection, ReturnTransaction());
        }

        public void InsertListOfValues(string request)
        {
            InsertAdapter(request, connection, ReturnTransaction());
        }

        public void SpecialInsertListOfValues(string tableName, string values)
        {
            string request = SQLRequests.FastFormerInsertValueRequst(SelectColumnsNames(tableName), Catalog, tableName, values);
            InsertAdapter(request, connection, ReturnTransaction());
        }

        public string ListOfValues(string tableName, string values)
        {
            return SQLRequests.FastFormerInsertValueRequst(SelectColumnsNames(tableName), Catalog, tableName, values);
        }

        public void UpdateValue(string tableName, string updateColumn, string updateValue, string filterColumn, string filterValue)
        {
            string request = SQLRequests.UpdateRowRequest(Catalog, tableName, updateColumn, updateValue, filterColumn, filterValue);
            UpdateAdapter(request, connection, ReturnTransaction());
        }

        public void DeleteValue(string tableName, string filterColumn, string filterValue)
        {
            string request = SQLRequests.DeleteRowRequest(Catalog, tableName, filterColumn, filterValue);
            DeleteAdapter(request, connection, ReturnTransaction());
        }

        /// <summary>
        /// Makes UPDATE requests
        /// </summary>
        /// <returns>Count of affected rows</returns>
        static void AnotherRequest(string request, SqlConnection connection, SqlTransaction transaction)
        {
            SqlCommand cmd = new SqlCommand(request, connection);
            cmd.Transaction = transaction;
            //adapter.DeleteCommand.Transaction = ReturnTransaction();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        /// <summary>
        /// Возвращает список словарей значений таблицы
        /// </summary>
        static List<Dictionary<string, string>> ReturnListDictsFromDB(string request, List<string> columnsNames, SqlConnection connection, SqlTransaction transaction)
        {
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
                        rowData[columnsNames[i]] = "'null'";
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

        static List<string> ReturnListFromDB(string request, SqlConnection connection, SqlTransaction transaction, bool likeDBString = false, bool itsRow = false)
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
            else if (itsRow)
            {
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                        listTablesNames.Add("'" + reader.GetValue(i).ToString() + "'");
                }
            }
            else
            {
                while (reader.Read())
                {
                    listTablesNames.Add(reader.GetValue(0).ToString());
                }
            }
            
            reader.Close();
            command.Dispose();
            return listTablesNames;
        }

        /// <summary>
        /// Проверяет на валидность данные дефолтных таблиц
        /// </summary>
        /// <returns>Успешность прохождения валидации</returns>
        public bool ValidateDefaultTables(BackgroundWorker worker)
        {
            Dictionary<string, Tuple<string, Dictionary<string, List<string>>>> defaultTables = SpecialTablesValues.DefaultTables;
            Dictionary<string, List<Dictionary<string, string>>> problemTables = new Dictionary<string, List<Dictionary<string, string>>>();

            foreach (string tableName in defaultTables.Keys)
            {
                List<Dictionary<string, string>> invalidRows = SelectAllFrom(tableName, defaultTables[tableName].Item2, filterIN: false);

                if (invalidRows.Count > 0)
                {
                    problemTables.Add(tableName, new List<Dictionary<string, string>>(invalidRows));

                    worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, $"Недопустимые значения в {tableName}, в колонке {string.Join("", defaultTables[tableName].Item2.Keys)}:");
                    string invalidUniqueValues = "";
                    foreach (Dictionary<string, string> invalidRow in invalidRows)
                    {
                        invalidUniqueValues += invalidRow[string.Join("", defaultTables[tableName].Item2.Keys)] + " ";
                    }
                    worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(VisualConsts.SPACE_SIZE) + $"{invalidUniqueValues}");
                }
            }

            if (problemTables.Count > 0)
            {
                Thread.Sleep(2000);
                if (MessageBox.Show("Исправить дочернюю базу данных?", "Системное сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, "Приступаем к корректировке данных...");
                    RebuildDefaultTables(worker, problemTables, defaultTables);
                    return true;
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// Вносит изменения в БД. Корректирует невалидные данные дефолтных таблиц
        /// </summary>
        public void RebuildDefaultTables(BackgroundWorker worker, Dictionary<string, List<Dictionary<string, string>>> problemTables, Dictionary<string, Tuple<string, Dictionary<string, List<string>>>> defaultTables)
        {
            foreach (string tableName in problemTables.Keys)
            {
                foreach (Dictionary<string, string> row in problemTables[tableName])
                {
                    Dictionary<string, string> foreignsDict = SelectTablesAndForeignKeyUsage(tableName);

                    foreach (string updateTable in foreignsDict.Keys)
                    {
                        UpdateValue(updateTable, foreignsDict[updateTable], defaultTables[tableName].Item1, foreignsDict[updateTable], row[foreignsDict[updateTable]]);
                    }
                    DeleteValue(tableName, string.Join("", defaultTables[tableName].Item2.Keys), row[string.Join("", defaultTables[tableName].Item2.Keys)]);
                }
            }
            worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, "Данные скорректированы.");
        }
    }
}
