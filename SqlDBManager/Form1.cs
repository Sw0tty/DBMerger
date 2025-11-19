using NotesNamespace;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;


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
            checkConnectionMainCatalog.Text = "Проверить соединение";
            checkConnectionDaughterCatalog.Text = checkConnectionMainCatalog.Text;
            cancel.Location = new System.Drawing.Point(162, 561);

            SetButtonsFont();
            LoadProperties();

            // Only test init
            textBox1.Text = "202_bd_1";
            textBox2.Text = "sa";
            textBox3.Text = "123";
            textBox4.Text = "202_bd_2";
            textBox5.Text = "sa";
            textBox6.Text = "123";

        }

        private void SetButtonsFont()
        {
            button3.Font = Consts.VisualConsts.BUTTON_FONT;
            toSettings.Font = Consts.VisualConsts.BUTTON_FONT;
            backToSelectBases.Font = Consts.VisualConsts.BUTTON_FONT;
            backToSettings.Font = Consts.VisualConsts.BUTTON_FONT;
            toMerge.Font = Consts.VisualConsts.BUTTON_FONT;
            mergerInfo.Font = Consts.VisualConsts.BUTTON_FONT;
            mergeLog.Font = Consts.VisualConsts.BUTTON_FONT;
            startMerge.Font = Consts.VisualConsts.BUTTON_FONT;
            checkConnectionMainCatalog.Font = Consts.VisualConsts.BUTTON_FONT;
            checkConnectionDaughterCatalog.Font = Consts.VisualConsts.BUTTON_FONT;
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
            Consts.VisualConsts.TAB_ACCESS = false;
            if (nextPage)
                tabControl.SelectedIndex++;
            else tabControl.SelectedIndex--;
            Consts.VisualConsts.TAB_ACCESS = true;
        }

        public void SetToDefaultStatusBlock()
        {
            Consts.MergeProgress.ClearAllTasks();
            Consts.MERGE_WAS_SUCCESS = false;
            Consts.LOG_SAVED = false;
            Consts.ALL_OF_IMPORT = 0;
            Consts.ALL_OF_CHECK = 0;
            progressBar1.Value = 0;
            progressBar2.Value = 0;
        }

        private void checkConnectionMainCatalog_Click(object sender, EventArgs e)
        {
            TrimAllOnForm();

            if (textBox1.Text != textBox4.Text && !dirtyJobBackWorker.IsBusy)
            {
                mainDBGroupBox.Enabled = false;
                groupBox2.Enabled = false;
                toSettings.Enabled = false;
                dirtyJobBackWorker.RunWorkerAsync(argument: new Tuple<string, string, string, string>(comboBox1.Text, textBox1.Text, textBox2.Text, textBox3.Text));               
            }
            else
            {
                ProgramMessages.SameCatalogMessage();
            }
        }

        private void checkConnectionDaughterCatalog_Click(object sender, EventArgs e)
        {
            TrimAllOnForm();

            if (textBox1.Text != textBox4.Text && !dirtyJobBackWorker.IsBusy)
            {
                mainDBGroupBox.Enabled = false;
                groupBox2.Enabled = false;
                toSettings.Enabled = false;
                dirtyJobBackWorker.RunWorkerAsync(argument: new Tuple<string, string, string, string>(comboBox2.Text, textBox4.Text, textBox5.Text, textBox6.Text));
            }
            else
            {
                ProgramMessages.SameCatalogMessage();
            }
        }

        private void toSettings_Click(object sender, EventArgs e)
        {
            TrimAllOnForm();

            if (textBox1.Text != textBox4.Text && !dirtyJobBackWorker.IsBusy)
            {
                mainDBGroupBox.Enabled = false;
                groupBox2.Enabled = false;
                toSettings.Enabled = false;
                dirtyJobBackWorker.RunWorkerAsync();
            }
            else
            {
                ProgramMessages.SameCatalogMessage();
            }
        }

        private void backToSelectBases_Click(object sender, EventArgs e)
        {
            WrapTabControl(tabControl1, false);
        }

        public void toMerge_Click(object sender, EventArgs e)
        {
            TrimAllOnForm();
            if (mergeRulesPath.Text == null || mergeRulesPath.Text == "")
            {
                ProgramMessages.MergeRulesNotPassedMessage();
            }
            else
            {
                WrapTabControl(tabControl1, true);
            }
        }

        private void backToSettings_Click(object sender, EventArgs e)
        {
            WrapTabControl(tabControl1, false);
        }

        private void mergerInfo_Click(object sender, EventArgs e)
        {
            InfoForm inforamtion = new InfoForm();
            inforamtion.ShowDialog();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            Consts.StopMergeConsts.STOP_MERGE = true;
        }

        private void mergeLog_Click(object sender, EventArgs e)
        {
            HelpFunction.SaveMergeLog(textBoxStatus.Text);
        }

        private void startMerge_Click(object sender, EventArgs e)
        {
            if (Consts.MERGE_WAS_SUCCESS && !Consts.LOG_SAVED)
            {
                if (ProgramMessages.LogNotSavedMessage() == DialogResult.Yes)
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
            MergeManager mergeManager = new MergeManager(fastRequestMod_RadioButton.Checked, mergeRulesPath.Text);

            string text1 = null;
            string text2 = null;
            bool operationStatusSuccess = true;

            Invoke(new Action(() => {
                text1 = comboBox1.Text;
                text2 = comboBox1.Text;
                mergeLog.Visible = false;
                cancel.Visible = true;
                startMerge.Enabled = false;
                backToSettings.Enabled = false;
                textBoxStatus.Clear();
                label12.Text = "Записей импортировано: 0";
            }));

            DBCatalog mainCatalog = new DBCatalog(text1, textBox1.Text, textBox2.Text, textBox3.Text);
            DBCatalog daughterCatalog = new DBCatalog(text2, textBox4.Text, textBox5.Text, textBox6.Text);

            mainCatalog.OpenConnection();
            daughterCatalog.OpenConnection();
            mainCatalog.StartTransaction();
            daughterCatalog.StartTransaction();

            Consts.MergeProgress.FormTasks(mainCatalog);
            if (recalc_v2.Checked)
                Consts.MergeProgress.AddToAllTasks(2);
            if (recalc_v3.Checked)
                Consts.MergeProgress.AddToAllTasks(3);

            Consts.WriteLastCatalogs(mainCatalog.ReturnCatalogName(), daughterCatalog.ReturnCatalogName());

            worker.ReportProgress(Consts.WorkerConsts.BLOCK_HEADING, "--- Предварительные проверки ---");
            worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, "Валидируем каталоги на возможность слияния...");

            try
            {
                if (!Wrappers.WrapValidator(Validator.ValidateCountTables, mainCatalog, daughterCatalog, worker))
                    throw new MergerExceptions.ValidationException($"В главном каталоге {Validator.TablesCount.Item1} таблиц, когда в дочернем {Validator.TablesCount.Item2} таблиц.");

                if (!Wrappers.WrapValidator(Validator.ValidateNamesTables, mainCatalog, daughterCatalog, worker))
                    throw new MergerExceptions.ValidationException(MergerExceptions.ErrorMessages.NOT_EQUEL_NAMES);

                if (!Wrappers.WrapCustomValidator(Validator.ValidateDefaultTablesValues, mainCatalog, daughterCatalog, worker))
                    throw new MergerExceptions.ValidationException(MergerExceptions.ErrorMessages.NOT_ALLOWED_VALUES);
            }
            catch (MergerExceptions.ValidationException error)
            {
                operationStatusSuccess = false;
                e.Result = error.Message;
                ProgramMessages.ValidationErrorMessage();
            }
            catch (Exception error)
            {
                operationStatusSuccess = false;
                e.Result = error.Message;
                ProgramMessages.ErrorMessage();
            }

            if (operationStatusSuccess)
            {
                worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, "Считываем и валидируем файл правил слияния...");

                try
                {
                    mergeManager.LoadMergeRules();
                }
                catch (Exception error)
                {
                    operationStatusSuccess = false;
                    e.Result = error.Message;
                    ProgramMessages.ErrorMessage();
                }
            }

            if (operationStatusSuccess)
            {
                worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, "Валидация успешно завершена!");
                worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, "Снимаем копию главной БД...");

                BackupManager backupManager = new BackupManager(mainCatalog.SelectCatalogPath(), mainCatalog.ReturnCatalogName(), "master", text1, textBox2.Text, textBox3.Text);
                backupManager.OpenConnection();

                backupManager.SetParams();

                backupManager.CreateReserveBackup();
                if (backUp_v2.Checked)
                {
                    backupManager.RestoreFromBackup();
                    backupManager.DeleteReserveBackup();
                    backupManager.CloseConnection();
                    mainCatalog.CommitTransaction();
                    mainCatalog.CloseConnection();
                    mainCatalog = new DBCatalog(text1, textBox1.Text + Consts.VisualConsts.TAIL_OF_MERGED_FILES, textBox2.Text, textBox3.Text);
                    mainCatalog.OpenConnection();
                    mainCatalog.StartTransaction();
                    backupManager = new BackupManager(mainCatalog.SelectCatalogPath(), mainCatalog.ReturnCatalogName(), "master", text1, textBox2.Text, textBox3.Text);
                }
                else
                {
                    backupManager.CloseConnection();
                }
                worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, "Копия успешно создана!");
                
                // Добавления внешних ссылок по ключам, которые необходимы в процессе слияния
                worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, "Вносим правки ключей таблиц...");
                operationStatusSuccess = Wrappers.WrapSimpleMergeFunc(mergeManager.RepairDBKeys, mainCatalog, worker);

                if (operationStatusSuccess)
                {
                    worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, "Необходимые правки успешно применены!");
                    worker.ReportProgress(Consts.WorkerConsts.BLOCK_HEADING, "--- Очистка выделенных таблиц ---");
                    operationStatusSuccess = mergeManager.ProcessClearingTables(mainCatalog, worker);
                }

                if (operationStatusSuccess)
                {
                    worker.ReportProgress(Consts.WorkerConsts.BLOCK_HEADING, "--- Инициализация менеджера резервных данных ---");
                    ReservedValuesManager.InitClass();
                    worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, HelpFunction.CreateSpace(Consts.VisualConsts.SPACE_SIZE) + "Менеджер подготовлен.");
                    worker.ReportProgress(Consts.WorkerConsts.BLOCK_HEADING, "--- Обработка константных таблиц ---");
                    operationStatusSuccess = mergeManager.ProcessOnlyIdsTables(mainCatalog, daughterCatalog, worker);
                }

                if (operationStatusSuccess)
                {
                    worker.ReportProgress(Consts.WorkerConsts.BLOCK_HEADING, "--- Обработка простых таблиц ---");
                    operationStatusSuccess = mergeManager.ProcessSimpleTables(mainCatalog, daughterCatalog, worker);
                }

                if (operationStatusSuccess)
                {
                    worker.ReportProgress(Consts.WorkerConsts.BLOCK_HEADING, "--- Обработка таблиц с внешними ключами ---");
                    //operationStatusSuccess = MergeManager.ProcessLinksTables(mainCatalog, daughterCatalog, worker);
                    operationStatusSuccess = mergeManager.ProcessLinksTables_NEW(mainCatalog, daughterCatalog, worker);
                }

                if (operationStatusSuccess)
                {
                    worker.ReportProgress(Consts.WorkerConsts.BLOCK_HEADING, "--- Заключение слияния ---");
                    //worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, "Возвращаем временные изменения...");

                    // пункт переименовывает колонки, что не нужно делать с новой логикой
                    // operationStatusSuccess = Wrappers.WrapSimpleMergeFunc(MergeManager.RenameAfterMergeTableColumn, mainCatalog, worker);
                    
                    //worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, "Изменения применены!");
                }

                if (operationStatusSuccess)
                {
                    mainCatalog.CommitTransaction();
                    Consts.MERGE_WAS_SUCCESS = true;

                    if (!recalc_v1.Checked)
                    {
                        mainCatalog.StartTransaction();
                        worker.ReportProgress(Consts.WorkerConsts.BLOCK_HEADING, "--- Приступаем к пересчету ---");
                        RecalcManager recalcManager = new RecalcManager(mainCatalog);
                        worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, "Пересчитываем описи...");
                        recalcManager.RecalcInventory(worker);
                        worker.ReportProgress(Consts.MergeProgress.UpdateMainBar(), Consts.WorkerConsts.ITS_MAIN_PROGRESS_BAR);
                        worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, "Пересчитываем фонды...");
                        recalcManager.RecalcFund(worker);
                        worker.ReportProgress(Consts.MergeProgress.UpdateMainBar(), Consts.WorkerConsts.ITS_MAIN_PROGRESS_BAR);

                        if (recalc_v3.Checked)
                        {
                            worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, "Пересчитываем паспорта...");
                            recalcManager.RecalcAndCreatePassport(worker);
                            worker.ReportProgress(Consts.MergeProgress.UpdateMainBar(), Consts.WorkerConsts.ITS_MAIN_PROGRESS_BAR);
                        }

                        mainCatalog.CommitTransaction();
                        worker.ReportProgress(Consts.WorkerConsts.MIDDLE_STATUS_CODE, "Пересчет завершен!");
                    }

                    e.Result = "Слияние успешно завершено!";
                    ProgramMessages.MergeCompletedMessage();

                }
                else
                {
                    mainCatalog.RollbackTransaction();
                    daughterCatalog.RollbackTransaction();

                    backupManager.OpenConnection();
                    if (backUp_v1.Checked)
                        backupManager.DeleteReserveBackup();
                    else
                    {
                        //mainCatalog.RollbackTransaction();
                        mainCatalog.CloseConnection();
                        //backupManager.OpenConnection();
                        backupManager.DropCatalog();
                    }
                    backupManager.CloseConnection();

                    if (Consts.StopMergeConsts.STOP_MERGE)
                    {
                        Consts.StopMergeConsts.STOP_MERGE = false;
                        ProgramMessages.UserCanceledMessage();
                    }
                    else
                    {
                        ProgramMessages.ErrorMessage();
                    }
                }
            }

            Invoke(new Action(() => {
                cancel.Visible = false;
                mergeLog.Visible = true;
                mergeLog.Enabled = true;
                startMerge.Enabled = true;
                backToSettings.Enabled = true;
            }));

            mainCatalog.CloseConnection();
            daughterCatalog.CloseConnection();
        }

        private void mergerBackWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == Consts.WorkerConsts.BLOCK_HEADING)
            {
                textBoxStatus.AppendText("\r\n" + HelpFunction.CreateSpace(Consts.VisualConsts.HEADING_SPACE) + e.UserState.ToString() + "\r\n");
            }
            else if (e.ProgressPercentage == Consts.WorkerConsts.MIDDLE_STATUS_CODE)
            {
                textBoxStatus.AppendText(e.UserState.ToString() + "\r\n");
            }
            else if (e.ProgressPercentage == Consts.WorkerConsts.ERROR_STATUS_CODE)
            {
                textBoxStatus.AppendText("--- ERROR ---" + "\r\n" + e.UserState.ToString() + "\r\n" + "--- ERROR ---" + "\r\n");
            }
            else if (e.ProgressPercentage == Consts.WorkerConsts.UPDATE_COUNT_OF_IMPORT)
            {
                Invoke(new Action(() => label12.Text = $"Записей импортировано: {Consts.ALL_OF_IMPORT}"));
            }
            else if (e.ProgressPercentage == Consts.WorkerConsts.UPDATE_COUNT_OF_CHECK)
            {
                //label14.Text = $"Записей обработано: {Consts.ALL_OF_CHECK}";
            }
            else if (e.UserState.ToString() == Consts.WorkerConsts.CLEAN_PROGRESS_BAR)
            {
                label13.Text = $"{0} %";
                progressBar1.Value = 0;
            }
            else if (e.UserState.ToString() == Consts.WorkerConsts.ITS_BLOCK_PROGRESS_BAR)
            {
                label13.Text = $"{e.ProgressPercentage} %";
                progressBar1.Value = e.ProgressPercentage;
            }
            else if (e.UserState.ToString() == Consts.WorkerConsts.ITS_MAIN_PROGRESS_BAR)
            {
                progressBar2.Value = e.ProgressPercentage;
            }            
        }

        private void mergerBackWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                ProgramMessages.UserCanceledMessage();
            }
            else if (e.Result != null)
            {
                textBoxStatus.AppendText(e.Result.ToString() + "\r\n");
            }
        }

        private void dirtyJobBackWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (e.Argument != null)
            {
                Tuple<string, string, string, string> takingParams = e.Argument as Tuple<string, string, string, string>;

                ConnectionChecker.CheckConnectionMessage(takingParams.Item1, takingParams.Item2, takingParams.Item3, takingParams.Item4);
            }
            else
            {
                string text1 = null;
                string text2 = null;

                Invoke(new Action(() => {
                    text1 = comboBox1.Text;
                    text2 = comboBox2.Text;
                }));

                if (!ConnectionChecker.CheckConnection(text1, textBox1.Text, textBox2.Text, textBox3.Text))
                {
                    ProgramMessages.CheckConnectingSettings("главной");
                }
                else if (!ConnectionChecker.CheckConnection(text2, textBox4.Text, textBox5.Text, textBox6.Text))
                {
                    ProgramMessages.CheckConnectingSettings("дочерней");
                }
                else
                {
                    Invoke(new Action(() => {
                        SaveProperties();
                        comboBox1.SelectedItem = comboBox1.Text;
                        WrapTabControl(tabControl1, true);
                    }));
                }
            }

            Invoke(new Action(() =>
            {
                mainDBGroupBox.Enabled = true;
                groupBox2.Enabled = true;
                toSettings.Enabled = true;
            }));
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (Consts.DEBUG_MOD)
                e.Cancel = false;
            else
                e.Cancel = Consts.VisualConsts.TAB_ACCESS;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*if (mergerBackWorker.IsBusy)
                Consts.StopMergeConsts.STOP_MERGE = true;*/
        }

        private void getMergeRulesTemplate_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();

            fileDialog.Filter = "JSON file (*.json)|*.json|All files (*.*)|*.*";
            fileDialog.RestoreDirectory = true;
            fileDialog.FileName = "RulesTemplate";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter outputFile = new StreamWriter(fileDialog.FileName))
                {
                    outputFile.WriteLine(Consts.MergeManager.MERGE_RULES_TEMPLATE);
                }
            }
        }

        private void selectMergeRules_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "JSON (*.json)|*.json|All files (*.*)|*.*";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    mergeRulesPath.Text = openFileDialog.FileName;
                }
            }
        }












        private void button3_Click(object sender, EventArgs e)
        {

            



            /*try
            {
                throw new MergerExceptions.WrongVersionException(MergerExceptions.WrongVersionException.SELF_ERROR);
            }
            catch (MergerExceptions.WrongVersionException error)
            {
                MessageBox.Show(error.Message);
            }*/
            /*Thread myThread = new Thread(() => {
                while (true)
                {
                    MessageBox.Show("");
                }
            });

            myThread.Start();*/

            /* DBCatalog testDBCatalog = new DBCatalog(@"(local)\SQLexpress2022", "main", "sa", "123");
             BackgroundWorker plugWorker = new BackgroundWorker();

             testDBCatalog.OpenConnection();
             testDBCatalog.StartTransaction();
             MessageBox.Show(Validator.ValidateVersion(testDBCatalog).ToString());

             RecalcManager recalcManager = new RecalcManager(testDBCatalog);

             //recalcManager.RecalcAndCreatePassport(plugWorker);
             //recalcManager.RecalcPassports();
             recalcManager.RecalcInventory(plugWorker);
             //recalcManager.RecalcFund(plugWorker);



             testDBCatalog.CommitTransaction();
             testDBCatalog.CloseConnection();

             MessageBox.Show("End of recalc tests...");*/
        }

        private void flowLayoutPanel1_Resize(object sender, EventArgs e)
        {
            groupBox5.Height = groupBox5.Height / 2 + flowLayoutPanel1.Height;
        }
    }
}
