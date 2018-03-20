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
    public partial class ListDocs : MetroFramework.Forms.MetroForm
    {
        public ListDocs()
        {
            InitializeComponent();
            Fillcombo();
            Fillcombo2();
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

        private void ListDocs_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'docsDataSet.Документ' table. You can move, or remove it, as needed.
            this.документTableAdapter.Fill(this.docsDataSet.Документ);
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

        private void metroButtonFind1_Click(object sender, EventArgs e)
        {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM Документ WHERE id_сотрудник = '" + metroTextBox3.Text + "'";
                cmd.ExecuteNonQuery();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                документBindingSource.DataSource = dt;

                connection.Close();
        }

        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            connection.Open();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Документ WHERE Id_сотрудник IN (SELECT Id_сотрудник FROM Сотрудник WHERE Id_должность = (SELECT Id_должность FROM Должность WHERE Наименование_должность = '" + metroComboBox1.Text + "'))";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            документBindingSource.DataSource = dt;

            connection.Close();
        }

        private void metroComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            connection.Open();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Документ WHERE Id_категория = (SELECT Id_категория FROM Категория WHERE Название_категория = '" + metroComboBox2.Text + "')";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            документBindingSource.DataSource = dt;

            connection.Close();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            string id_doc = metroTextBox5.Text;
            try
            {
                string place = metroTextBox4.Text;
                string dat = metroDateTime1.Text;
                System.Diagnostics.Process.Start(@"" + place + "");
                connection.Open();
                SqlCommand jr = new SqlCommand("INSERT INTO Журнал(Id_пользователь, Id_событие, Дата, Время, Id_документ) VALUES(@id_p, @id_sob, @dat, @time, @id_doc)", connection);
                string curTimeLong = DateTime.Now.ToLongTimeString();
                jr.Parameters.AddWithValue("@id_p", MyClass.sValue);
                jr.Parameters.AddWithValue("@id_sob", "7");
                jr.Parameters.AddWithValue("@dat", dat);
                jr.Parameters.AddWithValue("@time", curTimeLong);
                jr.Parameters.AddWithValue("@id_doc", id_doc);
                jr.ExecuteNonQuery();
                connection.Close();
            }
            catch
            {
                MetroFramework.MetroMessageBox.Show(this, "Документ или директория не существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
