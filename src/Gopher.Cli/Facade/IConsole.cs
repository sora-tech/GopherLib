namespace Gopher.Cli.Facade
{
    public interface IConsole
    {
        void SetHilight();
        void SetNormal();
        void WriteLine(string text);

        void Reset();
    }
}
