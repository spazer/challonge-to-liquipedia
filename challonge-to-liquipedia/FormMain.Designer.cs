namespace challonge_to_liquipedia
{
    partial class FormMain
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
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.buttonRetrieve = new System.Windows.Forms.Button();
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxURL = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.richTextBoxWinnersInput = new System.Windows.Forms.RichTextBox();
            this.richTextBoxLosersInput = new System.Windows.Forms.RichTextBox();
            this.buttonFill = new System.Windows.Forms.Button();
            this.richTextBoxOutput = new System.Windows.Forms.RichTextBox();
            this.numericUpDownWinnersStart = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownWinnersEnd = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownWinnersOffset = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownLosersOffset = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownLosersEnd = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownLosersStart = new System.Windows.Forms.NumericUpDown();
            this.checkBoxFillByes = new System.Windows.Forms.CheckBox();
            this.checkBoxFillByeWins = new System.Windows.Forms.CheckBox();
            this.checkBoxTrimTags = new System.Windows.Forms.CheckBox();
            this.buttonAKA = new System.Windows.Forms.Button();
            this.labelAkaDatabaseRev = new System.Windows.Forms.Label();
            this.radioButtonSmash = new System.Windows.Forms.RadioButton();
            this.radioButtonFighters = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWinnersStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWinnersEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWinnersOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLosersOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLosersEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLosersStart)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(121, 25);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(100, 20);
            this.textBoxPassword.TabIndex = 0;
            // 
            // buttonRetrieve
            // 
            this.buttonRetrieve.Location = new System.Drawing.Point(12, 100);
            this.buttonRetrieve.Name = "buttonRetrieve";
            this.buttonRetrieve.Size = new System.Drawing.Size(75, 23);
            this.buttonRetrieve.TabIndex = 1;
            this.buttonRetrieve.Text = "Retrieve";
            this.buttonRetrieve.UseVisualStyleBackColor = true;
            this.buttonRetrieve.Click += new System.EventHandler(this.buttonRetrieve_Click);
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Location = new System.Drawing.Point(12, 25);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(100, 20);
            this.textBoxUsername.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(118, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "API Key";
            // 
            // textBoxURL
            // 
            this.textBoxURL.Location = new System.Drawing.Point(12, 74);
            this.textBoxURL.Name = "textBoxURL";
            this.textBoxURL.Size = new System.Drawing.Size(412, 20);
            this.textBoxURL.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Challonge URL";
            // 
            // richTextBoxWinnersInput
            // 
            this.richTextBoxWinnersInput.Location = new System.Drawing.Point(12, 156);
            this.richTextBoxWinnersInput.Name = "richTextBoxWinnersInput";
            this.richTextBoxWinnersInput.Size = new System.Drawing.Size(341, 124);
            this.richTextBoxWinnersInput.TabIndex = 4;
            this.richTextBoxWinnersInput.Text = "";
            // 
            // richTextBoxLosersInput
            // 
            this.richTextBoxLosersInput.Location = new System.Drawing.Point(360, 156);
            this.richTextBoxLosersInput.Name = "richTextBoxLosersInput";
            this.richTextBoxLosersInput.Size = new System.Drawing.Size(341, 124);
            this.richTextBoxLosersInput.TabIndex = 3;
            this.richTextBoxLosersInput.Text = "";
            // 
            // buttonFill
            // 
            this.buttonFill.Location = new System.Drawing.Point(349, 100);
            this.buttonFill.Name = "buttonFill";
            this.buttonFill.Size = new System.Drawing.Size(75, 23);
            this.buttonFill.TabIndex = 5;
            this.buttonFill.Text = "Fill";
            this.buttonFill.UseVisualStyleBackColor = true;
            this.buttonFill.Click += new System.EventHandler(this.buttonFill_Click);
            // 
            // richTextBoxOutput
            // 
            this.richTextBoxOutput.Location = new System.Drawing.Point(12, 306);
            this.richTextBoxOutput.Name = "richTextBoxOutput";
            this.richTextBoxOutput.Size = new System.Drawing.Size(689, 151);
            this.richTextBoxOutput.TabIndex = 3;
            this.richTextBoxOutput.Text = "";
            // 
            // numericUpDownWinnersStart
            // 
            this.numericUpDownWinnersStart.Location = new System.Drawing.Point(227, 130);
            this.numericUpDownWinnersStart.Name = "numericUpDownWinnersStart";
            this.numericUpDownWinnersStart.Size = new System.Drawing.Size(38, 20);
            this.numericUpDownWinnersStart.TabIndex = 6;
            // 
            // numericUpDownWinnersEnd
            // 
            this.numericUpDownWinnersEnd.Location = new System.Drawing.Point(271, 130);
            this.numericUpDownWinnersEnd.Name = "numericUpDownWinnersEnd";
            this.numericUpDownWinnersEnd.Size = new System.Drawing.Size(38, 20);
            this.numericUpDownWinnersEnd.TabIndex = 7;
            // 
            // numericUpDownWinnersOffset
            // 
            this.numericUpDownWinnersOffset.Location = new System.Drawing.Point(315, 130);
            this.numericUpDownWinnersOffset.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericUpDownWinnersOffset.Name = "numericUpDownWinnersOffset";
            this.numericUpDownWinnersOffset.Size = new System.Drawing.Size(38, 20);
            this.numericUpDownWinnersOffset.TabIndex = 8;
            // 
            // numericUpDownLosersOffset
            // 
            this.numericUpDownLosersOffset.Location = new System.Drawing.Point(663, 130);
            this.numericUpDownLosersOffset.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericUpDownLosersOffset.Name = "numericUpDownLosersOffset";
            this.numericUpDownLosersOffset.Size = new System.Drawing.Size(38, 20);
            this.numericUpDownLosersOffset.TabIndex = 11;
            // 
            // numericUpDownLosersEnd
            // 
            this.numericUpDownLosersEnd.Location = new System.Drawing.Point(619, 130);
            this.numericUpDownLosersEnd.Name = "numericUpDownLosersEnd";
            this.numericUpDownLosersEnd.Size = new System.Drawing.Size(38, 20);
            this.numericUpDownLosersEnd.TabIndex = 10;
            // 
            // numericUpDownLosersStart
            // 
            this.numericUpDownLosersStart.Location = new System.Drawing.Point(575, 130);
            this.numericUpDownLosersStart.Name = "numericUpDownLosersStart";
            this.numericUpDownLosersStart.Size = new System.Drawing.Size(38, 20);
            this.numericUpDownLosersStart.TabIndex = 9;
            // 
            // checkBoxFillByes
            // 
            this.checkBoxFillByes.AutoSize = true;
            this.checkBoxFillByes.Checked = true;
            this.checkBoxFillByes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFillByes.Location = new System.Drawing.Point(430, 104);
            this.checkBoxFillByes.Name = "checkBoxFillByes";
            this.checkBoxFillByes.Size = new System.Drawing.Size(64, 17);
            this.checkBoxFillByes.TabIndex = 12;
            this.checkBoxFillByes.Text = "Fill Byes";
            this.checkBoxFillByes.UseVisualStyleBackColor = true;
            // 
            // checkBoxFillByeWins
            // 
            this.checkBoxFillByeWins.AutoSize = true;
            this.checkBoxFillByeWins.Checked = true;
            this.checkBoxFillByeWins.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFillByeWins.Location = new System.Drawing.Point(500, 104);
            this.checkBoxFillByeWins.Name = "checkBoxFillByeWins";
            this.checkBoxFillByeWins.Size = new System.Drawing.Size(86, 17);
            this.checkBoxFillByeWins.TabIndex = 12;
            this.checkBoxFillByeWins.Text = "Fill Bye Wins";
            this.checkBoxFillByeWins.UseVisualStyleBackColor = true;
            // 
            // checkBoxTrimTags
            // 
            this.checkBoxTrimTags.AutoSize = true;
            this.checkBoxTrimTags.Location = new System.Drawing.Point(93, 104);
            this.checkBoxTrimTags.Name = "checkBoxTrimTags";
            this.checkBoxTrimTags.Size = new System.Drawing.Size(73, 17);
            this.checkBoxTrimTags.TabIndex = 13;
            this.checkBoxTrimTags.Text = "Trim Tags";
            this.checkBoxTrimTags.UseVisualStyleBackColor = true;
            // 
            // buttonAKA
            // 
            this.buttonAKA.Location = new System.Drawing.Point(626, 12);
            this.buttonAKA.Name = "buttonAKA";
            this.buttonAKA.Size = new System.Drawing.Size(75, 23);
            this.buttonAKA.TabIndex = 5;
            this.buttonAKA.Text = "Get AKA DB";
            this.buttonAKA.UseVisualStyleBackColor = true;
            this.buttonAKA.Click += new System.EventHandler(this.buttonAKA_Click);
            // 
            // labelAkaDatabaseRev
            // 
            this.labelAkaDatabaseRev.AutoSize = true;
            this.labelAkaDatabaseRev.Location = new System.Drawing.Point(623, 38);
            this.labelAkaDatabaseRev.Name = "labelAkaDatabaseRev";
            this.labelAkaDatabaseRev.Size = new System.Drawing.Size(57, 13);
            this.labelAkaDatabaseRev.TabIndex = 14;
            this.labelAkaDatabaseRev.Text = "Rev: none";
            // 
            // radioButtonSmash
            // 
            this.radioButtonSmash.AutoSize = true;
            this.radioButtonSmash.Checked = true;
            this.radioButtonSmash.Location = new System.Drawing.Point(563, 15);
            this.radioButtonSmash.Name = "radioButtonSmash";
            this.radioButtonSmash.Size = new System.Drawing.Size(57, 17);
            this.radioButtonSmash.TabIndex = 15;
            this.radioButtonSmash.TabStop = true;
            this.radioButtonSmash.Text = "Smash";
            this.radioButtonSmash.UseVisualStyleBackColor = true;
            this.radioButtonSmash.CheckedChanged += new System.EventHandler(this.radioButtonDatabase_CheckedChanged);
            // 
            // radioButtonFighters
            // 
            this.radioButtonFighters.AutoSize = true;
            this.radioButtonFighters.Location = new System.Drawing.Point(563, 36);
            this.radioButtonFighters.Name = "radioButtonFighters";
            this.radioButtonFighters.Size = new System.Drawing.Size(62, 17);
            this.radioButtonFighters.TabIndex = 15;
            this.radioButtonFighters.TabStop = true;
            this.radioButtonFighters.Text = "Fighters";
            this.radioButtonFighters.UseVisualStyleBackColor = true;
            this.radioButtonFighters.CheckedChanged += new System.EventHandler(this.radioButtonDatabase_CheckedChanged);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 469);
            this.Controls.Add(this.radioButtonFighters);
            this.Controls.Add(this.radioButtonSmash);
            this.Controls.Add(this.labelAkaDatabaseRev);
            this.Controls.Add(this.checkBoxTrimTags);
            this.Controls.Add(this.checkBoxFillByeWins);
            this.Controls.Add(this.checkBoxFillByes);
            this.Controls.Add(this.numericUpDownLosersOffset);
            this.Controls.Add(this.numericUpDownLosersEnd);
            this.Controls.Add(this.numericUpDownLosersStart);
            this.Controls.Add(this.numericUpDownWinnersOffset);
            this.Controls.Add(this.numericUpDownWinnersEnd);
            this.Controls.Add(this.numericUpDownWinnersStart);
            this.Controls.Add(this.buttonAKA);
            this.Controls.Add(this.buttonFill);
            this.Controls.Add(this.richTextBoxWinnersInput);
            this.Controls.Add(this.richTextBoxLosersInput);
            this.Controls.Add(this.richTextBoxOutput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonRetrieve);
            this.Controls.Add(this.textBoxURL);
            this.Controls.Add(this.textBoxUsername);
            this.Controls.Add(this.textBoxPassword);
            this.Name = "FormMain";
            this.Text = "Challonge to Liquipedia v1.1.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWinnersStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWinnersEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWinnersOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLosersOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLosersEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLosersStart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Button buttonRetrieve;
        private System.Windows.Forms.TextBox textBoxUsername;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxURL;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox richTextBoxWinnersInput;
        private System.Windows.Forms.RichTextBox richTextBoxLosersInput;
        private System.Windows.Forms.Button buttonFill;
        private System.Windows.Forms.RichTextBox richTextBoxOutput;
        private System.Windows.Forms.NumericUpDown numericUpDownWinnersStart;
        private System.Windows.Forms.NumericUpDown numericUpDownWinnersEnd;
        private System.Windows.Forms.NumericUpDown numericUpDownWinnersOffset;
        private System.Windows.Forms.NumericUpDown numericUpDownLosersOffset;
        private System.Windows.Forms.NumericUpDown numericUpDownLosersEnd;
        private System.Windows.Forms.NumericUpDown numericUpDownLosersStart;
        private System.Windows.Forms.CheckBox checkBoxFillByes;
        private System.Windows.Forms.CheckBox checkBoxFillByeWins;
        private System.Windows.Forms.CheckBox checkBoxTrimTags;
        private System.Windows.Forms.Button buttonAKA;
        private System.Windows.Forms.Label labelAkaDatabaseRev;
        private System.Windows.Forms.RadioButton radioButtonSmash;
        private System.Windows.Forms.RadioButton radioButtonFighters;
    }
}

