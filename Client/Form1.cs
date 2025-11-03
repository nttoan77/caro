using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace CaroClient
{
    public partial class Form1 : Form
    {
        const int BOARD_SIZE = 15;
        const int CELL_SIZE = 35;

        enum Cell { Empty, X, O }

        Cell[,] board = new Cell[BOARD_SIZE, BOARD_SIZE];
        bool myTurn = true;
        Cell mySymbol = Cell.X;
        UdpClient client;
        IPEndPoint serverEP;

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
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
            Invalidate();
        }

        private void ConnectServer(string ip, int port)
        {
            try
            {
                client = new UdpClient();
                serverEP = new IPEndPoint(IPAddress.Parse(ip), port);
                byte[] ping = Encoding.UTF8.GetBytes("PING");
                client.Send(ping, ping.Length, serverEP);
                MessageBox.Show("Đã kết nối server!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
            }
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
            int col = (e.X - 50) / CELL_SIZE;
            int row = (e.Y - 50) / CELL_SIZE;
            if (row < 0 || col < 0 || row >= BOARD_SIZE || col >= BOARD_SIZE) return;

            if (board[row, col] == Cell.Empty)
            {
                board[row, col] = mySymbol;
                Invalidate();

                if (CheckWin(row, col))
                {
                    MessageBox.Show($"{mySymbol} thắng!");
                    ResetBoard();
                    return;
                }

                mySymbol = (mySymbol == Cell.X) ? Cell.O : Cell.X;
            }
        }

        private bool CheckWin(int r, int c)
        {
            int[][] dirs = new int[][]
            {
                new int[]{0,1}, new int[]{1,0}, new int[]{1,1}, new int[]{1,-1}
            };
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
            if (comboBoxMode.Text == "Online")
            {
                string ip = txtIP.Text.Trim();
                if (string.IsNullOrEmpty(ip)) ip = "127.0.0.1";
                ConnectServer(ip, 5000);
            }
            else
                MessageBox.Show("Chỉ dùng khi chọn chế độ Online!");
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetBoard();
        }
    }
}
