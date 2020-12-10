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
            itemIcons[i].GetComponent<ItemUIUsableScript>().AssignItem(entry.Value);
            // itemIcons[i].GetComponent<Image>().sprite = entry.Value.item.icon;
            // TextMeshProUGUI itemAmount = itemIcons[i].GetComponentInChildren<TextMeshProUGUI>();
            // itemAmount.text = string.Format("{0}/{1}", entry.Value.quantity, entry.Value.maxQuantity);
            // itemIcons[i].SetActive(true);
            i += 1;
        }
    }

    void OnDestroy()
    {
        BattleEvents.battleEvents.onItemPickup -= AdjustItemUI;
        BattleEvents.battleEvents.onItemUsed -= AdjustItemUI;
    }
}
