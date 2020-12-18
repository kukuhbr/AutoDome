using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

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

    public void GarageOpen()
    {
        garage.SetActive(true);
    }

    public void GarageClose()
    {
        garage.SetActive(false);
    }

    public void InventoryOpen()
    {
        inventory.SetActive(true);
        ItemUICollection coll = inventory.GetComponentInChildren<ItemUICollection>();
        coll.AdjustItemCollectionUI(PlayerManager.playerManager.playerData.inventory);
    }

    public void InventoryClose()
    {
        inventory.SetActive(false);
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
