using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemUICollection : MonoBehaviour
{
    private List<GameObject> itemIcons = new List<GameObject>();
    [SerializeField]
    private GameObject itemIconPrefab;
    public bool showZero = false;
    public bool inInventoryOnly = false;

    void Awake() {
        for(int i = 0; i < 6; i++) {
            GameObject temp = Instantiate(itemIconPrefab, this.transform);
            temp.SetActive(false);
            itemIcons.Add(temp);
        }
    }

    public void AdjustItemCollectionUI(Inventory collection)
    {
        int i = 0;
        foreach(KeyValuePair<int, InventoryEntry> entry in collection.items) {
            if(i >= itemIcons.Count) {
                GameObject temp = Instantiate(itemIconPrefab, this.transform);
                temp.SetActive(false);
                itemIcons.Add(temp);
            }
            bool notZero = entry.Value.quantity > 0;
            bool inInventory = Database.database.databaseItem.GetItemById(entry.Key).inInventory;
            if (!(notZero || showZero)) continue;
            if (!(!inInventoryOnly || inInventory)) continue;
            itemIcons[i].GetComponent<ItemUIIcon>().AssignItem(entry.Value);
            i += 1;
        }
        for (; i < itemIcons.Count; i++) {
            itemIcons[i].GetComponent<ItemUIIcon>().AssignItem(null);
        }
    }

    public void AdjustItemCollectionUI(SlotInventory collection)
    {
        for(int i = 0; i < collection.items.Count; i++) {
            //itemIcons[i].GetComponent<ItemUIBattleInventory>().idSlot = i;
            if (itemIcons[i].TryGetComponent<ItemUIBattleInventory>(out ItemUIBattleInventory script)) {
                script.idSlot = i;
            } else if (itemIcons[i].TryGetComponent<ItemUIUsable>(out ItemUIUsable usableScript)) {
                usableScript.idSlot = i;
            }
            if (collection.items[i].quantity != 0) {
                itemIcons[i].GetComponentInChildren<ItemUIIcon>().AssignItem(collection.items[i]);
            } else {
                ItemUIIcon itemUIIcon = itemIcons[i].GetComponentInChildren<ItemUIIcon>();
                itemUIIcon.AssignItem(null);
                itemUIIcon.gameObject.SetActive(true);
            }
            itemIcons[i].SetActive(true);
        }
    }
}
