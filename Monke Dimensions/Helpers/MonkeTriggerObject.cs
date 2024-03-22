using UnityEngine;
using UnityEngine.Internal;

namespace Monke_Dimensions.Helpers;

public class MonkeTriggerObject : MonoBehaviour
{
    private bool isTriggering = false;
    public string TriggerObjectName { get; set; }

    [Tooltip("Default value = 0.5")]
    public float triggerTime = 0.5f;
    private float triggerTimer;
#if EDITOR

#else
    private void Start() => 
        gameObject.layer = 18;
    private void Update() =>
     triggerTimer += Time.deltaTime;

    private void OnTriggerEnter(Collider collider)
    {
        if (isTriggering && triggerTimer >= triggerTime) return;
        triggerTimer = 0f;
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