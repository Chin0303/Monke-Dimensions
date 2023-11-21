using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Monke_Dimensions.Models;
using UnityEngine;

namespace Monke_Dimensions
{
    internal class DimensionController
    {
        // https://stackoverflow.com/questions/20520238

        public static async Task LoadDimensions()
        {
            Debug.Log("-> Loaded Dimension(s): <-");

            string path = Path.Combine(Path.GetDirectoryName(typeof(DimensionController).Assembly.Location), "Dimensions");
            var dimensionFiles = Directory.GetFiles(path, "*.dimension");

            foreach (string dimensionFile in dimensionFiles)
            {
                string currentPath = Path.GetFullPath(dimensionFile);

                using (var zip = ZipFile.OpenRead(currentPath))
                {

                    ZipArchiveEntry packageEntry = zip.GetEntry("Package.json");
                    ZipArchiveEntry bundleEntry = zip.Entries.FirstOrDefault(entry => string.IsNullOrEmpty(Path.GetExtension(entry.Name)));

                    if (bundleEntry == null || packageEntry == null)
                    {
                        Debug.LogError("Invalid dimension: " + currentPath);
                        continue;
                    }

                    using (StreamReader bundleReader = new StreamReader(bundleEntry.Open()))

                    using (StreamReader packageReader = new StreamReader(packageEntry.Open()))
                    {
                        DimensionPackage package = Newtonsoft.Json.JsonConvert.DeserializeObject<DimensionPackage>(packageReader.ReadToEnd());
                        Debug.Log($"-> Name: {package.Name}, Author: {package.Author} <-");
                    }
                }
            }
            await Task.Yield(); // Stupid warning >:(
        }
    }
}
