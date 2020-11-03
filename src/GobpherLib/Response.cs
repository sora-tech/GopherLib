using System;

namespace GobpherLib
{
    public class Response
    {
        public Response(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return;
            }


            if (Enum.IsDefined(typeof(ResponseType), (int)data[0]))
            {
                Type = (ResponseType)data[0];
            }
        }

        public ResponseType Type { get; private set; } = ResponseType.Unknown;
        public string Display { get; private set; } = string.Empty;
        public string Selector { get; private set; } = string.Empty;
        public string Domain { get; private set; } = string.Empty;
        public int Port { get; private set; } = 70; //Default Gopher port
    }
}
