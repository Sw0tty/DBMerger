using NotesNamespace;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDBManager
{
    public abstract class SQLAdapters
    {
        /// <summary>
        /// Makes SELECT requests
        /// </summary>
        /// <returns>Tuple of: Count of affected rows - Selected data</returns>
        protected Tuple<int, List<Dictionary<string, string>>> SelectAdapter(string request, SqlConnection connection, SqlTransaction transaction)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet dataSet = new DataSet();
            adapter.SelectCommand = new SqlCommand(request, connection);
            adapter.SelectCommand.Transaction = transaction;
            List<Dictionary<string, string>> selectedData = new List<Dictionary<string, string>>();
            adapter.Fill(dataSet);

            foreach (DataTable table in dataSet.Tables)
            {
                List<string> columns = new List<string>();
                foreach (DataColumn column in table.Columns)
                {
                    columns.Add(column.ColumnName);
                }

                foreach (DataRow row in table.Rows)
                {
                    var cells = row.ItemArray;
                    Dictionary<string, string> valueFromTable = new Dictionary<string, string>();
                    int columnNum = 0;

                    foreach (object cell in cells)
                    {
                        if (cell.ToString() == "")
                        {
                            valueFromTable[columns[columnNum]] = "'null'";
                        }
                        else
                        {
                            valueFromTable[columns[columnNum]] = "'" + cell.ToString() + "'";
                        }
                        columnNum++;
                    }
                    selectedData.Add(new Dictionary<string, string>(valueFromTable));
                }
            }
            adapter.Dispose();
            return new Tuple<int, List<Dictionary<string, string>>>(selectedData.Count, selectedData);
        }

        /// <summary>
        /// Makes SELECT COUNT requests
        /// </summary>
        /// <returns>Count of affected rows</returns>
        protected int SelectCountAdapter(string request, SqlConnection connection, SqlTransaction transaction)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(request, connection);
            adapter.SelectCommand.Transaction = transaction;
            SqlDataReader reader = adapter.SelectCommand.ExecuteReader();
            reader.Read();
            int count = Convert.ToInt32(reader.GetValue(0));
            reader.Close();
            adapter.Dispose();
            return count;
        }

        /// <summary>
        /// Makes MODIFICATION/MANIPULATION catalog requests
        /// </summary>
        /// <returns>Count of affected rows</returns>
        protected int AlterAdapter(string request, SqlConnection connection, SqlTransaction transaction)
        {
            SqlCommand command = new SqlCommand(request, connection);
            command.Transaction = transaction;
            int rowsAffected = command.ExecuteNonQuery();
            command.Dispose();
            return rowsAffected;
        }

        /// <summary>
        /// Makes INSERT requests
        /// </summary>
        /// <returns>Count of affected rows</returns>
        protected int InsertAdapter(string request, SqlConnection connection, SqlTransaction transaction)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.InsertCommand = new SqlCommand(request, connection);
            adapter.InsertCommand.Transaction = transaction;
            int rowsAffected = adapter.InsertCommand.ExecuteNonQuery();
            adapter.Dispose();
            return rowsAffected;
        }

        /// <summary>
        /// Makes UPDATE requests
        /// </summary>
        /// <returns>Count of affected rows</returns>
        protected int UpdateAdapter(string request, SqlConnection connection, SqlTransaction transaction)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.UpdateCommand = new SqlCommand(request, connection);
            adapter.UpdateCommand.Transaction = transaction;
            int rowsAffected = adapter.UpdateCommand.ExecuteNonQuery();
            adapter.Dispose();
            return rowsAffected;
        }

        /// <summary>
        /// Makes DELTE requests
        /// </summary>
        /// <returns>Count of affected rows</returns>
        protected int DeleteAdapter(string request, SqlConnection connection, SqlTransaction transaction)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.DeleteCommand = new SqlCommand(request, connection);
            adapter.DeleteCommand.Transaction = transaction;
            int rowsAffected = adapter.DeleteCommand.ExecuteNonQuery();
            adapter.Dispose();
            return rowsAffected;
        }
    }
}
