using Microsoft.Extensions.Logging;

namespace Communications
{
    public class Networking
    {
        //Fields
        private delegate void ReportMessageArrived(Networking channel, string message);
        private delegate void ReportDisconnect(Networking channel);
        private delegate void ReportConnectionEstablished(Networking channel);
        
        public string ID { get; set; }

        public Networking(ILogger logger, ReportConnectionEstablished onConnect, ReportDisconnect reportDisconnect
                          ReportMessageArrived onMessage, char terminationCharacter)
        {

        }

    }
}