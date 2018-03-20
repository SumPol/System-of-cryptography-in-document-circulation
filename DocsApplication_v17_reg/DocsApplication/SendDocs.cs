using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using MetroFramework;
using System.Data.SqlClient;
using Word = Microsoft.Office.Interop.Word;
using System.Security.Cryptography;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace DocsApplication
{
    public partial class SendDocs : MetroFramework.Forms.MetroForm
    {
        public SendDocs()
        {
            InitializeComponent();
            Fillcombo();
            Fillcombo2();
        }

        SqlConnection connection = new SqlConnection(@"Data Source=HOME\SQLEXPRESS;Initial Catalog=Docs;Integrated Security=True");

        public static string file = "";
        string mail = @"^[A-Za-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$";
        void Fillcombo()
        {

            SqlDataReader myReader;
            string connetionString = null;
            SqlConnection connection;
            connetionString = @"Data Source=HOME\SQLEXPRESS;Initial Catalog=Docs;Integrated Security=True";
            connection = new SqlConnection(connetionString);
            string sql = null;
            sql = "SELECT * FROM Должность";

            try
            {
                connection.Open();
                SqlCommand sqlCmd = new SqlCommand(sql, connection);
                myReader = sqlCmd.ExecuteReader();
                while (myReader.Read())
                {
                    metroComboBox1.Items.Add(myReader["Наименование_должность"].ToString());
                }

                myReader.Close();
            }
            catch
            {
                MetroFramework.MetroMessageBox.Show(this, "Нет подключения к базе данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void Fillcombo2()
        {

            SqlDataReader myReader;
            string connetionString = null;
            SqlConnection connection;
            connetionString = @"Data Source=HOME\SQLEXPRESS;Initial Catalog=Docs;Integrated Security=True";
            connection = new SqlConnection(connetionString);
            string sql = null;
            sql = "SELECT * FROM Категория";

            try
            {
                connection.Open();
                SqlCommand sqlCmd = new SqlCommand(sql, connection);
                myReader = sqlCmd.ExecuteReader();
                while (myReader.Read())
                {
                    metroComboBox2.Items.Add(myReader["Название_категория"].ToString());
                }

                myReader.Close();
            }
            catch
            {
                MetroFramework.MetroMessageBox.Show(this, "Нет подключения к базе данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SendDocs_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'docsDataSet.Шифрованный_документ' table. You can move, or remove it, as needed.
            this.шифрованный_документTableAdapter.Fill(this.docsDataSet.Шифрованный_документ);
            // TODO: This line of code loads data into the 'docsDataSet.Сотрудник' table. You can move, or remove it, as needed.
            this.сотрудникTableAdapter.Fill(this.docsDataSet.Сотрудник);

        }

        private void metroTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM Сотрудник WHERE ФИО LIKE('" + metroTextBox1.Text + "%')";
                cmd.ExecuteNonQuery();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                сотрудникBindingSource.DataSource = dt;

                connection.Close();
            }
            catch
            {
                MetroFramework.MetroMessageBox.Show(this, "Нет подключения к базе данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void metroButtonFind1_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM Шифрованный_документ WHERE id_документ IN (SELECT id_документ FROM Документ WHERE id_сотрудник = '" + metroTextBox7.Text + "')";
                cmd.ExecuteNonQuery();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                шифрованныйдокументBindingSource.DataSource = dt;

                connection.Close();
            }
            catch
            {
                MetroFramework.MetroMessageBox.Show(this, "Нет подключения к базе данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM Шифрованный_документ WHERE id_документ IN (SELECT id_документ FROM Документ WHERE Id_сотрудник IN (SELECT Id_сотрудник FROM Сотрудник WHERE Id_должность = (SELECT Id_должность FROM Должность WHERE Наименование_должность = '" + metroComboBox1.Text + "')))";
                cmd.ExecuteNonQuery();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                шифрованныйдокументBindingSource.DataSource = dt;

                connection.Close();
            }
            catch
            {
                MetroFramework.MetroMessageBox.Show(this, "Нет подключения к базе данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void metroComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM Шифрованный_документ WHERE id_документ IN (SELECT id_документ FROM Документ WHERE Id_категория = (SELECT Id_категория FROM Категория WHERE Название_категория = '" + metroComboBox2.Text + "'))";
                cmd.ExecuteNonQuery();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                шифрованныйдокументBindingSource.DataSource = dt;

                connection.Close();
            }
            catch
            {
                MetroFramework.MetroMessageBox.Show(this, "Нет подключения к базе данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            file = metroTextBox3.Text;
            string namedoc = metroTextBox5.Text;
            string key = metroTextBox4.Text;
            string alg = metroTextBox6.Text;
            Match match = Regex.Match(metroTextBox8.Text, mail);
            if (metroTextBox8.Text != "")
            {
                if (key != "")
                {
                    if (match.Success)
                    {
                        try
                        {
                            SmtpClient client = new SmtpClient("smtp.mail.ru", 25);
                            client.Credentials = new NetworkCredential("shark_snake@mail.ru", "998374d");
                            client.EnableSsl = true;
                            string from = "shark_snake@mail.ru";
                            string to = "" + metroTextBox8.Text + "";
                            string subject = "Данные сотрудника";
                            string text = "Документ: " + namedoc + ", Алгоритм: " + alg + ", " + key + "";
                            MailMessage message = new MailMessage(from, to, subject, text);
                            Attachment sendfile = new Attachment(file);
                            message.Attachments.Add(sendfile);
                            client.Send(message);
                            MetroFramework.MetroMessageBox.Show(this, "Документ отправлен", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch
                        {
                            MetroFramework.MetroMessageBox.Show(this, "Нет сети", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Адрес неверный", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "Отсутствует ключ", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "Введите почту получателя", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
