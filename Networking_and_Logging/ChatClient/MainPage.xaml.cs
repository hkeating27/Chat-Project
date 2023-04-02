using Communications;
using FileLogger;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;

namespace ChatClient {

	public partial class MainPage : ContentPage
	{
		//Field
		private Networking network;
		private string serverName;
		private string text;
		TcpClient client;

        public MainPage()
		{
			InitializeComponent();
			network = new Networking(new CustomFileLogger("Information", "clientLogging"), client, connectionComplete, connectionDropped, messageArrived,
										 '\n');

			serverName = "localhost";
			text = "";
		}

		private void ConnectToServer(object sender, EventArgs e)
		{
			network.Connect(Dns.GetHostName(), 11000);
		}

		private void connectionComplete(Networking client)
		{
			network = client;
			Label connectedLabel = new Label();
			connectedLabel.Text = "Connection Successful. You may now send messages to the server.";
			messages.Add(connectedLabel);
			network.AwaitMessagesAsync(true);
		}

		private void connectionDropped(Networking client)
		{
			network = client;
			Label disconnectedLabel = new Label();
			disconnectedLabel.Text = "You have been disconnected from the server.";
			messages.Add(disconnectedLabel);
		}

		private void messageArrived(Networking client, string text)
		{
			network = client;
			Label messageLabel = new Label();
			messageLabel.Text = text;
			messages.Add(messageLabel);
		}

		private void ServerAddressChanged(object sender, EventArgs e)
		{
			serverName = (sender as Entry).Text;
		}

		private void MessageCompleted(object sender, EventArgs e)
		{
			text = (sender as Entry).Text;
		}
	}
}

