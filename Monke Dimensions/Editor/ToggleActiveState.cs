using Monke_Dimensions.Helpers;
using UnityEngine;

namespace Monke_Dimensions.Editor;

public class ToggleActiveState : MonkeTriggerObject
{
    public GameObject ObjectToToggle;
    private bool isOn;

    public override void MonkeTrigger(Collider collider)
    {
        ObjectToToggle.SetActive(isOn);
        isOn = !isOn;

        base.MonkeTrigger(collider);
    }
}