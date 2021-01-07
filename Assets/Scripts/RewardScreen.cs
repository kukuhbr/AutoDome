using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Linq;
using TMPro;
using GoogleMobileAds.Api;

public class RewardScreen : MonoBehaviour
{
    public TextMeshProUGUI rewardParts;
    public PlayerItemHandler playerItemHandler;
    private ItemUICollection itemCollectionHandler;
    [SerializeField]
    private Button adButton;

    void Awake()
    {
        itemCollectionHandler = GetComponentInChildren<ItemUICollection>(true);
        Debug.Log(itemCollectionHandler);
    }

    public void Display()
    {
        rewardParts.text = playerItemHandler.battleInventory.GetEntry(9).quantity.ToString();
        // Set inventories
        itemCollectionHandler.AdjustItemCollectionUI(playerItemHandler.battleInventory);
    }

    public void AddBonusParts(int parts)
    {
        playerItemHandler.battleInventory.Add(9, parts);
    }

    public void LoadMenu()
    {
        Tuple<List<int>, List<int>> rewards = playerItemHandler.battleInventory.makeTuple();
        PlayerManager.playerManager.playerData.inventory.Add(rewards.Item1, rewards.Item2);
        playerItemHandler.battleInventory.Clear();
        PlayerManager.SavePlayerData(PlayerManager.playerManager.playerData);
        SceneLoader.sceneLoader.LoadScene(SceneIndex.MAIN_MENU);
        SoundsManager.soundsManager.PlaySFX(SoundsManager.SoundsEnum.ui_select);
    }

    public void WatchAd()
    {
        SoundsManager.soundsManager.PlaySFX(SoundsManager.SoundsEnum.ui_select);
        string battleAdId = GoogleMobileAdsScript.adUnitTest;
        RewardedAd battleRewardedAd = GoogleMobileAdsScript.instance.CreateAndLoadRewardedAd(battleAdId);
        if (battleRewardedAd.IsLoaded()) {
            battleRewardedAd.Show();
        }
        battleRewardedAd.OnUserEarnedReward += AdReward;
        //AdReward();
    }

    void AdReward(object sender, EventArgs args)
    {
        int[] items = playerItemHandler.battleInventory.items.Keys.ToArray();
        int repetition = UnityEngine.Random.Range(1, 4);
        int[] choice = new int[repetition];
        int[] quantity = new int[repetition];
        string notificationText = "You have gained bonus ";
        for(int i = 0; i < repetition; i++) {
            bool inventoryItem = false;
            while(!inventoryItem) {
                choice[i] = items[UnityEngine.Random.Range(0, items.Length)];
                ItemBase itemData = Database.database.databaseItem.GetItemById(choice[i]);
                inventoryItem = itemData.inInventory;
                if(!inventoryItem) continue;
                if(i < repetition - 1) {
                    notificationText += itemData.itemName + ", ";
                } else {
                    notificationText += itemData.itemName + ".";
                }
            }
            quantity[i] = playerItemHandler.battleInventory.GetEntry(choice[i]).quantity;
        }
        playerItemHandler.battleInventory.ForceAdd(new List<int>(choice), new List<int>(quantity));
        Display();
        Destroy(adButton.gameObject);
        Notifier.NotifyBig(notificationText, 2);
    }
}