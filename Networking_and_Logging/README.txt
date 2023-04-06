**Author:** Nathaniel Taylor
**Partner:** Hunter Keating
**Date Created:** 03/24/2023
**Course:** CS 3500, University of Utah, School of Computing
**GitHub IDs:** NSwimer1321 and hkeating27
**Repo:** https://github.com/uofu-cs3500-spring23/assignment-seven---chatting-bluemangroup
**Date of Last Submission:** 04/06/2023 Time: 11:00 pm
**Solution:** Networking_and_Logging
**Copyright:** CS 3500, Nathaniel Taylor, and Hunter Keating - This work may not be copied for use in 
Academic Coursework

**Overview of Networking_and_Logging Functionality:**
This solution can create two GUIs, one GUI, the client GUI, displays functionality that allows the user to input the 
name of the server they would like to connect to as well as the name the would like to identify as in the server. 
Once connected they can send messages to all other clients in the server. The client GUI also displays a border and a
button. If the user clicks the button, then the names of all clients connected to the server will be displayed. 
The other GUI, the server GUI, can display a border that will display the names of all connected clients. The GUI
also has a button that when clicked will shut down the server, disconnecting all clients connected to the server.
The GUI also has two entries that hold the server name and the server IP address. The client GUI server name and the
server GUI server name and server IP address are automatically configured to work with a local host. All of the
underlying functionality for how the two GUIs work are in the Networking class.


**Partnership:**
GUI- Nathaniel
Connect- Hunter
WaitForClients- Nathaniel
WaitForMessages- Nathaniel and Hunter
Messaging- Hunter and Nathaniel
Debugging- Nathaniel and Hunter
Miscellanious Methods- Nathaniel and Hunter


**Design Choices:**
We decided that our default log level would be the Information level because it helps to track the general-flow
of the application and it is good for long-term use, both features we thought would be good for the default logging
level since it provides enough information so we know when something is going wrong or right, but it also doesn't
overload us with information that we can always get just by temporarily throwing an exception and looking at the
exception message.
We decided for all of our logging messages to be Information level messages as well for many of the same reasons as
discussed above. We didn't think that we needed a ton of information from our log messages, we just needed to know
whether or not something was working. We though that if we needed indepth information on why something wasn't
working then throwing an exception was better because we could just look up the error and StackOverflow 
solutions.


**Branching:**
Most of our programming was during pair programming or when we were sure we were the only ones working on it, so
we did not create any seperate branches.


**Testing:**
We created two tests. The first test was a simple test case where we created a server and a client. We then connected
the client to the server, had the client await messages, and had the client send a message. This test made sure that
almost everything Networking does (connecting, waiting for clients, waiting for messages, and sending messages) works
as intended. That is particularly helpful with figuring out whether a bug is in the GUI or Networking.
The second test we created was very similar to the first one. The only real difference was that we created 25 clients
to connect to the server. We had each client send a message. We then checked to make sure each message was correct.
This test tested to see whether or not Networking worked as intended with multiple clients.


**Time Tracking:**
Estimated- 16
Actual- 24
Time spent adding code- 8
Time spent debugging- 16

There were so many new things introduced into this assignment that it makes sense that more time would be spent
debugging rather than coding, but even then I feel like we spent so much more time debugging than we should have.
We looked up several errors we had never encountered before and the fixes to those errors, but even then sometimes
the recommended fixes wouldn't help the error, so we would have to spend hours debugging and trying to fix the error
ourselves.
