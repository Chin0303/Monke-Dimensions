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
    }

    [HarmonyPatch(typeof(GorillaTagger), "Awake")]
    internal class GorillaTagInitDone
    {
        public async static void Postfix() => await DimensionController.LoadDimensions();
    }
}
