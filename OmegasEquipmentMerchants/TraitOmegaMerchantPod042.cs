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

        var allWeapons = SpawnListThing.Get(id: "cat_weapon", func: (SourceThing.Row s) => 
            EClass.sources.categories.map[key: s.category].IsChildOf(id: "weapon")).rows;

        HashSet<string> excludeCategories = new HashSet<string> { "ammo", "throw" };

        foreach (var weaponRow in allWeapons)
        {
            if (excludeCategories.Contains(weaponRow.category))
            {
                continue;
            }
            
            CardBlueprint.Set(_bp: new CardBlueprint
            {
                rarity = Rarity.Mythical
            });

            Thing weapon = ThingGen.Create(id: weaponRow.id, lv: genLv);

            weapon.c_IDTState = 0;

            inventory?.AddThing(t: weapon);
        }
    }
}