using System.Windows.Forms;
using System;


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

        private void button4_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void button5_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void button5_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void button6_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void button6_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void button7_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void button7_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }
    }
}
