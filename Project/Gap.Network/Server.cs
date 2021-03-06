﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gap.Network
{
    using System.Net.Sockets;
    using System.Threading;

    public delegate void ClientConnectedEventHandler(object sender, ClientConnectedEventArgs e);

    public class Server
    {
        public event ClientConnectedEventHandler ClientConnected;

        private readonly Configuration configuration = Configuration.Load();

        private readonly TcpListener tcpListener;

        public Server()
        {
            tcpListener = new TcpListener(configuration.ServerAddress, configuration.ServerPort);
        }

        public void Start()
        {
            this.tcpListener.Start(5);

            Console.WriteLine("Server is running on {0}:{1}", configuration.ServerAddress, configuration.ServerPort);

            this.HandleConnections();
        }

        private void HandleConnections()
        {
            while (true)
            {
                TcpClient tcpClient = tcpListener.AcceptTcpClient();

                var clientConnectedEventArgs = new ClientConnectedEventArgs { TcpClient = tcpClient };

                //Thread clientThread = new Thread(() => OnClientConnected(clientConnectedEventArgs));
                //clientThread.Name = "Server_clientThread";
                //clientThread.Start();

                Task.Run(() => OnClientConnected(clientConnectedEventArgs));
            }
        }

        private void OnClientConnected(ClientConnectedEventArgs e)
        {
            ClientConnectedEventHandler handler = this.ClientConnected;

            if (handler != null)
            {
                handler(this, e);
            }
        }
    }

    public class ClientConnectedEventArgs : EventArgs
    {
        public TcpClient TcpClient { get; set; }
    }
}
