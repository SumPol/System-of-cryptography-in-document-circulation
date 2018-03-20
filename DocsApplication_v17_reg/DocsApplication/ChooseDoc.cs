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
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace DocsApplication
{
    public partial class ChooseDoc : MetroFramework.Forms.MetroForm
    {
        public ChooseDoc()
        {
            InitializeComponent();
        }

        public byte[] array = null;
        public static string file = "";
        public string key = "";
        public string metk = "";
        string mail = @"^[A-Za-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$";
        private void metroButton1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                openFileDialog1.Filter = "doc files (*.doc)|*.doc";
                if (openFileDialog1.ShowDialog() != DialogResult.OK)
                    return;
                metroTextBox1.Text = openFileDialog1.FileName;
                array = File.ReadAllBytes(metroTextBox1.Text);
            }
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (array != null)
            {
                string p = metroTextBox2.Text;
                string result = "";
                byte[] hash = Encoding.ASCII.GetBytes("" + p + "");
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] hashenc = md5.ComputeHash(hash);

                foreach (var b in hashenc)
                {
                    result += b.ToString("x2");
                }
                String key = result.Substring(3, 16);
                Cryptography.Generic myCrypt = new Cryptography.Generic();
                if (p.Length == 16)
                {
                    saveFileDialog1.Filter = "doc files (*.doc)|*.doc";
                    if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK
                        && saveFileDialog1.FileName.Length > 0)
                    {
                        File.WriteAllBytes(saveFileDialog1.FileName, (myCrypt.Crypt(Cryptography.CryptMethod.ENCRYPT, Cryptography.CryptClass.AES, array, key) as byte[]));
                        file = saveFileDialog1.FileName;
                        metk = "1";
                        MetroFramework.MetroMessageBox.Show(this, "Документ зашифрован", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "Ключ должен содержать 16 символов", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "Выберите файл", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
}

        private void metroButton3DecAES_Click(object sender, EventArgs e)
        {
            if (array != null)
            {
                try
                {
                    string p = metroTextBox2.Text;
                    string result = "";
                    byte[] hash = Encoding.ASCII.GetBytes("" + p + "");
                    MD5 md5 = new MD5CryptoServiceProvider();
                    byte[] hashenc = md5.ComputeHash(hash);

                    foreach (var b in hashenc)
                    {
                        result += b.ToString("x2");
                    }
                    String key = result.Substring(3, 16);
                    Cryptography.Generic myCrypt = new Cryptography.Generic();
                    if (p.Length == 16)
                    {
                        saveFileDialog2.Filter = "doc files (*.doc)|*.doc";
                        if (saveFileDialog2.ShowDialog() == System.Windows.Forms.DialogResult.OK
                            && saveFileDialog2.FileName.Length > 0)
                        {
                            File.WriteAllBytes(saveFileDialog2.FileName, (myCrypt.Crypt(Cryptography.CryptMethod.DECRYPT, Cryptography.CryptClass.AES, array, key) as byte[]));
                            MetroFramework.MetroMessageBox.Show(this, "Документ расшифрован", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Ключ должен содержать 16 символов", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch
                {
                    MetroFramework.MetroMessageBox.Show(this, "Неверный ключ или алгоритм", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "Выберите файл", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void metroButton6Send_Click(object sender, EventArgs e)
        {
            Match match = Regex.Match(metroTextBox3.Text, mail);
            if (file != "")
            {
                if (metroTextBox3.Text != "")
                {
                    if (match.Success)
                    {
                        try
                        {
                            SmtpClient client = new SmtpClient("smtp.mail.ru", 25);
                            client.Credentials = new NetworkCredential("shark_snake@mail.ru", "998374d");
                            client.EnableSsl = true;
                            string from = "shark_snake@mail.ru";
                            string to = "" + metroTextBox3.Text + "";
                            string subject = "Данные сотрудника";
                            string text = "Документ, Алгоритм: " + metk + ", " + metroTextBox2.Text + "";
                            MailMessage message = new MailMessage(from, to, subject, text);
                            Attachment sendfile = new Attachment(file);
                            message.Attachments.Add(sendfile);
                            client.Send(message);
                            MetroFramework.MetroMessageBox.Show(this, "Документ отправлен", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch
                        {
                            MetroFramework.MetroMessageBox.Show(this, "Нет сети", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Адрес неверный", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "Введите почту получателя", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "Выберете и зашифруйте файл для отправки", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            }
    }
}
