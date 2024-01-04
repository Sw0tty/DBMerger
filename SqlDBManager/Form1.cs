using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NotesNamespace;

namespace SqlDBManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            label1.Text = "База данных:";
            label2.Text = "Наименование базы данных:";
            label3.Text = "Логин:";
            label4.Text = "Пароль:";
            label5.Text = label1.Text;
            label6.Text = label2.Text;
            label7.Text = label3.Text;
            label8.Text = label4.Text;

            button2.Text = "Проверить соединение";
            button3.Text = button2.Text;

            StringCollection coll = Properties.Settings.Default.comboBox1Default;
            
            if (coll != null)
            {
                foreach (var s in coll)
                {
                    comboBox1.Items.Add(s);
                }
                //comboBox1.Text = Properties.Settings.Default.comboBox1LastSelect;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
             Проверяет соединения с основной БД
             */

            SqlConnection cnn;
            SqlCommand command;
            SqlDataReader reader;
            String source,
                   catalog = "main",
                   login,
                   password,
                   request,
                   value,
                   connectionString,
                   output = "";
            List<string> db_tables = new List<string>();

            

            //connectionString = $@"Data Source={source};Initial Catalog={catalog};User ID={login};Password={password}";

            connectionString = @"Data Source=(local)\SQLEXPRESS2022;Initial Catalog=main;User ID=sa;Password=123";
            
            request = $"SELECT TABLE_NAME FROM {catalog}.INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' order by TABLE_NAME";

            cnn = new SqlConnection(connectionString);
            cnn.Open();

            command = new SqlCommand(request, cnn);
            reader = command.ExecuteReader();

            while (reader.Read())
            {   
                //output += reader.GetValue(0) + " - " + reader.GetValue(1) + "\n";
                
                db_tables.Add(reader.GetString(0));
            }

            reader.Close();
            command.Dispose();

            for (int i = 0; i < db_tables.Count; i++)
            {
                request = $"SELECT COUNT(*) FROM {db_tables[i]}";
                command = new SqlCommand(request, cnn);
                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    listBox1.Items.Add("In " + db_tables[i] + " table: " + Convert.ToString(reader.GetValue(0)) + " values.");
                }

                reader.Close();
                command.Dispose();
            }
            //MessageBox.Show(output);
            cnn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*
             Проверяет соединения с главной БД
             */

            string newElement = comboBox1.Text;

            SqlCommand command;
            SqlDataReader reader;
            String source = HelpFunction.ClearString(comboBox1.Text),
                   catalog = HelpFunction.ClearString(textBox1.Text),
                   login = HelpFunction.ClearString(textBox2.Text),
                   password = HelpFunction.ClearString(textBox3.Text),
                   connectionString,
                   request,
                   response = "";

            SQLManager.CheckConnection(HelpFunction.ClearString(comboBox1.Text),
                                       HelpFunction.ClearString(textBox1.Text),
                                       HelpFunction.ClearString(textBox2.Text),
                                       HelpFunction.ClearString(textBox3.Text));
            /*connectionString = $@"Data Source={source};Initial Catalog={catalog};User ID={login};Password={password};Connect Timeout=30";

            SqlConnection cnn = new SqlConnection(connectionString);

            
            try
            {
                SqlExtensions.QuickOpen(cnn, 30);

                cnn.Open();

                if (textBox1.Text == "")
                {
                    request = "SELECT name FROM sys.databases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb');";

                    command = new SqlCommand(request, cnn);
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        response += Convert.ToString(reader.GetValue(0)) + "\n";
                    }

                    MessageBox.Show(response, "Connected!");

                    reader.Close();
                    command.Dispose();
                }
                
                cnn.Close();
            }
            catch
            {
                MessageBox.Show("Something wrong");
            }*/

            


            /*            if (newElement != "")
                        {
                            comboBox1.Items.Add(newElement);

                            StringCollection coll = new StringCollection();
                            coll.AddRange(comboBox1.Items.Cast<string>().ToArray());
                            Properties.Settings.Default.comboBoxDefault = coll;
                            Properties.Settings.Default.Save();
                        }*/
        }

        private void button3_Click(object sender, EventArgs e)
        {
            /*
             Проверяет соединения с дочерней БД
             */
            if (HelpFunction.ClearString(textBox1.Text) != HelpFunction.ClearString(textBox5.Text))
            {
                SQLManager.CheckConnection(HelpFunction.ClearString(comboBox1.Text),
                                           HelpFunction.ClearString(textBox1.Text),
                                           HelpFunction.ClearString(textBox2.Text),
                                           HelpFunction.ClearString(textBox3.Text));
            }
            else
            {
                MessageBox.Show("Вабрана одна и тажа база данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            /*
             Переход к надстройке слияния. Проверяет соединения с основной и дочерней БД
             */
            for(int i = 0; i < 2; i++)
            {
                SQLManager.CheckConnection(HelpFunction.ClearString(comboBox1.Text),
                                           HelpFunction.ClearString(textBox1.Text),
                                           HelpFunction.ClearString(textBox2.Text),
                                           HelpFunction.ClearString(textBox3.Text));
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
