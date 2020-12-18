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
            DatabaseItem databaseItem = Resources.Load<DatabaseItem>("DatabaseItem");
            GetComponent<Image>().sprite = databaseItem.GetItemById(entry.id).icon;
            GetComponentInChildren<TextMeshProUGUI>().text = string.Format(displayFormat, entry.quantity, entry.maxQuantity);
            gameObject.SetActive(true);
        } else {
            GetComponent<Image>().sprite = null;
            GetComponentInChildren<TextMeshProUGUI>().text = null;
            gameObject.SetActive(false);
        }
    }
}
