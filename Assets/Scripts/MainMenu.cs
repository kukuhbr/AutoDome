using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static MainMenu mainMenu;
    public TextMeshProUGUI currencyText;
    public GameObject garage;
    public GameObject inventory;
    private void Awake()
    {
        mainMenu = this;
    }
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        currencyText.text = PlayerManager.playerManager.GetCurrency("bolt").ToString();
    }

    // Update is called once per frame
    void Update()
    {
        // if (PlayerPlaceholder)
        // {
        //     PlayerPlaceholder.transform.Rotate(0, 20*Time.deltaTime, 0, Space.World);
        // }
    }

    public void Deploy()
    {
        SceneLoader.sceneLoader.LoadScene(SceneIndex.BATTLE_SOLO);
    }

    void LockScroll() {
        GetComponentInChildren<ScrollRect>().horizontal = false;
    }

    void UnlockScroll() {
        GetComponentInChildren<ScrollRect>().horizontal = true;
    }

    public void GarageOpen()
    {
        garage.SetActive(true);
        LockScroll();
    }

    public void GarageClose()
    {
        garage.SetActive(false);
        UnlockScroll();
    }

    public void InventoryOpen()
    {
        inventory.SetActive(true);
        ItemUICollection coll = inventory.GetComponentInChildren<ItemUICollection>();
        coll.AdjustItemCollectionUI(PlayerManager.playerManager.playerData.inventory);
        LockScroll();
    }

    public void InventoryClose()
    {
        inventory.SetActive(false);
        UnlockScroll();
    }

    public event Action<int> onItemFocusChange;
    public void TriggerItemFocusChange(int id)
    {
        if(onItemFocusChange != null)
        {
            onItemFocusChange(id);
        }
    }
}
