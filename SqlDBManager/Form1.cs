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

            label1.Text = "Сервер:";
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

        // Логика первой вкладки формы
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
            /*
             Проверяет соединения с главной БД
             */
            ConnectionChecker.CheckConnectionMessage(comboBox1.Text.Trim(' '),
                                                     textBox1.Text.Trim(' '),
                                                     textBox2.Text.Trim(' '),
                                                     textBox3.Text.Trim(' '));
        }

        private void button3_Click(object sender, EventArgs e)
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
            // 1. Создаем объекты соединения
            DBCatalog mainCatalog = new DBCatalog(comboBox1.Text.Trim(' '), textBox1.Text.Trim(' '), textBox2.Text.Trim(' '), textBox3.Text.Trim(' '));
            DBCatalog daughterCatalog = new DBCatalog(comboBox2.Text.Trim(' '), textBox4.Text.Trim(' '), textBox5.Text.Trim(' '), textBox6.Text.Trim(' '));

            // 2. Открываем соединения с БД
            mainCatalog.OpenConnection();
            daughterCatalog.OpenConnection();
            tabControl1.SelectedIndex++;
            listBox1.Items.Add("Валидируем каталоги на возможность слияния...");

            if (mainCatalog.ValidateCountTables(daughterCatalog.SelectCountTables()))
            {
                if (mainCatalog.ValidateNamesTables(daughterCatalog.SelectTablesNames()))
                {
                    listBox1.Items.Add("Валидация прошла успешно!");

                    //    4. Выбираем набор таблиц на каждый акт действий (DONE)

                    //    5. Очищаем логи
                    MergeManager.ClearLogs(mainCatalog, listBox1);

                    //    6. Проходим по дефолтным таблицам
                    MergeManager.ProcessDefaultTables(mainCatalog, daughterCatalog, listBox1);
                    
                    //    7. Проходим по таблицам с ключами (провряем на уникальность)


                    mainCatalog.CloseConnection();
                    daughterCatalog.CloseConnection();
                }
                else
                {
                    MessageBox.Show("Ошибка валидации!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    listBox1.Items.Add("Наименования таблиц не совпадает!"); // В функции предусмотреть возвращение ошибочной таблицы
                    // Передать в listBox, что какая-то из таблиц не найдена, отменить слияние
                }
            }
            else
            {
                MessageBox.Show("Ошибка валидации!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                listBox1.Items.Add("Количество таблиц в заданных каталогах не равно!");
                // Передать в listBox, что количество таблиц не равно, отменить слияние
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

        private void button10_Click(object sender, EventArgs e)
        {
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

    }
}
