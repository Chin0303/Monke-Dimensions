using UnityEngine;

namespace Monke_Dimensions.Helpers;

public class MonkeTriggerObject : MonoBehaviour
{
#if EDITOR

#else
    private void Start() => 
        gameObject.layer = 18;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.name == GorillaTagger.Instance.bodyCollider.name || collider.GetComponentInParent<GorillaTriggerColliderHandIndicator>() != null)
        {
            MonkeTrigger(collider);
        }
    }
#endif
    public virtual void MonkeTrigger(Collider collider)
    {
#if EDITOR
#else
        var hand = collider.GetComponentInParent<GorillaTriggerColliderHandIndicator>();
        GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(211, hand.isLeftHand, 0.12f);
        GorillaTagger.Instance.StartVibration(hand.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);

        Debug.Log("Triggered: " + collider.gameObject.name);
#endif

    }
}