using Communications;
using FileLogger;

namespace ChatClient;

public partial class MainPage : ContentPage
{
	//Field
	private Networking client;
	private string serverName;
	private string text;

	public MainPage()
	{
		InitializeComponent();
		client     = new Networking(new CustomFileLogger(), connectionComplete, connectionDropped, messageArrived, 
									'\n');
		serverName = "";
		text       = "";
	}

	private void ConnectToServer(object sender, EventArgs e)
	{
		client.Connect(serverName, 11000);
	}

	private void connectionComplete(Networking client)
	{

	}

	private void connectionDropped(Networking client)
	{

	}

	private void messageArrived(Networking client, string text)
	{

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

