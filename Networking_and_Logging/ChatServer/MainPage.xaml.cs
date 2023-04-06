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
            someoneConnected.Text = channel.ID + " has connected";
            Application.Current.Dispatcher.Dispatch((Action)(() => allSentMessages.Add(someoneConnected)));

            Thread messageThread = new Thread(() => channel.AwaitMessagesAsync(infinite: true));
            messageThread.Start();
        }

        private void connectionDropped(Networking channel)
        {
            connectedClients.Remove(channel);
            Label someoneDisconnected = new Label();
            someoneDisconnected.Text = "Someone has disconnected";
            allSentMessages.Add(someoneDisconnected);
        }

        private void messageArrived(Networking channel, string text)
        {
            foreach (Networking client in  connectedClients)
            {
                client.Send(text);
            }

            Label someoneSentMsg = new Label();
            someoneSentMsg.Text = "Someone has sent a message";
            allSentMessages.Add(someoneSentMsg);
        }

        private void ServerShutdown(object sender, EventArgs e)
        {
            foreach (Networking client in connectedClients)
                client.Disconnect();
            connectedClients.Clear();
        }
    }
}