using Communications;
using FileLogger;
using Microsoft.Extensions.Logging;
using System.Net;

namespace ChatClient
{

	public partial class MainPage : ContentPage
	{
		//Field
		private Networking network;
		private string serverName;
		private string text;

		/// <summary>
		/// Initializes the GUI, creates a new Networking object, and initializes the
		/// serverName and text fields.
		/// </summary>
		public MainPage()
		{
			InitializeComponent();
			network = new Networking(new CustomFileLogger("Information", "clientLogging"), connectionComplete,
										 connectionDropped, messageArrived, '\n');
			serverName = "localhost";
			text = "";
		}

		/// <summary>
		/// Connects the client to the specified server
		/// </summary>
		/// <param name="sender">The sender of the event that triggers this method</param>
		/// <param name="e">The Event Arguments of the event that triggers this method</param>
		private void ConnectToServer(object sender, EventArgs e)
		{
			network.Connect(Dns.GetHostName(), 11000);
		}

		/// <summary>
		/// The callback method that the Networking object calls whenever this client connects to the server
		/// </summary>
		/// <param name="client">The client that connected to the server</param>
		private void connectionComplete(Networking client)
		{
			network = client;
			Label connectedLabel = new Label();
			connectedLabel.Text = "Connection Successful. You may now send messages to the server.";
			messages.Add(connectedLabel);
			network.AwaitMessagesAsync(true);
		}

		/// <summary>
		/// The callback method that the Networking object calls whenever the specified client
		/// gets disconnected from the server
		/// </summary>
		/// <param name="client">The specified client</param>
		private void connectionDropped(Networking client)
		{
			network = client;
			Label disconnectedLabel = new Label();
			disconnectedLabel.Text = "You have been disconnected from the server.";
			messages.Add(disconnectedLabel);
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
			messages.Add(messageLabel);
		}

        /// <summary>
        /// The method called whenever the user changes the name of the server they want to connect to
        /// </summary>
        /// <param name="sender">The sender of the event that triggers this method</param>
        /// <param name="e">The Event Arguments of the event that triggers this method</param>
        private void ServerAddressChanged(object sender, EventArgs e)
		{
			serverName = (sender as Entry).Text;
		}

        /// <summary>
        /// The method called whenever the user changes the message they want to send
        /// </summary>
        /// <param name="sender">The sender of the event that triggers this method</param>
        /// <param name="e">The Event Arguments of the event that triggers this method</param>
        private void MessageCompleted(object sender, EventArgs e)
		{
			text = (sender as Entry).Text;
		}
	}
}

