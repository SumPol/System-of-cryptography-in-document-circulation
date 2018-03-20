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
    public partial class Main : MetroFramework.Forms.MetroForm
    {
        public Main()
        {
            InitializeComponent();
        }

        private void metroTile4_Click(object sender, EventArgs e)
        {
            ListCustoms LC = new ListCustoms();
            LC.Show();
        }

        private void metroTile5_Click(object sender, EventArgs e)
        {
            ChangeCustom ChC = new ChangeCustom();
            ChC.Show();
        }

        private void metroTile3_Click(object sender, EventArgs e)
        {
            AddCustoms AddCus = new AddCustoms();
            AddCus.Show();
        }

        private void metroTile1_Click(object sender, EventArgs e)
        {
            ListDocs ListDocs = new ListDocs();
            ListDocs.Show();
        }

        private void metroLink1_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            this.Close();
            f1.Show();
        }

        private void metroTile2_Click(object sender, EventArgs e)
        {
            CreateDoc CDoc = new CreateDoc();
            CDoc.Show();
        }

        private void metroTile6_Click(object sender, EventArgs e)
        {
            CryptForm CrFr = new CryptForm();
            CrFr.Show();
        }
    }
}
