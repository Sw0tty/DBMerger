using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SqlDBManager
{
    public static class ReservedValuesManager
    {
        public static void InitClass()
        {
            ReservedKeyPairs = new Dictionary<string, List<KeyPair>>();
            TablesForeignKeys = new Dictionary<string, List<ForeignKey>>();
            ValuesToInsert = new List<Dictionary<string, string>>();
        }

        public class KeyPair
        {
            public string idMainCatalog;
            public string idDaughterCatalog;
            public bool existingInMainCatalog;

            public KeyPair(string idMainCatalogValue, string idDaughterCatalogValue, bool existingInMainCatalogValue)
            {
                idMainCatalog = idMainCatalogValue;
                idDaughterCatalog = idDaughterCatalogValue;
                existingInMainCatalog = existingInMainCatalogValue;
            }
        }

        public class ForeignKey
        {
            public string columnWithForeignKey;
            public string linkOnTable;
            public string linkOnColumn;

            public ForeignKey(string columnWithForeignKeyValue, string linkOnTableValue, string linkOnColumnValue)
            {
                columnWithForeignKey = columnWithForeignKeyValue;
                linkOnTable = linkOnTableValue;
                linkOnColumn = linkOnColumnValue;
            }
        }

        // "myTable": [
        //      { "idMainCatalog": "22", "idDaughterCatalog": "54" },
        //      { "idMainCatalog": "23", "idDaughterCatalog": "623" },
        //      { "idMainCatalog": "24", "idDaughterCatalog": "3" },
        // ]
        /// <summary>
        /// Словарь с наименованиями таблиц в роли ключа и списка словарей типа "Новый ключ - Старный ключ" в роли значения
        /// </summary>
        static Dictionary<string, List<KeyPair>> ReservedKeyPairs { get; set; } = new Dictionary<string, List<KeyPair>>();

        // "myTable": [
        //      { "columnWithForeignKey": "user_id", "linkOnTable": "tblUsers" },
        //      { "columnWithForeignKey": "category_id", "linkOnTable": "tblCategories" },
        // ]
        /// <summary>
        /// Словарь со структурой связей между таблицами для обновления данных в составных таблицах
        /// </summary>
        static Dictionary<string, List<ForeignKey>> TablesForeignKeys { get; set; } = new Dictionary<string, List<ForeignKey>>();

        /// <summary>
        /// Уникальные значения с проверяемой таблицы, которые необходимы импортировать в главный каталог
        /// </summary>
        static List<Dictionary<string, string>> ValuesToInsert { get; set; } = new List<Dictionary<string, string>>();

        public static Dictionary<string, List<KeyPair>> GetReservedKeyPairs()
        {
            return ReservedKeyPairs;
        }

        public static List<KeyPair> GetReservedKeyPairsByTable(string tableName, bool onlyNew)
        {
            if (onlyNew)
                return ReservedKeyPairs[tableName].Where(row => row.existingInMainCatalog == false).ToList();
            return ReservedKeyPairs[tableName];
        }

        public static Dictionary<string, List<ForeignKey>> GetTablesForeignKeys()
        {
            return TablesForeignKeys;
        }

        public static List<ForeignKey> GetForeignKeysByTable(string tableName)
        {
            return TablesForeignKeys[tableName];
        }

        public static List<Dictionary<string, string>> GetValuesToInsert(string orderBy = null)
        {
            if (orderBy != null)
            {
                ValuesToInsert.OrderBy(row => row[orderBy]).ToList();
            }
            return ValuesToInsert;
        }

        public static KeyPair FindReservedKeyPair(string tableName, string idMainCatalog = null, string idDaughterCatalog = null)
        {
            foreach (var pair in ReservedKeyPairs[tableName])
            {
                if (idMainCatalog != null && pair.idMainCatalog == idMainCatalog)
                {
                    return pair;
                }
                if (idDaughterCatalog != null && pair.idDaughterCatalog == idDaughterCatalog)
                {
                    return pair;
                }
            }
            return null;
        }

        /// <summary>
        /// Метод добавления новой пары в список ключей
        /// </summary>
        public static void AddPairToReservedPairs(string tableName, string idMainCatalog, string idDaughterCatalog, bool existingInMainCatalog)
        {
            if (!ReservedKeyPairs.ContainsKey(tableName))
            {
                ReservedKeyPairs.Add(tableName, new List<KeyPair>());
            }

            foreach (var pair in ReservedKeyPairs[tableName])
            {
                if (pair.idMainCatalog == idMainCatalog && pair.idDaughterCatalog == idDaughterCatalog)
                {
                    Console.WriteLine(pair.idMainCatalog + "  " + pair.idDaughterCatalog);
                    Console.WriteLine("  ");
                    //
                }
            }
            ReservedKeyPairs[tableName].Add(new KeyPair(idMainCatalog, idDaughterCatalog, existingInMainCatalog));
        }

        /// <summary>
        /// 
        /// </summary>
        public static void AddForeignKeys(Dictionary<string, string> foreignKeys, string currentTableName)
        {
            foreach (var tableName in foreignKeys.Keys)
            {
                if (!TablesForeignKeys.ContainsKey(tableName))
                {
                    TablesForeignKeys.Add(tableName, new List<ForeignKey>());
                }

                TablesForeignKeys[tableName].Add(new ForeignKey(foreignKeys[tableName], currentTableName, null));
            }
        }

        public static void AddValueToListOfInserts(Dictionary<string, string> value)
        {
            ValuesToInsert.Add(new Dictionary<string, string>(value));
        }

        public static void ClearValuesToInsert()
        {
            ValuesToInsert.Clear();
        }
    }
}
