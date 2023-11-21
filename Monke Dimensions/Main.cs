using System.Reflection;
using BepInEx;
using HarmonyLib;

namespace Monke_Dimensions
{
    [BepInPlugin(GUID, NAME, VERSION)]
    internal class Main:BaseUnityPlugin
    {
        internal const string
            GUID = "chin.monkedimensions",
            NAME = "Monke Dimensions",
            VERSION = "1.0.0";

        internal Main()
        {
            new Harmony(GUID).PatchAll(Assembly.GetExecutingAssembly());
        }

        public static void Init()
        {
            // Do setup stuff thing i dont fucking know
        }
    }

    [HarmonyPatch(typeof(GorillaTagger), "Awake")]
    internal class GorillaTagInitDone
    {
        public static void Postfix() => DimensionController.SkibidiToilet();
    }
}
