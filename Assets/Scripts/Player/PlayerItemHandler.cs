using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemHandler : MonoBehaviour
{
    public Inventory battleInventory;
    [SerializeField]
    private List<GameObject> buffVisualPrefab;
    [SerializeField]
    private GameObject bombEffector;
    void Start() {
        battleInventory = PlayerManager.playerManager.playerData.battleInventory;
        foreach(KeyValuePair<int, InventoryEntry> entry in battleInventory.items) {
            int id = entry.Key;
            Database.database.databaseItem.GetItemById(id).SetHandler(this.gameObject);
        }
    }
    public void Buff(ItemBuff buff) {
        float[] buffAmount = {0f, 0f, 0f};
        buffAmount[(int)buff.buffType] += buff.strength;
        GameObject buffVisual = null;
        if(buffVisualPrefab[(int)buff.buffType]) {
            buffVisual = Instantiate(buffVisualPrefab[(int)buff.buffType], this.transform);
        }
        GetComponent<PlayerScript>().damage += buffAmount[0];
        GetComponent<PlayerScript>().moveSpeed += buffAmount[1];
        GetComponent<PlayerScript>().fireRate -= buffAmount[2];
        StartCoroutine(BuffTimer(buff.duration, buffAmount, buffVisual));
        GetComponent<SoundsManager>().PlaySFX(SoundsManager.SoundsEnum.character_buff);
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
        bool isSuccessful = battleInventory.Add(item.id, quantity);
        BattleEvents.battleEvents.TriggerItemPickup(item.id);
        SoundsManager.soundsManager.PlaySFX(SoundsManager.SoundsEnum.item_pickup);
        return(isSuccessful);
    }

    public void Use(ItemUsable item, bool fromInventory) {
        if(item.battleUsable) {
            if(battleInventory.GetEntry(item.id).cooldown == 0f) {
                PlayerScript player = GetComponent<PlayerScript>();
                switch(item.usableType) {
                    case (ItemUsable.UsableType.medkit) :
                    player.Heal(item.strength);
                    SoundsManager.soundsManager.PlaySFX(SoundsManager.SoundsEnum.use_medkit);
                    break;
                    case (ItemUsable.UsableType.ammokit) :
                    player.Reload(item.strength);
                    SoundsManager.soundsManager.PlaySFX(SoundsManager.SoundsEnum.use_ammo);
                    break;
                    case (ItemUsable.UsableType.bomb) :
                    player.SpawnBomb(bombEffector, item.strength);
                    SoundsManager.soundsManager.PlaySFX(SoundsManager.SoundsEnum.use_bomb, .6f);
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
    }
}
