using BepInEx.Configuration;

namespace OmegasEquipmentMerchants
{
    internal static class OmegasEquipmentMerchantsConfig
    {
        internal static ConfigEntry<int> CostMultiplier;
        
        internal static void LoadConfig(ConfigFile config)
        {
            CostMultiplier = config.Bind(
                section: ModInfo.Name,
                key: "Cost Multiplier",
                defaultValue: 1,
                description: "Set the multiplier for item costs in the shop.\n" +
                             "Must be an integer value (e.g., 2 for double cost, 3 for triple cost).\n" +
                             "店のアイテムの価格の倍率を設定します。\n" +
                             "整数値である必要があります（例: 2 は価格が2倍になります、3 は価格が3倍になります）。\n" +
                             "设置商店商品价格的倍增器。\n" +
                             "必须为整数值（例如，2 表示价格翻倍，3 表示价格为三倍）。"
            );
        }
    }
}