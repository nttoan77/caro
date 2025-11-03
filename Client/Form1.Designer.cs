namespace CaroClient
{
    partial class Form1
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
            this.comboBoxMode = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxDifficulty = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // comboBoxMode
            this.comboBoxMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMode.FormattingEnabled = true;
            this.comboBoxMode.Items.AddRange(new object[] {
            "Local 2P",
            "AI",
            "Online"});
            this.comboBoxMode.Location = new System.Drawing.Point(80, 15);
            this.comboBoxMode.Name = "comboBoxMode";
            this.comboBoxMode.Size = new System.Drawing.Size(121, 25);
            this.comboBoxMode.TabIndex = 0;

            // label1
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 18);
            this.label1.Name = "label1";
            this.label1.Text = "Chế độ:";

            // comboBoxDifficulty
            this.comboBoxDifficulty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDifficulty.FormattingEnabled = true;
            this.comboBoxDifficulty.Items.AddRange(new object[] {
            "Easy",
            "Medium",
            "Hard"});
            this.comboBoxDifficulty.Location = new System.Drawing.Point(300, 15);
            this.comboBoxDifficulty.Name = "comboBoxDifficulty";
            this.comboBoxDifficulty.Size = new System.Drawing.Size(100, 25);
            this.comboBoxDifficulty.TabIndex = 2;

            // label2
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(220, 18);
            this.label2.Name = "label2";
            this.label2.Text = "Độ khó:";

            // txtIP
            this.txtIP.Location = new System.Drawing.Point(490, 15);
            this.txtIP.Name = "txtIP";
            this.txtIP.PlaceholderText = "127.0.0.1";
            this.txtIP.Size = new System.Drawing.Size(100, 25);
            this.txtIP.TabIndex = 4;

            // label3
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(420, 18);
            this.label3.Name = "label3";
            this.label3.Text = "Server IP:";

            // btnConnect
            this.btnConnect.Location = new System.Drawing.Point(600, 15);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(80, 28);
            this.btnConnect.Text = "Kết nối";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);

            // btnReset
            this.btnReset.Location = new System.Drawing.Point(690, 15);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(80, 28);
            this.btnReset.Text = "Chơi lại";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);

            // Form1
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxDifficulty);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxMode);
            this.Name = "Form1";
            this.Text = "🎮 Game Caro (Client)";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private ComboBox comboBoxMode;
        private Label label1;
        private ComboBox comboBoxDifficulty;
        private Label label2;
        private TextBox txtIP;
        private Label label3;
        private Button btnConnect;
        private Button btnReset;
    }
}
