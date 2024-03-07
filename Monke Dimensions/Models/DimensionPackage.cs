#if EDITOR

#else
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Monke_Dimensions.Models;

internal class DimensionPackage
{
    public string Name { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
    public string SpawnPoint { get; set; }
    public string TerminalPoint { get; set; }

    [JsonProperty("Addons")]
    public AddonsList Addons { get; set; } = new AddonsList();

    public DimensionPackage(string name, string author, string description, string spawnPoint, string terminalPoint)
    {
        Name = name;
        Author = author;
        Description = description;
        SpawnPoint = spawnPoint;
        TerminalPoint = terminalPoint;

        Addons = new AddonsList
        {
            SlipperyObjects = new List<string>(),
            ExtraTerminals = new List<string>(),
        };
    }

    public class AddonsList
    {
        public List<string> SlipperyObjects { get; set; }
        public List<string> ExtraTerminals { get; set; }
    }
}
#endif