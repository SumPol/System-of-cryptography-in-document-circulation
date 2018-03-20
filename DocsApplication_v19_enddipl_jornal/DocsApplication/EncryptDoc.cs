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
    public partial class EncryptDoc : MetroFramework.Forms.MetroForm
    {
        public EncryptDoc()
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

        private void EncryptDoc_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'docsDataSet.Сотрудник' table. You can move, or remove it, as needed.
            this.сотрудникTableAdapter.Fill(this.docsDataSet.Сотрудник);
            // TODO: This line of code loads data into the 'docsDataSet.Документ' table. You can move, or remove it, as needed.
            this.документTableAdapter.Fill(this.docsDataSet.Документ);

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
            cmd.CommandText = "SELECT * FROM Документ WHERE id_сотрудник = '" + metroTextBox5.Text + "'";
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

        private void metroButton2EncAES_Click(object sender, EventArgs e)
        {
            string p = metroTextBox4.Text;
            string namedoc = metroTextBox3.Text;
            string id_doc = metroTextBox5.Text;
            string keydoc = metroTextBox4.Text;
            string file2 = @"D:\Docs\Шифрованные_документы\" + namedoc + ".doc";
            string result = "";
            try
            {
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

                if (namedoc != "")
                {
                    if (p.Length == 16)
                    {

                        connection.Open();
                        SqlCommand xp = new SqlCommand("INSERT INTO Шифрованный_документ(Id_документ, Название_шифровдок, Ключ, Id_алгоритм, Месторасположение) VALUES(@id_doc, @namedoc, @keydoc, @id_a, @mest)", connection);


                        xp.Parameters.AddWithValue("@id_doc", id_doc);
                        xp.Parameters.AddWithValue("@namedoc", namedoc);
                        xp.Parameters.AddWithValue("@keydoc", keydoc);
                        xp.Parameters.AddWithValue("@id_a", "1");
                        xp.Parameters.AddWithValue("@mest", file2);

                        xp.ExecuteNonQuery();
                        MetroFramework.MetroMessageBox.Show(this, "Данные занесены", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        connection.Close();

                        File.WriteAllBytes(file2, (myCrypt.Crypt(Cryptography.CryptMethod.ENCRYPT, Cryptography.CryptClass.AES, array, key) as byte[]));
                        MetroFramework.MetroMessageBox.Show(this, "Документ зашифрован", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        string dat = metroDateTime1.Text;
                        connection.Open();
                        SqlCommand jr = new SqlCommand("INSERT INTO Журнал(Id_пользователь, Id_событие, Дата, Время, Id_документ) VALUES(@id_p, @id_sob, @dat, @time, @id_doc)", connection);
                        string curTimeLong = DateTime.Now.ToLongTimeString();
                        jr.Parameters.AddWithValue("@id_p", MyClass.sValue);
                        jr.Parameters.AddWithValue("@id_sob", "3");
                        jr.Parameters.AddWithValue("@dat", dat);
                        jr.Parameters.AddWithValue("@time", curTimeLong);
                        jr.Parameters.AddWithValue("@id_doc", id_doc);
                        jr.ExecuteNonQuery();
                        connection.Close();

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
            catch
            {
                MetroFramework.MetroMessageBox.Show(this, "Документ или директория не существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void metroButton5EncRC4_Click(object sender, EventArgs e)
        {
            int key = 0;
            string p = metroTextBox4.Text;
            string namedoc = metroTextBox3.Text;
            string id_doc = metroTextBox5.Text;
            string keydoc = metroTextBox4.Text;
            string result = "";
            string infile = metroTextBox6.Text;
            string outfile = @"D:\Docs\Шифрованные_документы\" + namedoc + ".doc";
            if (infile != "")
            {
                try
                {
                    byte[] hash = Encoding.ASCII.GetBytes("" + p + "");
                    MD5 md5 = new MD5CryptoServiceProvider();
                    byte[] hashenc = md5.ComputeHash(hash);

                    foreach (var b in hashenc)
                    {
                        result += b.ToString("x2");
                    }
                    String par = result.Substring(3, 16);

                    if (namedoc != "")
                    {
                        if (p.Length == 16)
                        {
                            int.TryParse(par, out key);
                            byte[] bytekey = BitConverter.GetBytes(key);

                            var ob = new RC4();
                            ob.text = ob.ReadByteArrayFromFile(infile);
                                ob.init(bytekey);
                                ob.WriteByteArrayToFile(ob.code(), outfile);
                            if (ob.text != null)
                            {
                                connection.Open();
                                SqlCommand xp = new SqlCommand("INSERT INTO Шифрованный_документ(Id_документ, Название_шифровдок, Ключ, Id_алгоритм, Месторасположение) VALUES(@id_doc, @namedoc, @keydoc, @id_a, @mest)", connection);


                                xp.Parameters.AddWithValue("@id_doc", id_doc);
                                xp.Parameters.AddWithValue("@namedoc", namedoc);
                                xp.Parameters.AddWithValue("@keydoc", keydoc);
                                xp.Parameters.AddWithValue("@id_a", "2");
                                xp.Parameters.AddWithValue("@mest", outfile);

                                xp.ExecuteNonQuery();
                                MetroFramework.MetroMessageBox.Show(this, "Данные занесены", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                connection.Close();
                                MetroFramework.MetroMessageBox.Show(this, "Документ зашифрован", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                string dat = metroDateTime1.Text;
                                connection.Open();
                                SqlCommand jr = new SqlCommand("INSERT INTO Журнал(Id_пользователь, Id_событие, Дата, Время, Id_документ) VALUES(@id_p, @id_sob, @dat, @time, @id_doc)", connection);
                                string curTimeLong = DateTime.Now.ToLongTimeString();
                                jr.Parameters.AddWithValue("@id_p", MyClass.sValue);
                                jr.Parameters.AddWithValue("@id_sob", "3");
                                jr.Parameters.AddWithValue("@dat", dat);
                                jr.Parameters.AddWithValue("@time", curTimeLong);
                                jr.Parameters.AddWithValue("@id_doc", id_doc);
                                jr.ExecuteNonQuery();
                                connection.Close();
                            }
                            else
                            {
                                MetroFramework.MetroMessageBox.Show(this, "Выбранный документ не существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            }
                        else
                        {
                            MetroFramework.MetroMessageBox.Show(this, "Введите ключ", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Введите название документа", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch
                {
                    MetroFramework.MetroMessageBox.Show(this, "Выбранный документ не существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "Выбранный документ не существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

