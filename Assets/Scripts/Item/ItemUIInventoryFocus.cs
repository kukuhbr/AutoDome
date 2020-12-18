using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemUIInventoryFocus : MonoBehaviour, IPointerClickHandler
{
    void Start()
    {
        MainMenu.mainMenu.onItemFocusChange += UpdateFocus;
    }

    void UpdateFocus(int id)
    {
        if (GetComponent<ItemUIIcon>().inventoryEntry.id == id) {
            GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 1f);
        } else {
            GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
        }
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        MainMenu.mainMenu.TriggerItemFocusChange(GetComponent<ItemUIIcon>().inventoryEntry.id);
    }

    void OnDestroy()
    {
        MainMenu.mainMenu.onItemFocusChange -= UpdateFocus;
    }
}
