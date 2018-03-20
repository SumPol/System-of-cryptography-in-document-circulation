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
    public partial class Promotion : MetroFramework.Forms.MetroForm
    {
        public Promotion()
        {
            InitializeComponent();
        }

        SqlConnection connection = new SqlConnection(@"Data Source=HOME\SQLEXPRESS;Initial Catalog=Docs;Integrated Security=True");
        private readonly string TemplateFileName = @"D:\Docs\Шаблон\LAW47274_17_20170024_171405.doc";
        private void Promotion_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'docsDataSet.Сотрудник' table. You can move, or remove it, as needed.
            this.сотрудникTableAdapter.Fill(this.docsDataSet.Сотрудник);

        }

        private void metroTextBox1_KeyUp(object sender, KeyEventArgs e)
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

        private void ReplaceWordStub(string stubToReplace, string text, Word.Document wordDocument)
        {
            var range = wordDocument.Content;
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: stubToReplace, ReplaceWith: text);
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            string id_s = metroTextBox2.Text;
            string namedoc = metroTextBox3.Text;
            string dat = metroDateTime1.Text;
            string fio = metroTextBox4.Text;
            string curTimeLong = DateTime.Now.ToLongTimeString();
            string file = @"D:\Docs\Поощрение\" + namedoc + ".doc";

            string nomdoc = "";
            string dolj = "";
            string sp = "";


            if (metroTextBox3.Text != "")
            {
                connection.Open();
                SqlCommand xp = new SqlCommand("INSERT INTO Документ(Id_сотрудник, Id_категория, Наименование_документа, Дата_создания, Месторасположение) VALUES(@id_s, @id_k, @namedoc, @dat, @mest)", connection);
                SqlCommand sd = new SqlCommand("SELECT Наименование_должность FROM Должность WHERE Id_должность = '" + metroTextBox5.Text + "'", connection);
                SqlCommand spodr = new SqlCommand("SELECT Наименование_подразделения FROM Подразделение WHERE Id_подразделения = (SELECT Id_подразделения FROM Должность WHERE Id_должность = '" + metroTextBox5.Text + "')", connection);
                SqlCommand jr = new SqlCommand("INSERT INTO Журнал(Id_пользователь, Id_событие, Дата, Время, Id_документ) VALUES(@id_p, @id_sob, @dat, @time, @id_doc)", connection);
                SqlDataReader reader2 = sd.ExecuteReader();
                if (reader2.Read())
                {
                    dolj = reader2[0].ToString();
                }
                reader2.Close();
                SqlDataReader reader3 = spodr.ExecuteReader();
                if (reader3.Read())
                {
                    sp = reader3[0].ToString();
                }
                reader3.Close();


                xp.Parameters.AddWithValue("@id_s", id_s);
                xp.Parameters.AddWithValue("@id_k", "7");
                xp.Parameters.AddWithValue("@namedoc", namedoc);
                xp.Parameters.AddWithValue("@dat", dat);
                xp.Parameters.AddWithValue("@mest", file);

                xp.CommandText += "; SELECT SCOPE_IDENTITY();";

                nomdoc = Convert.ToString(xp.ExecuteScalar());
                jr.Parameters.AddWithValue("@id_p", MyClass.sValue);
                jr.Parameters.AddWithValue("@id_sob", "2");
                jr.Parameters.AddWithValue("@dat", dat);
                jr.Parameters.AddWithValue("@time", curTimeLong);
                jr.Parameters.AddWithValue("@id_doc", nomdoc);
                jr.ExecuteNonQuery();
                connection.Close();
                try
                {
                    var wordApp = new Word.Application();
                    wordApp.Visible = false;

                    var wordDocument = wordApp.Documents.Open(TemplateFileName);
                    ReplaceWordStub("{nom}", nomdoc, wordDocument);
                    ReplaceWordStub("{date}", dat, wordDocument);
                    ReplaceWordStub("{fio}", fio, wordDocument);
                    ReplaceWordStub("{id}", id_s, wordDocument);
                    ReplaceWordStub("{dolj}", dolj, wordDocument);
                    ReplaceWordStub("{sp}", sp, wordDocument);

                    wordDocument.SaveAs(file);
                    wordApp.Visible = true;
                }
                catch
                {
                    MetroMessageBox.Show(this, "", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                MetroMessageBox.Show(this, "Данные занесены", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MetroMessageBox.Show(this, "Введите название документа", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
