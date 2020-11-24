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

        public string Print(int width)
        {
            if (response == null)
            {
                response = new Response("");
            }

            var empty = new string(' ', width);
            string val = "";

            switch (response.Type)
            {
                case ItemType.Unknown:
                    return empty;
                case ItemType.File:
                case ItemType.Directory:
                    val = $"{(char)response.Type} {response.Display} : {response.Domain}";
                    break;
                case ItemType.PhoneBook:
                    break;
                case ItemType.Error:
                    break;
                case ItemType.BinHexed:
                    break;
                case ItemType.Binary:
                case ItemType.DOSBinary:
                    val = $"{(char)response.Type} {response.Display} : {response.Domain}";
                    break;
                case ItemType.UUEncoded:
                    break;
                case ItemType.IndexSearch:
                    val = $"{(char)response.Type} {response.Display} : {response.Domain}";
                    break;
                case ItemType.Telnet:
                    break;
                case ItemType.RedundantServer:
                    break;
                case ItemType.TN3270:
                    break;
                case ItemType.GIF:
                case ItemType.Image:
                    val = $"{(char)response.Type} {response.Display} : {response.Domain}";
                    break;
                case ItemType.Info:
                    val= $"{(char)response.Type} {response.Display}";
                    break;
                default:
                    break;
            }

            if(val.Length > width)
            {
                return val.Substring(0, width);
            }

            var padding = new string(' ', width - val.Length);

            return val + padding;
        }
    }
}