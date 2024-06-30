#if EDITOR

#else
using GorillaLocomotion;
#endif
using UnityEngine;
using System.Collections;

namespace Monke_Dimensions.Helpers
{
    public class MonkeTriggerObject : MonoBehaviour
    {
        private float debounceTime = 0.25f;
        private float touchTime;

        private void Start() =>
            gameObject.layer = 18;
#if EDITOR

#else
        private void OnTriggerEnter(Collider collider)
        {
            if (!(touchTime + debounceTime < Time.time))
            {
                return;
            }

            if (collider == GorillaTagger.Instance.bodyCollider || collider.GetComponentInParent<GorillaTriggerColliderHandIndicator>() != null)
            {
                touchTime = Time.time;
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
}
