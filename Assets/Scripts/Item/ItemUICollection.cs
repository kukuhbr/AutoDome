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

    void Awake() {
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
            if(i >= itemIcons.Count) {
                GameObject temp = Instantiate(itemIconPrefab, this.transform);
                temp.SetActive(false);
                itemIcons.Add(temp);
            }
            bool notZero = entry.Value.quantity > 0;
            if (notZero || showZero) {
                itemIcons[i].GetComponent<ItemUIIcon>().AssignItem(entry.Value);
                i += 1;
            }
        }
        for (; i < itemIcons.Count; i++) {
            itemIcons[i].GetComponent<ItemUIIcon>().AssignItem(null);
        }
    }
}
