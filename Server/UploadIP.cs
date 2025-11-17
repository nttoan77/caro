using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

class ServerIPUploader
{
    public static async Task UploadIP()
    {
        string publicIP = new WebClient().DownloadString("https://api.ipify.org");

        using (HttpClient client = new HttpClient())
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("ip", publicIP)
            });

            await client.PostAsync("https://your-server.com/save-ip", data);
        }

        Console.WriteLine("Uploaded public IP: " + publicIP);
    }
}
