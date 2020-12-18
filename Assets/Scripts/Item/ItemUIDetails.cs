using System;
using UnityEngine;
using TMPro;

public class ItemUIDetails : MonoBehaviour
{
    void Start() {
        MainMenu.mainMenu.onItemFocusChange += UpdateDetails;
    }

    void UpdateDetails(int id) {
        DatabaseItem databaseItem = Resources.Load<DatabaseItem>("DatabaseItem");
        ItemBase item = databaseItem.GetItemById(id);
        TextMeshProUGUI[] textDetails = GetComponentsInChildren<TextMeshProUGUI>();
        if(textDetails.Length == 2) {
            textDetails[0].text = item.itemName;
            textDetails[1].text = item.description;
        }
    }

    void OnDestroy() {
        MainMenu.mainMenu.onItemFocusChange -= UpdateDetails;
    }
}
