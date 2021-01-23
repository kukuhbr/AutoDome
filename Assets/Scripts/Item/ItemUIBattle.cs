using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemUIBattle : MonoBehaviour
{
    [SerializeField]
    private ItemUICollection itemUICollection;
    private SlotInventory battleSlot;

    void Start() {
        itemUICollection = GetComponent<ItemUICollection>();
        BattleEvents.battleEvents.onItemPickup += HandleItemPickup;
        BattleEvents.battleEvents.onItemUsed += AdjustItemUI;
        battleSlot = PlayerManager.playerManager.playerData.battleSlot;
        AdjustItemUI();
    }

    void AdjustItemUI()
    {
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
