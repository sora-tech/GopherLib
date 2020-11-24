using GopherLib;

namespace Gopher.Cli
{
    public class Display
    {
        private Response response;

        public Display(Response response)
        {
            this.response = response;
        }

        public string Print()
        {
            if (response == null)
            {
                response = new Response("");
            }

            switch (response.Type)
            {
                case ItemType.Unknown:
                    return string.Empty;
                case ItemType.File:
                case ItemType.Directory:
                    return $"{(char)response.Type} {response.Display} : {response.Domain}";
                case ItemType.PhoneBook:
                    break;
                case ItemType.Error:
                    break;
                case ItemType.BinHexed:
                    break;
                case ItemType.Binary:
                case ItemType.DOSBinary:
                    return $"{(char)response.Type} {response.Display} : {response.Domain}";
                case ItemType.UUEncoded:
                    break;
                case ItemType.IndexSearch:
                    return $"{(char)response.Type} {response.Display} : {response.Domain}";
                case ItemType.Telnet:
                    break;
                case ItemType.RedundantServer:
                    break;
                case ItemType.TN3270:
                    break;
                case ItemType.GIF:
                case ItemType.Image:
                    return $"{(char)response.Type} {response.Display} : {response.Domain}";
                case ItemType.Info:
                    return $"{(char)response.Type} {response.Display}";
                default:
                    break;
            }

            return string.Empty;
        }
    }
}
