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
    public static Action<string> OnDimensionEnter { get; set; }

    /// <summary>
    /// Raised when leaving a dimension.
    /// </summary>
    public static Action<string> OnDimensionLeave { get; set; }

    /// <summary>
    /// Raised when a triggerevent occurs.
    /// </summary>
    public static Action<TriggerEvent, GameObject, bool> OnDimensionTriggerEvent { get; set;}

}
#endif