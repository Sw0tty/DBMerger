using System.Windows.Forms;


namespace SqlDBManager
{
    public static class ProgramMessages
    {
        public static void ErrorMessage()
        {
            MessageBox.Show("В процессе возникла ошибка!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ValidationErrorMessage()
        {
            MessageBox.Show("Ошибка валидации!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void MergeCompletedMessage()
        {
            MessageBox.Show("Слияние успешно завершено!", "Слияние завершено", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void SameCatalogMessage()
        {
            MessageBox.Show("Вабрана одна и тажа база данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void CheckConnectingSettings(string catalog)
        {
            MessageBox.Show($"Проверьте настройки соединения с {catalog} БД", "Ошибка соединения", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void UserCanceledMessage()
        {
            MessageBox.Show("Процесс был остановлен пользователем", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ConnectionToCatalogMessage()
        {
            MessageBox.Show("", "Справка", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void InfoAboutArchiveMessage()
        {
            MessageBox.Show("", "Справка", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public static void RecalculationMessage()
        {
            MessageBox.Show("", "Справка", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void BackUpSaveMessage()
        {
            MessageBox.Show("", "Справка", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void SaveLogMessage()
        {
            MessageBox.Show("", "Справка", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
