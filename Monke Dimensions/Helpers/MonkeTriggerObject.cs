using UnityEngine;

namespace Monke_Dimensions.Helpers;

public class MonkeTriggerObject : MonoBehaviour
{
    private bool isTriggering = false;
    public string TriggerObjectName { get; set; }
#if EDITOR

#else
    private void Start() => gameObject.layer = 18;
    private void OnTriggerEnter(Collider collider)
    {
        if (isTriggering) return;

        if (collider.name == GorillaTagger.Instance.bodyCollider.name || collider.GetComponentInParent<GorillaTriggerColliderHandIndicator>() != null)
        {
            isTriggering = true;
            MonkeTrigger(collider);
            isTriggering = false;
        }
    }
#endif
    public virtual void MonkeTrigger(Collider collider)
    {
        Debug.Log("Triggered: " + collider.gameObject.name);
    }

}