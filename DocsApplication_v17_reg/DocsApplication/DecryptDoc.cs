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

namespace DocsApplication
{
    public partial class DecryptDoc : MetroFramework.Forms.MetroForm
    {
        public DecryptDoc()
        {
            InitializeComponent();
            Fillcombo();
            Fillcombo2();
        }

        SqlConnection connection = new SqlConnection(@"Data Source=HOME\SQLEXPRESS;Initial Catalog=Docs;Integrated Security=True");

        public byte[] array = null;
        public string key = "";
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

        private void DecryptDoc_Load(object sender, EventArgs e)
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

        private void metroButton3DecAES_Click(object sender, EventArgs e)
        {
                try
            {
                string p = metroTextBox4.Text;
                string namedoc = metroTextBox3.Text;
                string result = "";
                string file2 = @"D:\Docs\Расшифрованные_документы\" + namedoc + ".doc";
                string alg = metroTextBox5.Text;

                byte[] hash = Encoding.ASCII.GetBytes("" + p + "");
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] hashenc = md5.ComputeHash(hash);

                foreach (var b in hashenc)
                {
                    result += b.ToString("x2");
                }
                String key = result.Substring(3, 16);
                Cryptography.Generic myCrypt = new Cryptography.Generic();
                array = File.ReadAllBytes(metroTextBox6.Text);

                if (alg == "1")
                {
                    if (namedoc != "")
                    {
                        if (p.Length == 16)
                        {
                            File.WriteAllBytes(file2, (myCrypt.Crypt(Cryptography.CryptMethod.DECRYPT, Cryptography.CryptClass.AES, array, key) as byte[]));
                            MetroFramework.MetroMessageBox.Show(this, "Документ расшифрован", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MetroFramework.MetroMessageBox.Show(this, "Ключ должен содержать 16 символов", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Введите название документа", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "Выбран неверный алгоритм расшифрования", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                MetroFramework.MetroMessageBox.Show(this, "Введён неверный ключ или файла не существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

