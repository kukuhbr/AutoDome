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
    private Image backgroundImage;

    void Start() {
        itemUICollection = GetComponent<ItemUICollection>();
        backgroundImage = GetComponent<Image>();
        backgroundImage.enabled = false;
        BattleEvents.battleEvents.onItemPickup += AdjustItemUI;
        BattleEvents.battleEvents.onItemUsed += AdjustItemUI;
    }

    void AdjustItemUI()
    {
        backgroundImage.enabled = true;
        itemUICollection.AdjustItemCollectionUI(handler.battleInventory);
    }

    void OnDestroy()
    {
        BattleEvents.battleEvents.onItemPickup -= AdjustItemUI;
        BattleEvents.battleEvents.onItemUsed -= AdjustItemUI;
    }
}
