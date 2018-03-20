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

namespace DocsApplication
{
    public partial class AddCustoms : MetroFramework.Forms.MetroForm
    {
        public AddCustoms()
        {
            InitializeComponent();
            Fillcombo();
        }

        SqlConnection connection = new SqlConnection(@"Data Source=HOME\SQLEXPRESS;Initial Catalog=Docs;Integrated Security=True");

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

        private void metroButtonAdd_Click(object sender, EventArgs e)
        {
            string fio = metroTextBox2.Text;
            string id_d = "";
            string pol = metroComboBox2.Text;
            string pasp = maskedTextBox1.Text;
            string inn = metroTextBox3.Text;
            string pension = maskedTextBox2.Text;
            string phone = metroTextBox4.Text;
            string adr = metroTextBox5.Text;
            string index = metroTextBox6.Text;

            try
            {
                connection.Open();
                SqlCommand xp = new SqlCommand("INSERT INTO Сотрудник(id_должность, ФИО, Пол, Паспорт, ИНН, Пенсионное_страхование, Телефон, Адрес, Индекс) VALUES(@id_d, @fio, @pol, @pasp, @inn, @pension, @phone, @adr, @index)", connection);
                SqlCommand sl = new SqlCommand("SELECT id_должность FROM Должность WHERE Наименование_должность = '" + metroComboBox1.Text + "'", connection);
                SqlDataReader reader = sl.ExecuteReader();
                if (reader.Read())
                {
                    string a = reader[0].ToString();
                    id_d = a;
                }
                xp.Parameters.AddWithValue("@id_d", id_d);
                xp.Parameters.AddWithValue("@fio", fio);
                xp.Parameters.AddWithValue("@pol", pol);
                xp.Parameters.AddWithValue("@pasp", pasp);
                xp.Parameters.AddWithValue("@inn", inn);
                xp.Parameters.AddWithValue("@pension", pension);
                xp.Parameters.AddWithValue("@phone", phone);
                xp.Parameters.AddWithValue("@adr", adr);
                xp.Parameters.AddWithValue("@index", index);
                reader.Close();
                xp.ExecuteNonQuery();
                MetroFramework.MetroMessageBox.Show(this, "Данные занесены", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                connection.Close();
            }
            catch
            {
                MetroFramework.MetroMessageBox.Show(this, "Нет подключения к базе данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
