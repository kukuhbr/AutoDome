using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static MainMenu mainMenu;
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI energyFillText;
    public TextMeshProUGUI currencyText;
    public GameObject garage;
    public GameObject inventory;
    private PlayerData player;
    private void Awake()
    {
        mainMenu = this;
        player = PlayerManager.playerManager.playerData;
    }
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        currencyText.text = player.inventory.GetEntry(9).quantity.ToString();
        energyText.text = player.inventory.GetEntry(10).quantity.ToString();
        energyFillText.text = RefillEnergyLive();
    }

    public string RefillEnergyLive() {
        string timeToFill = "";
        if (!player.isEnergyMax()) {
            TimeSpan diff = DateTime.Now - player.lastEnergyFill;
            TimeSpan fillTime = PlayerManager.energyFillTime;
            bool timeCheck = TimeSpan.Compare(diff, fillTime) != -1;
            if(timeCheck) {
                player.IncreaseEnergy();
                player.lastEnergyFill = DateTime.Now;
            }
            diff = DateTime.Now - player.lastEnergyFill;
            timeToFill = "Energy\n" + (fillTime - diff).ToString(@"mm\:ss");
        } else {
            player.lastEnergyFill = DateTime.Now;
            timeToFill = "Energy Full";
        }
        return timeToFill;
    }

    public void Deploy()
    {
        //bool energyEnough = PlayerManager.playerManager.playerData.inventory.Remove(10, 1);
        if (player.DecreaseEnergy()) {
            SceneLoader.sceneLoader.LoadScene(SceneIndex.BATTLE_SOLO);
        } else {
            //Notifier.NotifyInstant("You don't have enough energy");
            Notifier.Notify("You don't have enough energy", "Watch Ad");
        }
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
        int id = SceneLoader.sceneLoader.selectedCharacterIndex;
        bool isUpgradeAvailable = player.VehicleUpgradeAvailable(id);
        if(isUpgradeAvailable) {
            bool haveItems = player.VehicleUpgradeHaveItems(id);
            if (haveItems) {
                player.VehicleUpgradeRemoveItems(id);
                player.UpgradeVehicleGrade(id);
                TriggerUpgradeVehicle();
            } else {
                Notifier.NotifyInstant("Not Enough Materials");
            }
        } else {
            //Notifier.NotifyInstant("Vehicle Max Upgrade");
            Notifier.NotifyInstant("Cannot Upgrade Further");
        }

    }

    public void InventoryOpen()
    {
        inventory.SetActive(true);
        ItemUICollection coll = inventory.GetComponentInChildren<ItemUICollection>();
        coll.AdjustItemCollectionUI(player.DisplayInventory());
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

    // public event Action onUpgradeVehicleFail;
    // public void TriggerUpgradeVehicleFail()
    // {
    //     if(onUpgradeVehicleFail != null)
    //     {
    //         onUpgradeVehicleFail();
    //         Debug.Log("Upgrade vehicle fail!");
    //     }
    // }
}
