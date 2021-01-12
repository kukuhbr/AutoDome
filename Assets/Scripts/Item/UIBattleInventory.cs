using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIBattleInventory : MonoBehaviour
{
    void Start()
    {
        UpdateView();
    }

    void UpdateView()
    {
        ItemUICollection coll = GetComponent<ItemUICollection>();
        coll.AdjustItemCollectionUI(PlayerManager.playerManager.playerData.battleSlot);
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
