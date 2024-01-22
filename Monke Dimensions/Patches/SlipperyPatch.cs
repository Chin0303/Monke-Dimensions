using HarmonyLib;
using GorillaLocomotion;

namespace Monke_Dimensions.Patches
{
    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch("GetSlidePercentage", MethodType.Normal)]
    public class SlidePatch
    {
        public static void Postfix(Player __instance, ref float __result)
        {
            if (SlipperyHands.Instance)
                __result = SlipperyHands.Instance.enabled ? 1 : __result;
        }
    }
}
