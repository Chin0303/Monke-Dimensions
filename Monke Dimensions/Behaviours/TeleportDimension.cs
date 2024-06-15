#if EDITOR
#else
using Monke_Dimensions.Models;
using Monke_Dimensions.Patches;
using UnityEngine;

using Monke_Dimensions.API;
using Monke_Dimensions.Helpers;

namespace Monke_Dimensions.Behaviours;

internal class TeleportDimension : MonoBehaviour
{

    public static void OnTeleport(DimensionPackage dimensionPackage)
    {
        GameObject spawnPointObject = FindObjectOfType<DimensionDescriptor>().SpawnPosition;
        GameObject terminalPointObject = FindObjectOfType<DimensionDescriptor>().TerminalPosition;

        Vector3 spawnPoint = spawnPointObject.transform.position;
        Vector3 terminalPoint = terminalPointObject.transform.position;

        TeleportPatch.TeleportPlayer(spawnPoint, 0f, true);

        GameObject standObject = GameObject.Find("StandMD(Clone)");
        standObject.transform.position = terminalPoint;
        DimensionEvents.OnDimensionEnter($"{dimensionPackage.Name}, {dimensionPackage.Author}");
    }

    public static void ReturnToMonke(DimensionPackage packg)
    {
        Vector3 SpawnStump = new Vector3(-68.617f, 11.422f, -81.257f);
        TeleportPatch.TeleportPlayer(SpawnStump, 0f, true);
        GameObject.Find("StandMD(Clone)").transform.position = new Vector3(-68.817f, 11.422f, -81.777f);
        DimensionEvents.OnDimensionLeave($"{packg.Name}, {packg.Author}");
    }
}
#endif