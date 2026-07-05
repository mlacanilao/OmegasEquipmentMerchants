using System;

namespace OmegasEquipmentMerchants;

internal static class MerchantInventory
{
    internal static Thing? Prepare(Card? owner)
    {
        if (owner is null)
        {
            return null;
        }

        int inventoryWidth = OmegasEquipmentMerchantsConfig.GetMerchantInventoryWidth();

        var ownerThings = owner.things;
        if (ownerThings is null)
        {
            return null;
        }

        Thing inventory = ownerThings.Find(id: "chest_merchant");
        if (inventory is null)
        {
            inventory = ThingGen.Create(id: "chest_merchant");
            owner.AddThing(t: inventory);
            FeatureTestLog.Log(
                feature: "Merchant Chest",
                detail: FeatureTestLog.FormatOwner(owner: owner) + ", created chest_merchant.");
        }

        var items = inventory.things;
        if (items != null && items.width != inventoryWidth)
        {
            items.ChangeSize(w: inventoryWidth, h: items.height);
        }

        return inventory;
    }

    internal static void FitHeight(Thing? inventory)
    {
        var items = inventory?.things;
        if (items is null)
        {
            return;
        }

        int width = Math.Max(val1: 1, val2: items.width);
        long neededHeightLong = ((long)items.Count + width - 1L) / width;
        int neededHeight = (int)Math.Min(
            val1: int.MaxValue,
            val2: Math.Max(val1: 1L, val2: neededHeightLong));

        if (items.height != neededHeight)
        {
            items.ChangeSize(w: width, h: neededHeight);
        }
    }
}
