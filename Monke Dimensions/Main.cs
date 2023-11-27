using System;
using System.Reflection;
using BepInEx;
using HarmonyLib;
using Monke_Dimensions.Behaviours;
using UnityEngine;

namespace Monke_Dimensions
{
    [BepInPlugin(GUID, NAME, VERSION)]
    internal class Main:BaseUnityPlugin
    {
        internal const string
            GUID = "chin.monkedimensions",
            NAME = "Monke Dimensions",
            VERSION = "1.0.0";

        void Awake()
        {
            new Harmony(GUID).PatchAll(Assembly.GetExecutingAssembly());
            Utilla.Events.GameInitialized += setup;
        }

        public void setup(object sender, EventArgs e)
        {
            AssetLoader.LoadAssets("Monke_Dimensions.Resources.stand");
            GameObject Meow = new GameObject("DimensionThing");
            Comps.SetupComps();
            Meow.AddComponent<DimensionManager>();
        }
    }
}
