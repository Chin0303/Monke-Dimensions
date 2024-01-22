using BepInEx;
using HarmonyLib;
using Monke_Dimensions.Behaviours;
using Photon.Pun;
using UnityEngine;
using Utilla;
using Utilla.Models;

namespace Monke_Dimensions;

[BepInPlugin(GUID, NAME, VERSION)]
[BepInDependency("org.legoandmars.gorillatag.utilla", "1.6.11")]
[ModdedGamemode]
internal class Main : BaseUnityPlugin
{
    internal static GameObject StandMD;

    internal const string
        GUID = "chin.monkedimensions",
        NAME = "Monke Dimensions",
        VERSION = "1.0.1";

    internal Main()
    {
        new Harmony(GUID).PatchAll(typeof(Main).Assembly);
        Utilla.Events.GameInitialized += (sender, e) =>
        {
            AssetLoader.LoadAssets("Monke_Dimensions.Resources.stand");

            Comps.SetupComps();

            var dimensionManager = new GameObject("Dimension Manager").AddComponent<DimensionManager>();
            StandMD = GameObject.Find("StandMD(Clone)");
            new GameObject("Dimension Teleport").AddComponent<TeleportDimension>().transform.SetParent(dimensionManager.gameObject.transform);
        };

        Events.RoomJoined += (sender, e) =>
        {
            string gamemode = e.Gamemode;
            bool inModded = gamemode.ToUpper().Contains("MODDED");
            StandMD.SetActive(inModded);
        };
        Events.RoomLeft += (sender, e) => StandMD.SetActive(false);
    }

    [ModdedGamemodeJoin]
    private void OnJoin() => StandMD.SetActive(true);
    [ModdedGamemodeLeave]
    private void OnLeave() => StandMD.SetActive(false);
}