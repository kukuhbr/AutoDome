using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemUIBattle : MonoBehaviour
{
    [SerializeField]
    private PlayerItemHandler handler;
    private ItemUICollection itemUICollection;

    void Start() {
        itemUICollection = GetComponent<ItemUICollection>();
        BattleEvents.battleEvents.onItemPickup += AdjustItemUI;
        BattleEvents.battleEvents.onItemUsed += AdjustItemUI;
    }

    void AdjustItemUI()
    {
        itemUICollection.AdjustItemCollectionUI(handler.battleInventory);
    }

    void OnDestroy()
    {
        BattleEvents.battleEvents.onItemPickup -= AdjustItemUI;
        BattleEvents.battleEvents.onItemUsed -= AdjustItemUI;
    }
}
