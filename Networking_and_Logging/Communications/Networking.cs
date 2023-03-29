namespace Communications
{
    public class Networking
    {
        delegate void ReportMessageArrived(Networking channel, string message);
        delegate void ReportDisconnect(Networking channel);
        delegate void ReportConnectionEstablished(Networking channel);
    }
}