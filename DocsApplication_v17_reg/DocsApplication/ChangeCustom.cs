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
    public partial class ChangeCustom : MetroFramework.Forms.MetroForm
    {
        public ChangeCustom()
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

        private void ChangeCustom_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'docsDataSet.Сотрудник' table. You can move, or remove it, as needed.
            this.сотрудникTableAdapter.Fill(this.docsDataSet.Сотрудник);

        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                SqlCommand xp = new SqlCommand("UPDATE Сотрудник SET ФИО = '" + metroTextBox2.Text + "', Паспорт = '" + maskedTextBox1.Text + "', ИНН = '" + metroTextBox3.Text + "', Пенсионное_страхование = '" + maskedTextBox2.Text + "', Телефон = '" + metroTextBox4.Text + "', Адрес = '" + metroTextBox5.Text + "', Индекс = '" + metroTextBox6.Text + "' WHERE Id_сотрудник = '" + metroTextBox7.Text + "'", connection);
                xp.ExecuteNonQuery();
                MetroFramework.MetroMessageBox.Show(this, "Данные изменены", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                connection.Close();
            }
            catch
            {
                MetroFramework.MetroMessageBox.Show(this, "Нет подключения к базе данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
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
            catch
            {
                MetroFramework.MetroMessageBox.Show(this, "Нет подключения к базе данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
