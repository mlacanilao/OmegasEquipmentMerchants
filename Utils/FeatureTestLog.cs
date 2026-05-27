using System.Collections.Generic;

namespace OmegasEquipmentMerchants;

internal static class FeatureTestLog
{
    private static readonly HashSet<string> LoggedOnceKeys = new HashSet<string>();

    internal static void Log(string feature, string detail)
    {
        OmegasEquipmentMerchants.LogInfo(message: "[FeatureTest] " + feature + ": " + detail);
    }

    internal static void LogOnce(string feature, string key, string detail)
    {
        string combinedKey = feature + "|" + key;
        if (LoggedOnceKeys.Add(item: combinedKey) == false)
        {
            return;
        }

        Log(feature: feature, detail: detail);
    }

    internal static string FormatOwner(Card? owner)
    {
        if (owner == null)
        {
            return "owner=<null>";
        }

        return "ownerId=" +
               (owner.id ?? "<empty>") +
               ", ownerName=" +
               owner.NameSimple;
    }

    internal static string FormatStockResult(
        int beforeCount,
        int generatedCount,
        int skippedCount,
        int afterCount,
        EquipmentScalingResult scaling)
    {
        return "beforeCount=" +
               beforeCount.ToString() +
               ", generatedCount=" +
               generatedCount.ToString() +
               ", skippedCount=" +
               skippedCount.ToString() +
               ", afterCount=" +
               afterCount.ToString() +
               ", shopLv=" +
               scaling.ShopLv.ToString() +
               ", depthLv=" +
               scaling.DepthLv.ToString() +
               ", fameLv=" +
               scaling.FameLv.ToString() +
               ", fameShopLv=" +
               scaling.FameShopLv.ToString() +
               ", genLv=" +
               scaling.GenLv.ToString();
    }
}
