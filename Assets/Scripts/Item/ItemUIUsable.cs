using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemUIUsable : MonoBehaviour, IPointerClickHandler
{
    private bool isGameOver = false;
    [SerializeField]
    private RectTransform cooldownImage;
    public int idSlot;
    void Start()
    {
        BattleEvents.battleEvents.onGameOver += GameOver;
        cooldownImage.sizeDelta = new Vector2(0f, 0f);
    }

    void GameOver()
    {
        isGameOver = true;
    }

    void LateUpdate()
    {
        InventoryEntry inventoryEntry = GetComponent<ItemUIIcon>().inventoryEntry;
        if(inventoryEntry != null) {
            float cooldown = GetItemCooldown(inventoryEntry.id);
            float height = Mathf.Lerp(0f, 100f, cooldown / 4f);
            cooldownImage.sizeDelta = new Vector2(0f, height);
        } else {
            cooldownImage.sizeDelta = new Vector2(0f, 0f);
        }
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        InventoryEntry inventoryEntry = GetComponent<ItemUIIcon>().inventoryEntry;
        if(inventoryEntry != null) {
            if (!isGameOver && GetItemCooldown(inventoryEntry.id) == 0f) {
                DatabaseItem databaseItem = Database.database.databaseItem;
                PlayerManager.playerManager.playerData.battleSlot.UseSlot(idSlot);
                databaseItem.GetItemById(inventoryEntry.id).Use();
            }
        }
    }

    private float GetItemCooldown(int id)
    {
        return PlayerManager.playerManager.playerData.battleInventory.GetEntry(id).cooldown;
    }

    void OnDestroy()
    {
        BattleEvents.battleEvents.onGameOver -= GameOver;
    }
}
