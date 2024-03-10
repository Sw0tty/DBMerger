using System.Collections.Generic;
using System.Drawing;
using System;


namespace SqlDBManager
{
    public static class Consts
    {
        // -- Settings Consts --
        /// <summary>
        /// При истине выбрасывает исключения и прекращает работу с указание места ошибки
        /// </summary>
        public const bool DEBUG_MOD = true;
        public const bool FAST_REQUEST_MOD = true;

        // -- Program Lock Consts --
        private static List<string> SupportingVersions { get; } = new List<string>() { "5.0.0" };
        public static int ALL_OF_IMPORT = 0;
        public static int ALL_OF_CHECK = 0;
        public const int MAX_COUNT_OF_IMPORTS = 1000;
        public static bool MERGE_WAS_SUCCESS = false;
        public static bool LOG_SAVED = false;
        public static string LAST_MAIN_CATALOG = null;
        public static string LAST_DAUGHTER_CATALOG = null;
        public static Tuple<string, string, string, string> PRE_SETTINGS = null;

        public static void WriteLastCatalogs(string mainCatalog, string daughterCatalog)
        {
            LAST_MAIN_CATALOG = mainCatalog;
            LAST_DAUGHTER_CATALOG = daughterCatalog;
        }

        public static List<string> ReturnSupportingVersions()
        {
            return SupportingVersions;
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
            public static bool STOP_MERGE = false;
        }

        public static class RecalcConsts
        {
            public static class CarrierType
            {
                public const string Electronic = "'E'";
                public const string Traditional = "'T'";
            }

            public static class DelteStatus
            {
                public const string PASSPORT_ACTIVE_STATUS = "0253E5AE-57EE-4D21-AAD9-15EBECA3E854";
                public const string PASSPORT_DELETE_STATUS = "4FF026A0-6EEB-4500-90F8-15EBE74B66C9";
            }

            public static class UnitCategory
            {
                public const string OCD = "c";
                public const string UNIQUE = "a";
            }

            public static class UnitKind
            {
                /// <summary>
                /// Единица хранения
                /// </summary>
                public const string KEEPING = "703";

                /// <summary>
                /// Единица учета
                /// </summary>
                public const string ACCOUNTING = "704";
            }

            public static class UnitFeature
            {
                public const string DAMAGED = "16354";
                public const string FLAMED = "10055";
                public const string FADED = "16356";
            }

            public static class UnitWork
            {
                public const string NEED_CARDBOARDED = "9";
                public const string NEED_RESTORATION = "1";
                public const string NEED_BINDING = "2";
                public const string NEED_DISINFECTION = "3";
                public const string NEED_DISINSECTION = "4";
                public const string NEED_ENCIPHERING = "7";
                public const string NEED_COVER_CHANGE = "8";
                public const string NEED_KPO = "5";
            }
        }

        public static class VisualConsts
        {
            public const int SPACE_SIZE = 4;
            public const int HEADING_SPACE = 40;
            public static bool TAB_ACCESS = true;
            public static Font BUTTON_FONT = new Font("Microsoft Sans Serif", 9, FontStyle.Regular);
            public const string TAIL_OF_MERGED_FILES = "_merged";
        }

        public static class TextsConsts
        {
            public const string START_MERGE_BUTTON = "Начать слияние";
            public const string NEXT_BUTTON = "Далее";
            public const string BACK_BUTTON = "Назад";
            public const string LOG_BUTTON = "Сохранить итог слияния";
            public const string CANCEL_BUTTON = "Отмена";
            public const string RECULC_V1 = "Без пересчета";
            public const string RECULC_V2 = "Пересчет без паспортов";
            public const string RECULC_V3 = "Полный пересчет";
            public const string RESERVE_COPY_V1 = "Выгрузить копию до слияния";
            public const string RESERVE_COPY_V2 = "Создать новую БД с объединенными данными";
        }

        public static class MergeWorks
        {
            public const string SKIP = "SKIP";
            public const string CLEARING = "CLEAR";
            public const string DEFAULT_TABLE = "DEFAULT";
            public const string COMPOSITE_TABLE = "COMPOSITE";
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
