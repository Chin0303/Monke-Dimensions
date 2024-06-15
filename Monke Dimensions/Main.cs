#if EDITOR

#else
using BepInEx;
using HarmonyLib;
using Monke_Dimensions.API;
using Monke_Dimensions.Behaviours;
using UnityEngine;
using Utilla;
using System;
using System.IO;
using System.Reflection;

namespace Monke_Dimensions;

[BepInPlugin(GUID, NAME, VERSION)]
[BepInDependency("org.legoandmars.gorillatag.utilla", "1.6.11")]
[ModdedGamemode]
internal class Main : BaseUnityPlugin
{
    public static AssetBundle assetBundle;
    internal static GameObject StandMD;

    internal const string
        GUID = "chin.monkedimensions",
        NAME = "Monke Dimensions",
        VERSION = "1.2.2";

    internal Main()
    {
        new Harmony(GUID).PatchAll(typeof(Main).Assembly);
        Utilla.Events.GameInitialized += (sender, e) =>
        {
            assetBundle = LoadAssetBundle("Monke_Dimensions.Resources.stand");

            Instantiate(assetBundle.LoadAsset<GameObject>("StandMD"));
            GameObject.Find("StandMD(Clone)/RealStand").AddComponent<GorillaSurfaceOverride>();
            Comps.SetupComps();

            var dimensionManager = new GameObject("Dimension Manager").AddComponent<DimensionManager>();
            StandMD = GameObject.Find("StandMD(Clone)");
            StandMD.transform.position = new(-68.617f, 11.422f, -81.257f);
            StandMD.transform.rotation = Quaternion.Euler(0, 116.5558f, 0);
            new GameObject("Dimension Teleport").AddComponent<TeleportDimension>().transform.SetParent(dimensionManager.gameObject.transform);

            Comps.Confetti = assetBundle.LoadAsset<GameObject>("Confetti");

        };

        Events.RoomJoined += (sender, e) =>
        {
            string gamemode = e.Gamemode;
            bool inModded = gamemode.ToUpper().Contains("MODDED");
            StandMD.SetActive(inModded);
        };

        Events.RoomLeft += (sender, e) => StandMD.SetActive(false);

        Action<string> value = Debug.Log;
        DimensionEvents.OnDimensionEnter += value;
        DimensionEvents.OnDimensionLeave += value;
    }

    public AssetBundle LoadAssetBundle(string path)
    {
        Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
        AssetBundle bundle = AssetBundle.LoadFromStream(stream);
        stream.Close();
        return bundle;
    }

    [ModdedGamemodeJoin]
    private void OnJoin() => StandMD.SetActive(true);
    [ModdedGamemodeLeave]
    private void OnLeave() => StandMD.SetActive(false);
}

#endif
