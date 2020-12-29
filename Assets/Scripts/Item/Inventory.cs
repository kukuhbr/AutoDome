
using System.Collections.Generic;
using System;
using UnityEngine;

public class Inventory {
    public Dictionary<int, InventoryEntry> items;
    public int maxUsable = 99;
    private int maxCollectable = 99;
    private int maxParts = 99999;
    private int maxEnergy = 5;

    public Inventory() {
        items = new Dictionary<int, InventoryEntry>();
    }

    public Inventory(int usable, int collectable, int parts = 99999, int energy = 5) {
        maxUsable = usable;
        maxCollectable = collectable;
        maxParts = parts;
        maxEnergy = energy;
        items = new Dictionary<int, InventoryEntry>();
    }

    int GetMaxQuantity(int id) {
        if (id == 9) return maxParts;
        if (id == 10) return maxEnergy;
        ItemBase item = Database.database.databaseItem.GetItemById(id);
        if(item is ItemCollectable) {
            return maxCollectable;
        } else if (item is ItemUsable) {
            return maxUsable;
        }
        return 0;
    }

    public bool Add(int id, int quantity) {
        bool inventoryEnough = true;
        DatabaseItem databaseItem = Database.database.databaseItem;
        if(items.ContainsKey(id)) {
            items[id].quantity += quantity;
            if (items[id].quantity > items[id].maxQuantity) {
                items[id].quantity = items[id].maxQuantity;
                inventoryEnough = false;
            }
        } else {
            int maxQuantity = GetMaxQuantity(id);
            InventoryEntry entry = new InventoryEntry(id, quantity, maxQuantity);
            items.Add(id, entry);
        }
        return inventoryEnough;
    }

    public void ForceAdd(int id, int quantity) {
        DatabaseItem databaseItem = Database.database.databaseItem;
        if(items.ContainsKey(id)) {
            items[id].quantity += quantity;
        } else {
            int maxQuantity = GetMaxQuantity(id);
            InventoryEntry entry = new InventoryEntry(id, quantity, maxQuantity);
            items.Add(id, entry);
            items[id].quantity = quantity;
        }
    }

    public bool Add(InventoryEntry entry) {
        bool inventoryEnough = true;
        if(items.ContainsKey(entry.id)) {
            inventoryEnough = Add(entry.id, entry.quantity);
        } else {
            items.Add(entry.id, entry);
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

    public void ForceAdd(List<int> itemIds, List<int> itemQuantities) {
        for(int i = 0; i < itemIds.Count; i++) {
            int id = itemIds[i];
            ForceAdd(id, itemQuantities[i]);
        }
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

    public bool Remove(int id, int quantity) {
        if(HaveItems(new List<int>{id}, new List<int>{quantity})) {
            items[id].quantity -= quantity;
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

    public Tuple<List<int>, List<int>> makeTuple() {
        List<int> ids = new List<int>();
        List<int> quantities = new List<int>();
        foreach(KeyValuePair<int, InventoryEntry> entry in items) {
            ids.Add(entry.Key);
            quantities.Add(entry.Value.quantity);
        }
        return new Tuple<List<int>, List<int>>(ids, quantities);
    }

    public void Clear() {
        items.Clear();
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

