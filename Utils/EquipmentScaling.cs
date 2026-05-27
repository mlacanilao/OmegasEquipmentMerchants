using UnityEngine;

namespace OmegasEquipmentMerchants;

internal readonly struct EquipmentScalingResult
{
    internal EquipmentScalingResult(int shopLv, int depthLv, int fameLv, int fameShopLv, int genLv)
    {
        ShopLv = shopLv;
        DepthLv = depthLv;
        FameLv = fameLv;
        FameShopLv = fameShopLv;
        GenLv = genLv;
    }

    internal int ShopLv { get; }

    internal int DepthLv { get; }

    internal int FameLv { get; }

    internal int FameShopLv { get; }

    internal int GenLv { get; }
}

internal static class EquipmentScaling
{
    private const int MinGenLevel = 1;
    private const int MaxSafeGenLevel = 1_900_000_000;
    private const int TravelMerchantFameBase = 10;

    internal static EquipmentScalingResult GetResult(int shopLv)
    {
        int depthLv = EClass.player.stats.deepest;
        int fameLv = EClass.pc.FameLv;
        int fameShopLv = fameLv + fameLv / 2 + TravelMerchantFameBase;
        int highestLv = Mathf.Max(a: Mathf.Max(a: shopLv, b: depthLv), b: fameShopLv);
        int genLv = Mathf.Clamp(value: highestLv, min: MinGenLevel, max: MaxSafeGenLevel);

        return new EquipmentScalingResult(
            shopLv: shopLv,
            depthLv: depthLv,
            fameLv: fameLv,
            fameShopLv: fameShopLv,
            genLv: genLv);
    }
}
