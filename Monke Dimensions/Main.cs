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
using BepInEx.Logging;
using Monke_Dimensions.Editor.Grabbables;
using Photon.Pun;

namespace Monke_Dimensions;

[BepInPlugin(GUID, NAME, VERSION)]
[BepInDependency("org.legoandmars.gorillatag.utilla", "1.6.11")]
[ModdedGamemode]
internal class Main : BaseUnityPlugin
{
    public static AssetBundle assetBundle;
    internal static GameObject StandMD;

    private GameObject Manegerl;
    private DimensionManager DimensionInstance;

    public static ManualLogSource Logger;

    public static bool inModded = true;

    private const string
        GUID = "chin.monkedimensions",
        NAME = "Monke Dimensions",
        VERSION = "1.3.2";

    internal Main()
    {
        Logger = base.Logger;
        new Harmony(GUID).PatchAll(typeof(Main).Assembly);

        Utilla.Events.GameInitialized += async (sender, e) =>
        {
            assetBundle = LoadAssetBundle("Monke_Dimensions.Resources.stand");
            Instantiate(assetBundle.LoadAsset<GameObject>("StandMD"));
            GameObject.Find("StandMD(Clone)/RealStand").AddComponent<GorillaSurfaceOverride>();
            Comps.SetupComps();

            var dimensionManager = new GameObject("Dimension Manager").AddComponent<DimensionManager>();
            DimensionInstance = dimensionManager.GetComponent<DimensionManager>();
            StandMD = GameObject.Find("StandMD(Clone)");
            new GameObject("Dimension Teleport").AddComponent<TeleportDimension>().transform.SetParent(dimensionManager.gameObject.transform);

            Comps.Confetti = assetBundle.LoadAsset<GameObject>("Confetti");
            StandMD.transform.position = new(-64.9861f, 11.422f, -84.0595f);
            StandMD.transform.rotation = Quaternion.Euler(0, 318.3739f, 0);
        };

        Events.RoomJoined += (sender, e) =>
        {
            string gamemode = e.Gamemode;
            inModded = gamemode.ToUpper().Contains("MODDED");
            StandMD.SetActive(inModded);
            if (DimensionManager.Instance.inDimension && !inModded)
            {
                DimensionInstance.LoadSelectedDimension(DimensionInstance.dimensionNames[DimensionInstance.currentPage]);
                TeleportDimension.ReturnToMonke(DimensionInstance.currentDimensionPackage);
            }
        };

        DimensionEvents.OnDimensionTriggerEvent += (a, b, c, d) => { };

        Events.RoomLeft += (sender, e) => { StandMD.SetActive(false); inModded = true; };

        Action<string> value = Logger.LogInfo;

        DimensionEvents.OnDimensionEnter += value => { if (!PhotonNetwork.InRoom) return; Manegerl = new GameObject("MeowManager").AddComponent<GrabManager>().gameObject; };
        DimensionEvents.OnDimensionLeave += value => { Manegerl?.SafeDestroy(); };
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