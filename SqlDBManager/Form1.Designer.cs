﻿namespace SqlDBManager
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.mergerInfo = new System.Windows.Forms.Button();
            this.toSettings = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.checkConnectionDaughterCatalog = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.mainDBGroupBox = new System.Windows.Forms.GroupBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.checkConnectionMainCatalog = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.backUp_v2 = new System.Windows.Forms.RadioButton();
            this.backUp_v1 = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.toMerge = new System.Windows.Forms.Button();
            this.backToSelectBases = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.recalc_v3 = new System.Windows.Forms.RadioButton();
            this.recalc_v2 = new System.Windows.Forms.RadioButton();
            this.recalc_v1 = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.cancel = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.startMerge = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.textBoxStatus = new System.Windows.Forms.TextBox();
            this.mergeLog = new System.Windows.Forms.Button();
            this.backToSettings = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.radioButton8 = new System.Windows.Forms.RadioButton();
            this.radioButton9 = new System.Windows.Forms.RadioButton();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.mergerBackWorker = new System.ComponentModel.BackgroundWorker();
            this.dirtyJobBackWorker = new System.ComponentModel.BackgroundWorker();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.mainDBGroupBox.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Сервер:";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(506, 629);
            this.tabControl1.TabIndex = 3;
            this.tabControl1.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl1_Selecting);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tableLayoutPanel1);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.mainDBGroupBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(498, 603);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Подключение";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.mergerInfo, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.toSettings, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 552);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(480, 45);
            this.tableLayoutPanel1.TabIndex = 14;
            // 
            // mergerInfo
            // 
            this.mergerInfo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.mergerInfo.Location = new System.Drawing.Point(3, 5);
            this.mergerInfo.Name = "mergerInfo";
            this.mergerInfo.Size = new System.Drawing.Size(148, 34);
            this.mergerInfo.TabIndex = 12;
            this.mergerInfo.Text = "Справка";
            this.mergerInfo.UseVisualStyleBackColor = true;
            this.mergerInfo.Click += new System.EventHandler(this.mergerInfo_Click);
            this.mergerInfo.MouseEnter += new System.EventHandler(this.mergerInfo_MouseEnter);
            this.mergerInfo.MouseLeave += new System.EventHandler(this.mergerInfo_MouseLeave);
            // 
            // toSettings
            // 
            this.toSettings.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.toSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toSettings.Location = new System.Drawing.Point(329, 5);
            this.toSettings.Name = "toSettings";
            this.toSettings.Size = new System.Drawing.Size(148, 34);
            this.toSettings.TabIndex = 13;
            this.toSettings.Text = "Далее";
            this.toSettings.UseVisualStyleBackColor = true;
            this.toSettings.Click += new System.EventHandler(this.toSettings_Click);
            this.toSettings.MouseEnter += new System.EventHandler(this.toSettings_MouseEnter);
            this.toSettings.MouseLeave += new System.EventHandler(this.toSettings_MouseLeave);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.panel2);
            this.groupBox2.Controls.Add(this.textBox4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.checkConnectionDaughterCatalog);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.comboBox2);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.textBox5);
            this.groupBox2.Controls.Add(this.textBox6);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox2.Location = new System.Drawing.Point(9, 236);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(480, 310);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Дочерняя база данных";
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(6, 226);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(468, 78);
            this.panel2.TabIndex = 12;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(20, 97);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(437, 22);
            this.textBox4.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 16);
            this.label5.TabIndex = 10;
            this.label5.Text = "label5";
            // 
            // checkConnectionDaughterCatalog
            // 
            this.checkConnectionDaughterCatalog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkConnectionDaughterCatalog.Location = new System.Drawing.Point(297, 186);
            this.checkConnectionDaughterCatalog.Name = "checkConnectionDaughterCatalog";
            this.checkConnectionDaughterCatalog.Size = new System.Drawing.Size(175, 34);
            this.checkConnectionDaughterCatalog.TabIndex = 6;
            this.checkConnectionDaughterCatalog.Text = "button3";
            this.checkConnectionDaughterCatalog.UseVisualStyleBackColor = true;
            this.checkConnectionDaughterCatalog.Click += new System.EventHandler(this.checkConnectionDaughterCatalog_Click);
            this.checkConnectionDaughterCatalog.MouseEnter += new System.EventHandler(this.checkConnectionDaughterCatalog_MouseEnter);
            this.checkConnectionDaughterCatalog.MouseLeave += new System.EventHandler(this.checkConnectionDaughterCatalog_MouseLeave);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 79);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 16);
            this.label6.TabIndex = 1;
            this.label6.Text = "label6";
            // 
            // comboBox2
            // 
            this.comboBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(20, 42);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(437, 24);
            this.comboBox2.TabIndex = 5;
            this.comboBox2.Text = "(local)\\SQLEXPRESS";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 126);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 16);
            this.label7.TabIndex = 9;
            this.label7.Text = "label7";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(255, 126);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 16);
            this.label8.TabIndex = 2;
            this.label8.Text = "label8";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(20, 144);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(205, 22);
            this.textBox5.TabIndex = 7;
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(258, 144);
            this.textBox6.Name = "textBox6";
            this.textBox6.PasswordChar = '•';
            this.textBox6.Size = new System.Drawing.Size(199, 22);
            this.textBox6.TabIndex = 8;
            // 
            // mainDBGroupBox
            // 
            this.mainDBGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainDBGroupBox.Controls.Add(this.textBox3);
            this.mainDBGroupBox.Controls.Add(this.label4);
            this.mainDBGroupBox.Controls.Add(this.checkConnectionMainCatalog);
            this.mainDBGroupBox.Controls.Add(this.label1);
            this.mainDBGroupBox.Controls.Add(this.comboBox1);
            this.mainDBGroupBox.Controls.Add(this.label3);
            this.mainDBGroupBox.Controls.Add(this.label2);
            this.mainDBGroupBox.Controls.Add(this.textBox1);
            this.mainDBGroupBox.Controls.Add(this.textBox2);
            this.mainDBGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mainDBGroupBox.Location = new System.Drawing.Point(9, 6);
            this.mainDBGroupBox.Name = "mainDBGroupBox";
            this.mainDBGroupBox.Size = new System.Drawing.Size(480, 224);
            this.mainDBGroupBox.TabIndex = 10;
            this.mainDBGroupBox.TabStop = false;
            this.mainDBGroupBox.Text = "Основная база данных";
            // 
            // textBox3
            // 
            this.textBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox3.Location = new System.Drawing.Point(258, 144);
            this.textBox3.Name = "textBox3";
            this.textBox3.PasswordChar = '•';
            this.textBox3.Size = new System.Drawing.Size(199, 22);
            this.textBox3.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(255, 126);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 16);
            this.label4.TabIndex = 10;
            this.label4.Text = "Пароль:";
            // 
            // checkConnectionMainCatalog
            // 
            this.checkConnectionMainCatalog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkConnectionMainCatalog.Location = new System.Drawing.Point(291, 179);
            this.checkConnectionMainCatalog.Name = "checkConnectionMainCatalog";
            this.checkConnectionMainCatalog.Size = new System.Drawing.Size(181, 34);
            this.checkConnectionMainCatalog.TabIndex = 6;
            this.checkConnectionMainCatalog.Text = "Проверить соединение";
            this.checkConnectionMainCatalog.UseVisualStyleBackColor = true;
            this.checkConnectionMainCatalog.Click += new System.EventHandler(this.checkConnectionMainCatalog_Click);
            this.checkConnectionMainCatalog.MouseEnter += new System.EventHandler(this.checkConnectionMainCatalog_MouseEnter);
            this.checkConnectionMainCatalog.MouseLeave += new System.EventHandler(this.checkConnectionMainCatalog_MouseLeave);
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(20, 40);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(437, 24);
            this.comboBox1.TabIndex = 5;
            this.comboBox1.Text = "(local)\\SQLEXPRESS";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 16);
            this.label3.TabIndex = 9;
            this.label3.Text = "Логин:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Наименование БД:";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox1.Location = new System.Drawing.Point(20, 91);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(437, 22);
            this.textBox1.TabIndex = 7;
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox2.Location = new System.Drawing.Point(20, 144);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(205, 22);
            this.textBox2.TabIndex = 8;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Window;
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.tableLayoutPanel2);
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(498, 603);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Предварительные настройки";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.backUp_v2);
            this.groupBox1.Controls.Add(this.backUp_v1);
            this.groupBox1.Location = new System.Drawing.Point(9, 422);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(480, 48);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Сохранение резервной копии";
            // 
            // backUp_v2
            // 
            this.backUp_v2.AutoSize = true;
            this.backUp_v2.Location = new System.Drawing.Point(187, 20);
            this.backUp_v2.Name = "backUp_v2";
            this.backUp_v2.Size = new System.Drawing.Size(264, 17);
            this.backUp_v2.TabIndex = 1;
            this.backUp_v2.Text = "Создать новую БД с объединенными данными";
            this.backUp_v2.UseVisualStyleBackColor = true;
            // 
            // backUp_v1
            // 
            this.backUp_v1.AutoSize = true;
            this.backUp_v1.Checked = true;
            this.backUp_v1.Location = new System.Drawing.Point(19, 20);
            this.backUp_v1.Name = "backUp_v1";
            this.backUp_v1.Size = new System.Drawing.Size(162, 17);
            this.backUp_v1.TabIndex = 0;
            this.backUp_v1.TabStop = true;
            this.backUp_v1.Text = "Сделать копию до слияния";
            this.backUp_v1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.toMerge, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.backToSelectBases, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(9, 552);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(480, 45);
            this.tableLayoutPanel2.TabIndex = 15;
            // 
            // toMerge
            // 
            this.toMerge.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.toMerge.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toMerge.Location = new System.Drawing.Point(329, 5);
            this.toMerge.Name = "toMerge";
            this.toMerge.Size = new System.Drawing.Size(148, 34);
            this.toMerge.TabIndex = 1;
            this.toMerge.Text = "Далее";
            this.toMerge.UseVisualStyleBackColor = true;
            this.toMerge.Click += new System.EventHandler(this.toMerge_Click);
            this.toMerge.MouseEnter += new System.EventHandler(this.toMerge_MouseEnter);
            this.toMerge.MouseLeave += new System.EventHandler(this.toMerge_MouseLeave);
            // 
            // backToSelectBases
            // 
            this.backToSelectBases.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.backToSelectBases.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.backToSelectBases.Location = new System.Drawing.Point(3, 5);
            this.backToSelectBases.Name = "backToSelectBases";
            this.backToSelectBases.Size = new System.Drawing.Size(148, 34);
            this.backToSelectBases.TabIndex = 0;
            this.backToSelectBases.Text = "Назад";
            this.backToSelectBases.UseVisualStyleBackColor = true;
            this.backToSelectBases.Click += new System.EventHandler(this.backToSelectBases_Click);
            this.backToSelectBases.MouseEnter += new System.EventHandler(this.backToSelectBases_MouseEnter);
            this.backToSelectBases.MouseLeave += new System.EventHandler(this.backToSelectBases_MouseLeave);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.recalc_v3);
            this.groupBox4.Controls.Add(this.recalc_v2);
            this.groupBox4.Controls.Add(this.recalc_v1);
            this.groupBox4.Location = new System.Drawing.Point(9, 367);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(480, 49);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Способ пересчета";
            // 
            // recalc_v3
            // 
            this.recalc_v3.AutoSize = true;
            this.recalc_v3.Location = new System.Drawing.Point(300, 20);
            this.recalc_v3.Name = "recalc_v3";
            this.recalc_v3.Size = new System.Drawing.Size(114, 17);
            this.recalc_v3.TabIndex = 2;
            this.recalc_v3.Text = "Полный пересчет";
            this.recalc_v3.UseVisualStyleBackColor = true;
            // 
            // recalc_v2
            // 
            this.recalc_v2.AutoSize = true;
            this.recalc_v2.Location = new System.Drawing.Point(144, 20);
            this.recalc_v2.Name = "recalc_v2";
            this.recalc_v2.Size = new System.Drawing.Size(150, 17);
            this.recalc_v2.TabIndex = 1;
            this.recalc_v2.Text = "Пересчет без паспортов";
            this.recalc_v2.UseVisualStyleBackColor = true;
            // 
            // recalc_v1
            // 
            this.recalc_v1.AutoSize = true;
            this.recalc_v1.Checked = true;
            this.recalc_v1.Location = new System.Drawing.Point(19, 20);
            this.recalc_v1.Name = "recalc_v1";
            this.recalc_v1.Size = new System.Drawing.Size(119, 17);
            this.recalc_v1.TabIndex = 0;
            this.recalc_v1.TabStop = true;
            this.recalc_v1.Text = "Не пересчитывать";
            this.recalc_v1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.panel1);
            this.groupBox3.Controls.Add(this.radioButton2);
            this.groupBox3.Controls.Add(this.radioButton1);
            this.groupBox3.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(9, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(480, 355);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Информация об архиве";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.Controls.Add(this.textBox11);
            this.panel1.Controls.Add(this.label17);
            this.panel1.Controls.Add(this.textBox9);
            this.panel1.Controls.Add(this.textBox10);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.label16);
            this.panel1.Controls.Add(this.textBox7);
            this.panel1.Controls.Add(this.label15);
            this.panel1.Enabled = false;
            this.panel1.Location = new System.Drawing.Point(6, 51);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(470, 298);
            this.panel1.TabIndex = 10;
            // 
            // textBox11
            // 
            this.textBox11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox11.Location = new System.Drawing.Point(13, 173);
            this.textBox11.Multiline = true;
            this.textBox11.Name = "textBox11";
            this.textBox11.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox11.Size = new System.Drawing.Size(446, 116);
            this.textBox11.TabIndex = 11;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(10, 153);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(123, 17);
            this.label17.TabIndex = 10;
            this.label17.Text = "Описание архива";
            // 
            // textBox9
            // 
            this.textBox9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox9.Location = new System.Drawing.Point(13, 77);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(446, 25);
            this.textBox9.TabIndex = 7;
            // 
            // textBox10
            // 
            this.textBox10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox10.Location = new System.Drawing.Point(13, 125);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(446, 25);
            this.textBox10.TabIndex = 9;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(10, 9);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(163, 17);
            this.label11.TabIndex = 2;
            this.label11.Text = "Краткое наименование";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(10, 105);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(98, 17);
            this.label16.TabIndex = 8;
            this.label16.Text = "Адрес архива";
            // 
            // textBox7
            // 
            this.textBox7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox7.Location = new System.Drawing.Point(13, 29);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(446, 25);
            this.textBox7.TabIndex = 4;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(10, 57);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(159, 17);
            this.label15.TabIndex = 6;
            this.label15.Text = "Полное наименование";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(218, 24);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(90, 21);
            this.radioButton2.TabIndex = 5;
            this.radioButton2.Text = "Поменять";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(19, 24);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(193, 21);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Оставить без изменений";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.cancel);
            this.tabPage3.Controls.Add(this.label14);
            this.tabPage3.Controls.Add(this.startMerge);
            this.tabPage3.Controls.Add(this.label13);
            this.tabPage3.Controls.Add(this.label12);
            this.tabPage3.Controls.Add(this.progressBar2);
            this.tabPage3.Controls.Add(this.progressBar1);
            this.tabPage3.Controls.Add(this.textBoxStatus);
            this.tabPage3.Controls.Add(this.mergeLog);
            this.tabPage3.Controls.Add(this.backToSettings);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(498, 603);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Процесс слияния";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(162, 522);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(174, 34);
            this.cancel.TabIndex = 10;
            this.cancel.Text = "Отмена";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Visible = false;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(19, 473);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(458, 20);
            this.label14.TabIndex = 9;
            this.label14.Text = "Записей обработано: 0";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label14.Visible = false;
            // 
            // startMerge
            // 
            this.startMerge.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.startMerge.Location = new System.Drawing.Point(342, 561);
            this.startMerge.Name = "startMerge";
            this.startMerge.Size = new System.Drawing.Size(148, 34);
            this.startMerge.TabIndex = 8;
            this.startMerge.Text = "Начать слияние";
            this.startMerge.UseVisualStyleBackColor = true;
            this.startMerge.Click += new System.EventHandler(this.startMerge_Click);
            this.startMerge.MouseEnter += new System.EventHandler(this.startMerge_MouseEnter);
            this.startMerge.MouseLeave += new System.EventHandler(this.startMerge_MouseLeave);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(232, 500);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(41, 13);
            this.label13.TabIndex = 7;
            this.label13.Text = "label13";
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.Location = new System.Drawing.Point(19, 457);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(458, 15);
            this.label12.TabIndex = 6;
            this.label12.Text = "Записей импортировано: 0";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(19, 522);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(458, 20);
            this.progressBar2.TabIndex = 5;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(19, 496);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(458, 20);
            this.progressBar1.TabIndex = 4;
            // 
            // textBoxStatus
            // 
            this.textBoxStatus.Location = new System.Drawing.Point(19, 18);
            this.textBoxStatus.Multiline = true;
            this.textBoxStatus.Name = "textBoxStatus";
            this.textBoxStatus.ReadOnly = true;
            this.textBoxStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxStatus.Size = new System.Drawing.Size(458, 435);
            this.textBoxStatus.TabIndex = 3;
            // 
            // mergeLog
            // 
            this.mergeLog.Enabled = false;
            this.mergeLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mergeLog.Location = new System.Drawing.Point(162, 561);
            this.mergeLog.Name = "mergeLog";
            this.mergeLog.Size = new System.Drawing.Size(174, 34);
            this.mergeLog.TabIndex = 1;
            this.mergeLog.Text = "Сохранить итог слияния";
            this.mergeLog.UseVisualStyleBackColor = true;
            this.mergeLog.Click += new System.EventHandler(this.mergeLog_Click);
            this.mergeLog.MouseEnter += new System.EventHandler(this.mergeLog_MouseEnter);
            this.mergeLog.MouseLeave += new System.EventHandler(this.mergeLog_MouseLeave);
            // 
            // backToSettings
            // 
            this.backToSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.backToSettings.Location = new System.Drawing.Point(8, 561);
            this.backToSettings.Name = "backToSettings";
            this.backToSettings.Size = new System.Drawing.Size(148, 34);
            this.backToSettings.TabIndex = 0;
            this.backToSettings.Text = "Назад";
            this.backToSettings.UseVisualStyleBackColor = true;
            this.backToSettings.Click += new System.EventHandler(this.backToSettings_Click);
            this.backToSettings.MouseEnter += new System.EventHandler(this.backToSettings_MouseEnter);
            this.backToSettings.MouseLeave += new System.EventHandler(this.backToSettings_MouseLeave);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.flowLayoutPanel2);
            this.tabPage4.Controls.Add(this.button3);
            this.tabPage4.Controls.Add(this.listView1);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(498, 603);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Tests";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoScroll = true;
            this.flowLayoutPanel2.Controls.Add(this.groupBox5);
            this.flowLayoutPanel2.Controls.Add(this.groupBox6);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(46, 57);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Padding = new System.Windows.Forms.Padding(15, 0, 10, 0);
            this.flowLayoutPanel2.Size = new System.Drawing.Size(217, 117);
            this.flowLayoutPanel2.TabIndex = 14;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.flowLayoutPanel1);
            this.groupBox5.Location = new System.Drawing.Point(18, 3);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(189, 46);
            this.groupBox5.TabIndex = 12;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "groupBox5";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.radioButton8);
            this.flowLayoutPanel1.Controls.Add(this.radioButton9);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(183, 23);
            this.flowLayoutPanel1.TabIndex = 11;
            this.flowLayoutPanel1.Resize += new System.EventHandler(this.flowLayoutPanel1_Resize);
            // 
            // radioButton8
            // 
            this.radioButton8.AutoSize = true;
            this.radioButton8.Location = new System.Drawing.Point(3, 3);
            this.radioButton8.Name = "radioButton8";
            this.radioButton8.Size = new System.Drawing.Size(85, 17);
            this.radioButton8.TabIndex = 0;
            this.radioButton8.TabStop = true;
            this.radioButton8.Text = "radioButton8";
            this.radioButton8.UseVisualStyleBackColor = true;
            // 
            // radioButton9
            // 
            this.radioButton9.AutoSize = true;
            this.radioButton9.Location = new System.Drawing.Point(94, 3);
            this.radioButton9.Name = "radioButton9";
            this.radioButton9.Size = new System.Drawing.Size(85, 17);
            this.radioButton9.TabIndex = 1;
            this.radioButton9.TabStop = true;
            this.radioButton9.Text = "radioButton9";
            this.radioButton9.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Location = new System.Drawing.Point(18, 55);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(189, 48);
            this.groupBox6.TabIndex = 13;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "groupBox6";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(354, 373);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(68, 79);
            this.button3.TabIndex = 9;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // listView1
            // 
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(284, 162);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(174, 127);
            this.listView1.TabIndex = 6;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            // 
            // mergerBackWorker
            // 
            this.mergerBackWorker.WorkerReportsProgress = true;
            this.mergerBackWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.mergerBackWorker_DoWork);
            this.mergerBackWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.mergerBackWorker_ProgressChanged);
            this.mergerBackWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.mergerBackWorker_RunWorkerCompleted);
            // 
            // dirtyJobBackWorker
            // 
            this.dirtyJobBackWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.dirtyJobBackWorker_DoWork);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 629);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Утилита слияния баз данных";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.mainDBGroupBox.ResumeLayout(false);
            this.mainDBGroupBox.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button checkConnectionMainCatalog;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox mainDBGroupBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button checkConnectionDaughterCatalog;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button toSettings;
        private System.Windows.Forms.Button backToSelectBases;
        private System.Windows.Forms.Button toMerge;
        private System.Windows.Forms.Button mergeLog;
        private System.Windows.Forms.Button backToSettings;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button mergerInfo;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.TextBox textBoxStatus;
        private System.ComponentModel.BackgroundWorker mergerBackWorker;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.ComponentModel.BackgroundWorker dirtyJobBackWorker;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button startMerge;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton recalc_v3;
        private System.Windows.Forms.RadioButton recalc_v2;
        private System.Windows.Forms.RadioButton recalc_v1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.RadioButton backUp_v2;
        private System.Windows.Forms.RadioButton backUp_v1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.RadioButton radioButton8;
        private System.Windows.Forms.RadioButton radioButton9;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Panel panel2;
    }
}

