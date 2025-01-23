using BepInEx;
using HarmonyLib;

namespace OmegasEquipmentMerchants
{
    internal static class ModInfo
    {
        internal const string Guid = "omegaplatinum.elin.omegasequipmentmerchants";
        internal const string Name = "Omegas Equipment Merchants";
        internal const string Version = "1.2.1.0";
    }

    [BepInPlugin(GUID: ModInfo.Guid, Name: ModInfo.Name, Version: ModInfo.Version)]
    internal class OmegasEquipmentMerchants : BaseUnityPlugin
    {
        internal static OmegasEquipmentMerchants Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            
            OmegasEquipmentMerchantsConfig.LoadConfig(config: Config);
            
            Harmony.CreateAndPatchAll(type: typeof(Patcher), harmonyInstanceId: ModInfo.Guid);
        }
        
        internal static void Log(object payload)
        {
            Instance?.Logger.LogInfo(data: payload);
        }
    }
}