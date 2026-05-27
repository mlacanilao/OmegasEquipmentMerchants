using System.Runtime.CompilerServices;
using BepInEx;
using HarmonyLib;

namespace OmegasEquipmentMerchants;

internal static class ModInfo
{
    internal const string Guid = "omegaplatinum.elin.omegasequipmentmerchants";
    internal const string Name = "Omegas Equipment Merchants";
    internal const string Version = "3.0.0";
}

[BepInPlugin(GUID: ModInfo.Guid, Name: ModInfo.Name, Version: ModInfo.Version)]
internal class OmegasEquipmentMerchants : BaseUnityPlugin
{
    internal static OmegasEquipmentMerchants? Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        OmegasEquipmentMerchantsConfig.LoadConfig(config: Config);
        FeatureTestLog.Log(
            feature: "Bootstrap",
            detail: "config loaded; costMultiplier=" +
                    (OmegasEquipmentMerchantsConfig.CostMultiplier?.Value.ToString() ?? "<missing>") +
                    ", merchantInventoryWidth=" +
                    (OmegasEquipmentMerchantsConfig.MerchantInventoryWidth?.Value.ToString() ?? "<missing>"));
        Harmony.CreateAndPatchAll(type: typeof(Patcher), harmonyInstanceId: ModInfo.Guid);
        FeatureTestLog.Log(feature: "Bootstrap", detail: "Harmony patches registered through Patcher.");
    }

    internal static void LogDebug(object message, [CallerMemberName] string caller = "")
    {
        Instance?.Logger.LogDebug(data: $"[{caller}] {message}");
    }

    internal static void LogInfo(object message)
    {
        Instance?.Logger.LogInfo(data: message);
    }

    internal static void LogError(object message)
    {
        Instance?.Logger.LogError(data: message);
    }
}
