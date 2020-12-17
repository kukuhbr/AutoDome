using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemUIUsableScript : MonoBehaviour, IPointerClickHandler
{
    private bool isGameOver = false;
    [SerializeField]
    private RectTransform cooldownImage;
    void Start()
    {
        BattleEvents.battleEvents.onGameOver += GameOver;
        cooldownImage.sizeDelta = new Vector2(100f, 0f);
    }

    void GameOver()
    {
        isGameOver = true;
    }

    void LateUpdate()
    {
        InventoryEntry inventoryEntry = GetComponent<ItemUIIcon>().inventoryEntry;
        if(inventoryEntry != null) {
            float height = Mathf.Lerp(0f, 100f, inventoryEntry.cooldown / 4f);
            cooldownImage.sizeDelta = new Vector2(100f, height);
        } else {
            cooldownImage.sizeDelta = new Vector2(100f, 0f);
        }
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        InventoryEntry inventoryEntry = GetComponent<ItemUIIcon>().inventoryEntry;
        if(inventoryEntry != null) {
            if (!isGameOver && inventoryEntry.cooldown == 0f) {
                DatabaseItem databaseItem = Resources.Load<DatabaseItem>("DatabaseItem");
                databaseItem.GetItemById(inventoryEntry.id).Use();
                //inventoryEntry.item.Use();
            }
        }
    }

    void OnDestroy()
    {
        BattleEvents.battleEvents.onGameOver -= GameOver;
    }
}
