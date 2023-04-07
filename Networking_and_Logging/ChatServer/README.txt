**Author:** Nathaniel Taylor
**Partner:** Hunter Keating
**Course:** CS 3500, University of Utah, School of Computing
**GitHub IDs:** NSwimer1321 and hkeating27
**Repo:** https://github.com/uofu-cs3500-spring23/assignment-seven---chatting-bluemangroup
**Date:** 04/06/2023 Time: 11:00 pm
**Project Name:** ChatServer
**Copyright:** CS 3500 and Nathaniel Taylor - This work may not be copied for use in Academic Coursework


**Comments to Evaluators:**
None of the command functions work. We are aware that fragments of messages sometimes add on quasi-randomly, and that this is due to the buffer not fully clearing. 
We halved the amount of extra messages using Array.Clear() in AwaitMessageAsync() in Networking, but some still remain.


**Assignment Specific Topics:**
I have mostly the same comments as I did in the ChatClient README, with the exception that once the ChatClient
GUI got working, the ChatServer GUI was working pretty soon after.


**Consulted Peers:**
We didn't specifically ask anyone for help on this project. We did, however, look on Piazza and ask
if we had a question and asked a question ourself.


**References:**
https://stackoverflow.com/questions/3360555/how-to-pass-parameters-to-threadstart-method-in-thread#:~:text=The%20simplest%20is%20just%20string%20filename%20%3D...%20Thread,needing%20to%20cast%20from%20object%20all%20the%20time.