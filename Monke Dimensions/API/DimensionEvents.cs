#if EDITOR

#else
using System;
using UnityEngine;

namespace Monke_Dimensions.API;

public class DimensionEvents
{
    /// <summary>
    /// Raised when entering a dimension.
    /// </summary>
    public static Action<string> OnDimensionEnter;

    /// <summary>
    /// Raised when leaving a dimension.
    /// </summary>
    public static Action<string> OnDimensionLeave;

    /// <summary>
    /// Raised when a triggerevent occurs.
    /// </summary>
    public static Action<TriggerEvent, GameObject, GameObject, bool> OnDimensionTriggerEvent;

/*     
     Second GameObject variable is the GameObject that is being triggered by the trigger GameObject
     Dont attack me for being bad at writing api's :P
*/

}
#endif