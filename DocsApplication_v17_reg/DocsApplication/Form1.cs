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

        private void metroButton3_Click(object sender, EventArgs e)
        {
            Main fr2 = new Main();
            this.Hide();
            fr2.Show();
        }
    }
}
