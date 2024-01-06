using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using BepInEx;
using HarmonyLib;
using Monke_Dimensions.Behaviours;
using UnityEngine;
using Utilla;

namespace Monke_Dimensions
{
    [BepInPlugin(GUID, NAME, VERSION)]
    [ModdedGamemode]
    internal class Main : BaseUnityPlugin
    {
        internal static bool inRoom;
        internal GameObject StandMD;

        internal const string
            GUID = "chin.monkedimensions",
            NAME = "Monke Dimensions",
            VERSION = "1.0.0";

        internal Main()
        {
            new Harmony(GUID).PatchAll(typeof(Main).Assembly);
            Utilla.Events.GameInitialized += (sender, e) =>
            {
                AssetLoader.LoadAssets("Monke_Dimensions.Resources.stand");

                Comps.SetupComps();
                new DimensionManager();
                StandMD = GameObject.Find("StandMD(Clone)");
            };
        }

        [ModdedGamemodeJoin]
        private void OnJoin() => StandMD.SetActive(true);
        [ModdedGamemodeLeave]
        private void OnLeave() => StandMD.SetActive(false);
    }
}
