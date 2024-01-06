using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using UnityEngine;
using Monke_Dimensions.Interaction;
using Monke_Dimensions.Models;
using System.Threading.Tasks;

namespace Monke_Dimensions.Behaviours
{
    internal class DimensionManager : MonoBehaviour
    {
        public static DimensionManager Instance { get; private set; }
        public DimensionPackage currentPackage;

        public DimensionPackage package;

        internal GameObject loadedDimensionObj;

        private Dictionary<string, DimensionPackage> dimensions = new Dictionary<string, DimensionPackage>();
        private Dictionary<string, AssetBundle> loadedAssetBundles = new Dictionary<string, AssetBundle>();

        public string currentSelectedDimensionName;
        private GameObject[] currentLoadedDimensionObjects;

        public int currentPage = 1;
        public List<string> dimensionNames;

        private bool inDimension;

        internal DimensionManager()
        {
            Instance = this;
            loadedDimensionObj = new GameObject("LoadedDimension");
            LoadDimensions();
        }

        public void LoadDimensions()
        {
            Comps.LeftBtn.AddComponent<Button>();
            Comps.RightBtn.AddComponent<Button>();
            Comps.LoadBtn.AddComponent<Button>();

            Debug.Log("-> Found Dimension(s): <-");

            string path = Path.Combine(Path.GetDirectoryName(typeof(DimensionManager).Assembly.Location), "Dimensions");
            var dimensionFiles = Directory.GetFiles(path, "*.dimension");

            dimensionNames = new List<string>();

            foreach (string dimensionFile in dimensionFiles)
            {
                LoadDimension(dimensionFile);
            }
        }

        private void LoadDimension(string dimensionFile)
        {
            string currentPath = Path.GetFullPath(dimensionFile);

            using (var zip = ZipFile.OpenRead(currentPath))
            {
                ZipArchiveEntry packageEntry = zip.GetEntry("Package.json");

                if (packageEntry == null)
                {
                    Debug.LogError("Invalid dimension: " + currentPath);
                    return;
                }

                using (StreamReader packageReader = new StreamReader(packageEntry.Open()))
                {
                    package = Newtonsoft.Json.JsonConvert.DeserializeObject<DimensionPackage>(packageReader.ReadToEnd());

                    Debug.Log($"-> Name: {package.Name}, Author: {package.Author}, Spawnpoint Name: {package.SpawnPoint}, Terminal Name: {package.TerminalPoint} <-");

                    dimensions.Add(package.Name, package);
                    dimensionNames.Add(package.Name);
                }
            }
        }

        private async void LoadAssets(string zipFilePath, DimensionPackage selectedDimension)
        {
            if (loadedAssetBundles.TryGetValue(selectedDimension.Name, out AssetBundle cachedAssetBundle))
            {
                InstantiateDimensions(cachedAssetBundle);
                return;
            }

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

            TaskCompletionSource<AssetBundle> tcs = new TaskCompletionSource<AssetBundle>();

            AssetBundleCreateRequest assetBundleCreateRequest = AssetBundle.LoadFromMemoryAsync(bundleBytes);

            assetBundleCreateRequest.completed += operation =>
            {
                AssetBundle assetBundle = assetBundleCreateRequest.assetBundle;

                if (!loadedAssetBundles.ContainsKey(selectedDimension.Name))
                {
                    loadedAssetBundles[selectedDimension.Name] = assetBundle;
                }
                else
                {
                    assetBundle.Unload(true);
                    assetBundle = loadedAssetBundles[selectedDimension.Name];
                }

                tcs.SetResult(assetBundle);
            };

            AssetBundle loadedAssetBundle = await tcs.Task;

            InstantiateDimensions(loadedAssetBundle);
        }

        private void InstantiateDimensions(AssetBundle assetBundle)
        {
            currentLoadedDimensionObjects = assetBundle.LoadAllAssets<GameObject>();

            foreach (GameObject loadedObject in currentLoadedDimensionObjects)
            {
                GameObject instantiatedObject = Instantiate(loadedObject, loadedDimensionObj.transform);
                AddGorillaSurfaceOverride(instantiatedObject);
            }
            loadedDimensionObj.transform.position = new Vector3(0f, 0f, 600f);
            TeleportDimension.OnTeleport(currentPackage);
        }
        private void AddGorillaSurfaceOverride(GameObject obj)
        {
            obj.AddComponent<GorillaSurfaceOverride>();

            foreach (Transform child in obj.transform)
            {
                AddGorillaSurfaceOverride(child.gameObject);
            }
        }

        public void LoadSelectedDimension(string dimensionName)
        {
            if (inDimension)
            {
                TeleportDimension.ReturnToMonke();
                UnloadCurrentDimension();
                GameObject.Find("Environment Objects/LocalObjects_Prefab/Standard Sky/").SetActive(true);
                return;
            }
            if (dimensions.TryGetValue(dimensionName, out currentPackage))
            {
                Debug.Log($"Loading selected dimension: {currentPackage.Name} by {currentPackage.Author}");
                string dimensionFilePath = Path.Combine(Path.GetDirectoryName(typeof(DimensionManager).Assembly.Location), "Dimensions", $"{dimensionName}.dimension");

                LoadAssets(dimensionFilePath, currentPackage);
                inDimension = true;
                GameObject.Find("Environment Objects/LocalObjects_Prefab/Standard Sky/").SetActive(false);
            }
            else
            {
                Debug.LogError($"Dimension not found: {dimensionName}");
            }
        }

        public void UnloadCurrentDimension()
        {
            foreach (Transform child in loadedDimensionObj.transform)
            {
                Destroy(child.gameObject);
            }
            currentLoadedDimensionObjects = null;
            inDimension = false;
            loadedDimensionObj.transform.position = new Vector3(0f, 0f, 0f);
        }

        public void SwitchPage(int direction)
        {
            int totalPages = dimensionNames.Count;

            currentPage = (currentPage + direction + totalPages) % totalPages;

            string dimensionName = dimensionNames[currentPage];

            if (dimensions.TryGetValue(dimensionName, out DimensionPackage selectedDimension))
            {
                Comps.AuthorText.text = $"AUTHOR: {selectedDimension.Author}".ToUpper();
                Comps.NameText.text = $"DIMENSION: {selectedDimension.Name}".ToUpper();
                Comps.DescriptionText.text = selectedDimension.Description.ToUpper();
                Comps.StatusText.text = $"DIMENSIONS FOUND: ({currentPage + 1} / {totalPages})";
            }
        }
#if DEBUG
        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 200, 500));

            GUILayout.Label("Available Dimensions:");

            foreach (string dimensionName in dimensionNames)
            {
                if (GUILayout.Button(dimensionName))
                {
                    LoadSelectedDimension(dimensionName);
                }
            }

            GUILayout.Space(20);

            GUILayout.Label("Page Navigation:");

            if (GUILayout.Button("Previous Page"))
            {
                SwitchPage(-1);
            }

            if (GUILayout.Button("Next Page"))
            {
                SwitchPage(1);
            }

            GUILayout.Space(20);

            GUILayout.Label($"Current Page: {currentPage + 1}");

            GUILayout.Label("Load Current Dimension:");

            if (GUILayout.Button("Load Current Dimension"))
            {
                LoadSelectedDimension(dimensionNames[currentPage]);
            }

            if (GUILayout.Button("Unload Current Dimension"))
            {
                UnloadCurrentDimension();
            }

            GUILayout.EndArea();
        }
#endif
    }
}