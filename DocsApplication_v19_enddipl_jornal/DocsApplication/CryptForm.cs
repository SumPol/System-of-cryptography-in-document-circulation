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
    public partial class CryptForm : MetroFramework.Forms.MetroForm
    {
        public CryptForm()
        {
            InitializeComponent();
        }

        private void metroTile4_Click(object sender, EventArgs e)
        {
            ChooseDoc CHDoc = new ChooseDoc();
            CHDoc.Show();
        }

        private void metroTile1_Click(object sender, EventArgs e)
        {
            EncryptDoc EncDoc = new EncryptDoc();
            EncDoc.Show();
        }

        private void metroTile3_Click(object sender, EventArgs e)
        {
            DecryptDoc DecDoc = new DecryptDoc();
            DecDoc.Show();
        }

        private void metroTile2_Click(object sender, EventArgs e)
        {
            SendDocs SDoc = new SendDocs();
            SDoc.Show();
        }
    }
}
