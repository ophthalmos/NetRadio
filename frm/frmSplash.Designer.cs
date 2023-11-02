namespace NetRadio
{
    partial class SplashForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashForm));
            splashLabel = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // splashLabel
            // 
            splashLabel.BackColor = System.Drawing.SystemColors.Info;
            splashLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            splashLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            splashLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            splashLabel.Location = new System.Drawing.Point(0, 0);
            splashLabel.Name = "splashLabel";
            splashLabel.Size = new System.Drawing.Size(337, 63);
            splashLabel.TabIndex = 0;
            splashLabel.Text = resources.GetString("splashLabel.Text");
            // 
            // SplashForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(337, 63);
            Controls.Add(splashLabel);
            Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SplashForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            Activated += SplashForm_Activated;
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label splashLabel;
    }
}