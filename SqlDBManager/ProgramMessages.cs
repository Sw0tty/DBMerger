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

        public static DialogResult LogNotSavedMessage()
        {
            return MessageBox.Show("Результаты слияния не были сохранены в файл. Продолжить без сохранения?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        public static void ConnectionToCatalogMessage()
        {
            MessageBox.Show("", "Справка", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void InfoAboutArchiveMessage()
        {
            MessageBox.Show("Надстройка предлагает сразу внести изменения в данные об архиве, если в главной БД содержатся не актуальные данные.", "Справка", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public static void RecalculationMessage()
        {
            MessageBox.Show("Без пересчета - простое объединение без учета внесения новых данных.\n\n" +
                            "Пересчет без паспортов - пересчет будет выполнен по БД, без затрагивания паспортов. Паспорта будут содержать данные главной БД до слияния.\n\n" +
                            "Полный пересчет - пересчет по всем данным включая пересчет паспортов. Паспорта будут пересчитаны за каждый год с учетом всех новых данных.",
                            "Справка", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void BackUpSaveMessage()
        {
            MessageBox.Show("Сделать копию до слияния - будет оставлена копию главной БД до слияния в папке с резервными копиями.\n\n" +
                            "Создать новую БД с объедененными данными - будет создана объедененная БД рядом с главной. В слиянии будет участвовать копия главной БД.",
                            "Справка", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void SaveLogMessage()
        {
            MessageBox.Show("Кнопка сохранения лога слияния баз данных становится доступной после попытки слияния. Данные о последнем слиянии хранится в памяти до следующего нажатия или закрытия программы.", "Справка", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
