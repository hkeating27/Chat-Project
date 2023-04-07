using Communications;
using Microsoft.Extensions.Logging.Abstractions;
using System.Net;

namespace Networking_Tests
{
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// See test name
        /// </summary>
        [TestMethod]
        public void simpleClientServerTest()
        {
            Networking server = new Networking(NullLogger.Instance, newClient, s => { ; }, messageRecieved, '\n' );
            server.WaitForClients(11000, true);

            Networking clients = new Networking(NullLogger.Instance, clientConnect, s => { ; }, (a, s) => { ; }, '\n');
            clients.Connect(Dns.GetHostName(), 11000);
        }

        /// <summary>
        /// The callback method for the simpleClientServerTest's server network onConnect parameter
        /// </summary>
        /// <param name="client">the client made by the server after a connection</param>
        public void newClient(Networking client)
        {
            client.AwaitMessagesAsync();
        }

        /// <summary>
        /// The callback method for the simpleClientServerTest's server network onMessage parameter
        /// </summary>
        /// <param name="server">Called whenever the server recieves a message</param>
        /// <param name="message">The message recieved</param>
        public void messageRecieved(Networking server, string message)
        {
            Assert.AreEqual("message", message);
        }

        /// <summary>
        /// The callback method for the simpleClientServerTest's client network onConnect parameter
        /// </summary>
        /// <param name="network">The client who has connected</param>
        public void clientConnect(Networking network)
        {
            network.Send("message");
        }

        /// <summary>
        /// See test name
        /// </summary>
        [TestMethod]
        public void multipleClientConnectToServerTest()
        {
            Networking server = new Networking(NullLogger.Instance, multipleNewClients, s => { ; },
                                                                            multipleMessagesRecieved, '\n');
            server.WaitForClients(11000, true);

            Networking[] clients = new Networking[15];
            for(int i = 0; i < clients.Length; i++)
            {
                Networking client = new Networking(NullLogger.Instance, multipleClientConnect, s => { ; }, 
                    (a, s) => { ; }, '\n');
                clients[i] = client;
                client.Connect(Dns.GetHostName(), 11000);
            }
        }

        /// <summary>
        /// The callback method for the multipleClientConnectToServerTest's client networks onConnect parameter
        /// </summary>
        /// <param name="network">The client network who has connected</param>
        public void multipleClientConnect(Networking network)
        {
            network.Send("message");
        }

        /// <summary>
        /// The callback method for the multipleClientConnectToServerTest's server network onConnect parameter
        /// </summary>
        /// <param name="client"></param>
        public void multipleNewClients(Networking client)
        {
            client.AwaitMessagesAsync();
        }

        /// <summary>
        /// The callback method for the multipleClientConnectToServerTest's server network onMessage parameter
        /// </summary>
        /// <param name="client">The client who sent the message</param>
        /// <param name="text">The message sent</param>
        public void multipleMessagesRecieved(Networking client, string text)
        {
            List<string> messages = new List<string>();
            messages.Add(text);

            if (messages.Count == 25)
            {
                for (int i = 0; i < messages.Count; i++)
                {
                    Assert.AreEqual("message", text);
                }
            }
        }
    }
}