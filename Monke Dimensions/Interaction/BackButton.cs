using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monke_Dimensions.Behaviours;
using UnityEngine;

namespace Monke_Dimensions.Interaction
{
    internal class BackButton : MonoBehaviour
    {
        private float touchTime = 0f;
        private const float debounceTime = 0.25f;
        private void Start() => gameObject.layer = 18;
        private void OnTriggerEnter(Collider collider)
        {
            if (touchTime + debounceTime >= Time.time) return;
            var hand = collider.GetComponentInParent<GorillaTriggerColliderHandIndicator>();
            if (collider.TryGetComponent(out hand))
            {
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(211, hand.isLeftHand, 0.12f);
                GorillaTagger.Instance.StartVibration(hand.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
                TeleportDimension.ReturnToMonke();
                DimensionManager.Instance.UnloadCurrentDimension();
            }
        }
    }
}
