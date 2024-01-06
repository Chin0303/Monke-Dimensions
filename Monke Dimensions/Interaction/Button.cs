using Monke_Dimensions.Behaviours;
using UnityEngine;

namespace Monke_Dimensions.Interaction
{
    internal class Button : MonoBehaviour
    {
        public ButtonType BtnType;
        private float touchTime = 0f;
        private const float debounceTime = 0.25f;
        private void Start()
        {
            switch (gameObject.name)
            {
                case "Load Btn":
                    BtnType = ButtonType.Load;
                    break;
                case "Left Btn":
                    BtnType = ButtonType.Left;
                    break;
                case "Right Btn":
                    BtnType = ButtonType.Right;
                    break;
            }
            gameObject.layer = 18;
        }
        private void OnTriggerEnter(Collider collider)
        {
            if (touchTime + debounceTime >= Time.time) return;
            var hand = collider.GetComponentInParent<GorillaTriggerColliderHandIndicator>();
            if (collider.TryGetComponent(out hand))
            {
                GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(211, hand.isLeftHand, 0.12f);
                GorillaTagger.Instance.StartVibration(hand.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
                switch (BtnType)
                {
                    case ButtonType.Left:
                        DimensionManager.Instance.SwitchPage(-1);
                        break;
                    case ButtonType.Right:
                        DimensionManager.Instance.SwitchPage(1);
                        break;
                    case ButtonType.Load:
                        DimensionManager.Instance.LoadSelectedDimension(DimensionManager.Instance.dimensionNames[DimensionManager.Instance.currentPage]);
                        break;
                }
            }
        }
    }
}
