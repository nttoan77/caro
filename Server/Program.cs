


using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CaroServer
{
    class Player
    {
        public TcpClient? Client;
        public StreamReader? Reader;
        public StreamWriter? Writer;
        public string? Name;
        public Room? RoomRef;
    }

    class Room
    {
        public Player? P1;
        public Player? P2;
        public bool IsXTurn = true; // true = X, false = O
        private static Random rnd = new Random();

        public void Reset()
        {
            if (P1 != null && P2 != null)
            {
                // Random người đi X/O
                if (rnd.Next(2) == 0)
                {
                    Send(P1, "ROOM X");
                    Send(P2, "ROOM O");
                    Send(P1, "TURN");
                    Send(P1, $"START {P1.Name}");  // Thông báo người đi trước
                    Send(P2, $"START {P1.Name}");
                    IsXTurn = true;
                }
                else
                {
                    Send(P1, "ROOM O");
                    Send(P2, "ROOM X");
                    Send(P2, "TURN");
                    Send(P1, $"START {P2.Name}");
                    Send(P2, $"START {P2.Name}");
                    IsXTurn = false;
                }
            }
        }

        private void Send(Player? p, string text)
        {
            if (p?.Writer == null) return;
            try
            {
                p.Writer.WriteLine(text);
            }
            catch { }
        }
    }

    class Program
    {
        static List<Player> waitingPlayers = new List<Player>();
        static List<Room> rooms = new List<Room>();

        static void Main()
        {
            Console.Title = "=== CARO SERVER ===";
            TcpListener server = new TcpListener(IPAddress.Any, 5000);
            server.Start();
            Console.WriteLine("Server running on port 5000...\n");

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                Thread t = new Thread(() => HandleClient(client));
                t.IsBackground = true;
                t.Start();
            }
        }

        static void HandleClient(TcpClient client)
        {
            Player p = new Player
            {
                Client = client,
                Reader = new StreamReader(client.GetStream(), Encoding.UTF8),
                Writer = new StreamWriter(client.GetStream(), Encoding.UTF8) { AutoFlush = true }
            };

            Send(p, "HELLO");

            try
            {
                p.Name = p.Reader?.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(p.Name))
                    p.Name = "Unknown";

                Console.WriteLine($"Client joined: {p.Name}");
            }
            catch
            {
                client.Close();
                return;
            }

            // Ghép phòng
            lock (waitingPlayers)
            {
                waitingPlayers.Add(p);
                if (waitingPlayers.Count >= 2)
                {
                    Player p1 = waitingPlayers[0]!;
                    Player p2 = waitingPlayers[1]!;
                    waitingPlayers.RemoveRange(0, 2);

                    Room r = new Room { P1 = p1, P2 = p2 };
                    rooms.Add(r);
                    p1.RoomRef = r;
                    p2.RoomRef = r;

                    r.Reset(); // Random người đi trước và gửi START
                }
            }

            try
            {
                while (true)
                {
                    string? msg = p.Reader?.ReadLine();
                    if (string.IsNullOrEmpty(msg))
                    {
                        Thread.Sleep(50);
                        continue;
                    }

                    if (msg.StartsWith("MOVE"))
                    {
                        Room? r = p.RoomRef;
                        if (r?.P1 == null || r?.P2 == null) continue;

                        Player other = (r.P1 == p) ? r.P2! : r.P1!;
                        Send(other, msg);

                        r.IsXTurn = !r.IsXTurn;
                        Send(other, "TURN");
                    }
                    else if (msg.StartsWith("WIN"))
                    {
                        string winnerName = msg.Substring(4).Trim();
                        string log = $"{winnerName} thắng lúc {DateTime.Now}";
                        File.AppendAllText("result.txt", log + Environment.NewLine);
                        Console.WriteLine($"SAVE RESULT => {log}");

                        Room? r = p.RoomRef;
                        if (r?.P1 != null && r?.P2 != null)
                        {
                            Send(r.P1, $"WIN {winnerName}");
                            Send(r.P2, $"WIN {winnerName}");
                            r.Reset(); // Reset trận mới
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error with {p.Name}: {ex.Message}");
            }
            finally
            {
                client.Close();
                lock (waitingPlayers)
                {
                    waitingPlayers.Remove(p);
                }
            }
        }

        static void Send(Player? p, string text)
        {
            if (p?.Writer == null) return;
            try { p.Writer.WriteLine(text); }
            catch { }
        }
    }
}

