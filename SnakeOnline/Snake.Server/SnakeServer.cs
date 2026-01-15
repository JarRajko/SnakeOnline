using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using SnakeOnline.Snake.Core.Network;
using SnakeOnline.Snake.Core.Enum;

namespace SnakeOnline.Snake.Server
{
    public class SnakeServer
    {
        private UdpClient _udpListener;
        private IPEndPoint _remoteEndPoint;
        private const int Port = 11000;

        public SnakeServer()
        {
            _udpListener = new UdpClient(Port);
            _remoteEndPoint = new IPEndPoint(IPAddress.Any, Port);
        }

        public void SendWorldState(GameStatePacket packet)
        {
            if (_remoteEndPoint == null) return;

            string json = JsonSerializer.Serialize(packet);
            byte[] data = Encoding.UTF8.GetBytes(json);
            _udpListener.Send(data, data.Length, _remoteEndPoint);
        }

        public InputPacket ReceiveInput()
        {
            if (_udpListener.Available <= 0) return null;

            byte[] data = _udpListener.Receive(ref _remoteEndPoint);
            string json = Encoding.UTF8.GetString(data);
            return JsonSerializer.Deserialize<InputPacket>(json);
        }
    }
}