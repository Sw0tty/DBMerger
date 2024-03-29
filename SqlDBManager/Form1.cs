﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
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
using System.Transactions;
using System.Windows.Forms.VisualStyles;
using System.IO;

namespace SqlDBManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            mergerBackWorker.WorkerReportsProgress = true;
            mergerBackWorker.WorkerSupportsCancellation = true;

            label1.Text = "Сервер:";
            label2.Text = "Наименование базы данных:";
            label3.Text = "Логин:";
            label4.Text = "Пароль:";
            label5.Text = label1.Text;
            label6.Text = label2.Text;
            label7.Text = label3.Text;
            label8.Text = label4.Text;

            //textBox2.Font = new Font(FontFamily.GenericSansSerif, 14);

            

            textBox2.Text = "sa";
            textBox5.Text = textBox2.Text;
            textBox3.Text = "123";
            textBox6.Text = textBox3.Text;

            checkConnectionMainCatalog.Text = "Проверить соединение";
            checkConnectionDaughterCatalog.Text = checkConnectionMainCatalog.Text;

            

            LoadProperties();
        }

        private void LoadProperties()
        {
            StringCollection combo1Collection = Properties.Settings.Default.comboBox1Default;
            StringCollection combo2Collection = Properties.Settings.Default.comboBox1Default;

            if (combo1Collection != null)
            {
                foreach (var str in combo1Collection)
                {
                    comboBox1.Items.Add(str);
                }
                comboBox1.Text = Properties.Settings.Default.comboBox1LastSelect;
            }
            if (combo2Collection != null)
            {
                foreach (var str in combo2Collection)
                {
                    comboBox2.Items.Add(str);
                }
                comboBox2.Text = Properties.Settings.Default.comboBox2LastSelect;
            }
        }

        public void SaveProperties()
        {
            StringCollection combo1Collection = new StringCollection();
            StringCollection combo2Collection = new StringCollection();

            if (!comboBox1.Items.Contains(comboBox1.Text))
                comboBox1.Items.Add(comboBox1.Text);
            if (!comboBox2.Items.Contains(comboBox2.Text))
                comboBox2.Items.Add(comboBox2.Text);

            combo1Collection.AddRange(comboBox1.Items.Cast<string>().ToArray());
            combo2Collection.AddRange(comboBox2.Items.Cast<string>().ToArray());

            Properties.Settings.Default.comboBox1Default = combo1Collection;
            Properties.Settings.Default.comboBox1LastSelect = comboBox1.Text;
            Properties.Settings.Default.comboBox2Default = combo2Collection;
            Properties.Settings.Default.comboBox2LastSelect = comboBox2.Text;

            Properties.Settings.Default.Save();
        }

        private void TrimAllOnForm()
        {
            comboBox1.Text = comboBox1.Text.Trim(' ');
            textBox1.Text = textBox1.Text.Trim(' ');
            textBox2.Text = textBox2.Text.Trim(' ');
            textBox3.Text = textBox3.Text.Trim(' ');
            comboBox2.Text = comboBox2.Text.Trim(' ');
            textBox4.Text = textBox4.Text.Trim(' ');
            textBox5.Text = textBox5.Text.Trim(' ');
            textBox6.Text = textBox6.Text.Trim(' ');
        }

        public void WrapTabControl(TabControl tabControl, bool nextPage)
        {
            Consts.TAB_ACCESS = false;
            if (nextPage)
                tabControl.SelectedIndex++;
            else tabControl.SelectedIndex--;
            Consts.TAB_ACCESS = true;
        }

        public void SetToDefaultStatusBlock()
        {
            Consts.ClearAllTasks();
            Consts.MERGE_WAS_SUCCESS = false;
            Consts.LOG_SAVED = false;
            progressBar1.Value = 0;
            progressBar2.Value = 0;
        }

        // Логика первой вкладки формы
        private void checkConnectionMainCatalog_Click(object sender, EventArgs e)
        {
            // Проверяет соединения с главной БД

            TrimAllOnForm();

            if (textBox1.Text != textBox4.Text && !dirtyJobBackWorker.IsBusy)
            {
                mainDBGroupBox.Enabled = false;
                groupBox2.Enabled = false;
                dirtyJobBackWorker.RunWorkerAsync(argument: new Tuple<string, string, string, string>(comboBox1.Text, textBox1.Text, textBox2.Text, textBox3.Text));               
            }
            else
            {
                ProgramMessages.SameCatalogMessage();
            }
        }

        private void checkConnectionDaughterCatalog_Click(object sender, EventArgs e)
        {
            // Проверяет соединения с дочерней БД

            TrimAllOnForm();

            if (textBox1.Text != textBox4.Text && !dirtyJobBackWorker.IsBusy)
            {
                mainDBGroupBox.Enabled = false;
                groupBox2.Enabled = false;
                dirtyJobBackWorker.RunWorkerAsync(argument: new Tuple<string, string, string, string>(comboBox2.Text, textBox4.Text, textBox5.Text, textBox6.Text));
            }
            else
            {
                ProgramMessages.SameCatalogMessage();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Переход к надстройке слияния. Проверяет соединения с основной и дочерней БД

            TrimAllOnForm();

            if (textBox1.Text != textBox4.Text && !dirtyJobBackWorker.IsBusy)
            {
                mainDBGroupBox.Enabled = false;
                groupBox2.Enabled = false;
                dirtyJobBackWorker.RunWorkerAsync();
            }
            else
            {
                ProgramMessages.SameCatalogMessage();
            }
        }

        // Логика второй вкладки формы
        private void button5_Click(object sender, EventArgs e)
        {
            WrapTabControl(tabControl1, false);
        }

        public void button8_Click(object sender, EventArgs e)
        {
            WrapTabControl(tabControl1, true);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            WrapTabControl(tabControl1, false);
        }

        private void mergeLog_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = "c:\\";
                saveFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.FileName = "MergeLog";
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                    {
                        writer.WriteLine($"Merge log of '{Consts.LAST_MAIN_CATALOG}' and '{Consts.LAST_DAUGHTER_CATALOG}' catalogs. \n");

                        foreach (char s in textBoxStatus.Text)
                        {
                            writer.Write(s);
                        }
                    }
                    Consts.LOG_SAVED = true;
                }
            }
        }

        private void startMerge_Click(object sender, EventArgs e)
        {
            if (Consts.MERGE_WAS_SUCCESS && !Consts.LOG_SAVED)
            {
                if (MessageBox.Show("Результаты слияния не были сохранены в файл. Продолжить без сохранения?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!mergerBackWorker.IsBusy)
                    {
                        SetToDefaultStatusBlock();
                        mergerBackWorker.RunWorkerAsync();
                    }
                }
            }
            else if (!mergerBackWorker.IsBusy)
            {
                SetToDefaultStatusBlock();
                mergerBackWorker.RunWorkerAsync();
            }
        }

        private void mergerBackWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            string text1 = "";
            string text2 = "";

            Invoke(new Action(() => {
                text1 = comboBox1.Text;
                text2 = comboBox1.Text;
                mergeLog.Enabled = false;
                textBoxStatus.Clear();
            }));


            // 1. Создаем объекты соединения
            DBCatalog mainCatalog = new DBCatalog(text1, textBox1.Text, textBox2.Text, textBox3.Text);
            DBCatalog daughterCatalog = new DBCatalog(text2, textBox4.Text, textBox5.Text, textBox6.Text);
            // 2. Открываем соединения с БД
            mainCatalog.OpenConnection();
            daughterCatalog.OpenConnection();

            SqlTransaction transaction = mainCatalog.StartTransaction();
            SqlTransaction transaction2 = daughterCatalog.StartTransaction();

            Consts.WriteLastCatalogs(mainCatalog.ReturnCatalog(), daughterCatalog.ReturnCatalog());

            worker.ReportProgress(WorkerConsts.BLOCK_HEADING, "--- Предварительные проверки ---");
            worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, "Валидируем каталоги на возможность слияния...");

            if (Validator.ValidateVersions(mainCatalog, daughterCatalog))
            {
                if (Validator.ValidateCountTables(mainCatalog, daughterCatalog))
                {
                    if (Validator.ValidateNamesTables(mainCatalog, daughterCatalog))
                    {
                        // daughterCatalog.ValidateDefaultTables(worker)
                        if (Validator.ValidateDefaultTablesValues(mainCatalog, daughterCatalog, worker))
                        {
                            worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, "Валидация успешно завершена!");

                            // после данных запросов создается резервная копия

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




                            worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, "Вносим правки ключей таблиц...");

                            bool successOperation = MergeManager.RepeirDBKeys(mainCatalog, worker);

                            //  ALTER TABLE[5307_main].[dbo].[tblACT] ADD FOREIGN KEY(ISN_FUND) REFERENCES[5307_main].[dbo].[tblFUND](ISN_FUND);

                            if (successOperation)
                            {
                                worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, "Вносим временные изменения в таблицы...");

                                successOperation = MergeManager.RenameBeforeMergeTableColumn(mainCatalog, daughterCatalog, worker);
                            }

                            if (successOperation)
                            {
                                worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, "Необходимые правки успешно применены!");


                                // Очищаем логи
                                worker.ReportProgress(WorkerConsts.BLOCK_HEADING, "--- Очистка логов ---");
                                successOperation = MergeManager.ClearLogs(mainCatalog, worker);
                            }

                            if (successOperation)
                            {
                                // Проходим по дефолтным таблицам
                                worker.ReportProgress(WorkerConsts.BLOCK_HEADING, "--- Обработка дефолтных таблиц ---");
                                MergeManager.ProcessSkipTables(mainCatalog, daughterCatalog, worker);
                                successOperation = MergeManager.ProcessDefaultTables(mainCatalog, daughterCatalog, worker);
                            }

                            MessageBox.Show("End of default tables");

                            if (successOperation)
                            {
                                // Проходим по таблицам с ключами (провряем на уникальность)
                                worker.ReportProgress(WorkerConsts.BLOCK_HEADING, "--- Обработка таблиц с внешними ключами ---");
                                successOperation = MergeManager.ProcessLinksTables(mainCatalog, daughterCatalog, worker);
                            }

                            if (successOperation)
                            {
                                worker.ReportProgress(WorkerConsts.BLOCK_HEADING, "--- Заключение слияния ---");
                                worker.ReportProgress(WorkerConsts.MIDDLE_STATUS_CODE, "Возвращаем временные изменения...");
                                transaction2.Rollback();
                                successOperation = MergeManager.RenameAfterMergeTableColumn(mainCatalog, daughterCatalog, worker);
                            }

                            if (successOperation)
                            {
                                MessageBox.Show("NEXT STEP COMMIT!");
                                transaction.Commit();
                                Consts.MERGE_WAS_SUCCESS = true;

                                e.Result = "Слияние успешно завершено!";
                                ProgramMessages.MergeCompletedMessage();
                            }
                            else
                            {
                                transaction.Rollback();
                                transaction2.Rollback();
                                ProgramMessages.ErrorMessage();
                            }
                        }
                        else
                        {
                            e.Result = "Дефолтные таблицы содержат недопустимые значения!";
                            ProgramMessages.ValidationErrorMessage();
                        }
                    }
                    else
                    {
                        e.Result = "Наименования таблиц не совпадают!";
                        ProgramMessages.ValidationErrorMessage();
                    }
                }
                else
                {
                    e.Result = $"В главном каталоге {Validator.TablesCount.Item1} таблиц, когда в дочернем {Validator.TablesCount.Item2} таблиц.";
                    ProgramMessages.ValidationErrorMessage();
                }
            }
            else
            {
                e.Result = "Версии каталогов не сходятся!";
                ProgramMessages.ValidationErrorMessage();
            }
            Invoke(new Action(() => mergeLog.Enabled = true ));
            mainCatalog.CloseConnection();
            daughterCatalog.CloseConnection();
        }

        private void mergerBackWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == WorkerConsts.BLOCK_HEADING)
            {
                textBoxStatus.AppendText("\r\n" + HelpFunction.CreateSpace(VisualConsts.HEADING_SPACE) + e.UserState.ToString() + "\r\n");
            }
            else if (e.ProgressPercentage == WorkerConsts.MIDDLE_STATUS_CODE)
            {
                textBoxStatus.AppendText(e.UserState.ToString() + "\r\n");
            }
            else if (e.ProgressPercentage == WorkerConsts.ERROR_STATUS_CODE)
            {
                textBoxStatus.AppendText("--- ERROR ---" + "\r\n" + e.UserState.ToString() + "\r\n" + "--- ERROR ---" + "\r\n");
            }
            else if (e.UserState.ToString() == WorkerConsts.ITS_BLOCK_PROGRESS_BAR)
            {
                label13.Text = $"{e.ProgressPercentage} %";
                // progressBar1.Value = e.ProgressPercentage;
            }
            else if (e.UserState.ToString() == WorkerConsts.ITS_MAIN_PROGRESS_BAR)
            {
                progressBar2.Value = e.ProgressPercentage;
            }
            else if (e.ProgressPercentage == WorkerConsts.UPDATE_COUNT_OF_IMPORT)
            {
                label12.Text = $"Записей импортировано: {Consts.ALL_OF_IMPORT}";
            }
            else if (e.ProgressPercentage == WorkerConsts.UPDATE_COUNT_OF_CHECK)
            {
                label13.Text = "Записей обработано: {}";
            }
        }

        private void mergerBackWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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
            else if (e.Result != null)
            {
                // Finally, handle the case where the operation 
                // succeeded.
                textBoxStatus.AppendText(e.Result.ToString() + "\r\n");
            }
        }

        private void dirtyJobBackWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (e.Argument != null)
            {
                Tuple<string, string, string, string> takingParams = e.Argument as Tuple<string, string, string, string>;

                ConnectionChecker.CheckConnectionMessage(takingParams.Item1,
                                                         takingParams.Item2,
                                                         takingParams.Item3,
                                                         takingParams.Item4);
            }
            else
            {
                string text1 = "";
                string text2 = "";

                Invoke(new Action(() => text1 = comboBox1.Text ));
                Invoke(new Action(() => text2 = comboBox1.Text ));

                if (!ConnectionChecker.CheckConnection(text1,
                                                       textBox1.Text,
                                                       textBox2.Text,
                                                       textBox3.Text)){
                    ProgramMessages.CheckConnectingSettings("главной");
                }
                else if (!ConnectionChecker.CheckConnection(text2,
                                                            textBox4.Text,
                                                            textBox5.Text,
                                                            textBox6.Text))
                {
                    ProgramMessages.CheckConnectingSettings("дочерней");
                }
                else
                {
                    Invoke(new Action(() => SaveProperties() ));
                    Invoke(new Action(() => WrapTabControl(tabControl1, true) ));
                }
            }

            Invoke(new Action(() =>
            {
                mainDBGroupBox.Enabled = true;
                groupBox2.Enabled = true;
            }));
        }

        private void dirtyJobBackWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void dirtyJobBackWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (Consts.DEBUG_MOD)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = Consts.TAB_ACCESS;
            }
        }

        private void checkConnectionMainCatalog_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void checkConnectionMainCatalog_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void checkConnectionDaughterCatalog_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void checkConnectionDaughterCatalog_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void button4_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void button8_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void button8_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void button5_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void button5_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void button6_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void button6_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }


























        private void button3_Click(object sender, EventArgs e)
        {
            textBoxStatus.AppendText("dfgfdg" + "\r\n");
            textBoxStatus.AppendText("dfgfdg" + "\r\n");
            textBoxStatus.AppendText("dfgfdg" + "\r\n");
            textBoxStatus.AppendText("dfgfdg" + "\r\n");
            textBoxStatus.AppendText("dfgfdg" + "\r\n");
            textBoxStatus.AppendText("" + "\r\n");
            textBoxStatus.AppendText("dfgfdg" + "\r\n");
            textBoxStatus.AppendText("dfgfdg" + "\r\n");

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = "c:\\";
                saveFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.FileName = "MergeLog";
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                    {
                        writer.WriteLine($"Merge log of '' and '' \n");

                        foreach (char s in textBoxStatus.Text)
                        {
                            writer.Write(s);
                        }
                    }
                }
            }
        }

        public void Mess()
        {
            List<string> list = new List<string>() { "1", "2" };
            string s = "";
            Invoke(new Action(() => { MessageBox.Show("df"); }));
            for (int i = 0; i < 10; i++)
            {
                listBox2.Items.Add(i.ToString());
            }


            MessageBox.Show("df2");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string sss = "'1111'";

            MessageBox.Show("'" + sss.Substring(1, sss.Length - 2).Replace("'", "\''") + "'");
            //tabControl1.SelectedIndex--;




            //for (int i = 10; i <= 100; i += 10)
            //{
            //    for (int j = 10; j <= 100; j += 10)
            //    {
            //        progressBar1.Value = j;
            //        Thread.Sleep(1000);
            //    }
            //    progressBar2.Value = i;
            //}




            bool foo = true;

            if (foo)
            {
                try
                {
                    Mess();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            else
            {
                Mess();
            }

            

            BackupManager backUp = new BackupManager("C:\\Program Files\\Microsoft SQL Server\\MSSQL16.SQL2022\\MSSQL\\DATA\\5558.mdf", "5558", "(local)\\SQL2022", "sa", "123");

            backUp.OpenConnection();

            backUp.CreateReserveBackup("5558");

            MessageBox.Show("sdfsd");
            backUp.DeleteReserveBackup();

            MessageBox.Show("sdfsd");
            string myServer = Environment.MachineName;



            DataTable servers = SqlDataSourceEnumerator.Instance.GetDataSources();
            for (int i = 0; i < servers.Rows.Count; i++)
            {
                if (myServer == servers.Rows[i]["ServerName"].ToString()) ///// used to get the servers in the local machine////
                {
                    if ((servers.Rows[i]["InstanceName"] as string) != null)
                        listBox2.Items.Add(servers.Rows[i]["ServerName"] + "\\" + servers.Rows[i]["InstanceName"]);
                    else
                        listBox2.Items.Add(servers.Rows[i]["ServerName"].ToString());
                }
            }



            SqlDataSourceEnumerator instance = SqlDataSourceEnumerator.Instance;
            DataTable table = instance.GetDataSources();

            MessageBox.Show(table.Rows.Count.ToString());

            foreach (DataRow row in table.Rows)
            {
                if (row["InstanceName"].ToString() == "")
                    listBox2.Items.Add(row["ServerName"]);
                else
                    listBox2.Items.Add(row["ServerName"] + "\\" + row["InstanceName"]);
            }
            /* string baseNum = "10000315523";

             long baseToInt = Convert.ToInt64(baseNum);


             Dictionary<string, List<string>> filter = new Dictionary<string, List<string>>() { { "1", new List<string>() { "1", "2", "3", "4", "5", "6" } } };



             string req = $"SELECT * FROM [].[dbo].[] WHERE {string.Join("", filter.Keys)} in ({string.Join(", ", filter[string.Join("", filter.Keys)])})";







             //Dictionary<string, string> filter = new Dictionary<string, string>() { { "some_key", "some_value" } };

             MessageBox.Show(req);

             *//*List<Dictionary<string, string>> lst = new List<Dictionary<string, string>>();

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
             }*//*
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
             label10.Text = dictStr;*/
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
            //List<string> values = testCatalog.SelectRecordsWhere(keys, "tblCITIZEN_CL", "ISN_CITIZEN", "1");

            /*for (int i = 0; i < keys.Count; i++)
            {
                reserveRocords[keys[i]] = values[i];
            }*/

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

        private void button2_Click(object sender, EventArgs e)
        {
            //Visualizator.VisualizateReserv();

            Tuple<string, string, string> tr = new Tuple<string, string, string>("1", "2", "3");

        }
    }
}
