using HarmonyLib;

namespace OmegasEquipmentMerchants
{
    internal class Patcher
    {
        [HarmonyPostfix]
        [HarmonyPatch(declaringType: typeof(Trait), methodName: nameof(Trait.OnBarter))]
        internal static void TraitOnBarter(Trait __instance)
        {
            TraitPatch.OnBarterPostfix(__instance: __instance);
        }
        
        [HarmonyPostfix]
        [HarmonyPatch(declaringType: typeof(ShopTransaction), methodName: nameof(ShopTransaction.GetPrice))]
        internal static void ShopTransactionGetPrice(ShopTransaction __instance, ref int __result)
        {
            ShopTransactionPatch.GetPricePostfix(__instance: __instance, ref __result);
        }
    }
}