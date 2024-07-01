using UnityEngine;

namespace Monke_Dimensions.Editor;

public class WaterObject : MonoBehaviour
{
    [Tooltip("Use the water mesh from gorilla tag")]
    public bool useGorillaTagTexture;

    [HideInInspector]
    public GameObject ogWater;
    private void Start()
    {
#if EDITOR

#else
        GameObject.Find("Environment Objects/LocalObjects_Prefab/ForestToBeach/").SetActive(true);
        ogWater = Instantiate(GameObject.Find("Environment Objects/LocalObjects_Prefab/ForestToBeach/ForestToBeach_Prefab_V4/CaveWaterVolume/"));
        ogWater.transform.SetParent(transform, false);
        ogWater.name = "New Watuh";
        ogWater.transform.position = gameObject.transform.position;
        ogWater.transform.rotation = ogWater.transform.rotation;
        ogWater.transform.localScale = gameObject.transform.localScale;

        ogWater.transform.GetChild(0).GetComponent<MeshFilter>().mesh = gameObject.GetComponent<MeshFilter>().mesh;
        Behaviours.DimensionManager.Instance.LocalObjectsGameObject.SetActive(false);

        if (!useGorillaTagTexture)
            ogWater.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
#endif
    }
}