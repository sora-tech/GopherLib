using System;
using System.Collections.Generic;
using System.Linq;

namespace GobpherLib
{
    public class Client
    {
        public Client(Uri domain)
        {
            if(domain.Scheme != "gopher")
            {
                throw new Exception("Invalid scheme");
            }

            Domain = domain;
        }

        public Uri Domain { get; private set; }

        public List<Response> List(IConnection connection, string path)
        {
            var opened = connection.Open(Domain.Host, Domain.Port);

            if(opened == false)
            {
                return new List<Response>();
            }

            var response = connection.Request(path);

            if(string.IsNullOrWhiteSpace(response))
            {
                return new List<Response>();
            }

            var data = response.TrimEnd('\0').Split("\r\n");

            var result = data.Where(d => string.IsNullOrWhiteSpace(d) == false && d != ".")
                                .Select(d => new Response(d));

            return result.ToList();
        }

        public string TextFile(IConnection connection, string selector)
        {
            //Must request a document?
            if (string.IsNullOrWhiteSpace(selector))
            {
                return string.Empty;
            }

            var opened = connection.Open(Domain.Host, Domain.Port);

            if (opened == false)
            {
                return string.Empty;
            }

            var response = connection.Request(selector);

            //Strip trailing null from buffers and CRLF + . termination
            response = response.TrimEnd('\0').Trim().TrimEnd('.');

            // Replace CRLF + .. that is added not to early terminate the document
            response = response.Replace("\r\n..", "\r\n.");

            return response;
        }
    }
}
