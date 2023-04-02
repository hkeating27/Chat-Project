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

        public MainPage()
        {
            InitializeComponent();
            serverNetwork = new Networking(new CustomFileLogger("Information", "serverLogging"), connectionComplete, connectionDropped,
                                           messageArrived, '\n');
            connectedClients = new List<Networking>();
            Thread startThread = new Thread(() => serverNetwork.WaitForClients(11000, true));
            startThread.Start();
            connectedClients.Add(serverNetwork);
            serverShutdown.Text = "Hunter";
        }

        private void connectionComplete(Networking chanel)
        {
            
        }

        private void connectionDropped(Networking chanel)
        {

        }

        private void messageArrived(Networking chanel, string text)
        {

        }

        private void createEndPoint(object sender, EventArgs e)
        {

        }
    }
}