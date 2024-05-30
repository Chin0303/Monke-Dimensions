#if EDITOR

#else
using Monke_Dimensions.Behaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Monke_Dimensions.API;

public static class DimensionTools
{
    /// <param name="name"></param>
    /// <returns>
    /// Returns a gameobject in the current loaded dimension. 
    /// Returns null if the gameobject can't be found.
    /// </returns>
    public static GameObject FindObjectInDimension(string name)
    {
        foreach(GameObject gameObject in DimensionManager.Instance.currentLoadedDimensionObjects)
        {
            if(gameObject.name == name)
            {
                return gameObject;
            }
        }

        Debug.LogError("Null Reference! Object with name '" + name + "' not found in current dimension.");
        return null;
    }
}
#endif