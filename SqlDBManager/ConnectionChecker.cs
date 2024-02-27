using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System;


namespace SqlDBManager
{
    public static class ConnectionChecker
    {
        public static bool CheckConnection(string source, string catalog, string login, string password)
        {
            string connectionString = $@"Data Source={source};Initial Catalog={catalog};User ID={login};Password={password};Connect Timeout=30";
            SqlConnection cnn = new SqlConnection(connectionString);

            try
            {
                //SqlExtensions.QuickOpen(cnn, 60);
                cnn.Open();

                if (catalog == "")
                {
                    cnn.Close();
                    return false;
                }
                cnn.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static DialogResult CheckConnectionMessage(string source, string catalog, string login, string password)
        {
            SqlCommand command;
            SqlDataReader reader;
            List<string> response = new List<string>();
            string connectionString = $@"Data Source={source};Initial Catalog={catalog};User ID={login};Password={password};Connect Timeout=15";

            SqlConnection cnn = new SqlConnection(connectionString);

            try
            {
                cnn.Open();
                //SqlExtensions.QuickOpen(cnn, 60);

                if (catalog == "")
                {
                    string request = SQLRequests.SelectRequests.AllAllowedDataBasesRequest();
                    command = new SqlCommand(request, cnn);
                    reader = command.ExecuteReader();

                    while (reader.Read())
                        response.Add(reader.GetValue(0).ToString());

                    reader.Close();
                    command.Dispose();
                    cnn.Close();
                    return ProgramMessages.ConnectionWarningMessage(response);
                }
                cnn.Close();
                return ProgramMessages.ConnectionSuccessMessage();
            }
            catch
            {
                return ProgramMessages.ConnectionErrorMessage();
            }
        }
    }

    public static class SqlExtensions
    {
        public static void QuickOpen(this SqlConnection conn, int timeout)
        {
            // We'll use a Stopwatch here for simplicity. A comparison to a stored DateTime.Now value could also be used
            Stopwatch sw = new Stopwatch();
            bool connectSuccess = false;

            // Try to open the connection, if anything goes wrong, make sure we set connectSuccess = false
            Thread t = new Thread(delegate ()
            {
                try
                {
                    sw.Start();
                    conn.Open();
                    connectSuccess = true;
                }
                catch { }
            });

            // Make sure it's marked as a background thread so it'll get cleaned up automatically
            t.IsBackground = true;
            t.Start();

            // Keep trying to join the thread until we either succeed or the timeout value has been exceeded
            while (timeout > sw.ElapsedMilliseconds)
                if (t.Join(1))
                    break;

            // If we didn't connect successfully, throw an exception
            if (!connectSuccess)
                throw new Exception("Timed out while trying to connect.");
        }
    }
}
