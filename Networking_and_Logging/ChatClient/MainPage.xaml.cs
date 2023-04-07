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
		private List<string> participants;
		private string clientName;

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
			clientName = "";
			this.logger = logger;
			participants = new List<string>();
		}

		/// <summary>
		/// Connects the client to the specified server
		/// </summary>
		/// <param name="sender">The sender of the event that triggers this method</param>
		/// <param name="e">The Event Arguments of the event that triggers this method</param>
		private void ConnectToServer(object sender, EventArgs e)
		{
			//Initializes connection with GUI interaction
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
			//Lets the user know the connection was successful
            Label connected = new Label();
			connected.Text = "Connection successful.";
            sentMessages.Add(connected);

			//Sets the client ID if given
			if (clientName != "")
				client.ID = clientName;

			//New thread between client and server
            Thread messageThread = new Thread(() => client.AwaitMessagesAsync(infinite: true));
            messageThread.Start();

			//Logs success
			logger.LogInformation("A connection has been successfully established.");
        }

		/// <summary>
		/// The callback method that the Networking object calls whenever the specified client
		/// gets disconnected from the server
		/// </summary>
		/// <param name="client">The specified client</param>
		private void connectionDropped(Networking client)
		{
			//Lets user know they have been disconnected
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
			//GUI interface showing the message
			Label messageLabel = new Label();
			messageLabel.Text = text;

			//Gets the name of every participant
			if (text.Contains("Command Participants,"))
			{
				string nameChange = "";
				for (int i = 0; i <= text.Length; i++)
				{
					if (text[i] == '[' && text[i + 1] != ']')
						nameChange += text[i + 1];
					else
						nameChange += text[i];

					if (text[i] == ']')
					{
						participants.Add(nameChange);
					}
				}
			}
			
			else if (text.StartsWith("This client is in the server:"))
			{
				participants.Add(text.Substring(28));
			}
			//Otherwise send a normal message
			else
			{
				messageLabel.Text = client.ID + messageLabel.Text;
				Application.Current.Dispatcher.Dispatch((Action)(() => sentMessages.Add(messageLabel)));

				logger.LogInformation("A message has successfully arrived.");
			}
		}

        /// <summary>
        /// The method called whenever the user changes the name of the server they want to connect to
        /// </summary>
        /// <param name="sender">The sender of the event that triggers this method</param>
        /// <param name="e">The Event Arguments of the event that triggers this method</param>
        private void ServerAddressChanged(object sender, EventArgs e)
		{
			//Changes username
			serverName = (sender as Entry).Text;
			//Logs success
			logger.LogInformation("The server name has successfully been changed.");
		}

        /// <summary>
        /// The method called whenever the user changes the message they want to send
        /// </summary>
        /// <param name="sender">The sender of the event that triggers this method</param>
        /// <param name="e">The Event Arguments of the event that triggers this method</param>
        private void MessageCompleted(object sender, EventArgs e)
		{
			//Sets the text and sends it
			text = (sender as Entry).Text;
			network.Send(text);
			//Logs success
			logger.LogInformation("A message has successfully been written.");
		}

		/// <summary>
		/// Changes the ID property of the client's Networking object
		/// </summary>
		/// <param name="sender">The entry sending the event that triggers this method</param>
		/// <param name="e">The Event Arguments of the event that triggers this method</param>
		private void ChangeID(object sender, EventArgs e)
		{
			//Changes the client ID
			clientName = (sender as Entry).Text;
			//Logs success
			logger.LogInformation("The name of the client has successfully been changed.");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender">The button sending the event that triggers this method</param>
		/// <param name="e">The Event Arguments of the event that triggers this method</param>
		private void retrieveClients(object sender, EventArgs e)
		{
			//Retrieves list of all clients
			foreach (string name in participants)
			{
				Label participantLabel = new Label();
				participantLabel.Text = name;
				listOfParticipants.Add(participantLabel);
			}
		}
	}
}

