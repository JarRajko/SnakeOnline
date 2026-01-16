using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using SnakeOnline.Snake.Core.Network;
using SnakeOnline.Snake.Core.Enum;

namespace SnakeOnline.Snake.Client
{
    public class SnakeClient
    {
        private UdpClient _udpClient;
        private IPEndPoint _serverEndPoint;
        private const int Port = 11000;

        public SnakeClient(string serverIp)
        {
            _udpClient = new UdpClient();
            _serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIp), Port);
        }

        public void SendInput(InputPacket packet)
        {
            string json = JsonSerializer.Serialize(packet);
            byte[] data = Encoding.UTF8.GetBytes(json);
            _udpClient.Send(data, data.Length, _serverEndPoint);
        }

        public void Connect()
        {
            InputPacket helloPacket = new InputPacket();
            helloPacket.ChosenDirection = Core.Enum.Direction.NONE;
            SendInput(helloPacket);
        }

        public GameStatePacket ReceiveWorldState()
        {
            if (_udpClient.Available <= 0) return null;

            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = _udpClient.Receive(ref remoteEP);
            string json = Encoding.UTF8.GetString(data);
            return JsonSerializer.Deserialize<GameStatePacket>(json);
        }
    }
}