using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CaroClient
{
    public partial class Form1 : Form
    {
        const int BOARD_SIZE = 15;
        const int CELL_SIZE = 35;

        enum Cell { Empty, X, O }

        Cell[,] board = new Cell[BOARD_SIZE, BOARD_SIZE];
        bool myTurn = false;
        Cell mySymbol = Cell.X;

        TcpClient? client;
        StreamReader? reader;
        StreamWriter? writer;

        string playerName = "Player";

        // Tổng kết thắng thua
        int wins = 0;
        int losses = 0;

        // List hiển thị lịch sử trận đấu
        ListBox lstHistory = new ListBox();

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;

            lstHistory.Location = new Point(BOARD_SIZE * CELL_SIZE + 70, 50);
            lstHistory.Size = new Size(200, 400);
            Controls.Add(lstHistory);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ResetBoard();
        }

        private void ResetBoard()
        {
            for (int i = 0; i < BOARD_SIZE; i++)
                for (int j = 0; j < BOARD_SIZE; j++)
                    board[i, j] = Cell.Empty;

            myTurn = false;
            Invalidate();
        }

        private void ConnectServer(string ip, int port)
        {
            try
            {
                client = new TcpClient(ip, port);
                reader = new StreamReader(client.GetStream(), Encoding.UTF8);
                writer = new StreamWriter(client.GetStream(), Encoding.UTF8) { AutoFlush = true };

                writer.WriteLine(playerName);

                Thread t = new Thread(ReceiveThread);
                t.IsBackground = true;
                t.Start();

                MessageBox.Show("Đã kết nối server!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
            }
        }

        private void ReceiveThread()
        {
            try
            {
                while (true)
                {
                    string? msg = reader?.ReadLine();
                    if (msg == null) continue;

                    this.Invoke(new Action(() =>
                    {
                        if (msg.StartsWith("ROOM"))
                        {
                            string[] p = msg.Split(' ');
                            mySymbol = (p[1] == "X") ? Cell.X : Cell.O;
                            Text = $"Bạn: {playerName} ({mySymbol})";
                        }
                        else if (msg == "TURN")
                        {
                            myTurn = true;
                        }
                        else if (msg.StartsWith("MOVE"))
                        {
                            string[] s = msg.Split(' ');
                            int r = int.Parse(s[1]);
                            int c = int.Parse(s[2]);
                            Cell sym = (s[3] == "X") ? Cell.X : Cell.O;
                            board[r, c] = sym;
                            Invalidate();

                            if (sym != mySymbol) myTurn = true;
                        }
                        else if (msg.StartsWith("WIN"))
                        {
                            string winnerName = msg.Substring(4).Trim();

                            // Lưu/trình bày kết quả 1 lần
                            if (winnerName == playerName) wins++;
                            else losses++;

                            lstHistory.Items.Add($"{winnerName} thắng ");

                            ResetBoard();
                        }
                    }));
                }
            }
            catch { }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawBoard(e.Graphics);
        }

        private void DrawBoard(Graphics g)
        {
            for (int i = 0; i <= BOARD_SIZE; i++)
            {
                g.DrawLine(Pens.Black, 50, 50 + i * CELL_SIZE, 50 + BOARD_SIZE * CELL_SIZE, 50 + i * CELL_SIZE);
                g.DrawLine(Pens.Black, 50 + i * CELL_SIZE, 50, 50 + i * CELL_SIZE, 50 + BOARD_SIZE * CELL_SIZE);
            }

            for (int i = 0; i < BOARD_SIZE; i++)
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    if (board[i, j] == Cell.X)
                        g.DrawString("X", new Font("Arial", 18, FontStyle.Bold), Brushes.Red,
                            50 + j * CELL_SIZE + 5, 50 + i * CELL_SIZE + 5);
                    else if (board[i, j] == Cell.O)
                        g.DrawString("O", new Font("Arial", 18, FontStyle.Bold), Brushes.Blue,
                            50 + j * CELL_SIZE + 5, 50 + i * CELL_SIZE + 5);
                }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (!myTurn || writer == null) return;

            int col = (e.X - 50) / CELL_SIZE;
            int row = (e.Y - 50) / CELL_SIZE;

            if (row < 0 || col < 0 || row >= BOARD_SIZE || col >= BOARD_SIZE) return;

            if (board[row, col] == Cell.Empty)
            {
                board[row, col] = mySymbol;
                Invalidate();

                string send = $"MOVE {row} {col} {mySymbol}";
                writer.WriteLine(send);

                myTurn = false;

                if (CheckWin(row, col))
                {
                    // Chỉ gửi WIN 1 lần
                    writer.WriteLine($"WIN {playerName}");

                    // Lưu kết quả 1 lần
                    wins++;
                    // lstHistory.Items.Add($"{playerName} thắng 2");

                    ResetBoard();
                }
            }
        }

        private bool CheckWin(int r, int c)
        {
            int[][] dirs = new int[][] { new int[] { 0, 1 }, new int[] { 1, 0 }, new int[] { 1, 1 }, new int[] { 1, -1 } };
            foreach (var d in dirs)
            {
                int count = 1;
                count += Count(r, c, d[0], d[1]);
                count += Count(r, c, -d[0], -d[1]);
                if (count >= 5) return true;
            }
            return false;
        }

        private int Count(int r, int c, int dr, int dc)
        {
            int cnt = 0;
            Cell current = board[r, c];
            int nr = r + dr, nc = c + dc;
            while (nr >= 0 && nr < BOARD_SIZE && nc >= 0 && nc < BOARD_SIZE && board[nr, nc] == current)
            {
                cnt++;
                nr += dr;
                nc += dc;
            }
            return cnt;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            using (Form inputForm = new Form())
            {
                inputForm.Width = 300;
                inputForm.Height = 150;
                inputForm.Text = "Nhập tên người chơi";

                TextBox txtName = new TextBox() { Left = 50, Top = 20, Width = 200 };
                Button btnOK = new Button() { Text = "OK", Left = 100, Top = 60, Width = 80 };
                btnOK.DialogResult = DialogResult.OK;

                inputForm.Controls.Add(txtName);
                inputForm.Controls.Add(btnOK);
                inputForm.AcceptButton = btnOK;

                if (inputForm.ShowDialog() == DialogResult.OK)
                    playerName = string.IsNullOrWhiteSpace(txtName.Text) ? "Player" : txtName.Text.Trim();
            }

            string ip = txtIP.Text.Trim();
            if (string.IsNullOrEmpty(ip)) ip = "127.0.0.1";
            ConnectServer(ip, 5000);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetBoard();
        }
    }
}
