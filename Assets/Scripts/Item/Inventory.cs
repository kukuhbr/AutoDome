
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

    public Inventory GetItemOnlyType(string type) {
        int usable = this.maxUsable;
        int collectable = this.maxCollectable;
        int parts = this.maxParts;
        Inventory clone = new Inventory(usable, collectable, parts);
        foreach (var entry in items) {
            ItemBase item = Database.database.databaseItem.GetItemById(entry.Key);
            bool isTypeMatch = false;
            switch(type)
            {
                case "usable":
                isTypeMatch = (item is ItemUsable);
                break;
                case "collectable":
                isTypeMatch = (item is ItemCollectable);
                break;
            }
            if (isTypeMatch) {
                clone.Add(entry.Value);
            }
        }
        return clone;

    }

    public void Clear() {
        items.Clear();
    }
}

public class SlotInventory {
    public List<InventoryEntry> items;
    public Inventory inventory;
    public int maxQuantity;

    public SlotInventory(Inventory referenceInventory, int max = 3)
    {
        maxQuantity = max;
        items = new List<InventoryEntry>(6);
        for (int i = 0; i < 6; i++) {
            items.Add(new InventoryEntry(-1, 0, max));
        }
        inventory = referenceInventory;
        Debug.Log(items.Count);
    }

    public void ChangeReferenceInventory(Inventory referenceInventory)
    {
        inventory = referenceInventory;
    }

    public bool SetSlot(int id, int slot) {
        int remaining = inventory.GetEntry(id).quantity - SlotQuantityItem(id, slot);
        if (remaining > 0) {
            int quantity = remaining > maxQuantity ? maxQuantity : remaining;
            InventoryEntry entry = new InventoryEntry(id, quantity, maxQuantity);
            items[slot] = entry;
            return true;
        }
        InventoryEntry empty = new InventoryEntry(-1, 0, maxQuantity);
        items[slot] = empty;
        return false;
    }

    public void UnsetSlot(int slot)
    {
        items[slot].quantity = 0;
    }

    public void UseSlot(int slot)
    {
        if(items[slot].id == -1) return;
        if(items[slot].quantity == 0) return;
        //inventory.Remove(items[slot].id, 1);
        items[slot].quantity -= 1;
    }

    public int SlotQuantityItem(int id, int slot)
    {
        int sum = 0;
        for(int i = 0; i < items.Count; i++) {
            if (i == slot) continue;
            InventoryEntry entry = items[i];
            if (entry.id == id) {
                sum += entry.quantity;
            }
        }
        return sum;
    }

    public int FindAvailableSlot(int id)
    {
        // Find Item Of Same Type
        for(int i = 0; i < items.Count; i++) {
            InventoryEntry entry = items[i];
            if (entry.id != id) continue;
            if (entry.quantity < entry.maxQuantity) {
                return i;
            }
        }
        // Find First Empty Slot
        for(int i = 0; i < items.Count; i++) {
            InventoryEntry entry = items[i];
            if (entry.quantity == 0) return i;
        }
        return -1;
    }

    public bool IsEmpty()
    {
        for (int i = 0; i < items.Count; i++) {
            InventoryEntry entry = items[i];
            if (entry.quantity != 0) return false;
        }
        return true;
    }

    public void Recalculate()
    {
        for (int i = 0; i < items.Count; i++) {
            InventoryEntry entry = items[i];
            if (entry.quantity == 0) continue;
            SetSlot(entry.id, i);
        }
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

    public InventoryEntry(InventoryEntry copy) {
        id = copy.id;
        quantity = copy.quantity;
        maxQuantity = copy.maxQuantity;
        cooldown = copy.cooldown;
    }
}

