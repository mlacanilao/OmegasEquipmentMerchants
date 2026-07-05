using System.Collections.Generic;
using OmegasEquipmentMerchants;

public class TraitOmegaMerchantPod042 : TraitMerchant
{
    public override ShopType ShopType
    {
        get
        {
            return ShopType.Specific;
        }
    }

    public override void OnBarter(bool reroll = false)
    {
        Card? merchantOwner = owner;
        if (merchantOwner is null)
        {
            return;
        }

        FeatureTestLog.Log(
            feature: "Merchant Trait Dispatch",
            detail: FeatureTestLog.FormatOwner(owner: merchantOwner) + ", trait=" + GetType().Name);

        bool shouldGenerateCustomStock = ShouldGenerateCustomStock(merchantOwner: merchantOwner);
        base.OnBarter(reroll: reroll);

        Thing? inventory = MerchantInventory.Prepare(owner: merchantOwner);
        if (inventory is null)
        {
            return;
        }

        try
        {
            if (shouldGenerateCustomStock == false)
            {
                return;
            }

            EquipmentScalingResult scaling = EquipmentScaling.GetResult(shopLv: ShopLv);
            int beforeCount = inventory.things?.Count ?? 0;

            List<CardRow> allWeapons = SpawnListThing.Get(id: "cat_weapon", func: (SourceThing.Row s) =>
            {
                if (s.category.IsEmpty())
                {
                    return false;
                }

                SourceCategory.Row? category = EClass.sources.categories.map.TryGetValue(key: s.category);
                if (category is null)
                {
                    return false;
                }

                return category.IsChildOf(id: "weapon");
            }).rows;

            HashSet<string> excludeCategories = new HashSet<string> { "ammo", "throw" };
            int generatedCount = 0;
            int skippedCount = 0;

            foreach (var weaponRow in allWeapons)
            {
                if (excludeCategories.Contains(item: weaponRow.category))
                {
                    skippedCount++;
                    continue;
                }

                CardBlueprint.Set(_bp: new CardBlueprint
                {
                    rarity = Rarity.Mythical
                });

                Thing weapon = ThingGen.Create(id: weaponRow.id, lv: scaling.GenLv);

                weapon.c_IDTState = 0;

                inventory.AddThing(t: weapon);
                generatedCount++;
            }

            FeatureTestLog.Log(
                feature: "Weapon Merchant Stock",
                detail: FeatureTestLog.FormatOwner(owner: merchantOwner) +
                        ", " +
                        FeatureTestLog.FormatStockResult(
                            beforeCount: beforeCount,
                            generatedCount: generatedCount,
                            skippedCount: skippedCount,
                            afterCount: inventory.things?.Count ?? 0,
                            scaling: scaling));
        }
        finally
        {
            MerchantInventory.FitHeight(inventory: inventory);
        }
    }

    private bool ShouldGenerateCustomStock(Card merchantOwner)
    {
        if (EClass.world.date.IsExpired(merchantOwner.c_dateStockExpire) == false)
        {
            return false;
        }

        if (RestockDay < 0 && merchantOwner.isRestocking)
        {
            return false;
        }

        return true;
    }
}
