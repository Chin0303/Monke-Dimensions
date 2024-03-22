#if EDITOR

#else
using Monke_Dimensions.API;
#endif
using Monke_Dimensions.Helpers;
using UnityEngine;

namespace Monke_Dimensions.Editor;

public class ToggleActiveState : MonkeTriggerObject
{
    public GameObject ObjectToToggle;
    private bool isOn;
#if EDITOR

#else
    public override void MonkeTrigger(Collider collider)
    {
        isOn = !isOn;
        ObjectToToggle.SetActive(isOn);
        DimensionEvents.OnDimensionTriggerEvent(TriggerEvent.ToggleActiveState, this.gameObject, isOn);
        base.MonkeTrigger(collider);
    }
#endif
}