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
            var dimensionFiles = Directory.GetFiles(path, "*.dimension"); // .dimension is actually just a .zip but renamed lol

            foreach (string dimensionFile in dimensionFiles)
            {
                string currentPath = Path.GetFullPath(dimensionFile);

                using (var zip = ZipFile.OpenRead(currentPath))
                {
                    ZipArchiveEntry packageEntry = zip.GetEntry("Package.json");

                    if (packageEntry == null)
                    {
                        Debug.LogError("Invalid dimension: " + currentPath);
                        continue;
                    }

                    using (StreamReader packageReader = new StreamReader(packageEntry.Open()))
                    {
                        DimensionPackage package = Newtonsoft.Json.JsonConvert.DeserializeObject<DimensionPackage>(packageReader.ReadToEnd());
                        Debug.Log($"-> Name: {package.Name}, Author: {package.Author} <-");
                    }
                    await LoadAndInstantiateAssets(dimensionFile);
                }
            }
            await Task.Yield(); // Stupid warning >:(
        }


        /* I've spend 1.5 days only doing this, i couldve played fortnite :( */
        internal static async Task LoadAndInstantiateAssets(string zipFilePath)
        {
            byte[] zipBytes = File.ReadAllBytes(zipFilePath);
            byte[] bundleBytes;

            using (MemoryStream zipStream = new MemoryStream(zipBytes))
            using (ZipArchive zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Read))
            {
                ZipArchiveEntry selectedEntry = zipArchive.Entries.FirstOrDefault(entry => string.IsNullOrEmpty(Path.GetExtension(entry.Name)));

                using (Stream entryStream = selectedEntry.Open())
                using (MemoryStream bundleStream = new MemoryStream())
                {
                    await entryStream.CopyToAsync(bundleStream);
                    bundleBytes = bundleStream.ToArray();
                }
            }

            AssetBundleCreateRequest assetBundleCreateRequest = AssetBundle.LoadFromMemoryAsync(bundleBytes);

            AssetBundle assetBundle = assetBundleCreateRequest.assetBundle;

            GameObject[] gameObjects = assetBundle.LoadAllAssets<GameObject>();

            foreach (GameObject meow in gameObjects)
            {
                Debug.Log(meow.name);
                GameObject instantiatedObject = Object.Instantiate(meow); // will do better later only for testing
            }
        }
    }
}
