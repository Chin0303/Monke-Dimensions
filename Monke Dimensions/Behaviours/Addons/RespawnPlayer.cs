using UnityEngine;
using Monke_Dimensions.Helpers;
using Monke_Dimensions.Patches;

namespace Monke_Dimensions.Behaviours.Addons;

public class RespawnPlayer : MonkeTriggerObject
{
    public override void MonkeTrigger(Collider collider)
    {

        TeleportPatch.TeleportPlayer(tpGO.transform.position, 180f, false);

        base.MonkeTrigger(collider);
    }
}