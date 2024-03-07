#if EDITOR

#else
using BepInEx;
using GorillaLocomotion;
using HarmonyLib;
using Monke_Dimensions.API;
using Monke_Dimensions.Behaviours;
using Photon.Pun;
using UnityEngine;
using Utilla;

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

        Events.RoomLeft += (sender, e) => StandMD.SetActive(false);

        DimensionEvents.OnDimensionEnter += (dimension) =>
        {
            Debug.Log(dimension);
        };
    }

    [ModdedGamemodeJoin]
    private void OnJoin() => StandMD.SetActive(true);
    [ModdedGamemodeLeave]
    private void OnLeave() => StandMD.SetActive(false);
}

public static class RigClampPatch
{
    [HarmonyPatch(typeof(VRRig), "SanitizeVector3"), HarmonyPostfix]
    private static void SanitizeVector3_Postfix(Vector3 vec, ref Vector3 __result)
    {
        if (__result != Vector3.zero)
        {
            __result = Vector3.ClampMagnitude(vec, 100000f);
        }
    }
}

#endif