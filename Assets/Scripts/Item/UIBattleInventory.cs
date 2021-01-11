using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIBattleInventory : MonoBehaviour
{
    void OnEnable()
    {
        ItemUICollection coll = GetComponent<ItemUICollection>();
        coll.AdjustItemCollectionUI(PlayerManager.playerManager.playerData.battleInventory);
    }

    public event Action onBattleInventorySlotSelected;
    public void TriggerBattleInventorySlotSelected()
    {
        if(onBattleInventorySlotSelected != null)
        {
            onBattleInventorySlotSelected();
        }
    }
}
