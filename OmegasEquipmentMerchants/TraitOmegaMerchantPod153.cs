using System.Collections.Generic;

namespace OmegasEquipmentMerchants;

internal class TraitOmegaMerchantPod153 : TraitMerchant
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

        List<CardRow> allArmors = SpawnListThing.Get(id: "cat_armor", func: (SourceThing.Row s) =>
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

            return category.IsChildOf(id: "armor");
        }).rows;

        HashSet<string> excludeCategories = new HashSet<string> { "toolbelt", "lightsource" };
        int generatedCount = 0;
        int skippedCount = 0;

        foreach (var armorRow in allArmors)
        {
            if (excludeCategories.Contains(item: armorRow.category))
            {
                skippedCount++;
                continue;
            }

            CardBlueprint.Set(_bp: new CardBlueprint
            {
                rarity = Rarity.Mythical
            });

            Thing armor = ThingGen.Create(id: armorRow.id, lv: scaling.GenLv);

            armor.c_IDTState = 0;

            inventory?.AddThing(t: armor);
            generatedCount++;
        }

        MerchantInventory.FitHeight(inventory: inventory);

        FeatureTestLog.Log(
            feature: "Armor Merchant Stock",
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
