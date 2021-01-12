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
    int idFocused;
    public int idSlot;
    [SerializeField]
    private GameObject highlight;
    [SerializeField]
    private GameObject text;
    private UIBattleInventory battleInventoryHandler;
    void OnEnable()
    {
        highlight.SetActive(false);
        battleInventoryHandler = GetComponentInParent<UIBattleInventory>();
        MainMenu.mainMenu.onItemFocusChange += UpdateFocus;
        battleInventoryHandler.onBattleInventorySlotSelected += EndSelectPhase;
    }

    void UpdateFocus(int id)
    {
        ItemBase item = Database.database.databaseItem.GetItemById(id);
        isUsableItemFocused = item is ItemUsable;
        idFocused = id;
        if (isUsableItemFocused) {
            StartSelectPhase();
        } else {
            EndSelectPhase();
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
        SlotInventory battleSlot = PlayerManager.playerManager.playerData.battleSlot;
        if (!isSelectPhase) {
            battleSlot.UnsetSlot(idSlot);
            battleInventoryHandler.TriggerBattleInventorySlotSelected();
            return;
        }
        ItemBase item = Database.database.databaseItem.GetItemById(idFocused);
        bool success = battleSlot.SetSlot(idFocused, idSlot);
        if (!success) {
            Notifier.NotifyInstant("Not Enough Item");
        }
        battleInventoryHandler.TriggerBattleInventorySlotSelected();
    }

    void OnDisable()
    {
        MainMenu.mainMenu.onItemFocusChange -= UpdateFocus;
        battleInventoryHandler.onBattleInventorySlotSelected -= EndSelectPhase;
    }
}
