using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CaroServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "=== CARO SERVER ===";
            Console.WriteLine("Server Caro đang khởi động...");

            UdpClient server = new UdpClient(5000);
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);

            Console.WriteLine("Server sẵn sàng nhận kết nối tại cổng 5000\n");

            while (true)
            {
                byte[] data = server.Receive(ref remoteEP);
                string message = Encoding.UTF8.GetString(data);
                Console.WriteLine($"[{DateTime.Now:T}] Nhận từ {remoteEP}: {message}");

                if (message.StartsWith("MOVE"))
                {
                    // Xử lý nước đi (nếu cần)
                    string reply = "OK";
                    byte[] sendData = Encoding.UTF8.GetBytes(reply);
                    server.Send(sendData, sendData.Length, remoteEP);
                }
                else if (message == "PING")
                {
                    byte[] pong = Encoding.UTF8.GetBytes("PONG");
                    server.Send(pong, pong.Length, remoteEP);
                }
            }
        }
    }
}
