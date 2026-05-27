using BepInEx.Configuration;

namespace OmegasEquipmentMerchants;

internal static class OmegasEquipmentMerchantsConfig
{
    internal const int MinCostMultiplier = 1;
    internal const int MaxCostMultiplier = 100;
    internal const int MinMerchantInventoryWidth = 8;
    internal const int MaxMerchantInventoryWidth = 64;

    internal static ConfigEntry<int>? CostMultiplier { get; private set; }
    internal static ConfigEntry<int>? MerchantInventoryWidth { get; private set; }

    internal static void LoadConfig(ConfigFile config)
    {
        CostMultiplier = config.Bind(
            section: ModInfo.Name,
            key: "Cost Multiplier",
            defaultValue: 1,
            description: "Set the multiplier for buy prices from Pod 042 and Pod 153.\n" +
                             "Must be an integer value (e.g., 2 for double cost, 3 for triple cost).\n" +
                             "Sale prices stay vanilla.\n" +
                             "Values below 1 are treated as 1, and values above 100 are treated as 100.\n" +
                             "Final transaction prices above the game's integer limit are capped at 2147483646.\n" +
                             "ポッド042とポッド153から購入する価格の倍率を設定します。\n" +
                             "整数値である必要があります（例: 2 は価格が2倍になります、3 は価格が3倍になります）。\n" +
                             "売却価格はバニラのままです。\n" +
                             "1未満の値は1として扱われ、100を超える値は100として扱われます。\n" +
                             "ゲームの整数上限を超える最終取引価格は2147483646に制限されます。\n" +
                             "设置从辅助机042和辅助机153购买商品时的价格倍率。\n" +
                             "必须为整数值（例如，2 表示价格翻倍，3 表示价格为三倍）。\n" +
                             "出售价格保持原版。\n" +
                             "低于1的值按1处理，高于100的值按100处理。\n" +
                             "超过游戏整数上限的最终交易价格会限制为2147483646。"
        );

        MerchantInventoryWidth = config.Bind(
            section: ModInfo.Name,
            key: "Merchant Inventory Width",
            defaultValue: 8,
            description: "Target inventory width (number of columns) for Pod 042 and Pod 153 shop inventories.\n" +
                          "Values below 8 are treated as 8, and values above 64 are treated as 64. Inventory height grows or shrinks to fit generated stock.\n" +
                         "ポッド042とポッド153の店のインベントリ幅（列数）を設定します。\n" +
                          "8未満の値は8として扱われ、64を超える値は64として扱われます。生成された在庫に合わせて高さは増減します。\n" +
                         "设置辅助机042和辅助机153商店库存的目标宽度（列数）。\n" +
                          "低于8的值按8处理，高于64的值按64处理。库存高度会根据生成的商品数量增减。"
        );
    }

    internal static int GetCostMultiplier()
    {
        int configuredMultiplier = CostMultiplier?.Value ?? MinCostMultiplier;

        if (configuredMultiplier < MinCostMultiplier)
        {
            return MinCostMultiplier;
        }

        if (configuredMultiplier > MaxCostMultiplier)
        {
            return MaxCostMultiplier;
        }

        return configuredMultiplier;
    }

    internal static int GetMerchantInventoryWidth()
    {
        int configuredWidth = MerchantInventoryWidth?.Value ?? MinMerchantInventoryWidth;

        if (configuredWidth < MinMerchantInventoryWidth)
        {
            return MinMerchantInventoryWidth;
        }

        if (configuredWidth > MaxMerchantInventoryWidth)
        {
            return MaxMerchantInventoryWidth;
        }

        return configuredWidth;
    }
}
