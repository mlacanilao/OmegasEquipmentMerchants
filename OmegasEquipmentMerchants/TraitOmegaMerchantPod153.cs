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

        var allArmors = SpawnList.Get(id: "eq").rows;
        
        HashSet<string> armorCategories = new HashSet<string>
        {
            "head", "torso", "arm", "waist", "foot", "back",
            "shield", "amulet", "ring"
        };

        foreach (var armorRow in allArmors)
        {
            if (!armorCategories.Contains(armorRow.category))
            {
                continue;
            }
            
            CardBlueprint.Set(new CardBlueprint
            {
                rarity = Rarity.Mythical
            });

            Thing armor = ThingGen.Create(id: armorRow.id, lv: genLv);

            armor.c_IDTState = 0;
            
            inventory?.AddThing(t: armor);
        }
    }
}