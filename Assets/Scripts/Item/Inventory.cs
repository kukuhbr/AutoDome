
using System.Collections.Generic;
using UnityEngine;

public class Inventory {
    public Dictionary<int, InventoryEntry> items;

    public Inventory() {
        items = new Dictionary<int, InventoryEntry>();
    }

    public void SetMaxQuantity(List<int> itemIds, List<int> maxQuantities) {
        for(int i = 0; i < itemIds.Count; i++) {
            int id = itemIds[i];
            if(items.ContainsKey(id)) {
                items[id].maxQuantity = maxQuantities[i];
                if(items[id].quantity > items[id].maxQuantity)
                    items[id].quantity = items[id].maxQuantity;
            } else {
                InventoryEntry entry = new InventoryEntry(id, 0, maxQuantities[i]);
                items.Add(id, entry);
            }
        }
    }

    public bool Add(int id, int quantity) {
        bool inventoryEnough = true;
        DatabaseItem databaseItem = Resources.Load<DatabaseItem>("DatabaseItem");
        if(items.ContainsKey(id)) {
            items[id].quantity += quantity;
            if (items[id].quantity > items[id].maxQuantity) {
                items[id].quantity = items[id].maxQuantity;
                inventoryEnough = false;
            }
        } else {
            int maxQuantity = databaseItem.GetItemById(id).maxQuantity;
            InventoryEntry entry = new InventoryEntry(id, quantity, maxQuantity);
            items.Add(id, entry);
        }
        return inventoryEnough;
    }

    public bool Add(List<int> itemIds, List<int> itemQuantities) {
        bool inventoryEnough = true;
        for(int i = 0; i < itemIds.Count; i++) {
            int id = itemIds[i];
            inventoryEnough = inventoryEnough & Add(id, itemQuantities[i]);
        }
        return inventoryEnough;
    }

    public bool Remove(List<int> itemIds, List<int> itemQuantities) {
        if(HaveItems(itemIds, itemQuantities)) {
            for(int i = 0; i < itemIds.Count; i++) {
                int id = itemIds[i];
                items[id].quantity -= itemQuantities[i];
            }
            return true;
        } else {
            return false;
        }
    }

    public bool HaveItems(List<int> itemIds, List<int> itemQuantities) {
        for(int i = 0; i < itemIds.Count; i++) {
            int id = itemIds[i];
            if(items.ContainsKey(id)) {
                if(items[id].quantity < itemQuantities[i]) {
                    return false;
                }
            } else {
                return false;
            }
        }
        return true;
    }

    public bool HaveItem(int id) {
        return(items.ContainsKey(id));
    }

    public InventoryEntry GetEntry(int id) {
        if(items.ContainsKey(id)) {
            return items[id];
        }
        return new InventoryEntry(-1, -1, -1);
    }
}

public class InventoryEntry {
    public int id;
    public int quantity;
    public int maxQuantity;
    public float cooldown;
    public InventoryEntry(int _id, int _quantity, int _maxQuantity) {
        id = _id;
        quantity = _quantity;
        maxQuantity = _maxQuantity;
        cooldown = 0f;
    }
}

