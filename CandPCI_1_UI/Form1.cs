using CandPCI_1.Algorithms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CandPCI_1_UI
{
    public partial class Form1 : Form
    {
        private List<Point> currentKeyGrille;

        public Form1()
        {
            InitializeComponent();
            cipherSelectBox.SelectedIndex = 0;
        }

        private void cipherSelectBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cipherSelectBox.SelectedIndex == 0)
            {
                cardanGrilleKeyTable.Visible = false;
            }
            else
            {
                cardanGrilleKeyTable.Visible = true;
                ResizeTable();
            }

        }

        private void ResizeTable()
        {
            var tableOrder = (int)keyBox.Value;
            currentKeyGrille = new List<Point>();
            cardanGrilleKeyTable.RowCount = tableOrder;
            cardanGrilleKeyTable.ColumnCount = tableOrder;
            cardanGrilleKeyTable.DefaultCellStyle.Font = new Font("Elite", 15);
            cardanGrilleKeyTable.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            for (var i = 0; i < tableOrder; i++)
            {
                //var cellSize = 10;
                var cellSize = cardanGrilleKeyTable.Size.Height / tableOrder;
                cardanGrilleKeyTable.Columns[i].Width = cellSize;
                cardanGrilleKeyTable.Rows[i].Height = cellSize;
                for (var j = 0; j < tableOrder; j++)
                {
                    cardanGrilleKeyTable[i, j].Value = " ";
                }
            }
        }

        private void keyBox_ValueChanged(object sender, EventArgs e)
        {
            if (cipherSelectBox.SelectedIndex == 0)
                return;

            ResizeTable();
        }

        private void cardanGrilleKeyTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var tableOrder = (int)keyBox.Value;
            var orderOdd = tableOrder % 2 == 1;
            var halfOrder = tableOrder / 2;
            if (orderOdd && (e.ColumnIndex == halfOrder) && (e.RowIndex == halfOrder))
                return;

            var newPoint = new Point(e.RowIndex, e.ColumnIndex);
            if (currentKeyGrille.Contains(newPoint))
            {
                currentKeyGrille.Remove(newPoint);
                cardanGrilleKeyTable[e.ColumnIndex, e.RowIndex].Value = " ";
                for (int i = 0; i < currentKeyGrille.Count; i++)
                    cardanGrilleKeyTable[currentKeyGrille[i].Y, currentKeyGrille[i].X].Value = (i + 1).ToString();
            }
            else
            {
                currentKeyGrille.Add(newPoint);
                cardanGrilleKeyTable[e.ColumnIndex, e.RowIndex].Value = currentKeyGrille.Count.ToString();
            }
        }

        private void encryptButton_Click(object sender, EventArgs e)
        {
            if (cipherSelectBox.SelectedIndex == 0)
            {
                var key = (int)keyBox.Value;
                var message = sourceTextBox.Text;
                var encryptedMessage = new AffineCipher().Encrypt(message, key);
                processedTextBox.Text = encryptedMessage;
            }
            else
            {
                var key = new CardanGrilleKey()
                {
                    MatrixOrder = (int)keyBox.Value,
                    Positions = currentKeyGrille.Select(p => new Position(p.X, p.Y)).ToArray()
                };
                var message = sourceTextBox.Text;
                var encryptedMessage = new CardanGrilleCipher().Encrypt(message, key);
                processedTextBox.Text = encryptedMessage;
            }
        }

        private void decryptButton_Click(object sender, EventArgs e)
        {
            if (cipherSelectBox.SelectedIndex == 0)
            {
                var key = (int)keyBox.Value;
                var message = sourceTextBox.Text;
                var encryptedMessage = new AffineCipher().Decrypt(message, key);
                processedTextBox.Text = encryptedMessage;
            }
            else
            {
                var key = new CardanGrilleKey()
                {
                    MatrixOrder = (int)keyBox.Value,
                    Positions = currentKeyGrille.Select(p => new Position(p.X, p.Y)).ToArray()
                };
                var message = sourceTextBox.Text;
                var encryptedMessage = new CardanGrilleCipher().Decrypt(message, key);
                processedTextBox.Text = encryptedMessage;
            }
        }
    }
}
