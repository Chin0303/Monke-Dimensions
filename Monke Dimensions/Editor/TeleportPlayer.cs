using UnityEngine;

using Monke_Dimensions.Helpers;
#if EDITOR

#else
using Monke_Dimensions.Patches;
using Monke_Dimensions.API;
#endif

namespace Monke_Dimensions.Behaviours.Addons;

public class TeleportPlayer : MonkeTriggerObject
{ 
    public GameObject TeleportDestination;

    public override void MonkeTrigger(Collider collider)
    {
#if EDITOR

#else
        TeleportPatch.TeleportPlayer(TeleportDestination.transform.position, 180f, false);
        DimensionEvents.OnDimensionTriggerEvent(TriggerEvent.Teleport, TeleportDestination, false);

        base.MonkeTrigger(collider);
#endif
    }
}
