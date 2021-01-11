using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemUIBattleInventory : MonoBehaviour, IPointerClickHandler
{
    bool isUsableItemFocused;
    bool isSelectPhase;
    int focusedId;
    [SerializeField]
    private GameObject highlight;
    [SerializeField]
    private GameObject text;
    private UIBattleInventory battleInventoryHandler;
    void OnEnable()
    {
        MainMenu.mainMenu.onItemFocusChange += UpdateFocus;
        highlight.SetActive(false);
        //text.SetActive(false);
        battleInventoryHandler = GetComponentInParent<UIBattleInventory>();
        battleInventoryHandler.onBattleInventorySlotSelected += EndSelectPhase;
    }

    void UpdateFocus(int id)
    {
        ItemBase item = Database.database.databaseItem.GetItemById(id);
        isUsableItemFocused = item is ItemUsable;
        focusedId = id;
        if (isUsableItemFocused) {
            StartSelectPhase();
        }
    }

    void StartSelectPhase()
    {
        highlight.SetActive(true);
        isSelectPhase = true;
    }

    void EndSelectPhase()
    {
        highlight.SetActive(false);
        isSelectPhase = false;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (!isSelectPhase) return;
        battleInventoryHandler.TriggerBattleInventorySlotSelected();
        ItemBase item = Database.database.databaseItem.GetItemById(focusedId);
        int entryId = GetComponentInChildren<ItemUIIcon>().GetEntryId();
        if (entryId != -1) {
            // Something is here
            Debug.Log("Something");
        } else {
            // Nothing is here
            Debug.Log("Empty");
            text.GetComponent<TextMeshProUGUI>().text = "Empty";
        }
        text.GetComponent<TextMeshProUGUI>().text = item.itemName;
    }

    void OnDisable()
    {
        MainMenu.mainMenu.onItemFocusChange -= UpdateFocus;
        battleInventoryHandler.onBattleInventorySlotSelected -= EndSelectPhase;
    }
}
