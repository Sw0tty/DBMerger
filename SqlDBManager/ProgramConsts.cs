﻿using NotesNamespace;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDBManager
{
    public static class Consts
    {
        /// <summary>
        /// При истине выбрасывает исключения и прекращает работу с указание места ошибки
        /// </summary>
        public const bool DEBUG_MOD = true;
        public const bool FAST_REQUEST_MOD = true;

        public static int ALL_OF_IMPORT = 0;
        public static int ALL_OF_CHECK = 0;
        public const int MAX_COUNT_OF_IMPORTS = 1000;
        public static bool MERGE_WAS_SUCCESS = false;
        public static bool LOG_SAVED = false;
        public static string LAST_MAIN_CATALOG = null;
        public static string LAST_DAUGHTER_CATALOG = null;
        public static bool MAKE_EDITS = false;
        public static Tuple<string, string, string, string> PRE_SETTINGS = null;

        public static void WriteLastCatalogs(string mainCatalog, string daughterCatalog)
        {
            LAST_MAIN_CATALOG = mainCatalog;
            LAST_DAUGHTER_CATALOG = daughterCatalog;
        }

        public static class WorkerConsts
        {
            public const int BLOCK_HEADING = 800;
            public const int MIDDLE_STATUS_CODE = 999;
            public const int ERROR_STATUS_CODE = 505;
            public const int UPDATE_COUNT_OF_IMPORT = 444;
            public const int UPDATE_COUNT_OF_CHECK = 454;
            public const string ITS_BLOCK_PROGRESS_BAR = "BLOCK";
            public const string ITS_MAIN_PROGRESS_BAR = "MAIN";
            public const string CLEAN_PROGRESS_BAR = "CLEAN_BLOCK";
        }

        public static class StopMergeConsts
        {
            public const string STOP_ERROR_MESSAGE = "Операция слияния прервана пользователем.";
        }

        public static class VisualConsts
        {
            public const int SPACE_SIZE = 4;
            public const int HEADING_SPACE = 40;
            public static bool USER_STOP_MERGE = false;
            public static bool TAB_ACCESS = true;
            public static Font BUTTON_FONT = new Font("Microsoft Sans Serif", 9, FontStyle.Regular);
        }

        public static class TextsConsts
        {
            public const string START_MERGE_BUTTON = "Начать слияние";
            public const string NEXT_BUTTON = "Далее";
            public const string BACK_BUTTON = "Назад";
            public const string LOG_BUTTON = "Сохранить итог слияния";
            public const string CANCEL_BUTTON = "Отмена";
        }

        public static class MergeWorks
        {
            public const string SKIP = "SKIP";
            public const string CLEARING = "CLEAR";
            public const string DEFAULT_TABLE = "DEFAULT";
            public const string COMPOSITE_TABLE = "COMPOSITE";
        }

        public static class SettingsChecked
        {
            public static bool UPDATE_ARCHIVE = false;
        }

        public static class MergeProgress
        {
            public static int COUNT_OF_ALL_TASKS = 0;
            public static int MAIN_PROGRESS_NOW_STATUS = 0;
            public static int COUNT_OF_ALL_BLOCK_TASKS = 0;
            public static int BLOCK_PROGRESS_NOW = 0;

            public static void FormTasks(DBCatalog catalog)
            {
                COUNT_OF_ALL_TASKS = catalog.SelectCountTables();
            }

            /// <summary>
            /// Adds 1 completed task
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
        }
    }
}
