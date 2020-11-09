using System;

namespace GopherLib
{
    // The server response is described in RFC 1436 section 2
    public class Response
    {
        public Response(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return;
            }

            // First character of any response is the document type
            if (Enum.IsDefined(typeof(ResponseType), (int)data[0]))
            {
                Type = (ResponseType)data[0];
            }

            // Respones are tab delimited and must contain at least 4 sections
            var split = data[1..^0].Split('\t');
            if(split.Length < 4)
            {
                Type = ResponseType.Unknown;
                return;
            }

            Display = split[0];
            Selector = split[1];
            Domain = split[2];
            
            // The port must be a valid number
            if (int.TryParse(split[3], out var port))
            {
                Port = port;
            }
        }

        public ResponseType Type { get; private set; } = ResponseType.Unknown;
        public string Display { get; private set; } = string.Empty;
        public string Selector { get; private set; } = string.Empty;
        public string Domain { get; private set; } = string.Empty;
        public int Port { get; private set; } = 70; //Default Gopher port
    }
}
