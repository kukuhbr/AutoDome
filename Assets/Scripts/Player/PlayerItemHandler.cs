using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemHandler : MonoBehaviour
{
    public Inventory battleInventory;
    [SerializeField]
    private List<GameObject> buffVisualPrefab;
    void Start() {
        // battleInventory = new Inventory(3, 5, 2000);
        // //Debug
        // for(int i = 1; i < 9; i++) {
        //     if (i == 9) {
        //         battleInventory.Add(i, 15000);
        //     } else if (i == 10) {
        //         battleInventory.Add(i, 0);
        //     } else {
        //         battleInventory.Add(i, 3);
        //     }
        // }
        battleInventory = PlayerManager.playerManager.playerData.battleInventory;
        foreach(KeyValuePair<int, InventoryEntry> entry in battleInventory.items) {
            int id = entry.Key;
            Database.database.databaseItem.GetItemById(id).SetHandler(this.gameObject);
        }
        BattleEvents.battleEvents.TriggerItemPickup();
    }
    public void Buff(ItemBuff buff) {//ItemBuff.BuffType buffType, float strength, float duration) {
        //List<float> buff = new float {0f, 0f, 0f};
        float[] buffAmount = {0f, 0f, 0f};
        buffAmount[(int)buff.buffType] += buff.strength;
        GameObject buffVisual = null;
        if(buffVisualPrefab[(int)buff.buffType]) {
            //Debug.Log("Call Buff " + buff.buffType + " " + (int)buff.buffType);
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
            quantity = collectable.dropQuantity;
        }
        Debug.Log("get " + item.name);
        bool isSuccessful = battleInventory.Add(item.id, quantity);
        BattleEvents.battleEvents.TriggerItemPickup();
        return(isSuccessful);
    }

    public void Use(ItemUsable item, bool fromInventory) {
        if(item.battleUsable) {
            if(battleInventory.GetEntry(item.id).cooldown == 0f) {
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
                    battleInventory.Remove(new List<int> {item.id}, new List<int> {1});
                }
                StartCoroutine(Cooldown(4f, battleInventory.GetEntry(item.id)));
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
