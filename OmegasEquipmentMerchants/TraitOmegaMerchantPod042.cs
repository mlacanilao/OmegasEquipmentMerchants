using System.Collections.Generic;

namespace OmegasEquipmentMerchants;

internal class TraitOmegaMerchantPod042 : TraitMerchant
{
    public override ShopType ShopType
    {
        get
        {
            return ShopType.Specific;
        }
    }

    private void _OnBarter()
    {
        FeatureTestLog.Log(
            feature: "Merchant Trait Dispatch",
            detail: FeatureTestLog.FormatOwner(owner: owner) + ", trait=" + GetType().Name);

        Thing? inventory = MerchantInventory.Prepare(owner: owner);
        EquipmentScalingResult scaling = EquipmentScaling.GetResult(shopLv: ShopLv);
        int beforeCount = inventory?.things?.Count ?? 0;

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

            inventory?.AddThing(t: weapon);
            generatedCount++;
        }

        MerchantInventory.FitHeight(inventory: inventory);

        FeatureTestLog.Log(
            feature: "Weapon Merchant Stock",
            detail: FeatureTestLog.FormatOwner(owner: owner) +
                    ", " +
                    FeatureTestLog.FormatStockResult(
                        beforeCount: beforeCount,
                        generatedCount: generatedCount,
                        skippedCount: skippedCount,
                        afterCount: inventory?.things?.Count ?? 0,
                        scaling: scaling));
    }
}
