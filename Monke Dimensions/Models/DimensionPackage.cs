namespace Monke_Dimensions.Models
{
    internal class DimensionPackage
    {
        public string Name { get; set; }
        public string Author { get; set; }

        public DimensionPackage(string name, string author)
        {
            Name = name;
            Author = author;
        }
    }
}
