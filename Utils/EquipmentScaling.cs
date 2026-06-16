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
        long fameShopLvLong = (long)fameLv + fameLv / 2L + TravelMerchantFameBase;
        long highestLv = GetHighestLevel(
            shopLv: shopLv,
            depthLv: depthLv,
            fameShopLv: fameShopLvLong);
        int fameShopLv = ClampToInt(value: fameShopLvLong);
        int genLv = ClampGenerationLevel(value: highestLv);

        return new EquipmentScalingResult(
            shopLv: shopLv,
            depthLv: depthLv,
            fameLv: fameLv,
            fameShopLv: fameShopLv,
            genLv: genLv);
    }

    private static long GetHighestLevel(int shopLv, int depthLv, long fameShopLv)
    {
        long highestLv = shopLv;
        if (depthLv > highestLv)
        {
            highestLv = depthLv;
        }

        if (fameShopLv > highestLv)
        {
            highestLv = fameShopLv;
        }

        return highestLv;
    }

    private static int ClampGenerationLevel(long value)
    {
        if (value < MinGenLevel)
        {
            return MinGenLevel;
        }

        if (value > MaxSafeGenLevel)
        {
            return MaxSafeGenLevel;
        }

        return (int)value;
    }

    private static int ClampToInt(long value)
    {
        if (value < int.MinValue)
        {
            return int.MinValue;
        }

        if (value > int.MaxValue)
        {
            return int.MaxValue;
        }

        return (int)value;
    }
}
