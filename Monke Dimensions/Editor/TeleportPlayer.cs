using UnityEngine;

using Monke_Dimensions.Helpers;
using Monke_Dimensions.Patches;

namespace Monke_Dimensions.Behaviours.Addons;

public class TeleportPlayer : MonkeTriggerObject
{ 
    public GameObject TeleportDestination;

    public override void MonkeTrigger(Collider collider)
    {
#if EDITOR

#else
        TeleportPatch.TeleportPlayer(TeleportDestination.transform.position, 180f, false);

        base.MonkeTrigger(collider);
#endif
    }
}
