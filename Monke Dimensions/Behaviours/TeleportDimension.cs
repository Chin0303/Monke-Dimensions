using Monke_Dimensions.Models;
using Monke_Dimensions.Patches;
using UnityEngine;

namespace Monke_Dimensions.Behaviours
{
    internal class TeleportDimension : MonoBehaviour
    {
        public static void OnTeleport(DimensionPackage dimensionPackage)
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
        public static void ReturnToMonke()
        {
            Vector3 SpawnStump = new Vector3(-64.6577f, 11.1684f, -83.0683f);
            TeleportPatch.TeleportPlayer(SpawnStump, 180f, false);
            GameObject.Find("StandMD(Clone)").transform.position = new Vector3(-68.817f, 11.422f, -81.777f);
        }
    }
}