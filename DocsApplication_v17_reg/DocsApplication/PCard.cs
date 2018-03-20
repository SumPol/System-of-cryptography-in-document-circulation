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

namespace DocsApplication
{
    public partial class PCard : MetroFramework.Forms.MetroForm
    {
        SqlConnection connection = new SqlConnection(@"Data Source=HOME\SQLEXPRESS;Initial Catalog=Docs;Integrated Security=True");
        private readonly string TemplateFileName = @"D:\Docs\Шаблон\LAW47274_2_20170024_171405.doc";
        public PCard()
        {
            InitializeComponent();
        }

        private void PCard_Load(object sender, EventArgs e)
        {
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

        private void ReplaceWordStub(string stubToReplace, string text, Word.Document wordDocument)
        {
            var range = wordDocument.Content;
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: stubToReplace, ReplaceWith: text);
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            string id_s = metroTextBox2.Text;
            string id_d = metroTextBox5.Text;
            string namedoc = metroTextBox3.Text;
            string dat = metroDateTime1.Text;
            string fio = metroTextBox4.Text;
            string file = @"D:\Docs\Личная_карточка\" + namedoc + ".doc";

            string dolj = "";
            string inn = metroTextBox9.Text;
            string pension = metroTextBox8.Text;
            string pol = metroTextBox7.Text;
            string nomtd = "";
            string date2 = "";
            string pasp = metroTextBox6.Text;
            string index = metroTextBox11.Text;
            string adres = metroTextBox12.Text;
            string phone = metroTextBox13.Text;

            if (metroTextBox3.Text != "")
            {
                try
                {
                    connection.Open();
                    SqlCommand xp = new SqlCommand("INSERT INTO Документ(Id_сотрудник, Id_категория, Наименование_документа, Дата_создания, Месторасположение) VALUES(@id_s, @id_k, @namedoc, @dat, @mest)", connection);
                    SqlCommand sd = new SqlCommand("SELECT Наименование_должность FROM Должность WHERE Id_должность = '" + id_d + "'", connection);
                    SqlCommand sl = new SqlCommand("SELECT Id_документ, Дата_создания FROM Документ WHERE (Id_категория = '1' AND Id_сотрудник = (SELECT Id_сотрудник FROM Сотрудник WHERE ФИО = '" + metroTextBox4.Text + "'))", connection);

                    SqlDataReader reader2 = sd.ExecuteReader();
                    if (reader2.Read())
                    {
                        dolj = reader2[0].ToString();
                    }
                    reader2.Close();

                    SqlDataReader reader4 = sl.ExecuteReader();
                    if (reader4.Read())
                    {
                        nomtd = reader4[0].ToString();
                        date2 = reader4[1].ToString();

                    }
                    reader4.Close();
                    xp.Parameters.AddWithValue("@id_s", id_s);
                    xp.Parameters.AddWithValue("@id_k", "2");
                    xp.Parameters.AddWithValue("@namedoc", namedoc);
                    xp.Parameters.AddWithValue("@dat", dat);
                    xp.Parameters.AddWithValue("@mest", file);

                    xp.ExecuteNonQuery();
                    connection.Close();
                }
                catch
                {
                    MetroFramework.MetroMessageBox.Show(this, "Нет подключения к базе данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                try
                {
                    var wordApp = new Word.Application();
                    wordApp.Visible = false;

                    var wordDocument = wordApp.Documents.Open(TemplateFileName);
                    ReplaceWordStub("{date}", dat, wordDocument);
                    ReplaceWordStub("{fio}", fio, wordDocument);
                    ReplaceWordStub("{id}", id_s, wordDocument);
                    ReplaceWordStub("{inn}", inn, wordDocument);
                    ReplaceWordStub("{pension}", pension, wordDocument);
                    ReplaceWordStub("{pol}", pol, wordDocument);
                    ReplaceWordStub("{nomtd}", nomtd, wordDocument);
                    ReplaceWordStub("{dat}", date2, wordDocument);
                    ReplaceWordStub("{pasp}", pasp, wordDocument);
                    ReplaceWordStub("{index}", index, wordDocument);
                    ReplaceWordStub("{adres}", adres, wordDocument);
                    ReplaceWordStub("{phone}", phone, wordDocument);
                    ReplaceWordStub("{dolj}", dolj, wordDocument);

                    wordDocument.SaveAs(file);
                    wordApp.Visible = true;
                }
                catch
                {
                    MetroMessageBox.Show(this, "Шаблон не найден или директория сохранения не существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                MetroMessageBox.Show(this, "Данные занесены", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //connection.Close();
            }
            else
            {
                MetroMessageBox.Show(this, "Введите название документа", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
    }

