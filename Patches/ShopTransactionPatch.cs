namespace OmegasEquipmentMerchants;

internal static class ShopTransactionPatch
{
    private const string WeaponMerchantId = "pod042";
    private const string ArmorMerchantId = "pod153";
    private const int MaxValidTransactionPrice = int.MaxValue - 1;

    internal static void GetPricePostfix(ShopTransaction __instance, Thing thing, int count, bool sell, ref int __result)
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

        InvOwner trader = __instance.trader;
        if (trader is null)
        {
            return;
        }

        if (thing is null)
        {
            return;
        }

        string? ownerId = trader.owner?.id;
        if (ownerId != WeaponMerchantId && ownerId != ArmorMerchantId)
        {
            return;
        }

        int costMultiplier = OmegasEquipmentMerchantsConfig.GetCostMultiplier();
        long vanillaBuybackTotal = GetSameTransactionBuybackTotal(
            transaction: __instance,
            thing: thing,
            count: count,
            trader: trader);
        long merchantStockTotal = (long)__result - vanillaBuybackTotal;
        if (merchantStockTotal < 0L)
        {
            return;
        }

        long multipliedPrice = vanillaBuybackTotal + merchantStockTotal * costMultiplier;
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

    private static long GetSameTransactionBuybackTotal(
        ShopTransaction transaction,
        Thing thing,
        int count,
        InvOwner trader)
    {
        if (count <= 0)
        {
            return 0L;
        }

        int remainingCount = count;
        long buybackTotal = 0L;
        long vanillaSellPrice = thing.GetPrice(currency: trader.currency, sell: true, priceType: trader.priceType);
        foreach (var item in transaction.sold)
        {
            if (item is null)
            {
                continue;
            }

            if (item.thing == null)
            {
                continue;
            }

            if (item.thing.id == thing.id && (long)item.price == vanillaSellPrice)
            {
                int matchedCount = item.num;
                if (matchedCount > remainingCount)
                {
                    matchedCount = remainingCount;
                }

                remainingCount -= matchedCount;
                buybackTotal += (long)matchedCount * vanillaSellPrice;
            }

            if (remainingCount == 0)
            {
                break;
            }
        }

        return buybackTotal;
    }
}
