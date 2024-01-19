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
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;

            label1.Text = "Сервер:";
            label2.Text = "Наименование базы данных:";
            label3.Text = "Логин:";
            label4.Text = "Пароль:";
            label5.Text = label1.Text;
            label6.Text = label2.Text;
            label7.Text = label3.Text;
            label8.Text = label4.Text;

            checkConnectionMainCatalog.Text = "Проверить соединение";
            checkConnectionDaughterCatalog.Text = checkConnectionMainCatalog.Text;

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

        // Логика первой вкладки формы
        private void checkConnectionMainCatalog_Click(object sender, EventArgs e)
        {
            /*
             Проверяет соединения с главной БД
             */
            if (textBox1.Text.Trim(' ') != textBox4.Text.Trim(' '))
            {
                ConnectionChecker.CheckConnectionMessage(comboBox1.Text.Trim(' '),
                                                     textBox1.Text.Trim(' '),
                                                     textBox2.Text.Trim(' '),
                                                     textBox3.Text.Trim(' '));
            }
            else
            {
                MessageBox.Show("Вабрана одна и тажа база данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkConnectionDaughterCatalog_Click(object sender, EventArgs e)
        {
            /*
             Проверяет соединения с дочерней БД
             */
            if (textBox1.Text.Trim(' ') != textBox4.Text.Trim(' '))
            {
                ConnectionChecker.CheckConnectionMessage(comboBox2.Text.Trim(' '),
                                                         textBox4.Text.Trim(' '),
                                                         textBox5.Text.Trim(' '),
                                                         textBox6.Text.Trim(' '));
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
            if (!ConnectionChecker.CheckConnection(comboBox1.Text.Trim(' '),
                                                   textBox1.Text.Trim(' '),
                                                   textBox2.Text.Trim(' '),
                                                   textBox3.Text.Trim(' ')))
            {
                MessageBox.Show("Проверьте настройки соединения с главной БД", "Ошибка соединения", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!ConnectionChecker.CheckConnection(comboBox2.Text.Trim(' '),
                                                        textBox4.Text.Trim(' '),
                                                        textBox5.Text.Trim(' '),
                                                        textBox6.Text.Trim(' ')))
            {
                MessageBox.Show("Проверьте настройки соединения с дочерней БД", "Ошибка соединения", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (textBox1.Text.Trim(' ') == textBox4.Text.Trim(' '))
            {
                MessageBox.Show("Вабрана одна и тажа база данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // идем на следующую форму
                tabControl1.SelectedIndex++;
            }
        }


        // Логика второй вкладки формы
        private void button5_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex--;
        }

        public void button8_Click(object sender, EventArgs e)
        {
            // Вызов backGroundWorker
            if (!backgroundWorker1.IsBusy)
            {
                // Start the asynchronous operation.
                backgroundWorker1.RunWorkerAsync();
            }
            

            /*
            // 1. Создаем объекты соединения
            // 2. Открываем соединения с БД
            3. Валидируем БД на наименования таблиц. Если не сходится, то закрываем соединение, выдаем ошибку. Иначе идем дальше
            4. Выбираем набор таблиц на каждый акт действий
            5. Очищаем логи
            6. Проходим по дефолтным таблицам
            7. Проходим по таблицам с ключами (провряем на уникальность)
             */
        }



        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            string text1 = "";
            string text2 = "";

            Invoke(new Action(() => text1 = comboBox1.Text.Trim(' ')));
            Invoke(new Action(() => text2 = comboBox1.Text.Trim(' ')));

            // 1. Создаем объекты соединения
            DBCatalog mainCatalog = new DBCatalog(text1, textBox1.Text.Trim(' '), textBox2.Text.Trim(' '), textBox3.Text.Trim(' '));
            DBCatalog daughterCatalog = new DBCatalog(text2, textBox4.Text.Trim(' '), textBox5.Text.Trim(' '), textBox6.Text.Trim(' '));

            // 2. Открываем соединения с БД
            mainCatalog.OpenConnection();
            daughterCatalog.OpenConnection();
            

            Invoke(new Action(() => tabControl1.SelectedIndex++));

/*            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(1000);
            }*/
            //Thread.Sleep(1000);
            //listBox1.Items.Add("Валидируем каталоги на возможность слияния...");

            worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, "Валидируем каталоги на возможность слияния...");
            

            if (mainCatalog.ValidateCountTables(daughterCatalog.SelectCountTables()))
            {
                if (mainCatalog.ValidateNamesTables(daughterCatalog.SelectTablesNames()))
                {
                    // Валидация tblSECURITY_REASON и других дефолтных значений
                    // Сделать завтра же. После этого покопаться в линкованных таблицах. При багах на эти таблицы вернуться.
                    // При обнаружении ошибок предлогать исправить БД
                    // Проверить tblFUND ключ tblFUND - tblFUND

                    if (daughterCatalog.ValidateDefaultTables(worker))
                    {

                        // по окончаю всех валидаций
                        worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, "Валидация успешно завершена!");

                        

                        // --------------
                        // Создаем резервную копию для транзакций
                        /*                    BackupManager backupManager = new BackupManager(mainCatalog.SelectCatalogPath()[0], mainCatalog.ReturnCatalog(), text1, textBox2.Text.Trim(' '), textBox3.Text.Trim(' '));

                                            backupManager.OpenConnection();
                                            backupManager.CreateReserveBackup(mainCatalog.ReturnCatalog());

                                            MessageBox.Show("Created");

                                            mainCatalog.CloseConnection();

                                            backupManager.RestoreFromBackup(mainCatalog.ReturnCatalog());

                                            MessageBox.Show("restored");


                                            backupManager.DeleteReserveBackup();

                                            MessageBox.Show("Deleted");

                                            MessageBox.Show("Break");*/
                        // --------------

                        // Очищаем логи
                        worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, "\r\n" + "--- Очистка логов ---");
                        bool successOperation = MergeManager.ClearLogs(mainCatalog, worker);

                        // Проходим по дефолтным таблицам
                        worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, "\r\n" + "--- Обработка дефолтных таблиц ---");
                        MergeManager.ProcessSkipTables(mainCatalog, daughterCatalog, worker);
                        MergeManager.ProcessDefaultTables(mainCatalog, daughterCatalog, worker);

                        MessageBox.Show("End of default tables");

                        // Проходим по таблицам с ключами (провряем на уникальность)
                        worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, "\r\n" + "--- Обработка таблиц с внешними ключами ---");
                        MergeManager.ProcessLinksTables(mainCatalog, daughterCatalog, worker);



                        e.Result = "Слияние успешно завершено!";
                    }
                    else
                    {
                        e.Result = "Дефолтные таблицы содержат недопустимые значения!";
                        MessageBox.Show("Ошибка валидации!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    //listBox1.Items.Add("Наименования таблиц не совпадает!"); // В функции предусмотреть возвращение ошибочной таблицы
                    //textBoxStatus.AppendText("Наименования таблиц не совпадает!" + "\r\n");
                    e.Result = "Наименования таблиц не совпадают!";
                    // Передать в listBox, что какая-то из таблиц не найдена, отменить слияние

                    MessageBox.Show("Ошибка валидации!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                //listBox1.Items.Add("Количество таблиц в заданных каталогах не равно!");
                //textBoxStatus.AppendText("Количество таблиц в заданных каталогах не равно!" + "\r\n");
                e.Result = "Количество таблиц в заданных каталогах не равно!";
                // Передать в listBox, что количество таблиц не равно, отменить слияние

                MessageBox.Show("Ошибка валидации!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            mainCatalog.CloseConnection();
            daughterCatalog.CloseConnection();
        }

/*        static public void CloseConnections(DBCatalog mainCatalog, DBCatalog daughterCatalog)
        {
            mainCatalog.CloseConnection();
            daughterCatalog.CloseConnection();
        }*/

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == WorkerConsts.MIDDLE_STATUS_CODE)
            {
                textBoxStatus.AppendText(e.UserState.ToString() + "\r\n");
            }
            
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
/*            else if (e.Cancelled)
            {
                // Next, handle the case where the user canceled 
                // the operation.
                // Note that due to a race condition in 
                // the DoWork event handler, the Cancelled
                // flag may not have been set, even though
                // CancelAsync was called.
                resultLabel.Text = "Canceled";
            }*/
            else
            {
                // Finally, handle the case where the operation 
                // succeeded.
                textBoxStatus.AppendText(e.Result.ToString() + "\r\n");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string baseNum = "10000315523";

            long baseToInt = Convert.ToInt64(baseNum);


            Dictionary<string, List<string>> filter = new Dictionary<string, List<string>>() { { "1", new List<string>() { "1", "2", "3", "4", "5", "6" } } };



            string req = $"SELECT * FROM [].[dbo].[] WHERE {string.Join("", filter.Keys)} in ({string.Join(", ", filter[string.Join("", filter.Keys)])})";







            //Dictionary<string, string> filter = new Dictionary<string, string>() { { "some_key", "some_value" } };

            MessageBox.Show(req);

            /*List<Dictionary<string, string>> lst = new List<Dictionary<string, string>>();

            for (int k = 0; k < 5; k++)
            {
                Dictionary<string, string> dictTableData = new Dictionary<string, string>();
                for (int i = 0; i < 5; i++)
                {
                    dictTableData[i.ToString()] = i.ToString();
                }
                lst.Add(new Dictionary<string, string>(dictTableData));
            }

            for (int i = 0; i < lst.Count; i++)
            {
                MessageBox.Show(lst[i]["1"]);
            }*/
            Tuple<string, string> s = new Tuple<string, string>("dffdg", "sdfsd");

            MessageBox.Show(s.Item1 + "---" + s.Item2);

            Dictionary<string, string> f = new Dictionary<string, string>();

            f["1"] = "Apple";

            Dictionary<string, string> ddd = new Dictionary<string, string>(f);

            ddd["1"] = "Melone";
            f["1"] = "dsf";


            MessageBox.Show(f["1"]);
            MessageBox.Show(ddd["1"]);



            string usageName = "";

            // Пример пришедших данных
            // "FUND" -> "ISN_FUND"
            for (int i = 0; i < 10; i++)
            {
                usageName = $"{i}";
            }
            MessageBox.Show(usageName);



            string catalog = "ArchiveFund";
            string sss = $"C:\\Program Files\\Microsoft SQL Server\\MSSQL16.SQLEXPRESS2022\\MSSQL\\DATA\\{catalog}.mdf";
            sss = sss.Replace($"DATA\\{catalog}.mdf", $"Backup\\{catalog}_reserv.bak");

            MessageBox.Show(sss);


            Dictionary<string, Func< List<string>, string>> trest = new Dictionary<string, Func<List<string>, string>> { { "eqUsers", Some } };
            // test button
            string value = "Nothing";
            Dictionary<int, string> dict = new Dictionary<int, string>() { { 1, "1"}, { 2, "2" }, { 3, "3" } };

            for (int i = 1; i <= dict.Count; i++)
            {
                MessageBox.Show(dict[i]);
            }
            //string element = dict.TryGetValue("5", out value);
            string dictStr = "";
            //dict.Remove(element);

            //label9.Text = element;

            foreach(string item in dict.Values)
            {
                if (trest.ContainsKey("eqUsers"))
                {
                    MessageBox.Show(trest["eqUsers"](new List<string> { "login" }));
                }
                dictStr += item;
            }
            label10.Text = dictStr;
        }

        public string Some(List<string> columns)
        {
            List<string> strings = new List<string>() { "[ID]", "[Login]", "[Department]", "[Deleted]", "[OwnerID]", "[CreationDateTime]", "[StatusID]", "[Email]", "[patronymic]", "[Position]", "[Phone]", "[Room_Number]", "[Description]", "[surname]", "[AccessGranted]", "[Supervisor]", "[FirstName]", "[BUILD_IN_ACCOUNT]", "[Pass]", "[Cookie]", "[UserTheme]" };

            string so = "Testtstst";
            return $"SELECT {string.Join(", ", strings).Replace('\"', '\'')} FROM ";
        }


        private void button1_Click(object sender, EventArgs e)
        {
            /*
             На данный момент служит тестовой вкладкой
             */
            /*
             Проверяет соединения с основной БД
             */

            SqlConnection cnn, cnn2;
            SqlCommand command;
            SqlDataReader reader;
            String source,
                   catalog = "test_db",
                   login,
                   password,
                   request,
                   value,
                   connectionString,
                   connectionString2,
                   request2,
                   output = "";
            List<string> db_tables = new List<string>();

            /*Dictionary<string, string> reserveRocords = new Dictionary<string, string>();
            List<string> keys = new List<string>() { "1", "2", "3" };
            List<string> values = new List<string>() { "app", "store", "some" };

            for (int i = 0; i < keys.Count; i++)
            {
                reserveRocords[keys[i]] = values[i];
                MessageBox.Show(keys[i] + "  " + values[i]);
            }

            foreach (string key in reserveRocords.Values)
            {
                MessageBox.Show(key);
            }*/



            Dictionary<string, string> inDict = new Dictionary<string, string>() { { "h", "fd" } };

            List<Dictionary<string, string>> lst = new List<Dictionary<string, string>>();

            lst.Add(inDict);

            Dictionary<string, List<Dictionary<string, string>>> testReserveDict = new Dictionary<string, List<Dictionary<string, string>>>();

            string someTestkey = "testTable";
            Dictionary<string, string> newDict = new Dictionary<string, string>() { { "oldKey", "newKey()" } };

            if (!testReserveDict.ContainsKey(someTestkey))
            {
                testReserveDict[someTestkey] = new List<Dictionary<string, string>>();
            }
            // Добавление нового значения
            testReserveDict[someTestkey].Add(new Dictionary<string, string>() { { "oldKey", "newKey()" } });

            // Получение нового значения
            MessageBox.Show(testReserveDict[someTestkey][0]["oldKey"]);
            DBCatalog testCatalog = new DBCatalog(comboBox1.Text.Trim(' '), "test_db", "sa", "123");
            testCatalog.OpenConnection();


            // ----------------------
            Dictionary<string, string> reserveRocords = new Dictionary<string, string>();
            List<string> keys = testCatalog.SelectColumnsNames("tblCITIZEN_CL");
            List<string> values = testCatalog.SelectRecordsWhere(keys, "tblCITIZEN_CL", "ISN_CITIZEN", "1");

            for (int i = 0; i < keys.Count; i++)
            {
                reserveRocords[keys[i]] = values[i];
            }

            reserveRocords["ISN_CITIZEN"] = "'3'";
            reserveRocords.Remove("ID"); // Обязательно удаляем ID. Он формируется новый в запросе

            testCatalog.InsertValue("tblCITIZEN_CL", reserveRocords);


            MessageBox.Show("BreakPoint test!");
            // ----------------------

            //connectionString = $@"Data Source={source};Initial Catalog={catalog};User ID={login};Password={password}";

            connectionString = @"Data Source=(local)\SQLEXPRESS;Initial Catalog=test_db;User ID=sa;Password=123";

            //connectionString2 = @"Data Source=(local)\SQLEXPRESS;Initial Catalog=test;User ID=sa;Password=123";

            request = $"SELECT TABLE_NAME FROM [{catalog}].INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' order by TABLE_NAME";


            cnn = new SqlConnection(connectionString);
            //cnn2 = new SqlConnection(connectionString2);
            cnn.Open();

            //cnn2.Open();

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
                    //listView1.Items.Add("In " + db_tables[i] + " table: " + Convert.ToString(reader.GetValue(0)) + " values.");
                    listBox2.Items.Add("In " + db_tables[i] + " table: " + Convert.ToString(reader.GetValue(0)) + " values.");
                }

                reader.Close();
                command.Dispose();
            }
            //MessageBox.Show(output);
            cnn.Close();
        }

        private void checkConnectionMainCatalog_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void checkConnectionMainCatalog_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }
    }

    public static class WorkerConsts
    {
        public const int MIDDLE_STATUS_CODE = 999;
    }
}
