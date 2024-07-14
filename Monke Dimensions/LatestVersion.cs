using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace Monke_Dimensions;

public class LatestVersion
{
    public static bool NeedToUpdate()
    {
        string localVersion = GetLocalVersion();
        string githubVersion = GetGithubVersion();

        return localVersion != githubVersion;
    }

    private static string GetLocalVersion()
    {
        using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Monke_Dimensions.VERSION"))
        using (StreamReader reader = new StreamReader(manifestResourceStream))
        {
            return reader.ReadLine();
        }
    }

    private static string GetGithubVersion()
    {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = client.GetAsync("https://raw.githubusercontent.com/Chin0303/Monke-Dimensions/master/VERSION").Result;
            response.EnsureSuccessStatusCode();

            using (Stream stream = response.Content.ReadAsStreamAsync().Result)
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadLine();
            }
        }
    }
}