using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryEntry {
    public ItemBase item;
    public int quantity;
    public int maxQuantity = 5;
    public InventoryEntry(ItemBase _item, int _quantity, int _maxQuantity = 5) {
        item = _item;
        quantity = _quantity;
        maxQuantity = _maxQuantity;
    }
}

public class PlayerItemHandler : MonoBehaviour
{
    public Dictionary<int, InventoryEntry> battleInventory;
    void Start() {
        battleInventory = new Dictionary<int, InventoryEntry>();
    }
    public void Buff(ItemBuff buff) {//ItemBuff.BuffType buffType, float strength, float duration) {
        //List<float> buff = new float {0f, 0f, 0f};
        float[] buffAmount = {0f, 0f, 0f};
        buffAmount[(int)buff.buffType] += buff.strength;
        GetComponent<PlayerScript>().damage += buffAmount[0];
        GetComponent<PlayerScript>().moveSpeed += buffAmount[1];
        GetComponent<PlayerScript>().fireRate -= buffAmount[2];
        StartCoroutine(BuffTimer(buff.duration, buffAmount));
    }

    IEnumerator BuffTimer(float seconds, float[] buffAmount) {
        yield return new WaitForSeconds(seconds);
        GetComponent<PlayerScript>().damage -= buffAmount[0];
        GetComponent<PlayerScript>().moveSpeed -= buffAmount[1];
        GetComponent<PlayerScript>().fireRate += buffAmount[2];
    }

    public bool Pickup(ItemBase item) {
        int quantity = 1;
        if(item is ItemCollectable) {
            ItemCollectable collectable = (ItemCollectable)item;
            quantity = collectable.quantity;
        }
        // Check if exist in inventory
        if(battleInventory.ContainsKey(item.id)) {
            InventoryEntry entry = battleInventory[item.id];
            if (entry.quantity < entry.maxQuantity) {
                entry.quantity += quantity;
                if (entry.quantity > entry.maxQuantity) {
                    entry.quantity = entry.maxQuantity;
                }
                battleInventory[item.id] = entry;
                BattleEvents.battleEvents.TriggerItemPickup();
                return true;
            }
            return false;
        } else {
            InventoryEntry entry = new InventoryEntry(item, quantity);
            battleInventory.Add(item.id, entry);
            BattleEvents.battleEvents.TriggerItemPickup();
            return true;
        }
    }

    public void Use(ItemUsable item) {
        if(item.battleUsable) {
            PlayerScript player = GetComponent<PlayerScript>();
            switch(item.usableType) {
                case (ItemUsable.UsableType.medkit) :
                player.Heal(item.strength);
                break;
                case (ItemUsable.UsableType.ammokit) :
                player.Reload(item.strength);
                break;
                case (ItemUsable.UsableType.bomb) :
                //Bomb behavior
                break;
            }
            BattleEvents.battleEvents.TriggerItemUsed();
        }
    }
}
