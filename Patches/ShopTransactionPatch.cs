namespace OmegasEquipmentMerchants;

internal static class ShopTransactionPatch
{
    private const string WeaponMerchantId = "pod042";
    private const string ArmorMerchantId = "pod153";
    private const int MaxValidTransactionPrice = int.MaxValue - 1;

    internal static void GetPricePostfix(ShopTransaction __instance, bool sell, ref int __result)
    {
        if (EClass.core?.IsGameStarted != true)
        {
            return;
        }

        if (sell == true)
        {
            return;
        }

        if (__result == -1)
        {
            return;
        }

        string? ownerId = __instance.trader?.owner?.id;
        if (ownerId != WeaponMerchantId && ownerId != ArmorMerchantId)
        {
            return;
        }

        int costMultiplier = OmegasEquipmentMerchantsConfig.GetCostMultiplier();
        long multipliedPrice = (long)__result * costMultiplier;
        string priceState = "normal";
        if (multipliedPrice >= int.MaxValue)
        {
            __result = MaxValidTransactionPrice;
            priceState = "capped";
        }
        else if (multipliedPrice <= -int.MaxValue)
        {
            __result = -1;
            return;
        }
        else
        {
            __result = (int)multipliedPrice;
        }

        FeatureTestLog.LogOnce(
            feature: "Price Multiplier",
            key: ownerId + "|" + costMultiplier.ToString() + "|" + priceState,
            detail: "matched custom merchant; ownerId=" +
                    ownerId +
                    ", multiplier=" +
                    costMultiplier.ToString() +
                    ", priceState=" +
                    priceState +
                    ", result=" +
                    __result.ToString());
    }
}
