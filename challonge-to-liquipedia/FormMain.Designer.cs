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
            this.checkBoxFinalBracket = new System.Windows.Forms.CheckBox();
            this.radioButtonSingles = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxSeparator = new System.Windows.Forms.TextBox();
            this.radioButtonDoubles = new System.Windows.Forms.RadioButton();
            this.buttonCircuit = new System.Windows.Forms.Button();
            this.numericUpDownCircuit = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWinnersStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWinnersEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWinnersOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLosersOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLosersEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLosersStart)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCircuit)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(161, 31);
            this.textBoxPassword.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(132, 22);
            this.textBoxPassword.TabIndex = 0;
            // 
            // buttonRetrieve
            // 
            this.buttonRetrieve.Location = new System.Drawing.Point(16, 123);
            this.buttonRetrieve.Margin = new System.Windows.Forms.Padding(4);
            this.buttonRetrieve.Name = "buttonRetrieve";
            this.buttonRetrieve.Size = new System.Drawing.Size(100, 28);
            this.buttonRetrieve.TabIndex = 1;
            this.buttonRetrieve.Text = "Retrieve";
            this.buttonRetrieve.UseVisualStyleBackColor = true;
            this.buttonRetrieve.Click += new System.EventHandler(this.buttonRetrieve_Click);
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Location = new System.Drawing.Point(16, 31);
            this.textBoxUsername.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(132, 22);
            this.textBoxUsername.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(157, 11);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "API Key";
            // 
            // textBoxURL
            // 
            this.textBoxURL.Location = new System.Drawing.Point(16, 91);
            this.textBoxURL.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxURL.Name = "textBoxURL";
            this.textBoxURL.Size = new System.Drawing.Size(548, 22);
            this.textBoxURL.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 71);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Challonge URL";
            // 
            // richTextBoxWinnersInput
            // 
            this.richTextBoxWinnersInput.Location = new System.Drawing.Point(16, 192);
            this.richTextBoxWinnersInput.Margin = new System.Windows.Forms.Padding(4);
            this.richTextBoxWinnersInput.Name = "richTextBoxWinnersInput";
            this.richTextBoxWinnersInput.Size = new System.Drawing.Size(453, 152);
            this.richTextBoxWinnersInput.TabIndex = 4;
            this.richTextBoxWinnersInput.Text = "";
            // 
            // richTextBoxLosersInput
            // 
            this.richTextBoxLosersInput.Location = new System.Drawing.Point(480, 192);
            this.richTextBoxLosersInput.Margin = new System.Windows.Forms.Padding(4);
            this.richTextBoxLosersInput.Name = "richTextBoxLosersInput";
            this.richTextBoxLosersInput.Size = new System.Drawing.Size(453, 152);
            this.richTextBoxLosersInput.TabIndex = 3;
            this.richTextBoxLosersInput.Text = "";
            // 
            // buttonFill
            // 
            this.buttonFill.Location = new System.Drawing.Point(465, 123);
            this.buttonFill.Margin = new System.Windows.Forms.Padding(4);
            this.buttonFill.Name = "buttonFill";
            this.buttonFill.Size = new System.Drawing.Size(100, 28);
            this.buttonFill.TabIndex = 5;
            this.buttonFill.Text = "Fill";
            this.buttonFill.UseVisualStyleBackColor = true;
            this.buttonFill.Click += new System.EventHandler(this.buttonFill_Click);
            // 
            // richTextBoxOutput
            // 
            this.richTextBoxOutput.Location = new System.Drawing.Point(16, 377);
            this.richTextBoxOutput.Margin = new System.Windows.Forms.Padding(4);
            this.richTextBoxOutput.Name = "richTextBoxOutput";
            this.richTextBoxOutput.Size = new System.Drawing.Size(917, 185);
            this.richTextBoxOutput.TabIndex = 3;
            this.richTextBoxOutput.Text = "";
            // 
            // numericUpDownWinnersStart
            // 
            this.numericUpDownWinnersStart.Location = new System.Drawing.Point(303, 160);
            this.numericUpDownWinnersStart.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownWinnersStart.Name = "numericUpDownWinnersStart";
            this.numericUpDownWinnersStart.Size = new System.Drawing.Size(51, 22);
            this.numericUpDownWinnersStart.TabIndex = 6;
            // 
            // numericUpDownWinnersEnd
            // 
            this.numericUpDownWinnersEnd.Location = new System.Drawing.Point(361, 160);
            this.numericUpDownWinnersEnd.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownWinnersEnd.Name = "numericUpDownWinnersEnd";
            this.numericUpDownWinnersEnd.Size = new System.Drawing.Size(51, 22);
            this.numericUpDownWinnersEnd.TabIndex = 7;
            // 
            // numericUpDownWinnersOffset
            // 
            this.numericUpDownWinnersOffset.Location = new System.Drawing.Point(420, 160);
            this.numericUpDownWinnersOffset.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownWinnersOffset.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericUpDownWinnersOffset.Name = "numericUpDownWinnersOffset";
            this.numericUpDownWinnersOffset.Size = new System.Drawing.Size(51, 22);
            this.numericUpDownWinnersOffset.TabIndex = 8;
            // 
            // numericUpDownLosersOffset
            // 
            this.numericUpDownLosersOffset.Location = new System.Drawing.Point(884, 160);
            this.numericUpDownLosersOffset.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownLosersOffset.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericUpDownLosersOffset.Name = "numericUpDownLosersOffset";
            this.numericUpDownLosersOffset.Size = new System.Drawing.Size(51, 22);
            this.numericUpDownLosersOffset.TabIndex = 11;
            // 
            // numericUpDownLosersEnd
            // 
            this.numericUpDownLosersEnd.Location = new System.Drawing.Point(825, 160);
            this.numericUpDownLosersEnd.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownLosersEnd.Name = "numericUpDownLosersEnd";
            this.numericUpDownLosersEnd.Size = new System.Drawing.Size(51, 22);
            this.numericUpDownLosersEnd.TabIndex = 10;
            // 
            // numericUpDownLosersStart
            // 
            this.numericUpDownLosersStart.Location = new System.Drawing.Point(767, 160);
            this.numericUpDownLosersStart.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownLosersStart.Name = "numericUpDownLosersStart";
            this.numericUpDownLosersStart.Size = new System.Drawing.Size(51, 22);
            this.numericUpDownLosersStart.TabIndex = 9;
            // 
            // checkBoxFillByes
            // 
            this.checkBoxFillByes.AutoSize = true;
            this.checkBoxFillByes.Checked = true;
            this.checkBoxFillByes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFillByes.Location = new System.Drawing.Point(573, 128);
            this.checkBoxFillByes.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxFillByes.Name = "checkBoxFillByes";
            this.checkBoxFillByes.Size = new System.Drawing.Size(80, 20);
            this.checkBoxFillByes.TabIndex = 12;
            this.checkBoxFillByes.Text = "Fill Byes";
            this.checkBoxFillByes.UseVisualStyleBackColor = true;
            // 
            // checkBoxFillByeWins
            // 
            this.checkBoxFillByeWins.AutoSize = true;
            this.checkBoxFillByeWins.Checked = true;
            this.checkBoxFillByeWins.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFillByeWins.Location = new System.Drawing.Point(667, 128);
            this.checkBoxFillByeWins.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxFillByeWins.Name = "checkBoxFillByeWins";
            this.checkBoxFillByeWins.Size = new System.Drawing.Size(106, 20);
            this.checkBoxFillByeWins.TabIndex = 12;
            this.checkBoxFillByeWins.Text = "Fill Bye Wins";
            this.checkBoxFillByeWins.UseVisualStyleBackColor = true;
            // 
            // checkBoxTrimTags
            // 
            this.checkBoxTrimTags.AutoSize = true;
            this.checkBoxTrimTags.Location = new System.Drawing.Point(124, 128);
            this.checkBoxTrimTags.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxTrimTags.Name = "checkBoxTrimTags";
            this.checkBoxTrimTags.Size = new System.Drawing.Size(91, 20);
            this.checkBoxTrimTags.TabIndex = 13;
            this.checkBoxTrimTags.Text = "Trim Tags";
            this.checkBoxTrimTags.UseVisualStyleBackColor = true;
            // 
            // buttonAKA
            // 
            this.buttonAKA.Location = new System.Drawing.Point(835, 15);
            this.buttonAKA.Margin = new System.Windows.Forms.Padding(4);
            this.buttonAKA.Name = "buttonAKA";
            this.buttonAKA.Size = new System.Drawing.Size(100, 28);
            this.buttonAKA.TabIndex = 5;
            this.buttonAKA.Text = "Get AKA DB";
            this.buttonAKA.UseVisualStyleBackColor = true;
            this.buttonAKA.Click += new System.EventHandler(this.buttonAKA_Click);
            // 
            // labelAkaDatabaseRev
            // 
            this.labelAkaDatabaseRev.AutoSize = true;
            this.labelAkaDatabaseRev.Location = new System.Drawing.Point(831, 47);
            this.labelAkaDatabaseRev.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAkaDatabaseRev.Name = "labelAkaDatabaseRev";
            this.labelAkaDatabaseRev.Size = new System.Drawing.Size(68, 16);
            this.labelAkaDatabaseRev.TabIndex = 14;
            this.labelAkaDatabaseRev.Text = "Rev: none";
            // 
            // radioButtonSmash
            // 
            this.radioButtonSmash.AutoSize = true;
            this.radioButtonSmash.Checked = true;
            this.radioButtonSmash.Location = new System.Drawing.Point(751, 18);
            this.radioButtonSmash.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonSmash.Name = "radioButtonSmash";
            this.radioButtonSmash.Size = new System.Drawing.Size(70, 20);
            this.radioButtonSmash.TabIndex = 15;
            this.radioButtonSmash.TabStop = true;
            this.radioButtonSmash.Text = "Smash";
            this.radioButtonSmash.UseVisualStyleBackColor = true;
            this.radioButtonSmash.CheckedChanged += new System.EventHandler(this.radioButtonDatabase_CheckedChanged);
            // 
            // radioButtonFighters
            // 
            this.radioButtonFighters.AutoSize = true;
            this.radioButtonFighters.Location = new System.Drawing.Point(751, 44);
            this.radioButtonFighters.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonFighters.Name = "radioButtonFighters";
            this.radioButtonFighters.Size = new System.Drawing.Size(76, 20);
            this.radioButtonFighters.TabIndex = 15;
            this.radioButtonFighters.TabStop = true;
            this.radioButtonFighters.Text = "Fighters";
            this.radioButtonFighters.UseVisualStyleBackColor = true;
            this.radioButtonFighters.CheckedChanged += new System.EventHandler(this.radioButtonDatabase_CheckedChanged);
            // 
            // checkBoxFinalBracket
            // 
            this.checkBoxFinalBracket.AutoSize = true;
            this.checkBoxFinalBracket.Checked = true;
            this.checkBoxFinalBracket.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFinalBracket.Location = new System.Drawing.Point(789, 128);
            this.checkBoxFinalBracket.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxFinalBracket.Name = "checkBoxFinalBracket";
            this.checkBoxFinalBracket.Size = new System.Drawing.Size(127, 20);
            this.checkBoxFinalBracket.TabIndex = 16;
            this.checkBoxFinalBracket.Text = "Fill Final Bracket";
            this.checkBoxFinalBracket.UseVisualStyleBackColor = true;
            // 
            // radioButtonSingles
            // 
            this.radioButtonSingles.AutoSize = true;
            this.radioButtonSingles.Checked = true;
            this.radioButtonSingles.Location = new System.Drawing.Point(8, 4);
            this.radioButtonSingles.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonSingles.Name = "radioButtonSingles";
            this.radioButtonSingles.Size = new System.Drawing.Size(73, 20);
            this.radioButtonSingles.TabIndex = 17;
            this.radioButtonSingles.TabStop = true;
            this.radioButtonSingles.Text = "Singles";
            this.radioButtonSingles.UseVisualStyleBackColor = true;
            this.radioButtonSingles.CheckedChanged += new System.EventHandler(this.radioButtonSinglesDoubles_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.textBoxSeparator);
            this.panel1.Controls.Add(this.radioButtonDoubles);
            this.panel1.Controls.Add(this.radioButtonSingles);
            this.panel1.Location = new System.Drawing.Point(565, 74);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(368, 49);
            this.panel1.TabIndex = 18;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(97, 6);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 16);
            this.label4.TabIndex = 20;
            this.label4.Text = "Separator";
            // 
            // textBoxSeparator
            // 
            this.textBoxSeparator.Location = new System.Drawing.Point(101, 22);
            this.textBoxSeparator.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxSeparator.Name = "textBoxSeparator";
            this.textBoxSeparator.Size = new System.Drawing.Size(132, 22);
            this.textBoxSeparator.TabIndex = 19;
            // 
            // radioButtonDoubles
            // 
            this.radioButtonDoubles.AutoSize = true;
            this.radioButtonDoubles.Location = new System.Drawing.Point(8, 25);
            this.radioButtonDoubles.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonDoubles.Name = "radioButtonDoubles";
            this.radioButtonDoubles.Size = new System.Drawing.Size(79, 20);
            this.radioButtonDoubles.TabIndex = 18;
            this.radioButtonDoubles.Text = "Doubles";
            this.radioButtonDoubles.UseVisualStyleBackColor = true;
            this.radioButtonDoubles.CheckedChanged += new System.EventHandler(this.radioButtonSinglesDoubles_CheckedChanged);
            // 
            // buttonCircuit
            // 
            this.buttonCircuit.Location = new System.Drawing.Point(357, 124);
            this.buttonCircuit.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCircuit.Name = "buttonCircuit";
            this.buttonCircuit.Size = new System.Drawing.Size(100, 28);
            this.buttonCircuit.TabIndex = 19;
            this.buttonCircuit.Text = "Circuit";
            this.buttonCircuit.UseVisualStyleBackColor = true;
            this.buttonCircuit.Click += new System.EventHandler(this.buttonCircuit_Click);
            // 
            // numericUpDownCircuit
            // 
            this.numericUpDownCircuit.Location = new System.Drawing.Point(303, 128);
            this.numericUpDownCircuit.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownCircuit.Name = "numericUpDownCircuit";
            this.numericUpDownCircuit.Size = new System.Drawing.Size(51, 22);
            this.numericUpDownCircuit.TabIndex = 20;
            this.numericUpDownCircuit.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(951, 577);
            this.Controls.Add(this.numericUpDownCircuit);
            this.Controls.Add(this.buttonCircuit);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.checkBoxFinalBracket);
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
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormMain";
            this.Text = "Challonge to Liquipedia v2.0.5";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWinnersStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWinnersEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWinnersOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLosersOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLosersEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLosersStart)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCircuit)).EndInit();
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
        private System.Windows.Forms.CheckBox checkBoxFinalBracket;
        private System.Windows.Forms.RadioButton radioButtonSingles;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxSeparator;
        private System.Windows.Forms.RadioButton radioButtonDoubles;
        private System.Windows.Forms.Button buttonCircuit;
        private System.Windows.Forms.NumericUpDown numericUpDownCircuit;
    }
}

