
using UnityEngine;
using Monke_Dimensions.Helpers;

#if !EDITOR
using GorillaLocomotion;
#endif

namespace Monke_Dimensions.Editor;

public class ChangeVelocity : MonoBehaviour
{
    private float defaultJumpMultiplier = 1.1f;
    private float defaultMaxJumpSpeed = 6.5f;

    public float JumpMultiplier = 1.1f;
    public float MaxJumpSpeed = 6.5f;
    private void Awake() => gameObject.layer = 18;
#if EDITOR
#else

    private void OnTriggerStay(Collider collider)
    {
        Player.Instance.jumpMultiplier = JumpMultiplier;
        Player.Instance.maxJumpSpeed = MaxJumpSpeed;
    }

    private void OnTriggerExit(Collider collider)
    {
        Player.Instance.jumpMultiplier = defaultJumpMultiplier;
        Player.Instance.maxJumpSpeed = defaultMaxJumpSpeed;
    }
#endif
}
