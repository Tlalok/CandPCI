namespace CandPCI_1_UI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.sourceTextBox = new System.Windows.Forms.TextBox();
            this.cipherSelectBox = new System.Windows.Forms.ComboBox();
            this.keyBox = new System.Windows.Forms.NumericUpDown();
            this.cardanGrilleKeyTable = new System.Windows.Forms.DataGridView();
            this.processedTextBox = new System.Windows.Forms.TextBox();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.encryptButton = new System.Windows.Forms.Button();
            this.decryptButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.keyBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardanGrilleKeyTable)).BeginInit();
            this.SuspendLayout();
            // 
            // sourceTextBox
            // 
            this.sourceTextBox.Location = new System.Drawing.Point(12, 12);
            this.sourceTextBox.Multiline = true;
            this.sourceTextBox.Name = "sourceTextBox";
            this.sourceTextBox.Size = new System.Drawing.Size(246, 130);
            this.sourceTextBox.TabIndex = 0;
            // 
            // cipherSelectBox
            // 
            this.cipherSelectBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cipherSelectBox.FormattingEnabled = true;
            this.cipherSelectBox.Items.AddRange(new object[] {
            "Аффинный",
            "Решетка Кардано"});
            this.cipherSelectBox.Location = new System.Drawing.Point(264, 11);
            this.cipherSelectBox.Name = "cipherSelectBox";
            this.cipherSelectBox.Size = new System.Drawing.Size(178, 21);
            this.cipherSelectBox.TabIndex = 2;
            this.cipherSelectBox.SelectedIndexChanged += new System.EventHandler(this.cipherSelectBox_SelectedIndexChanged);
            // 
            // keyBox
            // 
            this.keyBox.Location = new System.Drawing.Point(264, 38);
            this.keyBox.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.keyBox.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.keyBox.Name = "keyBox";
            this.keyBox.Size = new System.Drawing.Size(178, 20);
            this.keyBox.TabIndex = 3;
            this.keyBox.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.keyBox.ValueChanged += new System.EventHandler(this.keyBox_ValueChanged);
            // 
            // cardanGrilleKeyTable
            // 
            this.cardanGrilleKeyTable.AllowUserToAddRows = false;
            this.cardanGrilleKeyTable.AllowUserToDeleteRows = false;
            this.cardanGrilleKeyTable.AllowUserToResizeColumns = false;
            this.cardanGrilleKeyTable.AllowUserToResizeRows = false;
            this.cardanGrilleKeyTable.CausesValidation = false;
            this.cardanGrilleKeyTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.cardanGrilleKeyTable.ColumnHeadersVisible = false;
            this.cardanGrilleKeyTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.cardanGrilleKeyTable.Location = new System.Drawing.Point(264, 64);
            this.cardanGrilleKeyTable.Name = "cardanGrilleKeyTable";
            this.cardanGrilleKeyTable.ReadOnly = true;
            this.cardanGrilleKeyTable.RowHeadersVisible = false;
            this.cardanGrilleKeyTable.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.cardanGrilleKeyTable.ShowCellErrors = false;
            this.cardanGrilleKeyTable.ShowCellToolTips = false;
            this.cardanGrilleKeyTable.ShowEditingIcon = false;
            this.cardanGrilleKeyTable.ShowRowErrors = false;
            this.cardanGrilleKeyTable.Size = new System.Drawing.Size(178, 178);
            this.cardanGrilleKeyTable.TabIndex = 4;
            this.cardanGrilleKeyTable.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.cardanGrilleKeyTable_CellClick);
            // 
            // processedTextBox
            // 
            this.processedTextBox.Location = new System.Drawing.Point(12, 170);
            this.processedTextBox.Multiline = true;
            this.processedTextBox.Name = "processedTextBox";
            this.processedTextBox.ReadOnly = true;
            this.processedTextBox.Size = new System.Drawing.Size(246, 130);
            this.processedTextBox.TabIndex = 5;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Column2";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Column3";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // encryptButton
            // 
            this.encryptButton.Location = new System.Drawing.Point(264, 248);
            this.encryptButton.Name = "encryptButton";
            this.encryptButton.Size = new System.Drawing.Size(178, 23);
            this.encryptButton.TabIndex = 6;
            this.encryptButton.Text = "Зашифровать";
            this.encryptButton.UseVisualStyleBackColor = true;
            this.encryptButton.Click += new System.EventHandler(this.encryptButton_Click);
            // 
            // decryptButton
            // 
            this.decryptButton.Location = new System.Drawing.Point(264, 277);
            this.decryptButton.Name = "decryptButton";
            this.decryptButton.Size = new System.Drawing.Size(178, 23);
            this.decryptButton.TabIndex = 7;
            this.decryptButton.Text = "Расшифровать";
            this.decryptButton.UseVisualStyleBackColor = true;
            this.decryptButton.Click += new System.EventHandler(this.decryptButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 312);
            this.Controls.Add(this.decryptButton);
            this.Controls.Add(this.encryptButton);
            this.Controls.Add(this.processedTextBox);
            this.Controls.Add(this.cardanGrilleKeyTable);
            this.Controls.Add(this.keyBox);
            this.Controls.Add(this.cipherSelectBox);
            this.Controls.Add(this.sourceTextBox);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.keyBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardanGrilleKeyTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox sourceTextBox;
        private System.Windows.Forms.ComboBox cipherSelectBox;
        private System.Windows.Forms.NumericUpDown keyBox;
        private System.Windows.Forms.DataGridView cardanGrilleKeyTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.TextBox processedTextBox;
        private System.Windows.Forms.Button encryptButton;
        private System.Windows.Forms.Button decryptButton;
    }
}

