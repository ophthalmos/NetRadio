namespace NetRadio
{
    partial class FrmBrowser
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
            imageList = new System.Windows.Forms.ImageList(components);
            listView = new System.Windows.Forms.ListView();
            lbName = new System.Windows.Forms.ColumnHeader();
            lbURL = new System.Windows.Forms.ColumnHeader();
            contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            acceptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            cancelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            statusStrip = new System.Windows.Forms.StatusStrip();
            toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            toolTip = new System.Windows.Forms.ToolTip(components);
            timer = new System.Windows.Forms.Timer(components);
            btnAccept = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            btnDetail = new System.Windows.Forms.Button();
            btnEdit = new System.Windows.Forms.Button();
            contextMenuStrip.SuspendLayout();
            statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // imageList
            // 
            imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            imageList.ImageSize = new System.Drawing.Size(32, 32);
            imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // listView
            // 
            listView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            listView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { lbName, lbURL });
            listView.ContextMenuStrip = contextMenuStrip;
            listView.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            listView.FullRowSelect = true;
            listView.GridLines = true;
            listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            listView.LabelEdit = true;
            listView.Location = new System.Drawing.Point(0, 0);
            listView.Margin = new System.Windows.Forms.Padding(0);
            listView.MultiSelect = false;
            listView.Name = "listView";
            listView.ShowItemToolTips = true;
            listView.Size = new System.Drawing.Size(684, 407);
            listView.TabIndex = 0;
            listView.UseCompatibleStateImageBehavior = false;
            listView.View = System.Windows.Forms.View.Details;
            listView.ColumnWidthChanged += ListView_ColumnWidthChanged;
            listView.SelectedIndexChanged += ListView_SelectedIndexChanged;
            listView.DoubleClick += ListView_DoubleClick;
            listView.KeyDown += ListView_KeyDown;
            // 
            // lbName
            // 
            lbName.Text = "Name";
            lbName.Width = 200;
            // 
            // lbURL
            // 
            lbURL.Text = "URL";
            lbURL.Width = 480;
            // 
            // contextMenuStrip
            // 
            contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { editToolStripMenuItem, propertiesToolStripMenuItem, toolStripSeparator2, acceptToolStripMenuItem, cancelToolStripMenuItem });
            contextMenuStrip.Name = "contextMenuStrip";
            contextMenuStrip.Size = new System.Drawing.Size(185, 98);
            contextMenuStrip.Opening += ContextMenuStrip_Opening;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.Image = Properties.Resources.edit;
            editToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.ShortcutKeyDisplayString = "F2";
            editToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            editToolStripMenuItem.Text = "Edit";
            editToolStripMenuItem.Click += EditToolStripMenuItem_Click;
            // 
            // propertiesToolStripMenuItem
            // 
            propertiesToolStripMenuItem.Image = Properties.Resources.retrun;
            propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            propertiesToolStripMenuItem.ShortcutKeyDisplayString = "Alt+Enter";
            propertiesToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            propertiesToolStripMenuItem.Text = "Properties";
            propertiesToolStripMenuItem.Click += PropertiesToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(181, 6);
            // 
            // acceptToolStripMenuItem
            // 
            acceptToolStripMenuItem.Image = Properties.Resources.accept;
            acceptToolStripMenuItem.Name = "acceptToolStripMenuItem";
            acceptToolStripMenuItem.ShortcutKeyDisplayString = "Enter";
            acceptToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            acceptToolStripMenuItem.Text = "Accept";
            acceptToolStripMenuItem.Click += AcceptToolStripMenuItem_Click;
            // 
            // cancelToolStripMenuItem
            // 
            cancelToolStripMenuItem.Image = Properties.Resources.cancel;
            cancelToolStripMenuItem.Name = "cancelToolStripMenuItem";
            cancelToolStripMenuItem.ShortcutKeyDisplayString = "Esc";
            cancelToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            cancelToolStripMenuItem.Text = "Cancel";
            cancelToolStripMenuItem.Click += CancelToolStripMenuItem_Click;
            // 
            // toolStripSeparator
            // 
            toolStripSeparator.Name = "toolStripSeparator";
            toolStripSeparator.Size = new System.Drawing.Size(6, 6);
            // 
            // statusStrip
            // 
            statusStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripStatusLabel });
            statusStrip.Location = new System.Drawing.Point(0, 439);
            statusStrip.Name = "statusStrip";
            statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            statusStrip.Size = new System.Drawing.Size(684, 22);
            statusStrip.TabIndex = 1;
            statusStrip.Text = "statusStrip";
            // 
            // toolStripStatusLabel
            // 
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            toolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // toolTip
            // 
            toolTip.AutoPopDelay = 500;
            toolTip.InitialDelay = 100;
            toolTip.ReshowDelay = 100;
            // 
            // timer
            // 
            timer.Interval = 10;
            timer.Tick += Timer_Tick;
            // 
            // btnAccept
            // 
            btnAccept.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnAccept.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnAccept.Enabled = false;
            btnAccept.Location = new System.Drawing.Point(567, 411);
            btnAccept.Name = "btnAccept";
            btnAccept.Size = new System.Drawing.Size(105, 25);
            btnAccept.TabIndex = 2;
            btnAccept.Text = "Accept";
            btnAccept.UseVisualStyleBackColor = true;
            btnAccept.Click += BtnAccept_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(466, 411);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(95, 25);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnDetail
            // 
            btnDetail.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnDetail.Enabled = false;
            btnDetail.Location = new System.Drawing.Point(12, 410);
            btnDetail.Name = "btnDetail";
            btnDetail.Size = new System.Drawing.Size(105, 25);
            btnDetail.TabIndex = 4;
            btnDetail.Text = "Properties";
            btnDetail.UseVisualStyleBackColor = true;
            btnDetail.Click += BtnDetail_Click;
            // 
            // btnEdit
            // 
            btnEdit.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnEdit.Enabled = false;
            btnEdit.Location = new System.Drawing.Point(123, 411);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new System.Drawing.Size(105, 25);
            btnEdit.TabIndex = 5;
            btnEdit.Text = "Edit Name";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += BtnEdit_Click;
            // 
            // FrmBrowser
            // 
            AcceptButton = btnAccept;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(684, 461);
            Controls.Add(btnEdit);
            Controls.Add(btnDetail);
            Controls.Add(btnCancel);
            Controls.Add(btnAccept);
            Controls.Add(statusStrip);
            Controls.Add(listView);
            Font = new System.Drawing.Font("Segoe UI", 9.5F);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new System.Drawing.Size(435, 210);
            Name = "FrmBrowser";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "frmBrowser";
            Load += FrmBrowser_Load;
            Resize += FrmBrowser_Resize;
            contextMenuStrip.ResumeLayout(false);
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader lbName;
        private System.Windows.Forms.ColumnHeader lbURL;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem acceptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cancelToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDetail;
        private System.Windows.Forms.Button btnEdit;
    }
}