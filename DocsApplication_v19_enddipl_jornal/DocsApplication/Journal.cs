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
    public partial class Journal : MetroFramework.Forms.MetroForm
    {
        public Journal()
        {
            InitializeComponent();
        }

        SqlConnection connection = new SqlConnection(@"Data Source=HOME\SQLEXPRESS;Initial Catalog=Docs;Integrated Security=True");
        private void Journal_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'docsDataSet.View_2' table. You can move, or remove it, as needed.
            this.view_2TableAdapter.Fill(this.docsDataSet.View_2);
            // TODO: This line of code loads data into the 'docsDataSet.View_1' table. You can move, or remove it, as needed.
            this.view_1TableAdapter.Fill(this.docsDataSet.View_1);

        }

        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            connection.Open();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM View_1 WHERE  Название_события = '" + metroComboBox1.Text + "'";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            view1BindingSource.DataSource = dt;

            connection.Close();
        }

        private void metroTextBox2_KeyUp(object sender, KeyEventArgs e)
        {
            connection.Open();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM View_2 WHERE ФИО LIKE('" + metroTextBox2.Text + "%')";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            view2BindingSource.DataSource = dt;

            connection.Close();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            connection.Open();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM View_1 WHERE ФИО = '" + metroTextBox1.Text + "'";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            view1BindingSource.DataSource = dt;

            connection.Close();
        }

        private void metroDateTime1_ValueChanged(object sender, EventArgs e)
        {
            connection.Open();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM View_1 WHERE Дата = '" + metroDateTime1.Text + "'";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            view1BindingSource.DataSource = dt;

            connection.Close();
        }
    }
}
