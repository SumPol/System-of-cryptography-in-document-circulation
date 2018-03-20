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
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection connection = new SqlConnection(@"Data Source=HOME\SQLEXPRESS;Initial Catalog=Docs;Integrated Security=True");
        int i = 0;

        private void metroButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT * FROM Пользователь WHERE Логин = '" + metroTextBox1.Text.Trim() + "'";
                SqlDataAdapter sda = new SqlDataAdapter(query, connection);
                DataTable dtbl = new DataTable();
                sda.Fill(dtbl);
                if (dtbl.Rows.Count == 1)
                {

                    string query3 = "SELECT * FROM Пользователь WHERE Логин = '" + metroTextBox1.Text.Trim() + "'AND Пароль = '" + metroTextBox2.Text.Trim() + "'";
                    SqlDataAdapter sda3 = new SqlDataAdapter(query3, connection);
                    DataTable dtbl3 = new DataTable();
                    sda3.Fill(dtbl3);
                    if (dtbl3.Rows.Count == 1)
                    {
                        MyClass.sValue = Convert.ToString(dtbl3.Rows[0][0]);
                        string curTimeLong = DateTime.Now.ToLongTimeString();
                        string dat = metroDateTime1.Text;

                        connection.Open();
                        SqlCommand xp = new SqlCommand("INSERT INTO Журнал(Id_пользователь, Id_событие, Дата, Время) VALUES(@id_p, @id_sob, @dat, @time)", connection);
                        xp.Parameters.AddWithValue("@id_p", MyClass.sValue);
                        xp.Parameters.AddWithValue("@id_sob", "1");
                        xp.Parameters.AddWithValue("@dat", dat);
                        xp.Parameters.AddWithValue("@time", curTimeLong);
                        xp.ExecuteNonQuery();
                        connection.Close();

                        Main fr2 = new Main();
                        this.Hide();
                        fr2.Show();
                    }
                    else
                    {
                        i++;
                        MetroFramework.MetroMessageBox.Show(this, "Неверный пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (i == 3)
                        {
                            Application.Exit();
                        }
                    }
                }
                else
                {
                    i++;
                    MetroFramework.MetroMessageBox.Show(this, "Неверный логин", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (i == 3)
                    {
                        Application.Exit();
                    }
                }
            }
            catch
            {
                MetroFramework.MetroMessageBox.Show(this, "Нет подключения к базе данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
