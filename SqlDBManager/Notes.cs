using SqlDBManager;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NotesNamespace
{
    
    public class Notes
    {
        public int Digital;

        public Notes(int digital) {
            Digital = digital;
        }
    }

    public static class HelpFunction
    {
        public static int RetDigit()
        {
            return 1;
        }
        public static string ClearString(string str)
        {
            return str.Replace(" ", "");
        }
    }

    public static class SQLManager
    {
/*        protected string Catalog;

        public SQLManager(string catalog)
        {
            Catalog = catalog;
        }*/

        public static DialogResult CheckConnection(string source, string catalog, string login, string password)
        {

            SqlCommand command;
            SqlDataReader reader;
            String request, connectionString, response = "";

            connectionString = $@"Data Source={source};Initial Catalog={catalog};User ID={login};Password={password};Connect Timeout=30";

            SqlConnection cnn = new SqlConnection(connectionString);

            try
            {
                SqlExtensions.QuickOpen(cnn, 30);
                
                if (catalog == "")
                {
                    request = "SELECT name FROM sys.databases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb');";

                    command = new SqlCommand(request, cnn);
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        response += Convert.ToString(reader.GetValue(0)) + " ";
                    }

                    reader.Close();
                    command.Dispose();
                    cnn.Close();
                    return MessageBox.Show($"Соединение установлено, но не выбран каталог!\nДоступные каталоги: {response}",
                                            "Предупреждение",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Warning);
                }
                cnn.Close();
                return MessageBox.Show("Соединение установлено!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                return MessageBox.Show("Соединение не удалось!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}


/*

Запросы

--Таблицы С внешними ключами

SELECT name AS [Tables]
FROM sys.tables
WHERE OBJECTPROPERTY(object_id, 'TableHasForeignKey') = 1
ORDER BY[Tables];


--Таблицы БЕЗ внешних ключей

SELECT name AS [Tables]
FROM sys.tables
WHERE OBJECTPROPERTY(object_id, 'TableHasForeignKey') = 0
ORDER BY[Tables];


-- Все таблицы из выбранного каталога БД

$"SELECT TABLE_NAME FROM {catalog}.INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' order by TABLE_NAME";





*/

/*


------Tables_to_clean_up------

1. Таблицы в названии которых есть LOG



------Default_tables------

Запросом у которых нет внешних ключей


------Tables_with_links------

 
 
 */