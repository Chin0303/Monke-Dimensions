using Monke_Dimensions.Helpers;
using System.Linq;
using UnityEngine;

namespace Monke_Dimensions.Behaviours.Addons;

internal class ToggleActiveState : MonkeTriggerObject
{
    private GameObject triggerObject;
    private bool isOn;

    public override void MonkeTrigger(Collider collider)
    {
        string triggerObjectName = AddonExtensions.GetTriggerEvent(TriggerEvents.ToggleActiveState);

        if (triggerObject == null) triggerObject = GameObject.Find(triggerObjectName);

        isOn = !isOn;
        triggerObject.SetActive(isOn);

        base.MonkeTrigger(collider);
    }
}
