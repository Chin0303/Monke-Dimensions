using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Monke_Dimensions.Behaviours;

internal class Comps : MonoBehaviour
{
    public static GameObject LeftBtn;
    public static GameObject RightBtn;
    public static GameObject LoadBtn;

    public static Text AuthorText;
    public static Text NameText;
    public static Text DescriptionText;
    public static Text StatusText;
    public static List<GameObject> EnviormentObjects = new List<GameObject>()
    {
        GameObject.Find("Environment Objects/LocalObjects_Prefab/Standard Sky/"),
        GameObject.Find("Environment Objects/LocalObjects_Prefab/CityToSkyJungle/"),
        GameObject.Find("Environment Objects/LocalObjects_Prefab/SkyJungleBottom/")
    };

    public static void SetupComps()
    {
        NameText = GameObject.Find("UI/Screen/Name").GetComponent<Text>();
        AuthorText = GameObject.Find("UI/Screen/Author").GetComponent<Text>();
        StatusText = GameObject.Find("UI/Screen/Current").GetComponent<Text>();
        DescriptionText = GameObject.Find("UI/Screen/Description").GetComponent<Text>();


        LoadBtn = GameObject.Find("Buttons/Load Btn");

        RightBtn = GameObject.Find("Buttons/Right Btn");
        LeftBtn = GameObject.Find("Buttons/Left Btn");
    }
}
