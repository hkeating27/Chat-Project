using Communications;
using FileLogger;
using System.Net;

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
        public MainPage()
        {
            InitializeComponent();
            serverName.Text = Dns.GetHostName();
            ipAddress.Text = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();

            serverNetwork = new Networking(new CustomFileLogger("Information", "serverLogging"), connectionComplete, connectionDropped,
                                           messageArrived, '\n');
            connectedClients = new List<Networking>();
            Thread startThread = new Thread(() => serverNetwork.WaitForClients(11000, true));
            startThread.Start();
        }

        private void connectionComplete(Networking chanel)
        {
            
        }

        private void connectionDropped(Networking chanel)
        {

        }

        private void messageArrived(Networking chanel, string text)
        {
            foreach (Networking client in  connectedClients)
            {
                client.Send(text);
            }
        }
    }
}