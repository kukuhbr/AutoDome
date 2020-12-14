using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemUIUsableScript : MonoBehaviour, IPointerClickHandler
{
    InventoryEntry inventoryEntry;
    private bool isGameOver = false;
    [SerializeField]
    private RectTransform cooldownImage;
    void Start()
    {
        BattleEvents.battleEvents.onGameOver += GameOver;
    }

    void GameOver()
    {
        isGameOver = true;
    }

    void LateUpdate()
    {
        if(inventoryEntry != null) {
            float height = Mathf.Lerp(0f, 100f, inventoryEntry.cooldown / 4f);
            cooldownImage.sizeDelta = new Vector2(100f, height);
        }
    }

    public void AssignItem(InventoryEntry entry)
    {
        inventoryEntry = entry;
        if(entry != null) {
            GetComponent<Image>().sprite = entry.item.icon;
            GetComponentInChildren<TextMeshProUGUI>().text = string.Format("{0}/{1}", entry.quantity, entry.maxQuantity);
            gameObject.SetActive(true);
        } else {
            GetComponent<Image>().sprite = null;
            cooldownImage.sizeDelta = new Vector2(100f, 0f);
            GetComponentInChildren<TextMeshProUGUI>().text = null;
            gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if(inventoryEntry != null) {
            if (!isGameOver && inventoryEntry.cooldown == 0f) {
                inventoryEntry.item.Use();
            }
        }
    }

    void OnDestroy()
    {
        BattleEvents.battleEvents.onGameOver -= GameOver;
    }
}
