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

            var data = response.Split("");

            var result = data.Where(d => string.IsNullOrWhiteSpace(d) == false)
                                .Select(d => new Response(d));

            return result.ToList();
        }
    }
}
