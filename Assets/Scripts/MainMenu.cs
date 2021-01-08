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
        SoundsManager.soundsManager.PlayLoop(SoundsManager.SoundsEnum.music_menu, "menu");
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
                player.IncreaseEnergy(true);
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
        int selectedIndex = SceneLoader.sceneLoader.selectedCharacterIndex;
        bool characterObtained = PlayerManager.playerManager.playerData.GetVehicleGrade(selectedIndex) != -1;
        if(!characterObtained) {
            Notifier.NotifyInstant("You don't have this vehicle");
            SoundsManager.soundsManager.PlaySFX(SoundsManager.SoundsEnum.ui_back);
            return;
        }
        if (true) {//player.DecreaseEnergy()) {
            SoundsManager.soundsManager.PlaySFX(SoundsManager.SoundsEnum.ui_start);
            PlayerManager.playerManager.playerData.SetupBattleInventory();
            SceneLoader.sceneLoader.LoadScene(SceneIndex.BATTLE_SOLO);
            SoundsManager.soundsManager.StopLoop("menu");
            SoundsManager.soundsManager.PlayLoop(SoundsManager.SoundsEnum.music_battle, "music_battle");
        } else {
            //Notifier.NotifyInstant("You don't have enough energy");
            SoundsManager.soundsManager.PlaySFX(SoundsManager.SoundsEnum.ui_back);
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
        player.IncreaseEnergy(true);
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
        SoundsManager.soundsManager.PlaySFX(SoundsManager.SoundsEnum.ui_select);
    }

    public void GarageClose()
    {
        isMenuInFocus = false;
        garage.SetActive(false);
        UnlockScroll();
        SoundsManager.soundsManager.PlaySFX(SoundsManager.SoundsEnum.ui_back);
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
                PlayerManager.SavePlayerData(player);
                SoundsManager.soundsManager.PlaySFX(SoundsManager.SoundsEnum.ui_select);
            } else {
                Notifier.NotifyInstant("Not Enough Materials");
                SoundsManager.soundsManager.PlaySFX(SoundsManager.SoundsEnum.ui_back);
            }
        } else {
            //Notifier.NotifyInstant("Vehicle Max Upgrade");
            Notifier.NotifyInstant("Cannot Upgrade Further");
            SoundsManager.soundsManager.PlaySFX(SoundsManager.SoundsEnum.ui_back);
        }

    }

    public void InventoryOpen()
    {
        if(isMenuInFocus) return;
        isMenuInFocus = true;
        inventory.SetActive(true);
        ItemUICollection coll = inventory.GetComponentInChildren<ItemUICollection>();
        coll.AdjustItemCollectionUI(player.inventory);
        LockScroll();
        SoundsManager.soundsManager.PlaySFX(SoundsManager.SoundsEnum.ui_select);
    }

    public void InventoryClose()
    {
        isMenuInFocus = false;
        inventory.SetActive(false);
        UnlockScroll();
        SoundsManager.soundsManager.PlaySFX(SoundsManager.SoundsEnum.ui_back);
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
