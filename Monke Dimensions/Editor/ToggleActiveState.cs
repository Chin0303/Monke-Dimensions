#if EDITOR

#else
using Monke_Dimensions.API;
#endif
using Monke_Dimensions.Helpers;
using UnityEngine;

namespace Monke_Dimensions.Editor;

public class ToggleActiveState : MonkeTriggerObject
{
    [Tooltip("Every toggle active state trigger must have an UNIQUE id")]
    public string uniqueID;
    public GameObject ObjectToToggle;
    private bool isOn, eventOnCooldown;
    private float lastEventTime;
    private const float eventCooldown = 0.3f;
#if EDITOR

#else
    public override void MonkeTrigger(Collider collider)
    {
        if (eventOnCooldown || Time.time - lastEventTime < eventCooldown)
        {
            Debug.Log("Event is on cooldown.");
            return;
        }

        lastEventTime = Time.time;
        isOn = !isOn;
        ObjectToToggle.SetActive(isOn);
        DimensionEvents.OnDimensionTriggerEvent(TriggerEvent.ToggleActiveState, this.gameObject, ObjectToToggle, isOn);

        StartCooldown();
    }

    private void StartCooldown()
    {
        eventOnCooldown = true;
        Invoke(nameof(ResetCooldown), eventCooldown);
    }

    private void ResetCooldown()
    {
        eventOnCooldown = false;
    }
#endif
}