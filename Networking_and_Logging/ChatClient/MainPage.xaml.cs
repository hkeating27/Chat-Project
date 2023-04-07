using Communications;
using FileLogger;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Internals;
using System.Net;
using System.Net.Sockets;
using System.Threading.Channels;

namespace ChatClient {

	/// <summary>
	/// This class represents the logic behind the GUI of a client
	/// Written By: Nathaniel Taylor
	/// Debugged By: Hunter Keating and Nathaniel Taylor
	/// </summary>
	public partial class MainPage : ContentPage
	{
		//Field
		private Networking network;
		private string serverName;
		private string text;
		private ILogger<MainPage> logger;

		/// <summary>
		/// Initializes the GUI, creates a new Networking object, and initializes the
		/// serverName and text fields.
		/// </summary>
		public MainPage(ILogger<MainPage> logger)
		{
			InitializeComponent();
			network = new Networking(logger, connectionComplete, connectionDropped, messageArrived, '\n');
			serverAdd.Text = "localhost";
			serverName = Dns.GetHostName();
			text = "";
			this.logger = logger;
		}

		/// <summary>
		/// Connects the client to the specified server
		/// </summary>
		/// <param name="sender">The sender of the event that triggers this method</param>
		/// <param name="e">The Event Arguments of the event that triggers this method</param>
		private void ConnectToServer(object sender, EventArgs e)
		{
			Label beginConnection = new Label();
			beginConnection.Text = "Connecting...";
			sentMessages.Add(beginConnection);
			network.Connect(serverName, 11000);

			logger.LogInformation("A connection has successfully begun.");
        }

		/// <summary>
		/// The callback method that the Networking object calls whenever this client connects to the server
		/// </summary>
		/// <param name="client">The client that connected to the server</param>
		private void connectionComplete(Networking client)
		{
            Label connected = new Label();
			connected.Text = "Connection successful.";
            sentMessages.Add(connected);

            Thread messageThread = new Thread(() => client.AwaitMessagesAsync(infinite: true));
            messageThread.Start();

			logger.LogInformation("A connection has been successfully established.");
        }

		/// <summary>
		/// The callback method that the Networking object calls whenever the specified client
		/// gets disconnected from the server
		/// </summary>
		/// <param name="client">The specified client</param>
		private void connectionDropped(Networking client)
		{
			Label disconnectedLabel = new Label();
			disconnectedLabel.Text = "You have been disconnected from the server.";
            Application.Current.Dispatcher.Dispatch((Action)(() => sentMessages.Add(disconnectedLabel)));

			logger.LogInformation("A connection has dropped. This could be because the server was shutdown" +
								  " or an error has occured.");
		}

		/// <summary>
		/// The callback method that the Networking object calls whenever the specified client gets
		/// sent a message.
		/// </summary>
		/// <param name="client">The specified client</param>
		/// <param name="text">The sent message</param>
		private void messageArrived(Networking client, string text)
		{
			network = client;
			Label messageLabel = new Label();
			messageLabel.Text = text;

			
			if (text.Contains("Command Participants,"))
			{

			}
			Application.Current.Dispatcher.Dispatch((Action)(() => sentMessages.Add(messageLabel)));

			logger.LogInformation("A message has successfully arrived.");
		}

        /// <summary>
        /// The method called whenever the user changes the name of the server they want to connect to
        /// </summary>
        /// <param name="sender">The sender of the event that triggers this method</param>
        /// <param name="e">The Event Arguments of the event that triggers this method</param>
        private void ServerAddressChanged(object sender, EventArgs e)
		{
			serverName = (sender as Entry).Text;

			logger.LogInformation("The server name has successfully been changed.");
		}

        /// <summary>
        /// The method called whenever the user changes the message they want to send
        /// </summary>
        /// <param name="sender">The sender of the event that triggers this method</param>
        /// <param name="e">The Event Arguments of the event that triggers this method</param>
        private void MessageCompleted(object sender, EventArgs e)
		{
			text = (sender as Entry).Text;
			network.Send(text);

			logger.LogInformation("A message has successfully been written.");
		}

		/// <summary>
		/// Changes the ID property of the client's Networking object
		/// </summary>
		/// <param name="sender">The entry sending the event that triggers this method</param>
		/// <param name="e">The Event Arguments of the event that triggers this method</param>
		private void ChangeID(object sender, EventArgs e)
		{
			network.ID = (sender as Entry).Text;

			logger.LogInformation("The name of the client has successfully been changed.");
		}
	}
}

