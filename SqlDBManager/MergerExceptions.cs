using System;


namespace SqlDBManager
{
    public class MergerExceptions
    {
        public class ErrorMessages
        {
            public const string WRONG_VERSION = "Версии каталогов не сходятся!";
            public static string NOT_EQUEL_COUNT = $"В главном каталоге {Validator.TablesCount.Item1} таблиц, когда в дочернем {Validator.TablesCount.Item2} таблиц.";
            public const string NOT_EQUEL_NAMES = "Наименования таблиц не совпадают!";
            public const string NOT_ALLOWED_VALUES = "Дефолтные таблицы содержат недопустимые значения!";
        }

        public class StopMergeException : Exception
        {
            public StopMergeException(string message)
                : base(message) { }
        }

        public class ValidationException : Exception
        {
            public ValidationException(string message)
                : base(message) { }
        }
    }
}
