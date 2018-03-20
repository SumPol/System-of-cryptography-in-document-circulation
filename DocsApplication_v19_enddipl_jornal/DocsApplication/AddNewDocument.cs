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
    public partial class AddNewDocument : MetroFramework.Forms.MetroForm
    {
        public AddNewDocument()
        {
            InitializeComponent();
            Fillcombo();
            Fillcombo2();
        }

        SqlConnection connection = new SqlConnection(@"Data Source=HOME\SQLEXPRESS;Initial Catalog=Docs;Integrated Security=True");

        public static string file = "";
        void Fillcombo()
        {

            SqlDataReader myReader;
            string connetionString = null;
            SqlConnection connection;
            connetionString = @"Data Source=HOME\SQLEXPRESS;Initial Catalog=Docs;Integrated Security=True";
            connection = new SqlConnection(connetionString);
            string sql = null;
            sql = "SELECT * FROM Должность";

            connection.Open();
            SqlCommand sqlCmd = new SqlCommand(sql, connection);
            myReader = sqlCmd.ExecuteReader();
            while (myReader.Read())
            {
                metroComboBox1.Items.Add(myReader["Наименование_должность"].ToString());
            }

            myReader.Close();
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

            connection.Open();
            SqlCommand sqlCmd = new SqlCommand(sql, connection);
            myReader = sqlCmd.ExecuteReader();
            while (myReader.Read())
            {
                metroComboBox2.Items.Add(myReader["Название_категория"].ToString());
            }

            myReader.Close();
        }

        private void AddNewDocument_Load(object sender, EventArgs e)
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

        private void metroButton1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                openFileDialog1.Filter = "doc files (*.doc)|*.doc";
                if (openFileDialog1.ShowDialog() != DialogResult.OK)
                    return;
                metroTextBox4.Text = openFileDialog1.FileName;
            }
        }

        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            connection.Open();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Сотрудник WHERE Id_должность = (SELECT Id_должность FROM Должность WHERE Наименование_должность = '" + metroComboBox1.Text + "')";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            сотрудникBindingSource.DataSource = dt;

            connection.Close();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            file = metroTextBox4.Text;
            string namedoc = metroTextBox3.Text;
            string id_s = metroTextBox2.Text;
            string id_k = "";
            string datcret = metroDateTime1.Text;

            if (file != "")
            {
                if (namedoc != "")
                {
                    if (metroComboBox2.Text != "")
                    {
                        connection.Open();
                        SqlCommand xp = new SqlCommand("INSERT INTO Документ(id_сотрудник, id_категория, Наименование_документа, Дата_создания, Месторасположение) VALUES(@id_s, @id_k, @namedoc, @datcret, @mest)", connection);
                        SqlCommand sl = new SqlCommand("SELECT id_категория FROM Категория WHERE Название_категория = '" + metroComboBox2.Text + "'", connection);
                        SqlCommand jr = new SqlCommand("INSERT INTO Журнал(Id_пользователь, Id_событие, Дата, Время, Id_документ) VALUES(@id_p, @id_sob, @dat, @time, @id_doc)", connection);
                        SqlDataReader reader = sl.ExecuteReader();
                        if (reader.Read())
                        {
                            id_k = reader[0].ToString();
                        }

                        xp.Parameters.AddWithValue("@id_s", id_s);
                        xp.Parameters.AddWithValue("@id_k", id_k);
                        xp.Parameters.AddWithValue("@namedoc", namedoc);
                        xp.Parameters.AddWithValue("@datcret", datcret);
                        xp.Parameters.AddWithValue("@mest", file);
                        reader.Close();
                        xp.CommandText += "; SELECT SCOPE_IDENTITY();";
                        //xp.ExecuteNonQuery();
                        string nomdoc = Convert.ToString(xp.ExecuteScalar());
                        string curTimeLong = DateTime.Now.ToLongTimeString();
                        string dat = metroDateTime2.Text;
                        jr.Parameters.AddWithValue("@id_p", MyClass.sValue);
                        jr.Parameters.AddWithValue("@id_sob", "5");
                        jr.Parameters.AddWithValue("@dat", dat);
                        jr.Parameters.AddWithValue("@time", curTimeLong);
                        jr.Parameters.AddWithValue("@id_doc", nomdoc);
                        jr.ExecuteNonQuery();
                        MetroFramework.MetroMessageBox.Show(this, "Данные занесены", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        connection.Close();
                    }
                    else
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Выберете категорию документа", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "Введите название документа", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "Выберете файл", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
