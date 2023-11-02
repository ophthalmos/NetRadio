namespace NetRadio
{
    partial class FrmSearch
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbSearch = new System.Windows.Forms.TextBox();
            this.lblSeach = new System.Windows.Forms.Label();
            this.linkLblRadioBrowserInfo = new System.Windows.Forms.LinkLabel();
            this.btnAccept = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblMsg1 = new System.Windows.Forms.Label();
            this.lblMsg2 = new System.Windows.Forms.Label();
            this.panel = new System.Windows.Forms.Panel();
            this.lblName = new System.Windows.Forms.Label();
            this.lblRow = new System.Windows.Forms.Label();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.panel.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbSearch
            // 
            this.tbSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tbSearch.Location = new System.Drawing.Point(8, 2);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.PlaceholderText = "Station";
            this.tbSearch.Size = new System.Drawing.Size(207, 18);
            this.tbSearch.TabIndex = 0;
            this.tbSearch.WordWrap = false;
            // 
            // lblSeach
            // 
            this.lblSeach.AutoSize = true;
            this.lblSeach.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSeach.Location = new System.Drawing.Point(15, 9);
            this.lblSeach.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSeach.Name = "lblSeach";
            this.lblSeach.Size = new System.Drawing.Size(182, 17);
            this.lblSeach.TabIndex = 11;
            this.lblSeach.Text = "Search radio station by name:";
            // 
            // linkLblRadioBrowserInfo
            // 
            this.linkLblRadioBrowserInfo.AutoSize = true;
            this.linkLblRadioBrowserInfo.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.linkLblRadioBrowserInfo.Location = new System.Drawing.Point(152, 73);
            this.linkLblRadioBrowserInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkLblRadioBrowserInfo.Name = "linkLblRadioBrowserInfo";
            this.linkLblRadioBrowserInfo.Size = new System.Drawing.Size(117, 17);
            this.linkLblRadioBrowserInfo.TabIndex = 5;
            this.linkLblRadioBrowserInfo.TabStop = true;
            this.linkLblRadioBrowserInfo.Text = "radio-browser.info";
            this.linkLblRadioBrowserInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkLblRadioBrowserInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLblRadioBrowserInfo_LinkClicked);
            // 
            // btnAccept
            // 
            this.btnAccept.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnAccept.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnAccept.FlatAppearance.BorderSize = 0;
            this.btnAccept.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAccept.Image = global::NetRadio.Properties.Resources.epul;
            this.btnAccept.Location = new System.Drawing.Point(232, 37);
            this.btnAccept.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(41, 25);
            this.btnAccept.TabIndex = 3;
            this.btnAccept.UseVisualStyleBackColor = true;
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblInfo.Location = new System.Drawing.Point(14, 73);
            this.lblInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(137, 17);
            this.lblInfo.TabIndex = 4;
            this.lblInfo.Text = "The search is done on";
            // 
            // lblMsg1
            // 
            this.lblMsg1.AutoSize = true;
            this.lblMsg1.BackColor = System.Drawing.Color.FloralWhite;
            this.lblMsg1.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblMsg1.Location = new System.Drawing.Point(4, 6);
            this.lblMsg1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMsg1.Name = "lblMsg1";
            this.lblMsg1.Size = new System.Drawing.Size(89, 17);
            this.lblMsg1.TabIndex = 7;
            this.lblMsg1.Text = "Selected Row:";
            // 
            // lblMsg2
            // 
            this.lblMsg2.AutoSize = true;
            this.lblMsg2.BackColor = System.Drawing.Color.FloralWhite;
            this.lblMsg2.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblMsg2.Location = new System.Drawing.Point(4, 29);
            this.lblMsg2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMsg2.Name = "lblMsg2";
            this.lblMsg2.Size = new System.Drawing.Size(93, 17);
            this.lblMsg2.TabIndex = 9;
            this.lblMsg2.Text = "Current Name:";
            // 
            // panel
            // 
            this.panel.BackColor = System.Drawing.Color.FloralWhite;
            this.panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel.Controls.Add(this.lblName);
            this.panel.Controls.Add(this.lblRow);
            this.panel.Controls.Add(this.lblMsg2);
            this.panel.Controls.Add(this.lblMsg1);
            this.panel.Location = new System.Drawing.Point(14, 95);
            this.panel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(259, 54);
            this.panel.TabIndex = 6;
            // 
            // lblName
            // 
            this.lblName.BackColor = System.Drawing.Color.FloralWhite;
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblName.Location = new System.Drawing.Point(101, 29);
            this.lblName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(157, 23);
            this.lblName.TabIndex = 10;
            this.lblName.Text = "name";
            // 
            // lblRow
            // 
            this.lblRow.BackColor = System.Drawing.Color.FloralWhite;
            this.lblRow.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblRow.Location = new System.Drawing.Point(101, 7);
            this.lblRow.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRow.Name = "lblRow";
            this.lblRow.Size = new System.Drawing.Size(157, 22);
            this.lblRow.TabIndex = 8;
            this.lblRow.Text = "row";
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pnlSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSearch.Controls.Add(this.tbSearch);
            this.pnlSearch.Location = new System.Drawing.Point(14, 37);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(219, 25);
            this.pnlSearch.TabIndex = 1;
            // 
            // FrmSearch
            // 
            this.AcceptButton = this.btnAccept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(287, 159);
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.linkLblRadioBrowserInfo);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.lblSeach);
            this.Controls.Add(this.lblInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSearch";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "NetRadio";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmSearch_KeyDown);
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbSearch;
        private System.Windows.Forms.Label lblSeach;
        private System.Windows.Forms.LinkLabel linkLblRadioBrowserInfo;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblMsg1;
        private System.Windows.Forms.Label lblMsg2;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblRow;
        private System.Windows.Forms.Panel pnlSearch;
    }
}