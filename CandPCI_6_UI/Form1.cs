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

            picture = BitmapHelper.LoadBitmap(openFileDialog.FileName);
            //picture = new Bitmap(openFileDialog.FileName);
            pictureBox.Image = picture;

            var lsb = new LsbMethod(BitmapHelper.BitmapToByteRgbMarshal(picture), 1);
            var readPrefix = lsb.ReadLongInt();
            if (readPrefix != prefix)
                return;
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

            //var mask = 256 - 1 - 2;
            //for (var i = 0; i < bytes.Length; i++)
            //    bytes[i] = (byte)(bytes[i] & mask);
            BitmapHelper.ByteToBitmapRgbMarshal(picture, lsb.Data);
            pictureBox.Image = picture;

            lsb = new LsbMethod(BitmapHelper.BitmapToByteRgbMarshal(picture), 1);
            var readPrefix = lsb.ReadLongInt();
            var readLengthMessage = lsb.ReadInt();
            var readMessage = lsb.ReadMessage(readLengthMessage);
            MessageBox.Show((readPrefix == prefix).ToString());
            AddedMessageBox.Text = readMessage;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.Cancel)
                return;

            //using (var m = new MemoryStream())
            //{
            //    picture.Save(m, ImageFormat.Jpeg);

            //    var img = Image.FromStream(m);

            //    img.Save(saveFileDialog.FileName);
            //}
            //pictureBox.Image.Save(saveFileDialog.FileName, ImageFormat.Bmp);
            var lsb = new LsbMethod(BitmapHelper.BitmapToByteRgbMarshal(picture), 1);
            picture = new Bitmap(pictureBox.Image);
            var readPrefix = lsb.ReadLongInt();
            picture.Save(saveFileDialog.FileName, ImageFormat.Png);
        }
    }
}
