using Communications;
using FileLogger;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace ChatServer
{
    /// <summary>
    /// This class represents the logic behind the GUI of a server
    /// Written By: Nathaniel Taylor
    /// Debugged By: Hunter Keating and Nathaniel Taylor
    /// </summary>
    public partial class MainPage : ContentPage
    {
        //Field
        Networking serverNetwork;
        List<Networking> connectedClients;
        private ILogger<MainPage> logger;

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
            this.logger = logger;

            Thread startThread = new Thread(() => serverNetwork.WaitForClients(11000, true));
            startThread.Start();
        }

        /// <summary>
        /// The callback method that the Networking object calls whenever a client has successfully connected to 
        /// the server.
        /// </summary>
        /// <param name="channel">The client that has connected to the server</param>
        private void connectionComplete(Networking channel)
        {
            connectedClients.Add(channel);
            Label someoneConnected = new Label();
            someoneConnected.Text = channel.ID + " has connected";
            Application.Current.Dispatcher.Dispatch((Action)(() => allSentMessages.Add(someoneConnected)));

            logger.LogInformation("A connection to the server has successfully been made.");
        }

        /// <summary>
        /// The callback method that the Networking object calls whenever a client has disconnected from the server.
        /// </summary>
        /// <param name="channel">The client that has disconnected from the server</param>
        private void connectionDropped(Networking channel)
        {
            connectedClients.Remove(channel);
            Label someoneDisconnected = new Label();
            someoneDisconnected.Text = "Someone has disconnected";
            Application.Current.Dispatcher.Dispatch((Action)(() => allSentMessages.Add(someoneDisconnected)));

            logger.LogInformation("A client has been disconnected from the server.");
        }

        /// <summary>
        /// The callback method that the Networking object calls whenever a message has successfully arrived
        /// </summary>
        /// <param name="channel">The client that sent the message</param>
        /// <param name="text">The message that was sent</param>
        private void messageArrived(Networking channel, string text)
        {
            if (text.Contains("Command Name ["))
            {
                text = text.Substring(13);
                string message = "";
                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i] != ']' || text[i] != ']')
                        message += text[i];
                }
                channel.ID = message;
            }
            else if (text == "Command Participants")
                commandParticipants(channel);
            else
            {
                foreach (Networking client in connectedClients)
                {
                    client.Send(text);
                }

                Label someoneSentMsg = new Label();
                someoneSentMsg.Text = "Someone has sent a message";
                Application.Current.Dispatcher.Dispatch((Action)(() => allSentMessages.Add(someoneSentMsg)));

                logger.LogInformation("A message:" + text + " has successfully been sent to all clients.");
            }
        }

        /// <summary>
        /// The method called whenever the user clicks on the "Server Shutdown" button.
        /// </summary>
        /// <param name="sender">The sender of the event that called this method</param>
        /// <param name="e">The Event Arguments of the event that called this method</param>
        private void ServerShutdown(object sender, EventArgs e)
        {
            foreach (Networking client in connectedClients)
                client.Disconnect();
            connectedClients.Clear();

            logger.LogInformation("The server has successfully been shutdown.");
        }

        /// <summary>
        /// Sends a list of participants back to the requesting client
        /// </summary>
        private void commandParticipants(Networking client)
        {
            foreach(Networking channel in connectedClients)
                client.Send(channel.ID);
        }
    }
}