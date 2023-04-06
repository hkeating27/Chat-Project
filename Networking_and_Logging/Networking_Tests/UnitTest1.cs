using Communications;
using Microsoft.Extensions.Logging.Abstractions;
using System.Net;

namespace Networking_Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Networking server = new Networking(NullLogger.Instance, newClient, s => { ; }, messageRecieved, '\n' );
            server.WaitForClients(11000, true);

            Networking client = new Networking(NullLogger.Instance, clientConnect, s => { ; }, (a, s) => { ; }, '\n');
            client.Connect(Dns.GetHostName(), 11000);
        }

        public void newClient(Networking client)
        {
            client.AwaitMessagesAsync();
        }

        public void messageRecieved(Networking server, string message)
        {
            Assert.AreEqual("message", message);
        }

        public void clientConnect(Networking network)
        {
            network.Send("message");
        }
    }
}