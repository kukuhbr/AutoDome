using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIBattleInventory : MonoBehaviour
{
    SlotInventory battleSlot;
    void Awake()
    {
        battleSlot = PlayerManager.playerManager.playerData.battleSlot;
        battleSlot.Recalculate();
    }
    void Start()
    {
        UpdateView();
    }

    void UpdateView()
    {
        ItemUICollection coll = GetComponent<ItemUICollection>();
        coll.AdjustItemCollectionUI(battleSlot);
    }

    public event Action onBattleInventorySlotSelected;
    public void TriggerBattleInventorySlotSelected()
    {
        UpdateView();
        if(onBattleInventorySlotSelected != null)
        {
            onBattleInventorySlotSelected();
        }
    }
}
