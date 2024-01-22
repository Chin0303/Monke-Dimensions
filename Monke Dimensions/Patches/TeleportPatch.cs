// stolen from kyle who stole it from graic

using GorillaLocomotion;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace Monke_Dimensions.Patches;

[HarmonyPatch(typeof(Player))]
[HarmonyPatch("LateUpdate", MethodType.Normal)]
internal class TeleportPatch
{
    private static bool _isTeleporting = false,
        _rotate = false;
    private static Vector3 _teleportPosition;
    private static float _teleportRotation;
    private static bool _killVelocity;

    internal static bool Prefix(Player __instance, ref Vector3 ___lastPosition, ref Vector3[] ___velocityHistory, ref Vector3 ___lastHeadPosition, ref Vector3 ___lastLeftHandPosition, ref Vector3 ___lastRightHandPosition, ref Vector3 ___currentVelocity, ref Vector3 ___denormalizedVelocityAverage)
    {
        try
        {
            if (_isTeleporting)
            {

                var playerRigidBody = __instance.GetComponent<Rigidbody>();
                if (playerRigidBody != null)
                {
                    Vector3 correctedPosition = _teleportPosition - __instance.bodyCollider.transform.position + __instance.transform.position;

                    if (_killVelocity)
                        playerRigidBody.velocity = Vector3.zero;

                    __instance.transform.position = correctedPosition;
                    if (_rotate)
                        __instance.Turn(_teleportRotation - __instance.headCollider.transform.rotation.eulerAngles.y);

                    ___lastPosition = correctedPosition;
                    ___velocityHistory = new Vector3[__instance.velocityHistorySize];

                    ___lastHeadPosition = __instance.headCollider.transform.position;
                    var leftHandMethod = typeof(Player).GetMethod("GetCurrentLeftHandPosition",
                        BindingFlags.NonPublic | BindingFlags.Instance);
                    ___lastLeftHandPosition = (Vector3)leftHandMethod.Invoke(__instance, new object[] { });

                    var rightHandMethod = typeof(Player).GetMethod("GetCurrentRightHandPosition",
                        BindingFlags.NonPublic | BindingFlags.Instance);
                    ___lastRightHandPosition = (Vector3)rightHandMethod.Invoke(__instance, new object[] { });
                    ___currentVelocity = Vector3.zero;
                    ___denormalizedVelocityAverage = Vector3.zero;
                }
                _isTeleporting = false;
                return true;
            }
        }
        catch (System.Exception e) { Debug.LogException(e); }
        return true;
    }

    internal static void TeleportPlayer(Vector3 destinationPosition, float destinationRotation, bool killVelocity = false)
    {
        if (_isTeleporting)
            return;
        _killVelocity = killVelocity;
        _teleportPosition = destinationPosition;
        _teleportRotation = destinationRotation;
        _isTeleporting = true;
        _rotate = true;
    }
}