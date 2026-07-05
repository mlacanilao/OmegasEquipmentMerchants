using HarmonyLib;

namespace OmegasEquipmentMerchants;

internal static class Patcher
{
    [HarmonyPostfix]
    [HarmonyPatch(declaringType: typeof(ShopTransaction), methodName: nameof(ShopTransaction.GetPrice))]
    internal static void ShopTransactionGetPrice(ShopTransaction __instance, Thing t, int n, bool sell, ref int __result)
    {
        ShopTransactionPatch.GetPricePostfix(__instance: __instance, thing: t, count: n, sell: sell, __result: ref __result);
    }
}
