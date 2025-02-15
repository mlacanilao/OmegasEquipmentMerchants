using System.Collections.Generic;
using UnityEngine;

class TraitOmegaMerchantPod042 : TraitMerchant
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

        var allWeapons = SpawnList.Get(id: "eq").rows;
        
        HashSet<string> weaponCategories = new HashSet<string>
        {
            "dagger", "sword", "axe", "blunt", "polearm",
            "scythe", "staff", "martial", "cane", "bow",
            "crossbow", "gun"
        };

        foreach (var weaponRow in allWeapons)
        {
            if (!weaponCategories.Contains(weaponRow.category))
            {
                continue;
            }
            
            CardBlueprint.Set(new CardBlueprint
            {
                rarity = Rarity.Mythical
            });

            Thing weapon = ThingGen.Create(id: weaponRow.id, lv: genLv);

            weapon.c_IDTState = 0;
            
            inventory?.AddThing(t: weapon);
        }
    }
}