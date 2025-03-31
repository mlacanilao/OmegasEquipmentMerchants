using System.Collections.Generic;
using UnityEngine;

class TraitOmegaMerchantPod153 : TraitMerchant
{
    public override ShopType ShopType
    {
        get
        {
            return ShopType.Specific;
        }
    }

    void _OnBarter()
    {
        var inventory = this.owner?.things?.Find(id: "chest_merchant");
        if (inventory is null)
        {
            inventory = ThingGen.Create(id: "chest_merchant");
            this.owner?.AddThing(t: inventory);
        }

        int shopLv = this.ShopLv;
        int depthLv = EClass.player.stats.deepest;
        int genLv = Mathf.Max(a: shopLv, b: depthLv);

        var allArmors = SpawnListThing.Get(id: "cat_armor", func: (SourceThing.Row s) => 
            EClass.sources.categories.map[key: s.category].IsChildOf(id: "armor")).rows;

        HashSet<string> excludeCategories = new HashSet<string> { "toolbelt", "lightsource" };

        foreach (var armorRow in allArmors)
        {
            if (excludeCategories.Contains(armorRow.category))
            {
                continue;
            }
            
            CardBlueprint.Set(_bp: new CardBlueprint
            {
                rarity = Rarity.Mythical
            });

            Thing armor = ThingGen.Create(id: armorRow.id, lv: genLv);

            armor.c_IDTState = 0;
        
            inventory?.AddThing(t: armor);
        }
        
        var items = inventory?.things;
        
        if (items?.Count > items?.GridSize) {
            items?.ChangeSize(w: items.width, h: items.Count / items.width + 1);
        }
    }
}