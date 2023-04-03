using Communications;
using FileLogger;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;

namespace ChatServer
{
    public partial class MainPage : ContentPage
    {
        //Field
        Networking serverNetwork;
        List<Networking> connectedClients;

        /// <summary>
        /// Initializes the GUI, sets the server up to work on the local host, creates a new Networking object, 
        /// initializes the list of Networking objects, and creates a new thread that the server will wait 
        /// for clients on.
        /// </summary>
        public MainPage(ILogger<MainPage> logger)
        {
            InitializeComponent();
            serverName.Text = Dns.GetHostName();
            ipAddress.Text = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();

            serverNetwork = new Networking(logger, connectionComplete, connectionDropped, messageArrived, '\n');
            connectedClients = new List<Networking>();

            Thread startThread = new Thread(() => serverNetwork.WaitForClients(11000, true));
            startThread.Start();
            connectedClients.Add(serverNetwork);
        }

        private void connectionComplete(Networking channel)
        {
            connectedClients.Add(channel);
            Label someoneConnected = new Label();
            someoneConnected.Text = "Someone has connected";
            allSentMessages.Add(someoneConnected);
        }

        private void connectionDropped(Networking channel)
        {
            connectedClients.Remove(channel);
        }

        private void messageArrived(Networking channel, string text)
        {
            foreach (Networking client in  connectedClients)
            {
                client.Send(text);
            }
        }
    }
}