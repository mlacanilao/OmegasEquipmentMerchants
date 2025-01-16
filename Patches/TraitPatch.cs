using System;
using UnityEngine;

namespace OmegasEquipmentMerchants
{
    internal class TraitPatch
    {
        internal static void OnBarterPostfix(Trait __instance)
        {
            if (EClass.core?.IsGameStarted == false ||
                __instance.ShopType != ShopType.Specific ||
                (__instance.owner.id != "pod042" && __instance.owner.id != "pod153"))
            {
                return;
            }

            var inventory = __instance.owner.things.Find(id: "chest_merchant");
            int shopLv = __instance.ShopLv;
            int voidLv = Mathf.Abs(value: EClass.game.spatials.Find(id: "void").GetDeepestLv()) + 50;
            int depthLv = EClass.player.stats.deepest;
            
            int genLv = Mathf.Max(shopLv, voidLv, depthLv);

            if (inventory is null)
            {
                return;
            }

            foreach (Thing item in inventory.things)
            {
                if (item.rarity == Rarity.Mythical)
                {
                    continue;
                }

                item.ChangeRarity(q: Rarity.Mythical);
                
                if (item.category.slot != 0)
                {
                    int num = EClass.rnd(a: 3) + 5;
                    for (int i = 0; i < num; i++)
                    {
                        item.AddEnchant(lv: genLv);
                    }
                }

                if (item.IsRangedWeapon && !item.IsMeleeWithAmmo)
                {
                    int num = (EClass.rnd(a: 10) == 0) ? 1 : 0;
                    num = EClass.rnd(a: 2) + 4 + num;
                    for (int i = 0; i < num; i++)
                    {
                        item.AddSocket();
                    }

                    for (int i = 0; i < EClass.rnd(a: num + 1); i++)
                    {
                        Tuple<SourceElement.Row, int> enchant = Thing.GetEnchant(lv: genLv,
                            func: (SourceElement.Row r) => r.tag.Contains(id: "modRanged"), neg: false);
                        if (enchant != null && InvOwnerMod.IsValidRangedMod(t: item, row: enchant.Item1))
                        {
                            item.ApplySocket(id: enchant.Item1.id, lv: enchant.Item2, mod: null);
                        }
                    }
                }
            }
        }
    }
}