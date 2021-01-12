using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemUIIcon : MonoBehaviour
{
    public InventoryEntry inventoryEntry;
    public string displayFormat;
    public void AssignItem(InventoryEntry entry)
    {
        inventoryEntry = entry;
        if(entry != null) {
            DatabaseItem databaseItem = Database.database.databaseItem;
            GetComponent<Image>().sprite = databaseItem.GetItemById(entry.id).icon;
            GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            GetComponentInChildren<TextMeshProUGUI>().text = string.Format(displayFormat, entry.quantity, entry.maxQuantity);
            gameObject.SetActive(true);
        } else {
            GetComponent<Image>().sprite = null;
            GetComponent<Image>().color = new Color(0, 0, 0, .05f);
            GetComponentInChildren<TextMeshProUGUI>().text = "Empty";
            gameObject.SetActive(false);
        }
    }

    public int GetEntryId()
    {
        if (inventoryEntry == null) return -1;
        return inventoryEntry.id;
    }
}
