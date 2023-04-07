using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Communications
{
    /// <summary>
    /// This class represents an object used by the client and server GUIs to connect a client to a server,
    /// create a server that listens for connections and accepts them, allow clients to wait for messages, allow
    /// clients to send messages, allow a client to be disconnected, and allow a server to stop allowing clients
    /// to connect.
    /// Written By: Nathaniel Taylor and Hunter Keating
    /// Debugged By: Hunter Keating and Nathaniel Taylor
    /// </summary>
    public class Networking
    {
        //Delegates
        public delegate void ReportMessageArrived(Networking channel, string message);
        public delegate void ReportDisconnect(Networking channel);
        public delegate void ReportConnectionEstablished(Networking channel);

        //Fields
        private ReportMessageArrived onMessage;
        private ReportDisconnect reportDisconnect;
        private ReportConnectionEstablished onConnect;
        private CancellationTokenSource cancelSource;
        private char terminationChar;
        private TcpClient client;
        private ILogger logger;
        private byte[] buffer;

        /// <summary>
        /// Gets or sets the name of the client
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Creates and sets up a new Networking object
        /// </summary>
        /// <param name="logger">a logger object provided via dependency injection</param>
        /// <param name="onConnect">A callback method to be called when a client successfully connects</param>
        /// <param name="reportDisconnect">A callback method to be called when a client is disconnected</param>
        /// <param name="onMessage">A callback method to be called when a client recieves a message</param>
        /// <param name="terminationCharacter">The character that tells the program when to terminate an action</param>
        public Networking(ILogger logger, ReportConnectionEstablished onConnect, ReportDisconnect reportDisconnect,
                          ReportMessageArrived onMessage, char terminationCharacter)
        {
            ID                    = "";
            cancelSource           = new CancellationTokenSource();
            terminationChar       = terminationCharacter;
            this.logger           = logger;
            this.onMessage        = onMessage;
            this.onConnect        = onConnect;
            this.reportDisconnect = reportDisconnect;
            client                = new TcpClient();
            buffer = new byte[1];
        }

        /// <summary>
        /// Creates and sets up a new Networking object with an already connected client
        /// </summary>
        /// <param name="logger">a logger object provided via dependency injection</param>
        /// <param name="client">A previously connected client</param>
        /// <param name="onConnect">A callback method to be called when a client successfully connects</param>
        /// <param name="reportDisconnect">A callback method to be called when a client is disconnected</param>
        /// <param name="onMessage">A callback method to be called when a client recieves a message</param>
        /// <param name="terminationCharacter">The character that tells the program when to terminate an action</param>
        private Networking(ILogger logger, TcpClient client, ReportConnectionEstablished onConnect, ReportDisconnect
                           reportDisconnect, ReportMessageArrived onMessage, char terminationCharacter)
        {
            ID = "";
            cancelSource = new CancellationTokenSource();
            terminationChar = terminationCharacter;
            this.logger = logger;
            this.onMessage = onMessage;
            this.onConnect = onConnect;
            this.reportDisconnect = reportDisconnect;
            this.client = client;
            buffer = new byte[1];
        }

        /// <summary>
        /// Connects a client network to the server network
        /// </summary>
        /// <param name="host">the host name of the server</param>
        /// <param name="port">the port the host is connected to</param>
        /// <exception cref="Exception">Used only for debugging and logging</exception>
        public void Connect(string host, int port)
        {
            try
            {
                client = new TcpClient(host, port);
                EndPoint? remoteEndPoint = client.GetStream().Socket.RemoteEndPoint;
                if (remoteEndPoint != null && ID == "")
                    ID = remoteEndPoint.ToString();
                onConnect(this);
            }
            catch (Exception e)
            {
                string message = e.Message;
                logger.LogInformation(message);
            }
        }

        /// <summary>
        /// A method used by clients to wait for messages from other clients
        /// </summary>
        /// <param name="infinite">tells the client whether or not to continuously wait for messages</param>
        public async void AwaitMessagesAsync(bool infinite = true)
        {
            try
            {
                //Creates a stream between server and client
                NetworkStream clientStream = client.GetStream();
                if (infinite)
                {
                    while (infinite)
                    {
                        //Checks message length and throws if it is empty
                        int total = await clientStream.ReadAsync(buffer);
                        if (total == 0)
                            throw new Exception("You have been disconnected.");
                        onMessage(this, System.Text.Encoding.Default.GetString(buffer));
                        //Clears the buffer, reducing the amount of repeat messages
                        Array.Clear(buffer, 0, buffer.Length);
                    }
                }
            }
            catch
            {
                //Ends if the user disconnects
                reportDisconnect(this);
            }
        }

        /// <summary>
        /// A method used by servers to wait for client networks to connect to them
        /// </summary>
        /// <param name="port">The port the server is connected and listening to</param>
        /// <param name="infinte">Tells the client whether or not to continuously wait for clients</param>
        public async void WaitForClients(int port, bool infinte)
        {
            //Listener that takes in any TCP client
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            try
            {
                while (infinte)
                {
                    //Creates clients and connection to them, as well as establishing their ID
                    TcpClient connection = await listener.AcceptTcpClientAsync(cancelSource.Token);
                    Networking newConnection = new Networking(logger, connection, onConnect, reportDisconnect,
                                                              onMessage, terminationChar);
                    newConnection.ID = connection.GetStream().Socket.RemoteEndPoint.ToString();
                    //Methods for connecting
                    newConnection.AwaitMessagesAsync();
                    onConnect(newConnection);
                }
            }
            catch
            {
                //Stop listening in case infinite is false
                listener.Stop();
            }
        }

        /// <summary>
        /// Tells the server to stop allowing clients to connect
        /// </summary>
        public void StopWaitingForClients()
        {
            cancelSource.Cancel();
        }

        /// <summary>
        /// Allows clients to disconnect from the server
        /// </summary>
        public void Disconnect()
        {
            reportDisconnect(this);
            client.Close();
        }

        /// <summary>
        /// Sends a message to a client
        /// </summary>
        /// <param name="text">The message to be sent</param>
        public async void Send(string text)
        {
            //Sends message
            if (!text.Contains(terminationChar))
            {
                text += terminationChar;
                //Changes string to a buffer that the server can read
                buffer = Encoding.UTF8.GetBytes(text);
                NetworkStream clientStream = client.GetStream();
                await clientStream.WriteAsync(buffer);
                //Logger stores success
                logger.LogInformation("A message has successfully been sent.");
            }
        }
    }
}