using Microsoft.Extensions.Logging;
using System.Net.Sockets;

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
        private CancellationTokenSource cancelToken;
        private char terminationChar;
        private TcpClient? client;
        private ILogger logger;

        public string ID { get; set; }

        public Networking(ILogger logger, ReportConnectionEstablished onConnect, ReportDisconnect reportDisconnect,
                          ReportMessageArrived onMessage, char terminationCharacter)
        {
            ID                    = "Jim";
            cancelToken           = new CancellationTokenSource(terminationChar);
            terminationChar       = terminationCharacter;
            this.logger           = logger;
            this.onMessage        = onMessage;
            this.onConnect        = onConnect;
            this.reportDisconnect = reportDisconnect;
        }

        public void Connect(string host, int port)
        {
            try
            {
                client = new TcpClient(host, port);
            }
            catch
            {
                logger.LogError("There was a problem connecting to the server. Check to make sure the given" +
                    " host and port are correct.");
            }
        }

        public async void AwaitMessagesAsync(bool infinite = true)
        {
            if (client != null)
            {
                NetworkStream clientStream = client.GetStream();
                byte[] message = new byte[clientStream.Length];clien
                if (infinite == true)
                {
                    while (infinite)
                    {
                        await clientStream.ReadAsync(message, 0, message.Length, cancelToken.Token);
                    }
                }
                else
                {
                    await clientStream.ReadAsync(message, 0, message.Length, cancelToken.Token);
                    clientStream.EndRead();
                    return;
                }
            }
        }

        public async void WaitForClients(int port, bool infinte)
        {

        }

        public void StopWaitingForClients()
        {

        }

        public void Disconnect()
        {

        }

        public async void Start(string text)
        {

        }
    }
}