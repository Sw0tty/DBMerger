using System;
using System.Windows.Forms;


namespace SqlDBManager
{
    public partial class InfoForm : Form
    {
        public InfoForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ProgramMessages.ConnectionToCatalogMessage();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ProgramMessages.InfoAboutArchiveMessage();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ProgramMessages.RecalculationMessage();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ProgramMessages.BackUpSaveMessage();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ProgramMessages.SaveLogMessage();
        }
    }
}
