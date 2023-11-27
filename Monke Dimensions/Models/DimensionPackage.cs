namespace Monke_Dimensions.Models
{
    internal class DimensionPackage
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }

        public string SpawnPoint { get; set; }
        public string TerminalPoint { get; set; }

        public DimensionPackage(string name, string author, string description, string spawnPoint, string terminalPoint)
        {
            Name = name;
            Author = author;
            Description = description;
            SpawnPoint = spawnPoint;
            TerminalPoint = terminalPoint;
        }
    }
}