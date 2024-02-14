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
        /*        {
                    Source = source;
                    Catalog = catalog;
                    Login = login;
                    Password = password;
                    connectionString = $@"Data Source={Source};Initial Catalog={Catalog};User ID={Login};Password={Password};Connect Timeout=30";
                    connection = new SqlConnection(connectionString);
                    transaction = null;
                }*/

/*        public System.Data.SqlClient.SqlTransaction StartTransaction()
        {
            transaction = connection.BeginTransaction();
            return transaction;
        }*/
        /*        public void OpenConnection()
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

                public SqlConnection ReturnConnection()
                {
                    return connection;
                }*/





        /// <summary>
        /// Возвращает путь расположения каталога
        /// </summary>
        public string SelectCatalogPath()
        {
            string request = SQLRequests.BackUpRequests.CatalogPathRequest(Catalog);
            return ReturnStringFromDB(request, ReturnConnection(), ReturnTransaction(), itsValue: true);
        }

        public string ReturnValues(Dictionary<string, string> row)
        {
            return $"(NEWID(), {string.Join(", ", row.Values).Replace("'null'", "null")})";
        }

        public int SelectCountTables()
        {
            string request = SQLRequests.SelectRequests.CountTablesRequest(Catalog);
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
            string request = SQLRequests.SelectRequests.LastInsertRecordRequest(Catalog, columns, tableName, orderByColumn);
            return ReturnStringFromDB(request, ReturnConnection(), ReturnTransaction(), itsValue: true);
            //return ReturnListFromDB(request, connection, ReturnTransaction(), itsRow: true);
        }

        /// <summary>
        /// Возвращает количество записей в переданной таблице
        /// </summary>
        public int SelectCountRowsTable(string tableName)
        {
            // On SelectCountAdapter
            string request = SQLRequests.SelectRequests.CountRowsRequest(Catalog, tableName);
            SqlCommand command = new SqlCommand(request, ReturnConnection());
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
        public List<Dictionary<string, string>> SelectAllFrom(string tableName, List<string> columns, bool allowsNull, Dictionary<string, List<string>> filter = null, bool filterIN = true)
        {
            string request = SQLRequests.SelectRequests.AllRecordsRequest(Catalog, tableName, filter, filterIN, columns);
            //MessageBox.Show(tableName + "       " + request);
/*            if (columns == null)
            {
                columns = SelectColumnsNames(tableName);
            }*/
            return ReturnListDictsFromDB(request, columns, allowsNull, ReturnConnection(), ReturnTransaction());
        }

/*        public string SelectNewID()
        {
            string request = SQLRequests.NewIDRequest(Catalog);
            return ReturnStringFromDB(request, ReturnConnection(), ReturnTransaction(), itsValue: true);
        }*/

        public string SelectIDFrom(string tableName, string idLikeColumn, string filterValue)
        {
            string request = SQLRequests.SelectRequests.IDFromRequest(Catalog, tableName, idLikeColumn, filterValue);
            return ReturnStringFromDB(request, ReturnConnection(), ReturnTransaction(), itsValue: true);
        }

        public string SelectReferenceTableName(string currentTableName, string foreignColumnName)
        {
            string request = SQLRequests.SelectRequests.ReferenceTableNameRequest(Catalog, currentTableName, foreignColumnName);
            return ReturnStringFromDB(request, ReturnConnection(), ReturnTransaction(), itsValue: false);
        }

        public List<string> SelectTablesNames()
        {
            string request = SQLRequests.SelectRequests.AllTablesNamesRequest(Catalog);
            return ReturnListFromDB(request, ReturnConnection(), ReturnTransaction());
        }

        /// <summary>
        /// Returning list of tables with logs data
        /// </summary>
        public List<string> SelectLogTables()
        {
            string request = SQLRequests.SelectRequests.LogTablesRequest(Catalog);
            return ReturnListFromDB(request, ReturnConnection(), ReturnTransaction());
        }

        public List<string> SelectDefaultSkipTables()
        {
            string request = SQLRequests.SelectRequests.SkipRequest(Catalog);
            return ReturnListFromDB(request, ReturnConnection(), ReturnTransaction());
        }

        public List<string> SelectDefaultProcessingTables()
        {
            // Дефолтные таблицы на обработку
            string request = SQLRequests.SelectRequests.ProcessingRequest(Catalog);
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
            string request = SQLRequests.SelectRequests.RecordsUsingAsForeignKeyRequest(Catalog, tableName);
            return ReturnDictFromDB(request, ReturnConnection(), ReturnTransaction());
        }

        public Dictionary<string, string> SelectCatalogVersion()
        {
            string request = SQLRequests.SelectRequests.SelectVersionRequest(Catalog);
            return ReturnDictFromDB(request, ReturnConnection(), ReturnTransaction());
        }

        /// <summary>
        /// Возвращает список наименований столбцов переданной таблицы
        /// </summary>
        public List<string> SelectColumnsNames(string tableName, List<string> excludeColumns)
        {
            string request = SQLRequests.SelectRequests.ColumnsNamesRequest(Catalog, tableName);
            return ReturnListFromDB(request, ReturnConnection(), ReturnTransaction(), excludeColumns);
        }

        /// <summary>
        /// Очищает переданную таблицу
        /// </summary>
        /// <returns>Количество удаленных записей</returns>
        public int ClearTable(string tableName)
        {
            string request = SQLRequests.DeleteRequests.ClearTableRequest(Catalog, tableName);
            return DeleteAdapter(request, ReturnConnection(), ReturnTransaction());
        }

        public void AddReference(string repairTableName, string referenceTableName, string linkColumn)
        {
            string request = SQLRequests.UpdateRequests.AddForeignKeyOnTable(Catalog, repairTableName, referenceTableName, linkColumn);
            // on AlterAdapter
            AnotherRequest(request, ReturnConnection(), ReturnTransaction());
        }

        public void RenameColumn(string tableName, string oldColumnName, string newColumnName)
        {
            string request = SQLRequests.UpdateRequests.RenameTableColumnRequest(Catalog, tableName, oldColumnName, newColumnName);
            // on AlterAdapter
            AnotherRequest(request, ReturnConnection(), ReturnTransaction());
        }

        /// <summary>
        /// Вставляет переданные данные в указанную таблицу (ID формируется средствами SQL)
        /// </summary>
        public void InsertValue(string tableName, Dictionary<string, string> data, bool withoutID = false)
        {
            string request = SQLRequests.InsertRequests.InsertDictValueRequst(Catalog, tableName, data, withoutID);
            InsertAdapter(request, ReturnConnection(), ReturnTransaction());
        }

/*        public void InsertListOfValues(string request)
        {
            InsertAdapter(request, ReturnConnection(), ReturnTransaction());
        }*/

        public void SpecialInsertListOfValues(string tableName, string values, List<string> excludeColumns)
        {
            string request = SQLRequests.InsertRequests.FastFormerInsertValueRequst(SelectColumnsNames(tableName, excludeColumns), Catalog, tableName, values);
            InsertAdapter(request, ReturnConnection(), ReturnTransaction());
        }

/*        public string ListOfValues(string tableName, string values)
        {
            return SQLRequests.FastFormerInsertValueRequst(SelectColumnsNames(tableName), Catalog, tableName, values);
        }*/

        public void UpdateValue(string tableName, string updateColumn, string updateValue, string filterColumn, string filterValue)
        {
            string request = SQLRequests.UpdateRequests.UpdateRowRequest(Catalog, tableName, updateColumn, updateValue, filterColumn, filterValue);
            UpdateAdapter(request, ReturnConnection(), ReturnTransaction());
        }

        public void DeleteValue(string tableName, string filterColumn, string filterValue)
        {
            string request = SQLRequests.DeleteRequests.DeleteRowRequest(Catalog, tableName, filterColumn, filterValue);
            DeleteAdapter(request, ReturnConnection(), ReturnTransaction());
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

        static string ReturnStringFromDB(string request, SqlConnection connection, SqlTransaction transaction, bool itsValue)
        {
            SqlCommand command = new SqlCommand(request, connection);
            command.Transaction = transaction;
            SqlDataReader reader = command.ExecuteReader();
            string valueFromDB = null;

            while (reader.Read())
            {
                if (itsValue)
                {
                    valueFromDB = "'" + reader.GetSqlValue(0).ToString() + "'";
                }
                else
                {
                    valueFromDB = reader.GetSqlValue(0).ToString();
                }
            }
            
            reader.Close();
            command.Dispose();
            return valueFromDB;
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

        /// <summary>
        /// Проверяет на валидность данные дефолтных таблиц
        /// </summary>
        /// <returns>Успешность прохождения валидации</returns>
        /*public bool ValidateDefaultTables(BackgroundWorker worker)
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
                    worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + $"{invalidUniqueValues}");
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
        }*/

        /// <summary>
        /// Вносит изменения в БД. Корректирует невалидные данные дефолтных таблиц
        /// </summary>
        /*public void RebuildDefaultTables(BackgroundWorker worker, Dictionary<string, List<Dictionary<string, string>>> problemTables, Dictionary<string, Tuple<string, Dictionary<string, List<string>>>> defaultTables)
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
        }*/
    }
}
