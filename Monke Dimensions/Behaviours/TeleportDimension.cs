using Monke_Dimensions.Models;
using Monke_Dimensions.Patches;
using UnityEngine;
using Monke_Dimensions.Helpers;

namespace Monke_Dimensions.Behaviours;

internal class TeleportDimension : MonoBehaviour
{
    public static GameObject closestTerminalPoint;

    public static void OnTeleport(DimensionPackage dimensionPackage, bool respawnPlayer = false)
    {
        if (respawnPlayer)
        {
            string teleportDestination = AddonExtensions.GetTriggerEvent(TriggerEvents.TeleportPlayer);

            var tpGO = GameObject.Find(teleportDestination);
            TeleportPatch.TeleportPlayer(tpGO.transform.position, 180f, false);
        }
        else
        {
            string spawnPointName = dimensionPackage.SpawnPoint;
            string terminalPointName = dimensionPackage.TerminalPoint;

            GameObject spawnPointObject = GameObject.Find(spawnPointName);
            GameObject terminalPointObject = GameObject.Find(terminalPointName);

            Vector3 spawnPoint = spawnPointObject.transform.position;
            Vector3 terminalPoint = terminalPointObject.transform.position;

            TeleportPatch.TeleportPlayer(spawnPoint, 180f, false);

            GameObject standObject = GameObject.Find("StandMD(Clone)");
            standObject.transform.position = terminalPoint;
        }
    }

    public static void ReturnToMonke()
    {
        Vector3 SpawnStump = new Vector3(-64.6577f, 11.1684f, -83.0683f);
        TeleportPatch.TeleportPlayer(SpawnStump, 180f, false);
        GameObject.Find("StandMD(Clone)").transform.position = new Vector3(-68.817f, 11.422f, -81.777f);
    }

    private void FixedUpdate()
    {
        // proabbly couold do it better, ill worry about it later
        if (DimensionManager.Instance.inDimension && DimensionManager.Instance.extraTerminals.Count > 1)
        {
            closestTerminalPoint = GorillaTagger.Instance.offlineVRRig.gameObject.FindClosestTerminal(DimensionManager.Instance.extraTerminals);
            Main.StandMD.transform.position = closestTerminalPoint.transform.position;
        }
    }
}
