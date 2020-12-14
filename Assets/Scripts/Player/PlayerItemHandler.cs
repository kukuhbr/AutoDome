using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryEntry {
    public ItemBase item;
    public int quantity;
    public int maxQuantity = 99;
    public float cooldown;
    public InventoryEntry(ItemBase _item, int _quantity, int _maxQuantity = 5) {
        item = _item;
        quantity = _quantity;
        maxQuantity = _maxQuantity;
    }
}

public class PlayerItemHandler : MonoBehaviour
{
    public Dictionary<int, InventoryEntry> battleInventory;
    [SerializeField]
    private List<GameObject> buffVisualPrefab;
    void Start() {
        battleInventory = new Dictionary<int, InventoryEntry>();
    }
    public void Buff(ItemBuff buff) {//ItemBuff.BuffType buffType, float strength, float duration) {
        //List<float> buff = new float {0f, 0f, 0f};
        float[] buffAmount = {0f, 0f, 0f};
        buffAmount[(int)buff.buffType] += buff.strength;
        GameObject buffVisual = null;
        if(buffVisualPrefab[(int)buff.buffType]) {
            Debug.Log("Call Buff " + buff.buffType + " " + (int)buff.buffType);
            buffVisual = Instantiate(buffVisualPrefab[(int)buff.buffType], this.transform);
        }
        GetComponent<PlayerScript>().damage += buffAmount[0];
        GetComponent<PlayerScript>().moveSpeed += buffAmount[1];
        GetComponent<PlayerScript>().fireRate -= buffAmount[2];
        StartCoroutine(BuffTimer(buff.duration, buffAmount, buffVisual));
    }

    IEnumerator BuffTimer(float seconds, float[] buffAmount, GameObject buffVisual) {
        yield return new WaitForSeconds(seconds);
        GetComponent<PlayerScript>().damage -= buffAmount[0];
        GetComponent<PlayerScript>().moveSpeed -= buffAmount[1];
        GetComponent<PlayerScript>().fireRate += buffAmount[2];
        if(buffVisual) {
            Destroy(buffVisual);
        }
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

    public void ReduceItem(ItemBase item) {
        if(battleInventory.ContainsKey(item.id)) {
            battleInventory[item.id].quantity -= 1;
        }
    }

    public void Use(ItemUsable item, bool fromInventory) {
        if(item.battleUsable) {
            if(battleInventory[item.id].cooldown == 0f) {
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
                if (fromInventory) {
                    ReduceItem(item);
                }
                StartCoroutine(Cooldown(4f, battleInventory[item.id]));
                BattleEvents.battleEvents.TriggerItemUsed();
            }
        }
    }

    IEnumerator Cooldown(float seconds, InventoryEntry entry) {
        float timeLeft = seconds;
        while(timeLeft > 0) {
            timeLeft -= Time.deltaTime;
            entry.cooldown = timeLeft;
            yield return null;
        }
        entry.cooldown = 0f;
        Debug.Log("Cooldown is done!");
    }
}
