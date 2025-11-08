using System.Windows.Forms;
using System.Drawing;

namespace CaroClient
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private ComboBox comboBoxMode;
        private Label labelMode;
        private ComboBox comboBoxDifficulty;
        private Label labelDifficulty;
        private TextBox txtIP;
        private Label labelIP;
        private Button btnConnect;
        private Button btnReset;
        private Label lblScore;
        private ListBox lstHistory;
        private Label lblWinner;

        private RichTextBox rtbChat;   // hiển thị tin nhắn
        private TextBox txtMessage;    // nhập tin nhắn
        private Button btnSend;        // gửi tin nhắn

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.comboBoxMode = new ComboBox();
            this.labelMode = new Label();
            this.comboBoxDifficulty = new ComboBox();
            this.labelDifficulty = new Label();
            this.txtIP = new TextBox();
            this.labelIP = new Label();
            this.btnConnect = new Button();
            this.btnReset = new Button();
            this.lblScore = new Label();
            this.lstHistory = new ListBox();
            this.lblWinner = new Label();

            this.SuspendLayout();

            // ================= ComboBox Mode =================
            this.labelMode.Text = "Chế độ:";
            this.labelMode.Location = new Point(20, 15);
            this.labelMode.AutoSize = true;
            this.labelMode.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            this.comboBoxMode.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxMode.Items.AddRange(new object[] { "Local 2P", "AI", "Online" });
            this.comboBoxMode.Location = new Point(100, 12);
            this.comboBoxMode.Size = new Size(120, 25);
            this.comboBoxMode.Font = new Font("Segoe UI", 10);

            // ================= ComboBox Difficulty =================
            this.labelDifficulty.Text = "Độ khó:";
            this.labelDifficulty.Location = new Point(235, 15);
            this.labelDifficulty.AutoSize = true;
            this.labelDifficulty.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            this.comboBoxDifficulty.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxDifficulty.Items.AddRange(new object[] { "Easy", "Medium", "Hard" });
            this.comboBoxDifficulty.Location = new Point(310, 12);
            this.comboBoxDifficulty.Size = new Size(100, 25);
            this.comboBoxDifficulty.Font = new Font("Segoe UI", 10);

            // ================= TextBox IP =================
            this.labelIP.Text = "Server IP:";
            this.labelIP.Location = new Point(415, 15);
            this.labelIP.AutoSize = true;
            this.labelIP.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            this.txtIP.Location = new Point(500, 12);
            this.txtIP.Size = new Size(100, 25);
            this.txtIP.Font = new Font("Segoe UI", 10);
            this.txtIP.PlaceholderText = "127.0.0.1";

            // ================= Buttons =================
            this.btnConnect.Text = "Bắt đầu";
            this.btnConnect.Location = new Point(620, 10);
            this.btnConnect.Size = new Size(80, 28);
            this.btnConnect.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            this.btnConnect.BackColor = Color.LightGreen;
            this.btnConnect.FlatStyle = FlatStyle.Flat;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);

            this.btnReset.Text = "Chơi lại";
            this.btnReset.Location = new Point(710, 10);
            this.btnReset.Size = new Size(80, 28);
            this.btnReset.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            this.btnReset.BackColor = Color.LightCoral;
            this.btnReset.FlatStyle = FlatStyle.Flat;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);

            // ================= Label Score =================
            this.lblScore.Text = "Tỷ số: 0 - 0";
            this.lblScore.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            this.lblScore.Location = new Point(810, 12);
            this.lblScore.AutoSize = true;

            // ================= ListBox History =================
            this.lstHistory.Location = new Point(600, 60);
            this.lstHistory.Size = new Size(500, 150);
            this.lstHistory.Font = new Font("Segoe UI", 9);
            this.lstHistory.BorderStyle = BorderStyle.FixedSingle;

            // ================= Label Winner =================
            this.lblWinner.Location = new Point(50, 580);
            this.lblWinner.Size = new Size(400, 30);
            this.lblWinner.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            this.lblWinner.Text = "Người thắng: ";
            
            // ================= RichTextBox Chat =================
            this.rtbChat = new RichTextBox();
            this.rtbChat.Location = new Point(600, 230);
            this.rtbChat.Size = new Size(380, 240);
            this.rtbChat.Font = new Font("Segoe UI", 9);
            this.rtbChat.ReadOnly = true;
            this.rtbChat.BackColor = Color.WhiteSmoke;
            this.rtbChat.BorderStyle = BorderStyle.FixedSingle;

            // ================= TextBox Message =================
            this.txtMessage = new TextBox();
            this.txtMessage.Location = new Point(600, 480);
            this.txtMessage.Size = new Size(300, 28);
            this.txtMessage.Font = new Font("Segoe UI", 10);
            this.txtMessage.KeyDown += TxtMessage_KeyDown; // nhấn Enter gửi

            // ================= Button Send =================
            this.btnSend = new Button();
            this.btnSend.Text = "Gửi";
            this.btnSend.Location = new Point(910, 480);
            this.btnSend.Size = new Size(70, 28);
            this.btnSend.BackColor = Color.LightBlue;
            this.btnSend.FlatStyle = FlatStyle.Flat;
            this.btnSend.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            this.btnSend.Click += BtnSend_Click;

            // ================= Thêm vào Form =================
            this.Controls.Add(this.rtbChat);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnSend);
                

            // ================= Form =================
            this.ClientSize = new Size(1000, 650);
            this.Controls.Add(this.labelMode);
            this.Controls.Add(this.comboBoxMode);
            this.Controls.Add(this.labelDifficulty);
            this.Controls.Add(this.comboBoxDifficulty);
            this.Controls.Add(this.labelIP);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.lblScore);
            this.Controls.Add(this.lstHistory);
            this.Controls.Add(this.lblWinner);

            this.Text = "🎮 Game Caro (Client)";
            this.Load += new System.EventHandler(this.Form1_Load);

            this.BackColor = Color.WhiteSmoke;

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
