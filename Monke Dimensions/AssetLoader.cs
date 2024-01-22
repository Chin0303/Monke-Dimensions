using System.IO;
using System.Reflection;
using UnityEngine;

namespace Monke_Dimensions;

internal class AssetLoader : MonoBehaviour
{
    public static void LoadAssets(string path)
    {
        Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
        AssetBundle assetBundle = AssetBundle.LoadFromStream(manifestResourceStream);

        GameObject Stand = Instantiate<GameObject>(assetBundle.LoadAsset<GameObject>("StandMD"));
        GameObject.Find("StandMD(Clone)/RealStand").AddComponent<GorillaSurfaceOverride>();
        // For some reason the heat death of the universe happens when this mf doesnt have that component
        assetBundle.Unload(false);
    }
}
