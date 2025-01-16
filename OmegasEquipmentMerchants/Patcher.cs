using HarmonyLib;

namespace OmegasEquipmentMerchants
{
    internal class Patcher
    {
        [HarmonyPostfix]
        [HarmonyPatch(declaringType: typeof(Trait), methodName: nameof(Trait.OnBarter))]
        internal static void TraitOnBarter(Trait __instance)
        {
            TraitPatch.OnBarterPostfix(__instance:__instance);
        }
    }
}