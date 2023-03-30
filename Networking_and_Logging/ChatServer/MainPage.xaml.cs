using Communications;
using FileLogger;

namespace ChatServer
{
    public partial class MainPage : ContentPage
    {
        //Field
        Networking serverNetwork;

        public MainPage()
        {
            InitializeComponent();
            serverNetwork = new Networking(new CustomFileLogger(), connectionComplete, connectionDropped,
                                           messageArrived, '\n');
            serverNetwork.WaitForClients(11000, true);
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
    }
}