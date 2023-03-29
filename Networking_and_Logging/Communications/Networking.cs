using Microsoft.Extensions.Logging;

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

        public string ID { get; set; }

        public Networking(ILogger logger, ReportConnectionEstablished onConnect, ReportDisconnect reportDisconnect,
                          ReportMessageArrived onMessage, char terminationCharacter)
        {
            ID = "Jim";
            this.onMessage = onMessage;
            this.reportDisconnect = reportDisconnect;
            this.onConnect = onConnect;
        }

        public void Connect(string host, int port)
        {
            
        }

        public async void AwaitMessagesAsync(bool infinite = true)
        {

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