namespace NetRadio
{
    partial class FrmStationInfo
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
            components = new System.ComponentModel.Container();
            panelBackground = new System.Windows.Forms.Panel();
            tbxURL = new System.Windows.Forms.TextBox();
            linkLblHomepage = new System.Windows.Forms.LinkLabel();
            lblName = new System.Windows.Forms.Label();
            lblBitrate = new System.Windows.Forms.Label();
            lblCodec = new System.Windows.Forms.Label();
            lblVotes = new System.Windows.Forms.Label();
            lblLanguage = new System.Windows.Forms.Label();
            lblCountry = new System.Windows.Forms.Label();
            pictureBox = new System.Windows.Forms.PictureBox();
            defBitrate = new System.Windows.Forms.Label();
            defCodec = new System.Windows.Forms.Label();
            defVotes = new System.Windows.Forms.Label();
            defLanguage = new System.Windows.Forms.Label();
            defCountry = new System.Windows.Forms.Label();
            btnClose = new System.Windows.Forms.Button();
            btnNext = new System.Windows.Forms.Button();
            btnPrevious = new System.Windows.Forms.Button();
            toolTip = new System.Windows.Forms.ToolTip(components);
            panelBackground.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            SuspendLayout();
            // 
            // panelBackground
            // 
            panelBackground.BackColor = System.Drawing.SystemColors.ControlLightLight;
            panelBackground.Controls.Add(tbxURL);
            panelBackground.Controls.Add(linkLblHomepage);
            panelBackground.Controls.Add(lblName);
            panelBackground.Controls.Add(lblBitrate);
            panelBackground.Controls.Add(lblCodec);
            panelBackground.Controls.Add(lblVotes);
            panelBackground.Controls.Add(lblLanguage);
            panelBackground.Controls.Add(lblCountry);
            panelBackground.Controls.Add(pictureBox);
            panelBackground.Controls.Add(defBitrate);
            panelBackground.Controls.Add(defCodec);
            panelBackground.Controls.Add(defVotes);
            panelBackground.Controls.Add(defLanguage);
            panelBackground.Controls.Add(defCountry);
            panelBackground.Dock = System.Windows.Forms.DockStyle.Top;
            panelBackground.Location = new System.Drawing.Point(0, 0);
            panelBackground.Name = "panelBackground";
            panelBackground.Size = new System.Drawing.Size(363, 225);
            panelBackground.TabIndex = 1;
            // 
            // tbxURL
            // 
            tbxURL.BackColor = System.Drawing.SystemColors.ControlLightLight;
            tbxURL.Location = new System.Drawing.Point(15, 41);
            tbxURL.Name = "tbxURL";
            tbxURL.ReadOnly = true;
            tbxURL.ShortcutsEnabled = false;
            tbxURL.Size = new System.Drawing.Size(341, 24);
            tbxURL.TabIndex = 13;
            tbxURL.WordWrap = false;
            // 
            // linkLblHomepage
            // 
            linkLblHomepage.Location = new System.Drawing.Point(12, 195);
            linkLblHomepage.Name = "linkLblHomepage";
            linkLblHomepage.Size = new System.Drawing.Size(344, 19);
            linkLblHomepage.TabIndex = 12;
            linkLblHomepage.TabStop = true;
            linkLblHomepage.Text = "Homepage";
            linkLblHomepage.MouseClick += LinkLblHomepage_MouseClick;
            // 
            // lblName
            // 
            lblName.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            lblName.Location = new System.Drawing.Point(12, 17);
            lblName.Name = "lblName";
            lblName.Size = new System.Drawing.Size(339, 15);
            lblName.TabIndex = 11;
            lblName.Text = "Name";
            // 
            // lblBitrate
            // 
            lblBitrate.Location = new System.Drawing.Point(100, 170);
            lblBitrate.Name = "lblBitrate";
            lblBitrate.Size = new System.Drawing.Size(120, 15);
            lblBitrate.TabIndex = 10;
            lblBitrate.Text = "label1";
            // 
            // lblCodec
            // 
            lblCodec.Location = new System.Drawing.Point(100, 145);
            lblCodec.Name = "lblCodec";
            lblCodec.Size = new System.Drawing.Size(120, 15);
            lblCodec.TabIndex = 9;
            lblCodec.Text = "label1";
            // 
            // lblVotes
            // 
            lblVotes.Location = new System.Drawing.Point(100, 120);
            lblVotes.Name = "lblVotes";
            lblVotes.Size = new System.Drawing.Size(120, 15);
            lblVotes.TabIndex = 8;
            lblVotes.Text = "label1";
            // 
            // lblLanguage
            // 
            lblLanguage.Location = new System.Drawing.Point(100, 95);
            lblLanguage.Name = "lblLanguage";
            lblLanguage.Size = new System.Drawing.Size(120, 15);
            lblLanguage.TabIndex = 7;
            lblLanguage.Text = "label1";
            // 
            // lblCountry
            // 
            lblCountry.Location = new System.Drawing.Point(100, 70);
            lblCountry.Name = "lblCountry";
            lblCountry.Size = new System.Drawing.Size(120, 15);
            lblCountry.TabIndex = 6;
            lblCountry.Text = "label1";
            // 
            // pictureBox
            // 
            pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pictureBox.InitialImage = Properties.Resources.epul;
            pictureBox.Location = new System.Drawing.Point(231, 67);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new System.Drawing.Size(125, 125);
            pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            pictureBox.TabIndex = 5;
            pictureBox.TabStop = false;
            // 
            // defBitrate
            // 
            defBitrate.AutoSize = true;
            defBitrate.Location = new System.Drawing.Point(12, 170);
            defBitrate.Name = "defBitrate";
            defBitrate.Size = new System.Drawing.Size(48, 17);
            defBitrate.TabIndex = 4;
            defBitrate.Text = "Bitrate:";
            // 
            // defCodec
            // 
            defCodec.AutoSize = true;
            defCodec.Location = new System.Drawing.Point(12, 145);
            defCodec.Name = "defCodec";
            defCodec.Size = new System.Drawing.Size(48, 17);
            defCodec.TabIndex = 3;
            defCodec.Text = "Codec:";
            // 
            // defVotes
            // 
            defVotes.AutoSize = true;
            defVotes.Location = new System.Drawing.Point(12, 120);
            defVotes.Name = "defVotes";
            defVotes.Size = new System.Drawing.Size(43, 17);
            defVotes.TabIndex = 2;
            defVotes.Text = "Votes:";
            // 
            // defLanguage
            // 
            defLanguage.AutoSize = true;
            defLanguage.Location = new System.Drawing.Point(12, 95);
            defLanguage.Name = "defLanguage";
            defLanguage.Size = new System.Drawing.Size(68, 17);
            defLanguage.TabIndex = 1;
            defLanguage.Text = "Language:";
            // 
            // defCountry
            // 
            defCountry.AutoSize = true;
            defCountry.Location = new System.Drawing.Point(12, 70);
            defCountry.Name = "defCountry";
            defCountry.Size = new System.Drawing.Size(56, 17);
            defCountry.TabIndex = 0;
            defCountry.Text = "Country:";
            // 
            // btnClose
            // 
            btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnClose.Location = new System.Drawing.Point(242, 231);
            btnClose.Name = "btnClose";
            btnClose.Size = new System.Drawing.Size(114, 25);
            btnClose.TabIndex = 2;
            btnClose.Text = "Close (Esc/Enter)";
            btnClose.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            btnNext.DialogResult = System.Windows.Forms.DialogResult.Yes;
            btnNext.Location = new System.Drawing.Point(135, 231);
            btnNext.Name = "btnNext";
            btnNext.Size = new System.Drawing.Size(101, 25);
            btnNext.TabIndex = 4;
            btnNext.Text = "&Next (PgDn)";
            toolTip.SetToolTip(btnNext, "F4");
            btnNext.UseVisualStyleBackColor = true;
            // 
            // btnPrevious
            // 
            btnPrevious.DialogResult = System.Windows.Forms.DialogResult.No;
            btnPrevious.Location = new System.Drawing.Point(5, 231);
            btnPrevious.Name = "btnPrevious";
            btnPrevious.Size = new System.Drawing.Size(124, 25);
            btnPrevious.TabIndex = 5;
            btnPrevious.Text = "&Previous (PgUp)";
            toolTip.SetToolTip(btnPrevious, "F3");
            btnPrevious.UseVisualStyleBackColor = true;
            // 
            // FrmStationInfo
            // 
            AcceptButton = btnClose;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = btnClose;
            ClientSize = new System.Drawing.Size(363, 266);
            Controls.Add(btnPrevious);
            Controls.Add(btnNext);
            Controls.Add(btnClose);
            Controls.Add(panelBackground);
            Font = new System.Drawing.Font("Segoe UI", 9.5F);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmStationInfo";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "frmStationInfo";
            TopMost = true;
            panelBackground.ResumeLayout(false);
            panelBackground.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelBackground;
        private System.Windows.Forms.Label defLanguage;
        private System.Windows.Forms.Label defCountry;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblBitrate;
        private System.Windows.Forms.Label lblCodec;
        private System.Windows.Forms.Label lblVotes;
        private System.Windows.Forms.Label lblLanguage;
        private System.Windows.Forms.Label lblCountry;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label defBitrate;
        private System.Windows.Forms.Label defCodec;
        private System.Windows.Forms.Label defVotes;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.LinkLabel linkLblHomepage;
        private System.Windows.Forms.TextBox tbxURL;
        private System.Windows.Forms.ToolTip toolTip;
    }
}