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
            this.panelBackground = new System.Windows.Forms.Panel();
            this.tbxURL = new System.Windows.Forms.TextBox();
            this.linkLblHomepage = new System.Windows.Forms.LinkLabel();
            this.lblName = new System.Windows.Forms.Label();
            this.lblBitrate = new System.Windows.Forms.Label();
            this.lblCodec = new System.Windows.Forms.Label();
            this.lblVotes = new System.Windows.Forms.Label();
            this.lblLanguage = new System.Windows.Forms.Label();
            this.lblCountry = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.defBitrate = new System.Windows.Forms.Label();
            this.defCodec = new System.Windows.Forms.Label();
            this.defVotes = new System.Windows.Forms.Label();
            this.defLanguage = new System.Windows.Forms.Label();
            this.defCountry = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.panelBackground.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // panelBackground
            // 
            this.panelBackground.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panelBackground.Controls.Add(this.tbxURL);
            this.panelBackground.Controls.Add(this.linkLblHomepage);
            this.panelBackground.Controls.Add(this.lblName);
            this.panelBackground.Controls.Add(this.lblBitrate);
            this.panelBackground.Controls.Add(this.lblCodec);
            this.panelBackground.Controls.Add(this.lblVotes);
            this.panelBackground.Controls.Add(this.lblLanguage);
            this.panelBackground.Controls.Add(this.lblCountry);
            this.panelBackground.Controls.Add(this.pictureBox);
            this.panelBackground.Controls.Add(this.defBitrate);
            this.panelBackground.Controls.Add(this.defCodec);
            this.panelBackground.Controls.Add(this.defVotes);
            this.panelBackground.Controls.Add(this.defLanguage);
            this.panelBackground.Controls.Add(this.defCountry);
            this.panelBackground.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelBackground.Location = new System.Drawing.Point(0, 0);
            this.panelBackground.Name = "panelBackground";
            this.panelBackground.Size = new System.Drawing.Size(363, 225);
            this.panelBackground.TabIndex = 1;
            // 
            // tbxURL
            // 
            this.tbxURL.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tbxURL.Location = new System.Drawing.Point(15, 41);
            this.tbxURL.Name = "tbxURL";
            this.tbxURL.ReadOnly = true;
            this.tbxURL.ShortcutsEnabled = false;
            this.tbxURL.Size = new System.Drawing.Size(341, 24);
            this.tbxURL.TabIndex = 13;
            this.tbxURL.WordWrap = false;
            // 
            // linkLblHomepage
            // 
            this.linkLblHomepage.Location = new System.Drawing.Point(12, 195);
            this.linkLblHomepage.Name = "linkLblHomepage";
            this.linkLblHomepage.Size = new System.Drawing.Size(344, 19);
            this.linkLblHomepage.TabIndex = 12;
            this.linkLblHomepage.TabStop = true;
            this.linkLblHomepage.Text = "Homepage";
            this.linkLblHomepage.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LinkLblHomepage_MouseClick);
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblName.Location = new System.Drawing.Point(12, 17);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(339, 15);
            this.lblName.TabIndex = 11;
            this.lblName.Text = "Name";
            // 
            // lblBitrate
            // 
            this.lblBitrate.Location = new System.Drawing.Point(100, 170);
            this.lblBitrate.Name = "lblBitrate";
            this.lblBitrate.Size = new System.Drawing.Size(120, 15);
            this.lblBitrate.TabIndex = 10;
            this.lblBitrate.Text = "label1";
            // 
            // lblCodec
            // 
            this.lblCodec.Location = new System.Drawing.Point(100, 145);
            this.lblCodec.Name = "lblCodec";
            this.lblCodec.Size = new System.Drawing.Size(120, 15);
            this.lblCodec.TabIndex = 9;
            this.lblCodec.Text = "label1";
            // 
            // lblVotes
            // 
            this.lblVotes.Location = new System.Drawing.Point(100, 120);
            this.lblVotes.Name = "lblVotes";
            this.lblVotes.Size = new System.Drawing.Size(120, 15);
            this.lblVotes.TabIndex = 8;
            this.lblVotes.Text = "label1";
            // 
            // lblLanguage
            // 
            this.lblLanguage.Location = new System.Drawing.Point(100, 95);
            this.lblLanguage.Name = "lblLanguage";
            this.lblLanguage.Size = new System.Drawing.Size(120, 15);
            this.lblLanguage.TabIndex = 7;
            this.lblLanguage.Text = "label1";
            // 
            // lblCountry
            // 
            this.lblCountry.Location = new System.Drawing.Point(100, 70);
            this.lblCountry.Name = "lblCountry";
            this.lblCountry.Size = new System.Drawing.Size(120, 15);
            this.lblCountry.TabIndex = 6;
            this.lblCountry.Text = "label1";
            // 
            // pictureBox
            // 
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.InitialImage = global::NetRadio.Properties.Resources.epul;
            this.pictureBox.Location = new System.Drawing.Point(231, 67);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(125, 125);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox.TabIndex = 5;
            this.pictureBox.TabStop = false;
            // 
            // defBitrate
            // 
            this.defBitrate.AutoSize = true;
            this.defBitrate.Location = new System.Drawing.Point(12, 170);
            this.defBitrate.Name = "defBitrate";
            this.defBitrate.Size = new System.Drawing.Size(48, 17);
            this.defBitrate.TabIndex = 4;
            this.defBitrate.Text = "Bitrate:";
            // 
            // defCodec
            // 
            this.defCodec.AutoSize = true;
            this.defCodec.Location = new System.Drawing.Point(12, 145);
            this.defCodec.Name = "defCodec";
            this.defCodec.Size = new System.Drawing.Size(48, 17);
            this.defCodec.TabIndex = 3;
            this.defCodec.Text = "Codec:";
            // 
            // defVotes
            // 
            this.defVotes.AutoSize = true;
            this.defVotes.Location = new System.Drawing.Point(12, 120);
            this.defVotes.Name = "defVotes";
            this.defVotes.Size = new System.Drawing.Size(43, 17);
            this.defVotes.TabIndex = 2;
            this.defVotes.Text = "Votes:";
            // 
            // defLanguage
            // 
            this.defLanguage.AutoSize = true;
            this.defLanguage.Location = new System.Drawing.Point(12, 95);
            this.defLanguage.Name = "defLanguage";
            this.defLanguage.Size = new System.Drawing.Size(68, 17);
            this.defLanguage.TabIndex = 1;
            this.defLanguage.Text = "Language:";
            // 
            // defCountry
            // 
            this.defCountry.AutoSize = true;
            this.defCountry.Location = new System.Drawing.Point(12, 70);
            this.defCountry.Name = "defCountry";
            this.defCountry.Size = new System.Drawing.Size(56, 17);
            this.defCountry.TabIndex = 0;
            this.defCountry.Text = "Country:";
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(242, 231);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(114, 25);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close (Esc/Enter)";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            this.btnNext.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnNext.Location = new System.Drawing.Point(135, 231);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(101, 25);
            this.btnNext.TabIndex = 4;
            this.btnNext.Text = "Next (F4/PgDn)";
            this.btnNext.UseVisualStyleBackColor = true;
            // 
            // btnPrevious
            // 
            this.btnPrevious.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnPrevious.Location = new System.Drawing.Point(5, 231);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(124, 25);
            this.btnPrevious.TabIndex = 5;
            this.btnPrevious.Text = "Previous (F3/PgUp)";
            this.btnPrevious.UseVisualStyleBackColor = true;
            // 
            // FrmStationInfo
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(363, 266);
            this.Controls.Add(this.btnPrevious);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.panelBackground);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmStationInfo";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmStationInfo";
            this.TopMost = true;
            this.panelBackground.ResumeLayout(false);
            this.panelBackground.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);

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
    }
}