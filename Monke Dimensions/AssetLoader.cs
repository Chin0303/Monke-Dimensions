#if EDITOR

#else
using Monke_Dimensions.Behaviours;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Monke_Dimensions;

internal class AssetLoader : MonoBehaviour
{
    public static AssetBundle assetBundle;
    public static void LoadAssets(string path)
    {
        Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
        assetBundle = AssetBundle.LoadFromStream(manifestResourceStream);

        Comps.PagePrefab = assetBundle.LoadAsset<GameObject>("PagePanel");
        Comps.ItemPrefab = assetBundle.LoadAsset<GameObject>("Item");

        Instantiate(assetBundle.LoadAsset<GameObject>("StandMD"));
        GameObject.Find("StandMD(Clone)/RealStand").AddComponent<GorillaSurfaceOverride>();
        
        assetBundle.Unload(false);
    }
}
#endif