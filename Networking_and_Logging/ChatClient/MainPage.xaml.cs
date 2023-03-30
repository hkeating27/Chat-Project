using Communications;
using FileLogger;

namespace ChatClient;

public partial class MainPage : ContentPage
{
	//Field
	private Networking network;
	private string serverName;
	private string text;

	public MainPage()
	{
		InitializeComponent();
		network     = new Networking(new CustomFileLogger(), connectionComplete, connectionDropped, messageArrived, 
									 '\n');
		serverName = "";
		text       = "";
	}

	private void ConnectToServer(object sender, EventArgs e)
	{
		network.Connect(serverName, 11000);
	}

	private void connectionComplete(Networking client)
	{
		Label connectedLabel = new Label();
		connectedLabel.Text  = "Connection Successful. You may now send messages.";
		messages.Add(connectedLabel);
	}

	private void connectionDropped(Networking client)
	{
		Label disconnectedLabel = new Label();
		disconnectedLabel.Text  = "You have been disconnected from the server.";
		messages.Add(disconnectedLabel);
	}

	private void messageArrived(Networking client, string text)
	{
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

