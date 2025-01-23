namespace OmegasEquipmentMerchants
{
    internal class ShopTransactionPatch
    {
        internal static void GetPricePostfix(ShopTransaction __instance, ref int __result)
        {
            if (EClass.core?.IsGameStarted == false || 
                (__instance.trader.owner.id != "pod042" && __instance.trader.owner.id != "pod153"))
            {
                return;
            }

            int costMultiplier = OmegasEquipmentMerchantsConfig.CostMultiplier?.Value ?? 1;

            __result *= costMultiplier;
        }
    }
}