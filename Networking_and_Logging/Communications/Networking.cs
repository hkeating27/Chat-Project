﻿using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Communications
{
    public class Networking
    {
        public delegate void ReportMessageArrived(Networking channel, string message);
        public delegate void ReportDisconnect(Networking channel);
        public delegate void ReportConnectionEstablished(Networking channel);

        //Fields
        private ReportMessageArrived onMessage;
        private ReportDisconnect reportDisconnect;
        private ReportConnectionEstablished onConnect;
        private CancellationTokenSource cancelSource;
        private char terminationChar;
        private TcpClient client;
        private ILogger logger;

        public string ID { get; set; }

        public Networking(ILogger logger, ReportConnectionEstablished onConnect, ReportDisconnect reportDisconnect,
                          ReportMessageArrived onMessage, char terminationCharacter)
        {
            ID                    = "";
            cancelSource           = new CancellationTokenSource();
            terminationChar       = terminationCharacter;
            this.logger           = logger;
            this.onMessage        = onMessage;
            this.onConnect        = onConnect;
            this.reportDisconnect = reportDisconnect;
            client                = new TcpClient();
        }

        public Networking(ILogger logger, TcpClient client, ReportConnectionEstablished onConnect, ReportDisconnect
                           reportDisconnect, ReportMessageArrived onMessage, char terminationCharacter)
        {
            ID = "";
            cancelSource = new CancellationTokenSource();
            terminationChar = terminationCharacter;
            this.logger = logger;
            this.onMessage = onMessage;
            this.onConnect = onConnect;
            this.reportDisconnect = reportDisconnect;
            this.client = client;
        }

        public void Connect(string host, int port)
        {
            try
            {
                client = new TcpClient(Dns.GetHostName(), port);
                throw new Exception("Connection was successful!");
            }
            catch
            {
                /*logger.LogError("There was a problem connecting to the server. Check to make sure the given " +
                                "host and port are correct.");*/
                throw new Exception("Connection was unsuccessful.");
            }
        }

        public async void AwaitMessagesAsync(bool infinite = true)
        {
            try
            {
                NetworkStream clientStream = client.GetStream();
                byte[] message = new byte[clientStream.Length];
                if (infinite == true)
                {
                    while (infinite)
                    {
                        await clientStream.ReadAsync(message, 0, message.Length);
                        onMessage(this, message.ToString());
                    }
                }
                else
                {
                    await clientStream.ReadAsync(message, 0, message.Length);
                    onMessage(this, message.ToString());
                    return;
                }
            }
            catch (ObjectDisposedException)
            {
                reportDisconnect(this);
            }
        }

        public async void WaitForClients(int port, bool infinte)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            try
            {
                while (infinte)
                {
                    TcpClient connection = await listener.AcceptTcpClientAsync(cancelSource.Token);
                    Networking newConnection = new Networking(logger, connection, onConnect, reportDisconnect,
                                                              onMessage, terminationChar);
                    onConnect(newConnection);
                }
            }
            catch
            {
                listener.Stop();
            }
        }

        public void StopWaitingForClients()
        {
            cancelSource.Cancel();
        }

        public void Disconnect()
        {
            client.Close();
        }

        public async void Send(string text)
        {
            if (!text.Contains(terminationChar))
            {
                text += terminationChar;
                NetworkStream clientStream = client.GetStream();
                byte[] message = Encoding.UTF8.GetBytes(text);
                CancellationTokenSource source = new CancellationTokenSource(terminationChar);
                await clientStream.WriteAsync(message, 0, message.Length, source.Token);
            }
        }
    }
}