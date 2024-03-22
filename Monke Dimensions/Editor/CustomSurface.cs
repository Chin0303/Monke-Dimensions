using UnityEngine;

namespace Monke_Dimensions.Editor;

public class CustomSurface : MonoBehaviour
{
    public int surfaceIndexInteger;
#if EDITOR

#else
    private void Start()
    {
        if(gameObject.GetComponent<GorillaSurfaceOverride>() != null)
            gameObject.GetComponent<GorillaSurfaceOverride>().overrideIndex = surfaceIndexInteger;
        else
            gameObject.AddComponent<GorillaSurfaceOverride>().overrideIndex = surfaceIndexInteger;
    }
#endif
}