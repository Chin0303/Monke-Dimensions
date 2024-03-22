﻿#if EDITOR

#else
namespace Monke_Dimensions.Models;

public class DimensionPackage
{
    public string Name { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
    public string SpawnPoint { get; set; }
    public string TerminalPoint { get; set; }

    public DimensionPackage(string name, string author, string description, string spawnPoint)
    {
        Name = name;
        Author = author;
        Description = description;
        SpawnPoint = spawnPoint;
    }
}
#endif