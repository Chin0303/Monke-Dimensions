using Monke_Dimensions.Helpers;
using System.Linq;
using UnityEngine;

namespace Monke_Dimensions.Behaviours.Addons;

public class ToggleActiveState : MonkeTriggerObject
{
    public  GameObject triggerObject;
    private bool isOn;

    public override void MonkeTrigger(Collider collider)
    {

        isOn = !isOn;
        triggerObject.SetActive(isOn);

        base.MonkeTrigger(collider);
    }
}
