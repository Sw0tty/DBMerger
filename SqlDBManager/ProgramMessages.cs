using System.Collections.Generic;
using System.Windows.Forms;


namespace SqlDBManager
{
    public static class ProgramMessages
    {
        public static void ErrorMessage()
        {
            MessageBox.Show("В процессе возникла ошибка!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void MergeRulesNotPassedMessage()
        {
            MessageBox.Show("Файл с правилами слияния не выбран!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static DialogResult ConnectionSuccessMessage()
        {
            return MessageBox.Show("Соединение установлено!", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static DialogResult ConnectionErrorMessage()
        {
            return MessageBox.Show("Соединение не удалось!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static DialogResult ConnectionWarningMessage(List<string> response)
        {
            return MessageBox.Show($"Соединение установлено, но не выбран каталог!\n\nДоступные каталоги: {string.Join(", ", response)}",
                                   "Предупреждение",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Warning);
        }

        public static DialogResult UnsupportingCatalog(string catalog)
        {
            return MessageBox.Show($"Каталог '{catalog}' не является базой ПО Архивный фонд или содержит неподдерживаемую версию базы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            MessageBox.Show("Вабрана одна и тажа база данных", "Ошибка соединения", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void CheckConnectingSettings(string catalog)
        {
            MessageBox.Show($"Проверьте настройки соединения с {catalog} БД", "Ошибка соединения", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void UserCanceledMessage()
        {
            MessageBox.Show("Процесс был прерван пользователем", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static DialogResult LogNotSavedMessage()
        {
            return MessageBox.Show("Результаты слияния не были сохранены в файл. Продолжить без выгрузки?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        public static DialogResult CancelMergeProcess()
        {
            return MessageBox.Show("Вы действительно хотите оставить процесс слияния?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        public static DialogResult MakeEdits()
        {
            return MessageBox.Show("Внести правки для продолжения слияния?", "Системное сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        public static void ConnectionToCatalogMessage()
        {
            MessageBox.Show("", "Справка", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void InfoAboutArchiveMessage()
        {
            MessageBox.Show("Надстройка предлагает сразу внести изменения в данные об архиве, если в главной БД содержатся неактуальные данные.", "Справка", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public static void RecalculationMessage()
        {
            MessageBox.Show($"{Consts.TextsConsts.RECULC_V1} - простое объединение без пересчета на основе новых данных. Пересчеты останутся в прежнем состоянии.\n\n" +
                            $"{Consts.TextsConsts.RECULC_V2} - пересчет будет выполнен по БД на основе содержащихся данных, без затрагивания паспортов. Паспорта будут содержать данные главной БД до слияния.\n\n" +
                            $"{Consts.TextsConsts.RECULC_V3} - пересчет по всем данным, включая пересчет паспортов. Паспорта будут пересчитаны за каждый год с учетом всех новых данных.",
                            "Справка", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void BackUpSaveMessage()
        {
            MessageBox.Show($"{Consts.TextsConsts.RESERVE_COPY_V1} - будет оставлена копия главной БД до слияния в папке с резервными копиями.\n\n" +
                            $"{Consts.TextsConsts.RESERVE_COPY_V2} - в SQL Server будет создана объедененная БД рядом с главной. В слиянии будет участвовать копия главной БД.",
                            "Справка", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void SaveLogMessage()
        {
            MessageBox.Show("Кнопка сохранения лога слияния баз данных становится доступной после попытки слияния. Данные о последнем слиянии хранятся в памяти до следующего нажатия или закрытия программы.", "Справка", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
