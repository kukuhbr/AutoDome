using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public class MainMenu : MonoBehaviour
{
    public static MainMenu mainMenu;
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI energyFillText;
    public TextMeshProUGUI currencyText;
    public GameObject garage;
    public GameObject inventory;
    private PlayerData player;
    private bool isMenuInFocus;
    private void Awake()
    {
        mainMenu = this;
        player = PlayerManager.playerManager.playerData;
        isMenuInFocus = false;
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
        if(isMenuInFocus) return;
        //bool energyEnough = PlayerManager.playerManager.playerData.inventory.Remove(10, 1);
        if (player.DecreaseEnergy()) {
            SceneLoader.sceneLoader.LoadScene(SceneIndex.BATTLE_SOLO);
        } else {
            //Notifier.NotifyInstant("You don't have enough energy");
            Notifier.Notify("You don't have enough energy", "Watch Ad", WatchAd);
        }
    }

    void WatchAd()
    {
        string energyAdId = GoogleMobileAdsScript.adUnitTest;
        RewardedAd energyRewardedAd = GoogleMobileAdsScript.instance.CreateAndLoadRewardedAd(energyAdId);
        if (energyRewardedAd.IsLoaded()) {
            energyRewardedAd.Show();
        }
        energyRewardedAd.OnUserEarnedReward += EnergyAdReward;
    }

    void EnergyAdReward(object sender, EventArgs args)
    {
        player.IncreaseEnergy();
        Notifier.Notify("Thank you for supporting!");
    }

    void LockScroll() {
        GetComponentInChildren<ScrollRect>().horizontal = false;
    }

    void UnlockScroll() {
        GetComponentInChildren<ScrollRect>().horizontal = true;
    }

    public void GarageOpen()
    {
        if(isMenuInFocus) return;
        isMenuInFocus = true;
        garage.SetActive(true);
        LockScroll();
    }

    public void GarageClose()
    {
        isMenuInFocus = false;
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
        if(isMenuInFocus) return;
        isMenuInFocus = true;
        inventory.SetActive(true);
        ItemUICollection coll = inventory.GetComponentInChildren<ItemUICollection>();
        coll.AdjustItemCollectionUI(player.DisplayInventory());
        LockScroll();
    }

    public void InventoryClose()
    {
        isMenuInFocus = false;
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
