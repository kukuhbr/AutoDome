using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemUIScript : MonoBehaviour
{
    public PlayerItemHandler handler;
    private List<GameObject> itemIcons;
    [SerializeField]
    private GameObject itemIconPrefab;

    void Start() {
        itemIcons = new List<GameObject>();
        for(int i = 0; i < 6; i++) {
            GameObject temp = Instantiate(itemIconPrefab, this.transform);
            temp.SetActive(false);
            itemIcons.Add(temp);
        }
        BattleEvents.battleEvents.onItemPickup += AdjustItemUI;
        BattleEvents.battleEvents.onItemUsed += AdjustItemUI;
    }

    void AdjustItemUI()
    {
        // Bad if item number > GameObject List length
        int i = 0;
        foreach(KeyValuePair<int, InventoryEntry> entry in handler.battleInventory) {
            if (i < 6 && entry.Value.quantity > 0) {
                itemIcons[i].GetComponent<ItemUIUsableScript>().AssignItem(entry.Value);
                i += 1;
            }
        }
        for (; i < 6; i++) {
            itemIcons[i].GetComponent<ItemUIUsableScript>().AssignItem(null);
        }
    }

    void OnDestroy()
    {
        BattleEvents.battleEvents.onItemPickup -= AdjustItemUI;
        BattleEvents.battleEvents.onItemUsed -= AdjustItemUI;
    }
}
