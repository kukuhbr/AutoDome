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
        currencyText.text = PlayerManager.playerManager.playerData.inventory.GetEntry(9).quantity.ToString();
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

    public void GarageUpgrade()
    {
        PlayerData player = PlayerManager.playerManager.playerData;
        int id = SceneLoader.sceneLoader.selectedCharacterIndex;
        bool isUpgradeAvailable = player.VehicleUpgradeAvailable(id);
        if(isUpgradeAvailable) {
            bool haveItems = player.VehicleUpgradeHaveItems(id);
            if (haveItems) {
                player.VehicleUpgradeRemoveItems(id);
                player.UpgradeVehicleGrade(id);
                TriggerUpgradeVehicle();
            } else {
                TriggerUpgradeVehicleFail();
            }
        }

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

    public event Action onUpgradeVehicle;
    public void TriggerUpgradeVehicle()
    {
        if(onUpgradeVehicle != null)
        {
            onUpgradeVehicle();
        }
    }

    public event Action onUpgradeVehicleFail;
    public void TriggerUpgradeVehicleFail()
    {
        if(onUpgradeVehicleFail != null)
        {
            onUpgradeVehicleFail();
            Debug.Log("Upgrade vehicle fail!");
        }
    }
}
