using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemUIUsableScript : MonoBehaviour, IPointerClickHandler
{
    InventoryEntry inventoryEntry;
    bool isCooldown = false;
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
    public void AssignItem(InventoryEntry entry)
    {
        inventoryEntry = entry;
        GetComponent<Image>().sprite = entry.item.icon;
        GetComponentInChildren<TextMeshProUGUI>().text = string.Format("{0}/{1}", entry.quantity, entry.maxQuantity);
        gameObject.SetActive(true);
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (!isCooldown && !isGameOver) {
            isCooldown = true;
            StartCoroutine(Cooldown(4f));
        }
    }

    IEnumerator Cooldown(float seconds) {
        float timeLeft = seconds;
        //Image cooldownImage = GetComponentInChildren<Image>();
        //RectTransform cooldownRect = cooldownImage.gameObject.GetComponent<RectTransform>();
        while(timeLeft > 0) {
            timeLeft -= Time.deltaTime;
            float height = Mathf.Lerp(0f, 100f, timeLeft / seconds);
            Debug.Log(height);
            cooldownImage.sizeDelta = new Vector2(100f, height);
            Debug.Log(cooldownImage.name + " " + cooldownImage.sizeDelta);
            yield return null;
        }
        Debug.Log("Cooldown is done!");
        isCooldown = false;
    }

    void OnDestroy()
    {
        BattleEvents.battleEvents.onGameOver -= GameOver;
    }
}
