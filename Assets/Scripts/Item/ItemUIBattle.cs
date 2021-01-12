using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemUIBattle : MonoBehaviour
{
    [SerializeField]
    //private PlayerItemHandler handler;
    private ItemUICollection itemUICollection;
    //private Image backgroundImage;
    private SlotInventory battleSlot;

    void Start() {
        itemUICollection = GetComponent<ItemUICollection>();
        //backgroundImage = GetComponent<Image>();
        //backgroundImage.enabled = false;
        BattleEvents.battleEvents.onItemPickup += HandleItemPickup;
        BattleEvents.battleEvents.onItemUsed += AdjustItemUI;
        battleSlot = PlayerManager.playerManager.playerData.battleSlot;
        AdjustItemUI();
    }

    void AdjustItemUI()
    {
        //backgroundImage.enabled = !battleSlot.IsEmpty();
        itemUICollection.AdjustItemCollectionUI(battleSlot);
    }

    void HandleItemPickup(int id)
    {
        ItemBase pickup = Database.database.databaseItem.GetItemById(id);
        if (pickup is ItemUsable) {
            int availableSlot = battleSlot.FindAvailableSlot(id);
            if (availableSlot != -1) {
                battleSlot.SetSlot(id, availableSlot);
            }
            AdjustItemUI();
        }
    }

    void OnDestroy()
    {
        BattleEvents.battleEvents.onItemPickup -= HandleItemPickup;
        BattleEvents.battleEvents.onItemUsed -= AdjustItemUI;
    }
}
