using Gopher.Cli.Facade;

namespace Gopher.Cli.Display
{
    public class Document
    {
        private readonly int width;
        private readonly int height;
        private readonly string response;

        public Document(string response, int consoleWidth, int consoleHeight)
        {
            this.width = consoleWidth;
            this.height = consoleHeight;
            this.response = response;
        }

        public void Draw(IConsole console)
        {
            if (string.IsNullOrWhiteSpace(response))
            {
                return;
            }

            var lines = response.Split("\r\n");
            var max = lines.Length > height ? height : lines.Length;
            
            for (int l = 0; l < max; l++)
            {
                string line = lines[l];
                if (line.Length > width)
                {
                    console.WriteLine(line.Substring(0, width));
                }
                else
                {
                    var padding = new string(' ', width - line.Length);
                    console.WriteLine(line + padding);
                }
            }
        }
    }
}
