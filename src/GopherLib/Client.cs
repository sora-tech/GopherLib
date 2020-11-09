using System;
using System.Collections.Generic;
using System.Linq;

namespace GopherLib
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

        private const string terminator = "\t\r\n\0";

        public Uri Domain { get; private set; }

        public List<Response> Menu(IConnection connection, string selector)
        {
            var opened = connection.Open(Domain.Host, Domain.Port);

            if(opened == false)
            {
                return new List<Response>();
            }

            var response = connection.Request(selector + terminator);

            return ParseResponse(response);
        }

        public Span<byte> Binary(IConnection connection, string selector)
        {
            if (string.IsNullOrWhiteSpace(selector))
            {
                return new Span<byte>();
            }

            var opened = connection.Open(Domain.Host, Domain.Port);
            if (opened == false)
            {
                return new Span<byte>();
            }

            var response = connection.RequestBytes(selector + terminator);

            return response;
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

            var response = connection.Request(selector + terminator);

            //Strip trailing null from buffers and CRLF + . termination
            response = response.TrimEnd('\0').Trim().TrimEnd('.');

            // Replace CRLF + .. that is added not to early terminate the document
            response = response.Replace("\r\n..", "\r\n.");

            return response;
        }

        public List<Response> Search(IConnection connection, string selector, string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return new List<Response>();
            }

            var opened = connection.Open(Domain.Host, Domain.Port);

            if (opened == false)
            {
                return new List<Response>();
            }

            var response = connection.Request($"{selector}\t{term}");

            return ParseResponse(response);
        }

        private static List<Response> ParseResponse(string response)
        {
            if (string.IsNullOrWhiteSpace(response))
            {
                return new List<Response>();
            }

            var data = response.TrimEnd('\0').Split("\r\n");

            var result = data.Where(d => string.IsNullOrWhiteSpace(d) == false && d != ".")
                                .Select(d => new Response(d));

            return result.ToList();
        }
    }
}
