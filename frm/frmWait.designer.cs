namespace NetRadio
{
    partial class FrmWait
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
            this.labelPleaseWait = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelPleaseWait
            // 
            this.labelPleaseWait.BackColor = System.Drawing.Color.Transparent;
            this.labelPleaseWait.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelPleaseWait.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPleaseWait.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPleaseWait.Location = new System.Drawing.Point(0, 0);
            this.labelPleaseWait.Name = "labelPleaseWait";
            this.labelPleaseWait.Size = new System.Drawing.Size(257, 40);
            this.labelPleaseWait.TabIndex = 0;
            this.labelPleaseWait.Text = "Please wait...";
            this.labelPleaseWait.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FrmWait
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(257, 40);
            this.Controls.Add(this.labelPleaseWait);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmWait";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "WaitWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelPleaseWait;
    }
}