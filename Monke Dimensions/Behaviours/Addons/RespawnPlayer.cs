using UnityEngine;
using Monke_Dimensions.Helpers;

namespace Monke_Dimensions.Behaviours.Addons;

internal class RespawnPlayer : MonkeTriggerObject
{
    public override void MonkeTrigger(Collider collider)
    {
        TeleportDimension.OnTeleport(DimensionManager.Instance.currentPackage, true);

        base.MonkeTrigger(collider);
    }
}