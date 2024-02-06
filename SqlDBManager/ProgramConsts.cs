using NotesNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDBManager
{
    public static class VisualConsts
    {
        public const int SPACE_SIZE = 4;
        public const int HEADING_SPACE = 40;
    }

    public static class WorkerConsts
    {
        public const int BLOCK_HEADING = 800;
        public const int MIDDLE_STATUS_CODE = 999;
        public const int ERROR_STATUS_CODE = 500;
        public const string ITS_BLOCK_PROGRESS_BAR = "BLOCK";
        public const string ITS_MAIN_PROGRESS_BAR = "MAIN";
    }

    public static class Consts
    {
        /// <summary>
        /// При истине выбрасывает исключения и прекращает работу с указание места ошибки
        /// </summary>
        public const bool DEBUG_MOD = true;
        public const bool FAST_REQUEST_MOD = true;

        public const int MAX_COUNT_OF_IMPORTS = 1000;
        public static bool MERGE_WAS_SUCCESS = false;
        public static bool LOG_SAVED = false;
        public static string LAST_MAIN_CATALOG = null;
        public static string LAST_DAUGHTER_CATALOG = null;
        public static bool TAB_ACCESS = true;
        public static int COUNT_OF_ALL_TASKS = SpecialTablesValues.DefaultTables.Count +
                                               SpecialTablesValues.WithoutKeysTables.Count +
                                               MergeManager.DefaultTablesParams.Count +
                                               MergeManager.LinksTablesParams.Count +
                                               3 +
                                               100;
        public static int MAIN_PROGRESS_NOW_STATUS = 0;
        public static int COUNT_OF_ALL_BLOCK_TASKS = 0;
        public static int BLOCK_PROGRESS_NOW = 0;

        /// <summary>
        /// Добавляет 1 исполненную задачу
        /// </summary>
        public static int UpdateMainBar()
        {
            MAIN_PROGRESS_NOW_STATUS++;
            return MAIN_PROGRESS_NOW_STATUS * 100 / COUNT_OF_ALL_TASKS;
        }

        public static void ClearTasksBlock()
        {
            COUNT_OF_ALL_BLOCK_TASKS = 0;
            BLOCK_PROGRESS_NOW = 0;
        }

        public static void ClearAllTasks()
        {
            MAIN_PROGRESS_NOW_STATUS = 0;
            COUNT_OF_ALL_BLOCK_TASKS = 0;
            BLOCK_PROGRESS_NOW = 0;
        }

        public static void AddTaskInBlock(int countTasks = 0)
        {
            if (countTasks != 0)
            {
                COUNT_OF_ALL_BLOCK_TASKS += countTasks;
            }
            else
            {
                COUNT_OF_ALL_BLOCK_TASKS++;
            }
        }

        public static int UpdateBlockBar()
        {
            BLOCK_PROGRESS_NOW++;
            return BLOCK_PROGRESS_NOW * 100 / COUNT_OF_ALL_BLOCK_TASKS;
        }

        public static void AddToAllTasks(int countTasks)
        {
            COUNT_OF_ALL_TASKS += countTasks;
        }

        public static void WriteLastCatalogs(string mainCatalog, string daughterCatalog)
        {
            LAST_MAIN_CATALOG = mainCatalog;
            LAST_DAUGHTER_CATALOG = daughterCatalog;
        }
    }
}
