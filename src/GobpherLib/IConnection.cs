namespace GobpherLib
{
    public interface IConnection
    {
        bool Open(string domain, int port);
        string Request(string path);
        byte[] RequestBytes(string path);
    }
}
