namespace SnakeOnline
{
    partial class MainForm
    {
                                private System.ComponentModel.IContainer components = null;

                                        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            gamePanel = new Panel();
            txtIpAddress = new TextBox();
            btnJoin = new Button();
            btnHost = new Button();
            btnSolo = new Button();
            gamePanel.SuspendLayout();
            SuspendLayout();
            // 
            // gamePanel
            // 
            gamePanel.Controls.Add(txtIpAddress);
            gamePanel.Controls.Add(btnJoin);
            gamePanel.Controls.Add(btnHost);
            gamePanel.Controls.Add(btnSolo);
            gamePanel.Location = new Point(34, 23);
            gamePanel.Name = "gamePanel";
            gamePanel.Size = new Size(521, 515);
            gamePanel.TabIndex = 0;
            // 
            // txtIpAddress
            // 
            txtIpAddress.Location = new Point(88, 263);
            txtIpAddress.Name = "txtIpAddress";
            txtIpAddress.Size = new Size(100, 23);
            txtIpAddress.TabIndex = 3;
            // 
            // btnJoin
            // 
            btnJoin.Location = new Point(88, 227);
            btnJoin.Name = "btnJoin";
            btnJoin.Size = new Size(100, 30);
            btnJoin.TabIndex = 2;
            btnJoin.Text = "Join the party";
            btnJoin.UseVisualStyleBackColor = true;
            btnJoin.Click += btnJoin_Click_1;
            // 
            // btnHost
            // 
            btnHost.Location = new Point(338, 227);
            btnHost.Name = "btnHost";
            btnHost.Size = new Size(100, 30);
            btnHost.TabIndex = 1;
            btnHost.Text = "Host the party";
            btnHost.UseVisualStyleBackColor = true;
            btnHost.Click += btnHost_Click;
            // 
            // btnSolo
            // 
            btnSolo.AccessibleRole = AccessibleRole.None;
            btnSolo.Location = new Point(219, 227);
            btnSolo.Name = "btnSolo";
            btnSolo.Size = new Size(100, 30);
            btnSolo.TabIndex = 0;
            btnSolo.Text = "Solo leveling";
            btnSolo.UseVisualStyleBackColor = true;
            btnSolo.Click += btnSolo_Click_1;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(597, 568);
            Controls.Add(gamePanel);
            Name = "MainForm";
            Text = "Form1";
            gamePanel.ResumeLayout(false);
            gamePanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel gamePanel;
        private Button btnJoin;
        private Button btnHost;
        private Button btnSolo;
        private TextBox txtIpAddress;
    }
}
