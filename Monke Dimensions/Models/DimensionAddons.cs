namespace Monke_Dimensions.Models
{
    internal class DimensionAddons
    {
        public string[] SlipperyObjects { get; set; }
        public string[] WaterObjects { get; set; }

        public DimensionAddons(string[] slipperyObjects, string[] waterObjects) 
        {
            SlipperyObjects = slipperyObjects;
            WaterObjects = waterObjects;
        }
    }
}

// Addons sounds the best, idk a different class name 
