using System.Net.Sockets;
using System.Net;
using System.Text;
using System.ComponentModel.Design;

namespace ConsoleApp1
{
    class Program
    {
        static int port = 8080;
        static void Main(string[] args)
        {
            IPAddress iPAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipPoint = new IPEndPoint(iPAddress, port);
            EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            try
            {
                listenSocket.Bind(ipPoint);
                Console.WriteLine("Server started! Waiting for connection...");

                while (true)
                {
                    int bytes = 0;
                    byte[] data = new byte[1024];
                    bytes = listenSocket.ReceiveFrom(data, ref remoteEndPoint);

                    string msg = Encoding.Unicode.GetString(data, 0, bytes);
                    Console.WriteLine($"{DateTime.Now.ToShortTimeString()}: {msg} from {remoteEndPoint}");
                    string message = "";
                    if (msg.ToLower().Contains("hello"))
                        message = "Hello!";
                    else if (msg.ToLower().Contains("bye"))
                        message = "Bye!";
                    else
                        message = "Message was send!";
                    data = Encoding.Unicode.GetBytes(message);
                    listenSocket.SendTo(data, remoteEndPoint);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}