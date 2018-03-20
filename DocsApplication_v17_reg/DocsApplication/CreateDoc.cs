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

namespace DocsApplication
{
    public partial class CreateDoc : MetroFramework.Forms.MetroForm
    {
        public CreateDoc()
        {
            InitializeComponent();
        }

        private void metroTile1_Click(object sender, EventArgs e)
        {
            Hiring HR = new Hiring();
            HR.Show();
        }

        private void metroTile2_Click(object sender, EventArgs e)
        {
            PCard PCd = new PCard();
            PCd.Show();
        }

        private void metroTile3_Click(object sender, EventArgs e)
        {
            TransferJob TJob = new TransferJob();
            TJob.Show();
        }

        private void metroTile4_Click(object sender, EventArgs e)
        {
            Vacation Vac = new Vacation();
            Vac.Show();
        }

        private void metroTile5_Click(object sender, EventArgs e)
        {
            Fire Fr = new Fire();
            Fr.Show();
        }

        private void metroTile6_Click(object sender, EventArgs e)
        {
            BusinessTrip BT = new BusinessTrip();
            BT.Show();
        }

        private void metroTile7_Click(object sender, EventArgs e)
        {
            Promotion Promot = new Promotion();
            Promot.Show();
        }

        private void metroTile8_Click(object sender, EventArgs e)
        {
            AddNewDocument AddNewD = new AddNewDocument();
            AddNewD.Show();
        }
    }
}
