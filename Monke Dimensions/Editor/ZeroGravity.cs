using UnityEngine;

namespace Monke_Dimensions.Editor;

public class ZeroGravity : MonoBehaviour
{
    private bool isOn;
    #if EDITOR

#else
    private void Awake() =>
        gameObject.layer = 18;

    private void OnTriggerStay(Collider collider)
    {
        if (collider.GetComponentInParent<GorillaTriggerColliderHandIndicator>() != null)
        {
            isOn = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.GetComponentInParent<GorillaTriggerColliderHandIndicator>() != null)
        {
            isOn = false;
        }
    }

    private void FixedUpdate()
    {
        GorillaTagger.Instance.rigidbody.AddForce(Vector3.up * (isOn ? 9.5f : 0), ForceMode.Acceleration);
    }
#endif
}
