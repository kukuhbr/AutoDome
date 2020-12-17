using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemUICollection : MonoBehaviour
{
    private List<GameObject> itemIcons;
    [SerializeField]
    private GameObject itemIconPrefab;

    void Start() {
        itemIcons = new List<GameObject>();
        for(int i = 0; i < 10; i++) {
            GameObject temp = Instantiate(itemIconPrefab, this.transform);
            temp.SetActive(false);
            itemIcons.Add(temp);
        }
    }

    public void AdjustItemCollectionUI(Inventory collection)
    {
        int i = 0;
        foreach(KeyValuePair<int, InventoryEntry> entry in collection.items) {
            if(entry.Value.quantity > 0) {
                if(i >= itemIcons.Count) {
                    GameObject temp = Instantiate(itemIconPrefab, this.transform);
                    temp.SetActive(false);
                    itemIcons.Add(temp);
                }
                itemIcons[i].GetComponent<ItemUIIcon>().AssignItem(entry.Value);
                i += 1;
            }
        }
        for (; i < itemIcons.Count; i++) {
            itemIcons[i].GetComponent<ItemUIIcon>().AssignItem(null);
        }
    }
}
