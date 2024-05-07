using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ChatServer.Net.IO;


namespace ChatServer.Net
{
    public class Server
    {
        TcpClient tcpClient;
        public Server()
        {
            tcpClient = new TcpClient();
        }

        public void ConnectToServer(string username)
        {
            if (!tcpClient.Connected)
            {
                tcpClient.Connect("127.0.0.1", 7891);
                var connectPacket = new PacketBuilder();
                connectPacket.WriteOpCode(0);
                connectPacket.WriteMessage(username);
                tcpClient.Client.Send(connectPacket.GetPacketBytes());
            }
        }
    }
}
