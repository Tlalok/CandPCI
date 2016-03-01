using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CandPCI_6;
using System.IO;

namespace CandPCI_6_UI
{
    public partial class Form1 : Form
    {
        private Bitmap picture;

        private long prefix = 0x4BD94CD8BC3AA920;
        public Form1()
        {
            InitializeComponent();
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                return;

            AddedMessageBox.Text = String.Empty;

            picture = BitmapHelper.LoadBitmap(openFileDialog.FileName);
            pictureBox.Image = picture;

            var lsb = new LsbMethod(BitmapHelper.BitmapToByteRgbMarshal(picture), 1);
            var readPrefix = lsb.ReadLongInt();
            if (readPrefix != prefix)
            {
                MessageBox.Show("Added message not found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var readLengthMessage = lsb.ReadInt();
            var readMessage = lsb.ReadMessage(readLengthMessage);
            AddedMessageBox.Text = readMessage;
        }

        private void addMessageButton_Click(object sender, EventArgs e)
        {
            if (AddingMessageBox.Text.Length == 0)
                return;
            var bytes = BitmapHelper.BitmapToByteRgbMarshal(picture);

            var lsb = new LsbMethod(bytes, 1);
            lsb.WriteLongInt(prefix);
            lsb.WriteInt(AddingMessageBox.Text.Length * 2);
            lsb.WriteString(AddingMessageBox.Text);

            BitmapHelper.ByteToBitmapRgbMarshal(picture, lsb.Data);
            pictureBox.Image = picture;

            lsb = new LsbMethod(BitmapHelper.BitmapToByteRgbMarshal(picture), 1);
            var readPrefix = lsb.ReadLongInt();
            if (readPrefix != prefix)
            {
                MessageBox.Show("Error occured while adding message.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            var readLengthMessage = lsb.ReadInt();
            var readMessage = lsb.ReadMessage(readLengthMessage);
            AddedMessageBox.Text = readMessage;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.Cancel)
                return;
            picture.Save(saveFileDialog.FileName, ImageFormat.Png);
        }
    }
}
