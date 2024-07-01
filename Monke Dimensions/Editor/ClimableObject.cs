namespace Monke_Dimensions.Editor;
public class ClimableObject :
#if !EDITOR
GorillaLocomotion.Climbing.GorillaClimbable
#else
UnityEngine.MonoBehaviour
#endif 
{
    private void Start()
    {
        gameObject.layer = 18;
    }
}