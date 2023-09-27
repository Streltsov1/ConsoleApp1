using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int port = int.Parse(PortTb.Text);
            string address =IPTb.Text;
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
                EndPoint remoteIpPoint = new IPEndPoint(IPAddress.Any, 0);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                string message = MessageTb.Text;
                byte[] data = Encoding.Unicode.GetBytes(message);
                socket.SendTo(data, ipPoint);
                int bytes = 0;
                string response = "";
                data = new byte[1024]; // 1KB
                do
                {
                    bytes = socket.ReceiveFrom(data, ref remoteIpPoint);
                    response += Encoding.Unicode.GetString(data, 0, bytes);
                } while (socket.Available > 0);

                Dialog.Text += response + "\n";
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
