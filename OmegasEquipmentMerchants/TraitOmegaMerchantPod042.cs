using System.Collections.Generic;
using UnityEngine;

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

        Thing? inventory = owner?.things?.Find(id: "chest_merchant");
        if (inventory is null)
        {
            inventory = ThingGen.Create(id: "chest_merchant");
            owner?.AddThing(t: inventory);
            FeatureTestLog.Log(
                feature: "Merchant Chest",
                detail: FeatureTestLog.FormatOwner(owner: owner) + ", created chest_merchant.");
        }

        EquipmentScalingResult scaling = EquipmentScaling.GetResult(shopLv: ShopLv);

        int beforeCount = inventory?.things?.Count ?? 0;
        ThingContainer? items = inventory?.things;
        int inventoryWidth = OmegasEquipmentMerchantsConfig.GetMerchantInventoryWidth();

        if (items is not null && items.width != inventoryWidth)
        {
            items.ChangeSize(w: inventoryWidth, h: items.height);
        }

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

        if (items is not null)
        {
            int width = Mathf.Max(a: 1, b: items.width);
            int neededHeight = Mathf.Max(a: 1, b: (items.Count + width - 1) / width);

            if (items.height != neededHeight)
            {
                items.ChangeSize(w: width, h: neededHeight);
            }
        }

        FeatureTestLog.Log(
            feature: "Weapon Merchant Stock",
            detail: FeatureTestLog.FormatOwner(owner: owner) +
                    ", " +
                    FeatureTestLog.FormatStockResult(
                        beforeCount: beforeCount,
                        generatedCount: generatedCount,
                        skippedCount: skippedCount,
                        afterCount: items?.Count ?? 0,
                        scaling: scaling));
    }
}
